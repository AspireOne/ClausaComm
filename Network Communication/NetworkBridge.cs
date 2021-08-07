using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        private readonly HashSet<RemoteObject> UncomfirmedData = new();
        private readonly Server Server;
        private readonly Client Client;
        private readonly ContactStatusWatcher StatusWatcher;
        private readonly PingSender PingSender;
        public bool Running { get; private set; }
        private readonly HashSet<Contact> AllContacts;
        private readonly Action<Contact> AddContactMethod;
        private static bool Created;

        public NetworkBridge(HashSet<Contact> allContacts, Action<Contact> addContactMethod)
        {
            if (Created)
                throw new MultipleInstancesException(nameof(NetworkBridge));

            AllContacts = allContacts;
            AddContactMethod = addContactMethod;

            Client = new();
            StatusWatcher = new(allContacts);
            PingSender = new((ip, obj) => Client.Send(ip, obj), allContacts);
            Server = new(HandleIncomingData);

            Created = true;
        }

        public void Run()
        {
            if (Running)
                return;

            Running = true;
            Server.Run();
            Client.Run();
            PingSender.Run();
            StatusWatcher.Run();

            SubscribeToUserEvents();

            RemoteContactData contactData = new(Contact.UserContact);
            FullContactDataRequest request = new();

            RemoteObject contactDataObj = new(contactData);
            RemoteObject requestObj = new(request);
            foreach (var contact in AllContacts)
            {
                Client.Send(contact.Ip, contactDataObj);
                Client.Send(contact.Ip, requestObj);
            }
        }

        public void HandleIncomingData(RemoteObject obj, string ip)
        {
            Debug.WriteLine($"Received data from {ip}");
            // If the data demand to be confirmed, send back a confirmation.
            if (obj.Data.Confirm)
                SendBackConfirmation(obj.ObjectId, ip);

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

            switch (obj.Data.ObjType)
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

                case RemoteObject.ObjectType.DataReceiveConfirmation:
                    UncomfirmedData.RemoveWhere(o => o.ObjectId == ((ReceptionConfirmation)obj.Data).ConfirmedDataId);
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
        private Contact RetrieveContact(RemoteObject obj, string ip)
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

            if (obj.Data.ObjType == RemoteObject.ObjectType.ContactData)
            {
                contact = new(ip);

                AddContactMethod.Invoke(contact);
                return contact;
            }

            // Else return null (and the parent method will send a request to the sender to send back the data).

            return null;
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

        /// <summary> Sends a confirmation to {ip} about receiving object {objectId}</summary>
        private void SendBackConfirmation(string objectId, string ip)
        {
            ReceptionConfirmation confirmationData = new(objectId);
            RemoteObject confirmationObj = new(confirmationData);
            Client.Send(ip, confirmationObj);
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