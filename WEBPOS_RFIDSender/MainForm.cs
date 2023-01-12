using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WEBPOS_RFIDSender.Common;
using WEBPOS_RFIDSender.StateModel;
using System.Drawing;

namespace WEBPOS_RFIDSender
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private bool appActiving = false;
        private int workingTask_ID;
        public Boolean Ischeckin = true;
        public string Name { get; set; }
        public string Id { get; set; }
        public string Deparment { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public DateTime beginTime { get; set; }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (Ischeckin)
            {
                this.label5.Text = "CHECK-IN";
            }
            else
            {
                this.label5.Text = "CHECK-OUT";
            }
            Process[] processes = Process.GetProcessesByName("WEBPOS_RFIDSender");
            if (processes.Length > 1)
            {
                MessageBox.Show("An other application is running! Please close it and start again.");
                this.Close();
            }

            //AppStarting waitForm = new AppStarting();
            //waitForm.ShowDialog();

            appActiving = true;
            CommonFunction.readConfigFile();
            GlobalVariables.staticTitle = OCRText.getTileStatic_Image("TitleState");

  

            Program.mainForm.infoLog.Text += ">>> WEBPOS is detected!\r\n";
            

            GlobalVariables.OPOSRFID1.CreateControl();
            GlobalVariables.opos.OPOS_EnableDevice(GlobalVariables.OPOSRFID1, GlobalVariables.device_name);

            statusContent.Text = "ACTIVING";
            WindowState = FormWindowState.Maximized;
        }

        private void OPOSRFID1_DataEvent_Test(object sender, AxOPOSRFIDLib._DOPOSRFIDEvents_DataEventEvent e)
        {
            Console.WriteLine("OPOSRFID1_DataEvent");

            GlobalVariables.OPOSRFID1.DataEventEnabled = true;
        }

        private void MainWorker()
        {
            //CommonFunction.readConfigFile();
            GlobalVariables.staticTitle = OCRText.getTileStatic_Image("TitleState");

            bool WEBPOSis_detected = CommonFunction.GetWebPOSScreen();
            while (!WEBPOSis_detected)
            {
                Thread.Sleep(1000);
                Console.WriteLine(">>> No WEBPOS detected!");
                Program.mainForm.infoLog.Text += ">>> No WEBPOS detected!\r\n";
                WEBPOSis_detected = CommonFunction.GetWebPOSScreen();
            }

            Console.WriteLine(">>> WEBPOS is detected!");
            Program.mainForm.infoLog.Text += ">>> WEBPOS is detected!\r\n";

            GlobalVariables.OPOSRFID1.CreateControl();
            GlobalVariables.opos.OPOS_EnableDevice(GlobalVariables.OPOSRFID1, GlobalVariables.device_name);
            
        }

        public static void WriteLog(string data)
        {
            using (TextWriter writer = new StreamWriter("Log_data.txt", true))  // true is for append mode
            {
                writer.WriteLine(data);
            }
        }
        private Task Worker()
        {   
            while (true)
            {
                //Console.WriteLine("Task Running....");
                //if (isProcessClosed("AIR_START"))
                //{
                //    if (GlobalVariables.OPOSStatus == "READING")
                //    {
                //        GlobalVariables.opos.OPOS_StopReading(GlobalVariables.OPOSRFID1);
                //    }
                //    GlobalVariables.opos.OPOS_DisableDevice(GlobalVariables.OPOSRFID1);
                //    GlobalVariables.list_rfid.Clear();
                //    GlobalVariables.rfid_code.Clear();
                //    appActiving = false;
                //    statusContent.Text = "DISABLE";

                //    //Task.Run(() => WaitPOS());
                //}

                string working_window = OCRText.GetWorkingTitle();
                switch (working_window)
                {
                    case "会員/客層入力":                       
                        if (GlobalVariables.OPOSStatus.Equals("READING") && appActiving == true)
                        {
                            GlobalVariables.opos.OPOS_StopReading(GlobalVariables.OPOSRFID1);
                            Console.WriteLine("PRESCAN_INIT: " + working_window);
                        }
                        GlobalVariables.list_rfid.Clear();
                        GlobalVariables.rfid_code.Clear();
                        break;
                    case "商品登録":
                        if (GlobalVariables.OPOSStatus.Equals("STOPPING") && appActiving == true)
                        {
                            GlobalVariables.opos.OPOS_StartReading(GlobalVariables.OPOSRFID1, GlobalVariables.rT);
                            Console.WriteLine("SCAN_INIT: " + working_window);
                        }
                        break;
                    case "個数修正":
                        break;
                    case "現金入力":
                        if (GlobalVariables.OPOSStatus.Equals("READING") && appActiving == true)
                        {
                            GlobalVariables.opos.OPOS_StopReading(GlobalVariables.OPOSRFID1);
                            Console.WriteLine("PAYMENT_INIT: " + working_window);
                        }
                        break;
                    default:
                        break;
                }

                if (!appActiving)
                {
                    return null;
                }

            }
        }

        private Task WaitPOS()
        {
            while (true)
            {
                if (!isProcessClosed("AIR_START"))
                {
                    statusContent.Text = "ACTIVING";
                    appActiving = true;
                    MainWorker();
                    return null;
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (appActiving)
            {
                if (GlobalVariables.OPOSStatus == "READING")
                {
                    GlobalVariables.opos.OPOS_StopReading(GlobalVariables.OPOSRFID1);
                }
                GlobalVariables.opos.OPOS_DisableDevice(GlobalVariables.OPOSRFID1);
            }
        }
        public void ClearAll()
        {
            textBoxName.Clear();
            textBoxID.Text = "";
            textBoxDepartment.Text = "";
            pictureBoxInfo.Image = null;
            GlobalVariables.opos.OPOS_StopReading(GlobalVariables.OPOSRFID1);
            GlobalVariables.list_rfid.Clear();
            GlobalVariables.rfid_code.Clear();

            /*Clear_NowCheckInOut();
            Clear_YesterdayCheckInOut();*/

        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            
                    GlobalVariables.opos.OPOS_StartReading(GlobalVariables.OPOSRFID1, GlobalVariables.rT);
                    appActiving = true;

        }

        private void stopBtn_Click(object sender, EventArgs e)
        {

               GlobalVariables.opos.OPOS_StopReading(GlobalVariables.OPOSRFID1);
               GlobalVariables.list_rfid.Clear();
               GlobalVariables.rfid_code.Clear();
               appActiving = false;

        }

        private bool isProcessClosed(string process_name)
        {
            Process[] processes = Process.GetProcessesByName(process_name);
            if (processes.Length > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void infoLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void statusContent_TextChanged(object sender, EventArgs e)
        {
            //switch (this.Text)
            //{
            //    case "ACTIVING":
            //        startBtn.Enabled = false;
            //        stopBtn.Enabled = true;
            //        break;
            //    case "DISABLE":
            //        startBtn.Enabled = true;
            //        stopBtn.Enabled = false;
            //        break;
            //}
        }

        private void statusLB_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxTimeLastCheckIn_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxNowCheckOut_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBoxCheckin_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Switch mode click!");
            ClearAll();
            this.notice.Text = "";

            if (Ischeckin)
            {
                Ischeckin = false;
                this.label4.Text = "CHECK-OUT";
                this.label4.ForeColor = Color.DarkOrange;
                GlobalVariables.opos.OPOS_StartReading(GlobalVariables.OPOSRFID1, GlobalVariables.rT);

            }
            else
            {
                Ischeckin = true;
                this.label4.Text = "CHECK-IN";
                this.label4.ForeColor = Color.DarkGreen;
                GlobalVariables.opos.OPOS_StartReading(GlobalVariables.OPOSRFID1, GlobalVariables.rT);

            }
        }

        private void pictureBoxInfo_Click(object sender, EventArgs e)
        {

        }
    }
}
