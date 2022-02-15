using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClausaComm.Contacts;
using ClausaComm.Exceptions;
using ClausaComm.Extensions;
using ClausaComm.Forms;
using ClausaComm.Messages;
using ClausaComm.Network_Communication.Networking;
using ClausaComm.Network_Communication.Objects;
using ClausaComm.Utils;

namespace ClausaComm.Network_Communication
{
    // Singleton.
    public class NetworkBridge
    {
        public delegate void MessageReceivedEventHandler(ChatMessage message, Contact contact);
        public delegate void NewContactEventHandler(Contact contact);
        /// <summary>Invoked when a message is received from the network</summary>
        public event MessageReceivedEventHandler? MessageReceived;
        /// <summary>Invoked when a new contact is sent from the network.</summary>
        public event NewContactEventHandler? NewContactReceived;
        public bool Running { get; private set; }
        private readonly HashSet<Contact> AllContacts;
        private static bool InstanceCreated;
        
        /// <param name="allContacts">A collection with all the existing contacts</param>
        public NetworkBridge(HashSet<Contact> allContacts, Control uiThread)
        {
            if (InstanceCreated)
                throw new MultipleInstancesException(nameof(NetworkBridge));

            AllContacts = allContacts;

            // TODO: Don't run it all on the ui thread!
            PeerManager.OnReceive += (message, endpoint) => uiThread.Invoke(() => HandleIncomingData(message, endpoint.Address));
            PeerManager.OnConnect += endpoint => HandleNewConnection(endpoint.Address);
            PeerManager.OnDisconnect += endpoint => uiThread.Invoke(() =>
                AllContacts.First(contact => contact.Ip.Equals(endpoint.Address)).CurrentStatus = Contact.Status.Offline);

            InstanceCreated = true;
        }

        /// <summary>Runs the network connects to all contacts. Non-blocking.</summary>
        public void Run()
        {
            if (Running)
                return;

            Running = true;
            ThreadUtils.RunThread(() => PeerManager.Run(HandleServerRunning));

            void HandleServerRunning(bool running)
            {
                if (!running)
                {
                    Logger.Log($"{nameof(NetworkBridge)}: Server did not start - NOT trying to connect to contacts.");
                    return;
                }

                Logger.Log($"{nameof(NetworkBridge)}: Recursively trying to connect to all contacts.");
                AllContacts.ForEach(contact => Connect(contact));
                
                SubscribeToUserEvents();
            }
        }

        public static bool Connect(Contact contact)
        {
            bool connected = PeerManager.CreateConnection(contact.Ip);
            if (!connected)
                Logger.Log($"{nameof(NetworkBridge)}: Could not connect to {contact.Ip}");
            return connected;
        }

        // Send all user data on connection (both ongoing and ingoing).
        private static void HandleNewConnection(IPAddress ip)
        {
            RemoteContactData contactData = new(Contact.UserContact);
            PeerManager.Send(ip, new RemoteObject(contactData));
        }

        private void HandleIncomingData(RemoteObject obj, IPAddress ip)
        {
            Logger.Log($"Received data from {ip} (type: {obj.Data.ObjectType})");

            Contact? contact = RetrieveOrCreateContact(obj, ip);

            if (contact is null)
            {
                Logger.Log($"Could not get or create contact. Returning. ip: {ip}");
                return;
            }

            contact.Id ??= obj.ContactId;
            
            if (!contact.Ip.Equals(ip))
                contact.Ip = ip;

            if (contact.CurrentStatus == Contact.Status.Offline)
                contact.CurrentStatus = Contact.Status.Online;
            
            switch (obj.Data.ObjectType)
            {
                case RemoteObject.ObjectType.ContactData:
                    UpdateContactData((RemoteContactData)obj.Data, contact);
                    break;

                case RemoteObject.ObjectType.StatusUpdate:
                    UpdateContactStatus((RemoteStatusUpdate)obj.Data, contact);
                    break;

                case RemoteObject.ObjectType.ChatMessage:
                    HandleMessageReceived((ChatMessage)obj.Data, contact);
                    break;

                case RemoteObject.ObjectType.File: // Should never reach this switch statement, so we want to throw an exception if it does.
                default:
                    throw new NotImplementedException($"One of {nameof(RemoteObject)}'s ObjectTypes weren't implemented in {nameof(NetworkBridge)}'s {nameof(HandleIncomingData)} method.");
            }
        }

        public bool SendMessage(ChatMessage message, IPAddress ip)
        {
            RemoteObject obj = new(message);
            bool msgSendResult = PeerManager.Send(ip, obj);

            if (message.FilePath is not null)
                PeerManager.Send(ip, new RemoteObject(new RemoteFile(message.FilePath)));

            return msgSendResult;
        }

        /// <summary>
        /// Identifies the contact in the user's contact list based on ID and returns it.<br/>
        /// Else identifies it based on IP (but it must not have an ID) and returns it.<br/>
        /// Else if the object received is an object with contact data, creates it and returns it.<br/>
        /// Else returns null.
        /// </summary>
        private Contact? RetrieveOrCreateContact(RemoteObject obj, IPAddress ip)
        {
            // If AllContacts contains a contact with the sender's ID, return that contact.
            
            Contact contact = AllContacts.FirstOrDefault(c => c.Id == obj.ContactId);

            if (contact is not null)
                return contact;

            // else if AllContacts contains a contact with the sender's IP AND the contact's ID
            // is null (contact added by the user via IP but not initialized yet), return that contact.
            // The caller method will assign the ID.

            contact = AllContacts.FirstOrDefault(c => c.Ip.Equals(ip) && c.Id is null);

            if (contact is not null)
                return contact;

            // After this point we know that the contact doesn't exist in our list yet.

            // If the sender has sent all needed data to create it (ContactData object), create it and return it.
            // The parent method will assign the data to the contact.
            // Else return null.
            
            if (obj.Data.ObjectType != RemoteObject.ObjectType.ContactData)
                return null;
            
            contact = new Contact(ip);
            NewContactReceived?.Invoke(contact);
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
        private void SendToAll(RemoteObject obj) =>
            AllContacts.NotOffline().ForEach(contact => PeerManager.Send(contact.Ip, obj));

        private void HandleMessageReceived(ChatMessage message, Contact sender)
        {
            string filePath = string.IsNullOrEmpty(message.FilePath)
                ? null
                : Path.Combine(Paths.FileSavePath, Path.GetFileName(message.FilePath));
            message = ChatMessage.ReconstructMessage(message.Text, ChatMessage.Ways.In, message.Id, message.Time, filePath);
            MessageReceived?.Invoke(message, sender);
        }

        /// <summary> Updates {contact}'s status to match that in the {statusUpdate} object.</summary>
        private static void UpdateContactStatus(RemoteStatusUpdate statusUpdate, Contact contact)
            => contact.CurrentStatus = statusUpdate.Status;

        /// <summary> Updates {contact}'s data to match those in the {data} object.</summary>
        private static void UpdateContactData(RemoteContactData data, Contact contact)
        {
            if (data.Name is not null) 
                contact.Name = data.Name;

            if (data.Base64ProfilePic is not null)
            {
                Image img = ImageUtils.ImageFromBase64String(data.Base64ProfilePic);
                contact.ProfilePic = img;
            }
        }
    }
}