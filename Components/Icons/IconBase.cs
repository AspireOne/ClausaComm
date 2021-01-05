using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClausaComm.Components.Icons
{
    public partial class IconBase : PictureBox
    {
        public static readonly Color DefaultIconColor = Color.White;
        public bool UnderlineOnHover { get; set; } = false;
        public Pen UnderlineAppearance { get; set; } = new(Brushes.White, 3);

        protected bool IsHovering { get; private set; } = false;
        protected bool IsMouseDown { get; private set; } = false;

        // Hide the base class's Image property and change accessor to protected, so that it's not changeable from the outside.
        new public Image Image
        {
            get => base.Image;
            protected set => base.Image = value;
        }

        public bool ColorIconOnHover { get; set; } = false;
        public bool ColorIconOnClick { get; set; } = false;

        public SolidBrush BoxOnHoverBrush { get; set; } = new SolidBrush(Color.Transparent);
        public bool ColorBoxOnHover { get; set; } = false;


        protected IconBase()
        {
            InitializeComponent();
            SizeMode = PictureBoxSizeMode.StretchImage;
            Cursor = Cursors.Hand;
        }

        protected IconBase(IContainer container) : this() => container.Add(this);

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (IsHovering)
            {
                if (UnderlineOnHover)
                {
                    Point horizontalLineStart = new(0, Height);
                    Point horizontalLineStop = new(Width, Height);
                    pe.Graphics.DrawLine(UnderlineAppearance, horizontalLineStart, horizontalLineStop);
                }

                if (ColorBoxOnHover)
                {
                    Rectangle rect = new Rectangle(0, 0, Width, Height);
                    pe.Graphics.FillRectangle(BoxOnHoverBrush, ClientRectangle);
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            IsHovering = true;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            IsMouseDown = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            IsMouseDown = false;
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            IsHovering = false;
            Invalidate();
        }
    }
}
