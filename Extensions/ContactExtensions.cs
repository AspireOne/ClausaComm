using System.Collections.Generic;
using System.Linq;
using ClausaComm.Contacts;

namespace ClausaComm.Extensions
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