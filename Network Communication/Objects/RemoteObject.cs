using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ClausaComm.Contacts;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ClausaComm.Network_Communication.Objects
{
    [Serializable]
    public readonly struct RemoteObject
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
        };
        
        public enum ObjectType
        {
            Message,
            ContactData,
            StatusUpdate
        }

        public readonly ISendable Data;
        public readonly string ContactId;


        public RemoteObject(ISendable data)
        {
            Data = data;
            ContactId = Contact.UserContact.Id;
        }

        public string Serialize() => JsonConvert.SerializeObject(this, typeof(RemoteObject), SerializerSettings);
        public byte[] SerializeToUtf8Bytes() => Encoding.UTF8.GetBytes(Serialize());
        
        public static RemoteObject Deserialize(byte[] obj) => Deserialize(Encoding.UTF8.GetString(obj));
        public static RemoteObject Deserialize(string obj) => JsonConvert.DeserializeObject<RemoteObject>(obj, SerializerSettings);
    }
}