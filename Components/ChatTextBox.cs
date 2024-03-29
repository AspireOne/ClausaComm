﻿using System;
using System.Drawing;
using System.Windows.Forms;
using ClausaComm.Messages;

namespace ClausaComm.Components
{
    public sealed partial class ChatTextBox : TextBox
    {
        #region backing properties

        private int _nLeftRect = 2;
        private int _nTopRect = 3;
        private int _nHeightRect = 13;
        private int _nWeightRect = 13;

        #endregion backing properties

        #region properties

        private int RectLeft
        {
            get => _nLeftRect;
            set
            {
                _nLeftRect = value;
                Invalidate();
            }
        }

        private int RectTop
        {
            get => _nTopRect;
            set
            {
                _nTopRect = value;
                Invalidate();
            }
        }

        private int RectHeight
        {
            get => _nHeightRect;
            set
            {
                _nHeightRect = value;
                Invalidate();
            }
        }

        private int RectWidth
        {
            get => _nWeightRect;
            set
            {
                _nWeightRect = value;
                Invalidate();
            }
        }

        #endregion properties
        
        public ChatTextBox()
        {
            MaxLength = ChatMessage.MaxTextLength - 2;
            ForeColor = Constants.UiConstants.ChatTextBoxTextColor;
            BackColor = Constants.UiConstants.ChatTextBoxBackColor;
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
            Region = Region.FromHrgn(CreateRoundRectRgn(RectLeft, RectTop, Width, Height, RectHeight, RectWidth));
            BackColor = Constants.UiConstants.ChatTextBoxBackColor;
            ForeColor = Constants.UiConstants.ChatTextBoxTextColor;
        }
    }
}