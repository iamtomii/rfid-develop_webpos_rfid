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
using System.Configuration;
using System.Web;
using System.Globalization;
using static RFIDAttendance.API_odoo;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

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
        public Boolean Ischeckin = true;
        public Boolean Isstartstreamvideo = false;
        public string Name { get; set; }
        public DateTime beginTime { get; set; }
        private void Form1_Load(object sender, EventArgs e)
        {
            AppStarting waitForm = new AppStarting();
            waitForm.ShowDialog();
            try {
                pictureBoxmute.Load(string.Format("Images/{0}.png", "volume"));
            }
            catch(Exception err)
            {
                WriteLogE(err);
            }
            
            Process[] processes = Process.GetProcessesByName("WEBPOS_RFIDSender");
            if (processes.Length > 1)
            {
                MessageBox.Show("An other application is running! Please close it and start again.");
                this.Close();
            }
            CommonFunction.readConfigFile();
            GlobalVariables.staticTitle = OCRText.getTileStatic_Image("TitleState");
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
                this.gifusermanual.Visible = false;

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
                //GlobalVariables.cam_check.StreamAndSaveVideo("output.avi");
                try
                {
                    Image gif_usermanual = Image.FromFile(GlobalVariables.url_gif_usermanual);
                    this.gifusermanual.Image = gif_usermanual;
                } catch (Exception ex)
                {
                    WriteLogE(ex);
                }
                
                ReadLogRFID_checkin();
                Task.Run(() => ResetCheckIn());
                Task.Run(() => CheckRFIDReset());
                Task.Run(() => CheckTimeMute());
                Task.Run(() => playVideo());
                Task.Run(() => isStartStreamVideo());
                Task.Run(() => streamVideoSaveVideo());
                Task.Run(() => dailyCutVideo());

            }
        }


        public static void WriteLogE(Exception exception)
        {
            using (TextWriter writer = new StreamWriter("Log_data.txt", true))  // true is for append mode
            {
                writer.WriteLine(
                    "=>{0} An Error occurred: {1}  Message: {2}{3}",
                    DateTime.Now,
                    exception.StackTrace,
                    exception.Message,
                    Environment.NewLine
                    );
            }
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
                WriteLogE(e);
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
        private async Task dailyCutVideo()
        {
            while (true)
            {
                try {
                    String inputfilename = getCurrentTime().ToString("yyyy_MM_dd")+@"\"+getCurrentTime().ToString("yyyy_MM_dd")+GlobalVariables.video_extension;                   

                    DateTime time_startCutVideo = DateTime.ParseExact(GlobalVariables.time_startcutvideo, "HH:mm:ss", CultureInfo.InvariantCulture);
                    if ((getCurrentTime().Hour == time_startCutVideo.Hour) & (getCurrentTime().Minute == time_startCutVideo.Minute) & (getCurrentTime().Second == time_startCutVideo.Second))
                    {
                        foreach (string rfid in GlobalVariables.list_rfid_checkin)
                        {
                            API_odoo api = new API_odoo();
                            string timecheckin = await api.APIGetTimeCheckincutVideoAsync(GlobalVariables.url_Odoo, GlobalVariables.url_gettimecheckin, rfid);
                            if (!(timecheckin == "false"))
                            {
                                Task.Run(() => cutVideo(setStarttoCutVideo(timecheckin), GlobalVariables.check_videolength, inputfilename, createOutputFileName(rfid, getCurrentTime(), "checkin")));
                            }

                            string timecheckout = await api.APIGetTimeCheckoutcutVideoAsync(GlobalVariables.url_Odoo, GlobalVariables.url_gettimecheckout, rfid);
                            WriteLog(timecheckout);
                            if (!(timecheckout == "false"))
                            {
                                Task.Run(() => cutVideo(setStarttoCutVideo(timecheckout), GlobalVariables.check_videolength, inputfilename, createOutputFileName(rfid, getCurrentTime(), "checkout")));
                            }
                        }
                    }
                } catch (Exception e) { WriteLogE(e); }
            }
        }
        private string createOutputFileName(string rfid,DateTime currentDateTime,string action)
        {

            string outputfilename = currentDateTime.ToString("yyyy_MM_dd")+@"\"+action +"_" + rfid + "_" + currentDateTime.ToString("yyyy_MM_dd") + GlobalVariables.video_extension;
            return outputfilename;
        }
        private int setStarttoCutVideo(string timecheckin)
        {
            int timeseconds = convertStringTimetoTotalSecond(timecheckin);
            int seconds_config = convertStringTimetoTotalSecond(GlobalVariables.time_startstreamvideo);
            int start = timeseconds - seconds_config - 5;
            return start;
        }
        private async Task cutVideo(int start, int end,string inputfilename,string outputfilename)
        {       
            if (start >= 0) {
                string outputFilename_path = GlobalVariables.url_videooutput + outputfilename;
                string inputFilename_path = GlobalVariables.url_videoinput + inputfilename;
                string command = $"-i \"{inputFilename_path}\" -ss {start} -t {end} \"{outputFilename_path}\"";
                WriteLog(inputFilename_path);
                WriteLog(command);
                Task<bool> executeTask = FfmpegHandler.ExecuteFFMpegAsync(command);
                // To cancel the task, call cancellationTokenSource.Cancel()
                // This will cause the process to be killed and the task to complete with a result of false
                bool result = await executeTask;
            }
        }
        private DateTime getCurrentTime()
        {
            DateTime currentDateTime = DateTime.Now;
            return currentDateTime;
        }
        private async Task isStartStreamVideo()
        {
            while (true)
            {
                DateTime time_startStreamVideo = DateTime.ParseExact(GlobalVariables.time_startstreamvideo, "HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime time_endStreamVideo = DateTime.ParseExact(GlobalVariables.time_endstreamvideo, "HH:mm:ss", CultureInfo.InvariantCulture);
                if ((getCurrentTime().Hour == time_startStreamVideo.Hour) & (getCurrentTime().Minute == time_startStreamVideo.Minute) & (getCurrentTime().Second == time_startStreamVideo.Second))
                {
                    Isstartstreamvideo = true;
                }
                else if ((getCurrentTime().Hour == time_endStreamVideo.Hour) & (getCurrentTime().Minute == time_endStreamVideo.Minute) & (getCurrentTime().Second == time_endStreamVideo.Second))
                {
                    Isstartstreamvideo = false;
                }
                Thread.Sleep(1000);
            }
        }
        private async Task streamVideoSaveVideo()
        {
            while (true)
            {
                if (Isstartstreamvideo) {
                    String datetime = DateTime.Now.ToString("yyyy_MM_dd");
                    string folder_videoname = datetime;
                    string path = @"SaveVideo\" + folder_videoname;
                    Directory.CreateDirectory(path);
                    String outputfilename = path + @"\" + datetime + GlobalVariables.video_extension;
                    Console.WriteLine(outputfilename);
                    GlobalVariables.cam_check.StreamAndSaveVideo(outputfilename, GlobalVariables.fps);
                    //pictureBoxInfo.Invoke(new Action(() => pictureBoxInfo.Image = image));
                }
            }
        }
        private int convertStringTimetoTotalSecond(String time)
        {
            DateTime dateTimecheckin = DateTime.ParseExact(time, "HH:mm:ss", CultureInfo.InvariantCulture);
            TimeSpan timeOfDay = dateTimecheckin.TimeOfDay;
            double seconds = timeOfDay.TotalSeconds;

            return Convert.ToInt32(seconds);
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
                    WriteLogE(e);
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
                            WriteLogE(e);
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
                        WriteLogE(e);
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
                        WriteLogE(e);
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
        private void printList(List<string> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
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

        private async void pictureBox1_Click_1(object sender, EventArgs e)
        {
            //int start = 1;
            //int end = 60;
            //string outputFilename = $@"C:\RFID\rfid\rfid-develop_webpos_rfid\rfid-develop_attendance_rfid\RFIDAttendance\bin\Release\SaveVideo\testouput.mp4";
            //string txtInputFile = @"C:\RFID\rfid\rfid-develop_webpos_rfid\rfid-develop_attendance_rfid\RFIDAttendance\bin\Release\output.avi";
            //string command = $"-i \"{txtInputFile}\" -ss {start} -t {end} \"{outputFilename}\"";
            //WriteLog(outputFilename);
            //WriteLog(command);

            //FfmpegHandler.ExecuteFFMpeg(command);
            //Task.Run(() => cutVideo(1,60));




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

        private void pictureBox1_Click_2(object sender, EventArgs e)
        {

        }
    }
}
