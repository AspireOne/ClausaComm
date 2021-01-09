﻿using System;
using System.Collections.Generic;

namespace ClausaComm
{
    public class Message
    {
        public enum Ways { In, Out }
        public const int MaxCharacters = 20_000;
        public const int MaxFileSizeBytes = 1_047_527_424; //999 mb
        private readonly XmlFile Config;
        public readonly string Text;
        public readonly string SenderIp;
        public readonly string ReceiverIp;
        public readonly FileObj File;

        private bool _save = false;
        public bool Save
        {
            get => _save;
            set
            {
                if (value == Save)
                    return;

                if (_save = value)
                    Config.Save();
                else
                    Config.Delete();
            }
        }

        public DateTime TimeSent { get; set; } = DateTime.Now;



        public Message(string senderIp, string receiverIp, string text) : this(senderIp, receiverIp) => Text = text;
        public Message(string senderIp, string receiverIp, FileObj file) : this(senderIp, receiverIp) => File = file;
        private Message(string senderIp, string receiverIp)
        {
            SenderIp = senderIp;
            ReceiverIp = receiverIp;
            Config = new(this);
        }



        private class XmlFile
        {
            public enum SavedInfo { TimeSent, File, Text }
            private static readonly string XmlPath = ProgramDirectory.MessagesPath;
            Message Msg;

            public XmlFile(Message message)
            {
                Msg = message;
            }

            public void Save()
            {

            }

            public IEnumerable<Message> GetByContact(Contact contact)
            {
                return null;
            }

            public void Edit()
            {

            }

            public void Delete()
            {

            }
        }
    }
}
