using ClausaComm.Exceptions;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace ClausaComm.Contacts
{
    public class UserStatusWatcher
    {
        private readonly Contact User;
        private static bool Created;
        private static readonly int NecessaryIdleTimeMillis = (int)TimeSpan.FromMinutes(10).TotalMilliseconds;
        private static readonly int TimerIntervalMillis = (int)TimeSpan.FromSeconds(30).TotalMilliseconds;
        private readonly Timer Timer;
        public bool Running { get; private set; }

        private struct Lastinputinfo
        {
            public uint cbSize;

            public uint dwTime;
        }

        public UserStatusWatcher(Contact user)
        {
            if (Created)
                throw new MultipleInstancesException(nameof(UserStatusWatcher));

            User = user;
            Created = true;
            Timer = new(OnTimerTick, null, Timeout.Infinite, TimerIntervalMillis);
        }

        public void Run()
        {
            if (Running)
                return;

            Running = true;
            Timer.Change(0, TimerIntervalMillis);
        }

        // https://www.pinvoke.net/default.aspx/user32.getlastinputinfo
        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref Lastinputinfo plii);

        [DllImport("Kernel32.dll")]
        private static extern uint GetLastError();

        private void OnTimerTick(object state)
        {
            uint idleTimeMillis = GetIdleTimeMillis();

            if (idleTimeMillis >= NecessaryIdleTimeMillis)
                User.CurrentStatus = Contact.Status.Idle;
            else if (User.CurrentStatus == Contact.Status.Idle)
                User.CurrentStatus = Contact.Status.Online;

            Debug.WriteLine($"UserStatusWatcher timer tick fired. idle time: {idleTimeMillis}. User status: {User.CurrentStatus}");
        }

        public static uint GetIdleTimeMillis()
        {
            Lastinputinfo lastInput = new();
            lastInput.cbSize = (uint)Marshal.SizeOf(lastInput);

            if (!GetLastInputInfo(ref lastInput))
                throw new Exception(GetLastError().ToString());

            return (uint)Environment.TickCount - lastInput.dwTime;
        }
    }
}