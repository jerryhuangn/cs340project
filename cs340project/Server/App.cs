using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using Server;
using System.Diagnostics;

namespace cs340project
{
    /// <summary>
    /// Class owner: Ben Dilts
    /// 
    /// Class that contains all the information
    /// for a single node in the Application.
    /// </summary>
    public class App
    {
        #region Factory

        static Dictionary<string, App> apps = new Dictionary<string, App>();

        /// <summary>
        /// Gets the <see cref="App"/> with the supplied name. If there is no
        /// <see cref="App"/> with the supplied name, then a new instance of the 
        /// <see cref="App"/> class is initialized and returned.
        /// </summary>
        /// <param name="name">The name of the <see cref="App"/>.</param>
        /// <returns>the <see cref="App"/> with the given name</returns>
        public static App GetApp(string name)
        {
            App ret;
            if(apps.TryGetValue(name, out ret))
                return ret;

            ret = new App(name);
            apps[name] = ret;

            return ret;
        }

        #endregion

        /// <summary>
        /// The newtork hub for the current App
        /// </summary>
        public NetworkHub Network;
        /// <summary>
        /// The current App's Name
        /// </summary>
        public string Name;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        /// <param name="name">The name of the <see cref="App"/>.</param>
        public App(string name)
        {
            Name = name;
            Network = new NetworkHub();

            Network.CommandReceived += new NetworkHub.NetworkHubCommandEvent(Network_CommandReceived);
        }

        /// <summary>
        /// Methode that handles the <see cref="App.Command"/> object received from the Network Communication.
        /// </summary>
        /// <param name="client">The <see cref="TcpClient"/> client.</param>
        /// <param name="cmd">The <see cref="App.Command"/> object that holds the commands to execute.</param>
        void Network_CommandReceived(TcpClient client, App.Command cmd)
        {
            object ReturnValue = RunCommand(cmd);
            if (ReturnValue is ISerializeMutator && !ReturnValue.GetType().Name.EndsWith("Proxy"))
                ReturnValue = ((ISerializeMutator)ReturnValue).ObjectToSerialize();

            Response ret = new Response(cmd.Id, ReturnValue);
            Network.SendObject(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), ((IPEndPoint)client.Client.RemoteEndPoint).Port, ret);
        }


        #region Object/command infrastructure

        Dictionary<int,object> objects = new Dictionary<int,object>();

        /// <summary>
        /// The object that holds the commands to be sent 
        /// back and forth from the nodes in the application.
        /// </summary>
        [Serializable]
        public class Command
        {
            /// <summary>
            /// The Object's Id
            /// </summary>
            public int? ObjectId;
            /// <summary>
            /// The Command's Name
            /// </summary>
            public string Name;
            /// <summary>
            /// The parameters for the Command being sent
            /// </summary>
            public object[] Parameters;
            /// <summary>
            /// The Id of the Command
            /// </summary>
            public string Id;

            /// <summary>
            /// Initializes a new instance of the <see cref="Command"/> class.
            /// </summary>
            /// <param name="id">The id.</param>
            /// <param name="name">The name.</param>
            /// <param name="p">The p.</param>
            public Command(int? id, string name, object[] p)
            {
                ObjectId = id;
                Name = name;
                Parameters = p;
                Id = Guid.NewGuid().ToString();

                for (int i = 0; i < p.Length; i++)
                {
                    if (p[i] is ISerializeMutator && !p[i].GetType().Name.EndsWith("Proxy"))
                        p[i] = ((ISerializeMutator)p[i]).ObjectToSerialize();
                }
            }
        }

        /// <summary>
        /// A class that holds the values for a response from the Network Communication
        /// </summary>
        [Serializable]
        public class Response
        {
            /// <summary>
            /// The Response Id
            /// </summary>
            public string Id;
            /// <summary>
            /// The Response's Return Value
            /// </summary>
            public object ReturnValue;

            /// <summary>
            /// Initializes a new instance of the <see cref="Response"/> class.
            /// </summary>
            /// <param name="id">The id of the <see cref="Proxy"/> object.</param>
            /// <param name="ret">The return object.</param>
            public Response(string id, object ret)
            {
                Id = id;
                ReturnValue = ret;
            }
        }


        /// <summary>
        /// Runs the command from the <see cref="App.Command"/> object.
        /// </summary>
        /// <param name="cmd">The <see cref="App.Command"/> object.</param>
        /// <returns></returns>
        public object RunCommand(Command cmd)
        {
            Debug.WriteLine("Running command " + cmd.Name + " on endpoint " + Network.EndPoint.ToString());

            if (!objects.ContainsKey((int)cmd.ObjectId))
                throw new ArgumentOutOfRangeException("ObjectId");

            object target = objects[(int)cmd.ObjectId];
            Type type = target.GetType();

            MethodInfo method = type.GetMethod(cmd.Name);
            return method.Invoke(target, cmd.Parameters);
        }

        #endregion

        /// <summary>
        /// Adds the object to the List inside of the <see cref="App"/>.
        /// </summary>
        /// <param name="o">The object to be added.</param>
        public void AddObject(int i, object o)
        {
            objects[i] = o;
        }

        public object GetObject(int i)
        {
            return objects[i];
        }

        public int ObjectCount()
        {
            return objects.Count;
        }

        public Dictionary<int,object>.KeyCollection ObjectKeys()
        {
            return objects.Keys;
        }
    }
}
