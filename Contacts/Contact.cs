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

        #region backing fields

        private Status _status = Status.Offline;
        private Image _profileImage = Resources.default_pfp;
        private string _name = "Unknown";
        private string _ip;
        private bool _save;
        private static Contact _userContact;
        private string? _id;

        #endregion backing fields

        #region Properties

        public static Contact UserContact => _userContact ??=
            XmlFile.Contacts.FirstOrDefault(contact => contact.IsUser)
            ?? new Contact(IpUtils.LocalIp) { Id = IdGenerator.GenerateId(8), IsUser = true, Save = true };

        // TODO: Rework the whole IsUser thing (save just UserId: id and remove he IsUser property)
        public bool IsUser { get; private init; }
        public string ProfilePicPath => Path.Combine(ProgramDirectory.ProfilePicsDirPath, $"{Id}.png");
        private bool HasDefaultProfilePic { get; set; } = true;

        public string? Id
        {
            get => _id;
            set
            {
                if (value == _id)
                    return;

                if (_id is not null)
                    throw new("Id cannot be reassigned.");

                if (Save)
                    Xml.Edit(XmlSavedInfo.Id, value);

                _id = value;
            }
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
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value), "The supplied IP was null.");

                if (!IpUtils.IsIpCorrect(value))
                    throw new InvalidIpException($"The supplied IP ({value}) was incorrect.");

                _ip = value;
            }
        }

        public Image ProfilePic
        {
            get => _profileImage;
            set
            {
                if (value is null || ImageUtils.AreImagesSame(ProfilePic, value))
                    return;

                _profileImage = value;
                HasDefaultProfilePic = false;
                if (Save)
                    SaveProfilePicture();
                
                /*if (value is null || value.Equals(Resources.default_pfp) || ReferenceEquals(value, Resources.default_pfp))
                {
                    _profileImage = Resources.default_pfp;
                    HasDefaultProfilePic = true;
                    if (Save)
                        DeleteProfilePicture();
                }*/

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
                    Xml.Save(out bool alreadyExists);

                    if (alreadyExists)
                        throw new("One instance of this contact is already saved.");

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

        public override int GetHashCode() => int.Parse(string.Concat(Ip.Where(c => c != '.').Skip(3)));

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
            catch (Exception e) when (e is FileNotFoundException or ArgumentException)
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