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
    public class RemoteObject
    {
        public enum ObjectType { Message }

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            IncludeFields = true,
            MaxDepth = 50,
            PropertyNamingPolicy = null
        };

        public readonly ISendable Data;
        

        public RemoteObject(ISendable data)
        {
            Data = data;
        }

        public string Serialize()
        {
            //return JsonSerializer.SerializeToUtf8Bytes(this, typeof(RemoteObject), SerializerOptions);
            return JsonSerializer.Serialize<RemoteObject>(this, SerializerOptions);
        }

        public static RemoteObject Deserialize(string obj)
        {
            return JsonSerializer.Deserialize<RemoteObject>(obj, SerializerOptions);
        }
    }
}
