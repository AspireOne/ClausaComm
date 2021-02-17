using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClausaComm.Components;
using ClausaComm.Forms;
using ClausaComm.Network_Communication.Networking;
using ClausaComm.Network_Communication.Objects;
using LiteNetLib;
using LiteNetLib.Utils;

namespace ClausaComm
{
    public static class Program
    {
        public static readonly string ThisProgramPath = Path.Combine(Directory.GetCurrentDirectory(), Process.GetCurrentProcess().MainModule.FileName);
        public const string Version = "0.0.3";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            EventBasedNetListener listener = new EventBasedNetListener();
            NetManager client = new NetManager(listener);
            client.Start();
            client.Connect("localhost", 9050, "SomeConnectionKey");
            //client.SendUnconnectedMessage(Encoding.UTF8.GetBytes("ahoy"), NetUtils.MakeEndPoint("localhost", 9050));
            listener.NetworkReceiveUnconnectedEvent += (fromPeer, dataReader, deliveryMethod) =>
            {
                Debug.WriteLine("We got: {0}", dataReader.GetString());
                dataReader.Recycle();
            };

            Task.Run(() =>
            {
                while (true)
                {
                    client.PollEvents();
                    Thread.Sleep(15);
                }
            });

            EventBasedNetListener listener2 = new();
            NetManager server = new NetManager(listener2);
            server.Start(9050 /* port */);

            listener2.ConnectionRequestEvent += request =>
            {
                if (server.ConnectedPeersCount < 10 /* max connections */)
                    request.AcceptIfKey("SomeConnectionKey");
                else
                    request.Reject();
            };
            /*
            listener2.NetworkReceiveUnconnectedEvent += (fromPeer, dataReader, deliveryMethod) =>
            {
                Debug.WriteLine("We got: {0}", dataReader.GetString());
                dataReader.Recycle();
            };
            */
            listener2.PeerConnectedEvent += peer =>
            {
                Debug.WriteLine("We got connection: {0}", peer.EndPoint); // Show peer ip
                NetDataWriter writer = new NetDataWriter();                 // Create writer class
                writer.Put("Hello client!");                                // Put some string
                peer.Send(writer, DeliveryMethod.ReliableOrdered);             // Send with reliability
            };

            while (true)
            {
                server.PollEvents();
                Thread.Sleep(15);
            }
            server.Stop();

            /*
            Client c = new();
            Server s = new((_, _) => Debug.WriteLine("Server received something"));
            s.Run();

            c.Run();
            c.Send("localhost", new(new Ping()));
            */
            Application.Run();
#if !DEBUG
            if (IsAnotherInstanceRunning())
                Close();
#endif

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mainForm = new MainForm();
#if !DEBUG
            CheckForNewVersionAsync(mainForm);
#endif
            Application.Run(mainForm);
            Close();
        }

        private static void CheckForNewVersionAsync(MainForm mainForm)
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
                MiddleButton = new NotificationPanel.NotificationArgs.ButtonArgs { ClickCallback = (_, _) => Close(true), Name = "Update now" },
                Title = "New update available",
                Content = $"Version {newVersion} is now available!"
            };

            mainForm.Invoke(new MethodInvoker(delegate
            {
                mainForm.NotificationPanel.ShowNotification(notifArgs);
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