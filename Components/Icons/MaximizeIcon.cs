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
    public partial class MaximizeIcon : LineIconBase
    {

        public enum FormState
        {
            Normal,
            Maximized
        }

        FormState _formState;
        public FormState State
        {
            get => _formState;
            set
            {
                _formState = value;
                Invalidate();
            }
        }

        public MaximizeIcon() : base() { }

        public MaximizeIcon(IContainer container) : base(container) { }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            int width = Width - (IconPadding * 2);
            int height = Height - (IconPadding * 2);

            int spaceX = (Width - width) / 2;
            int spaceY = (Height - height) / 2;

            int leftX = spaceX;
            int rightX = Width - spaceX;

            int topY = spaceY;
            int bottomY = Height - spaceY;

            Point leftLineStart = new(leftX, topY);
            Point leftLineEnd = new(leftX, bottomY);

            Point rightLineStart = new(rightX, topY);
            Point rightLineEnd = new(rightX, bottomY);

            Point topLineStart = new(leftX, topY);
            Point topLineEnd = new(rightX, topY);

            Point bottomLineStart = new(leftX, bottomY);
            Point bottomLineEnd = new(rightX, bottomY);

            var lines = new (Point, Point)[]
            {
                (leftLineStart, leftLineEnd),
                (rightLineStart, rightLineEnd),
                (topLineStart, topLineEnd),
                (bottomLineStart, bottomLineEnd)
            };

            foreach (var pair in lines)
            {
                pe.Graphics.DrawLine(CurrentLineAppearance, pair.Item1, pair.Item2);
            }
        }
    }
}