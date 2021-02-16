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
            // TODO: Profile all timers and check if it is more efficient to have just one global timer with a list of callbacks.
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
            if (contact is null || contact.Id is null)
            {
                if (contact is not null)
                    contact.Id = obj.ContactId;

                Client.Send(ip, new RemoteObject(new FullContactDataRequest()));
                return;
            }

            StatusWatcher.HandleActivityReceived(contact);

            switch (obj.Data.ObjType)
            {
                case RemoteObject.ObjectType.ContactData:
                    UpdateContactData((RemoteContactData)obj.Data, contact);
                    break;

                case RemoteObject.ObjectType.Message:
                    HandleMessageReceived((Message)obj.Data, contact);
                    break;

                case RemoteObject.ObjectType.StatusUpdate:
                    UpdateContactStatus((RemoteStatusUpdate)obj.Data, contact);
                    break;

                case RemoteObject.ObjectType.Ping:
                    // Ignored.
                    break;

                case RemoteObject.ObjectType.FullContactDataRequest:
                    SendFullContactData(ip);
                    break;

                case RemoteObject.ObjectType.DataReceiveConfirmation:
                    UncomfirmedData.RemoveWhere(o => o.ObjectId == ((DataReceiveConfirmation)obj.Data).ConfirmedDataId);
                    break;

                default:
                    throw new NotImplementedException($"One of {nameof(RemoteObject)}'s ObjectTypes weren't processed in {nameof(NetworkBridge)}'s {nameof(HandleIncomingData)} method.");
            }
        }

        public void SendMessage()
        {
        }

        private Contact RetrieveContact(RemoteObject obj, string ip)
        {
            // If AllContacts contains a contact with the sender's ID, return that contact.

            Contact contact = AllContacts.FirstOrDefault(c => c.Id == obj.ContactId);

            if (contact is not null)
                return contact;

            // else if AllContacts contains a contact with the sender's IP AND the contact's ID
            // is null (contact added by the user via IP but not initialized yet), return that contact.
            // The parent method will

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

        private void SendToAll(RemoteObject obj)
        {
            AllContacts.NotOffline().ForEach(contact => Client.Send(contact.Ip, obj));
        }

        private void SendBackConfirmation(string objectId, string ip)
        {
            DataReceiveConfirmation data = new(objectId);
            RemoteObject confirmationObj = new(data);
            Client.Send(ip, confirmationObj);
        }

        private void HandleMessageReceived(Message message, Contact sender)
        {
            // Handle the message, convert message.MessageFile to RemoteMessageFile etc.
        }

        private void SendFullContactData(string ip)
        {
            RemoteContactData data = new(Contact.UserContact);
            RemoteObject obj = new(data);
            Client.Send(ip, obj);
        }

        private static void UpdateContactStatus(RemoteStatusUpdate statusUpdate, Contact contact)
        {
            if (contact.CurrentStatus != statusUpdate.Status)
                contact.CurrentStatus = statusUpdate.Status;
        }

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