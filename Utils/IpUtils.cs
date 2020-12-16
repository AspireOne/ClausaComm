using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClausaComm.Network
{
    public static class IpUtils
    {
        public static readonly Regex IpRegex = new Regex(@"\b([0-9]{1,3}\.){3}[0-9]{1,3}\b");
        public static readonly (byte min, byte max) IpLength = new(7, 15);
        private static string _localIp;
        public static string LocalIp
        {
            get
            {
                if (_localIp is null)
                    RefreshLocalIp();
                return _localIp;
            }
            private set => _localIp = value;

        }

        static IpUtils()
        {
            RefreshLocalIp();
        }

        public static void RefreshLocalIp()
        {
            try
            {
                using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
                socket.Connect("1.1.1.1", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                string ip = endPoint.Address.ToString();
                LocalIp = ip;
            }
            catch
            {
                LocalIp = null;
            }
        }

        public static bool IsIpCorrect(string ip) => IpRegex.IsMatch(ip);
    }
}
