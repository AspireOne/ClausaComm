using System;
using System.Drawing;
using ClausaComm.Contacts;
using ClausaComm.Utils;

namespace ClausaComm.Network_Communication.Objects
{
    [Serializable]
    public readonly struct RemoteContactData : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjType => RemoteObject.ObjectType.ContactData;
        bool ISendable.Confirm => false;

        // Convert it so that it's serializable.
        public readonly string Base64ProfilePic;

        public readonly string Name;
        //public readonly string Id;

        public RemoteContactData(/*string id = null,*/string name = null, Image profilePic = null)
        {
            //Id = id;
            Name = name;
            Base64ProfilePic = ImageUtils.ImageToBase64String(profilePic);
        }

        public RemoteContactData(Contact contact) : this(/*contact.Id, */contact.Name, contact.ProfilePic)
        {
        }
    }
}