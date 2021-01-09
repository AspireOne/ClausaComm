using System;
using System.Windows.Forms;

namespace ClausaComm
{
    public static class Program
    {
        public const string ProgramName = "ClausaComm";
        public const string Version = "0.0.1";
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (IsAnotherInstanceRunning())
                Application.Exit();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.MainForm());
        }

        public static void Terminate()
        {
            Application.Exit();
        }

        // Supposes that the name of the executable is the same during the start of each instance.
        private static bool IsAnotherInstanceRunning()
            => System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Length > 1;
    }
}
