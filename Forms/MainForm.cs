using ClausaComm.Forms;
using ClausaComm.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Resources;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClausaComm.Extensions;
using ClausaComm.Components.Icons;

namespace ClausaComm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeComponentFurther();
            InitializeProgram();
        }

        private void InitializeComponentFurther()
        {
            ChatPanel1.ActionPanel = ActionPanel1;
            ChatPanel1.SendIcon = SendIcon1;
            ChatPanel1.Textbox = chatTextBox1;
            ActionPanel1.RemoveContactAction = RemoveContact;
        }

        private void InitializeProgram()
        {
            foreach (Contact contact in Contact.XmlFile.GetContacts())
                AddContact(contact);

            PanelOfContactPanels.SimulateClickOnFirstPanel();
        }

        public void AddContact(Contact contact)
        {
            Action clickActionIfUser = new(() => { new EditInfoPopup(contact).ShowDialog(); });
            Action<Contact> clickActionIfContact = new((Contact contact) => { ChangeChatContact(contact); });

            ContactPanel panel;

            if (contact.IsUser)
                panel = new ContactPanel(contact, OwnProfilePanel) { OnClickAction = clickActionIfUser };
            else
                panel = new ContactPanel(contact, PanelOfContactPanels) { OnClickActionContact = clickActionIfContact };
        }

        private void ContactSearchBox_TextChanged(object sender, EventArgs e)
        {
            PanelOfContactPanels.Panels.ForEach(panel => panel.Visible =
                string.IsNullOrWhiteSpace(ContactSearchBox.Text) ||
                panel.Contact.Name.Split(' ').Any(name => name.Contains(ContactSearchBox.Text, StringComparison.OrdinalIgnoreCase)));
        }

        private void AddContactPictureBox_Click(object sender, EventArgs e)
        {
            // Creating contacts for debugging purposes.
            AddContact(new Contact("92.16.1.71") { Name = "Mistr Yoda" });
            AddContact(new Contact("19.168.0.52") { Name = "Descartes" });
            AddContact(new Contact("14.118.8.13") { Name = "Socrates" });
            AddContact(new Contact("132.18.4.94") { Name = "Kant" });

            if (PanelOfContactPanels.Panels.Any())
            {
                PanelOfContactPanels.Panels.ElementAt(0).Contact.ProfilePic = Image.FromFile(@"C:\Users\matej\Desktop\a.jpg");
                PanelOfContactPanels.Panels.ElementAt(0).Contact.CurrentStatus = Contact.Status.Online;
                PanelOfContactPanels.Panels.ElementAt(0).Contact.Name = "Ej ej ejj";
            }

            new AddContactPopup(AddContact).ShowDialog();
        }

        private void ChangeChatContact(Contact contact)
        {
            ChatPanel1.Contact = contact;
        }

        public void RemoveContact(Contact contact)
        {
            contact.Save = false;
            PanelOfContactPanels.RemovePanel(contact);
            PanelOfContactPanels.SimulateClickOnFirstPanel();
        }
    }
}
