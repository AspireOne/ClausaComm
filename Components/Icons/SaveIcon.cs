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
    public partial class SaveIcon : ImageIconBase
    {
        public Pen CrossLineAppearance { get; set; } = new Pen(Color.White, 2);
        public Padding CrossPadding { get; set; } = new Padding(3, 3, 3, 3);

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
