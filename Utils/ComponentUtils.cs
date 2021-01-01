using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace ClausaComm.Utils
{
    public static class ComponentUtils
    {
        public enum MouseEvent { Enter, Leave, Down }

        public static void AddBackColorFilterOnMouseEvent(Control control, Dictionary<MouseEvent, Color> eventColorDict)
        {
            control.MouseUp += (object _, MouseEventArgs e) => control.BackColor = eventColorDict[MouseEvent.Enter];
            control.MouseEnter += (object _, EventArgs e) => control.BackColor = eventColorDict[MouseEvent.Enter];
            control.MouseDown += (object _, MouseEventArgs e) => control.BackColor = eventColorDict[MouseEvent.Down];
            control.MouseLeave += (object _, EventArgs e) => control.BackColor = eventColorDict[MouseEvent.Leave];
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
