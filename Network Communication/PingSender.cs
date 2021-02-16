﻿using ClausaComm.Network_Communication.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ClausaComm.Extensions;
using ClausaComm.Exceptions;

namespace ClausaComm.Network_Communication
{
    public class PingSender
    {
        public static readonly int FrequencyMillis = (int)TimeSpan.FromSeconds(90).TotalMilliseconds;
        private readonly Timer Timer;
        private static readonly RemoteObject Ping = new(new Ping());
        private static bool Created;

        public bool Running { get; private set; }
        private readonly Action<string, RemoteObject> SendMethod;
        private readonly HashSet<Contact> AllContacts;

        public PingSender(Action<string, RemoteObject> sendMethod, HashSet<Contact> allContacts)
        {
            if (Created)
                throw new MultipleInstancesException(nameof(PingSender));

            Timer = new(OnIntervalPassed, null, Timeout.Infinite, FrequencyMillis);
            AllContacts = allContacts;
            SendMethod = sendMethod;
            Created = true;
        }

        private void OnIntervalPassed(object o)
        {
            //AllContacts.Where(c => c.CurrentStatus != Contact.Status.Offline).ForEach(c => SendMethod.Invoke(c.Ip, Ping));
            foreach (Contact contact in AllContacts)
            {
                if (contact.CurrentStatus != Contact.Status.Offline)
                    SendMethod.Invoke(contact.Ip, Ping);
            }
        }

        // Do not mark as static.
        public void Start()
        {
            if (Running)
                return;

            if (!Timer.Change(0, FrequencyMillis))
                throw new Exception($"The Timer in class {nameof(PingSender)} threw an error when being updated.");

            Running = true;
        }
    }
}