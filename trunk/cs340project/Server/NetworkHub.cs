using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace cs340project
{
    /// <summary>
    /// The class that handles all the network communication
    /// </summary>
    public class NetworkHub
    {
        Dictionary<IPEndPoint, TcpClient> clients = new Dictionary<IPEndPoint, TcpClient>();

        #region Accepting new connections

        public IPEndPoint EndPoint { get; private set; }
        TcpListener listener = null;
        const int listenBacklogLength = 32;

        public NetworkHub()
        {
            Debug.WriteLine("Created a network hub");
        }

        /// <summary>
        /// Listens on the specified IP endpoint.
        /// </summary>
        public void Listen(IPEndPoint ep)
        {
            EndPoint = ep;

            listener = new TcpListener(EndPoint);
            listener.Start(listenBacklogLength);

            listener.BeginAcceptTcpClient(new AsyncCallback(Accept), null);
        }

        /// <summary>
        /// Delegate for Networking
        /// </summary>
        public delegate void NetworkHubClientEvent(TcpClient client);
        /// <summary>
        /// Occurs when [new connection].
        /// </summary>
        public event NetworkHubClientEvent NewConnection = null;
        /// <summary>
        /// Occurs when [disconnected].
        /// </summary>
        public event NetworkHubClientEvent Disconnected = null;

        class BeginReadData
        {
            public TcpClient client;
            public byte[] buffer = new byte[1024];

            public BeginReadData(TcpClient c)
            {
                client = c;
            }
        }

        /// <summary>
        /// Setups the <see cref="TcpClient"/>.
        /// </summary>
        /// <param name="client">The <see cref="TcpClient"/>.</param>
        void SetupClient(TcpClient client)
        {
            string IP = GetClientIP(client);
            clientMemoryStreams[IP] = new MemoryStream();

            BeginReadData data = new BeginReadData(client);
            client.GetStream().BeginRead(data.buffer, 0, data.buffer.Length, new AsyncCallback(OnRead), data);

            if (NewConnection != null)
                NewConnection(client);
        }


        /// <summary>
        /// Gets the client IP.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns>The IP address of the client</returns>
        string GetClientIP(TcpClient client) {
            return ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
        }

        /// <summary>
        /// Accepts the specified result.
        /// </summary>
        /// <param name="result">The result.</param>
        void Accept(IAsyncResult result)
        {
            try
            {
                //Accept the one incoming client:
                TcpClient client = listener.EndAcceptTcpClient(result);
                clients[(IPEndPoint)client.Client.RemoteEndPoint] = client;
                SetupClient(client);
            }
            catch
            {
                //Do nothing if we couldn't accept their connection.
                //They've probably just closed the connection since they first attempted
                //to connect.
            }

            //And start listening for our next connection:
            listener.BeginAcceptTcpClient(new AsyncCallback(Accept), null);
        }

        #endregion

        #region Sending/Receiving Objects

        public Dictionary<string, MemoryStream> clientMemoryStreams = new Dictionary<string, MemoryStream>();

        /// <summary>
        /// Delegate for the NetworkHub when it sends a command
        /// </summary>
        public delegate void NetworkHubCommandEvent(TcpClient client, App.Command cmd);
        /// <summary>
        /// Occurs when [command received].
        /// </summary>
        public event NetworkHubCommandEvent CommandReceived = null;

        /// <summary>
        /// Delegate for the NetworkHub when it receives the Response
        /// </summary>
        public delegate void NetworkHubResponseEvent(TcpClient client, App.Response cmd);
        /// <summary>
        /// Occurs when [response received].
        /// </summary>
        public event NetworkHubResponseEvent ResponseReceived = null;

        /// <summary>
        /// Delegate for the NetworkHub when it sends a message
        /// </summary>
        public delegate void NetworkHubMessageEvent(TcpClient client, string msg);
        /// <summary>
        /// Occurs when [message received].
        /// </summary>
        public event NetworkHubMessageEvent MessageReceived = null;

        /// <summary>
        /// Tests to see if an object is ready to read.
        /// </summary>
        /// <param name="IP">The IP address of the <see cref="App"/> that is sending a <see cref="App.Command"/> object.</param>
        /// <returns>The length of the <see cref="App.Command"/> object being sent over the IP address</returns>
        public int? ObjectReadyToRead(string IP)
        {
            if (clientMemoryStreams[IP].Length >= 4)
            {
                clientMemoryStreams[IP].Seek(0, SeekOrigin.Begin);
                int length = new BinaryReader(clientMemoryStreams[IP]).ReadInt32();

                if (clientMemoryStreams[IP].Length >= length + 4)
                    return length;
            }
            return null;
        }


        /// <summary>
        /// Called when [read].
        /// </summary>
        /// <param name="result">The result of the [read].</param>
        void OnRead(IAsyncResult result)
        {
            BeginReadData data = (BeginReadData)result.AsyncState;
            TcpClient client = data.client;
            string IP = GetClientIP(client);

            try
            {
                MemoryStream stream = clientMemoryStreams[IP];
                int bytesRead = client.GetStream().EndRead(result);
                stream.Seek(0, SeekOrigin.End);
                stream.Write(data.buffer, 0, bytesRead);

                List<object> commands = new List<object>();
                object o;
                while ((o = CheckForMessage(client, IP)) != null)
                    commands.Add(o);

                (new BinaryWriter(new MemoryStream(data.buffer))).Write((int)1);
                BeginReadData next = new BeginReadData(client);
                client.GetStream().BeginRead(next.buffer, 0, data.buffer.Length, new AsyncCallback(OnRead), next);

                foreach (object cmd in commands)
                {
                    if (cmd is App.Command)
                    {
                        if (CommandReceived != null)
                            CommandReceived(client, (App.Command)cmd);
                    }
                    else if (cmd is App.Response)
                    {
                        if (ResponseReceived != null)
                            ResponseReceived(client, (App.Response)cmd);
                    }
                    else if (cmd is string)
                    {
                        if (MessageReceived != null)
                            MessageReceived(client, (string)cmd);
                    }
                }
            }
            catch
            {
                Disconnect(client);
            }
        }

        /// <summary>
        /// Checks for messages on the specified IP.
        /// </summary>
        /// <param name="client">The <see cref="TcpClient"/>.</param>
        /// <param name="IP">The IP.</param>
        /// <returns></returns>
        private object CheckForMessage(TcpClient client, string IP)
        {
            int? length = ObjectReadyToRead(IP);
            if (length != null)
            {
                MemoryStream stream = clientMemoryStreams[IP];
                BinaryReader br = new BinaryReader(stream);

                byte[] rawData = br.ReadBytes((int)length);

                //Clear out the data we just read.
                clientMemoryStreams[IP] = new MemoryStream();
                if (stream.Length > stream.Position)
                {
                    byte[] rest = br.ReadBytes((int)(stream.Length - stream.Position));
                    clientMemoryStreams[IP].Write(rest, 0, rest.Length);
                }

                BinaryFormatter bf = new BinaryFormatter();
                object cmd = bf.Deserialize(new MemoryStream(rawData));
                return cmd;
            }
            return null;
        }

        /// <summary>
        /// Sends an object over the given IP and port.
        /// </summary>
        /// <param name="IP">The IP.</param>
        /// <param name="port">The port.</param>
        /// <param name="o">The object to send.</param>
        public void SendObject(string IP, int port, object o)
        {
            try
            {
                MemoryStream bytes = new MemoryStream();
                new BinaryFormatter().Serialize(bytes, o);

                Stream s = Connect(IP, port).GetStream();

                BinaryWriter bw = new BinaryWriter(s);
                bw.Write((int)bytes.Length);

                bytes.Seek(0, SeekOrigin.Begin);
                byte[] data = new BinaryReader(bytes).ReadBytes((int)bytes.Length);
                bw.Write(data);
            }
            catch { } //No biggie, we just got disconnected.
        }

        #endregion

        #region Making outgoing connections

        /// <summary>
        /// Connects the specified IP.
        /// </summary>
        /// <param name="IP">The IP.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public TcpClient Connect(string IP, int port)
        {
            TcpClient client = null;

            //Standardize the IP address format.
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), port);
            IP = ep.Address.ToString();

            //If we're already connected to them, just return the existing socket.
            if (clients.TryGetValue(ep, out client))
            {
                if (client.Connected)
                    return client;
                else
                    Disconnect(client);
            }

            client = new TcpClient();

            try
            {
                client.Connect(IP, port);
                clients[ep] = client;
                SetupClient(client);
                return client;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Disconnecting from other servers

        /// <summary>
        /// Disconnects the specified client.
        /// </summary>
        /// <param name="client">The client.</param>
        private void Disconnect(TcpClient client)
        {
            Debug.WriteLine("Disconnected");

            string IP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            if (client.Connected)
            {
                client.Client.Close();
                client.Close();
            }

            clients.Remove((IPEndPoint)client.Client.RemoteEndPoint);

            if (Disconnected != null)
                Disconnected(client);
        }

        #endregion
    }
}
