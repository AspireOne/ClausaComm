using System;
using System.Drawing;
using System.Windows.Forms;
using ClausaComm.Components;

namespace ClausaComm.Forms
{
    public class Popup
    {
        public readonly Form Overlay = new()
        {
            Opacity = 0.5,
            BackColor = Color.Black,
            Visible = false,
            Size = new(0, 0),
            FormBorderStyle = FormBorderStyle.None,
            ShowInTaskbar = false,
            TopMost = false,
        };

        // TODO: Resize and position it
        public Form PopupForm { get; set; }
        
        private Form _parentForm;
        private bool ParentHasTitlebar = false;
                
        public Form ParentForm
        {
            get => _parentForm;
            set
            {
                if (ReferenceEquals(_parentForm, value))
                    return;

                if (_parentForm != null)
                {
                    _parentForm.LocationChanged -= HandleMainFormLocationChange;
                    _parentForm.SizeChanged -= HandleParentFormSizeChange;
                }

                _parentForm = value;

                if (Overlay.Visible = value != null)
                {
                    value.LocationChanged += HandleMainFormLocationChange;
                    value.SizeChanged += HandleParentFormSizeChange;

                    ParentHasTitlebar = HasTitleBar(value);

                    HandleParentFormSizeChange(null, null);
                    HandleMainFormLocationChange(null, null);
                }
            }
        }


        private static bool HasTitleBar(Form form)
        {
            foreach (var control in form.Controls)
            {
                if (control is TitleBar)
                    return true;
            }
            return false;
        }


        private void HandleMainFormMinimized(object sender, EventArgs e)
        {

        }

        private void HandleMainFormLocationChange(object sender, EventArgs e)
        {
            int x = ParentForm.Location.X + ParentForm.Padding.Left;
            int y = ParentForm.Location.Y + ParentForm.Padding.Top;

            if (ParentHasTitlebar)
                y += TitleBar.BarHeight;

            Overlay.Location = new Point(x, y);
        }

        private void HandleParentFormSizeChange(object sender, EventArgs e)
        {
            int offsetX = ParentForm.Padding.Right + ParentForm.Padding.Left;
            int offsetY = ParentForm.Padding.Bottom + ParentForm.Padding.Top;

            if (ParentHasTitlebar)
                offsetY += TitleBar.BarHeight;

            var sizeOffset = new Size(offsetX, offsetY);
            Overlay.Size = ParentForm.Size - sizeOffset;
        }

        public Popup()
        {
            this.Overlay.BringToFront();
        }

        public Popup(Form parentForm, Form innerPopup) : this()
        {
            ParentForm = parentForm;
            PopupForm = innerPopup;
        }
    }
}
