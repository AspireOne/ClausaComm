using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClausaComm.Components;
using ClausaComm.Forms;
using ClausaComm.Utils;

namespace ClausaComm
{
    public static class Program
    {
        public const string Name = "ClausaComm";
        public const string Version = "1.0.2";
        public const string MinimizedArgument = "minimized";

        public static readonly string ExePath = Application.ExecutablePath;
        private static readonly string IndicatorFilePath = Path.Combine(ProgramDirectory.ThisPath, "0");

        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        private static void Main(string[] args)
        {
            // Check if program launched for the first time.
            if (!File.Exists(IndicatorFilePath))
            {
                Logger.Log("Launched for the first time, setting defaults.");
                File.Create(IndicatorFilePath).Dispose();
                File.SetAttributes(IndicatorFilePath, FileAttributes.Hidden);
                Settings.SetDefaults();
            }
            
            var instances = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ExePath));
            if (instances.Length > 1)
            {
                Utils.Utils.BringToFront(instances[0]);
                Close();
            }

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new MainForm(args.Contains(MinimizedArgument));
#if !DEBUG
            CheckAndDownloadNewVersionAsync(mainForm);
#endif
            Application.Run(mainForm);
            Close();
        }
        
        private static void CheckAndDownloadNewVersionAsync(MainForm mainForm)
        {
            _ = Task.Run(() =>
            {
                (bool newVersionAvailable, string newVersion) = UpdateManager.IsNewVersionAvailable().Result;
                if (newVersionAvailable)
                    UpdateManager.DownloadNewVersionBinaryAsync(completedHandler: (_, _) => ShowUpdateNotification(mainForm, newVersion));
            });
        }

        private static void ShowUpdateNotification(MainForm mainForm, string newVersion)
        {
            NotificationPanel.NotificationArgs notifArgs = new()
            {
                DurationMillis = 15000,
                LeftButton = new NotificationPanel.NotificationArgs.ButtonArgs
                    {ClickCallback = (_, _) => Close(true), Name = "restart now"},
                Title = "New update available",
                Content = $"Version {newVersion} is now available and will be installed on the next restart."
            };
            
            mainForm.Invoke(() => mainForm.NotificationPanel.ShowNotification(notifArgs));
        }

        private static void FinalizeTasksBeforeProgramExit(bool restart)
        {
            if (UpdateManager.UpdateDownloaded)
                UpdateManager.PrepareUpdateAndStartTimer(restart);
        }

        public static void Close(bool restart = false)
        {
            Logger.Log("closing program");
            FinalizeTasksBeforeProgramExit(restart);
            Application.Exit();
            Environment.Exit(0);
        }
    }
}