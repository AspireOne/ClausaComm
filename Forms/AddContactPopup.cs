using ClausaComm.Utils;
using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ClausaComm.Contacts;

namespace ClausaComm.Forms
{
    public partial class AddContactPopup : PopupBase
    {
        private struct IpBoxProps
        {
            public static readonly Color IncorrectColor = Color.Red;
            public static readonly Color NeutralColor = Color.Gray;
        }

        private struct AddButtonProps
        {
            public static readonly Cursor DisallowCursor = Cursors.No;
            public static readonly Cursor AllowCursor = Cursors.Hand;
        }

        private string IpTextBefore = "";
        private int CaretPositionBefore;
        private readonly Action<Contact> Callback;
        private readonly Action<Contact, Action<bool>> Connect;

        public AddContactPopup(Action<Contact> callback, MainForm containingForm, Action<Contact, Action<bool>> connect) : base(containingForm)
        {
            Callback = callback;
            Connect = connect;

            InitializeComponent();
            InitializeComponentFurther();
            Init();
            IpBox.KeyPress += (_, e) =>
            {
                if (e.KeyChar == (char)13)
                    OnAddButtonClicked();
            };
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        private void InitializeComponentFurther()
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
            Icon = Icon.FromHandle(Properties.Resources.default_pfp.GetHicon());

            AddButton.MouseDown += (_, _) => OnAddButtonClicked();
            AddButton.Cursor = Cursors.No;

            IpBox.Textbox.TextChanged += (_, _) => OnIpTextChange();
        }

        private void OnAddButtonClicked()
        {
            if (AddButton.Cursor != AddButtonProps.AllowCursor)
                return;
            
            IPAddress ip = IPAddress.Parse(IpBox.Textbox.Text);
                
            if (ContainingForm.Contacts.Any(x => x.Ip.Equals(ip)))
            {
                NoteLabel.Text = "Contact already exists.";
                return;
            }
                
            var contact = new Contact(ip);
            NoteLabel.Text = "Connecting...";
            Connect(contact, connected =>
            {
                if (!connected)
                    Invoke(() => NoteLabel.Text = "Contact is not online or doesn't exist.");
                else
                    Invoke(() =>
                    {
                        Callback(contact);
                        Close();
                    });
            });
        }

        [DllImport("user32.dll")]
        private static extern bool GetCaretPos(out Point lpPoint);

        private void OnIpTextChange()
        {
            string ip = IpBox.Textbox.Text;

            int amountOfPeriods = ip.Count(x => x == '.');

            if (ip.Any(ch => ch != '.' && !char.IsDigit(ch)) || ip.Split('.').Any(x => x.Length > 3) || amountOfPeriods > 3 || ip.Contains("..") || ip.StartsWith('.'))
            {
                RevertTextBox();
                return;
            }

            void RevertTextBox()
            {
                IpBox.Textbox.Text = IpTextBefore;
                IpBox.Textbox.SelectionStart = CaretPositionBefore;
                IpBox.Textbox.SelectionLength = 0;
            }

            IpTextBefore = ip;
            GetCaretPos(out Point p);
            // Returns +9 for each number and +4 for a period. We're finding the amount of periods and adding 6 for each of them, so
            // that they act like a number, in order to be able to count caret position properly. Yeah, ugly.
            CaretPositionBefore = (p.X + amountOfPeriods * 6) / 9;

            bool ipCorrect = IpUtils.IsIpCorrect(ip);
            AddButton.Cursor = ipCorrect ? AddButtonProps.AllowCursor : AddButtonProps.DisallowCursor;
            IpBox.BorderColor = string.IsNullOrEmpty(ip) || ipCorrect ? IpBoxProps.NeutralColor : IpBoxProps.IncorrectColor;
        }
    }
}