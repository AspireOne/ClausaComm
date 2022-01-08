using ClausaComm.Network_Communication.Objects;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using ClausaComm.Extensions;

namespace ClausaComm.Messages
{
    public struct ChatMessage : ISendable
    {
        public enum Ways { In, Out }
        public RemoteObject.ObjectType ObjectType => RemoteObject.ObjectType.ChatMessage;

        public readonly long Time;
        public readonly Ways Way;
        public readonly string Text;
        //public readonly MessageFile File;
        public readonly string Id;
        public const int MaxTextLength = 6000;

        public ChatMessage(string text, Ways way = Ways.Out)
        {
            if (text?.Length > MaxTextLength)
                throw new ArgumentException("The text is too long.", nameof(text));
            
            Time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Id = IdGenerator.GenerateId(5);
            Text = text;
            Way = way;
        }

        private ChatMessage(string text, Ways way, string id, long time) : this(text, way)
        {
            Time = time;
            Id = id;
        }

        public override bool Equals(object? obj) => obj is ChatMessage message && message.Id == Id;
        public override int GetHashCode() => Time.GetHashCode();

        public static ChatMessage ReconstructMessage(string text, Ways way, string id, long time) => new ChatMessage(text, way, id, time);
    }
}