using System;
using System.Windows.Forms;
using ClausaComm.Contacts;
using ClausaComm.Messages;

namespace ClausaComm.Components
{
    public partial class ChatMessagePanel : UserControl
    {
        // TODO: Change the color of the message to gray or something based on this.
        public bool Delivered { get; set; }
        public readonly Contact Contact;
        public readonly ChatMessage Message;
        public ChatMessagePanel()
        {
            InitializeComponent();
        }
        
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent is null)
                return;
            
            BackColor = Parent.BackColor;
            Parent.BackColorChanged += (_, ev) => this.BackColor = Parent.BackColor;
        }

        public ChatMessagePanel(ChatMessage message, Contact contact)
        {
            InitializeComponent();
            DateTimeOffset date = DateTimeOffset.FromUnixTimeMilliseconds(message.Time);
            ChatMessageName.Text = contact.Name;
            ChatMessageTime.Text = date.ToString();
            ChatMessageText.Text = message.Text;
            lock (contact.ProfilePic)
                ChatMessagePicture.Image = contact.ProfilePic;

            Contact = contact;
            Message = message;
        }
    }
}