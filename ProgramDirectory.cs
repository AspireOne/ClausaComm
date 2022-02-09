using System;
using System.IO;
using System.Xml.Linq;

namespace ClausaComm
{
    public static class ProgramDirectory
    {
        public static readonly string FileSavePath =
            Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
        public static readonly string MainDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ClausaComm");
        public static readonly string ProfilePicsDirPath = GetPath("ProfilePictures");
        public static readonly string MessagesPath = GetPath("messages.xml");
        public static readonly string ConfigPath = GetPath("config.xml");
        public static readonly string ContactsPath = GetPath("contacts.xml");
        public const string XmlRoot = "doc";

        static ProgramDirectory()
        {
            if (!Directory.Exists(MainDirPath))
                CreateProgramDirectory();
        }

        private static void CreateProgramDirectory()
        {
            Directory.CreateDirectory(MainDirPath);
            Directory.CreateDirectory(ProfilePicsDirPath).Attributes = FileAttributes.Hidden | FileAttributes.NotContentIndexed;
            CreateNewXml(ConfigPath);
            CreateNewXml(ContactsPath);
            CreateNewXml(MessagesPath);
        }

        private static string GetPath(string filename) => Path.Combine(MainDirPath, filename);

        public static void CreateNewXml(string path) => new XDocument(new XElement(XmlRoot)).Save(path);
    }
}