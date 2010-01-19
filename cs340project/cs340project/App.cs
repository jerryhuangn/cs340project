using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Net.Sockets;
using System.Net;
using System.Reflection;

namespace cs340project
{
    class App
    {
        #region Factory

        static Dictionary<string, App> apps = new Dictionary<string, App>();

        public static App GetApp(string name)
        {
            App ret;
            if (apps.TryGetValue(name, out ret))
                return ret;

            ret = new App();
            apps[name] = ret;

            return ret;
        }

        #endregion

        #region Network

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

        void Accept(IAsyncResult result)
        {
            try
            {
                //Accept the one incoming client:
                TcpClient client = listener.EndAcceptTcpClient(result);
                string IP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                clients[IP] = client;
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

        public TcpClient Connect(string IP, int port)
        {
            TcpClient client = null;

            //Standardize the IP address format.
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), port);
            IP = ep.ToString();

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
        }

        #endregion

        #endregion

        #region Object/command infrastructure

        List<ISerializable> objects = new List<ISerializable>();

        public class Command : ISerializable {
            public int ObjectId;
            public string Name;
            public object[] Parameters;
            public string Id;

            public Command(int id, string name, object[] p) {
                ObjectId = id;
                Name = name;
                Parameters = p;
                Id = Guid.NewGuid().ToString();
            }

            #region ISerializable Members

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("Id", Id);
                info.AddValue("ObjectId", ObjectId);
                info.AddValue("Name", Name);
                info.AddValue("Parameters", Parameters);
            }

            #endregion
        }

        public object RunCommand(Command cmd)
        {
            if (objects.Count <= cmd.ObjectId || objects[cmd.ObjectId] == null)
                throw new ArgumentOutOfRangeException("ObjectId");

            object target = objects[cmd.ObjectId];
            Type type = target.GetType();

            List<Type> paramTypes = new List<Type>();
            foreach(object p in cmd.Parameters)
                paramTypes.Add(p.GetType());

            MethodInfo method = type.GetMethod(cmd.Name, paramTypes.ToArray());
            return method.Invoke(target, cmd.Parameters);
        }

        #endregion
    }
}
