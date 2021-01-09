using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace ClausaComm.Utils
{
    public static class ComponentUtils
    {
        public enum MouseEvent { Enter, Leave, Down }

        public static void AddBackColorFilterOnMouseEvent(Control control, Dictionary<MouseEvent, Color> eventColorDict)
        {
            control.MouseUp += (_, _) => control.BackColor = eventColorDict[MouseEvent.Enter];
            control.MouseEnter += (_, _) => control.BackColor = eventColorDict[MouseEvent.Enter];
            control.MouseDown += (_, _) => control.BackColor = eventColorDict[MouseEvent.Down];
            control.MouseLeave += (_, _) => control.BackColor = eventColorDict[MouseEvent.Leave];
        }

        public static void ChangeControlsBackColor(Color color, IEnumerable<Control> controls)
        {
            foreach (Control control in controls)
                control.BackColor = color;
        }

        public static void ChangeControlsBackColor(Color color, ControlCollection controls)
        {
            foreach (Control control in controls)
                control.BackColor = color;
        }
    }
}
