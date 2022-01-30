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
    public partial class AddContactPopup : FormBase
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
        private readonly MainForm MainForm;

        private AddContactPopup()
        {
            InitializeComponent();
            InitializeComponentFurther();
        }

        public AddContactPopup(Action<Contact> callback, MainForm mainForm) : this()
        {
            Callback = callback;
            MainForm = mainForm;
        }

        private void InitializeComponentFurther()
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
            Icon = Icon.FromHandle(Properties.Resources.default_pfp.GetHicon());

            AddButton.MouseDown += (_, _) => OnAddButtonClicked();
            AddButton.Cursor = Cursors.No;

            IpBox.Textbox.TextChanged += (_, _) => OnIpTextChange();

            InitTitleBar(this, "Add a Contact");
        }

        private void OnAddButtonClicked()
        {
            if (AddButton.Cursor == AddButtonProps.AllowCursor)
            {
                IPAddress ip = IPAddress.Parse(IpBox.Textbox.Text);
                if (MainForm.Contacts.All(x => !x.Ip.Equals(ip)))
                {
                    var contact = new Contact(ip);
                    Callback(contact);
                }
                else
                {
                    MainForm.NotificationPanel.ShowNotification(new Components.NotificationPanel.NotificationArgs
                    {
                        Content = "Cannot add, because a contact with the same IP already exists.",
                        Title = "Cannot add contact",
                        DurationMillis = 4500,
                    });
                }
                Close();
            }
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