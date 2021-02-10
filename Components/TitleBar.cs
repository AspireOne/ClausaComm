using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using ClausaComm.Components.Icons;
using ClausaComm.Forms;

namespace ClausaComm.Components
{
    public partial class TitleBar : Panel
    {
        public string Title
        {
            get => TitleText.Text;
            set => TitleText.Text = value;
        }

        private NotifyIcon _notifyIcon;

        public NotifyIcon PinNotifyIcon
        {
            get => _notifyIcon;
            set
            {
                if (ReferenceEquals(_notifyIcon, value))
                    return;

                _notifyIcon = value;
                value.Click += (_, _) => UnpinForm();
            }
        }


        #region title bar elements initialization
        private const byte ElementSize = 24;
        private const byte ElementWidth = 1;
        private const byte BoxColorOpacity = 60;
        private static readonly Color ElementColor = Color.FromArgb(182, 182, 182);
        private static readonly Color CloseButtonColor = Color.FromArgb(210, 210, 200);
        private static readonly Color ElementColorOnHover = Color.FromArgb(255, 255, 255);
        private static readonly SolidBrush ElementBackgroundBrush = new(Color.FromArgb(BoxColorOpacity, Color.FromArgb(225, 165, 46)));
        private static readonly SolidBrush CloseButtonBackgroundBrush = new(Color.FromArgb(240, 255, 51, 58));
        // The "Location" property indicates in what order the elements should be. 0 = first.
        private readonly Label TitleText = new()
        {
            Font = new Font("Segoe UI", 11f, FontStyle.Regular, GraphicsUnit.Point),
            Location = new Point(1, 0),
            Dock = DockStyle.Bottom,
            Margin = new Padding(3, 0, 3, 0),
            Name = "TitleText",
            Text = "ClausaComm",
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.FromArgb(174, 174, 174)
        };

        private readonly PinIcon Pin = new()
        {
            IconColor = ElementColor,
            Location = new Point(0, 0),
            Name = "Pin",
            // The ImageIcon is for some reason smaller than LineIcon even when they're at the same width, so we are adding an offset of 2.
            Size = new Size(ElementSize + 2, ElementSize),
            Dock = DockStyle.Right,
            Margin = new Padding(0, 0, 2, 0),
            Padding = new Padding(6, 4, 6, 4),
            ColorBoxOnHover = true,
            BoxOnHoverBrush = ElementBackgroundBrush,
            ColorIconOnHover = true,
            HoverIconColor = ElementColorOnHover
        };

        private readonly PictureBox ProgramIconBox = new()
        {
            Dock = DockStyle.Left,
            Location = new Point(0, 0),
            Name = "ProgramIconBox",
            Size = new Size(ElementSize, ElementSize),
        };

        private readonly MinimizeIcon MinimizeButton = new()
        {
            Dock = DockStyle.Right,
            IconPaddingFactor = 4f,
            Location = new Point(1, 0),
            Name = "MinimizeButton",
            Size = new Size(ElementSize, ElementSize),
            LineColor = ElementColor,
            LineWidth = ElementWidth,
            ColorBoxOnHover = true,
            BoxOnHoverBrush = ElementBackgroundBrush,
            ColorIconOnHover = true,
            HoverLineColor = ElementColorOnHover
        };

        private readonly MaximizeIcon MaximizeButton = new()
        {
            Dock = DockStyle.Right,
            IconPaddingFactor = 4.1f,
            Location = new Point(2, 0),
            Name = "MaximizeButton",
            Size = new Size(ElementSize, ElementSize),
            LineColor = ElementColor,
            LineWidth = ElementWidth,
            ColorBoxOnHover = true,
            BoxOnHoverBrush = ElementBackgroundBrush,
            ColorIconOnHover = true,
            HoverLineColor = ElementColorOnHover
        };

        private readonly CrossIcon CloseButton = new()
        {
            Dock = DockStyle.Right,
            IconPaddingFactor = 4.3f,
            Location = new Point(3, 0),
            Name = "CloseButton",
            Size = new Size(ElementSize, ElementSize),
            LineColor = CloseButtonColor,
            LineWidth = ElementWidth,
            ColorBoxOnHover = true,
            BoxOnHoverBrush = CloseButtonBackgroundBrush,
            ColorIconOnHover = true,
            HoverLineColor = ElementColorOnHover
        };
        #endregion

        // We would make the form be passed in the constructor under normal circumstances, but the fucking WF editor basically isn't able to do that.
        public FormBase Form
        {
            get => _form;
            set
            {
                if (_form == value || (_form = value) is null)
                    return;

                InitForm();
                ChangeSizingElementsVisibility(value.Resizable);
                Pin.Visible = value.Pinnable;
            }
        }
        private FormBase _form;
        private bool MouseIsDown;
        private Point LastFormLocation;
        public const byte BarHeight = 25;


        #region constructors & initialization
        public TitleBar(IContainer container) : this() => container.Add(this);

        public TitleBar()
        {
            InitializeComponent();
            InitializeComponentFurther();
        }

        private void InitializeComponentFurther()
        {
            Controls.Add(ProgramIconBox);
            Controls.Add(TitleText);
            Controls.Add(Pin);
            Controls.Add(MinimizeButton);
            Controls.Add(MaximizeButton);
            Controls.Add(CloseButton);
            Location = new Point(0, 0);
            Name = "TitleBar";
            MinimumSize = new Size(0, BarHeight);
            MaximumSize = new Size(int.MaxValue, BarHeight);
            Dock = DockStyle.Top;
            TabIndex = 1;
            BackColor = Color.FromArgb(28, 28, 28);

            MouseDown += Drag;
            TitleText.MouseDown += Drag;
            ProgramIconBox.MouseDown += Drag;
            Pin.Click += (_, _) =>
            {
                Debug.WriteLine("Pin clicked");
                if (Form?.Pinnable == true)
                    PinForm();
            };
        }
        #endregion

        private void InitForm()
        {
            Form.ResizableChanged += (_, resizable) => ChangeSizingElementsVisibility(resizable);
            Form.PinnableChanged += (_, pinnable) => Pin.Visible = pinnable;
            CloseButton.Click += (_, _) => Form.Close();
            MinimizeButton.Click += (_, _) => Form.WindowState = FormWindowState.Minimized;
            MaximizeButton.Click += (_, _) => Form.WindowState =
            Form.WindowState == FormWindowState.Maximized
            ? FormWindowState.Normal
            : FormWindowState.Maximized;
        }

        private void PinForm()
        {
            if (PinNotifyIcon is not null)
            {
                Form.Visible = false;
                PinNotifyIcon.Visible = true;
            }
        }

        private void UnpinForm()
        {
            Form.Visible = true;
            if (PinNotifyIcon is not null)
                PinNotifyIcon.Visible = false;
        }

        private void ChangeSizingElementsVisibility(bool visible)
        {
            Pin.Visible = visible;
            MinimizeButton.Visible = visible;
            MaximizeButton.Visible = visible;
        }

        #region Importing DLLs to enable dragging
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        #endregion

        private void Drag(object sender, MouseEventArgs e)
        {
            if (Form is null || Form.WindowState == FormWindowState.Maximized)
                return;

            const int wmNclbuttondown = 0xA1;
            const int htCaption = 0x2;

            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                _ = SendMessage(Form.Handle, wmNclbuttondown, htCaption, 0);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MouseIsDown = true;
            LastFormLocation = e.Location;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (MouseIsDown && Form.WindowState != FormWindowState.Maximized)
            {
                Location = new Point(Location.X - LastFormLocation.X + e.X, Location.Y - LastFormLocation.Y + e.Y);
                Update();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MouseIsDown = false;
        }
    }
}
