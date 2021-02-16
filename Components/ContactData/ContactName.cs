using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ClausaComm.Contacts;

namespace ClausaComm.Components.ContactData
{
    public sealed partial class ContactName : Label, IContactUsable
    {
        private Contact _contact;
        public Contact Contact
        {
            get => _contact;
            set
            {
                if (_contact is not null)
                    _contact.NameChange -= NameChangeHandler;

                if ((_contact = value) is not null)
                {
                    value.NameChange += NameChangeHandler;
                    NameChangeHandler(null, value.Name);
                }
            }
        }



        public ContactName()
        {
            InitializeComponent();
            Name = "Name";
            AutoSize = false;
            TextAlign = ContentAlignment.MiddleLeft;
            BorderStyle = BorderStyle.None;
            Font = new Font("Segoe UI", 13, FontStyle.Regular);
        }

        public ContactName(IContainer container) : this() => container.Add(this);

        public ContactName(Contact contact) : this() => Contact = contact;

        private void NameChangeHandler(object _, string name)
        {
            Text = name;
            Invalidate();
        }
    }
}
