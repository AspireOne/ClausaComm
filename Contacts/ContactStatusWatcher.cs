using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ClausaComm.Exceptions;
using ClausaComm.Extensions;
using ClausaComm.Network_Communication;

namespace ClausaComm.Contacts
{
    public class ContactStatusWatcher
    {
        // This list stores the IDs of all online contacts (copying the whole contacts would be expensive).
        // the ids (contacts) are gradually removed as this method receives a ping from them. After the
        // timer interval runs off, the ids (contacts) that are left didn't send a ping and therefore
        // will be marked as offline.
        //
        // The list is re-created every time the timer fires.
        public static readonly int CheckTimeMillis = (int)TimeSpan.FromMinutes(2).TotalMilliseconds;

        private readonly Timer CheckTimer;

        private HashSet<string> Contacts = new();
        public bool Running { get; private set; }
        private static bool Created;
        private readonly HashSet<Contact> AllContacts;

        static ContactStatusWatcher()
        {
            // Just in case.
            if (CheckTimeMillis < PingSender.FrequencyMillis)
                throw new Exception("The interval for checking for a ping is smaller than the interval for sending a ping");
        }

        public ContactStatusWatcher(HashSet<Contact> allContacts)
        {
            if (Created)
                throw new MultipleInstancesException(nameof(ContactStatusWatcher));

            CheckTimer = new(HandleTimerTick, null, Timeout.Infinite, CheckTimeMillis);
            AllContacts = allContacts;
            Created = true;
        }

        public void Run()
        {
            if (Running)
                return;

            if (!CheckTimer.Change(0, CheckTimeMillis))
                throw new Exception($"The Timer in class {nameof(ContactStatusWatcher)} threw an error when being updated.");

            Running = true;
        }

        private void HandleTimerTick(object obj)
        {
            // For each contact that was left (didn't send a ping) make his status offline.
            foreach (string contactIdentifier in Contacts)
            {
                Contact contact = AllContacts.First(c => (c.Id ?? c.Ip) == contactIdentifier);
                contact.CurrentStatus = Contact.Status.Offline;
                Debug.WriteLine($"[ContactStatusWatcher] {contact.Name} went offline.");
            }

            // Create a list of the ids of not-offline offline contacts.
            Contacts = AllContacts.NotOffline().Select(c => c.Id ?? c.Ip).ToHashSet();
            Debug.WriteLine($"[ContactStatusWatcher] Amount of online contacts: {Contacts.Count}");
            Debug.WriteLine($"[ContactStatusWatcher] Amount of offline contacts: {AllContacts.Count - Contacts.Count}");
        }

        public void HandleActivityReceived(Contact contact)
        {
            Contacts.Remove(contact.Id ?? contact.Ip);

            if (contact.CurrentStatus == Contact.Status.Offline)
                contact.CurrentStatus = Contact.Status.Online;
            
            Debug.WriteLine($"[ContactStatusWatcher] received activity from {contact.Name}");
            }
    }
}