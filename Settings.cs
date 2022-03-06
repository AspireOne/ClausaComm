using System.Diagnostics;
using System.Windows.Forms;
using ClausaComm.Utils;
using Microsoft.Win32;

namespace ClausaComm;

public static class Settings
{
    public static void SetDefaults()
    {
        RunOnStartup = true;
    }
    
    private static bool? RunOnStartupCached;
    public static bool RunOnStartup
    {
        get
        {
            if (RunOnStartupCached.HasValue)
                return RunOnStartupCached.Value;
            
            using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Paths.RegistryStartupPath, false);
            string? value = registryKey?.GetValue(Program.Name, Program.ExePath)?.ToString();

            RunOnStartupCached = value is not null && value != string.Empty;
            return RunOnStartupCached.Value;
        }
        set
        {
            if (value == RunOnStartupCached)
                return;
            
            using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Paths.RegistryStartupPath, true);
            if (registryKey is null)
            {
                Logger.Log("RegistryKey (SubKey) was null when trying to set run at startup.");
                return;
            }
            
            if (value)
                registryKey.SetValue(Program.Name, $"\"{Paths.InstallationExe}\" {Program.MinimizedArgument}");
            else
                registryKey.DeleteValue(Program.Name, false);
            
            RunOnStartupCached = value;
        }
    }  
}