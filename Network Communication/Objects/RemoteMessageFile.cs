using System.IO;
using ClausaComm.Messages;

namespace ClausaComm.Network_Communication.Objects
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
