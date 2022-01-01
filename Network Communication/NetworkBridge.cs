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
    // Singleton.
    public class NetworkBridge
    {
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
            // edit: There's no reason we'd need to know that it's the first message. Both parties send an object with
            // all the contact's data right when they connect, so it'll be immediately updated (and will be no matter
            // if it's the first message or not).
            // Leaving the TODO here in case I missed something and it actually IS needed.
            
            // TODO somehow solve idle status
        }

        public void Run()
        {
            if (Running)
                return;

            Running = true;
            ThreadUtils.RunThread(() => NetworkManager.Run(HandleServerRunning));

            void HandleServerRunning(bool running)
            {
                if (!running)
                {
                    Debug.WriteLine($"{nameof(NetworkBridge)}: Server did not start - NOT trying to connect to contacts.");
                    return;
                }
                
                // TODO: Maybe wait for the server to start up?
                Debug.WriteLine($"{nameof(NetworkBridge)}: Recursively trying to connect to all contacts.");
                AllContacts.ForEach(contact => ThreadUtils.RunThread(() =>
                {
                    bool connected = NetworkManager.CreateConnection(IPAddress.Parse(contact.Ip));
                    if (!connected)
                        Debug.WriteLine($"{nameof(NetworkBridge)}: Could not connect to {contact.Ip}");
                }));
                
                SubscribeToUserEvents();
            }
        }

        // Send all user data (the other end will send theirs).
        private static void HandleNewConnection(IPAddress ip)
        {
            RemoteContactData contactData = new(Contact.UserContact);
            NetworkManager.Send(ip, new RemoteObject(contactData).SerializeToUtf8Bytes());
        }

        public void HandleIncomingData(RemoteObject obj, string ip) // TODO: Change all "string ip" to IPAddresses
        {
            Debug.WriteLine($"Received data from {ip}");

            Contact? contact = RetrieveOrCreateContact(obj, ip);

            if (contact is null)
                return;
            
            contact.Id ??= obj.ContactId;
            
            // TODO: Handle IP collision
            if (contact.Ip != ip)
                contact.Ip = ip;

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
        private Contact? RetrieveOrCreateContact(RemoteObject obj, string ip)
        {
            // If AllContacts contains a contact with the sender's ID, return that contact.
            
            Contact contact = AllContacts.FirstOrDefault(c => c.Id == obj.ContactId);

            if (contact is not null)
                return contact;

            // else if AllContacts contains a contact with the sender's IP AND the contact's ID
            // is null (contact added by the user via IP but not initialized yet), return that contact.
            // The caller method will assign the ID.

            contact = AllContacts.FirstOrDefault(c => c.Ip == ip && c.Id is null);

            if (contact is not null)
                return contact;

            // After this point we know that the contact doesn't exist in our list yet.

            // If the sender has sent all needed data to create it (ContactData object), create it and return it.
            // The parent method will assign the data to the contact.
            // Else return null.
            
            if (obj.Data.ObjectType != RemoteObject.ObjectType.ContactData)
                return null;
            
            contact = new Contact(ip);
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
            AllContacts.NotOffline().ForEach(contact => NetworkManager.Send(IPAddress.Parse(contact.Ip), obj.SerializeToUtf8Bytes()));
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
            NetworkManager.Send(IPAddress.Parse(ip), obj.SerializeToUtf8Bytes());
        }

        /// <summary> Updates {contact}'s status to match that in the {statusUpdate} object.</summary>
        private static void UpdateContactStatus(RemoteStatusUpdate statusUpdate, Contact contact)
        {
            // The check is there because when the properties get updated, they are sent to all connected peers,
            // so it would be wasteful to send it even when it actually didn't change.
            if (contact.CurrentStatus != statusUpdate.Status)
                contact.CurrentStatus = statusUpdate.Status;
        }

        /// <summary> Updates {contact}'s data to match those in the {data} object.</summary>
        private static void UpdateContactData(RemoteContactData data, Contact contact)
        {
            if (data.Name is not null && contact.Name != data.Name) 
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