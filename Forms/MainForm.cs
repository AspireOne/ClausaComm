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
using ClausaComm.Components;
using ClausaComm.Utils;
using System.Threading;

namespace ClausaComm.Forms
{
    public partial class MainForm : FormBase
    {

        public MainForm()
        {
            InitializeComponent();
            InitializeComponentFurther();
            InitializeProgram();
        }

        private void InitializeComponentFurther()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);

            ChatPanel1.ActionPanel = ActionPanel1;
            ChatPanel1.SendIcon = SendIcon1;
            ChatPanel1.Textbox = chatTextBox1;

            //ActionPanel1.RemoveContactAction = RemoveContact;
            Resizable = true;
            TitleBar.Form = this;
            TitleBar.BackColor = ChatPanel1.BackColor;

            AddContactIcon.Click += AddContactPictureBox_Click;
            ContactSearchBox.TextChanged += ContactSearchBox_TextChanged;
            this.BackColor = Color.FromArgb(40, 40, 40);
        }

        private void InitializeProgram()
        {
            bool userExists = false;
            foreach (Contact contact in Contact.XmlFile.GetContacts())
            {
                AddContact(contact);
                if (contact.IsUser)
                    userExists = true;
            }

            if (!userExists)
                AddContact(new Contact(IpUtils.LocalIp) { Save = true });

            PanelOfContactPanels.SimulateClickOnFirstPanel();
        }

        public void AddContact(Contact contact)
        {
            if (contact is null)
                throw new ArgumentNullException(nameof(contact));

            void clickActionIfUser() => ShowPopup(new EditInfoPopup(contact));
            void clickActionIfContact(Contact contact) => ChangeChatContact(contact);

            ContactPanel panel;

            if (contact.IsUser)
                panel = new ContactPanel(contact, OwnProfilePanel) { OnClickAction = clickActionIfUser };
            else
                panel = new ContactPanel(contact, PanelOfContactPanels) { OnClickActionContact = clickActionIfContact };

            PanelOfContactPanels.SimulateClickOnFirstPanel();
        }

        private void ContactSearchBox_TextChanged(object sender, EventArgs e)
        {
            PanelOfContactPanels.Panels.ForEach(panel => panel.Visible =
                string.IsNullOrWhiteSpace(ContactSearchBox.Text)
                || panel.Contact.Name
                    .Split(' ')
                    .Any(name => name.Contains(ContactSearchBox.Text, StringComparison.OrdinalIgnoreCase)));
        }

        private void AddContactPictureBox_Click(object sender, EventArgs e)
        {
            // Creating contacts for debugging purposes.
            AddContact(new Contact("92.16.1.71") { Name = "Mistr Yoda [Debug]" });
            AddContact(new Contact("19.168.0.52") { Name = "Descartes [Debug]" });
            AddContact(new Contact("14.118.8.13") { Name = "Socrates [Debug]" });
            AddContact(new Contact("132.18.4.94") { Name = "Kant [Debug]" });

            if (PanelOfContactPanels.Panels.Any())
            {
                PanelOfContactPanels.Panels.ElementAt(0).Contact.ProfilePic = Image.FromFile(@"C:\Users\matej\Desktop\d.png");
                PanelOfContactPanels.Panels.ElementAt(0).Contact.CurrentStatus = Contact.Status.Online;
                PanelOfContactPanels.Panels.ElementAt(0).Contact.Name = "Ej ej ejj";
            }

            ShowPopup(new AddContactPopup(AddContact));
        }

        private void ChangeControlsEnabled(bool enabled)
        {
            foreach (Control control in Controls)
            {
                if (!(control is TitleBar))
                    control.Enabled = enabled;
            }
        }

        private async void ShowPopup(Form popup)
        {
            ChangeControlsEnabled(false);
            await Task.Run(() => popup.ShowDialog());
            ChangeControlsEnabled(true);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (KeyPreview)
                e.Handled = true;
        }

        private void ChangeChatContact(Contact contact)
        {
            ChatPanel1.Contact = contact;
        }
        /*
        public void RemoveContact(Contact contact)
        {
            contact.Save = false;
            PanelOfContactPanels.RemovePanel(contact);

            if (!PanelOfContactPanels.Panels.Any())
                ChatPanel1.Contact = null;
            else
                PanelOfContactPanels.SimulateClickOnFirstPanel();
        }
        */
    }
}
