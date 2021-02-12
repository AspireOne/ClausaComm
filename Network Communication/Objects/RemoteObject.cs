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
        public enum ObjectType { Message, ContactData, StatusUpdate, Ping, FullContactDataRequest, DataReceiveConfirmation }

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            IncludeFields = true,
            MaxDepth = 64,
            PropertyNamingPolicy = null
        };

        public readonly ISendable Data;
        public readonly string ContactId;
        public readonly string ObjectId;


        public RemoteObject(ISendable data)
        {
            Data = data;
            ContactId = Contact.UserContact.Id;
            // We don't need the ID to be too long - the id will be needed usually just for a few seconds.
            ObjectId = IdGenerator.GenerateId(4);
        }

        public string Serialize() => JsonSerializer.Serialize(this, SerializerOptions);

        public byte[] SerializeToUtf8Bytes() => JsonSerializer.SerializeToUtf8Bytes(this, typeof(RemoteObject), SerializerOptions);

        public static RemoteObject Deserialize(byte[] obj) => JsonSerializer.Deserialize<RemoteObject>(obj, SerializerOptions);

        public static RemoteObject Deserialize(string obj) => JsonSerializer.Deserialize<RemoteObject>(obj, SerializerOptions);
    }
}
