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
using System.Text;
using System.Threading;
using ClausaComm.Contacts;
using ClausaComm.Messages;
using ClausaComm.Network_Communication;
using ClausaComm.Utils;

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
            UserStatusWatcher = new UserStatusWatcher(Contact.UserContact);
            UserStatusWatcher.Run();
            NetworkBridge = new NetworkBridge(Contacts, this);
            NetworkBridge.MessageReceived += OnMessageReceived;
            NetworkBridge.NewContactReceived += AddContact;
            NetworkBridge.Run();
        }

        private void InitializeComponentFurther()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            InitTitleBar(this);

            // ChatPanel
            ActionPanel1.MainForm = this;
            ChatScreen.ActionPanel = ActionPanel1;
            ChatScreen.SendIcon = SendIcon1;
            ChatScreen.Textbox = ChatTextBox1;

            AddContactIcon.Click += AddContactPictureBox_Click;
            ContactSearchBox.TextChanged += ContactSearchBox_TextChanged;
            NotificationPanel.Form = this;

            // This form
            Resizable = true;
            // Affects the borders.
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
            
            PanelOfContactPanels.SelectFirstPanel();
            ChatScreen.OnSendPressed += (message, contact) =>
            {
                if (NetworkBridge.SendMessage(message, contact.Ip))
                {
                    MessagesXml.SaveMessage(message, contact.Id);
                    ChatScreen.HandleMessageDelivered(contact, message);
                }
            };
        }

        private void OnMessageReceived(ChatMessage message, Contact contact)
        {
            ContactPanel panel = PanelOfContactPanels.Panels.First(panel => ReferenceEquals(panel.Contact, contact));
            if (!ReferenceEquals(ContactPanel.CurrentlySelectedPanel, panel))
                panel.Flash();

            MessagesXml.SaveMessage(message, contact.Id);
            
            if (ChatScreen.Contact != contact) 
                Sound.PlayNotificationSound();
            
            Invoke(() => ChatScreen.HandleMessageReceived(contact, message));
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

            PanelOfContactPanels.SelectFirstPanel();
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
            ShowPopup(new AddContactPopup(contact =>
            {
                AddContact(contact);
                NetworkBridge.Connect(contact);
            }, this));
        }

        private void ChangeControlsEnabled(bool enabled)
        {
            foreach (Control control in Controls)
                control.Enabled = enabled;
        }

        private void ShowPopup(Form popup)
        {
            //ChangeControlsEnabled(false);
            // A workaround for a probable bug in Control event signaling in WF.
            ThreadUtils.RunThread(() =>
            {
                Thread.Sleep(10);
                Invoke(popup.ShowDialog);
            });
            //ChangeControlsEnabled(true);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (KeyPreview)
                e.Handled = true;
        }

        private void ChangeChatContact(Contact contact)
        {
            ChatScreen.SetContact(contact);
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