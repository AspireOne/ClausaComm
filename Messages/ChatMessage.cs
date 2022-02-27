using ClausaComm.Network_Communication.Objects;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ClausaComm.Extensions;
using ClausaComm.Properties;
using Newtonsoft.Json;

namespace ClausaComm.Messages
{
    [Serializable]
    public class ChatMessage : ISendable
    {
        public event EventHandler<bool> DeliveredChanged;
        public enum Ways { In, Out }
        public const int MaxTextLength = 6000;
        public RemoteObject.ObjectType ObjectType => RemoteObject.ObjectType.ChatMessage;

        public long Time { get; private init; }
        [JsonIgnore]
        public Ways Way { get; init; }
        public string Text { get; init; }
        public string Id { get; private init; }
        
        public string? FilePath { get; set; }

        [JsonIgnore] private bool _delivered;
        [JsonIgnore]
        public bool Delivered
        {
            get => _delivered;
            set
            {
                _delivered = value;
                DeliveredChanged?.Invoke(this, value);
            }
        }

        public ChatMessage(string text, Ways way = Ways.Out)
        {
            if (text?.Length > MaxTextLength)
                throw new ArgumentException("The text is too long.", nameof(text));
            
            Time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Id = IdGenerator.GenerateId(6);
            Delivered = true;
            Text = text;
            Way = way;
        }

        [JsonConstructor]
        private ChatMessage(string text, Ways way, string id, long time, string? filePath) : this(text, way)
        {
            Time = time;
            FilePath = filePath;
            Id = id;
        }

        public static ChatMessage ReconstructMessage(string text, Ways way, string id, long time, string? filePath) => new(text, way, id, time, filePath);

        public override string ToString() => $"Way: {Way} | Id: {Id} | Time: {Time} | Text: {Text}";

        public override int GetHashCode() => Time.GetHashCode();
        // "Way" check is there because when the user is chatting with themselves, the messages will have the exact
        // same properties (including time, id...) and the only difference will be their way "out / in".
        public override bool Equals(object? obj) => obj is ChatMessage message && message.Id == Id/* && message.Way == Way*/;
        public static bool operator ==(ChatMessage left, ChatMessage right) => left.Equals(right);
        public static bool operator !=(ChatMessage left, ChatMessage right) => !left.Equals(right);
    }
}