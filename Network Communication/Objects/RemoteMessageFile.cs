using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ClausaComm.Messages
{
    internal class RemoteMessageFile : MessageFile
    {
        public readonly byte[] Bytes;

        public RemoteMessageFile(string filename, bool isImage, byte[] bytes) : base(filename, isImage)
        {
            Bytes = bytes;
        }

        public RemoteMessageFile(LocalMessageFile localFile) : this(localFile.Filename, localFile.IsImage, File.ReadAllBytes(localFile.Path))
        {
        }
    }
}
