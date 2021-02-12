using ClausaComm.Communication;
using ClausaComm.Forms;
using ClausaComm.Network;
using ClausaComm.Network_Communication;
using ClausaComm.Network_Communication.Networking;
using ClausaComm.Network_Communication.Objects;
using ClausaComm.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm
{
    public static class NetworkBridge
    {
        private static readonly HashSet<RemoteObject> UncomfirmedData = new();
        private static readonly Server Server = new();
        private static readonly Client Client = new();
        private static readonly ContactStatusWatcher StatusWatcher = new();
        private static readonly PingSender PingSender = new((ip, obj) => Client.Send(ip, obj));

        public static bool Running { get; private set; } = false;

        // We need to add an explicit Run method; the class wouldn't initialize itself before any member would be referenced.
        public static void Run()
        {
            if (Running)
                return;

            Running = true;
            Server.Start();
            Client.Start();
            PingSender.Start();
            StatusWatcher.Start();
        }

        public static void HandleIncomingData(RemoteObject obj, string ip)
        {
            // If the data demand to be confirmed, send back a confirmation.
            if (obj.Data.Confirm)
                SendBackConfirmation(obj.ObjectId, ip);

            // TODO: It may not be needed to always retrieve the full contact, but rather just a part of it (for example id).
            // Take a look at it.
            Contact contact = MainForm.Form.Contacts.FirstOrDefault(c => c.Id == obj.ContactId);
            if (contact is null)
            {
                contact = new(ip, obj.ContactId);
                MainForm.Form.AddContact(contact);
            }

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
                    StatusWatcher.HandlePingReceived(contact.Id);
                    break;

                case RemoteObject.ObjectType.FullContactDataRequest:
                    SendBackFullContactData(ip);
                    break;

                case RemoteObject.ObjectType.DataReceiveConfirmation:
                    UncomfirmedData.RemoveWhere(o => o.ObjectId == ((DataReceiveConfirmation)obj.Data).ConfirmedDataId);
                    break;
            }
        }

        private static void SendBackConfirmation(string objectId, string ip)
        {
            DataReceiveConfirmation data = new(objectId);
            RemoteObject confirmationObj = new(data);
            Client.Send(ip, confirmationObj);
        }

        private static void HandleMessageReceived(Message message, Contact sender)
        {
            // Handle the message, convert message.MessageFile to RemoteMessageFile etc.
        }

        private static void SendBackFullContactData(string ip)
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
                contact.ProfilePic = ImageUtils.ImageFromBase64String(data.Base64ProfilePic);
        }
    }
}
