using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ClausaComm.Messages
{
    [Serializable]
    internal class RemoteMessageFile : MessageFile
    {
        private static readonly JsonSerializerOptions SerializerOptions =  new() {IncludeFields = true, WriteIndented = false };
        public readonly byte[] Bytes;


        public RemoteMessageFile(string filename, bool isImage, byte[] bytes) : base(filename, isImage)
        {
            Bytes = bytes;
        }

        public string Serialize()
        {
            return JsonSerializer.Serialize(this, typeof(RemoteMessageFile), SerializerOptions);
        }

        public RemoteMessageFile Deserialize(byte[] json)
        {
            return JsonSerializer.Deserialize<RemoteMessageFile>(json, SerializerOptions);
        }

        public RemoteMessageFile(LocalMessageFile localFile) : this(localFile.Filename, localFile.IsImage, File.ReadAllBytes(localFile.Path))
        {
        }
    }
}
