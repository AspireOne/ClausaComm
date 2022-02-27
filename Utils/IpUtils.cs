using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace ClausaComm.Utils
{
    public static class IpUtils
    {
        public static readonly Regex IpRegex = new(@"\b([0-9]{1,3}\.){3}[0-9]{1,3}\b");
        private static readonly (string address, int port) TestingDns = new("1.1.1.1", 65530);
        private static IPAddress? _localIp;
        public static IPAddress LocalIp
        {
            get
            {
                if (_localIp is null)
                    RefreshLocalIp();
                
                return _localIp;
            }
            private set => _localIp = value;

        }

        /// <summary>
        /// Refreshes the local IP (if it's null will be null), assign it to the LocalIp property, and return it.
        /// </summary>
        public static IPAddress? RefreshLocalIp()
        {
            try
            {
                using Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, 0);
                socket.Connect(TestingDns.address, TestingDns.port);
                var endPoint = socket.LocalEndPoint as IPEndPoint;
                LocalIp = endPoint?.Address;
            }
            catch (Exception e)
            {
                Logger.Log("A handled error occured while refreshing local IP.");
                Logger.Log(e);
                LocalIp = null;
            }

            return LocalIp;
        }

        public static bool IsIpCorrect(string? ip) =>
            ip is not null && IpRegex.IsMatch(ip) && IPAddress.TryParse(ip, out _);
    }
}
