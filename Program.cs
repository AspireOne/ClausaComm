using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClausaComm.Components;
using ClausaComm.Forms;
using ClausaComm.Messages;

namespace ClausaComm
{
    public static class Program
    {
        public const string Version = "1.0.0";

        public static readonly string ThisProgramPath = Path.Combine(Directory.GetCurrentDirectory(),
            Process.GetCurrentProcess().MainModule.FileName);

        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        private static void Main()
        {
#if !DEBUG
            if (IsAnotherInstanceRunning())
                Close();
#endif

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var mainForm = new MainForm();
#if !DEBUG
            CheckAndDownloadNewVersionAsync(mainForm);
#endif
            Application.Run(mainForm);
            Close();
        }

        // TODO: Check all thread.new and consider switching it for task.run
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
                MiddleButton = new NotificationPanel.NotificationArgs.ButtonArgs
                    {ClickCallback = (_, _) => Close(true), Name = "Update now"},
                Title = "New update available",
                Content = $"Version {newVersion} is now available!"
            };

            mainForm.Invoke(new MethodInvoker(delegate { mainForm.NotificationPanel.ShowNotification(notifArgs); }));
        }

        private static void FinalizeTasksBeforeProgramExit(bool restart)
        {
            if (UpdateManager.UpdateDownloaded)
                UpdateManager.PrepareUpdateAndStartTimer(restart);
        }

        public static void Close(bool restart = false)
        {
            Debug.WriteLine("closing program");
            FinalizeTasksBeforeProgramExit(restart);
            Application.Exit();
            Environment.Exit(0);
        }

        private static bool IsAnotherInstanceRunning()
            => Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly()?.Location)).Length > 1;
    }
}