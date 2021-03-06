﻿using System;
using System.IO;
using System.Xml.Linq;

namespace ClausaComm
{
    public static class ProgramDirectory
    {
        public static readonly string DirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ClausaComm");
        public static readonly string ConfigPath = GetPath("config.xml");
        public static readonly string ContactsPath = GetPath("contacts.xml");
        public static readonly string MessagesPath = GetPath("messages.xml");
        public static readonly string ProfilePicsDirPath = GetPath("ProfilePictures");
        public static readonly string MessageFilesDirPath = GetPath("MessageFiles");
        public static readonly string XmlRoot = "doc";

        static ProgramDirectory()
        {
            if (!Directory.Exists(DirectoryPath))
                CreateProgramDirectory();
        }

        private static void CreateProgramDirectory()
        {
            Directory.CreateDirectory(DirectoryPath);
            Directory.CreateDirectory(ProfilePicsDirPath);
            Directory.CreateDirectory(MessageFilesDirPath).Attributes = FileAttributes.Directory |
                                                                        FileAttributes.Hidden |
                                                                        FileAttributes.NotContentIndexed;
            CreateNewXml(ConfigPath);
            CreateNewXml(MessagesPath);
            CreateNewXml(ContactsPath);
        }

        private static string GetPath(string filename) => Path.Combine(DirectoryPath, filename);

        private static void CreateNewXml(string path) => new XDocument(new XElement(XmlRoot)).Save(path);
    }
}