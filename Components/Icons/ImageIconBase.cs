using ClausaComm.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ClausaComm.Components.Icons
{
    public partial class ImageIconBase : IconBase
    {
        private Image ImageBeforeHover;
        private Image OriginalImage;
        private bool ImageBeforeHoverAssigned = false;

        #region backing fields
        private Color _iconColor = DefaultIconColor;
        private Color _hoverIconColor = Constants.UiConstants.ElementOnHoverColor;
        private Color _clickIconColor = Constants.UiConstants.ElementOnClickColor;
        #endregion

        #region accesors
        public Color IconColor
        {
            get => _iconColor;
            set
            {
                _iconColor = Constants.UiConstants.ReturnNewOrDefaultColor(DefaultIconColor, value);
                Image = ImageUtils.AlterColor(Image, value);
                Invalidate();
            }
        }

        public Color HoverIconColor
        {
            get => _hoverIconColor;
            set
            {
                _hoverIconColor = Constants.UiConstants.ReturnNewOrDefaultColor(Constants.UiConstants.ElementOnHoverColor, value);
                Invalidate();
            }
        }
        public Color ClickIconColor
        {
            get => _clickIconColor;
            set
            {
                Invalidate();
                _clickIconColor = Constants.UiConstants.ReturnNewOrDefaultColor(Constants.UiConstants.ElementOnClickColor, value);
            }
        }

        // should be protected, but windows forms gui designer is too fucking dumb to not initialize a default value on it and throw an error.
        public new Image Image
        {
            get => base.Image;
            set
            {
                // Resizing the image manually before assigning it to PictureBox, because it will have higher quality.
                OriginalImage = value;
                Image resized = ImageUtils.HQResize(value, Width - (Padding.Left + Padding.Right), Height - (Padding.Top + Padding.Bottom));
                base.Image = resized;
            }
        }
        #endregion


        #region constructors and initialization
        protected ImageIconBase(Image image) : base() => SetInitialImage(image);
        protected ImageIconBase(IContainer container) : base(container) { }
        private void SetInitialImage(Image image) => Image = ImageUtils.AlterColor(image, IconColor);
        #endregion

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // The default Image resizing has a shit quality, so we're doing it ourselves.
            Image resized = ImageUtils.HQResize(OriginalImage, Width - (Padding.Left + Padding.Right), Height - (Padding.Top + Padding.Bottom));
            base.Image = resized;
        }

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
                Image = ColorIconOnHover ? ImageUtils.AlterColor(Image, HoverIconColor) : ImageBeforeHover;
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
