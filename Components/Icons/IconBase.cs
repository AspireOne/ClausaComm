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

        protected bool IsHovering = false;
        protected bool IsClicking = false;

        // Hide the base class's Image property and change accessor to protected, so that it's not changeable.
        new protected Image Image
        {
            get => base.Image;
            set => base.Image = value;
        }


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
            if (IsHovering && UnderlineOnHover)
            {
                Point horizontalLineStart = new(0, Height);
                Point horizontalLineStop = new(Width, Height);
                pe.Graphics.DrawLine(UnderlineAppearance, horizontalLineStart, horizontalLineStop);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            //Cursor = Cursors.Hand;
            IsHovering = true;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            IsClicking = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            IsClicking = false;
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            //Cursor = Cursors.Default;
            IsHovering = false;
            Invalidate();
        }
    }
}
