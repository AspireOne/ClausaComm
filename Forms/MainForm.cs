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
using System.Threading.Tasks;
using ClausaComm.Components.Icons;
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
        private readonly bool StartPinned;

        public MainForm(bool startPinned)
        {
            InitializeComponent();
            InitializeComponentFurther();
            InitializeProgram();
            int x = Screen.PrimaryScreen.Bounds.Width / 2 - Width / 2;  
            int y = Screen.PrimaryScreen.Bounds.Height / 2 - Height / 2;  
            Location = new Point(x, y);
            
            StartPinned = startPinned;
            UserStatusWatcher = new UserStatusWatcher(Contact.UserContact);
            UserStatusWatcher.Run();
            NetworkBridge = new NetworkBridge(Contacts, this);
            NetworkBridge.MessageReceived += OnMessageReceived;
            NetworkBridge.NewContactReceived += AddContact;
            NetworkBridge.Run();
        }

        protected override void OnShown(EventArgs e)
        {
            Pinned = StartPinned;
            base.OnShown(e);
        }

        private void InitializeComponentFurther()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            
            InitTitleBar(this);
            var settingsIcon = new SettingsIcon();
            settingsIcon.Click += (_, _) => new SettingsPopup(this).ShowDialog();
            TitleBar.AddAdditionalElement(settingsIcon);
            TitleBar.Tooltip.SetToolTip(settingsIcon, "Settings");

            // ChatPanel
            ActionPanel1.MainForm = this;
            ChatScreen.ActionPanel = ActionPanel1;
            ChatScreen.SendIcon = SendIcon1;
            ChatScreen.FileSelectorIcon = FileSelectorIcon1;
            ChatScreen.Textbox = ChatTextBox1;
            
            ToolTip1.SetToolTip(FileSelectorIcon1, "Send a file");

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
                PanelOfContactPanels.MovePanelToTop(contact);
                
                Task.Run(() =>
                {
                    if (!NetworkBridge.SendMessage(message, contact.Ip))
                        return;
                    
                    MessagesXml.SaveMessage(message, contact.Id);
                    ChatScreen.HandleMessageDelivered(contact, message);
                });
            };
        }

        private void OnMessageReceived(ChatMessage message, Contact contact)
        {
            ContactPanel panel = PanelOfContactPanels.Panels.First(panel => ReferenceEquals(panel.Contact, contact));
            if (!ReferenceEquals(ContactPanel.CurrentlySelectedPanel, panel))
                panel.Flash();
            
            PanelOfContactPanels.MovePanelToTop(panel);
            MessagesXml.SaveMessage(message, contact.Id);
            
            if (ActiveForm != this)
                Sound.PlayNotificationSound();
            
            Invoke(() => ChatScreen.HandleMessageReceived(contact, message));
        }

        private void AddContact(Contact contactToAdd)
        {
            if (contactToAdd is null)
                throw new ArgumentNullException(nameof(contactToAdd));
            
            Contacts.Add(contactToAdd);

            void ClickActionIfUser(Contact contact)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(10);
                    Invoke(() => new EditInfoPopup(contactToAdd, this).ShowDialog());
                });
            }
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
            new AddContactPopup(AddContact, this, NetworkBridge.Connect).ShowDialog();
        }

        private void ChangeControlsEnabled(bool enabled)
        {
            foreach (Control control in Controls)
                control.Enabled = enabled;
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