using ClausaComm.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ClausaComm.Contacts;

namespace ClausaComm.Components.ContactData
{
    public sealed partial class ContactPanel : Panel
    {
        #region selected panel

        private readonly Pen ContactSelectedLinePenAppearance = new(Constants.UiConstants.SecondaryColor, 2);
        private readonly Color SelectedPanelBackgroundColor = Color.FromArgb(10, 255, 255, 255); /*Constants.UIConstants.SecondaryColor.R, Constants.UIConstants.SecondaryColor.G, Constants.UIConstants.SecondaryColor.B*/

        private static event EventHandler SelectedPanelChange;

        private static ContactPanel _currentlySelectedPanel;
        
        public static ContactPanel CurrentlySelectedPanel
        {
            get => _currentlySelectedPanel;
            private set
            {
                _currentlySelectedPanel = value;
                SelectedPanelChange?.Invoke(value, EventArgs.Empty);
            }
        }

        #endregion selected panel

        private static readonly Dictionary<ControlUtils.MouseEvent, Color> MouseEventBackColor = new()
        {
            { ControlUtils.MouseEvent.Enter, Color.FromArgb(15, 255, 255, 255) },
            { ControlUtils.MouseEvent.Leave, Color.Transparent },
            { ControlUtils.MouseEvent.Down, Color.FromArgb(40, 255, 255, 255) },
        };
        
        public Action<Contact> OnClickAction { get; set; }
        private readonly FlashTimer Flasher;
        private bool Selected;
        private readonly bool IsUserPanel;

        private static readonly Color FlashPeakColor = Color.FromArgb(80, Constants.UiConstants.SecondaryColor.R, Constants.UiConstants.SecondaryColor.G, Constants.UiConstants.SecondaryColor.B);

        public readonly Contact Contact;
        /*make public ?*/
        private readonly ContactStatus StatusBox;
        private readonly ContactName NameLabel;

        public ContactPanel(Contact contact, Panel parentContainer, bool userPanel = false)
        {
            IsUserPanel = userPanel;
            Flasher = new FlashTimer(this);
            Contact = contact;
            SelectedPanelChange += (_, _) => Invalidate();

            #region Panel initialization

            InitializeComponent();
            parentContainer.Controls.Add(this);
            DoubleBuffered = true;
            BorderStyle = BorderStyle.None;
            Dock = userPanel ? DockStyle.Fill : DockStyle.Top;
            Height = 57;
            Parent = parentContainer;
            //Name = Contact.Id ?? Contact.Ip;
            Padding = Padding.Empty;
            Cursor = Cursors.Hand;

            #endregion Panel initialization

            #region child controls initialization

            int imagePadding = userPanel ? 10 : 14;
            ContactProfilePicture profilePictureBox = new(Contact)
            {
                Dock = DockStyle.Left,
                Parent = this,
                Size = new Size(Height - imagePadding, Height),
                Padding = new Padding(0, imagePadding/2, 0, imagePadding/2),
            };

            const int nameOffset = 2;
            NameLabel = new ContactName(Contact)
            {
                Location = new Point(profilePictureBox.Width + nameOffset, 0),
                Size = new Size(Width - profilePictureBox.Width - nameOffset, Height),
                Font = new Font(userPanel ? "Sans UI" : "Segoe UI", 12, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Parent = this,
                ForeColor = Color.FromArgb(225, 225, 225)
            };

            const int statusIconSize = 13;
            StatusBox = new ContactStatus(Contact)
            {
                Size = new Size(statusIconSize, statusIconSize),
                Location = new Point(profilePictureBox.Width - statusIconSize, profilePictureBox.Height - statusIconSize),
                Parent = profilePictureBox
            };

            #endregion child controls initialization

            foreach (Control control in Controls)
            {
                control.MouseDown += (_, e) => OnMouseDown(e);
                control.MouseUp += (_, e) => OnMouseUp(e);
                control.MouseEnter += (_, e) => OnMouseEnter(e);
                control.MouseLeave += (_, e) => OnMouseLeave(e);
                control.Click += (_, e) => OnClick(e);
            }

            ControlUtils.AddBackColorFilterOnMouseEvent(this, MouseEventBackColor);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Selected = true;
            OnClickAction?.Invoke(Contact);
            ChangeBackgroundColor(SelectedPanelBackgroundColor);

            if (!IsUserPanel)
                CurrentlySelectedPanel = this;

            Invalidate();
        }

        private void ChangeBackgroundColor(Color newColor)
        {
            ControlUtils.ChangeControlsBackColor(newColor, Controls);
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (CurrentlySelectedPanel == this)
                DrawPanelSelectedLine(e.Graphics);
            else if (Selected)
            {
                Selected = false;
                ChangeBackgroundColor(Color.Transparent);
            }
        }

        private void DrawPanelSelectedLine(Graphics g)
        {
            Point verticalLineStart = new(Width, 0);
            Point verticalLineStop = new(Width, Height);

            g.DrawLine(ContactSelectedLinePenAppearance, verticalLineStart, verticalLineStop);
        }

        public void Flash()
        {
            if (!Flasher.Timer.Enabled)
                Flasher.Timer.Start();
        }

        private class FlashTimer
        {
            public readonly Timer Timer = new() { Interval = 1 };
            private readonly ContactPanel Panel;

            //Higher = faster.
            private const int FlashSpeed = 2;

            private bool IncreaseFlash = true;
            private int LastFlashAlpha = 0;

            public FlashTimer(ContactPanel panel)
            {
                Timer.Tick += OnFlashTimerTick;
                Panel = panel;
            }

            private void OnFlashTimerTick(object _, EventArgs e)
            {
                var modifiedColor = Color.FromArgb(LastFlashAlpha += IncreaseFlash ? FlashSpeed : -FlashSpeed, FlashPeakColor.R, FlashPeakColor.G, FlashPeakColor.B);

                Panel.ChangeBackgroundColor(modifiedColor);

                if (LastFlashAlpha == FlashPeakColor.A)
                {
                    IncreaseFlash = false;
                }
                else if (LastFlashAlpha == 0)
                {
                    IncreaseFlash = true;
                    Timer.Stop();
                    Panel.ChangeBackgroundColor(Panel.Selected ? Panel.SelectedPanelBackgroundColor : Color.Transparent);
                }
            }
        }
    }
}