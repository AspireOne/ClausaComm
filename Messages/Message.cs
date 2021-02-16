using ClausaComm.Network_Communication.Objects;

namespace ClausaComm.Messages
{
    public readonly struct Message : ISendable
    {
        public RemoteObject.ObjectType ObjType => RemoteObject.ObjectType.Message;
        bool ISendable.Confirm => true;
        public readonly string Text;
        public readonly MessageFile File;
        public readonly string Id;
        public const int MaxTextLength = 10000;

        public Message(string text) : this(text, null)
        {
        }

        public Message(MessageFile file) : this(null, file)
        {
        }

        private Message(string text, MessageFile file)
        {
            Text = text;
            File = file;
            Id = IdGenerator.GenerateId();
        }
    }
}