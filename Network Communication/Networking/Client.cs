﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Transactions;
using ClausaComm.Network_Communication.Objects;
using ClausaComm.Utils;

namespace ClausaComm.Network_Communication.Networking
{
    internal class Client : NetworkPeer
    {
        private readonly TcpClient UnderlyingClient = new();
        private const ushort ConnectTimeoutMillis = 1500;
        public readonly IPEndPoint TargetEndpoint;

        public Client(IPEndPoint targetEndpoint) => TargetEndpoint = targetEndpoint;

        public override bool Equals(object obj) => obj is Client client && TargetEndpoint.ToString() == client.TargetEndpoint.ToString();
        public override int GetHashCode() => int.Parse(TargetEndpoint.ToString().Replace(".", "").Replace(":", ""));

        /// <summary>
        /// Connects to the host and starts reading from the network. Non-blocking.
        /// </summary>
        /// <returns>True if successfully connected and not already running; false otherwise.</returns>
        public bool Run()
        {
            if (Running)
                return false;
            
            Running = true;
            
            try
            {
                if (!UnderlyingClient.ConnectAsync(TargetEndpoint).Wait(ConnectTimeoutMillis))
                    return false;
            }
            catch (Exception e)
            {
                Logger.Log($"{nameof(Client)}: There was a handled error during trying to connect a TcpClient (endpoint: {TargetEndpoint})");
                Logger.Log(e);
                Running = false;
                return false;
            }
            
            if (!UnderlyingClient.Connected)
            {
                Logger.Log($"{nameof(Client)}: Could not connect. (endpoint: {TargetEndpoint})");
                Running = false;
                return false;
            }
            
            Logger.Log($"{nameof(Client)}: Connected (endpoint: {TargetEndpoint})");
            RaiseConnect(TargetEndpoint);
            ThreadUtils.RunThread(() =>
            {
                StartReading(UnderlyingClient);
                Logger.Log($"{nameof(Client)}: Disconnected (endpoint: {TargetEndpoint})");
                Running = false; 
            });
            
            return true;
        }

        public bool Send(RemoteObject obj) => Send(UnderlyingClient, obj);
    }
}