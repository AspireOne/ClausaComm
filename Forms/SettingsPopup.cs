using System.Diagnostics;
using System.Windows.Forms;

namespace ClausaComm.Forms;

public partial class SettingsPopup : PopupBase
{
    public SettingsPopup(MainForm containingForm) : base(containingForm, "Settings")
   {
        InitializeComponent();
        RunAtStartupCheckbox.Checked = Settings.RunOnStartup;
        RunAtStartupCheckbox.CheckedChanged += (_, _) => Settings.RunOnStartup = RunAtStartupCheckbox.Checked;
        
        Init();
    }
}