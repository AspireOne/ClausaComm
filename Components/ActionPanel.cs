using ClausaComm.Components.ContactData;
using ClausaComm.Components.Icons;
using ClausaComm.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClausaComm.Components
{
    public partial class ActionPanel : Panel, IContactUsable
    {
        public Action<Contact> RemoveContactAction { get; set; }
        private Contact _contact;
        private readonly Control[] ChildControls;

        #region Icons
        private readonly PhoneIcon CallContactIcon = new()
        {
            ColorIconOnHover = false,
            HoverIconColor = System.Drawing.Color.FromArgb(44, 117, 252),
            ColorIconOnClick = true,
            Location = new System.Drawing.Point(65, 12),
            Name = "CallContactIcon",
            Padding = new Padding(6),
            Size = new System.Drawing.Size(43, 43),
            //tooltip1.setToolTip(this.CallContactIcon, "Call"),
            UnderlineOnHover = true,
        };

        private readonly CloseIcon RemoveContactIcon = new()
        {
            ColorCircleOnHover = false,
            ColorLinesOnHover = false,
            ColorCircleOnClick = false,
            ColorLinesOnClick = true,
            ShowCircle = false,
            LineColor = IconBase.DefaultIconColor,
            LineWidth = 2f,
            Location = new System.Drawing.Point(6, 12),
            Name = "RemoveContactIcon",
            Size = new System.Drawing.Size(43, 43),
            //p(this.RemoveContactIcon, "Remove Contact"),
            UnderlineOnHover = true,
        };
        #endregion

        #region Contact data
        private readonly Label IpLbl = new()
        {
            Anchor = AnchorStyles.None,
            Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point),
            Location = new System.Drawing.Point(355, 1),
            Name = "IpLbl",
            Size = new System.Drawing.Size(148, 65),
            TabIndex = 9,
            Text = "000.000.000.000",
            TextAlign = System.Drawing.ContentAlignment.MiddleRight,
        };

        private readonly ContactName NameLbl = new()
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Font = new System.Drawing.Font("Segoe UI", 13, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point),
            Contact = null,
            Location = new System.Drawing.Point(543, 1),
            Name = "NameLbl",
            Size = new System.Drawing.Size(226, 64),
            TabIndex = 6,
            Text = "Contact Name",
            TextAlign = System.Drawing.ContentAlignment.MiddleRight,
        };

        private readonly ContactStatus Status = new()
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            BackColor = System.Drawing.Color.Transparent,
            Contact = null,
            Location = new System.Drawing.Point(855, 29),
            Name = "Status",
            Size = new System.Drawing.Size(13, 13),
            TabIndex = 7,
            TabStop = false,
        };

        private readonly ContactProfilePicture ProfilePicture = new()
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Contact = null,
            Image = Resources.default_pfp,
            Location = new System.Drawing.Point(775, 1),
            Name = "ProfilePicture",
            Padding = new Padding(2),
            Size = new System.Drawing.Size(64, 64),
            SizeMode = PictureBoxSizeMode.StretchImage,
            TabIndex = 0,
            TabStop = false,
        };
        #endregion


        public Contact Contact
        {
            get => _contact;
            set
            {
                if (value == _contact)
                    return;

                _contact = value;

                Array.ForEach(ChildControls, control =>
                {
                    if (control is IContactUsable contactUsable)
                        contactUsable.Contact = value;
                });

                if (value is null)
                {
                    ChangeContactSpecificElementsVisibility(false);
                }
                else
                {
                    IpLbl.Text = value.Ip;
                    ChangeContactSpecificElementsVisibility(true);
                }

            }
        }


        public ActionPanel()
        {
            InitializeComponent();
            BackColor = System.Drawing.Color.FromArgb(29, 29, 31);
            Dock = DockStyle.Top;
            Location = new System.Drawing.Point(0, 0);
            Name = "ActionPanel";
            Size = new System.Drawing.Size(884, 67);

            ChildControls = new Control[] { Status, NameLbl, RemoveContactIcon, CallContactIcon, ProfilePicture, IpLbl };
            ChangeContactSpecificElementsVisibility(false);
            Array.ForEach(ChildControls, control => Controls.Add(control));
            RemoveContactIcon.Click += (object _, EventArgs _) => RemoveContactAction(Contact);
        }

        private void ChangeContactSpecificElementsVisibility(bool visible)
        {
            //Visible = visible;
            Array.ForEach(ChildControls, control => control.Visible = visible);
        }

        public ActionPanel(IContainer container) : this() => container.Add(this);
    }
}
