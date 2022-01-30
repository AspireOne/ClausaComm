using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Transactions;
using ClausaComm.Network_Communication.Objects;

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
        /// <returns>True if successfully connected and not already running; false otherwise.</returns>
        public virtual bool Run()
        {
            Debug.WriteLine($"{nameof(Client)}: Run method called. Already running: {Running} (endpoint: {TargetEndpoint})");
            if (Running)
                return false;
            
            Running = true;
            
            try
            {
                UnderlyingClient.Connect(TargetEndpoint);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{nameof(Client)}: There was a handled error during trying to connect a TcpClient (endpoint: {TargetEndpoint})");
                Debug.WriteLine(e);
                Running = false;
                return false;
            }
            
            if (!UnderlyingClient.Connected)
            {
                Debug.WriteLine($"{nameof(Client)}: Could not connect. (endpoint: {TargetEndpoint})");
                Running = false;
                return false;
            }
            
            Debug.WriteLine($"{nameof(Client)}: Connected (endpoint: {TargetEndpoint})");
            OnConnect?.Invoke(TargetEndpoint);
            StartReading(UnderlyingClient);
            Debug.WriteLine($"{nameof(Client)}: Stopped reading. (endpoint: {TargetEndpoint})");
            Running = false;
            return true;
        }

        public bool Send(byte[] bytes) => Send(UnderlyingClient, bytes);
    }
}