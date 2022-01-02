using ClausaComm.Network_Communication.Objects;
using System;
using System.Runtime.CompilerServices;

namespace ClausaComm.Messages
{
    public struct ChatMessage : ISendable
    {
        public enum Ways { In, Out }
        public RemoteObject.ObjectType ObjectType => RemoteObject.ObjectType.ChatMessage;
        public readonly Ways Way;
        public readonly string Text;
        //public readonly MessageFile File;
        public readonly string Id;
        public const int MaxTextLength = 6000;

        public ChatMessage(string text, Ways way = Ways.Out)
        {
            if (text?.Length > MaxTextLength)
                throw new ArgumentException("The text is too long.", nameof(text));
            
            Text = text;
            Id = IdGenerator.GenerateId(5);
            Way = way;
        }

        private ChatMessage(string text, Ways way, string id) : this(text, way) => Id = id;

        public static ChatMessage ReconstructMessage(string text, Ways way, string id) =>
            new ChatMessage(text, way, id);
    }
}