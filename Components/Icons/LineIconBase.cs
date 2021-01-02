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
    public partial class LineIconBase : IconBase
    {
        public float IconPaddingFactor { get; set; } = 3.4f;
        protected int IconPadding => (int)(Size.Width / IconPaddingFactor);

        #region Circle
        public Color CircleColor
        {
            get => CircleBrush.Color;
            set
            {
                CircleBrush.Color = Constants.UIConstants.ReturnNewOrDefaultColor(DefaultIconColor, value);
                Invalidate();
            }
        }

        public Color HoverCircleColor
        {
            get => HoverCircleBrush.Color;
            set
            {
                HoverCircleBrush.Color = Constants.UIConstants.ReturnNewOrDefaultColor(Constants.UIConstants.ElementOnHover.Color, value);
                Invalidate();
            }
        }

        public Color ClickCircleColor
        {
            get => ClickCircleBrush.Color;
            set
            {
                ClickCircleBrush.Color = Constants.UIConstants.ReturnNewOrDefaultColor(Constants.UIConstants.ElementOnClick.Color, value);
                Invalidate();
            }
        }

        protected SolidBrush HoverCircleBrush = new(Constants.UIConstants.ElementOnHover.Color);
        protected SolidBrush ClickCircleBrush = new(Constants.UIConstants.ElementOnClick.Color);
        protected SolidBrush CircleBrush = new(DefaultIconColor);

        protected SolidBrush CurrentCircleBrush
        {
            get
            {
                if (ColorCircleOnClick && IsClicking)
                    return ClickCircleBrush;

                if (ColorCircleOnHover && IsHovering)
                    return HoverCircleBrush;

                return CircleBrush;
            }
        }
        public bool ColorCircleOnHover { get; set; } = false;
        public bool ColorCircleOnClick { get; set; } = false;
        public bool ShowCircle { get; set; } = false;
        #endregion

        #region Lines
        // Making two separate accessors so that we can set in the designer.
        public float LineWidth
        {
            get => LineAppearance.Width;
            set
            {
                LineAppearance.Width = value;
                Invalidate();
            }
        }

        public Color LineColor
        {
            get => LineAppearance.Color;
            set
            {
                LineAppearance.Color = Constants.UIConstants.ReturnNewOrDefaultColor(DefaultLineColor, value);
                Invalidate();
            }
        }

        public Color HoverLineColor
        {
            get => HoverLineAppearance.Color;
            set
            {
                HoverLineAppearance.Color = Constants.UIConstants.ReturnNewOrDefaultColor(DefaultLineHoverColor, value);
                Invalidate();
            }
        }

        public Color ClickLineColor
        {
            get => ClickLineAppearance.Color;
            set
            {
                ClickLineAppearance.Color = Constants.UIConstants.ReturnNewOrDefaultColor(Constants.UIConstants.ElementOnClick.Color, value);
                Invalidate();
            }
        }

        public bool ColorLinesOnHover { get; set; } = false;
        public bool ColorLinesOnClick { get; set; } = false;

        protected Pen LineAppearance { get; set; } = new(Color.Gray, DefaultLineWidth);
        protected Pen HoverLineAppearance { get; set; } = new(Color.Gray, DefaultLineWidth);
        protected Pen ClickLineAppearance { get; set; } = new(Constants.UIConstants.ElementOnClick.Color, DefaultLineWidth);

        /// <summary> Will return pen according to current circumstances. If hovering - hovering pen. If not - normal pen. if clicking - click pen. </summary>

        protected Pen CurrentLineAppearance
        {
            get
            {
                if (ColorLinesOnClick && IsClicking)
                    return ClickLineAppearance;

                if (ColorLinesOnHover && IsHovering)
                    return HoverLineAppearance;

                return LineAppearance;
            }
        }

        protected static readonly Color DefaultLineColor = Color.Gray;
        protected static readonly Color DefaultLineHoverColor = DefaultLineColor;
        protected static readonly int DefaultLineWidth = 2;
        #endregion


        protected LineIconBase() : base() { }
        protected LineIconBase(IContainer container) : base(container) { }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            if (ShowCircle)
                DrawEllipse(pe.Graphics);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected void DrawEllipse(Graphics g)
        {
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            // Ofset is here because PictureBox crops the image otherwise.
            const byte offset = 2;
            g.FillEllipse(CurrentCircleBrush, 0, 0, Width - offset, Height - offset);
        }
    }
}
