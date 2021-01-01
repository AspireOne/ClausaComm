using ClausaComm.Utils;
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
    public partial class ImageIconBase : IconBase
    {
        public bool ColorIconOnHover { get; set; } = false;
        public bool ColorIconOnClick { get; set; } = false;

        private Image ImageBeforeHover;
        private bool ImageBeforeHoverAssigned = false;

        #region backing fields
        private Color _iconColor = DefaultIconColor;
        private Color _hoverIconColor = Constants.UIConstants.ElementOnHover.Color;
        private Color _clickIconColor = Constants.UIConstants.ElementOnClick.Color;
        #endregion

        #region properties
        public Color IconColor
        {
            get => _iconColor;
            set
            {
                _iconColor = Constants.UIConstants.ReturnNewOrDefaultColor(DefaultIconColor, value);
                Invalidate();
            }
        }

        public Color HoverIconColor
        {
            get => _hoverIconColor;
            set
            {
                _hoverIconColor = Constants.UIConstants.ReturnNewOrDefaultColor(Constants.UIConstants.ElementOnHover.Color, value);
                Invalidate();
            }
        }
        public Color ClickIconColor
        {
            get => _clickIconColor;
            set
            {
                Invalidate();
                _clickIconColor = Constants.UIConstants.ReturnNewOrDefaultColor(Constants.UIConstants.ElementOnClick.Color, value);
            }
        }

        new protected Image Image
        {
            get => base.Image;
            set
            {
                // Resizing the image manually before assigning it to PictureBox, because it will have higher quality.
                Image resized = ImageUtils.HQResize(value, Width - (Padding.Left + Padding.Right), Height - (Padding.Top + Padding.Bottom));
                base.Image = resized;
            }
        }
        #endregion


        #region constructors
        protected ImageIconBase() : base() { }
        protected ImageIconBase(Image image) : base()
        {
            SetInitialImage(image);
        }
        protected ImageIconBase(IContainer container) : base(container) { }

        private void SetInitialImage(Image image) => Image = ImageUtils.AlterColor(image, IconColor);
        #endregion

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!ImageBeforeHoverAssigned)
            {
                ImageBeforeHoverAssigned = true;
                ImageBeforeHover = Image;
            }

            if (ColorIconOnHover && HoverIconColor != Color.Empty)
                Image = ImageUtils.AlterColor(Image, HoverIconColor);

            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (ColorIconOnClick)
                Image = ImageUtils.AlterColor(Image, ClickIconColor);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (IsHovering)
                if (ColorIconOnHover)
                    Image = ImageUtils.AlterColor(Image, HoverIconColor);
                else
                    Image = ImageBeforeHover;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ImageBeforeHoverAssigned = false;
            Image = ImageBeforeHover;
            Invalidate();
        }
    }
}
