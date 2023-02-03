using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RFIDAttendance.Common;
using RFIDAttendance.StateModel;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using OPOSRFIDLib;
using System.Windows.Shapes;

namespace RFIDAttendance
{
    public partial class MainForm : Form
    {
        API_odoo.SignalResponse Signal = new API_odoo.SignalResponse();
        public MainForm()
        {
            InitializeComponent();
        }

        public bool mute = false;
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



            pictureBoxmute.Load(string.Format("Images/{0}.png", "volume"));
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
            GlobalVariables.opos.setAtenaPower(GlobalVariables.OPOSRFID1, GlobalVariables.atenaPowerCSV);

            //statusContent.Text = "ACTIVING";
            WindowState = FormWindowState.Maximized;
            GlobalVariables.opos.OPOS_StartReading(GlobalVariables.OPOSRFID1, GlobalVariables.rT);
            appActiving = true;
            if (GlobalVariables.cam_check == null)
            {
                GlobalVariables.cam_check = new CameraController();
                GlobalVariables.cam_check.StreamVideo(GlobalVariables.url_camera);

            }
            if (GlobalVariables.auto == "0")
            {
                this.pictureBoxCheckin.Visible = true;
                this.hightLight_Btn.Visible = true;
                this.label4.Visible = true;
                if (Ischeckin)
                {
                    this.label4.Text = "CHECK-IN";
                }
                else
                {
                    this.label4.Text = "CHECK-OUT";
                }
                Task.Run(() => CheckHour());
                Task.Run(() => CheckRFIDRemoved());
                Task.Run(() => playVideo());
            }
            else
            {
                this.pictureBoxCheckin.Visible = false;
                this.hightLight_Btn.Visible = false;
                this.label4.Visible = false;
                ReadLogRFID_checkin();
                Task.Run(() => ResetCheckIn());
                Task.Run(() => CheckRFIDReset());
                Task.Run(() => CheckTimeMute());
                Task.Run(() => playVideo());

            }

            

            


        }

        private void OPOSRFID1_DataEvent_Test(object sender, AxOPOSRFIDLib._DOPOSRFIDEvents_DataEventEvent e)
        {
            Console.WriteLine("OPOSRFID1_DataEvent");

            GlobalVariables.OPOSRFID1.DataEventEnabled = true;
        }
        
        public static void ReadLogRFID_checkin()
        {
            try
            {
                using (StreamReader reader = File.OpenText("Log_RFIDCheckin_data.txt"))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        GlobalVariables.list_rfid_checkin.Add(line);
                    }
                }
            } catch (Exception e)
            {
                WriteLog(e.ToString());
            }

        }
        public static void resetLog_checkin()
        {
            if (!File.Exists("Log_RFIDCheckin_data.txt"))
                File.Create("Log_RFIDCheckin_data.txt");

            TextWriter tw = new StreamWriter("Log_RFIDCheckin_data.txt", false);
            tw.Write(string.Empty);
            tw.Close();
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

                try 
                { 
                   GlobalVariables.cam_check.GetImageBitmap();
                    //pictureBoxInfo.Invoke(new Action(() => pictureBoxInfo.Image = image));
                } catch(Exception e)
                {
                    Console.WriteLine(e);
                    WriteLog(e.ToString());
                }
                
                Thread.Sleep(500);
            }
        }
        private Task CheckRFIDReset()
        {
            int countTime = 0;
            while (true)
            {
                Console.WriteLine("====================================");
                printList(GlobalVariables.rfid_code);
                printList(GlobalVariables.interval_rfid);
                Console.WriteLine("====================================");
                if (countTime == (int)Int64.Parse(GlobalVariables.timer_rfid)*60)
                {
                    GlobalVariables.rfid_code.Clear();
                    GlobalVariables.interval_rfid.Clear();
                    countTime = 0;

                }
                else if (countTime==5)
                {
                    Invoke(new Action(() =>
                    {
                        ClearAllinfo();
                    }));
                    countTime++;
                }
                else
                {
                    GlobalVariables.rfid_code.Sort();
                    GlobalVariables.interval_rfid.Sort();
                    if (!GlobalVariables.rfid_code.SequenceEqual(GlobalVariables.interval_rfid))
                    {
                        try { 
                            foreach(String rfid in GlobalVariables.rfid_code)
                            {
                                if (!GlobalVariables.interval_rfid.Contains(rfid))
                                {
                                    GlobalVariables.interval_rfid.Add(rfid);
                                    countTime = 0;
                                }
                            }
                        }catch (Exception e) {
                            WriteLog(e.ToString());
                        }

                      
                    }
                    else
                    {
                        countTime++;
                    }
                        
                }
                Console.WriteLine(countTime);
                Thread.Sleep(1000);
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
                printList(GlobalVariables.list_rfid_checkin);
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
                        WriteLog(e.ToString());
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
        private async Task ResetCheckIn()
        {
            while (true)
            {
                String hour =DateTime.Now.ToString("HH:mm:ss");

                if (hour.Equals(GlobalVariables.time_reset_check)) {
                    try
                    {
                        foreach (string rfid in GlobalVariables.list_rfid_checkin)
                        {
                            API_odoo api = new API_odoo();
                            Signal = await api.APIGetSignalCheckInCheckOut(GlobalVariables.url_Odoo, GlobalVariables.url_getsignalinout, rfid);
                            if (Signal.signalcheck == "checkout")
                            {
                                string message = await api.APIUpdateForgetCheckOut(rfid, GlobalVariables.url_Odoo, GlobalVariables.url_updateforgetcheckout);
                            }
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine(e);
                        WriteLog(e.ToString());
                    }
                    GlobalVariables.list_rfid_checkin.Clear();
                    Invoke(new Action(() =>
                    {
                    resetLog_checkin();
                    }));
                    }

                await Task.Delay(1000);
            }
        }
        private async Task CheckTimeMute()
        {
            while (true)
            {
                float hour = (float)float.Parse(DateTime.Now.ToString("HH.mm"));
                if (((hour >= GlobalVariables.time_checkin_sound_on1) && (hour <= GlobalVariables.time_checkin_sound_off1)) || ((hour >= GlobalVariables.time_checkin_sound_on2) && (hour <= GlobalVariables.time_checkin_sound_off2))) {
                    GlobalVariables.Ismutecheckin = false;
                }
                else
                {
                    GlobalVariables.Ismutecheckin = true;
                }
                if (((hour >= GlobalVariables.time_checkout_sound_on1) && (hour <= GlobalVariables.time_checkout_sound_off1)) ||((hour >= GlobalVariables.time_checkout_sound_on2) && (hour <= GlobalVariables.time_checkout_sound_off2))) {
                    GlobalVariables.Ismutecheckout = false;
                }
                else
                {
                    GlobalVariables.Ismutecheckout = true;
                }
                await Task.Delay(1000);
            }
        }
        void Check_Hour()
        {
            int hour = Int16.Parse(DateTime.Now.ToString("HH"));
            if (hour < GlobalVariables.hours_change)
            {
                Ischeckin = true;
                this.pictureBoxCheckin.Load(string.Format("Images/{0}.png", "Checkin"));
                label4.Text = "CHECK-IN";
            }
            else
            {
                Ischeckin = false;
                this.pictureBoxCheckin.Load(string.Format("Images/{0}.png", "Checkout"));
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
                pictureBoxCheckin.Load(string.Format("Images/{0}.png", "Checkout"));
                this.label4.ForeColor = Color.DarkOrange;
                GlobalVariables.opos.OPOS_StartReading(GlobalVariables.OPOSRFID1, GlobalVariables.rT);

            }
            else
            {
                Ischeckin = true;
                this.label4.Text = "CHECK-IN";
                this.label4.ForeColor = Color.DarkGreen;
                pictureBoxCheckin.Load(string.Format("Images/{0}.png", "Checkin"));
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


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            if (mute)
            {
                mute = false;
                pictureBoxmute.Load(string.Format("Images/{0}.png", "volume"));
            }else
            {
                mute = true;
                pictureBoxmute.Load(string.Format("Images/{0}.png", "mute"));
            }    
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C)
            {
                pictureBoxCheckin_Click(sender, new EventArgs());
            }else if (e.KeyCode == Keys.M)
            {
                pictureBox1_Click_1(sender, new EventArgs());
            }
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelInfo_Paint(object sender, PaintEventArgs e)
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
