using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClausaComm.Forms
{
    public /*abstract*/ class FormBase : Form
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

            const uint wmNchitTest = 0x0084;
            const uint wmMouseMove = 0x0200;

            const uint htLeft = 10;
            const uint htRight = 11;
            const uint htBottomRight = 17;
            const uint htBottom = 15;
            const uint htBottomLeft = 16;
            const uint htTop = 12;
            const uint htTopLeft = 13;
            const uint htTopRight = 14;

            const int resizeHandleSize = 10;
            const int resizeHandleDoubled = resizeHandleSize * 2;

            var handled = false;

            if (Resizable && (m.Msg == wmNchitTest || m.Msg == wmMouseMove))
            {
                Point screenPoint = new(m.LParam.ToInt32());
                Point clientPoint = PointToClient(screenPoint);
                int heightMinusResizeHandle = Size.Height - resizeHandleSize;
                int widthMinusResizeHandle = Size.Width - resizeHandleSize;
                int widthMinusResizeHandleDoubled = Size.Width - resizeHandleDoubled;
                int heightMinusResizeHandleDoubled = Size.Height - resizeHandleDoubled;

                var boxes = new (uint pos, Rectangle rect)[]
                {
                    (htBottomLeft, new Rectangle(0, heightMinusResizeHandle, resizeHandleSize, resizeHandleSize)),
                    (htBottom, new Rectangle(resizeHandleSize, heightMinusResizeHandle, widthMinusResizeHandleDoubled, resizeHandleSize)),
                    (htBottomRight, new Rectangle(widthMinusResizeHandle, heightMinusResizeHandle, resizeHandleSize, resizeHandleSize)),
                    (htRight, new Rectangle(widthMinusResizeHandle, resizeHandleSize, resizeHandleSize, heightMinusResizeHandleDoubled)),
                    (htTopRight, new Rectangle(widthMinusResizeHandle, 0, resizeHandleSize, resizeHandleSize)),
                    (htTop, new Rectangle(resizeHandleSize, 0, widthMinusResizeHandleDoubled, resizeHandleSize)),
                    (htTopLeft, new Rectangle(0, 0, resizeHandleSize, resizeHandleSize)),
                    (htLeft, new Rectangle(0, resizeHandleSize, resizeHandleSize, heightMinusResizeHandleDoubled))
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
