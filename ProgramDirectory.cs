using System;
using System.IO;
using System.Xml.Linq;

namespace ClausaComm
{
    public static class ProgramDirectory
    {
        public static readonly string ThisPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ClausaComm");
        public static readonly string ProfilePicsDirPath = GetPathAndCreate("ProfilePictures");
        public static readonly string MessagesPath = GetPathAndCreate("messages.xml");
        public static readonly string ConfigPath = GetPathAndCreate("config.txt");
        public static readonly string ContactsPath = GetPathAndCreate("contacts.xml");

        static ProgramDirectory()
        {
            if (!Directory.Exists(ThisPath))
                CreateProgramDirectory();
        }

        private static void CreateProgramDirectory()
        {
            Directory.CreateDirectory(ThisPath).Attributes = FileAttributes.NotContentIndexed;
            Directory.CreateDirectory(ProfilePicsDirPath).Attributes = FileAttributes.Hidden | FileAttributes.NotContentIndexed;
        }

        private static string GetPathAndCreate(string filename)
        {
            string path = Path.Combine(ThisPath, filename);
            Utils.Utils.CreateIfDoesntExist(path);
            return path;
        }
    }
}