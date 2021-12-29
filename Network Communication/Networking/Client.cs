using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Transactions;
using ClausaComm.Network_Communication.Objects;
using LiteNetLib;

namespace ClausaComm.Network_Communication.Networking
{
    internal class Client : NetworkNode
    {
        private readonly TcpClient UnderlyingClient = new();
        public readonly IPEndPoint TargetEndpoint;

        public Client(IPEndPoint targetEndpoint) => TargetEndpoint = targetEndpoint;

        public override bool Equals(object obj) => obj is Client client && TargetEndpoint.ToString() == client.TargetEndpoint.ToString();
        public override int GetHashCode() => int.Parse(TargetEndpoint.ToString().Replace(".", "").Replace(":", ""));

        /// <summary>
        /// Connects to the host and starts reading from the network. Blocking.
        /// </summary>
        /// <returns>True if successfully connected and able to read from the network; false otherwise.</returns>
        public virtual bool Run()
        {
            if (UnderlyingClient.Connected) return false;
            UnderlyingClient.Connect(TargetEndpoint);
            if (!UnderlyingClient.Connected) return false;

            OnConnect?.Invoke(TargetEndpoint);
            StartReading(UnderlyingClient);
            return true;
        }

        public bool Send(byte[] bytes) => Send(UnderlyingClient, bytes);
    }
}