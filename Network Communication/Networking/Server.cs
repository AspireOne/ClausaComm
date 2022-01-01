using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ClausaComm.Exceptions;

namespace ClausaComm.Network_Communication.Networking
{
    // Singleton
    internal class Server : NetworkNode
    {
        private static bool InstanceCreated;
        public const int Port = 5000;
        private readonly List<TcpClient> Connections = new();
        private TcpListener? Listener;

        public Server()
        {
            if (InstanceCreated)
                throw new MultipleInstancesException("Only one instance of Server can be created");
            InstanceCreated = true;
        }

        /// <summary>Will start listening and handling incoming connections. Blocking.</summary>
        /// <returns>False if there was an error during (or before) listening. Otherwise true.</returns>
        public bool Run()
        {
            if (Listener is not null && Listener.Server.Connected)
                return false;
            
            Listener = new TcpListener(IPAddress.Any, Port);

            try
            {
                Listener.Start();

            }
            catch (SocketException e)
            {
                Debug.WriteLine($"There was a handled error during a Server socket listening (err code: {e.ErrorCode})");
                Debug.WriteLine(e);
                return false;
            }
            Debug.WriteLine($"Server listening on port {Port} (any IP)...");
            
            while (true)
            {
                TcpClient client = Listener.AcceptTcpClient();
                OnConnect?.Invoke((IPEndPoint)client.Client.RemoteEndPoint);
                lock (Connections) 
                    Connections.Add(client);
                
                new Thread(() =>
                {
                    StartReading(client);
                    lock (Connections)
                        Connections.Remove(client);
                }).Start();
            }
        
            Listener.Stop();
            return true;
        }

        public bool Send(IPEndPoint endpoint, byte[] bytes)
        {
            TcpClient client;
            lock (Connections)
                client = Connections.Find(c => ((IPEndPoint)c.Client.RemoteEndPoint).ToString() == endpoint.ToString());
            
            return client is not null && Send(client, bytes);
        }
    }   
}