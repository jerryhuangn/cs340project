using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Server;
using System.Net;

namespace cs340project
{
    public partial class NetworkGUI : Form
    {
        App HypeerWeb;

        public NetworkGUI()
        {

            HypeerWeb = App.GetApp("HypeerWeb");

            InitializeComponent();

            IPAddress addr;
            string address = TextPrompt.Show("Which address to listen on?", "127.0.0.1");
            if (address == null || !IPAddress.TryParse(address, out addr))
            {
                throw new Exception("ARGH");
                return;
            }

            int iPort;
            string port = TextPrompt.Show("Which port to listen on?", "30000");
            if (port == null || !int.TryParse(port, out iPort) || iPort < 1000 || iPort > (1 << 16))
            {
                throw new Exception("ARGH");
                return;
            }

            HypeerWeb.Network.Listen(new IPEndPoint(addr, iPort));

            lbl_ip.Text = port;
        }

        public NetworkGUI(string address, string port, bool first)
        {

            HypeerWeb = App.GetApp("HypeerWeb");

            InitializeComponent();

            IPAddress addr;
            //string address = TextPrompt.Show("Which address to listen on?", "127.0.0.1");
            if (address == null || !IPAddress.TryParse(address, out addr))
            {
                throw new Exception("ARGH");
                return;
            }

            int iPort;
            //string port = TextPrompt.Show("Which port to listen on?", "30000");
            if (port == null || !int.TryParse(port, out iPort) || iPort < 1000 || iPort > (1 << 16))
            {
                throw new Exception("ARGH");
                return;
            }

            HypeerWeb.Network.Listen(new IPEndPoint(addr, iPort));

            if (first)
            {

                var menu1= new NetworkGUI(address, "3001", false);
                var menu2 = new NetworkGUI(address, "3002", false);
                var menu3 = new NetworkGUI(address, "3003", false);

                menu1.Show();
                menu2.Show();
                menu3.Show();
            }

            lbl_ip.Text = port;
        }


        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string host = TextPrompt.Show("Which host?", "127.0.0.1");
            if (host == null)
                return;

            int iPort;
            string port = TextPrompt.Show("Which port?", "30000");
            if (port == null || !int.TryParse(port, out iPort) || iPort < 1000 || iPort > (1 << 16))
                return;

            uint iNode;
            string node = TextPrompt.Show("Which node to connect to (and call Create on)?", "0");
            if (node == null || !uint.TryParse(node, out iNode))
                return;

            Node remote = Proxifier.GetProxy<Node>(host, iPort, "HypeerWeb", (int)iNode);

            createNode(remote);
        }

        private void createNode(Node remote)
        {
            Node local = new Node(HypeerWeb.Network.EndPoint);

            //Grab a (temporary) unique ID for this node, so the calls to this
            //node during the remote InsertNode call can work.
            local.Id = 25000;
            while (HypeerWeb.ObjectKeys().Contains((int)local.Id))
                local.Id++;
            HypeerWeb.AddObject((int)local.Id, local);

            local.OnIdSet += new Node.IdSet(local_OnIdSet);
            remote.InsertNode(local);

            refreshHypeerwebDumpToolStripMenuItem_Click(null, null);
        }


        void local_OnIdSet(Node n)
        {
            HypeerWeb.AddObject((int)n.Id, n);
        }

        private void createLocalNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (HypeerWeb.ObjectCount() == 0)
            {
                Node root = new Node(HypeerWeb.Network.EndPoint);
                HypeerWeb.AddObject((int)root.Id, root);
                refreshHypeerwebDumpToolStripMenuItem_Click(null, null);
            }
            else
            {
                foreach (int Id in HypeerWeb.ObjectKeys())
                {
                    createNode((Node)HypeerWeb.GetObject(Id));
                    break;
                }
            }
        }

        private void refreshHypeerwebDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (int Id in HypeerWeb.ObjectKeys())
            {
                Node existing = (Node)HypeerWeb.GetObject(Id);
                txtDump.Text = existing.DumpAllNodes();
                break;
            }
        }

        private void sendMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (HypeerWeb.ObjectCount() != 0)
            {
                foreach (int Id in HypeerWeb.ObjectKeys())
                {
                    sendMessage((Node)HypeerWeb.GetObject(Id));
                    break;
                }
            }
            refreshHypeerwebDumpToolStripMenuItem_Click(null, null);
        }

        private void sendMessage(Node node)
        {
            uint numb = 0;
            Random r = new Random(4356);
            numb = (uint)r.Next(0, HypeerWeb.ObjectCount());
            node.Send(new MessageVisitor("This is a message!"), numb);
        }

        private void BroadcastMessage(Node node)
        {
            node.Broadcast(new MessageVisitor("This is a Broadcast!"));
        }

        private void broadCastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (HypeerWeb.ObjectCount() != 0)
            {
                foreach (int Id in HypeerWeb.ObjectKeys())
                {
                    BroadcastMessage((Node)HypeerWeb.GetObject(Id));
                    break;
                }
            }
            refreshHypeerwebDumpToolStripMenuItem_Click(null, null);
        }
    }
}
