using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace ClausaComm.Network_Communication.Networking
{
    /// <summary>
    /// Creates an uniformed connection API (connect, send, onReceive, onConnect...) that combines the Server and
    /// the Client under the hood. For each connection (A -> B) a Client is created and kept in an internal List.
    /// Server (B -> A) keeps it's own list of connections, which this API uses. 
    /// </summary>
    internal static class NetworkManager
    {
        public static readonly List<Client> ClientConnections = new();
        public static readonly Server Server = new();
        public static NetworkNode.ReceiveHandler? OnReceive; // RemoteObject
        public static NetworkNode.ConnectionChangeHandler? OnDisconnect;
        /// <summary> This event is raised both if it's an outgoing or an ingoing connection.</summary>
        public static NetworkNode.ConnectionChangeHandler? OnConnect;

        static NetworkManager()
        {
            Server.OnConnect += endpoint => OnConnect?.Invoke(endpoint);
            Server.OnDisconnect += endpoint => OnDisconnect?.Invoke(endpoint);
            Server.OnReceive += (message, endpoint) => OnReceive?.Invoke(message, endpoint);
        }

        /// <summary>Will start up the Server. Blocking.</summary>
        public static void Run(Action<bool> serverRunningCallback)
        {
            Server.Run(serverRunningCallback);
        }

        /// <summary>
        /// Determines if a connection with the specified endpoint is present either to the server or from one of clients,
        /// and if so, uses it's Send method overloaded from NetworkNode.
        /// </summary>
        /// <returns>True if a connection was found and message successfully sent. False otherwise.</returns>
        public static bool Send(IPAddress ip, byte[] bytes)
        {
            IPEndPoint endpoint = new(ip, Server.Port);
            
            Client client;
            lock (ClientConnections)
                client = ClientConnections.Find(c => c.TargetEndpoint.ToString() == endpoint.ToString());

            return client?.Send(bytes) ?? Server.Send(endpoint, bytes);
        }

        /// <summary>
        /// Blocking. Will create a Client, hook it to events and run it (connect it to the specified endpoint).
        /// </summary>
        /// <returns>True if successfully connected. Otherwise false.</returns>
        public static bool CreateConnection(IPAddress ip)
        {
            IPEndPoint endpoint = new(ip, Server.Port);
            var client = new Client(endpoint);
            
            client.OnReceive += (message, _) => OnReceive?.Invoke(message, endpoint);
            client.OnConnect += _ =>
            {
                lock (ClientConnections)
                    ClientConnections.Add(client);
                OnConnect?.Invoke(endpoint);
            };
            client.OnDisconnect += _ =>
            {
                lock (ClientConnections)
                    ClientConnections.Remove(client);
                OnDisconnect?.Invoke(endpoint);
            };

            return client.Run();
        }
    }
}