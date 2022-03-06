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

            public const int BlueTint = 5;
            public static readonly Color SecondaryColor = Color.FromArgb(253, 172, 10);
            public static readonly Color ElementOnHoverColor = Color.FromArgb(253, 172, 10);
            public static readonly Color ElementOnClickColor = Color.FromArgb(253, 152, 10);
            
            public static readonly Color ChatBackColor = Color.FromArgb(55, 55, 55 + BlueTint);
            public static readonly Color ChatTextColor = Color.FromArgb(220, 220, 220);
            public static readonly Color ChatTextColorUndelivered = Color.FromArgb(140, 140, 140);
            public static readonly Color ChatTextColorLink = Color.FromArgb(28, 158, 255);
            public static readonly Color ChatTextColorLinkUndelivered = Color.FromArgb(91, 115, 185);
            
            public static readonly Color ChatMessageOnHoverColor = Color.FromArgb(46, 46, 46 + BlueTint);

            public static readonly Color ChatTextBoxBackColor = Color.FromArgb(68, 68, 68 + BlueTint);
            public static readonly Color ChatTextBoxTextColor = Color.FromArgb(215, 215, 215);
            public static readonly Color ChatTextBoxContainerBackColor = ChatBackColor;
            
            public static readonly Color TitleBarColor = Color.FromArgb(28, 28, 28 + BlueTint);
            public static readonly Color NotificationPanelColor = Color.FromArgb(29, 29, 29 + BlueTint);
            public static readonly Color UiColor = Color.FromArgb(42, 42, 42 + BlueTint);

            public static Color ReturnNewOrDefaultColor(Color defaultColor, Color newColor)
                => newColor == DefaultDesignerColorSignalizer ? defaultColor : newColor;
        }
    }

}
