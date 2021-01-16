using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClausaComm.Extensions;
using ClausaComm.Components.ContactData;
using ClausaComm.Components;
using ClausaComm.Utils;

namespace ClausaComm.Forms
{
    public partial class MainForm : FormBase
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeComponentFurther();
            InitializeProgram();

            InWindowNotification.NotificationArgs notifArgs = new()
            {
                DurationMillis = 15000,
                MiddleButton = new InWindowNotification.NotificationArgs.ButtonArgs { ClickCallback = (_, _) => Debug.WriteLine("click"), Name = "Update now"},
                Title = "New update available",
                Text = $"Version xx is now available! Current version is xx.fdddddddddddddgdfgfdgfdgfdgdfgfdgfdgfgfdgdfgfdgfdgfdgfdgfgdgd fgfdg fdg dfg fdg fdg"
            };

            inWindowNotification1.ShowNotification(notifArgs);
        }

        private void InitializeComponentFurther()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);

            // ChatPanel
            ChatPanel1.ActionPanel = ActionPanel1;
            ChatPanel1.SendIcon = SendIcon1;
            ChatPanel1.Textbox = chatTextBox1;

            //TitleBar
            TitleBar.Form = this;
            TitleBar.BackColor = ChatPanel1.BackColor;
            TitleBar.PinNotifyIcon = new NotifyIcon
            {
                Text = "Click to open ClausaComm",
                Icon = Properties.Resources.program_icon,
                Visible = false
            };

            AddContactIcon.Click += AddContactPictureBox_Click;
            ContactSearchBox.TextChanged += ContactSearchBox_TextChanged;
            inWindowNotification1.Form = this;

            // This form
            Resizable = true;
            BackColor = Color.FromArgb(40, 40, 40);
            Pinnable = true;
        }

        private void InitializeProgram()
        {
            var userExists = false;
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

        public void AddContact(Contact contactToAdd)
        {
            if (contactToAdd is null)
                throw new ArgumentNullException(nameof(contactToAdd));

            void ClickActionIfUser() => ShowPopup(new EditInfoPopup(contactToAdd));
            void ClickActionIfContact(Contact contact) => ChangeChatContact(contact);

            if (contactToAdd.IsUser)
                _ = new ContactPanel(contactToAdd, OwnProfilePanel) { OnClickAction = ClickActionIfUser };
            else
                _ = new ContactPanel(contactToAdd, PanelOfContactPanels) { OnClickActionContact = ClickActionIfContact };

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
                PanelOfContactPanels.Panels.ElementAt(0).Contact.ProfilePic = Image.FromFile(@"C:\Users\matej\Desktop\profilovky a obrázky\santa_mitu.png");
                PanelOfContactPanels.Panels.ElementAt(0).Contact.CurrentStatus = Contact.Status.Online;
                PanelOfContactPanels.Panels.ElementAt(0).Contact.Name = "Ej ej ejj";
                PanelOfContactPanels.Panels.ElementAt(0).FlashPanel();
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
            await Task.Run(popup.ShowDialog);
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
