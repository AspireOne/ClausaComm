using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;

namespace ClausaComm.Messages
{
    public static class MessagesXml
    {
        private static readonly XDocument Doc = XDocument.Load(ProgramDirectory.MessagesPath);
        private const string InNodeName = "in";
        private const string OutNodeName = "out";
        private const string ContactNodeName = "contact";
        private const string IdAttrName = "id";
        private const string TimeAttrName = "time";
        
        public static void SaveMessage(ChatMessage message, string contactId)
        {
            lock (Doc)
            {
                XElement contactNode = GetContactNode(contactId);

                if (contactNode is null)
                {
                    Doc.Root.Add(new XElement(ContactNodeName, new XAttribute(IdAttrName, contactId)));
                    SaveMessage(message, contactId);
                    return;
                }
                
                contactNode.Add(
                    new XElement(message.Way == ChatMessage.Ways.In ? InNodeName : OutNodeName, // First argument - name.
                        new XAttribute(IdAttrName, message.Id),
                        new XAttribute(TimeAttrName, message.Time),
                        message.Text // Value.
                        ));
                Doc.Save(ProgramDirectory.MessagesPath);
            }
        }

        public static IEnumerable<ChatMessage> GetMessages(string contactId, int messageCount = int.MaxValue)
        {
            XElement contactNode = GetContactNode(contactId);
            if (contactNode is null)
                yield break;

            int i = 0;
            foreach (XElement node in contactNode.Elements())
            {
                if (i++ >= messageCount)
                    yield break;
                
                ChatMessage.Ways way = node.Name == InNodeName ? ChatMessage.Ways.In : ChatMessage.Ways.Out;
                string id = node.Attribute(IdAttrName).Value;
                string text = node.Value;
                long time = long.Parse(node.Attribute(TimeAttrName).Value);
                
                ChatMessage message = ChatMessage.ReconstructMessage(text, way, id, time);
                yield return message;
            }
        }

        private static XElement? GetContactNode(string contactId)
        {
            lock (Doc)
            {
                return Doc.Root?.Elements().FirstOrDefault(node => node.Attribute("id")?.Value == contactId);   
            }
        }
    }
}