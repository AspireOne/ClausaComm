using System;
using System.Windows.Forms;
using ClausaComm.Contacts;
using ClausaComm.Messages;

namespace ClausaComm.Components
{
    public partial class ChatMessagePanel : UserControl
    {
        public ChatMessagePanel()
        {
            InitializeComponent();
        }
        
        public ChatMessagePanel(ChatMessage message, Contact contact)
        {
            InitializeComponent();
            DateTimeOffset date = DateTimeOffset.FromUnixTimeMilliseconds(message.Time);
            this.ChatMessageHeaderText.Text = contact.Name + " " + message.Time + " | " + date.LocalDateTime;
        }
    }
}