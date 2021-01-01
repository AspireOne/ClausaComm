using ClausaComm.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml.Linq;
using ClausaComm.Utils;

namespace ClausaComm
{
    public class Contact
    {
        public event EventHandler<Status> StatusChange;
        public event EventHandler<string> NameChange;
        public event EventHandler<Image> ProfilePicChange;
        public enum Status { Online, Idle, Offline }
        public const int MaxNameLength = 35;
        public const int MinNameLength = 3;
        private readonly IXmlFile Config;
        //public XmlFile Xml => Config as XmlFile;
        //public readonly HashSet<Contact> UnreadMessages = new HashSet<Contact>();

        public bool IsUser => _isUser;
        public string ProfilePicPath => Path.Combine(ProgramDirectory.ProfilePicsDirPath, $"{Ip}.png");
        private bool HasDefaultProfilePic => _hasDefaultProfilePic;
        private enum SavedInfo { Id, Name, Ip, ProfilePic }

        #region backing fields
        private Status _status = Status.Offline;
        private Image _profileImage = Resources.default_pfp;
        private string _name = "Unknown";
        private readonly string _ip;
        private bool _isUser = false;
        private bool _save = false;
        private bool _hasDefaultProfilePic = true;
        private string _Id;
        #endregion

        #region Properties
        public string Id
        {
            get => _Id;
            private init => _Id = value;
        }

        public Status CurrentStatus
        {
            get => _status;
            set
            {
                if (_status == value)
                    return;
                _status = value;
                StatusChange?.Invoke(this, value);
            }
        }

        public string Ip
        {
            get => _ip;
            private init
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value), "The supplied IP was null.");
                else if (!IpUtils.IsIpCorrect(value))
                    throw new InvalidIpException($"The supplied IP ({value}) was incorrect.");
                else
                {
                    _ip = value;
                    _isUser = IpUtils.LocalIp == value;
                }
            }
        }

        public Image ProfilePic
        {
            get => _profileImage;
            set
            {
                if (value == ProfilePic)
                    return;

                if (value is null || value.Equals(Resources.default_pfp))
                {
                    _profileImage = Resources.default_pfp;
                    _hasDefaultProfilePic = true;
                    if (Save)
                        DeleteProfilePicture();
                }
                else
                {
                    _profileImage = value;
                    _hasDefaultProfilePic = false;
                    if (Save)
                        SaveProfilePicture();
                }

                ProfilePicChange?.Invoke(this, _profileImage);
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (Name == value)
                    return;

                _name = value.Length is <= MaxNameLength and >= MinNameLength
                    ? value
                    : throw new ArgumentException($@"The supplied name's length is not right.
                       Should be <= {MaxNameLength} and >= {MinNameLength}, but was {value.Length})", nameof(value));

                NameChange?.Invoke(this, value);

                if (Save)
                    Config.Edit(SavedInfo.Name, value);
            }
        }

        public bool Save
        {
            get => _save;
            set
            {
                if (Save == value)
                    return;

                if (_save = value)
                {
                    bool exists = !Config.Save();

                    if (exists)
                        throw new Exception("One instance of this contact is already saved.");

                    if (!HasDefaultProfilePic)
                        SaveProfilePicture();
                }
                else
                {
                    Config.Remove();
                    DeleteProfilePicture();
                }
            }
        }
        #endregion

        public Contact(string ip)
        {
            Ip = ip;
            Config = new XmlFile(this);
            Id = IdGenerator.GenerateId();
        }

        public override bool Equals(object obj) => obj is Contact contact && contact.Id == Id;

        public override string ToString() => $"IP: {Ip}\nName: {Name}\nCurrentStatus: {CurrentStatus}\nID: {Id}";

        public override int GetHashCode() => int.Parse(Ip.Replace(".", ""));

        // Creating a private interface so that those methods cannot be accessed publicly.
        private interface IXmlFile
        {
            public void Remove();
            public void Edit(SavedInfo attributeToChange, string newValue);
            public bool Save();
        }

        public class XmlFile : IXmlFile
        {
            private static readonly Dictionary<SavedInfo, string> InfoFileRepresentationDict = new Dictionary<SavedInfo, string>()
            {
                {SavedInfo.Name, "name"},
                {SavedInfo.Ip, "ip"},
                {SavedInfo.Id, "id" }
            };
            private const string ContactNodeName = "contact";
            private readonly Contact Contact;
            private static readonly string Path = ProgramDirectory.ContactsPath;

            public XmlFile(Contact contact) => Contact = contact;

            bool IXmlFile.Save()
            {
                var doc = XDocument.Load(Path);

                if (doc.ToString().Contains(Contact.Id))
                    return false;

                doc.Root.Add(
                    new XElement(ContactNodeName,
                        new XElement(InfoFileRepresentationDict[SavedInfo.Name], Contact.Name),
                        new XElement(InfoFileRepresentationDict[SavedInfo.Ip], Contact.Ip),
                        new XElement(InfoFileRepresentationDict[SavedInfo.Id], Contact.Id)
                    )
                );
                doc.Save(Path);

                return true;
            }

            void IXmlFile.Remove()
            {
                var doc = XDocument.Load(Path);
                XElement contactNode = GetNode(doc);
                if (contactNode is not null)
                {
                    contactNode.Remove();
                    doc.Save(Path);
                }
            }

            void IXmlFile.Edit(SavedInfo attributeToChange, string newValue)
            {
                var doc = XDocument.Load(Path);
                XElement contactNode = GetNode(doc);

                if (contactNode is null)
                    throw new Exception("Contact was not found in the config file.");

                XElement element = contactNode.Element(InfoFileRepresentationDict[attributeToChange]);

                if (element is null)
                    throw new ArgumentException("The passed attribute to be changed was not found", nameof(attributeToChange));

                element.Value = newValue;
                doc.Save(Path);

            }

#nullable enable
            private static IEnumerable<XElement> GetContactNodes(XDocument? doc = null)
            {
                var document = doc ?? XDocument.Load(Path);
                return document.Root?.Elements() ?? Enumerable.Empty<XElement>();
            }

            private XElement GetNode(XDocument? doc = null)
            {
#pragma warning disable CS8603,CS8602 // Possible null reference return.
                return GetContactNodes(doc).FirstOrDefault(node => node.Element(InfoFileRepresentationDict[SavedInfo.Id]).Value == Contact.Id);
#pragma warning restore CS8603,CS8602 // Possible null reference return.
            }
#nullable restore

            public static IEnumerable<Contact> GetContacts()
            {
                foreach (var contactNode in GetContactNodes())
                {
                    string ip = contactNode.Element(InfoFileRepresentationDict[SavedInfo.Ip]).Value;
                    string id = contactNode.Element(InfoFileRepresentationDict[SavedInfo.Id]).Value;
                    string name = contactNode.Element(InfoFileRepresentationDict[SavedInfo.Name]).Value;

                    var contact = new Contact(ip) { _name = name, _save = false, _Id = id };

                    bool _ = TryGetProfilePicture(contact.ProfilePicPath, out Image profileImage);

                    contact.ProfilePic = profileImage;
                    contact._save = true;

                    yield return contact;
                }
            }
        }

        private void DeleteProfilePicture() => File.Delete(ProfilePicPath);

        private static bool TryGetProfilePicture(string path, out Image image)
        {
            try
            {
                using Stream imgStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Delete);
                using Image fromFile = Image.FromStream(imgStream);
                // Cloning because otherwise the program doesn't let of the image's handle for some reason.
                image = (Image)fromFile.Clone();

                return true;
            }
            catch (Exception e) when (e is FileNotFoundException || e is ArgumentException)
            {
                image = null;
                return false;
            }
        }

        private void SaveProfilePicture()
        {
            if (HasDefaultProfilePic)
            {
                System.Diagnostics.Debug.WriteLine("Has default profile pic, not saving.");
                return;
            }

            using Stream stream = new FileStream(ProfilePicPath, FileMode.Create, FileAccess.Write);
            ProfilePic.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        }

    }
}
