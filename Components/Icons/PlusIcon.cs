using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClausaComm.Components.Icons
{
    public partial class PlusIcon : LineIconBase
    {

        public PlusIcon(IContainer container) : base(container) { }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            DrawPlus(pe.Graphics);
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
    }
}
