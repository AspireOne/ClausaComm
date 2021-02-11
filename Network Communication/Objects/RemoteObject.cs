using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using ClausaComm.Communication;

namespace ClausaComm
{
    [Serializable]
    public readonly struct RemoteObject
    {
        public enum ObjectType { Message, ContactData, StatusUpdate }

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            IncludeFields = true,
            MaxDepth = 64,
            PropertyNamingPolicy = null
        };

        public readonly ISendable Data;
        public readonly string ContactId;
        

        public RemoteObject(ISendable data)
        {
            Data = data;
            ContactId = Contact.UserContact.Id;
        }

        public string Serialize() => JsonSerializer.Serialize<RemoteObject>(this, SerializerOptions);

        public byte[] SerializeToUtf8Bytes() => JsonSerializer.SerializeToUtf8Bytes(this, typeof(RemoteObject), SerializerOptions);

        public static RemoteObject Deserialize(byte[] obj) => JsonSerializer.Deserialize<RemoteObject>(obj, SerializerOptions);

        public static RemoteObject Deserialize(string obj) => JsonSerializer.Deserialize<RemoteObject>(obj, SerializerOptions);
    }
}
