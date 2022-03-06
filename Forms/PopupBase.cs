using System.Drawing;
using System.Windows.Forms;

namespace ClausaComm.Forms;

public class PopupBase : FormBase
{
    protected readonly MainForm ContainingForm;

    private readonly Label HeaderLabel = new()
    {
        Font = new Font("Segoe UI", 26.25F, FontStyle.Regular, GraphicsUnit.Point),
        ForeColor = Color.WhiteSmoke,
        Dock = DockStyle.Top,
        Location = new Point(0, 0),
        Height = 50,
        Name = "HeaderLabel",
        Text = "",
        TextAlign = ContentAlignment.MiddleCenter,
    };

    protected PopupBase(MainForm containingForm, string label)
    {
        ContainingForm = containingForm;
        HeaderLabel.Text = label;
    }

    protected void Init()
    {
        var background = new DimBackground { Size = ContainingForm.Size, Location = ContainingForm.Location, StartPosition = FormStartPosition.Manual};
        
        Controls.Add(HeaderLabel);
        // Add the title bar AFTER the header, so that they don't fight over docking priority.
        InitTitleBar(this, "");
        TitleBar.BackColor = BackColor;

        StartPosition = FormStartPosition.Manual;
        FormBorderStyle = FormBorderStyle.None;
        BackColor = Constants.UiConstants.UiColor;
        TitleBar.BackColor = Constants.UiConstants.UiColor; 
        ShowInTaskbar = false;
        Draggable = false;
        TopMost = false;
        
        Owner = ContainingForm;
        background.Owner = ContainingForm;
        background.Show();

        Closed += OnClose;

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