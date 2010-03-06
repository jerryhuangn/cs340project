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

        public GUI()
        {
            InitializeComponent();
            ConsoleWriter consoleWriter = new ConsoleWriter(textBoxConsole, 1024);
            Console.SetOut(consoleWriter);

            root = new Node();
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
            textBoxDisplay.Text = Node.DumpAllNodes();
        }

        private void clickAdd(object sender, EventArgs e)
        {
            Node addedNode = root.CreateNode();
            Console.WriteLine("added node " + addedNode.Id + " to root");

            updateDisplay();
        }

        private void clickRemove(object sender, EventArgs e)
        {
            Console.WriteLine("node not removed: is this method implemented?");

            updateDisplay();
        }

        private void clickBroadcast(object sender, EventArgs e)
        {
            Console.WriteLine("message not broadcast: is this method implemented?");
            //Console.WriteLine("broadcasting message: " + textBoxMessage.Text);
        }

        private void clickSend(object sender, EventArgs e)
        {
            Console.WriteLine("message not sent: is this method implemented?");
            //Console.WriteLine("sending message: " + textBoxMessage.Text);
        }
    }
}
