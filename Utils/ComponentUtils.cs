using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.Control;

namespace ClausaComm.Utils
{
    public static class ComponentUtils
    {
        public enum MouseEvent { Enter, Leave, Down }

        public static void AddBackColorFilterOnMouseEvent(Control control, Dictionary<MouseEvent, Color> eventColorDict)
        {
            if (eventColorDict.ContainsKey(MouseEvent.Enter))
            {
                control.MouseUp += (_, _) => control.BackColor = eventColorDict[MouseEvent.Enter];
                control.MouseEnter += (_, _) => control.BackColor = eventColorDict[MouseEvent.Enter];   
            }
            
            if (eventColorDict.ContainsKey(MouseEvent.Down))
                control.MouseDown += (_, _) => control.BackColor = eventColorDict[MouseEvent.Down];
            
            if (eventColorDict.ContainsKey(MouseEvent.Leave))
                control.MouseLeave += (_, _) => control.BackColor = eventColorDict[MouseEvent.Leave];
        }
        
        public static void SetDoubleBuffered(Control c)
        {
            //Taxes: Remote Desktop Connection and painting
            //http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp = typeof(Control).GetProperty("DoubleBuffered", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null); 
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
