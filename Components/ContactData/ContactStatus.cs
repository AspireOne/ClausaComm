using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ClausaComm.Components.ContactData
{
    public sealed partial class ContactStatus : PictureBox, IContactUsable
    {
        private Contact _contact;
        public const byte DefaultIconSize = 13;
        private static readonly Dictionary<Contact.Status, Brush> StatusColors = new()
        {
            { Contact.Status.Online, Brushes.Green },
            { Contact.Status.Offline, Brushes.Gray },
            { Contact.Status.Idle, Brushes.Orange },
        };

        public Contact Contact
        {
            get => _contact;
            set
            {
                if (_contact is not null)
                    _contact.StatusChange -= StatusChangeHandler;

                if ((_contact = value) is not null)
                    value.StatusChange += StatusChangeHandler;

                Invalidate();
            }
        }

        public ContactStatus()
        {
            InitializeComponent();
            Name = "Status";
            Size = new Size(DefaultIconSize, DefaultIconSize);
            BackColor = Color.Transparent;
            BorderStyle = BorderStyle.None;
        }

        public ContactStatus(IContainer container) : this() => container.Add(this);

        public ContactStatus(Contact contact) : this() => Contact = contact;



        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            pe.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pe.Graphics.FillEllipse(StatusColors[Contact?.CurrentStatus ?? Contact.Status.Offline], 0, 0, Size.Width - 1, Size.Height - 1);
        }

        public void StatusChangeHandler(object _, Contact.Status status) => Invalidate();
    }
}
