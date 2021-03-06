﻿using ClausaComm.Utils;
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

        private readonly Pen ContactSelectedLinePenAppearance = new(Constants.UIConstants.SecondaryColor, 2);
        private readonly Color SelectedPanelBackgroundColor = Color.FromArgb(10, 255, 255, 255); /*Constants.UIConstants.SecondaryColor.R, Constants.UIConstants.SecondaryColor.G, Constants.UIConstants.SecondaryColor.B*/

        private static event EventHandler SelectedPanelChange;

        private static ContactPanel _currentlySelectedPanel;

        private static ContactPanel CurrentlySelectedPanel
        {
            get => _currentlySelectedPanel;
            set
            {
                _currentlySelectedPanel = value;
                SelectedPanelChange?.Invoke(value, EventArgs.Empty);
            }
        }

        #endregion selected panel

        private static readonly Dictionary<ComponentUtils.MouseEvent, Color> MouseEventBackColor = new()
        {
            { ComponentUtils.MouseEvent.Enter, Color.FromArgb(15, 255, 255, 255) },
            { ComponentUtils.MouseEvent.Leave, Color.Transparent },
            { ComponentUtils.MouseEvent.Down, Color.FromArgb(40, 255, 255, 255) },
        };

        public Action OnClickAction { get; set; }
        public Action<Contact> OnClickActionContact { get; set; }
        private readonly FlashTimer Flasher;
        private bool Selected;

        private static readonly Color FlashPeakColor = Color.FromArgb(80, Constants.UIConstants.SecondaryColor.R, Constants.UIConstants.SecondaryColor.G, Constants.UIConstants.SecondaryColor.B);

        public readonly Contact Contact;
        /*make public ?*/
        private readonly ContactStatus StatusBox;
        private readonly ContactName NameLabel;

        public ContactPanel(Contact contact, Panel parentContainer)
        {
            Flasher = new FlashTimer(this);
            Contact = contact;
            SelectedPanelChange += (_, _) => Invalidate();

            #region Panel initialization

            InitializeComponent();
            parentContainer.Controls.Add(this);
            DoubleBuffered = true;
            BorderStyle = BorderStyle.None;
            Dock = Contact.IsUser ? DockStyle.Fill : DockStyle.Top;
            Height = 57;
            Parent = parentContainer;
            //Name = Contact.Id ?? Contact.Ip;
            Padding = Padding.Empty;
            Cursor = Cursors.Hand;

            #endregion Panel initialization

            #region child controls initialization

            ContactProfilePicture profilePictureBox = new(Contact)
            {
                Dock = DockStyle.Left,
                Parent = this,
                Size = new Size(this.Height, this.Height)
            };

            const int nameOffset = 2;
            NameLabel = new ContactName(Contact)
            {
                Location = new Point(profilePictureBox.Width + nameOffset, 0),
                Size = new Size(Width - profilePictureBox.Width - nameOffset, Height),
                Font = new Font(Contact.IsUser ? "Sans UI" : "Segoe UI", 13, FontStyle.Regular),
                Parent = this,
                ForeColor = Color.FromArgb(225, 225, 225)
            };

            const int statusIconSize = 13;
            StatusBox = contact.IsUser ? null : new ContactStatus(Contact)
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

            ComponentUtils.AddBackColorFilterOnMouseEvent(this, MouseEventBackColor);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            OnClickAction?.Invoke();
            OnClickActionContact?.Invoke(Contact);
            Selected = true;
            ChangeBackgroundColor(SelectedPanelBackgroundColor);

            if (!Contact.IsUser)
                CurrentlySelectedPanel = this;

            Invalidate();
        }

        private void ChangeBackgroundColor(Color newColor)
        {
            ComponentUtils.ChangeControlsBackColor(newColor, Controls);
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

        public void FlashPanel()
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
                    //TODO: Remove this quick fix and make the flash take into account base color.
                    Panel.ChangeBackgroundColor(Panel.Selected ? Panel.SelectedPanelBackgroundColor : Color.Transparent);
                }
            }
        }
    }
}