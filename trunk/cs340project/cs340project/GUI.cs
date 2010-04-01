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

namespace cs340project
{
    public partial class GUI : Form
    {
        Node root;
        Node selected;

        public GUI()
        {
            InitializeComponent();
            ConsoleWriter consoleWriter = new ConsoleWriter(textBoxConsole, 1024);
            Console.SetOut(consoleWriter);

            root = new Node(null);
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
                int y = 0;

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
            selected = root.GetNode(uint.Parse(b.Text));
            TbNodeInfo.Text = selected.ToString();

            TcNodeTabs.Enabled = true;
        }

        private void clickAdd(object sender, EventArgs e)
        {
            Node addedNode = new Node(null);
            root.InsertNode(addedNode);
            Console.WriteLine("added node " + addedNode.Id + " to root");

            updateDisplay();
        }

        private void clickRemove(object sender, EventArgs e)
        {
            if (!root.emptyWeb())
            {
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
            //Console.WriteLine("message not broadcast: is this method implemented?");
            uint numberVisited = root.BroadcastWithAck(new MessageVisitor(textBoxMessage.Text), 0);
            Console.WriteLine("message: " + textBoxMessage.Text + " broadcasted to " + numberVisited + "  nodes");
        }

        private void clickSend(object sender, EventArgs e)
        {
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
            Node addedNode = new Node(null);
            selected.InsertNode(addedNode);
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
    }
}
