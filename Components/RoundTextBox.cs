using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace ClausaComm.Components
{
    public class RoundTextBox : Control
    {
        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                if (value != Color.Transparent)
                {
                    textbox.BackColor = value;
                    textbox.Invalidate();
                }
            }
        }

        public Color OnFocusBorderColor
        {
            get => _onFocusBorderColor;
            set
            {
                _onFocusBorderColor = UIConstants.ReturnNewOrDefaultColor(UIConstants.ElementOnHover.Color, value);
                Invalidate();
            }
        }

        public bool ReadOnly
        {
            get => textbox.ReadOnly;
            set => textbox.ReadOnly = value;
        }
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }
        public int TextboxRadius
        {
            get => Radius;
            set { Radius = value; Invalidate(); }
        }
        public int BorderSize
        {
            get => _borderSize;
            set { _borderSize = value; Invalidate(); }
        }

        public int MaxCharacters
        {
            get => textbox.MaxLength;
            set => textbox.MaxLength = value;
        }

        public bool Multiline
        {
            get => textbox.Multiline;
            set => textbox.Multiline = value;
        }

        private int Radius = 15;
        public bool ColorBorderOnFocus { get; set; } = true;
        public TextBox textbox = new();
        private GraphicsPath Shape;
        private GraphicsPath InnerRect;
        private Color br;
        private Color _borderColor = Color.Transparent;
        private Color _onFocusBorderColor = UIConstants.ElementOnHover.Color;
        private int _borderSize = 10;



        //TODO: Fix this whole utterly fucking stupid class


        public RoundTextBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            Text = null;
            Font = new Font("Century Gothic", 12f);
            Size = new Size(135, 33);
            BackColor = Color.Transparent;
            ForeColor = Color.Black;
            br = Color.White;
            DoubleBuffered = true;

            textbox.Parent = this;
            Controls.Add(textbox);
            textbox.BorderStyle = BorderStyle.None;
            textbox.BackColor = br;
            textbox.Font = Font;
            textbox.KeyDown += TextBox_KeyDown;
            textbox.TextChanged += TextBox_TextChanged;
            textbox.MouseDoubleClick += TextBox_MouseDoubleClick;
            textbox.KeyPress += TextBox_KeyPress;
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        private void TextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                textbox.SelectAll();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            Text = textbox.Text;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.A))
            {
                textbox.SelectionStart = 0;
                textbox.SelectionLength = Text.Length;
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            textbox.Font = Font;
            Invalidate();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            textbox.ForeColor = ForeColor;
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            Shape = new RoundTextBoxRect(Width, Height, Radius, 0f, 0f).Path;
            InnerRect = new RoundTextBoxRect(Width - 0.5f, Height - 0.5f, Radius, 0.5f, 0.5f).Path;

            if (textbox.Height >= (Height - 4))
                Height = textbox.Height + 4;

            textbox.Location = new Point(Radius - 5, (Height / 2) - (textbox.Font.Height / 2));
            textbox.Width = Width - ((int)(Radius * 1.5));

            // TODO: Add border on focus
            Pen pp = new Pen(false && ColorBorderOnFocus ? OnFocusBorderColor : BorderColor, _borderSize);
            e.Graphics.DrawPath(pp, Shape);

            using (SolidBrush brush = new SolidBrush(br))
                e.Graphics.FillPath(brush, InnerRect);

            base.OnPaint(e);
        }

        private class RoundTextBoxRect
        {
            public readonly GraphicsPath Path = new();

            public RoundTextBoxRect(float width, float height, float radius, float x = 0, float y = 0)
            {
                if (radius <= 0)
                    Path.AddRectangle(new RectangleF(x, y, width, height));
                else
                {
                    float radiusMultiplied = radius * 2;
                    float roundWidth = width - radiusMultiplied - 1;
                    float roundHeight = height - radiusMultiplied - 1;
                    RectangleF ef = new RectangleF(x, y, radiusMultiplied, radiusMultiplied);
                    RectangleF ef2 = new RectangleF(roundWidth, x, radiusMultiplied, radiusMultiplied);
                    RectangleF ef3 = new RectangleF(x, roundHeight, radiusMultiplied, radiusMultiplied);
                    RectangleF ef4 = new RectangleF(roundWidth, roundHeight, radiusMultiplied, radiusMultiplied);
                    Path.AddArc(ef, 180, 90);
                    Path.AddArc(ef2, 270, 90);
                    Path.AddArc(ef4, 0, 90);
                    Path.AddArc(ef3, 90, 90);
                    Path.CloseAllFigures();
                }
            }
        }
    }
}
