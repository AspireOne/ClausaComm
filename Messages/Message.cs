using ClausaComm.Communication;
using ClausaComm.Messages;
using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;
using System.Drawing;

namespace ClausaComm
{
    public readonly struct Message : ISendable
    {
        public RemoteObject.ObjectType ObjType => RemoteObject.ObjectType.Message;
        bool ISendable.Confirm => true;
        public readonly string Text;
        public readonly MessageFile File;
        public readonly string Id;

        public Message(string text) : this(text, null) { }

        public Message(MessageFile file) : this(null, file) { }

        private Message(string text, MessageFile file)
        {
            Text = text;
            File = file;
            Id = IdGenerator.GenerateId();
        }
    }
}