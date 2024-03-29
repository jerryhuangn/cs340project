﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using Server;
using System.Diagnostics;

namespace cs340project
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Proxifier.CreateProxyClass(typeof(Node));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //Application.Run(new GUI());
            Application.Run(new NetworkGUI());

        }

    }
}
