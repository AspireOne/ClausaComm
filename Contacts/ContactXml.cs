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

            public void Save(out bool exists);
        }

        public class XmlFile : IXmlFile
        {
            private static readonly Dictionary<XmlSavedInfo, string> InfoXmlRepresentation = new()
            {
                { XmlSavedInfo.IsUser, "isUser" },
                { XmlSavedInfo.Name, "name" },
                { XmlSavedInfo.Ip, "ip" },
                { XmlSavedInfo.Id, "id" }
            };

            private static readonly XDocument Doc = XDocument.Load(ProgramDirectory.ContactsPath);
            private const string ContactNodeName = "contact";
            private readonly Contact Contact;
            public static readonly HashSet<Contact> Contacts = GetContacts().ToHashSet();

            public XmlFile(Contact contact) => Contact = contact;

            void IXmlFile.Save(out bool exists)
            {
                if (Doc.ToString().Contains(Contact.Id ?? Contact.Ip))
                {
                    exists = true;
                    return;
                }

                Contacts.Add(Contact);

                Doc.Root.Add(
                    new XElement(ContactNodeName,
                        new XElement(InfoXmlRepresentation[XmlSavedInfo.Name], Contact.Name),
                        new XElement(InfoXmlRepresentation[XmlSavedInfo.Ip], Contact.IsUser ? null : Contact.Ip),
                        new XElement(InfoXmlRepresentation[XmlSavedInfo.Id], Contact.Id),
                        new XElement(InfoXmlRepresentation[XmlSavedInfo.IsUser], Contact.IsUser)
                    )
                );

                Doc.Save(ProgramDirectory.ContactsPath);

                exists = false;
            }

            void IXmlFile.Remove()
            {
                Contacts.Remove(Contact);

                XElement contactNode = GetNode();

                if (contactNode is not null)
                {
                    contactNode.Remove();
                    Doc.Save(ProgramDirectory.ContactsPath);
                }
            }

            void IXmlFile.Edit(XmlSavedInfo attributeToChange, string newValue)
            {
                XElement contactNode = GetNode();

                if (contactNode is null)
                    throw new("Contact was not found in the config file.");

                XElement element = contactNode.Element(InfoXmlRepresentation[attributeToChange]);

                if (element is null)
                    throw new ArgumentException("The passed attribute to be changed was not found", nameof(attributeToChange));

                element.Value = newValue;
                Doc.Save(ProgramDirectory.ContactsPath);
            }

            private static IEnumerable<XElement> GetContactNodes()
            {
                return Doc.Root?.Elements() ?? Enumerable.Empty<XElement>();
            }

            private XElement GetNode()
            {
                return GetContactNodes().FirstOrDefault(node =>
                {
                    // Normal contact always has IP and may not have ID, and User always has ID and never has IP.

                    string id = node.Element(InfoXmlRepresentation[XmlSavedInfo.Id])?.Value;

                    if (id is not null)
                        return id == Contact.Id;

                    // Ip will never be null here.
                    string ip = node.Element(InfoXmlRepresentation[XmlSavedInfo.Ip])?.Value;

                    if (ip is not null)
                        return ip == Contact.Ip;

                    throw new("A contact saved to XML had neither ID or IP.");
                });
            }

            private static IEnumerable<Contact> GetContacts()
            {
                string localIp = IpUtils.RefreshLocalIp();

                foreach (var node in GetContactNodes())
                {
                    bool isUser = bool.Parse(node.Element(InfoXmlRepresentation[XmlSavedInfo.IsUser]).Value);
                    string ip = node.Element(InfoXmlRepresentation[XmlSavedInfo.Ip])?.Value;
                    string id = node.Element(InfoXmlRepresentation[XmlSavedInfo.Id])?.Value;
                    string name = node.Element(InfoXmlRepresentation[XmlSavedInfo.Name]).Value;

                    yield return ReconstructContact();

                    Contact ReconstructContact()
                    {
                        var contact = new Contact(isUser ? localIp : ip) { _name = name, _save = false, Id = id, IsUser = true };
                        TryGetProfilePicture(contact.ProfilePicPath, out Image profileImage);

                        contact.ProfilePic = profileImage;
                        contact._save = true;

                        Debug.WriteLine("Found contact from xml. Data: " + contact);
                        return contact;
                    }
                }
            }
        }
    }
}