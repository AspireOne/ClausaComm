using ClausaComm.Network_Communication.Objects;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using ClausaComm.Extensions;
using Newtonsoft.Json;

namespace ClausaComm.Messages
{
    [Serializable]
    public readonly struct ChatMessage : ISendable
    {
        public enum Ways { In, Out }
        public const int MaxTextLength = 6000;
        public RemoteObject.ObjectType ObjectType => RemoteObject.ObjectType.ChatMessage;

        public long Time { get; private init; }
        public Ways Way { get; init; }
        public string Text { get; init; }
        //public readonly MessageFile File;
        public string Id { get; private init; }

        public ChatMessage(string text, Ways way = Ways.Out)
        {
            if (text?.Length > MaxTextLength)
                throw new ArgumentException("The text is too long.", nameof(text));
            
            Time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Id = IdGenerator.GenerateId(5);
            Text = text;
            Way = way;
        }

        [JsonConstructor]
        private ChatMessage(string text, Ways way, string id, long time) : this(text, way)
        {
            Time = time;
            Id = id;
        }

        public static ChatMessage ReconstructMessage(string text, Ways way, string id, long time) => new(text, way, id, time);
        
        public override int GetHashCode() => Time.GetHashCode();
        public override bool Equals(object? obj) => obj is ChatMessage message && message.Id == Id;
        public static bool operator ==(ChatMessage left, ChatMessage right) => left.Equals(right);
        public static bool operator !=(ChatMessage left, ChatMessage right) => !(left == right);
    }
}