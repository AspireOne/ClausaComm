using System;
using System.Drawing;
using System.IO;
using System.Linq;
using ClausaComm.Properties;
using ClausaComm.Utils;

namespace ClausaComm.Contacts
{
    public partial class Contact
    {
        // Event handlers.
        public event EventHandler<Status> StatusChange;

        public event EventHandler<string> NameChange;

        public event EventHandler<Image> ProfilePicChange;

        // Status declaration.
        public enum Status { Online, Idle, Offline }

        public static readonly (int min, int max) NameLength = new(3, 25);

        //public readonly HashSet<Contact> UnreadMessages = new HashSet<Contact>();

        // TODO: Move this enum (SavedInfo) to a more appropriate place.

        private enum XmlSavedInfo { Id, Name, Ip, IsUser }

        #region backing fields

        private Status _status = Status.Offline;
        private Image _profileImage = Resources.default_pfp;
        private string _name = "Unknown";
        private readonly string _ip;
        private bool _save = false;
        private string _profilePicPath;
        private static Contact _userContact;

        #endregion backing fields

        #region Properties

        public static Contact UserContact => _userContact ??= XmlFile.Contacts.FirstOrDefault(contact => contact.IsUser) ?? new Contact(IpUtils.LocalIp) { Save = true };
        public bool IsUser { get; private init; }
        public string ProfilePicPath => _profilePicPath ??= Path.Combine(ProgramDirectory.ProfilePicsDirPath, $"{Id}.png");
        private bool HasDefaultProfilePic { get; set; } = true;

        public string Id { get; set; }

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
                    IsUser = IpUtils.LocalIp == value;
                }
            }
        }

        public Image ProfilePic
        {
            get => _profileImage;
            set
            {
                if (ProfilePic.Equals(value) || ReferenceEquals(value, ProfilePic))
                    return;

                if (value is null || value.Equals(Resources.default_pfp) || ReferenceEquals(value, Resources.default_pfp))
                {
                    _profileImage = Resources.default_pfp;
                    HasDefaultProfilePic = true;
                    if (Save)
                        DeleteProfilePicture();
                }
                else
                {
                    _profileImage = value;
                    HasDefaultProfilePic = false;
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

                if (value.Length > NameLength.max)
                    value = value.Substring(0, NameLength.max);
                else if (value.Length < NameLength.min)
                    for (int i = 0; i < NameLength.min - value.Length; ++i)
                        value += "_";
                _name = value;

                /*
                _name = value.Length is <= NameLength.max and >= NameLength.min
                    ? value
                    : throw new ArgumentException($@"The supplied name's length is not right.
                       Should be <= {NameLength.max} and >= {NameLength.min}, but was {value.Length})", nameof(value));
                */
                NameChange?.Invoke(this, value);

                if (Save)
                    Xml.Edit(XmlSavedInfo.Name, value);
            }
        }

        public bool Save
        {
            get => _save;
            set
            {
                if (Save == value)
                    return;

                if ((_save = value))
                {
                    bool exists = !Xml.Save();

                    if (exists)
                        throw new Exception("One instance of this contact is already saved.");

                    if (!HasDefaultProfilePic)
                        SaveProfilePicture();
                }
                else
                {
                    Xml.Remove();
                    DeleteProfilePicture();
                }
            }
        }

        #endregion Properties

        // Note: the contact doesn't have to have an ID! The user can add the contact via IP, and at that point,
        // the contact's data (including ID) are not initialized yet.
        public Contact(string ip)
        {
            Ip = ip ?? throw new ArgumentNullException(nameof(ip), "Ip of a contact must not be null");
            Xml = new XmlFile(this);
        }

        public override bool Equals(object obj) => obj is Contact contact && contact.Id == Id && contact.Ip == Ip;

        public override string ToString() => $"Name: {Name} | ID: {Id} | IsUser: {IsUser} | Save: {Save} | IP: {Ip}";

        public override int GetHashCode() => int.Parse(Ip.Replace(".", ""));

        private void DeleteProfilePicture() => File.Delete(ProfilePicPath);

        private static void TryGetProfilePicture(string path, out Image image)
        {
            try
            {
                using Stream imgStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Delete);
                using var fromFile = Image.FromStream(imgStream);
                // Cloning because otherwise the program doesn't let of the image's handle for some reason.
                image = (Image)fromFile.Clone();
            }
            catch (Exception e) when (e is FileNotFoundException || e is ArgumentException)
            {
                image = null;
            }
        }

        private void SaveProfilePicture()
        {
            if (HasDefaultProfilePic)
                return;

            using Stream stream = new FileStream(ProfilePicPath, FileMode.Create, FileAccess.Write);
            ProfilePic.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}