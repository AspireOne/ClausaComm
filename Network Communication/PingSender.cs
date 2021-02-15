using ClausaComm.Network_Communication.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ClausaComm.Extensions;
using ClausaComm.Exceptions;

namespace ClausaComm.Network_Communication
{
    public class PingSender
    {
        public static readonly TimeSpan interval = TimeSpan.FromSeconds(90);
        private static readonly Timer Timer = new(interval.TotalMilliseconds);
        private static readonly RemoteObject Ping = new(new Ping());
        private static bool Created;

        public bool Running { get; private set; }
        private readonly Action<string, RemoteObject> SendMethod;
        private readonly HashSet<Contact> AllContacts;

        public PingSender(Action<string, RemoteObject> sendMethod, HashSet<Contact> allContacts)
        {
            if (Created)
                throw new MultipleInstancesException(nameof(PingSender));

            Timer.Elapsed += OnIntervalPassed;
            AllContacts = allContacts;
            SendMethod = sendMethod;
            Created = true;
        }

        private void OnIntervalPassed(object o, ElapsedEventArgs e)
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

            Timer.Start();
            Running = true;
        }
    }
}