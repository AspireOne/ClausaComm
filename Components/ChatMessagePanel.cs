using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ClausaComm.Contacts;
using ClausaComm.Messages;
using ClausaComm.Utils;

namespace ClausaComm.Components
{
    public partial class ChatMessagePanel : UserControl
    {
        public readonly Contact Contact;
        public readonly ChatMessage Message;
        private readonly bool IsLink;

        public ChatMessagePanel(ChatMessage message, Contact contact)
        {
            InitializeComponent();
            // Adding 3_600_000 milliseconds because the time is one hour late for some reason.
            DateTimeOffset date = DateTimeOffset.FromUnixTimeMilliseconds(message.Time + 3_600_000);
            ChatMessageName.Text = contact.Name;
            ChatMessageTime.Text = date.ToString("dd/MM/yyyy HH:mm");
            ChatMessageText.Text = message.Text;
            
            if (IsLink = !string.IsNullOrEmpty(message.FilePath))
            {
                ChatMessageText.Text = message.FilePath;
                ChatMessageText.Click += (_, _) => OpenDirAndSelectFile(message.FilePath);
                ChatMessageText.Cursor = Cursors.Hand;
            }

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
            SetDelivered(message.Way == ChatMessage.Ways.In || message.Delivered);
            if (message.Way == ChatMessage.Ways.Out)
                message.DeliveredChanged += (_, delivered) => SetDelivered(delivered);
        }
        
        private void SetDelivered(bool delivered)
        {
            ChatMessageText.ForeColor = delivered
                ? (IsLink ? Constants.UiConstants.ChatTextColorLink : Constants.UiConstants.ChatTextColor)
                : (IsLink ? Constants.UiConstants.ChatTextColorLinkUndelivered : Constants.UiConstants.ChatTextColorUndelivered);
        }
        
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent is null)
                return;
            
            BackColor = Parent.BackColor;
            Parent.BackColorChanged += (_, _) => BackColor = Parent.BackColor;
        }

        public static void OpenDirAndSelectFile(string filePath)
        {
            IntPtr pidl = ILCreateFromPathW(filePath);
            SHOpenFolderAndSelectItems(pidl, 0, IntPtr.Zero, 0);
            ILFree(pidl);
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr ILCreateFromPathW(string pszPath);

        [DllImport("shell32.dll")]
        private static extern int SHOpenFolderAndSelectItems(IntPtr pidlFolder, int cild, IntPtr apidl, int dwFlags);

        [DllImport("shell32.dll")]
        private static extern void ILFree(IntPtr pidl);
    }
}