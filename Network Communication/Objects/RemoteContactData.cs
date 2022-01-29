using System;
using System.Diagnostics;
using System.Drawing;
using ClausaComm.Contacts;
using ClausaComm.Utils;
using Newtonsoft.Json;

namespace ClausaComm.Network_Communication.Objects
{
    [Serializable]
    public readonly struct RemoteContactData : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjectType => RemoteObject.ObjectType.ContactData;

        // Convert it so that it's serializable.
        // Must have a getter and a setter (resp. init) because otherwise deserialization doesn't work (JSON.NET).
        public string? Base64ProfilePic { get; private init; }
        public string? Name { get; init; }

        public RemoteContactData(string? name = null, Image? profilePic = null)
        {
            Name = name;
            Base64ProfilePic = ImageUtils.ImageToBase64String(profilePic);
        }
        
        [JsonConstructor]
        private RemoteContactData(string name, string base64ProfilePic)
        {
            Name = name;
            Base64ProfilePic = base64ProfilePic;
        }

        public RemoteContactData(Contact contact) : this(contact.Name, contact.ProfilePic) { }
    }
}