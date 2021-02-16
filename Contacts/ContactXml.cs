using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using ClausaComm.Utils;

namespace ClausaComm.Contacts
{
    // TODO: Replace this fucker with Json... Seriously, how tf could I think that this was a good idea...
    public partial class Contact
    {
        private readonly IXmlFile Xml;
        // public XmlFile ContactXml => Xml as XmlFile;

        // Creating a private interface so that those methods cannot be accessed publicly, but others can.
        private interface IXmlFile
        {
            public void Remove();

            public void Edit(XmlSavedInfo attributeToChange, string newValue);

            public bool Save();
        }

        public class XmlFile : IXmlFile
        {
            private static readonly Dictionary<XmlSavedInfo, string> InfoFileRepresentationDict = new()
            {
                { XmlSavedInfo.Name, "name" },
                { XmlSavedInfo.Ip, "ip" },
                { XmlSavedInfo.Id, "id" },
                { XmlSavedInfo.IsUser, "isUser" }
            };

            private const string ContactNodeName = "contact";
            private readonly Contact Contact;
            public static readonly HashSet<Contact> Contacts = GetContacts().ToHashSet();

            public XmlFile(Contact contact) => Contact = contact;

            bool IXmlFile.Save()
            {
                var doc = XDocument.Load(ProgramDirectory.ContactsPath);

                if (doc.ToString().Contains(Contact.Id ?? Contact.Ip))
                    return false;

                Contacts.Add(Contact);

                doc.Root.Add(
                    new XElement(ContactNodeName,
                        new XElement(InfoFileRepresentationDict[XmlSavedInfo.Name], Contact.Name),
                        Contact.IsUser ? null : new XElement(InfoFileRepresentationDict[XmlSavedInfo.Ip], Contact.Ip),
                        new XElement(InfoFileRepresentationDict[XmlSavedInfo.Id], Contact.Id),
                        new XElement(InfoFileRepresentationDict[XmlSavedInfo.IsUser], Contact.IsUser)
                    )
                );
                doc.Save(ProgramDirectory.ContactsPath);

                return true;
            }

            void IXmlFile.Remove()
            {
                Contacts.Remove(Contact);

                var doc = XDocument.Load(ProgramDirectory.ContactsPath);
                XElement contactNode = GetNode(doc);

                if (contactNode is not null)
                {
                    contactNode.Remove();
                    doc.Save(ProgramDirectory.ContactsPath);
                }
            }

            void IXmlFile.Edit(XmlSavedInfo attributeToChange, string newValue)
            {
                var doc = XDocument.Load(ProgramDirectory.ContactsPath);
                XElement contactNode = GetNode(doc);

                if (contactNode is null)
                    throw new Exception("Contact was not found in the config file.");

                XElement element = contactNode.Element(InfoFileRepresentationDict[attributeToChange]);

                if (element is null)
                    throw new ArgumentException("The passed attribute to be changed was not found", nameof(attributeToChange));

                element.Value = newValue;
                doc.Save(ProgramDirectory.ContactsPath);
            }

            private static IEnumerable<XElement> GetContactNodes(XDocument doc = null)
            {
                var document = doc ?? XDocument.Load(ProgramDirectory.ContactsPath);
                return document.Root?.Elements() ?? Enumerable.Empty<XElement>();
            }

            // TODO: Check if saving a contact without an ID throws an exception.
            private XElement GetNode(XDocument doc = null)
                => GetContactNodes(doc).FirstOrDefault(node => node.Element(InfoFileRepresentationDict[XmlSavedInfo.Id]).Value == Contact.Id);

            private static IEnumerable<Contact> GetContacts()
            {
                string localIp = IpUtils.RefreshLocalIp();

                foreach (var contactNode in GetContactNodes())
                {
                    bool isUser = bool.Parse(contactNode.Element(InfoFileRepresentationDict[XmlSavedInfo.IsUser]).Value);
                    string ip = contactNode.Element(InfoFileRepresentationDict[XmlSavedInfo.Ip])?.Value;
                    string id = contactNode.Element(InfoFileRepresentationDict[XmlSavedInfo.Id]).Value;
                    string name = contactNode.Element(InfoFileRepresentationDict[XmlSavedInfo.Name]).Value;

                    var contact = new Contact(isUser ? localIp : ip) { _name = name, _save = false, Id = id };
                    TryGetProfilePicture(contact.ProfilePicPath, out Image profileImage);

                    contact.ProfilePic = profileImage;
                    contact._save = true;

                    Debug.WriteLine("Found contact from xml. Data: " + contact);
                    yield return contact;
                }
            }
        }
    }
}