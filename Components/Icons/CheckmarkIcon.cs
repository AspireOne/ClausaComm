using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ClausaComm.Components.Icons
{
    public partial class CheckmarkIcon : LineIconBase
    {
        private const int ShorterSideOffsetY = 8;
        private const int ShorterSideOffsetX = 5;
        public CheckmarkIcon() : base()
        {
            ShowCircle = true;
        }

        public CheckmarkIcon(IContainer container) : base(container) { }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            DrawCheckmark(pe.Graphics);
        }

        private void DrawCheckmark(Graphics g)
        {
            //                 /
            //                /
            //          \    / a
            //         b \  /
            //            \/  
            //         -->  <-- They both start at this point

            Point start = new(Width / 2, Height - IconPadding);

            Point aEnd = new(Width - IconPadding, IconPadding);
            Point bEnd = new(IconPadding + ShorterSideOffsetX, IconPadding + ShorterSideOffsetY);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawLine(CurrentLineAppearance, start, aEnd);
            g.DrawLine(CurrentLineAppearance, start, bEnd);
        }
    }
}
