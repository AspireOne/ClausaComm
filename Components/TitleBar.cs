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

namespace ClausaComm.Components
{
    public partial class TitleBar : Panel
    {
        public string Title
        {
            get => TitleText.Text;
            set => TitleText.Text = value;
        }

        private bool _showResizeElements = true;

        public bool ShowResizeElements
        {
            get => _showResizeElements;
            set
            {
                _showResizeElements = value;

                Pin.Visible = value;
                MinimizeButton.Visible = value;
                MaximizeButton.Visible = value;
            }
        }

        #region title bar elements initialization
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
        };

        private readonly MaximizeIcon MaximizeButton = new()
        {
            Dock = DockStyle.Right,
            IconPaddingFactor = 4.1f,
            Location = new Point(2, 0),
            Name = "MaximizeButton",
            Size = new Size(ElementSize, ElementSize),
            LineColor = ElementColor,
        };

        private readonly CrossIcon CloseButton = new()
        {
            Dock = DockStyle.Right,
            IconPaddingFactor = 4.3f,
            Location = new Point(3, 0),
            Name = "CloseButton",
            Size = new Size(ElementSize, ElementSize),
            LineColor = ElementColor,
        };
        #endregion

        private const int ElementSize = 24;
        private static readonly Color ElementColor = Color.FromArgb(200, 200, 200);
        public Form Form { get; set /*init*/; }
        private bool mouseDown;
        private Point lastLocation;



        #region constructors
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
        }
        #endregion

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
