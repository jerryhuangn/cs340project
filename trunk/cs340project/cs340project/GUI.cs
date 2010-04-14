using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Server;
using System.Net;

namespace cs340project
{
    public partial class GUI : Form
    {
        Node selected;
        App HypeerWeb;

        /// <summary>
        /// Initializes a new instance of the <see cref="GUI"/> class.
        /// </summary>
        public GUI()
        {
            HypeerWeb = App.GetApp("HypeerWeb");

            InitializeComponent();
            ConsoleWriter consoleWriter = new ConsoleWriter(textBoxConsole, 1024);
            Console.SetOut(consoleWriter);

            IPAddress addr;
            string address = TextPrompt.Show("Which address to listen on?", "127.0.0.1");
            if (address == null || !IPAddress.TryParse(address, out addr))
            {
                throw new Exception("ARGH");
            }

            int iPort;
            string port = TextPrompt.Show("Which port to listen on?", "30000");
            if (port == null || !int.TryParse(port, out iPort) || iPort < 1000 || iPort > (1 << 16))
            {
                throw new Exception("ARGH");
            }

            HypeerWeb.Network.Listen(new IPEndPoint(addr, iPort));

            updateDisplay();
        }

        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);
        /// <summary>
        /// This method provides a thread safe way of setting a GUI control's property.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }

        private delegate void CallControlFunctionThreadSafeDelegate(Control control, string functionName);
        /// <summary>
        /// This method provides a thread safe way of calling a function GUI control's function (with no parameters).
        /// </summary>
        /// <param name="control"></param>
        /// <param name="functionName"></param>
        public static void CallControlFunctionThreadSafe(Control control, string functionName)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new CallControlFunctionThreadSafeDelegate(CallControlFunctionThreadSafe), new object[] { control, functionName });
            }
            else
            {
                control.GetType().InvokeMember(functionName, BindingFlags.InvokeMethod, null, control, null);
            }
        }

        private void updateDisplay()
        {
            Node root = getANode();
            if (root == null)
                return;
            textBoxDisplay.Text = root.DumpAllNodes();

            SplitDisplay.Panel1.Controls.Clear();
            CbListOfNodes.Items.Clear();

            for (uint i = 0; i < root.HypeerWebSize(); i++)
            {
                CbListOfNodes.Items.Add(i);

                Button c = new Button();

                c.Width = 30;
                c.Height = 30;

                int tempLeng = (SplitDisplay.Panel1.Width) / 35;
                int tempId = (int)i;
                int y = menuStrip1.Height + 5;

                while (tempId >= tempLeng)
                {
                    y += 35;
                    tempId -= tempLeng;
                }

                int x = 35 * tempId;

                c.Location = new Point(x, y);
                c.Text = i + "";

                c.Click += new EventHandler(c_Click);
                SplitDisplay.Panel1.Controls.Add(c);
            }
            CbListOfNodes.SelectedIndex = -1;
            CbListOfNodes.Text = "";
        }

        void c_Click(object sender, EventArgs e)
        {
            var b = (Button)sender;
            selected = (Node)HypeerWeb.GetObject(int.Parse(b.Text));
            TbNodeInfo.Text = selected.ToString();

            TcNodeTabs.Enabled = true;
        }

        private void clickAdd(object sender, EventArgs e)
        {
            Node addedNode = null;

            foreach (int Id in HypeerWeb.ObjectKeys())
            {
                addedNode = createNode((Node)HypeerWeb.GetObject(Id));
                break;
            }
            if (addedNode == null)
                return;
            Console.WriteLine("added node " + addedNode.Id + " to root");

            updateDisplay();
        }

        private void clickRemove(object sender, EventArgs e)
        {
            if (HypeerWeb.ObjectCount() > 1)
            {
                Node root = getANode();

                if (root == null)
                    return;

                Random r = new Random();

                //uint nodeToRemove = (uint)r.Next(0, Node.AllNodes.Count);
                uint nodeToRemove = (uint)r.Next(1, (int)root.HypeerWebSize());
                root.RemoveById(nodeToRemove);
                Console.WriteLine("removed node " + nodeToRemove + " from root");

                updateDisplay();
            }
            else
            {
                Console.WriteLine("root node has no more children to delete");
            }
        }

        private void clickBroadcast(object sender, EventArgs e)
        {
            Node root = getANode();

            if (root == null)
                return;

            //Console.WriteLine("message not broadcast: is this method implemented?");
            uint numberVisited = root.BroadcastWithAck(new MessageVisitor(textBoxMessage.Text), 0);
            Console.WriteLine("message: " + textBoxMessage.Text + " broadcasted to " + numberVisited + "  nodes");
        }

        private void clickSend(object sender, EventArgs e)
        {
            Node root = getANode();

            if (root == null)
                return;

            //Console.WriteLine("message not sent: textBoxMessage.Text");
            root.Send(new MessageVisitor(textBoxMessage.Text), 0);
            Console.WriteLine("sending message: " + textBoxMessage.Text);
        }

        private void GUI_ResizeEnd(object sender, EventArgs e)
        {
            updateDisplay();
        }

        private void BtAddNodeFromNode_Click(object sender, EventArgs e)
        {
            Node addedNode = createNode(selected);
            Console.WriteLine("added node " + addedNode.Id + " to " + selected.Id);
            updateDisplay();
        }

        private void BtRemoveThisNode_Click(object sender, EventArgs e)
        {
            Console.WriteLine("removed node " + selected.Id + " from self");
            selected.Remove();
            selected = null;

            TcNodeTabs.Enabled = false;
            TbNodeInfo.Text = "";
            updateDisplay();
        }

        private void btBroadcastMessageFromNode_Click(object sender, EventArgs e)
        {
            if (cbAckBroadcastFrom.Checked)
                MessageBox.Show("Number of nodes reached: " + selected.BroadcastWithAck(new MessageVisitor(tbBroadCastFromNode.Text), 0));
            else
                selected.Broadcast(new MessageVisitor(tbBroadCastFromNode.Text));
            updateDisplay();
        }

        private void BtRmove_Click(object sender, EventArgs e)
        {
            if (CbListOfNodes.SelectedIndex < 0)
                MessageBox.Show("Please select a node from the dropdown");
            else
                selected.RemoveById((uint)CbListOfNodes.SelectedIndex);
            selected = null;

            TcNodeTabs.Enabled = false;
            TbNodeInfo.Text = "";
            updateDisplay();
        }

        private void btSendMessageFromNode_Click(object sender, EventArgs e)
        {
            if (CbListOfNodes.SelectedIndex < 0)
                MessageBox.Show("Please select a node from the dropdown");
            else
                selected.Send(new MessageVisitor(tbMessageFromNode.Text), (uint)CbListOfNodes.SelectedIndex);
            updateDisplay();
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

            Node root = Proxifier.GetProxy<Node>(host, iPort, "HypeerWeb", (int)iNode);

            createNode(root);
        }

        private Node createNode(Node remote)
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

            return local;
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
            updateDisplay();
        }

        private Node getANode()
        {
            foreach (int Id in HypeerWeb.ObjectKeys())
            {
                return (Node)HypeerWeb.GetObject(Id);
            }
            return null;
        }
    }
}
