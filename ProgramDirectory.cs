using System;
using System.IO;
using System.Xml.Linq;

namespace ClausaComm
{
    public static class ProgramDirectory
    {
        public static readonly string ThisPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ClausaComm");
        public static readonly string ProfilePicsDirPath = GetPathAndCreateFile("ProfilePictures");
        public static readonly string MessagesPath = GetPathAndCreateFile("messages.xml");
        public static readonly string ConfigPath = GetPathAndCreateFile("config.xml");
        public static readonly string ContactsPath = GetPathAndCreateFile("contacts.xml");
        public const string XmlRoot = "doc";

        static ProgramDirectory()
        {
            if (!Directory.Exists(ThisPath))
                CreateProgramDirectory();
        }

        private static void CreateProgramDirectory()
        {
            Directory.CreateDirectory(ThisPath);
            Directory.CreateDirectory(ProfilePicsDirPath).Attributes = FileAttributes.Hidden | FileAttributes.NotContentIndexed;
            CreateNewXml(ConfigPath);
            CreateNewXml(ContactsPath);
            CreateNewXml(MessagesPath);
        }

        private static string GetPathAndCreateFile(string filename)
        {
            string path = Path.Combine(ThisPath, filename);
            bool isDirectory = !Path.GetFileName(path).Contains('.');
            
            if (isDirectory && !Directory.Exists(path))
                Directory.CreateDirectory(path);
            else if (!isDirectory && !File.Exists(path) && path.EndsWith("xml"))
                CreateNewXml(path);

            return path;
        }

        private static void CreateNewXml(string path) => new XDocument(new XElement(XmlRoot)).Save(path);
    }
}