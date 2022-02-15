using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClausaComm.Components
{
    public partial class SelectableTextBox : TextBox
    {
        public SelectableTextBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            //base.OnMouseWheel(e);
        }
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        const int WM_MOUSEWHEEL = 0x020A;

        //thanks to a-clymer's solution
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEWHEEL)
            {
                //directly send the message to parent without processing it
                //according to https://stackoverflow.com/a/19618100
                SendMessage(this.Parent.Handle, m.Msg, m.WParam, m.LParam);
                m.Result = IntPtr.Zero;
            }
            else base.WndProc(ref m);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent is null)
                return;
            
            BackColor = Parent.BackColor;
            Parent.BackColorChanged += (_, ev) => BackColor = Parent.BackColor;
        }

        [DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hWnd);

        private void HideCaret()
        {
            HideCaret(Handle);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            AdjustHeightToText();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            AdjustHeightToText();
        }

        private void AdjustHeightToText()
        {
            // amount of padding to add
            const int padding = 3;
            // get number of lines (first line is 0, so add 1)
            int numLines = GetLineFromCharIndex(TextLength) + 1;
            // get border thickness
            int border = Height - ClientSize.Height;
            // set height (height of one line * number of lines + spacing)
            Height = Font.Height * numLines + padding + border;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            //base.OnGotFocus(e);
            HideCaret();
        }

        public SelectableTextBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
    }
}