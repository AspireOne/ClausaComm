using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClausaComm.Extensions;
using ClausaComm.Components.ContactData;
using ClausaComm.Components;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using ClausaComm.Contacts;
using ClausaComm.Network_Communication;

namespace ClausaComm.Forms
{
    public partial class MainForm : FormBase
    {
        public readonly HashSet<Contact> Contacts = new();
        private readonly NetworkBridge NetworkBridge;
        private readonly UserStatusWatcher UserStatusWatcher;

        public MainForm()
        {
            InitializeComponent();
            InitializeComponentFurther();
            InitializeProgram();
            UserStatusWatcher = new(Contact.UserContact);
            UserStatusWatcher.Run();
            NetworkBridge = new(Contacts, AddContact);
            NetworkBridge.Run();
        }

        private void InitializeComponentFurther()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);

            // ChatPanel
            ActionPanel1.MainForm = this;
            ChatPanel.ActionPanel = ActionPanel1;
            ChatPanel.SendIcon = SendIcon1;
            ChatPanel.Textbox = chatTextBox1;

            //TitleBar
            TitleBar.Form = this;
            TitleBar.BackColor = ChatPanel.BackColor;
            TitleBar.PinNotifyIcon = new NotifyIcon
            {
                Text = "Click to open ClausaComm",
                Icon = Properties.Resources.program_icon,
                Visible = false
            };

            AddContactIcon.Click += AddContactPictureBox_Click;
            ContactSearchBox.TextChanged += ContactSearchBox_TextChanged;
            NotificationPanel.Form = this;

            // This form
            Resizable = true;
            BackColor = TitleBar.BackColor;
            Pinnable = true;
            Padding = DraggableWindowBorderSize;
        }

        private void InitializeProgram()
        {
            bool userAdded = false;
            Contact.XmlFile.Contacts.ForEach(contact =>
            {
                AddContact(contact);
                if (contact.IsUser)
                    userAdded = true;
            });
            
            // For the first startup.
            if (!userAdded)
                AddContact(Contact.UserContact);
            
            PanelOfContactPanels.SimulateClickOnFirstPanel();
            ChatPanel.OnSendPressed += message =>
            {
                // TODO: Save it to xml.
                NetworkBridge.SendMessage(message, IPAddress.Parse(ChatPanel.Contact.Ip));
            };
        }

        private void AddContact(Contact contactToAdd)
        {
            Contacts.Add(contactToAdd);

            if (contactToAdd is null)
                throw new ArgumentNullException(nameof(contactToAdd));

            void ClickActionIfUser(Contact contact) => ShowPopup(new EditInfoPopup(contactToAdd));
            void ClickActionIfContact(Contact contact) => ChangeChatContact(contact);
            
            _ = new ContactPanel(contactToAdd, PanelOfContactPanels) { OnClickAction = ClickActionIfContact };
    
            if (contactToAdd.IsUser)
                _ = new ContactPanel(contactToAdd, OwnProfilePanel, true) { OnClickAction = ClickActionIfUser };

            PanelOfContactPanels.SimulateClickOnFirstPanel();
        }

        private void ContactSearchBox_TextChanged(object sender, EventArgs e)
        {
            PanelOfContactPanels.Panels.ForEach(panel => panel.Visible =
                string.IsNullOrWhiteSpace(ContactSearchBox.Text)
                || panel.Contact.Name.Split(' ')
                    .Any(name => name.Contains(ContactSearchBox.Text, StringComparison.OrdinalIgnoreCase)));
        }

        private void AddContactPictureBox_Click(object sender, EventArgs e)
        {
#if DEBUG
            // Creating contacts for debugging purposes.

            if (PanelOfContactPanels.Panels.Count() < 2)
            {
                AddContact(new("92.16.1.71") { Name = "Mistr Yoda [Debug]", Id = "d18ss7f8" });
                AddContact(new("19.168.0.52") { Name = "Descartes [Debug]", Id = "PALDNGHV" });
                AddContact(new("14.118.8.13") { Name = "Socrates [Debug]", Id = "LCMIDNC" });
                AddContact(new("132.18.4.94") { Name = "Kant [Debug]", Id = "DFSDGDsq" });
            }

            // Personalizing the first debug contact for testing purposes.

            if (PanelOfContactPanels.Panels.Any())
            {
                PanelOfContactPanels.Panels.ElementAt(0).Contact.ProfilePic = Image.FromFile(@"C:\Users\matej\Pictures\profilovky a obrázky\mitu192.png");
                PanelOfContactPanels.Panels.ElementAt(0).Contact.CurrentStatus = Contact.Status.Online;
                PanelOfContactPanels.Panels.ElementAt(0).Contact.Name = "Ej ej ejj";
                PanelOfContactPanels.Panels.ElementAt(0).FlashPanel();
            }
#endif

            ShowPopup(new AddContactPopup(contact =>
            {
                AddContact(contact);
                NetworkBridge.Connect(contact);
            }, this));
        }

        private void ChangeControlsEnabled(bool enabled)
        {
            foreach (Control control in Controls)
                if (!(control is TitleBar))
                    control.Enabled = enabled;
        }

        private void ShowPopup(Form popup)
        {
            ChangeControlsEnabled(false);
            popup.ShowDialog();
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
            ChatPanel.Contact = contact;
        }

        private void ChatPanel1_Paint(object sender, PaintEventArgs e)
        {
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