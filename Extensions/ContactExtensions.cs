using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm.Contacts
{
    public static class ContactExtensions
    {
        /*
        public static T GetNotOfflineContacts<T>(this T contacts) where T : IEnumerable<Contact>
        {
            return (T)contacts.Where(contact => contact.CurrentStatus != Contact.Status.Offline);
        }
        */

        public static IEnumerable<Contact> NotOffline(this HashSet<Contact> contacts)
        {
            return contacts.Where(contact => contact.CurrentStatus != Contact.Status.Offline);
        }
    }
}