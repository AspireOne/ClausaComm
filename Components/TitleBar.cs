﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using ClausaComm.Components.Icons;
using ClausaComm.Extensions;
using ClausaComm.Forms;

namespace ClausaComm.Components
{
    public partial class TitleBar : Panel
    {
        public readonly ToolTip Tooltip = new()
        {
            AutoPopDelay = 1500,
            InitialDelay = 500,
            ReshowDelay = 10000,
        };
        public string Title
        {
            get => TitleText.Text;
            set => TitleText.Text = value;
        }


        #region title bar elements initialization
        private const byte ElementSize = 24;
        private const byte ElementWidth = 1;
        private const byte BoxColorOpacity = 40;
        private static readonly Color ElementColor = Color.FromArgb(182, 182, 182);
        private static readonly Color AdditionalElementColor = Color.FromArgb(244, 244, 244);
        private static readonly Color CloseButtonColor = Color.FromArgb(250, 250, 250);
        private static readonly Color ElementColorOnHover = Color.FromArgb(255, 255, 255);
        private static readonly SolidBrush ElementBackgroundBrush = new(Color.FromArgb(BoxColorOpacity, Color.FromArgb(225, 255, 255)));
        private static readonly SolidBrush CloseButtonBackgroundBrush = new(Color.FromArgb(240, 255, 51, 58));
        // The "Location" property indicates in what order the elements should be. 0 = first.
        private readonly Label TitleText = new()
        {
            Font = new Font("Segoe UI", 11f, FontStyle.Regular, GraphicsUnit.Point),
            Location = new Point(1, 0),
            Dock = DockStyle.Bottom,
            Margin = new Padding(6, 0, 3, 0),
            Name = "TitleText",
            Text = "ClausaComm",
            TextAlign = ContentAlignment.MiddleCenter,
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

        public void AddAdditionalElement(IconBase element)
        {
            element.Dock = DockStyle.Left;
            element.Width = ElementSize;
            element.Height = ElementSize;
            if (element is ImageIconBase imageIcon)
                imageIcon.IconColor = AdditionalElementColor;
            else if (element is LineIconBase lineIcon)
                lineIcon.LineColor = AdditionalElementColor;
            Controls.Add(element);
        }

        private void InitializeComponentFurther()
        {
            Controls.Add(TitleText);
            //Controls.Add(Pin);
            Controls.Add(MinimizeButton);
            Controls.Add(MaximizeButton);
            Controls.Add(CloseButton);
            Padding = new Padding(5, 0, 0, 0);

            Name = "TitleBar";
            MinimumSize = new Size(0, BarHeight);
            MaximumSize = new Size(int.MaxValue, BarHeight);
            BackColor = Constants.UiConstants.TitleBarColor;

            MouseDown += Drag;
            TitleText.MouseDown += Drag;
            Pin.Click += (_, _) =>
            {
                if (Form?.Pinnable == true)
                    Form.Pinned = true;
            };
        }
        #endregion

        private void InitForm()
        {
            Form.ResizableChanged += (_, resizable) => ChangeSizingElementsVisibility(resizable);
            Form.PinnableChanged += (_, pinnable) => Pin.Visible = pinnable;
            CloseButton.Click += (_, _) =>
            {
                if (Form.Pinnable)
                    Form.Pinned = true;
                else
                    Form.Close();
            };
            MinimizeButton.Click += (_, _) => Form.WindowState = FormWindowState.Minimized;
            MaximizeButton.Click += (_, _) => Form.WindowState =
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
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        #endregion

        private void Drag(object sender, MouseEventArgs e)
        {
            if (Form is null || Form.WindowState == FormWindowState.Maximized || !Form.Draggable)
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
