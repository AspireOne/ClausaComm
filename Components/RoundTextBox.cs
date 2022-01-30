using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace ClausaComm.Components
{
    public sealed class RoundTextBox : Control
    {
        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                if (value != Color.Transparent)
                {
                    Textbox.BackColor = value;
                    Textbox.Invalidate();
                }
            }
        }

        public Color OnHoverBorderColor
        {
            get => _onHoverBorderColor;
            set
            {
                _onHoverBorderColor = Constants.UiConstants.ReturnNewOrDefaultColor(Constants.UiConstants.ElementOnHoverColor, value);
                Invalidate();
            }
        }

        public bool ReadOnly
        {
            get => Textbox.ReadOnly;
            set => Textbox.ReadOnly = value;
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
            get => Textbox.MaxLength;
            set => Textbox.MaxLength = value;
        }

        public bool Multiline
        {
            get => Textbox.Multiline;
            set => Textbox.Multiline = value;
        }

        private int Radius = 15;
        public bool ColorBorderOnHover { get; set; } = true;
        public TextBox Textbox = new();
        private GraphicsPath Shape;
        private GraphicsPath InnerRect;
        private readonly Color Br;
        private Color _borderColor = Color.Transparent;
        private Color _onHoverBorderColor = Constants.UiConstants.ElementOnHoverColor;
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
            Br = Color.White;
            DoubleBuffered = true;

            Textbox.Parent = this;
            Controls.Add(Textbox);
            Textbox.BorderStyle = BorderStyle.None;
            Textbox.BackColor = Br;
            Textbox.Font = Font;
            Textbox.KeyDown += TextBox_KeyDown;
            Textbox.TextChanged += TextBox_TextChanged;
            Textbox.MouseDoubleClick += TextBox_MouseDoubleClick;
            Textbox.KeyPress += TextBox_KeyPress;
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }

        private void TextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Textbox.SelectAll();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            Text = Textbox.Text;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                Textbox.SelectionStart = 0;
                Textbox.SelectionLength = Text.Length;
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            Textbox.Font = Font;
            Invalidate();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            Textbox.ForeColor = ForeColor;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            Shape = new RoundTextBoxRect(Width, Height, Radius).Path;
            InnerRect = new RoundTextBoxRect(Width - 0.5f, Height - 0.5f, Radius, 0.5f, 0.5f).Path;

            if (Textbox.Height >= Height - 4)
                Height = Textbox.Height + 4;

            Textbox.Location = new Point(Radius - 5, (Height / 2) - (Textbox.Font.Height / 2));
            Textbox.Width = Width - (int)(Radius * 1.5);

            Pen pp = new(BorderColor, BorderSize);
            e.Graphics.DrawPath(pp, Shape);

            using (SolidBrush brush = new(Br))
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
                    RectangleF ef = new(x, y, radiusMultiplied, radiusMultiplied);
                    RectangleF ef2 = new(roundWidth, x, radiusMultiplied, radiusMultiplied);
                    RectangleF ef3 = new(x, roundHeight, radiusMultiplied, radiusMultiplied);
                    RectangleF ef4 = new(roundWidth, roundHeight, radiusMultiplied, radiusMultiplied);
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
