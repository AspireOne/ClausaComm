using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ClausaComm.Components.Icons
{
    public partial class MinimizeIcon : LineIconBase
    {
        public MinimizeIcon() : base() { }

        public MinimizeIcon(IContainer container) : base(container) { }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            DrawLine(pe.Graphics);
        }

        private void DrawLine(Graphics g)
        {
            int heightDivided = Height / 2;
            Point start = new(IconPadding, heightDivided);
            Point end = new(Width - IconPadding, heightDivided);
            g.DrawLine(CurrentLineAppearance, start, end);
        }
    }
}
