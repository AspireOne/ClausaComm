using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Transactions;
using ClausaComm.Exceptions;
using ClausaComm.Network_Communication.Objects;
using ClausaComm.Utils;

namespace ClausaComm.Network_Communication.Networking
{
    // Singleton.
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
        
        public bool ConnectionExists(IPAddress ip)
        {
            lock (Connections)
                return Connections.Exists(c => ((IPEndPoint)c.Client.RemoteEndPoint).Address.Equals(ip));
        }

        /// <summary>Will start listening and handling incoming connections. Blocking.</summary>
        /// <returns>False if there was an error during (or before) listening. Otherwise true.</returns>
        public void Run(Action<bool> runningCallback)
        {
            Logger.Log($"Server's Run method was called. Is already running: {Running}");
            
            if (Running)
                runningCallback.Invoke(false);

            Running = true;
            Listener = new TcpListener(IPAddress.Any, Port);

            try
            {
                Listener.Start();
            }
            catch (SocketException e)
            {
                Logger.Log($"There was an error during starting a Server socket listening (TcpListener.Start) (err code: {e.ErrorCode})");
                Logger.Log(e);
                Running = false;
                runningCallback.Invoke(false);
                return;
            }
            
            Logger.Log($"Server listening on port {Port} (any IP)...");
            runningCallback.Invoke(true);

            while (true)
            {
                TcpClient client = Listener.AcceptTcpClient();
                lock (Connections) 
                    Connections.Add(client);
                
                ThreadUtils.RunThread(() =>
                {
                    var connectionEndpoint = (IPEndPoint)client.Client.RemoteEndPoint;
                    Logger.Log($"Server got new connection. IP: {connectionEndpoint}");
                    RaiseConnect((IPEndPoint)client.Client.RemoteEndPoint);
                    StartReading(client);
                    Logger.Log($"Node disconnected from server. IP: {connectionEndpoint}");
                    lock (Connections)
                        Connections.Remove(client);
                });
            }
        
            Listener.Stop();
            Running = false;
        }

        public bool Send(IPEndPoint endpoint, RemoteObject obj)
        {
            TcpClient? client;
            lock (Connections)
                client = Connections.Find(c => ((IPEndPoint)c.Client.RemoteEndPoint).Address.Equals(endpoint.Address));

            Logger.Log($"Server's Send method was invoked (ip: {endpoint.ToString()}). Active connection to the desired endpoint found: {client is not null}");
            bool success = client is not null && Send(client, obj);
            Logger.Log($"Server's Send succesfull: {success}");
            return success;
        }
    }   
}