using ClausaComm.Components.Icons;
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

namespace ClausaComm.Components
{
    public partial class AddCloseIcon : LineIconBase
    {
        public enum Icon { Cross, Plus }
        public Icon CurrentIcon { get; set; }


        public AddCloseIcon() : base() { }
        public AddCloseIcon(IContainer container) : base(container) { }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (CurrentIcon == Icon.Cross)
                DrawCross(e.Graphics);
            else
                DrawPlus(e.Graphics);
        }

        private void DrawPlus(Graphics g)
        {
            int HalfHeight = Height / 2;
            int HalfWidth = Width / 2;

            Point horizontalLineStart = new(IconPadding, HalfHeight);
            Point horizontalLineStop = new(Width - IconPadding, HalfHeight);

            Point verticalLineStart = new(HalfWidth, IconPadding);
            Point verticalLineStop = new(HalfWidth, Height - IconPadding);

            g.DrawLine(CurrentLineAppearance, horizontalLineStart, horizontalLineStop);
            g.DrawLine(CurrentLineAppearance, verticalLineStart, verticalLineStop);
        }

        private void DrawCross(Graphics g)
        {
            Point aStart = new(IconPadding, IconPadding);
            Point aEnd = new(Width - IconPadding, Height - IconPadding);

            Point bStart = new(Width - IconPadding, IconPadding);
            Point bEnd = new(IconPadding, Height - IconPadding);

            g.DrawLine(CurrentLineAppearance, aStart, aEnd);
            g.DrawLine(CurrentLineAppearance, bStart, bEnd);
        }
    }
}
