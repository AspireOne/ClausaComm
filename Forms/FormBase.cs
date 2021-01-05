using ClausaComm.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClausaComm.Forms
{
    public /*abstract*/ partial class FormBase : Form
    {
        protected static readonly Padding DraggableWindowBorderSize = new(1, 1, 1, 1);
        protected static readonly Padding NonDraggableWindowBorderSize = new(0, 0, 0, 0);
        public event EventHandler<bool> ResizableChanged;
        private bool _resizable = false;
        public bool Resizable
        {
            get => _resizable;
            protected set
            {
                _resizable = value;
                Padding = Resizable ? DraggableWindowBorderSize : NonDraggableWindowBorderSize;
                ResizableChanged?.Invoke(this, value);
            }
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            /*
            * Modified from https://stackoverflow.com/questions/17748446/custom-resize-handle-in-border-less-form-c-sharp
            * changed Dictionary to pair array & created variables for repeating math operations.
            */

            const uint WmNchitTest = 0x0084;
            const uint WmMouseMove = 0x0200;

            const uint HTLeft = 10;
            const uint HTRight = 11;
            const uint HTBottomRight = 17;
            const uint HTBottom = 15;
            const uint HTBottomLeft = 16;
            const uint HTTop = 12;
            const uint HTTopLeft = 13;
            const uint HTTopRight = 14;

            const int ResizeHandleSize = 10;
            const int ResizeHandleDoubled = ResizeHandleSize * 2;

            bool handled = false;

            if (Resizable && (m.Msg == WmNchitTest || m.Msg == WmMouseMove))
            {
                Point screenPoint = new Point(m.LParam.ToInt32());
                Point clientPoint = PointToClient(screenPoint);
                int heightMinusResizeHandle = Size.Height - ResizeHandleSize;
                int widthMinusResizeHandle = Size.Width - ResizeHandleSize;
                int widthMinusResizeHandleDoubled = Size.Width - ResizeHandleDoubled;
                int heightMinusResizeHandleDoubled = Size.Height - ResizeHandleDoubled;

                var boxes = new (uint pos, Rectangle rect)[]
                {
                    (HTBottomLeft, new Rectangle(0, heightMinusResizeHandle, ResizeHandleSize, ResizeHandleSize)),
                    (HTBottom, new Rectangle(ResizeHandleSize, heightMinusResizeHandle, widthMinusResizeHandleDoubled, ResizeHandleSize)),
                    (HTBottomRight, new Rectangle(widthMinusResizeHandle, heightMinusResizeHandle, ResizeHandleSize, ResizeHandleSize)),
                    (HTRight, new Rectangle(widthMinusResizeHandle, ResizeHandleSize, ResizeHandleSize, heightMinusResizeHandleDoubled)),
                    (HTTopRight, new Rectangle(widthMinusResizeHandle, 0, ResizeHandleSize, ResizeHandleSize)),
                    (HTTop, new Rectangle(ResizeHandleSize, 0, widthMinusResizeHandleDoubled, ResizeHandleSize)),
                    (HTTopLeft, new Rectangle(0, 0, ResizeHandleSize, ResizeHandleSize)),
                    (HTLeft, new Rectangle(0, ResizeHandleSize, ResizeHandleSize, heightMinusResizeHandleDoubled))
                };

                for (var i = 0; i < boxes.Length; ++i)
                {
                    if (boxes[i].rect.Contains(clientPoint))
                    {
                        m.Result = (IntPtr)boxes[i].pos;
                        handled = true;
                        break;
                    }
                }
            }

            if (!handled)
                base.WndProc(ref m);
        }
    }
}
