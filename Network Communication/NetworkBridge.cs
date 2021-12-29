using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using ClausaComm.Contacts;
using ClausaComm.Exceptions;
using ClausaComm.Extensions;
using ClausaComm.Messages;
using ClausaComm.Network_Communication.Networking;
using ClausaComm.Network_Communication.Objects;
using ClausaComm.Utils;

namespace ClausaComm.Network_Communication
{
    public class NetworkBridge
    {
        // We don't need to confirm the data now that it's all connection based.
        //private readonly HashSet<RemoteObject> UncomfirmedData = new();
        //private readonly ContactStatusWatcher StatusWatcher;
        //private readonly PingSender PingSender;
        public bool Running { get; private set; }
        private readonly HashSet<Contact> AllContacts;
        private readonly Action<Contact> AddContactMethod;
        private static bool InstanceCreated;
        
        /// <param name="allContacts">A collection with all the existing contacts</param>
        /// <param name="addContactMethod">A method for adding new contacts that might be sent from the network.</param>
        public NetworkBridge(HashSet<Contact> allContacts, Action<Contact> addContactMethod)
        {
            if (InstanceCreated)
                throw new MultipleInstancesException(nameof(NetworkBridge));

            AllContacts = allContacts;
            AddContactMethod = addContactMethod;
            
            NetworkManager.OnReceive += (message, endpoint) => HandleIncomingData(message, endpoint.Address.ToString());
            NetworkManager.OnConnect += endpoint => HandleNewConnection(endpoint.Address);
            NetworkManager.OnDisconnect += endpoint => AllContacts
                .First(contact => contact.Ip == endpoint.Address.ToString())
                .CurrentStatus = Contact.Status.Offline;
            
            InstanceCreated = true;
            
            // TODO for tomorrow: wrap allContactData remoteObject type in a GreetingMessage or something - we need
            // TODO to identify first messages and react to them (add contact if doesn't exist, update the contact's data...)
            
            // TODO somehow solve idle status
        }

        public void Run()
        {
            if (Running)
                return;

            Running = true;
            new Thread(NetworkManager.Run).Start();
            AllContacts.ForEach(contact => new Thread(() => NetworkManager.CreateConnection(IPAddress.Parse(contact.Ip))).Start());

            SubscribeToUserEvents();
        }

        // Send all user data (and the other end will send theirs).
        private static void HandleNewConnection(IPAddress ip)
        {
            RemoteContactData contactData = new(Contact.UserContact);
            NetworkManager.Send(ip, new RemoteObject(contactData).SerializeToUtf8Bytes());
        }

        public void HandleIncomingData(RemoteObject obj, string ip) // TODO: Change all "string ip" to IPAddress -es
        {
            Debug.WriteLine($"Received data from {ip}");

            Contact contact = RetrieveContact(obj, ip);
            if (contact?.Id is null)
            {
                // If the ID is not assigned to the contact, take it from the remoteObject and assign it.
                if (contact is not null)
                    contact.Id = obj.ContactId;

                Client.Send(ip, new RemoteObject(new FullContactDataRequest()));
                return;
            }

            // If the contact's ID matches but the IP is different, update it.
            if (contact.Ip != ip)
                contact.Ip = ip;

            StatusWatcher.HandleActivityReceived(contact);

            switch (obj.Data.ObjectType)
            {
                case RemoteObject.ObjectType.ContactData:
                    UpdateContactData((RemoteContactData)obj.Data, contact);
                    break;

                case RemoteObject.ObjectType.StatusUpdate:
                    UpdateContactStatus((RemoteStatusUpdate)obj.Data, contact);
                    break;

                case RemoteObject.ObjectType.Message:
                    HandleMessageReceived((Message)obj.Data, contact);
                    break;

                case RemoteObject.ObjectType.Ping:
                    // Ignored (because the ping is already registered/recognized before this code).
                    break;

                case RemoteObject.ObjectType.FullContactDataRequest:
                    SendFullContactData(ip);
                    break;

                default:
                    throw new NotImplementedException($"One of {nameof(RemoteObject)}'s ObjectTypes weren't implemented in {nameof(NetworkBridge)}'s {nameof(HandleIncomingData)} method.");
            }
        }

        public void SendMessage()
        {
        }

        /// <summary>
        /// Identifies the contact in the user's contact list based on ID and returns it.<br/>
        /// Else identifies it based on IP (but it must not have an ID) and returns it.<br/>
        /// Else if the object received is an object with contact data, creates it and returns it.<br/>
        /// Else returns null.
        /// </summary>
        private Contact? RetrieveContact(RemoteObject obj, string ip)
        {
            // If AllContacts contains a contact with the sender's ID, return that contact.

            Contact contact = AllContacts.FirstOrDefault(c => c.Id == obj.ContactId);

            if (contact is not null)
                return contact;

            // else if AllContacts contains a contact with the sender's IP AND the contact's ID
            // is null (contact added by the user via IP but not initialized yet), return that contact.
            // The parent method will assign an ID and request other data.

            contact = AllContacts.FirstOrDefault(c => c.Ip == ip && c.Id is null);

            if (contact is not null)
                return contact;

            // After this point we know that the contact doesn't exist in our list yet.

            // If the sender has sent all needed data to create it (ContactData object), create it and return it.
            // The parent method will assign the data to the contact.
            // Else return null (and the parent method will send a request to the sender to send back the data).
            if (obj.Data.ObjectType != RemoteObject.ObjectType.ContactData)
                return null;
            
            contact = new(ip);
            AddContactMethod.Invoke(contact);
            return contact;

        }

        /// <summary> Subscribes to user's data changes and sends them to all contacts as soon as they occur.</summary>
        private void SubscribeToUserEvents()
        {
            Contact.UserContact.StatusChange += (_, status) =>
            {
                RemoteStatusUpdate statusData = new(status);
                ProcessAndSendToAll(statusData);
            };

            Contact.UserContact.NameChange += (_, name) =>
            {
                RemoteContactData contactData = new(name: name);
                ProcessAndSendToAll(contactData);
            };

            Contact.UserContact.ProfilePicChange += (_, profilePic) =>
            {
                RemoteContactData profilePicData = new(profilePic: profilePic);
                ProcessAndSendToAll(profilePicData);
            };

            void ProcessAndSendToAll(ISendable data)
            {
                RemoteObject dataObj = new(data);
                SendToAll(dataObj);
            }
        }

        /// <summary> Sends the object to all not-offline contacts.</summary>
        private void SendToAll(RemoteObject obj)
        {
            AllContacts.NotOffline().ForEach(contact => Client.Send(contact.Ip, obj));
        }

        private void HandleMessageReceived(Message message, Contact sender)
        {
            // TODO: Handle the message, convert message.MessageFile to RemoteMessageFile etc.
        }

        /// <summary> Sends an object with all user's data to {ip}.</summary>
        private void SendFullContactData(string ip)
        {
            RemoteContactData data = new(Contact.UserContact);
            RemoteObject obj = new(data);
            Client.Send(ip, obj);
        }

        /// <summary> Updates {contact}'s status to match that in the {statusUpdate} object.</summary>
        private static void UpdateContactStatus(RemoteStatusUpdate statusUpdate, Contact contact)
        {
            if (contact.CurrentStatus != statusUpdate.Status)
                contact.CurrentStatus = statusUpdate.Status;
        }

        /// <summary> Updates {contact}'s data to match those in the {data} object.</summary>
        private static void UpdateContactData(RemoteContactData data, Contact contact)
        {
            if (contact.Name != data.Name)
                contact.Name = data.Name;

            if (data.Base64ProfilePic is not null)
            {
                Image img = ImageUtils.ImageFromBase64String(data.Base64ProfilePic);
                if (contact.ProfilePic != img)
                    contact.ProfilePic = img;
            }
        }
    }
}