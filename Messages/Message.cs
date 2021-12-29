using ClausaComm.Network_Communication.Objects;
using System;

namespace ClausaComm.Messages
{
    public readonly struct Message : ISendable
    {
        public RemoteObject.ObjectType ObjectType => RemoteObject.ObjectType.Message;
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
            if (text?.Length > MaxTextLength)
                throw new ArgumentException("The text is too long.", nameof(text));

            Text = text;
            File = file;
            Id = IdGenerator.GenerateId(4);
        }
    }
}