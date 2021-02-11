using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using ClausaComm.Utils;

namespace ClausaComm.Components.Icons
{
    public partial class SaveIcon : ImageIconBase
    {
        public Pen CrossLineAppearance { get; set; } = new(Color.FromArgb(170, Color.WhiteSmoke), 2);
        public Padding CrossPadding { get; set; } = new(3, 3, 3, 3);

        public enum State { Save, Unsave }

        private State _currState = State.Save;

        public State CurrState
        {
            get => _currState;
            set
            {
                _currState = value;
                Invalidate();
            }
        }

        public SaveIcon() : base(Properties.Resources.save_icon)
        {
            CrossLineAppearance.Color = Color.FromArgb(170, IconColor);
            /*
             * This is a workaround for the Image bizzarely not displaying correctly until it's clicked on or hovered over.
             * Executing this code on the current thread does not solve it.
             */
            // TODO: Fix this (Save Image not displaying correctly until clicked on or hovered over - replace the current workaround).
            ThreadUtils.RunThread(() => { Thread.Sleep(50); Image = new Bitmap(Image); });
        }

        public SaveIcon(IContainer container) : base(container) { }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (CurrState == State.Unsave)
                DrawCross(pe.Graphics);
        }

        private void DrawCross(Graphics g)
        {
            Padding crossPadding = Padding + CrossPadding;

            Point aStart = new(crossPadding.Left, crossPadding.Top);
            Point aEnd = new(Width - crossPadding.Right, Height - crossPadding.Bottom);

            Point bStart = new(Width - crossPadding.Right, crossPadding.Top);
            Point bEnd = new(crossPadding.Left, Height - crossPadding.Bottom);

            g.DrawLine(CrossLineAppearance, aStart, aEnd);
            g.DrawLine(CrossLineAppearance, bStart, bEnd);
        }
    }
}
