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
using System.Collections.Generic;

namespace ClausaComm.Forms
{
    public partial class MainForm : FormBase
    {
        // Expose an instance of MainForm as static, because there will ever be only one instance of MainForm; MainForm thus acts like a static object,
        // and so it SHOULD be static in order for the logic (literal logic) to work properly. We cannot make it static directly, so we'll do it this way.
        public static MainForm Form;
        public readonly HashSet<Contact> Contacts = new();
        public MainForm()
        {
            Form = this;
            InitializeComponent();
            InitializeComponentFurther();
            InitializeProgram();
        }

        private void InitializeComponentFurther()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);

            // ChatPanel
            ActionPanel1.MainForm = this;
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
            NotificationPanel.Form = this;

            // This form
            Resizable = true;
            BackColor = TitleBar.BackColor;
            Pinnable = true;
            Padding = DraggableWindowBorderSize;
        }

        private void InitializeProgram()
        {
            Contact.XmlFile.Contacts.ForEach(c => AddContact(c));
            PanelOfContactPanels.SimulateClickOnFirstPanel();
        }

        public void AddContact(Contact contactToAdd)
        {
            Contacts.Add(contactToAdd);

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
                PanelOfContactPanels.Panels.ElementAt(0).Contact.ProfilePic = Image.FromFile(@"C:\Users\matej\Desktop\Desktop1\profilovky a obrázky\santa_mitu.png");
                PanelOfContactPanels.Panels.ElementAt(0).Contact.CurrentStatus = Contact.Status.Online;
                PanelOfContactPanels.Panels.ElementAt(0).Contact.Name = "Ej ej ejj";
                PanelOfContactPanels.Panels.ElementAt(0).FlashPanel();
            }

            ShowPopup(new AddContactPopup(AddContact));
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
