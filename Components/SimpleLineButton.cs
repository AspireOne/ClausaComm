using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClausaComm.Components
{
    public partial class SimpleLineButton : Label
    {
        private bool IsHovering;
        private static readonly Color DefaultLineColor = Color.White;
        public bool ColorLineOnHover { get; set; } = true;
        private static readonly int DefaultLineWidth = 3;

        private Pen Line { get; set; } = new(DefaultLineColor, DefaultLineWidth);
        private Pen LineOnHover { get; set; } = new(Constants.UIConstants.ElementOnHover.Color, DefaultLineWidth);

        public float LineWidth
        {
            get => Line.Width;
            set
            {
                Line.Width = value;
                LineOnHover.Width = value;
                Invalidate();
            }
        }

        public Color LineColor
        {
            get => Line.Color;
            set
            {
                Line.Color = Constants.UIConstants.ReturnNewOrDefaultColor(DefaultLineColor, value);
                Invalidate();
            }
        }

        public Color LineColorOnHover
        {
            get => LineOnHover.Color;
            set
            {
                LineOnHover.Color = Constants.UIConstants.ReturnNewOrDefaultColor(Constants.UIConstants.ElementOnHover.Color, value);
                Invalidate();
            }
        }



        public SimpleLineButton()
        {
            InitializeComponent();
            TextAlign = ContentAlignment.TopCenter;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            IsHovering = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            IsHovering = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Point horizontalLineStart = new(0, Height);
            Point horizontalLineStop = new(Width, Height);
            e.Graphics.DrawLine(IsHovering && ColorLineOnHover ? LineOnHover : Line, horizontalLineStart, horizontalLineStop);
        }

        public SimpleLineButton(IContainer container) : this() => container.Add(this);
    }
}
