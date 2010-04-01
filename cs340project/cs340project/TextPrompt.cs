using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cs340project
{
    public partial class TextPrompt : Form
    {
        public TextPrompt()
        {
            InitializeComponent();
        }

        public static string Show(string label, string val)
        {
            TextPrompt dlg = new TextPrompt();
            dlg.lblInput.Text = label;
            dlg.txtInput.Text = val;
            if (dlg.ShowDialog() == DialogResult.OK)
                return dlg.txtInput.Text;
            return null;
        }
    }
}
