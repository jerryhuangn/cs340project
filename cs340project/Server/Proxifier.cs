using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace cs340project
{
    /// <summary>
    /// Class owner: Ben Dilts
    /// 
    /// A Proxy is a generic utility class that allows for an object
    /// to reside on another <see cref="App"/> to be accessed by a local
    /// specific XXXXProxy.
    /// </summary>
    [Serializable]
    public class Proxy
    {
        string server = null;
        int port = -1;
        int id = -1;
        string app = "Test";

        [NonSerialized]
        Dictionary<string, object> responses = new Dictionary<string, object>();

        [OnDeserialized]
        void AfterUnserialize(StreamingContext context)
        {
            Debug.WriteLine("Listening to ResponseReceived...");
            App.GetApp(app).Network.ResponseReceived += new NetworkHub.NetworkHubResponseEvent(Network_ResponseReceived);
            responses = new Dictionary<string, object>();
        }

        class WaitingForResponse { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Proxy"/> class.
        /// </summary>
        /// <param name="server">The server for the <see cref="Proxy"/>. Can be either IP or Fully Qualified Domain Name</param>
        /// <param name="port">The port the <see cref="Proxy"/> communicates over.</param>
        /// <param name="appName">The name of the App on the other side of the network that we're connecting to.</param>
        /// <param name="id">The id of the <see cref="Proxy"/>.</param>
        public Proxy(string server, int port, string appName, int id)
        {
            this.server = server;
            this.port = port;
            this.id = id;
            this.app = appName;

            Debug.WriteLine("Proxy created for server " + server + ", port " + port + ", app " + appName + ", id " + id);

            Debug.WriteLine("Listening to ResponseReceived...");
            App.GetApp(app).Network.ResponseReceived += new NetworkHub.NetworkHubResponseEvent(Network_ResponseReceived);
        }

        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <param name="method">The method to be invoked.</param>
        /// <param name="parameters">The parameters for the method.</param>
        /// <returns></returns>
        public object Invoke(string method, params object[] parameters) {
            //First, just send off our command:
            App.Command cmd = new App.Command(id, method, parameters);
            responses[cmd.Id] = new WaitingForResponse();
            App.GetApp(app).Network.SendObject(server, port, cmd);

            //Wait for a reply to cmd.Id
            while (responses[cmd.Id] is WaitingForResponse)
                Application.DoEvents();

            object ret = responses[cmd.Id];
            responses.Remove(cmd.Id);
            return ret;
        }

        /// <summary>
        /// Methode that handles the response received from the Network Communication.
        /// </summary>
        /// <param name="client">The <see cref="TcpClient"/> client.</param>
        /// <param name="cmd">The <see cref="App.Command"/> object that holds the commands to execute.</param>
        void Network_ResponseReceived(TcpClient client, App.Response cmd)
        {
            responses[cmd.Id] = cmd.ReturnValue;
        }
    }

    /// <summary>
    /// Class owner: Ben Dilts
    /// 
    /// The Proxifier class takes the type of another class, and produces a new class named
    /// [ClassName]Proxy (e.g. PersonProxy) that duplicates the public virtual interface of that
    /// class in an inherited class.
    /// </summary>
    public class Proxifier
    {
        static Dictionary<Type, Type> ProxyTypes = new Dictionary<Type, Type>();

        /// <summary>
        /// Creates an instance of a proxy class for the type T.
        /// </summary>
        /// <typeparam name="T">The type of object to be proxified</typeparam>
        /// <param name="server">The server for the <see cref="Proxy"/>. Can be either IP or Fully Qualified Domain Name</param>
        /// <param name="port">The port the <see cref="Proxy"/> communicates over.</param>
        /// <param name="AppName">Name of the app.</param>
        /// <param name="id">The id of the <see cref="Proxy"/>.</param>
        /// <returns>A <see cref="Proxy"/> of type T</returns>
        public static T GetProxy<T>(string server, int port, string AppName, int id)
        {
            Type proxy = Proxifier.CreateProxyClass(typeof(T));
            return (T)Activator.CreateInstance(proxy, server, port, AppName, id);
        }

        /// <summary>
        /// Compiles a proxy class for type original using Reflection.
        /// </summary>
        /// <param name="original">The original type.</param>
        /// <returns>A new type that contains all the same methods and properties
        /// as the original type, only overridden to utilies the <see cref="Proxy"/> class</returns>
        public static Type CreateProxyClass(Type original)
        {
            Type ret;
            if (!ProxyTypes.TryGetValue(original, out ret))
            {
                List<Assembly> Assemblies = new List<Assembly>();

                string code = Proxifier.CreateProxyCode(original, Assemblies);
                //System.Diagnostics.Debug.WriteLine(code);

                CodeDomProvider compiler = CodeDomProvider.CreateProvider("C#");
                CompilerParameters parameters = new CompilerParameters();
                
                parameters.GenerateInMemory = false;
                parameters.OutputAssembly = original.Name + "Proxy.dll";

                parameters.ReferencedAssemblies.Add(Assembly.GetAssembly(original).Location);
                parameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
                foreach (Assembly a in Assemblies)
                    parameters.ReferencedAssemblies.Add(a.Location);

                CompilerResults result = compiler.CompileAssemblyFromSource(parameters, new string[] { code });
                ret = result.CompiledAssembly.GetType(original.Namespace + "." + original.Name + "Proxy");

                ProxyTypes[original] = ret;
            }
            return ret;
        }

        static string TypeString(Type t)
        {
            if (t.Namespace == "System" && t.Name == "Void")
                return "void";

            string ret = t.Namespace + "." + t.Name;
            ret = (new Regex("`.*$").Replace(ret, ""));

            if (t.IsGenericType)
            {
                List<string> p = new List<string>();
                foreach (Type arg in t.GetGenericArguments())
                    p.Add(TypeString(arg));

                ret += '<' + string.Join(",",p.ToArray()) + '>';
            }

            return ret;
        }

        /// <summary>
        /// Creates the code for a proxy class from the Type original, which may then later be compiled.
        /// </summary>
        /// <param name="original">The original type.</param>
        /// <returns>A string that contains all the source code to create a proxy
        /// class containing the same structure as the original Type.</returns>
        static string CreateProxyCode(Type original, List<Assembly> Assemblies)
        {
            StringBuilder ret = new StringBuilder();

            ret.AppendLine("using System;");
            ret.AppendLine("using System.Runtime.Serialization;");
            ret.AppendLine("namespace " + original.Namespace);
            ret.AppendLine("{");
            ret.AppendLine("\t[Serializable]");
            ret.AppendLine("\tclass " + original.Name + "Proxy : "+original.Namespace+"."+original.Name);
            ret.AppendLine("\t{");

            ret.AppendLine("\t\tcs340project.Proxy proxy = null;");
            ret.AppendLine("\t\tpublic " + original.Name + "Proxy(string server, int port, string AppName, int id)");
            ret.AppendLine("\t\t{");
            ret.AppendLine("\t\t\tproxy = new cs340project.Proxy(server, port, AppName, id);");
            ret.AppendLine("\t\t}");


            foreach (PropertyInfo property in original.GetProperties())
            {
                if (!property.GetAccessors()[0].IsVirtual)
                    continue;

                Assemblies.Add(Assembly.GetAssembly(property.PropertyType));
                ret.AppendLine("\t\tpublic override " + TypeString(property.PropertyType)+" " + property.Name);
                ret.AppendLine("\t\t{");

                foreach (MethodInfo method in property.GetAccessors())
                {
                    if (method.ReturnType == property.PropertyType)
                    {
                        ret.AppendLine("\t\t\tget");
                        ret.AppendLine("\t\t\t{");

                        Assemblies.Add(Assembly.GetAssembly(property.PropertyType));
                        ret.Append("\t\t\t\treturn ("+TypeString(property.PropertyType)+")this.proxy.Invoke(\""+method.Name+"\"");
                        foreach (ParameterInfo parameter in method.GetParameters())
                            ret.Append(", " + parameter.Name);
                        ret.AppendLine(");");
                        ret.AppendLine("\t\t\t}");
                    }
                    else if(method.ReturnType.Name == "Void")
                    {
                        
                        ret.AppendLine("\t\t\tset");
                        ret.AppendLine("\t\t\t{");
                        ret.Append("\t\t\t\tthis.proxy.Invoke(\"" + method.Name + "\"");
                        foreach (ParameterInfo parameter in method.GetParameters())
                            ret.Append(", " + parameter.Name);
                        ret.AppendLine(");");
                        ret.AppendLine("\t\t\t}");
                    }
                }
                ret.AppendLine("\t\t}");
            }
            
            foreach (MethodInfo method in original.GetMethods())
            {
                //For right now, skip getters/setters.  We'll get those elsewhere.
                if (method.IsSpecialName)
                    continue;

                if (!method.IsVirtual)
                    continue;

                ret.Append("\t\tpublic override ");
                if (method.IsStatic)
                    ret.Append("static ");

                Assemblies.Add(Assembly.GetAssembly(method.ReturnType));
                ret.Append(TypeString(method.ReturnType));

                ret.Append(" " + method.Name + "(");

                bool first = true;
                foreach (ParameterInfo parameter in method.GetParameters())
                {
                    if (!first)
                        ret.Append(", ");
                    first = false;

                    Assemblies.Add(Assembly.GetAssembly(parameter.ParameterType));
                    ret.Append(TypeString(parameter.ParameterType));
                    ret.Append(" " + parameter.Name);
                }
                ret.AppendLine(")");
                ret.AppendLine("\t\t{");
                if (method.ReturnType.Name != "Void")
                {
                    ret.Append("\t\t\treturn (" + method.ReturnType.Namespace + "." + method.ReturnType.Name + ")this.proxy.Invoke(\"" + method.Name + "\"");
                    foreach (ParameterInfo parameter in method.GetParameters())
                        ret.Append(", " + parameter.Name);
                    ret.AppendLine(");");
                }
                else
                {
                    ret.Append("\t\t\tthis.proxy.Invoke(\"" + method.Name + "\"");
                    foreach (ParameterInfo parameter in method.GetParameters())
                        ret.Append(", " + parameter.Name);
                    ret.AppendLine(");");
                }

                ret.AppendLine("\t\t}");
            }

            ret.AppendLine("\t}");
            ret.AppendLine("}");

            return ret.ToString();
        }
    }
}
