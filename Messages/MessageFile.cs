using System;

namespace ClausaComm.Messages
{
    public abstract class MessageFile
    {
        public readonly string Filename;
        public readonly bool IsImage;
        public const int MaxFileSizeBytes = 500_000_000; // 500 mb. (x * 10^6)

        protected MessageFile(string filename, bool isImage)
        {
            Filename = filename;
            IsImage = isImage;
        }
    }
}