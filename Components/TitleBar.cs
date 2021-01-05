using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        #region title bar elements initialization
        private const byte ElementSize = 24;
        private const byte ElementWidth = 1;
        private const byte BoxColorOpacity = 60;
        private static readonly Color ElementColor = Color.FromArgb(200, 200, 200);
        private static readonly SolidBrush ElementBackgroundBrush = new(Color.FromArgb(BoxColorOpacity, Color.FromArgb(225, 165, 46)));
        private static readonly SolidBrush CloseButtonBackgroundBrush = new(Color.FromArgb(BoxColorOpacity, Color.Red));
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
            Size = new Size(22, ElementSize),
            Dock = DockStyle.Right,
            Margin = new Padding(0, 0, 2, 0),
            Padding = new Padding(3, 3, 3, 3),
            ColorBoxOnHover = true,
            BoxOnHoverBrush = ElementBackgroundBrush
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
            BoxOnHoverBrush = ElementBackgroundBrush
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
            BoxOnHoverBrush = ElementBackgroundBrush
        };

        private readonly CrossIcon CloseButton = new()
        {
            Dock = DockStyle.Right,
            IconPaddingFactor = 4.3f,
            Location = new Point(3, 0),
            Name = "CloseButton",
            Size = new Size(ElementSize, ElementSize),
            LineColor = ElementColor,
            LineWidth = ElementWidth,
            ColorBoxOnHover = true,
            BoxOnHoverBrush = CloseButtonBackgroundBrush
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
            }
        }
        private FormBase _form;
        private bool mouseDown;
        private Point lastLocation;


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
            MinimumSize = new Size(0, 25);
            MaximumSize = new Size(int.MaxValue, 25);
            Dock = DockStyle.Top;
            TabIndex = 1;
            BackColor = Color.FromArgb(28, 28, 28);

            MouseDown += Drag;
            TitleText.MouseDown += Drag;
            ProgramIconBox.MouseDown += Drag;
        }
        #endregion

        private void InitForm()
        {
            Form.ResizableChanged += (object _, bool resizable) => ChangeSizingElementsVisibility(resizable);
            CloseButton.Click += (object _, EventArgs _) => Form.Close();
            MinimizeButton.Click += (object _, EventArgs _) => Form.WindowState = FormWindowState.Minimized;
            MaximizeButton.Click += (object _, EventArgs _) => Form.WindowState =
            Form.WindowState == FormWindowState.Maximized
            ? FormWindowState.Normal
            : FormWindowState.Maximized;
        }

        private void ChangeSizingElementsVisibility(bool visible)
        {
            Pin.Visible = visible;
            MinimizeButton.Visible = visible;
            MaximizeButton.Visible = visible;
        }

        #region Importing DLLs to enable dragging
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        #endregion

        private void Drag(object sender, MouseEventArgs e)
        {
            if (Form is null)
                return;

            const int WM_NCLBUTTONDOWN = 0xA1;
            const int HT_CAPTION = 0x2;

            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                _ = SendMessage(Form.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mouseDown = true;
            lastLocation = e.Location;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (mouseDown)
            {
                Location = new(Location.X - lastLocation.X + e.X, Location.Y - lastLocation.Y + e.Y);
                Update();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouseDown = false;
        }
    }
}
