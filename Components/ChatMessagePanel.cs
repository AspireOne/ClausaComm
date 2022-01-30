using System;
using System.Drawing;
using System.Windows.Forms;
using ClausaComm.Contacts;
using ClausaComm.Messages;
using ClausaComm.Utils;

namespace ClausaComm.Components
{
    public partial class ChatMessagePanel : UserControl
    {
        public bool Delivered { get; private set; }
        public readonly Contact Contact;
        public readonly ChatMessage Message;

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent is null)
                return;
            
            BackColor = Parent.BackColor;
            Parent.BackColorChanged += (_, _) => BackColor = Parent.BackColor;
        }

        public void MarkDelivered()
        {
            Delivered = true;
            ChatMessageText.ForeColor = Constants.UiConstants.ChatTextColor;
        }

        public ChatMessagePanel(ChatMessage message, Contact contact)
        {
            InitializeComponent();
            // Adding 3_600_000 milliseconds because the time is one hour late for some reason.
            DateTimeOffset date = DateTimeOffset.FromUnixTimeMilliseconds(message.Time + 3_600_000);
            ChatMessageName.Text = contact.Name;
            ChatMessageTime.Text = date.ToString("dd/MM/yyyy HH:mm");
            ChatMessageText.Text = message.Text;

            lock (contact.ProfilePic)
                ChatMessagePicture.Image = contact.ProfilePic;

            contact.NameChange += (_, name) => ChatMessageName.Text = name;
            contact.ProfilePicChange += (_, pic) =>
            {
                lock (contact.ProfilePic)
                    ChatMessagePicture.Image = pic;
            };

            Contact = contact;
            Message = message;
        }
    }
}