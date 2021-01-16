using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

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
                Terminate();

            if (UpdateManager.IsNewVersionAvailable().Result)
                UpdateManager.DownloadNewVersionBinaryAsync(null, (_, _) => Terminate(), null);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.MainForm());

            Terminate();
        }

        private static void FinalizeTasksBeforeProgramExit()
        {
            if (UpdateManager.UpdateDownloaded)
            {
                UpdateManager.PrepareUpdateAndStartTimer(false);
                
            }
        }

        public static void Terminate()
        {
            Debug.WriteLine("closing program");
            FinalizeTasksBeforeProgramExit();
            Application.Exit();
            Environment.Exit(0);
        }

        // Supposes that the name of the executable is the same during the start of each instance.
        private static bool IsAnotherInstanceRunning()
            => Process.GetProcessesByName(Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Length > 1;
    }
}
