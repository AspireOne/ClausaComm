using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClausaComm.Utils
{
    public static class IpUtils
    {
        public static readonly Regex IpRegex = new Regex(@"\b([0-9]{1,3}\.){3}[0-9]{1,3}\b");
        public static readonly (byte min, byte max) IpLength = new(7, 15);
        private static readonly (string address, int port) TestingDns = new("1.1.1.1", 65530);
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

        /// <summary>
        /// Refreshes the local IP (if it's null will be null), assign it to the LocalIp property, and return it.
        /// </summary>
        public static string RefreshLocalIp()
        {
            try
            {
                using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
                socket.Connect(TestingDns.address, TestingDns.port);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                LocalIp = endPoint.Address.ToString();
            }
            catch
            {
                LocalIp = null;
            }

            return LocalIp;
        }

        public static bool IsIpCorrect(string ip) => ip is not null && IpRegex.IsMatch(ip);
    }

    public class InvalidIpException : Exception
    {
        public InvalidIpException() { }
        public InvalidIpException(string message) : base(message) { }
        public InvalidIpException(string message, Exception inner) : base(message, inner) { }
    }
}
