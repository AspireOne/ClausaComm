using System.ComponentModel;
using System.Drawing;
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
            int halfHeight = Height / 2;
            int halfWidth = Width / 2;

            Point horizontalLineStart = new(IconPadding, halfHeight);
            Point horizontalLineStop = new(Width - IconPadding, halfHeight);

            Point verticalLineStart = new(halfWidth, IconPadding);
            Point verticalLineStop = new(halfWidth, Height - IconPadding);

            g.DrawLine(CurrentLineAppearance, horizontalLineStart, horizontalLineStop);
            g.DrawLine(CurrentLineAppearance, verticalLineStart, verticalLineStop);
        }
    }
}
