using ClausaComm.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using ClausaComm.Utils;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ClausaComm
{
    // TODO: Replace this fucker with Json... Seriously, how tf could I think that this was a good idea...
    public partial class Contact
    {
        private readonly IXmlFile Xml;
        public XmlFile ContactXml => Xml as XmlFile;

        // Creating a private interface so that those methods cannot be accessed publicly, but others can.
        private interface IXmlFile
        {
            public void Remove();

            public void Edit(SavedInfo attributeToChange, string newValue);

            public bool Save();
        }

        public class XmlFile : IXmlFile
        {
            private static readonly Dictionary<SavedInfo, string> InfoFileRepresentationDict = new()
            {
                { SavedInfo.Name, "name" },
                { SavedInfo.Ip, "ip" },
                { SavedInfo.Id, "id" },
                { SavedInfo.IsUser, "isUser" }
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
                        new XElement(InfoFileRepresentationDict[SavedInfo.Name], Contact.Name),
                        Contact.IsUser ? null : new XElement(InfoFileRepresentationDict[SavedInfo.Ip], Contact.Ip),
                        new XElement(InfoFileRepresentationDict[SavedInfo.Id], Contact.Id),
                        new XElement(InfoFileRepresentationDict[SavedInfo.IsUser], Contact.IsUser)
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

            void IXmlFile.Edit(SavedInfo attributeToChange, string newValue)
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
                => GetContactNodes(doc).FirstOrDefault(node => node.Element(InfoFileRepresentationDict[SavedInfo.Id]).Value == Contact.Id);

            private static IEnumerable<Contact> GetContacts()
            {
                string localIp = IpUtils.RefreshLocalIp();

                foreach (var contactNode in GetContactNodes())
                {
                    bool isUser = bool.Parse(contactNode.Element(InfoFileRepresentationDict[SavedInfo.IsUser]).Value);
                    string ip = contactNode.Element(InfoFileRepresentationDict[SavedInfo.Ip])?.Value;
                    string id = contactNode.Element(InfoFileRepresentationDict[SavedInfo.Id]).Value;
                    string name = contactNode.Element(InfoFileRepresentationDict[SavedInfo.Name]).Value;

                    var contact = new Contact(isUser ? localIp : ip) { _name = name, _save = false, _id = id };
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