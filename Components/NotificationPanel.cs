using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClausaComm.Components.Icons;
using ClausaComm.Forms;
using ClausaComm.Utils;

namespace ClausaComm.Components
{
    public sealed partial class NotificationPanel : Panel
    {
        private readonly Queue<NotificationArgs> NotificationQueue = new();
        private readonly Panel ContentPanel = new();
        private readonly Panel TitlePanel = new();
        private readonly Label Title = new();
        private readonly Label CloseTimeLabel = new();
        private readonly Panel ButtonsPanel = new();
        private readonly CrossIcon CloseButton = new();
        private readonly Label Content = new();
        private readonly Button MiddleButt = new();
        private readonly Button LeftButt = new();
        private readonly Button RightButt = new();

        private const int SwipeSpeed = 7;
        private const int InitialHeight = 92;
        private static readonly Pen BorderPen = new(Color.FromArgb(70, 70, 70), 1);
        private static readonly Size BorderSizeOffset = new(1, 1);
        private NotificationArgs LastArgs;

        private readonly Timer NotificationAutoCloseTimer = new()
        {
            Enabled = false,
            Interval = 1000
        };

        private readonly Timer NotificationSwipeTimer = new()
        {
            Enabled = false,
            Interval = 10
        };

        public MainForm Form { get; set; }

        public struct NotificationArgs
        {
            public struct ButtonArgs
            {
                public string Name;
                public EventHandler ClickCallback;
            }

            public override bool Equals(object? obj)
            {
                return obj is NotificationArgs args
                       && args.Title == Title
                       && args.Content == Content
                       && args.DurationMillis == DurationMillis;
            }

            public override int GetHashCode() 
                => HashCode.Combine(DurationMillis, Title, Content, LeftButton, MiddleButton, RightButton);

            public int DurationMillis;
            public string Title;
            public string Content;
            public ButtonArgs? LeftButton;
            public ButtonArgs? MiddleButton;
            public ButtonArgs? RightButton;
        }
        
        public NotificationPanel()
        {
            InitializeComponent();

            #region Panel initialization

            Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            AutoSize = true;
            BackColor = Constants.UiConstants.NotificationPanelColor;
            BorderStyle = BorderStyle.None;
            Controls.Add(ContentPanel);
            Controls.Add(TitlePanel);
            Controls.Add(ButtonsPanel);
            Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            //Location = new Point(611, 585);
            Name = "NotificationPanel";
            Size = new Size(267, InitialHeight);
            TabIndex = 11;
            Visible = false;
            Padding = new Padding(2, 2, 2, 2);

            #endregion Panel initialization

            #region Component Initialization

            ContentPanel.Controls.Add(Content);
            ContentPanel.AutoSize = true;
            ContentPanel.Location = new Point(4, 25);
            ContentPanel.Name = "ContentPanel";
            ContentPanel.Size = new Size(265, 42);
            ContentPanel.TabIndex = 7;

            Content.AutoSize = true;
            Content.Dock = DockStyle.Fill;
            Content.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            Content.Location = new Point(0, 0);
            Content.MaximumSize = new Size(Width, int.MaxValue);
            Content.Name = "Content";
            Content.Size = new Size(53, 17);
            Content.TabIndex = 1;
            Content.Text = "Content";

            TitlePanel.Controls.Add(CloseTimeLabel);
            TitlePanel.Controls.Add(CloseButton);
            TitlePanel.Controls.Add(Title);
            TitlePanel.Dock = DockStyle.Top;
            TitlePanel.Location = new Point(0, 0);
            TitlePanel.Name = "TitlePanel";
            TitlePanel.Size = new Size(265, 22);
            TitlePanel.TabIndex = 6;

            CloseTimeLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            CloseTimeLabel.Location = new Point(224, 0);
            CloseTimeLabel.Name = "CloseTimeLabel";
            CloseTimeLabel.Size = new Size(19, 22);
            CloseTimeLabel.TabIndex = 2;
            CloseTimeLabel.Text = "0";
            CloseTimeLabel.TextAlign = ContentAlignment.MiddleRight;

            CloseButton.CircleColor = Color.White;
            CloseButton.ClickCircleColor = Color.FromArgb(253, 152, 10);
            CloseButton.ClickLineColor = Color.FromArgb(253, 152, 10);
            CloseButton.ColorBoxOnHover = false;
            CloseButton.ColorCircleOnClick = false;
            CloseButton.ColorCircleOnHover = false;
            CloseButton.ColorIconOnClick = true;
            CloseButton.ColorIconOnHover = true;
            CloseButton.Cursor = Cursors.Hand;
            CloseButton.HoverCircleColor = Color.FromArgb(253, 172, 10);
            CloseButton.HoverLineColor = Color.White;
            CloseButton.IconPaddingFactor = 1.3f;
            CloseButton.Image = null;
            CloseButton.LineColor = Color.Gray;
            CloseButton.LineWidth = 1.6F;
            CloseButton.Location = new Point(242, 0);
            CloseButton.Name = "CloseButton";
            CloseButton.ShowCircle = false;
            CloseButton.Size = new Size(21, 20);
            CloseButton.SizeMode = PictureBoxSizeMode.StretchImage;
            CloseButton.TabIndex = 1;
            CloseButton.TabStop = false;
            CloseButton.UnderlineOnHover = false;
            CloseButton.Click += (_, _) => Hide();

            Title.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            Title.Location = new Point(0, 0);
            Title.ForeColor = Color.FromArgb(250, 155, 55);
            Title.Name = "Title";
            Title.Padding = new Padding(3, 0, 3, 0);
            Title.Size = new Size(218, 20);
            Title.TabIndex = 0;
            Title.Text = "A title";
            Title.TextAlign = ContentAlignment.MiddleLeft;

            ButtonsPanel.Controls.Add(RightButt);
            ButtonsPanel.Controls.Add(LeftButt);
            ButtonsPanel.Controls.Add(MiddleButt);
            ButtonsPanel.Dock = DockStyle.Bottom;
            ButtonsPanel.Location = new Point(0, 64);
            ButtonsPanel.Name = "ButtonsPanel";
            ButtonsPanel.Size = new Size(265, 26);
            ButtonsPanel.TabIndex = 5;

            RightButt.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            RightButt.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            RightButt.ForeColor = Color.FromArgb(64, 64, 64);
            RightButt.Location = new Point(186, 2);
            RightButt.Name = "RightButt";
            RightButt.Size = new Size(79, 24);
            RightButt.TabIndex = 2;
            RightButt.Text = "RightButton";
            RightButt.UseVisualStyleBackColor = true;

            LeftButt.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LeftButt.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LeftButt.ForeColor = Color.FromArgb(64, 64, 64);
            LeftButt.Location = new Point(0, 2);
            LeftButt.Name = "LeftButt";
            LeftButt.Size = new Size(77, 24);
            LeftButt.TabIndex = 4;
            LeftButt.Text = "LeftButton";
            LeftButt.UseVisualStyleBackColor = true;

            MiddleButt.Anchor = AnchorStyles.Bottom;
            MiddleButt.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            MiddleButt.ForeColor = Color.FromArgb(64, 64, 64);
            MiddleButt.Location = new Point(93, 2);
            MiddleButt.Name = "MiddleButt";
            MiddleButt.Size = new Size(78, 24);
            MiddleButt.TabIndex = 3;
            MiddleButt.Text = "MiddleButton";
            MiddleButt.UseVisualStyleBackColor = true;

            #endregion Component Initialization

            Button[] notificationButts = { LeftButt, MiddleButt, RightButt };
            Array.ForEach(notificationButts, butt => butt.Click += (_, _) => Hide());
        }

        public NotificationPanel(IContainer container) : this() => container.Add(this);

        public void ShowNotification(NotificationArgs args)
        {
            BringToFront();
            NotificationArgs lastArgs = LastArgs;
            LastArgs = args;
            
            if (Visible)
            {
                if (!args.Equals(lastArgs))
                    NotificationQueue.Enqueue(args);
                return;
            }

            int timeLeft = args.DurationMillis;
            Point originalLocation = Location;

            Title.Text = args.Title;
            Content.Text = args.Content;
            CloseTimeLabel.Text = (timeLeft / 1000).ToString();

            int contentPanelLowerY = ContentPanel.Location.Y + ContentPanel.Height;
            if (contentPanelLowerY > ButtonsPanel.Location.Y)
            {
                int exceedsBy = contentPanelLowerY - ButtonsPanel.Location.Y;
                Height += exceedsBy;
                Location = new(Location.X, Location.Y - exceedsBy);
            }

            RegisterNotificationButt(LeftButt, args.LeftButton);
            RegisterNotificationButt(MiddleButt, args.MiddleButton);
            RegisterNotificationButt(RightButt, args.RightButton);

            Show();
            StartSwipeIn();

            NotificationAutoCloseTimer.Tick += OnCloseTimerTick;
            NotificationAutoCloseTimer.Start();

            void OnCloseTimerTick(object sender, EventArgs e)
            {
                timeLeft -= NotificationAutoCloseTimer.Interval;
                if (timeLeft <= 0 || !Visible)
                {
                    NotificationAutoCloseTimer.Stop();
                    UnregisterNotificationButt(LeftButt, args.LeftButton);
                    UnregisterNotificationButt(MiddleButt, args.MiddleButton);
                    UnregisterNotificationButt(RightButt, args.RightButton);
                    NotificationAutoCloseTimer.Tick -= OnCloseTimerTick;

                    if (Visible)
                        StartSwipeOutAndHide(ResetAndShowNext);
                    else
                        ResetAndShowNext();

                    void ResetAndShowNext()
                    {
                        Height = InitialHeight;
                        Location = originalLocation;

                        if (NotificationQueue.Any())
                            ShowNotification(NotificationQueue.Dequeue());
                    }
                    return;
                }

                CloseTimeLabel.Text = (timeLeft / 1000).ToString();
            }

            static void RegisterNotificationButt(Button button, NotificationArgs.ButtonArgs? args)
            {
                if (button.Visible = args.HasValue)
                {
                    button.Text = args.Value.Name;
                    button.Click += args.Value.ClickCallback;
                }
            }

            static void UnregisterNotificationButt(Button button, NotificationArgs.ButtonArgs? args)
            {
                if (args.HasValue)
                    button.Click -= args.Value.ClickCallback;
            }
        }

        private void StartSwipeIn()
        {
            Point targetPos = Location;
            Location = new Point(Form.Width - Width / 2, targetPos.Y);

            NotificationSwipeTimer.Tick += OnSwipeTimerTick;
            NotificationSwipeTimer.Start();

            void OnSwipeTimerTick(object sender, EventArgs e)
            {
                if (Location.X <= targetPos.X)
                {
                    Location = targetPos;
                    NotificationSwipeTimer.Stop();
                    NotificationSwipeTimer.Tick -= OnSwipeTimerTick;
                    return;
                }

                Location = new Point(Location.X - SwipeSpeed, Location.Y);
            }
        }

        private void StartSwipeOutAndHide(Action completedCallback = null)
        {
            Point targetPos = new Point(Form.Width - Width / 2, Location.Y);

            NotificationSwipeTimer.Tick += OnSwipeTimerTick;
            NotificationSwipeTimer.Start();

            void OnSwipeTimerTick(object sender, EventArgs e)
            {
                if (Location.X >= targetPos.X)
                {
                    Location = targetPos;
                    NotificationSwipeTimer.Stop();
                    NotificationSwipeTimer.Tick -= OnSwipeTimerTick;
                    Hide();
                    completedCallback?.Invoke();
                    return;
                }
                Location = new Point(Location.X + SwipeSpeed, Location.Y);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Substract a pixel from the width and height, because otherwise the rectangle is one pixel larger than the panel.
            e.Graphics.DrawRectangle(BorderPen, new Rectangle(ClientRectangle.Location, ClientRectangle.Size - BorderSizeOffset));
        }
    }
}