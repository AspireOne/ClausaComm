using System;
using System.Drawing;
using System.Windows.Forms;
using ClausaComm.Components;

namespace ClausaComm.Forms
{
    public class FormBase : Form
    {
        protected static readonly Padding DraggableWindowBorderSize = new(3, 3, 3, 3);
        protected static readonly Padding NonDraggableWindowBorderSize = new(3, 3, 3, 3);

        protected readonly TitleBar TitleBar = new()
        {
            Dock = DockStyle.Top,
            Location = new Point(0, 0),
            Title = "ClausaComm"
        };
        public event EventHandler<bool> ResizableChanged;
        public event EventHandler<bool> PinnableChanged;

        private bool _resizable = false;
        private bool _pinnable = false;
        private Padding? PaddingBeforeMaximize;

        public new FormWindowState WindowState
        {
            get => base.WindowState;
            set
            {
                base.WindowState = value;
                // If the form is maximized and we allow it to be resized by dragging, unwanted behaviour happens.
                if (value == FormWindowState.Maximized)
                {
                    PaddingBeforeMaximize = Padding;
                    Padding = NonDraggableWindowBorderSize;
                }
                else if (value == FormWindowState.Normal)
                {
                    if (PaddingBeforeMaximize.HasValue)
                        Padding = PaddingBeforeMaximize.Value;
                }
            }
        }

        public bool Resizable
        {
            get => _resizable;
            protected set
            {
                _resizable = value;
                ResizableChanged?.Invoke(this, value);
            }
        }

        public bool Pinnable
        {
            get => _pinnable;
            protected set
            {
                _pinnable = value;
                PinnableChanged?.Invoke(this, value);
            }
        }

        protected void InitTitleBar(FormBase form, string title = "ClausaComm")
        {
            TitleBar.Form = form;
            TitleBar.Title = title;
            form.Controls.Add(TitleBar);
        }

        protected FormBase()
        {
            DoubleBuffered = true;
            Padding = Resizable ? DraggableWindowBorderSize : NonDraggableWindowBorderSize;
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
            BackColor = Constants.UiConstants.TitleBarColor; 

            /*
            DraggableSpace = new(false)
            {
                Opacity = 1,
                BackColor = Color.Red,
                FormBorderStyle = FormBorderStyle.None,
                ShowInTaskbar = false,
                Resizable = true,
            };

            const byte offset = 50;
            DraggableSpace.Size = new Size(Size.Width + offset, Size.Height + offset);
            this.LocationChanged += (_, _) => DraggableSpace.Location = new(this.Location.X - (offset / 2), this.Location.Y - (offset / 2));
            DraggableSpace.SizeChanged += (_, _) => Size = DraggableSpace.Size - new Size(offset, offset);
            */
        }

        // Allows the form to be hid and shown by clicking on it's taskbar icon.
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x20000;

                CreateParams cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            /*
            * Modified from https://stackoverflow.com/questions/17748446/custom-resize-handle-in-border-less-form-c-sharp
            * removed boilerplate, changed Dictionary to pair array & created variables for repeating math operations.
            */

            // The values can be found at https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-nchittest
            const uint WM_NCHITTEST = 0x0084;
            const uint WM_MOUSEMOVE = 0x0200;

            const uint HTLEFT = 10;
            const uint HTRIGHT = 11;
            const uint HTBOTTOMRIGHT = 17;
            const uint HTBOTTOM = 15;
            const uint HTBOTTOMLEFT = 16;
            const uint HTTOP = 12;
            const uint HTTOPLEFT = 13;
            const uint HTTOPRIGHT = 14;

            const int resizeHandleSize = 10;
            const int resizeHandleDoubled = resizeHandleSize * 2;

            var handled = false;

            if (Resizable && (m.Msg == WM_NCHITTEST || m.Msg == WM_MOUSEMOVE))
            {
                Point screenPoint = new(m.LParam.ToInt32());
                Point clientPoint = PointToClient(screenPoint);
                int heightMinusResizeHandle = Size.Height - resizeHandleSize;
                int widthMinusResizeHandle = Size.Width - resizeHandleSize;
                int widthMinusResizeHandleDoubled = Size.Width - resizeHandleDoubled;
                int heightMinusResizeHandleDoubled = Size.Height - resizeHandleDoubled;

                var boxes = new (uint pos, Rectangle rect)[]
                {
                    (HTBOTTOMLEFT, new Rectangle(0, heightMinusResizeHandle, resizeHandleSize, resizeHandleSize)),
                    (HTBOTTOM, new Rectangle(resizeHandleSize, heightMinusResizeHandle, widthMinusResizeHandleDoubled, resizeHandleSize)),
                    (HTBOTTOMRIGHT, new Rectangle(widthMinusResizeHandle, heightMinusResizeHandle, resizeHandleSize, resizeHandleSize)),
                    (HTRIGHT, new Rectangle(widthMinusResizeHandle, resizeHandleSize, resizeHandleSize, heightMinusResizeHandleDoubled)),
                    (HTTOPRIGHT, new Rectangle(widthMinusResizeHandle, 0, resizeHandleSize, resizeHandleSize)),
                    (HTTOP, new Rectangle(resizeHandleSize, 0, widthMinusResizeHandleDoubled, resizeHandleSize)),
                    (HTTOPLEFT, new Rectangle(0, 0, resizeHandleSize, resizeHandleSize)),
                    (HTLEFT, new Rectangle(0, resizeHandleSize, resizeHandleSize, heightMinusResizeHandleDoubled))
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