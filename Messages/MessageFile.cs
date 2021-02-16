namespace ClausaComm.Messages
{
    public abstract class MessageFile
    {
        public readonly string Filename;
        public readonly bool IsImage;

        protected MessageFile(string filename, bool isImage)
        {
            Filename = filename;
            IsImage = isImage;
        }
    }
}