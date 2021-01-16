using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accessibility;
using ClausaComm.Components;
using ClausaComm.Forms;

namespace ClausaComm
{

    public static class Program
    {

        public static readonly string ThisProgramPath = Path.Combine(Directory.GetCurrentDirectory(), Process.GetCurrentProcess().MainModule.FileName);
        public const string ProgramName = "ClausaComm";
        public const string Version = "0.0.2";


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (IsAnotherInstanceRunning())
                Close();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mainForm = new MainForm();

            new Thread(() =>
            {
                (bool newVersionAvailable, string? newVersion) = UpdateManager.IsNewVersionAvailable().Result;
                if (newVersionAvailable)
                    UpdateManager.DownloadNewVersionBinaryAsync(completedHandler: (_, _) => ShowUpdateNotification(mainForm, newVersion));
            }).Start();

            Application.Run(mainForm);
            Close();
        }

        private static void ShowUpdateNotification(MainForm mainForm, string newVersion)
        {
            InWindowNotification.NotificationArgs notifArgs = new()
            {
                DurationMillis = 15000,
                MiddleButton = new InWindowNotification.NotificationArgs.ButtonArgs { ClickCallback = (_, _) => Close(true), Name = "Update now"},
                Title = "New update available",
                Text = $"Version {newVersion} is now available! Current version is {Version}."
            };

            mainForm.Invoke(new MethodInvoker(delegate {
                mainForm.inWindowNotification1.ShowNotification(notifArgs);
            }));
        }

        private static void FinalizeTasksBeforeProgramExit(bool restart)
        {
            if (UpdateManager.UpdateDownloaded)
            {
                UpdateManager.PrepareUpdateAndStartTimer(restart);
            }
        }

        public static void Close(bool restart = false)
        {
            Debug.WriteLine("closing program");
            FinalizeTasksBeforeProgramExit(restart);
            Application.Exit();
            Environment.Exit(0);
        }

        private static bool IsAnotherInstanceRunning()
            => Process.GetProcessesByName(Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Length > 1;
    }
}
