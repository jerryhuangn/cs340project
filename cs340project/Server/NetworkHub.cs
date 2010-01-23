using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace cs340project
{
    public class NetworkHub
    {
        Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();

        #region Accepting new connections

        int port = 0;
        TcpListener listener = null;
        const int listenBacklogLength = 32;

        public void Listen(int port)
        {
            this.port = port;

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start(listenBacklogLength);

            listener.BeginAcceptTcpClient(new AsyncCallback(Accept), null);
        }

        public delegate void NetworkHubClientEvent(TcpClient client);
        public event NetworkHubClientEvent NewConnection = null;
        public event NetworkHubClientEvent Disconnected = null;

        Dictionary<string,byte[]> clientBuffers = new Dictionary<string,byte[]>();
        Dictionary<string, MemoryStream> clientMemoryStreams = new Dictionary<string, MemoryStream>();
        const int clientBufferSize = 1024;

        void SetupClient(TcpClient client)
        {
            string IP = GetClientIP(client);
            clientBuffers[IP] = new byte[clientBufferSize];
            clientMemoryStreams[IP] = new MemoryStream();
            client.GetStream().BeginRead(clientBuffers[IP], 0, clientBufferSize, new AsyncCallback(OnRead), client);

            if (NewConnection != null)
                NewConnection(client);
        }


        public delegate void NetworkHubCommandEvent(TcpClient client, App.Command cmd);
        public event NetworkHubCommandEvent CommandReceived = null;

        public delegate void NetworkHubResponseEvent(TcpClient client, App.Response cmd);
        public event NetworkHubResponseEvent ResponseReceived = null;

        public delegate void NetworkHubMessageEvent(TcpClient client, string msg);
        public event NetworkHubMessageEvent MessageReceived = null;

        int? ObjectReadyToRead(string IP)
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

        
        void OnRead(IAsyncResult result)
        {
            TcpClient client = (TcpClient)result.AsyncState;
            string IP = GetClientIP(client);

            try {
                MemoryStream stream = clientMemoryStreams[IP];
                int bytesRead = client.GetStream().EndRead(result);
                stream.Seek(0, SeekOrigin.End);
                stream.Write(clientBuffers[IP], 0, bytesRead);

                while (CheckForMessage(client, IP)) ;

                client.GetStream().BeginRead(clientBuffers[IP], 0, clientBufferSize, new AsyncCallback(OnRead), client);
            }
            catch {
                Disconnect(client);
            }
        }

        private bool CheckForMessage(TcpClient client, string IP)
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

                return true;
            }
            return false;
        }

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

        string GetClientIP(TcpClient client) {
            return ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
        }

        void Accept(IAsyncResult result)
        {
            try
            {
                //Accept the one incoming client:
                TcpClient client = listener.EndAcceptTcpClient(result);
                clients[GetClientIP(client)] = client;
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

        #region Making outgoing connections

        TcpClient Connect(string IP, int port)
        {
            TcpClient client = null;

            //Standardize the IP address format.
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), port);
            IP = ep.Address.ToString();

            //If we're already connected to them, just return the existing socket.
            if (clients.TryGetValue(IP, out client))
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
                clients[IP] = client;
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

        private void Disconnect(TcpClient client)
        {
            string IP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            if (client.Connected)
            {
                client.Client.Close();
                client.Close();
            }

            clients.Remove(IP);

            if (Disconnected != null)
                Disconnected(client);
        }

        #endregion
    }
}
