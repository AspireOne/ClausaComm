using System;
using System.Drawing;

namespace ClausaComm
{
    public static class Constants
    {
        public static readonly Random Random = new();
        public static class UiConstants
        {
            // The UI designer doesn't support resetting values to default or passing Color.Empty, so we can set rgb(0, 0, 0, 1) instead
            // and make it a substitute for "default".
            /// <summary>A designer substitute for "default".</summary>
            public static readonly Color DefaultDesignerColorSignalizer = Color.FromArgb(0, 0, 0, 1);
            
            public static readonly Color SecondaryColor = Color.FromArgb(253, 172, 10);
            public static readonly Color ElementOnHoverColor = Color.FromArgb(253, 172, 10);
            public static readonly Color ElementOnClickColor = Color.FromArgb(253, 152, 10);

            public static readonly Color ChatBackColor = Color.FromArgb(49, 49, 52);
            public static readonly Color ChatTextColor = Color.FromArgb(220, 220, 220);
            
            public static readonly Color ChatTextBoxBackColor = Color.FromArgb(62, 62, 64);
            public static readonly Color ChatTextBoxTextColor = Color.FromArgb(190, 190, 190);
            
            public static readonly Color TitleBarColor = Color.FromArgb(34, 34, 36);
            public static readonly Color NotificationPanelColor = Color.FromArgb(29, 29, 31);
            public static readonly Color UiColor = Color.FromArgb(42, 42, 44);

            public static Color ReturnNewOrDefaultColor(Color defaultColor, Color newColor)
                => newColor == DefaultDesignerColorSignalizer ? defaultColor : newColor;
        }
    }

}
