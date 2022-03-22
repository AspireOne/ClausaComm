using System;
using System.Collections.Generic;
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
        public readonly bool IsFile;

        private static readonly Dictionary<ControlUtils.MouseEvent, Color> MouseEventColors = new()
        {
            {ControlUtils.MouseEvent.Enter, Constants.UiConstants.ChatMessageOnHoverColor},
            {ControlUtils.MouseEvent.Leave, Constants.UiConstants.ChatBackColor},
        };

        public ChatMessagePanel(ChatMessage message, Contact contact)
        {
            InitializeComponent();
            (Message, Contact, IsFile) = (message, contact, !string.IsNullOrEmpty(message.FilePath));
            FillData();
            RegisterContactDataChanges();
            RegisterBackgroundChangeOnMouseEvent();

            SetDelivered(message.Way == ChatMessage.Ways.In || message.Delivered);
            if (message.Way == ChatMessage.Ways.Out)
                message.DeliveredChanged += (_, delivered) => SetDelivered(delivered);
        }

        private void RegisterContactDataChanges()
        {
            Contact.NameChange += (_, name) => ChatMessageName.Text = name;
            Contact.ProfilePicChange += (_, pic) =>
            {
                lock (Contact.ProfilePic)
                    ChatMessagePicture.Image = pic;
            };
        }

        private void FillData()
        {
            // Adding 3_600_000 milliseconds because the time is one hour late for some reason.
            DateTimeOffset date = DateTimeOffset.FromUnixTimeMilliseconds(Message.Time + 3_600_000);
            ChatMessageName.Text = Contact.Name;
            ChatMessageTime.Text = date.ToString("dd/MM/yyyy HH:mm");
            ChatMessageText.Text = Message.Text;
            ChatMessagePicture.Image = (Image)Contact.ProfilePic.Clone();

            if (!IsFile)
                return;
            
            ChatMessageText.Text = Message.FilePath;
            ChatMessageText.Click += (_, _) => OpenDirAndSelectFile(Message.FilePath);
            ChatMessageText.Cursor = Cursors.Hand;
        }
        
        private void RegisterBackgroundChangeOnMouseEvent()
        {
            foreach (Control control in ChatMessageHeader.Controls)
                PropagateMouseEvents(control);
            foreach (Control control in Controls)
                PropagateMouseEvents(control);

            //ControlUtils.AddBackColorFilterOnMouseEvent(this,  MouseEventColors);
        }
        
        private void PropagateMouseEvents(Control control)
        {
            ControlUtils.SetDoubleBuffered(control);
            control.MouseDown += (_, e) => OnMouseDown(e);
            control.MouseUp += (_, e) => OnMouseUp(e);
            control.MouseEnter += (_, e) => OnMouseEnter(e);
            control.MouseLeave += (_, e) => OnMouseLeave(e);
            control.Click += (_, e) => OnClick(e);
        }
        
        private void SetDelivered(bool delivered)
        {
            ChatMessageText.ForeColor = delivered
                ? (IsFile ? Constants.UiConstants.ChatTextColorLink : Constants.UiConstants.ChatTextColor)
                : (IsFile ? Constants.UiConstants.ChatTextColorLinkUndelivered : Constants.UiConstants.ChatTextColorUndelivered);
        }
        
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent is null)
                return;
            
            BackColor = Parent.BackColor;
            Parent.BackColorChanged += (_, _) => BackColor = Parent.BackColor;
        }

        private static void OpenDirAndSelectFile(string filePath)
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