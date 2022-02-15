using System.Windows.Forms;
using Microsoft.Win32;

namespace ClausaComm;

public static class Settings
{
    private static bool _RunOnStartup;
    public static bool RunOnStartup
    {
        get => _RunOnStartup;
        set
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (value)
                registryKey.SetValue(Program.Name, Program.ExePath);
            else
                registryKey.DeleteValue(Program.Name, false);
            
            registryKey.Close();
            registryKey.Dispose();
        }
    }  
}