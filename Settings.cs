using System.Diagnostics;
using System.Windows.Forms;
using ClausaComm.Utils;
using Microsoft.Win32;

namespace ClausaComm;

public static class Settings
{
    private static bool? RunOnStartupCached;
    public static bool RunOnStartup
    {
        get
        {
            if (RunOnStartupCached.HasValue)
                return RunOnStartupCached.Value;
            
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string? value = registryKey.GetValue(Program.Name, Program.ExePath)?.ToString();

            registryKey.Close();
            registryKey.Dispose();
            
            RunOnStartupCached = value is not null;
            return RunOnStartupCached.Value;
        }
        set
        {
            if (value == RunOnStartupCached)
                return;
            
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (value)
                registryKey.SetValue(Program.Name, $"{Program.ExePath} {Program.MinimizedArgument}");
            else
                registryKey.DeleteValue(Program.Name, false);
            
            RunOnStartupCached = value;
            registryKey.Close();
            registryKey.Dispose();
        }
    }  
}