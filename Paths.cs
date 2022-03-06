using System;
using System.IO;
using Microsoft.Win32;

namespace ClausaComm;

public static class Paths
{
    public const string RegistryStartupPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    public const string RegistryUninstallPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\ClausaComm";

    private static readonly string _FileSavePath
        = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ClausaComm Files");

    public static string FileSavePath
    {
        get
        {
            if (!Directory.Exists(_FileSavePath))
                Directory.CreateDirectory(_FileSavePath);
            
            return _FileSavePath;
        }
    }

    private static string? InstallationDirCached;
    public static string? InstallationDir
    {
        get
        {
            if (InstallationDirCached is not null)
                return InstallationDirCached;
            
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryUninstallPath);
            InstallationDirCached = key?.GetValue("InstallLocation")?.ToString()?.Replace("\"", "");
            return InstallationDirCached;
        }
    }

    public static string? InstallationExe
        => InstallationDir is null ? null : Path.Combine(InstallationDir, $"{Program.Name}.exe");
}