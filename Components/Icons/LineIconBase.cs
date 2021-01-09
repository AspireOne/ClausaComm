using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
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
                CircleBrush.Color = Constants.UIConstants.ReturnNewOrDefaultColor(IconBase.DefaultIconColor, value);
                Invalidate();
            }
        }

        public Color HoverCircleColor
        {
            get => HoverCircleBrush.Color;
            set
            {
                HoverCircleBrush.Color = Constants.UIConstants.ReturnNewOrDefaultColor(Constants.UIConstants.ElementOnHoverColor, value);
                Invalidate();
            }
        }

        public Color ClickCircleColor
        {
            get => ClickCircleBrush.Color;
            set
            {
                ClickCircleBrush.Color = Constants.UIConstants.ReturnNewOrDefaultColor(Constants.UIConstants.ElementOnClickColor, value);
                Invalidate();
            }
        }

        protected SolidBrush HoverCircleBrush = new(Constants.UIConstants.ElementOnHoverColor);
        protected SolidBrush ClickCircleBrush = new(Constants.UIConstants.ElementOnClickColor);
        protected SolidBrush CircleBrush = new(IconBase.DefaultIconColor);

        protected SolidBrush CurrentCircleBrush
        {
            get
            {
                if (ColorCircleOnClick && IsMouseDown)
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
            get => IconAppearance.Width;
            set
            {
                IconAppearance.Width = value;
                HoverIconAppearance.Width = value;
                ClickIconAppearance.Width = value;
                Invalidate();
            }
        }

        public Color LineColor
        {
            get => IconAppearance.Color;
            set
            {
                IconAppearance.Color = Constants.UIConstants.ReturnNewOrDefaultColor(DefaultIconColor, value);
                Invalidate();
            }
        }

        public Color HoverLineColor
        {
            get => HoverIconAppearance.Color;
            set
            {
                HoverIconAppearance.Color = Constants.UIConstants.ReturnNewOrDefaultColor(DefaultIconHoverColor, value);
                Invalidate();
            }
        }

        public Color ClickLineColor
        {
            get => ClickIconAppearance.Color;
            set
            {
                ClickIconAppearance.Color = Constants.UIConstants.ReturnNewOrDefaultColor(Constants.UIConstants.ElementOnClickColor, value);
                Invalidate();
            }
        }

        protected Pen IconAppearance { get; set; } = new(Color.Gray, DefaultLineWidth);
        protected Pen HoverIconAppearance { get; set; } = new(Color.Gray, DefaultLineWidth);
        protected Pen ClickIconAppearance { get; set; } = new(Constants.UIConstants.ElementOnClickColor, DefaultLineWidth);

        /// <summary> Will return pen according to current circumstances. If hovering - hovering pen. If not - normal pen. if clicking - click pen. </summary>

        protected Pen CurrentLineAppearance
        {
            get
            {
                if (ColorIconOnClick && IsMouseDown)
                    return ClickIconAppearance;

                if (ColorIconOnHover && IsHovering)
                    return HoverIconAppearance;

                return IconAppearance;
            }
        }

        protected new static readonly Color DefaultIconColor = Color.Gray;
        protected static readonly Color DefaultIconHoverColor = DefaultIconColor;
        protected const int DefaultLineWidth = 2;
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
