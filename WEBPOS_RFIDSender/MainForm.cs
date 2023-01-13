using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WEBPOS_RFIDSender.Common;
using WEBPOS_RFIDSender.StateModel;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

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
                this.label4.Text = "CHECK-IN";
            }
            else
            {
                this.label4.Text = "CHECK-OUT";
            }
            Process[] processes = Process.GetProcessesByName("WEBPOS_RFIDSender");
            if (processes.Length > 1)
            {
                MessageBox.Show("An other application is running! Please close it and start again.");
                this.Close();
            }

            //AppStarting waitForm = new AppStarting();
            //waitForm.ShowDialog();

            CommonFunction.readConfigFile();
            GlobalVariables.staticTitle = OCRText.getTileStatic_Image("TitleState");

  

            //Program.mainForm.infoLog.Text += ">>> WEBPOS is detected!\r\n";
            

            GlobalVariables.OPOSRFID1.CreateControl();
            GlobalVariables.opos.OPOS_EnableDevice(GlobalVariables.OPOSRFID1, GlobalVariables.device_name);
            Console.WriteLine("check"+GlobalVariables.OPOSRFID1.PowerState);
            Console.WriteLine("check"+GlobalVariables.OPOSRFID1.PowerNotify);
            Console.WriteLine("check"+GlobalVariables.OPOSRFID1.CapPowerReporting);

            //statusContent.Text = "ACTIVING";
            WindowState = FormWindowState.Maximized;
            GlobalVariables.opos.OPOS_StartReading(GlobalVariables.OPOSRFID1, GlobalVariables.rT);
            appActiving = true;
            if (GlobalVariables.cam_check == null)
            {
                GlobalVariables.cam_check = new CameraController();
                GlobalVariables.cam_check.StreamVideo("0");

            }
            Task.Run(() => CheckRFIDRemoved());
            Task.Run(() => playVideo());
            Task.Run(() => CheckHour());


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
                //Program.mainForm.infoLog.Text += ">>> No WEBPOS detected!\r\n";
                WEBPOSis_detected = CommonFunction.GetWebPOSScreen();
            }

            Console.WriteLine(">>> WEBPOS is detected!");
            //Program.mainForm.infoLog.Text += ">>> WEBPOS is detected!\r\n";

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
        private async Task playVideo()
        {
            while (true)
            {
                try { 
                   GlobalVariables.cam_check.GetImageBitmap(); 
                } catch(Exception e)
                {
                   
                }
                
                await Task.Delay(500);
            }
        }
        private Task CheckRFIDRemoved()
        {
            int countTime = 0;
            
            while (true)
            {

                Console.WriteLine("====================================");
                printList(GlobalVariables.rfid_code);
                printList(GlobalVariables.interval_rfid);
                Console.WriteLine("====================================");
                //if ()
                //{

                //}
             
                if (countTime == 5)
                {
                    try
                    {
                        foreach (string rfid in GlobalVariables.rfid_code)
                        {
                            if (!GlobalVariables.interval_rfid.Contains(rfid))
                            {

                                Console.WriteLine(string.Format("RFID {0} is removed!", rfid));
                                GlobalVariables.rfid_code.Remove(rfid);
                                GlobalVariables.count_list = 0;
                                countTime = 0;
                                Invoke(new Action(() =>
                                {
                                    ClearAllinfo();
                                }));

                            }
                        }
                    }
                    catch (Exception e) {
                       // Console.WriteLine(e);
                    }

                }

                else
                {
                    GlobalVariables.rfid_code.Sort();
                    GlobalVariables.interval_rfid.Sort();

                    if (!GlobalVariables.rfid_code.SequenceEqual(GlobalVariables.interval_rfid))
                    {
                        if (!(GlobalVariables.rfid_code.Count()== GlobalVariables.count_list))
                        {
                            GlobalVariables.count_list = GlobalVariables.rfid_code.Count();
                            countTime = 0;
                        }
                        countTime++;
                        
                    }
                    else
                    {
                        countTime = 0;
                    }

                    
                }

                GlobalVariables.interval_rfid.Clear();
                

                Console.WriteLine(countTime);
                Thread.Sleep(1000);
            }
        }

        private Task WaitPOS()
        {
            while (true)
            {
                if (!isProcessClosed("AIR_START"))
                {
                    //statusContent.Text = "ACTIVING";
                    appActiving = true;
                    MainWorker();
                    return null;
                }
            }
        }
        private async Task CheckHour()
        {
            while (true)
            {
                Invoke(new Action(() =>
                {
                    Check_Hour();
                }));
                

                await Task.Delay(600000);
            }
        }
        void Check_Hour()
        {
            int hour = Int16.Parse(DateTime.Now.ToString("HH"));
            if (hour < 20)
            {
                Ischeckin = true;
/*                this.checkin_pictureBox.Load(string.Format("Images/{0}.png", "Checkin"));*/
                label4.Text = "CHECK-IN";
            }
            else
            {
                Ischeckin = false;
                /*this.checkin_pictureBox.Load(string.Format("Images/{0}.png", "Checkout"));*/
                label4.Text = "CHECK-OUT";
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
            GlobalVariables.interval_rfid.Clear();
            GlobalVariables.count_list=0;


            Clear_NowCheckInOut();
            Clear_YesterdayCheckInOut();

        }
        public void ClearAllinfo()
        {
            textBoxName.Clear();
            textBoxID.Clear();
            textBoxDepartment.Clear();
            pictureBoxInfo.Image = null;
            notice.Text = "";
            Clear_NowCheckInOut();
            Clear_YesterdayCheckInOut();

        }
        public void ClearNotice()
        {
            if (!(notice.Text == null))
            {
                Thread.Sleep(2000);
                notice.Text = "";
            }
        }

        public void Clear_NowCheckInOut()
        {

            //checkinClear

            textBoxTimeNowCheckIn.Clear();

            pictureBoxNowCheckin.Image = null;

            //checkoutClear

            textBoxNowCheckOut.Clear();

            pictureBoxNowCheckout.Image = null;

        }
        public void Clear_YesterdayCheckInOut()
        {
            //checkinClear

            textBoxTimeLastCheckIn.Clear();

            pictureBoxLastCheckIn.Image = null;

            //checkoutClear

            textBoxTimeLastCheckOut.Clear();

            pictureBoxLastCheckOut.Image = null;


        }

/*        private void startBtn_Click(object sender, EventArgs e)
        {
            
                    GlobalVariables.opos.OPOS_StartReading(GlobalVariables.OPOSRFID1, GlobalVariables.rT);
                    appActiving = true;
                    GlobalVariables.list_rfid.Clear();
                    GlobalVariables.rfid_code.Clear();

        }*/

/*        private void stopBtn_Click(object sender, EventArgs e)
        {

               GlobalVariables.opos.OPOS_StopReading(GlobalVariables.OPOSRFID1);
               GlobalVariables.list_rfid.Clear();
               GlobalVariables.rfid_code.Clear();
               appActiving = false;

        }
*/
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

        //private void infoLog_TextChanged(object sender, EventArgs e)
        //{

        //}

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

        private void printList(List<string> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void notice_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void mainTitle_Click(object sender, EventArgs e)
        {

        }

        //private void infoLog_TextChanged(object sender, EventArgs e)
        //{

        //}

        //private void infoLog_TextChanged(object sender, EventArgs e)
        //{

        //}

        //private void infoLog_TextChanged(object sender, EventArgs e)
        //{

        //}
    }
}
