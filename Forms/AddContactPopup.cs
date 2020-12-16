using ClausaComm.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClausaComm.Forms
{
    public partial class AddContactPopup : Form
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


        public AddContactPopup(Action<Contact> callback)
        {
            InitializeComponent();
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
            Icon = Icon.FromHandle(Properties.Resources.default_pfp.GetHicon());

            AddButton.MouseDown += (object _, MouseEventArgs _) => OnAddButtonClicked();
            AddButton.Cursor = Cursors.No;

            IpBox.textbox.TextChanged += (object _, EventArgs _) => OnIpTextChange();
            Callback = callback;
        }

        private void OnAddButtonClicked()
        {
            if (AddButton.Cursor == AddButtonProps.AllowCursor)
            {
                if (!Contact.XmlFile.GetContacts().Any(x => x.Ip == IpBox.textbox.Text))
                {
                    var contact = new Contact(IpBox.textbox.Text) { Save = true };
                    Callback(contact);
                }
                Close();
            }
        }

        [DllImport("user32.dll")]
        private static extern bool GetCaretPos(out Point lpPoint);
        private void OnIpTextChange()
        {
            string ip = IpBox.textbox.Text;

            int amountOfPeriods = ip.Count(x => x == '.');
            foreach (var character in ip)
            {
                if ((character != '.' && !char.IsDigit(character))
                    || ip.Split('.').Any(x => x.Length > 3)
                    || amountOfPeriods > 3
                    || (ip == "." && character == '.'))
                {
                    IpBox.textbox.Text = IpTextBefore;
                    IpBox.textbox.SelectionStart = CaretPositionBefore;
                    IpBox.textbox.SelectionLength = 0;
                    return;
                }
            }

            IpTextBefore = ip;
            GetCaretPos(out Point p);
            // Returns +9 for each number and +4 for a period. We're finding the amount of periods and adding 6 for each of them, so
            // that they act like a number, in order to be able to count caret position properly. Yeah, ugly af.
            CaretPositionBefore = (p.X + (amountOfPeriods * 6)) / 9;
            System.Diagnostics.Debug.WriteLine("caret pos: " + CaretPositionBefore);

            bool ipCorrect = IpUtils.IsIpCorrect(ip);
            AddButton.Cursor = ipCorrect ? AddButtonProps.AllowCursor : AddButtonProps.DisallowCursor;
            IpBox.BorderColor = string.IsNullOrEmpty(ip) || ipCorrect ? IpBoxProps.NeutralColor : IpBoxProps.IncorrectColor;

        }
    }
}
