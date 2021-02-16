﻿using ClausaComm.Components.ContactData;
using ClausaComm.Components.Icons;
using ClausaComm.Forms;
using ClausaComm.Properties;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using ClausaComm.Contacts;

namespace ClausaComm.Components
{
    public partial class ActionPanel : Panel, IContactUsable
    {
        //public Action<Contact> RemoveContactAction { get; set; }
        private Contact _contact;
        private readonly Control[] ChildControls;

        private MainForm _mainForm;
        public MainForm MainForm
        {
            get => _mainForm;
            set
            {
                if (ReferenceEquals(_mainForm, value))
                    return;

                _mainForm = value;
                if (value is not null)
                {
                    MainForm.ToolTip1.SetToolTip(SaveUnsaveContactIcon,
                    "If the person's ip doesn't change, you can \n" +
                    "keep him saved instead of re-adding him \n" +
                    "every time you restart the program.");
                }
            }
        }

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

        private readonly SaveIcon SaveUnsaveContactIcon = new()
        {
            Location = new System.Drawing.Point(6, 12),
            Name = "RemoveContactIcon",
            Size = new System.Drawing.Size(43, 43),
            Padding = new Padding(5, 5, 5, 5),
            ColorIconOnClick = true,
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
                if (ReferenceEquals(value, _contact))
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
                    ChangeSaveIconStateAccordingly();
                }

            }
        }

        public ActionPanel()
        {
            InitializeComponent();
            CompleteComponentInitialization();

            ChildControls = new Control[] { Status, NameLbl, SaveUnsaveContactIcon, CallContactIcon, ProfilePicture, IpLbl };
            ChangeContactSpecificElementsVisibility(false);
            Array.ForEach(ChildControls, control => Controls.Add(control));

            SaveUnsaveContactIcon.Click += (_, _) =>
            {
                Contact.Save = !Contact.Save;
                ChangeSaveIconStateAccordingly();
            };
        }

        private void CompleteComponentInitialization()
        {
            BackColor = System.Drawing.Color.FromArgb(29, 29, 31);
            Dock = DockStyle.Top;
            Location = new System.Drawing.Point(0, 0);
            Name = "ActionPanel";
            Size = new System.Drawing.Size(884, 67);
        }

        public ActionPanel(IContainer container) : this() => container.Add(this);

        private void ChangeContactSpecificElementsVisibility(bool visible)
        {
            Array.ForEach(ChildControls, control => control.Visible = visible);
        }

        // Returns if was changed.
        private void ChangeSaveIconStateAccordingly()
        {
            SaveUnsaveContactIcon.CurrState = Contact.Save ? SaveIcon.State.Save : SaveIcon.State.Unsave;
        }
    }
}
