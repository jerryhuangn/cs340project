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

namespace cs340project
{
    /// <summary>
    /// A Proxy is a generic type that allows for an object
    /// to reside on another <see cref="App"/> and be accessed by the 
    /// local <see cref="App"/>
    /// </summary>
    public class Proxy
    {
        string server = null;
        int port = -1;
        int id = -1;
        string app = "Test";
        Dictionary<string, object> responses = new Dictionary<string, object>();

        class WaitingForResponse { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Proxy"/> class.
        /// </summary>
        /// <param name="server">The server for the <see cref="Proxy"/>. Can be either IP or Fully Qualified Domain Name</param>
        /// <param name="port">The port the <see cref="Proxy"/> communicates over.</param>
        /// <param name="id">The id of the <see cref="Proxy"/>.</param>
        public Proxy(string server, int port, string AppName, int id)
        {
            this.server = server;
            this.port = port;
            this.id = id;
            this.app = AppName;

            App.GetApp(app).Network.ResponseReceived += new NetworkHub.NetworkHubResponseEvent(Network_ResponseReceived);
        }

        /// <summary>
        /// Invokes the asynchronous communication of the object on its <see cref="Proxy"/>.
        /// </summary>
        /// <param name="method">The method to be invoked.</param>
        /// <param name="parameters">The parameters for the method being invoked.</param>
        /// <returns></returns>
        public App.Command InvokeAsync(string method, params object[] parameters) {
            App.Command cmd = new App.Command(id, method, parameters);
            responses[cmd.Id] = new WaitingForResponse();
            App.GetApp(app).Network.SendObject(server, port, cmd);
            return cmd;
        }

        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <param name="method">The method to be invoked.</param>
        /// <param name="parameters">The parameters for the method.</param>
        /// <returns></returns>
        public object Invoke(string method, params object[] parameters) {
            //First, just send off our command:
            App.Command cmd = InvokeAsync(method, parameters);

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
    /// The Proxifier class takes the type of another class, and produces a new class named
    /// [ClassName]Proxy (e.g. PersonProxy) that duplicates the public interface of that
    /// class in an inherited class.
    /// 
    /// The class to by proxified must be ISerializable, so we can send it to disk or over
    /// the network.
    /// </summary>
    public class Proxifier
    {
        static Dictionary<Type, Type> ProxyTypes = new Dictionary<Type, Type>();

        /// <summary>
        /// Creates a <see cref="Proxy"/> class for the object of type T.
        /// </summary>
        /// <typeparam name="T">The type of object to be proxified</typeparam>
        /// <param name="server">The server for the <see cref="Proxy"/>. Can be either IP or Fully Qualified Domain Name</param>
        /// <param name="port">The port the <see cref="Proxy"/> communicates over.</param>
        /// <param name="id">The id of the <see cref="Proxy"/>.</param>
        /// <returns>A <see cref="Proxy"/> of type T</returns>
        public static T GetProxy<T>(string server, int port, string AppName, int id)
        {
            Type proxy = Proxifier.CreateProxy(typeof(T));
            return (T)Activator.CreateInstance(proxy, server, port, AppName, id);
        }

        /// <summary>
        /// Creates the proxy using Reflection.
        /// </summary>
        /// <param name="original">The original type.</param>
        /// <returns>A new type that contains all the same methods and properties
        /// as the original type, only overridden to utilies the <see cref="Proxy"/> class</returns>
        static Type CreateProxy(Type original)
        {
            Type ret;
            if (!ProxyTypes.TryGetValue(original, out ret))
            {
                string code = Proxifier.CreateProxyCode(original);
                System.Diagnostics.Debug.WriteLine(code);

                CodeDomProvider compiler = CodeDomProvider.CreateProvider("C#");
                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateInMemory = true;
                parameters.ReferencedAssemblies.Add(Assembly.GetAssembly(original).Location);
                parameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
                CompilerResults result = compiler.CompileAssemblyFromSource(parameters, new string[] { code });
                ret = result.CompiledAssembly.GetType(original.Namespace + "." + original.Name + "Proxy");

                ProxyTypes[original] = ret;
            }
            return ret;
        }

        /// <summary>
        /// Creates the  code for the <see cref="Proxy"/> class.
        /// </summary>
        /// <param name="original">The original type.</param>
        /// <returns>A string that contains all the source code to create a proxy
        /// class containing the same structure as the original Type.</returns>
        static string CreateProxyCode(Type original) {
            StringBuilder ret = new StringBuilder();

            ret.AppendLine("using System;");
            ret.AppendLine("namespace " + original.Namespace);
            ret.AppendLine("{");
            ret.AppendLine("\tclass " + original.Name + "Proxy : "+original.Namespace+"."+original.Name);
            ret.AppendLine("\t{");

            ret.AppendLine("\t\tProxy proxy = null;");
            ret.AppendLine("\t\tpublic " + original.Name + "Proxy(string server, int port, string AppName, int id)");
            ret.AppendLine("\t\t{");
            ret.AppendLine("\t\t\tproxy = new Proxy(server, port, AppName, id);");
            ret.AppendLine("\t\t}");


            foreach (PropertyInfo property in original.GetProperties())
            {
                if (!property.GetAccessors()[0].IsVirtual)
                    continue;

                ret.AppendLine("\t\tpublic override " + property.PropertyType.Namespace + "." + property.PropertyType.Name+" " + property.Name);
                ret.AppendLine("\t\t{");

                foreach (MethodInfo method in property.GetAccessors())
                {
                    if (method.ReturnType == property.PropertyType)
                    {
                        ret.AppendLine("\t\t\tget");
                        ret.AppendLine("\t\t\t{");
                        ret.Append("\t\t\t\treturn ("+property.PropertyType.Namespace+"."+property.PropertyType.Name+")this.proxy.Invoke(\""+method.Name+"\"");
                        foreach (ParameterInfo parameter in method.GetParameters())
                            ret.Append(", " + parameter.Name);
                        ret.AppendLine(");");
                        ret.AppendLine("\t\t\t}");
                    }
                    else if(method.ReturnType.Name == "Void")
                    {
                        
                        ret.AppendLine("\t\t\tset");
                        ret.AppendLine("\t\t\t{");
                        ret.Append("\t\t\t\tthis.proxy.InvokeAsync(\"" + method.Name + "\"");
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

                if (method.ReturnType.Namespace == "System" && method.ReturnType.Name == "Void")
                    ret.Append("void");
                else
                    ret.Append(method.ReturnType.Namespace + "." + method.ReturnType.Name);

                ret.Append(" " + method.Name + "(");

                bool first = true;
                foreach (ParameterInfo parameter in method.GetParameters())
                {
                    if (!first)
                        ret.Append(", ");
                    first = false;

                    ret.Append(parameter.ParameterType.Namespace + "." + parameter.ParameterType.Name);
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
                    ret.Append("\t\t\tthis.proxy.InvokeAsync(\"" + method.Name + "\"");
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
