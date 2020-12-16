using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm
{
    public static class UIConstants
    {
        // The UI designer doesn't support resetting values to default or passing Color.Empty, so we can set rgb(0, 0, 0, 1) instead
        // and make it a substitute for "default".
        /// <summary>
        /// A designer substitute for "default".
        /// </summary>
        public static readonly Color DefaultColorSignalizer = Color.FromArgb(0, 0, 0, 1);
        public static readonly Color SecondaryColor = Color.FromArgb(252, 196, 11);

        public static Color ReturnNewOrDefaultColor(Color defaultColor, Color newColor)
            => newColor == DefaultColorSignalizer ? defaultColor : newColor;

        public readonly struct ElementOnHover
        {
            public static readonly Color Color = Color.FromArgb(252, 196, 11);
            public static readonly SolidBrush Brush = new SolidBrush(Color);
        }

        public readonly struct ElementOnClick
        {
            public static readonly Color Color = Color.FromArgb(255, 79, 25);
            public static readonly SolidBrush Brush = new SolidBrush(Color);
        }
    }
}
