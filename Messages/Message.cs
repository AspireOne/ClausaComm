using ClausaComm.Communication;
using ClausaComm.Messages;
using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;
using System.Drawing;

namespace ClausaComm
{
    public class Message : ISendable
    {
        public RemoteObject.ObjectType ObjType => RemoteObject.ObjectType.Message;
        public readonly string Text;
        public readonly MessageFile File;
        public readonly string Id = IdGenerator.GenerateId();

        public Message(string text)
        {
            Text = text /*?? ""*/;
        }

        public Message(MessageFile file)
        {
            File = file;
        }
    }
}