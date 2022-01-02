using ClausaComm.Components.ContactData;
using ClausaComm.Components.Icons;
using ClausaComm.Forms;
using ClausaComm.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
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
                    "keep them saved instead of re-adding them \n" +
                    "every time you restart the program.");
                }
            }
        }

        #region Icons

        private readonly PhoneIcon CallContactIcon = new()
        {
            ColorIconOnHover = false,
            HoverIconColor = Color.FromArgb(44, 117, 252),
            ColorIconOnClick = true,
            Location = new Point(65, 12),
            Name = "CallContactIcon",
            Padding = new Padding(6),
            Size = new Size(43, 43),
            //tooltip1.setToolTip(this.CallContactIcon, "Call"),
            UnderlineOnHover = true,
        };

        private readonly SaveIcon SaveUnsaveContactIcon = new()
        {
            Location = new Point(6, 12),
            Name = "RemoveContactIcon",
            Size = new Size(43, 43),
            Padding = new Padding(5, 5, 5, 5),
            ColorIconOnClick = true,
            UnderlineOnHover = true,
        };

        #endregion Icons

        #region Contact data

        private readonly Label IpLbl = new()
        {
            Anchor = AnchorStyles.None,
            Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point),
            Location = new Point(355, 1),
            Name = "IpLbl",
            Size = new Size(148, 65),
            TabIndex = 9,
            Text = "000.000.000.000",
            TextAlign = ContentAlignment.MiddleRight,
        };

        private readonly ContactName NameLbl = new()
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Font = new Font("Segoe UI", 13, FontStyle.Regular, GraphicsUnit.Point),
            Contact = null,
            Location = new Point(543, 1),
            Name = "NameLbl",
            Size = new Size(226, 64),
            TabIndex = 6,
            Text = "Contact Name",
            TextAlign = ContentAlignment.MiddleRight,
        };

        private readonly ContactStatus Status = new()
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            BackColor = Color.Transparent,
            Contact = null,
            Location = new Point(855, 29),
            Name = "Status",
            Size = new Size(13, 13),
            TabIndex = 7,
            TabStop = false,
        };

        private readonly ContactProfilePicture ProfilePicture = new()
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Contact = null,
            Image = Resources.default_pfp,
            Location = new Point(775, 1),
            Name = "ProfilePicture",
            Padding = new Padding(2),
            Size = new Size(64, 64),
            SizeMode = PictureBoxSizeMode.StretchImage,
            TabIndex = 0,
            TabStop = false,
        };

        #endregion Contact data

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
            InitializeComponentFurther();

            ChildControls = new Control[] { Status, NameLbl, SaveUnsaveContactIcon, CallContactIcon, ProfilePicture, IpLbl };
            ChangeContactSpecificElementsVisibility(false);
            Array.ForEach(ChildControls, control => Controls.Add(control));

            SaveUnsaveContactIcon.Click += (_, _) =>
            {
                if (Contact.Id is null)
                {
                    MainForm.NotificationPanel.ShowNotification(new NotificationPanel.NotificationArgs()
                    {
                        Content = "You cannot save a contact that has never been active.",
                        Title = "Cannot save contact",
                        DurationMillis = 4000,
                    });
                    return;
                }
                Contact.Save = !Contact.Save;
                ChangeSaveIconStateAccordingly();
            };
        }

        private void InitializeComponentFurther()
        {
            BackColor = Color.FromArgb(29, 29, 31);
            Dock = DockStyle.Top;
            Location = new Point(0, 0);
            Name = "ActionPanel";
            Size = new Size(884, 67);
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