using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClausaComm.Components
{
    public class RoundButton : Button
    {
        public Color BorderColor { get; set; } = Color.Transparent;
        public Color OnHoverBorderColor { get; set; } = Color.Transparent;
        public Color ButtonColor { get; set; } = Color.Gray;
        public Color OnHoverButtonColor { get; set; } = UIConstants.ElementOnHover.Color;
        public Color OnClickColor { get; set; } = UIConstants.ElementOnClick.Color;
        public Color TextColor { get; set; } = Color.Black;
        public int BorderThickness
        {
            get => _borderThickness;
            set
            {
                _borderThickness = value;
                BorderThicknessDividedByTwo = value / 2;
            }
        }

        private bool IsHovering;
        private bool IsClicking;
        private int _borderThickness = 3;
        private int BorderThicknessDividedByTwo = 1;
        // Creating our own OnDown event because we cannot override the inherited one and we need specific behavior.
        public event EventHandler OnDown;


        public RoundButton()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BorderColor = BackColor;
            DoubleBuffered = true;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            // Not calling base so that the Button doesn't highlight the background.
            IsHovering = true;
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            // Not calling base because we don't call base at mouse enter.
            IsHovering = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Not calling base so that the Button doesn't highlight the background.
            OnDown.Invoke(this, EventArgs.Empty);
            IsClicking = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            IsClicking = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Brush brush = new SolidBrush(IsHovering ? OnHoverBorderColor : BorderColor);

            //Border
            g.FillEllipse(brush, 0, 0, Height, Height);
            g.FillEllipse(brush, Width - Height, 0, Height, Height);
            g.FillRectangle(brush, Height / 2, 0, Width - Height, Height);

            brush.Dispose();
            brush = new SolidBrush(IsClicking ? OnClickColor : IsHovering ? OnHoverButtonColor : ButtonColor);

            int HeightMinusBorder = Height - BorderThickness;
            g.FillEllipse(brush, BorderThicknessDividedByTwo, BorderThicknessDividedByTwo, HeightMinusBorder, HeightMinusBorder);
            g.FillEllipse(brush, Width - Height + BorderThicknessDividedByTwo, BorderThicknessDividedByTwo, HeightMinusBorder, HeightMinusBorder);
            g.FillRectangle(brush, (Height / 2) + BorderThicknessDividedByTwo, BorderThicknessDividedByTwo, Width - HeightMinusBorder, HeightMinusBorder);

            brush.Dispose();
            brush = new SolidBrush(TextColor);

            //Button Text
            SizeF stringSize = g.MeasureString(Text, Font);
            g.DrawString(Text, Font, brush, (Width - stringSize.Width) / 2, (Height - stringSize.Height) / 2);

            brush.Dispose();
        }
    }
}