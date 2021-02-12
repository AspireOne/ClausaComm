using ClausaComm.Communication;
using ClausaComm.Forms;
using ClausaComm.Network;
using ClausaComm.Network_Communication;
using ClausaComm.Network_Communication.Networking;
using ClausaComm.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm
{
    // TODO: Rename to NetworkBridge?
    public static class NetworkCenter
    {
        public static bool Running { get; private set; } = false;
        private static readonly Server Server = new();
        private static readonly Client Client = new();
        /*
        private readonly HashSet<Contact> _onlineContacts = new();

        public IReadOnlyCollection<HashSet<Contact>> OnlineContacts => (IReadOnlyCollection<HashSet<Contact>>)_onlineContacts;
        */

        static NetworkCenter()
        {

        }

        // We need to add an explicit Run method; the class wouldn't initialize itself before any member would be referenced.
        public static void Run()
        {
            if (Running)
                return;

            Running = true;
            Server.RunAsync();
            Client.RunAsync();
        }

        public static void HandleIncomingData(RemoteObject obj, string ip)
        {
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
                    
                    break;
                case RemoteObject.ObjectType.StatusUpdate:
                    UpdateContactStatus((RemoteStatusUpdate)obj.Data, contact);
                    break;
            }
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

            if (data.Base64ProfilePicture is not null)
                contact.ProfilePic = ImageUtils.ImageFromBase64String(data.Base64ProfilePicture);
        }
    }
}
