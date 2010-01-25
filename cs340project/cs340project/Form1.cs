using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace cs340project
{
    public partial class Form1 : Form
    {
        App App = App.GetApp("Test");

        public Form1()
        {
            InitializeComponent();
            App.Network.NewConnection += new NetworkHub.NetworkHubClientEvent(Network_NewConnection);
            App.Network.Disconnected += new NetworkHub.NetworkHubClientEvent(Network_Disconnected);
            App.Network.MessageReceived += new NetworkHub.NetworkHubMessageEvent(Network_MessageReceived);
            App.Network.CommandReceived += new NetworkHub.NetworkHubCommandEvent(Network_CommandReceived);
            App.Network.ResponseReceived += new NetworkHub.NetworkHubResponseEvent(Network_ResponseReceived);
            App.AddObject(new Person());
            App.AddObject(new Person());
            App.AddObject(new Person());
            App.AddObject(new Person());
            App.AddObject(new Person());
        }


        #region Log output

        void Network_CommandReceived(System.Net.Sockets.TcpClient client, App.Command cmd)
        {
            Invoke(new ThreadStart(() =>
            {
                txtOutput.AppendText((((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()) + ":" + cmd.ObjectId+"."+cmd.Name+"(");
                foreach (object o in cmd.Parameters)
                    txtOutput.AppendText((o==null ? "null" : o.ToString()) + ",");
                txtOutput.AppendText(")" + Environment.NewLine);
            }));
        }
        void Network_ResponseReceived(System.Net.Sockets.TcpClient client, App.Response cmd)
        {
            Invoke(new ThreadStart(() =>
            {
                if(cmd.ReturnValue != null)
                    txtOutput.AppendText((((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()) + ":Responded with " + cmd.ReturnValue.ToString() + Environment.NewLine);
            }));
        }

        void Network_MessageReceived(System.Net.Sockets.TcpClient client, string msg)
        {
            Invoke(new ThreadStart(() =>
            {
                txtOutput.AppendText((((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()) + ":" + msg + Environment.NewLine);
            }));
        }

        void Network_Disconnected(System.Net.Sockets.TcpClient client)
        {
            Invoke(new ThreadStart(() =>
            {
                txtOutput.AppendText("Disconnected from server at " + (((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()) + Environment.NewLine);
            }));
        }

        void Network_NewConnection(System.Net.Sockets.TcpClient client)
        {
            Invoke(new ThreadStart(() =>
            {
                txtOutput.AppendText("Connected to server at " + (((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()) + Environment.NewLine);
            }));
        }

        #endregion

        private void button1_Click_1(object sender, EventArgs e)
        {
            App.Network.Listen(10000);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Person remote = Proxifier.GetProxy<Person>("127.0.0.1", 10000, "Test", 2);
            remote.Age = 15;
            remote.MyName = new PersonName("Ben", "Dilts", "Beandog");

            txtOutput.AppendText("Remote says Age is now " + remote.Age + Environment.NewLine);
            txtOutput.AppendText("Remote says Nick is now " + remote.MyName.Nick + Environment.NewLine);
        }
    }
}
