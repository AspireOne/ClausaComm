using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ClausaComm.Components.Icons
{
    public partial class IconBase : PictureBox
    {
        // Hide the base class's Image property and change accessor to protected, so that it's not changeable from the outside.
        // the solution above had to be removed because of Windows Forms' terrible designer.
        public new Image Image
        {
            get => base.Image;
            set => base.Image = value;
        }
        public bool UnderlineOnHover { get; set; }
        public Pen UnderlineAppearance { get; set; } = new(Brushes.White, 3);

        protected bool IsHovering { get; private set; }
        protected bool IsMouseDown { get; private set; }

        public bool ColorIconOnHover { get; set; }
        public bool ColorIconOnClick { get; set; }

        public bool ColorBoxOnHover { get; set; }
        public SolidBrush BoxOnHoverBrush { get; set; } = new(Color.Transparent);

        public static readonly Color DefaultIconColor = Color.White;
        /*
        public string Info { get; set; } = "debug, delete this;
        private static readonly Icon InfoIcon = Properties.Resources.info_icon1;
        private const byte InfoIconSize = 12;
        private const byte InfoIconY = 0;
        private int InfoIconX => Width - InfoIconSize;
        private bool IsMouseOverInfoIcon;
        */

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
            if (!IsHovering)
                return;
            
            if (UnderlineOnHover)
            {
                Point horizontalLineStart = new(0, Height);
                Point horizontalLineStop = new(Width, Height);
                pe.Graphics.DrawLine(UnderlineAppearance, horizontalLineStart, horizontalLineStop);
            }

            if (ColorBoxOnHover)
                pe.Graphics.FillRectangle(BoxOnHoverBrush, ClientRectangle);
            /*
            if (!string.IsNullOrEmpty(Info))
            {
                var rect = new Rectangle(InfoIconX - 2, InfoIconY, InfoIconSize - 2, InfoIconSize);
                Pen p = new(new SolidBrush(Color.White), 1);
                pe.Graphics.DrawEllipse(p, rect);
                // pe.Graphics.DrawIcon(InfoIcon, new Rectangle(InfoIconX, InfoIconY, InfoIconSize, InfoIconSize));
                pe.Graphics.DrawImage(InfoIcon, new Rectangle(InfoIconX, InfoIconY, InfoIconSize, InfoIconSize));
            }
            */
        }
        /*
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            IsMouseOverInfoIcon = e.X >= InfoIconX && e.X <= InfoIconX + InfoIconSize && e.Y >= InfoIconY && e.Y <= InfoIconY + InfoIconSize;
            //OnPaint(new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle()));
            Invalidate();
        }
        */

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
