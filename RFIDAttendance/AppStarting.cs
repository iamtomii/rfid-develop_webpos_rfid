﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RFIDAttendance.Common;

namespace RFIDAttendance
{
    public partial class AppStarting : Form
    {
        public AppStarting()
        {
            InitializeComponent();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void AppStarting_Load(object sender, EventArgs e)
        {
            bool WEBPOSis_detected = CommonFunction.GetWebPOSScreen();
            while (!WEBPOSis_detected)
            {
                //this.Show();
                Thread.Sleep(1000);
                Console.WriteLine("No WEBPOS detected!");
                WEBPOSis_detected = CommonFunction.GetWebPOSScreen();
            }

            this.Close();
        }
    }
}