using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ClausaComm.Contacts;
using ClausaComm.Messages;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ClausaComm.Network_Communication.Objects
{
    [Serializable]
    public readonly struct RemoteObject
    {
        private static readonly JsonSerializerSettings SerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            #if DEBUG
            Formatting = Formatting.Indented,
            #else
            Formatting = Formatting.None,
            #endif
        };
        public enum ObjectType { ChatMessage, File, ContactData, StatusUpdate }
        public ISendable Data { get; init; }
        public string ContactId { get; init; }

        public RemoteObject(ISendable data)
        {
            Data = data;
            ContactId = Contact.UserContact.Id;
        }
        
        [JsonConstructor]
        private RemoteObject(ISendable data, string contactId)
        {
            ContactId = contactId;
            Data = data;
        }
        
        public byte[] SerializeToUtf8Bytes() => Encoding.UTF8.GetBytes(Serialize());
        public string Serialize() => JsonConvert.SerializeObject(this, typeof(RemoteObject), SerializerSettings);

        public static RemoteObject Deserialize(byte[] obj) => Deserialize(Encoding.UTF8.GetString(obj));
        public static RemoteObject Deserialize(string obj) => JsonConvert.DeserializeObject<RemoteObject>(obj, SerializerSettings);
        
        public override string ToString() => $"ContactId: {ContactId} | Data ObjectType: {Data.ObjectType}";
    }
}