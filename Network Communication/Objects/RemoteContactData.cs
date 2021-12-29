using System;
using System.Drawing;
using ClausaComm.Contacts;
using ClausaComm.Utils;

namespace ClausaComm.Network_Communication.Objects
{
    [Serializable]
    public readonly struct RemoteContactData : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjectType => RemoteObject.ObjectType.ContactData;

        // Convert it so that it's serializable.
        public readonly string? Base64ProfilePic;
        public readonly string? Name;

        public RemoteContactData(string? name = null, Image? profilePic = null)
        {
            Name = name;
            Base64ProfilePic = ImageUtils.ImageToBase64String(profilePic);
        }

        public RemoteContactData(Contact contact) : this(contact.Name, contact.ProfilePic) { }
    }
}