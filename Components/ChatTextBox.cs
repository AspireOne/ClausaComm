using System;
using System.Windows.Forms;
using Message = ClausaComm.Messages.Message;

namespace ClausaComm.Components
{
    public sealed partial class ChatTextBox : TextBox
    {
        #region backing properties

        private int _nLeftRect = 2;
        private int _nTopRect = 3;
        private int _nHeightRect = 15;
        private int _nWeightRect = 15;

        #endregion backing properties

        #region properties

        public int RectLeft
        {
            get => _nLeftRect;
            set
            {
                _nLeftRect = value;
                Invalidate();
            }
        }

        public int RectTop
        {
            get => _nTopRect;
            set
            {
                _nTopRect = value;
                Invalidate();
            }
        }

        public int RectHeight
        {
            get => _nHeightRect;
            set
            {
                _nHeightRect = value;
                Invalidate();
            }
        }

        public int RectWidth
        {
            get => _nWeightRect;
            set
            {
                _nWeightRect = value;
                Invalidate();
            }
        }

        #endregion properties

        // TODO: Try to add border / adjust vertical alignment and/or try to change placeholder text's position
        public ChatTextBox()
        {
            MaxLength = Message.MaxTextLength - 2;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // X-coordinate of upper-left corner or padding at start.
            int nTopRect, // Y-coordinate of upper-left corner or padding at the top of the textbox.
            int nRightRect, // X-coordinate of lower-right corner or Width of the object.
            int nBottomRect,// Y-coordinate of lower-right corner or Height of the object.
            int nheightRect, // Height of ellipse.
            int nwidthRect // Width of ellipse.
        );

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(RectLeft, RectTop, Width, Height, RectHeight, RectWidth));
        }
    }
}