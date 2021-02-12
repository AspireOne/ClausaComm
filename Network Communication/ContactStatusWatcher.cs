using ClausaComm.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// System.Timers was a conscious decision over System.Threading.Timer.
using System.Timers;

namespace ClausaComm.Network_Communication
{
    public class ContactStatusWatcher
    {
        // This list stores the IDs of all online contacts (copying the whole contacts would be expensive).
        // the ids (contacts) are gradually removed as this method receives a ping from them. After the 
        // timer interval runs off, the ids (contacts) that are left didn't send a ping and therefore
        // will be marked as offline. 
        // 
        // The list is re-created every time the timer fires.
        private HashSet<string> NoPing = new();
        private static readonly double CheckTime = TimeSpan.FromMinutes(2).TotalMilliseconds;
        private static readonly Timer CheckTimer = new(CheckTime);
        public static bool Running { get; private set; } = false;

        static ContactStatusWatcher()
        {
            // Just in case.
            if (CheckTime < PingSender.interval.TotalMilliseconds)
                throw new Exception("The interval for checking for a ping is smaller than the interval for sending a ping");
        }
        public ContactStatusWatcher()
        {
            CheckTimer.Elapsed += HandleTimerTick;
        }

        public void Start()
        {
            if (Running)
                return;

            CheckTimer.Start();
            Running = true;
        }

        private void HandleTimerTick(object obj, ElapsedEventArgs e)
        {
            // For each contact that was left (didn't send a ping) make his status offline.
            foreach (string contactId in NoPing)
            {
                Contact contact = MainForm.Form.Contacts.First(c => c.Id == contactId);
                contact.CurrentStatus = Contact.Status.Offline;
            }

            // Create a list of the ids of online contacts.
            NoPing = MainForm.Form.Contacts.Where(c => c.CurrentStatus != Contact.Status.Offline).Select(c => c.Id).ToHashSet();
        }

        public void HandlePingReceived(string contactId)
        {
            NoPing.Remove(contactId);
        }
    }
}
