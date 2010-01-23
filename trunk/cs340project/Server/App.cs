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
    public class App
    {
        #region Factory

        static Dictionary<string, App> apps = new Dictionary<string, App>();

        public static App GetApp(string name)
        {
            App ret;
            if (apps.TryGetValue(name, out ret))
                return ret;

            ret = new App(name);
            apps[name] = ret;

            return ret;
        }

        #endregion

        public NetworkHub Network;
        public string Name;

        private App(string name)
        {
            Name = name;
            Network = new NetworkHub();

            Network.CommandReceived += new NetworkHub.NetworkHubCommandEvent(Network_CommandReceived);
        }

        void Network_CommandReceived(TcpClient client, App.Command cmd)
        {
            Response ret = new Response(cmd.Id, RunCommand(cmd));
        }


        #region Object/command infrastructure

        List<object> objects = new List<object>();

        [Serializable]
        public class Command
        {
            public int? ObjectId;
            public string Name;
            public object[] Parameters;
            public string Id;

            public Command(int? id, string name, object[] p)
            {
                ObjectId = id;
                Name = name;
                Parameters = p;
                Id = Guid.NewGuid().ToString();
            }
        }

        [Serializable]
        public class Response
        {
            public string Id;
            public object ReturnValue;

            public Response(string id, object ret)
            {
                Id = id;
                ReturnValue = ret;
            }
        }

        public object RunCommand(Command cmd)
        {
            if (objects.Count <= cmd.ObjectId || objects[(int)cmd.ObjectId] == null)
                throw new ArgumentOutOfRangeException("ObjectId");

            object target = objects[(int)cmd.ObjectId];
            Type type = target.GetType();

            List<Type> paramTypes = new List<Type>();
            foreach(object p in cmd.Parameters)
                paramTypes.Add(p.GetType());

            MethodInfo method = type.GetMethod(cmd.Name, paramTypes.ToArray());
            return method.Invoke(target, cmd.Parameters);
        }

        #endregion

        public void AddObject(object o)
        {
            objects.Add(o);
        }
    }
}
