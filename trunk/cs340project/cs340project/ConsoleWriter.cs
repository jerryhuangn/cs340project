using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace cs340project
{
    class ConsoleWriter : StringWriter
    {
        int capacity;
        TextBox box;
        StringBuilder builder;

        public ConsoleWriter(TextBox box, int capacity)
        {
            this.capacity = capacity;
            this.box = box;
            builder = this.GetStringBuilder();
        }

        private void updateTextBox()
        {
            Debug.WriteLine("before builder length: " + builder.Length);
            // pretty sure this is not very efficient
            if (builder.Length > capacity)
                builder.Remove(0, builder.Length - capacity);
            Debug.WriteLine("after builder length:  " + builder.Length);
            GUI.SetControlPropertyThreadSafe(box, "Text", this.ToString());
            GUI.SetControlPropertyThreadSafe(box, "SelectionStart", box.Text.Length);
            GUI.CallControlFunctionThreadSafe(box, "ScrollToCaret");
        }        

        public override void Write(bool value)
        {
            base.Write(value);
            updateTextBox();
        }

        public override void Write(char[] buffer)
        {
            base.Write(buffer);
            updateTextBox();
        }

        public override void Write(decimal value)
        {
            base.Write(value);
            updateTextBox();
        }

        public override void Write(char value)
        {
            base.Write(value);
            updateTextBox();
        }

        public override void Write(char[] buffer, int index, int count)
        {
            base.Write(buffer, index, count);
            updateTextBox();
        }

        public override void Write(double value)
        {
            base.Write(value);
            updateTextBox();
        }

        public override void Write(float value)
        {
            base.Write(value);
            updateTextBox();
        }

        public override void Write(int value)
        {
            base.Write(value);
            updateTextBox();
        }

        public override void Write(long value)
        {
            base.Write(value);
            updateTextBox();
        }

        public override void Write(object value)
        {
            base.Write(value);
            updateTextBox();
        }

        public override void Write(string format, object arg0)
        {
            base.Write(format, arg0);
            updateTextBox();
        }

        public override void Write(string format, object arg0, object arg1)
        {
            base.Write(format, arg0, arg1);
            updateTextBox();
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            base.Write(format, arg0, arg1, arg2);
            updateTextBox();
        }

        public override void Write(string format, params object[] arg)
        {
            base.Write(format, arg);
            updateTextBox();
        }

        public override void Write(string value)
        {
            base.Write(value);
            updateTextBox();
        }

        public override void Write(uint value)
        {
            base.Write(value);
            updateTextBox();
        }

        public override void Write(ulong value)
        {
            base.Write(value);
            updateTextBox();
        }
    }
}
