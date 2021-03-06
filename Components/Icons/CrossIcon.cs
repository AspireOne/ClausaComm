﻿using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ClausaComm.Components.Icons
{
    public partial class CrossIcon : LineIconBase
    {

        public CrossIcon() : base() { }
        public CrossIcon(IContainer container) : base(container) { }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            DrawCross(pe.Graphics);
        }

        private void DrawCross(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Point aStart = new(IconPadding, IconPadding);
            Point aEnd = new(Width - IconPadding, Height - IconPadding);

            Point bStart = new(Width - IconPadding, IconPadding);
            Point bEnd = new(IconPadding, Height - IconPadding);

            g.DrawLine(CurrentLineAppearance, aStart, aEnd);
            g.DrawLine(CurrentLineAppearance, bStart, bEnd);
        }
    }
}
