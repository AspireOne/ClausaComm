using System.Drawing;
using System.Windows.Forms;
using ClausaComm.Utils;

namespace ClausaComm.Forms;

public class PopupBase : FormBase
{
    protected readonly MainForm ContainingForm;

    protected PopupBase(MainForm containingForm)
    {
        ContainingForm = containingForm;
    }

    protected void Init()
    {
        var background = new DimBackground { Size = ContainingForm.Size, Location = ContainingForm.Location, StartPosition = FormStartPosition.Manual};
        
        InitTitleBar(this, "");
        TitleBar.BackColor = BackColor;
        StartPosition = FormStartPosition.Manual;
        ShowInTaskbar = false;
        Draggable = false;
        TopMost = false;
        
        Owner = ContainingForm;
        background.Owner = ContainingForm;
        background.Show();

        this.Closed += OnClose;

        void OnClose(object b, object a)
        {
            Closed -= OnClose;
            background.Close();
            // Workaround for the background form closing the containing form on the background form's close.
            ContainingForm.Activate();
            ContainingForm.Focus();
        }
        
        Location = new Point(
            ContainingForm.Location.X + (ContainingForm.Size.Width - Size.Width) / 2,
            ContainingForm.Location.Y + (ContainingForm.Size.Height - Size.Height) / 2 - TitleBar.Height);
    }
}