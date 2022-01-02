using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Xml.Linq;

namespace ClausaComm.Messages
{
    public static class MessagesXml
    {
        private static readonly XDocument Doc = XDocument.Load(ProgramDirectory.MessagesPath);
        
        public static void SaveMessage(ChatMessage message, string contactId)
        {
            lock (Doc)
            {
                XElement contactNode = GetContactNode(contactId);

                if (contactNode is null)
                {
                    Doc.Root.Add(new XElement("contact", new XAttribute("id", contactId)));
                    SaveMessage(message, contactId);
                    return;
                }
                
                contactNode.Add(new XElement(message.Way == ChatMessage.Ways.In ? "IN" : "OUT", message.Text, new XAttribute("id", message.Id)));
                Doc.Save(ProgramDirectory.MessagesPath);
            }
        }

        public static IEnumerable<ChatMessage> GetMessages(int messageCount, string contactId)
        {
            XElement contactNode = GetContactNode(contactId);
            if (contactNode is null)
                yield break;

            int i = 0;
            foreach (XElement element in contactNode.Elements())
            {
                if (i++ >= messageCount)
                    yield break;
                
                ChatMessage.Ways way = element.Name == "IN" ? ChatMessage.Ways.In : ChatMessage.Ways.Out;
                string id = element.Attribute("id").Value;
                string text = element.Value;
                
                ChatMessage message = ChatMessage.ReconstructMessage(text, way, id);
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