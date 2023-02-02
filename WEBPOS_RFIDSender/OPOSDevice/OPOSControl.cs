using AxOPOSRFIDLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WEBPOS_RFIDSender.Common;
using WEBPOS_RFIDSender.OPOSDevice;
using WEBPOS_RFIDSender.WinAPI;
using System.Drawing;
using Newtonsoft.Json;
using System.Speech.Synthesis;
using System.Xml.Linq;
using System.IO;
using System.Net;
using System.Web;
using System.Threading;
using NAudio.Wave;
using System.Windows.Media.Animation;

namespace WEBPOS_RFIDSender.OposControl
{
    public delegate void FlushState();                                                        // Update real-time status invoke

    //public class MyJson
    //{
    //    public string code { get; set; }
    //    public Dictionary<string, string> data { get; set; }
    //    public bool isSuccess { get; set; }
    //    public string message { get; set; }
    //}
    public class OPOS : System.Windows.Forms.UserControl
    {
        API_odoo.MyResponse infoEmp = new API_odoo.MyResponse();
        API_odoo.SignalResponse Signal = new API_odoo.SignalResponse();
        private bool IsFlush = false;
        CheckCreateNewForm checkdialog = new CheckCreateNewForm();

        public OPOS()
        {
            GlobalVariables.OPOSRFID1.DataEvent += OPOSRFID1_DataEvent;
        }

        private async void OPOSRFID1_DataEvent(object sender, _DOPOSRFIDEvents_DataEventEvent e)
        {
            if (GlobalVariables.cam_check == null)
            {
                GlobalVariables.cam_check = new CameraController();
                GlobalVariables.cam_check.StreamVideo(GlobalVariables.url_camera);

            }
            WindowAPI.PostMessage(IntPtr.Zero, (uint)GlobalVariables.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(GlobalVariables.Keys), "Return")), (int)(0));
            int TagCount;
            string UserData;
            /*HttpClient api_client = new HttpClient();
            api_client.BaseAddress = new Uri(GlobalVariables.url_api);
            api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));*/
            // TagCount
            TagCount = GlobalVariables.OPOSRFID1.TagCount;

            var loopTo = TagCount;
            TagCount = GlobalVariables.OPOSRFID1.TagCount;

            for (int i = 0; i < TagCount; i++)
            {
                //var code_value = OPOSRFID1.CurrentTagID;
                UserData = " Userdata=" + GlobalVariables.OPOSRFID1.CurrentTagUserData;

                if (UserData == " Userdata=")
                {
                    UserData = "";
                }
                var code_value = GlobalVariables.OPOSRFID1.CurrentTagID + UserData;
                string new_code = ConvertTagIDCode(code_value);

                //take book out


                if (!GlobalVariables.rfid_code.Contains(new_code))
                {
                    Console.WriteLine("RFID CODE: " + new_code);
                    //Program.mainForm.infoLog.Invoke(new Action(() => Program.mainForm.infoLog.AppendText(String.Format("{0}\r\n", new_code))));

                    GlobalVariables.rfid_code.Add(new_code);
                    //call API
                    if (GlobalVariables.auto == "0")
                    {
                        if (!GlobalVariables.interval_rfid.Contains(new_code))
                        {
                            GlobalVariables.interval_rfid.Add(new_code);
                        }
                        showInfoAsync(new_code);
                    }

                    else
                    {
                        try { 
                            foreach (String rfid in GlobalVariables.rfid_code.ToList()) {
                                if (!(rfid.Contains(new_code)))
                                {
                                    GlobalVariables.rfid_code.Remove(rfid);
                                }
                            
                            }
                        
                        } catch (Exception error)
                        {
                            Console.WriteLine(error);
                        }
                        GlobalVariables.interval_rfid.Clear();

                        showInfoAutoResetAsync(new_code);
                    }

                }

                GlobalVariables.OPOSRFID1.NextTag();
            }
            GlobalVariables.OPOSRFID1.DataEventEnabled = true;
        }
        public async void WriteLogRFID_checkout(string data)
        {
            using (TextWriter writer = new StreamWriter("Log_RFIDCheckin_data.txt", true))  // true is for append mode
            {
                writer.WriteLine(data);
            }
        }
        private async Task showInfoAutoResetAsync(string new_code)
        {
            API_odoo api = new API_odoo();         
            if (!GlobalVariables.list_rfid_checkin.Contains(new_code))
            {
                String image_checkin = GlobalVariables.cam_check.GetImage();
                Console.WriteLine(image_checkin);
                //String image_checkin = GlobalData.Cam1.GetImage();
                string dateTime_checkin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string message = await api.APICheckin(new_code, image_checkin, GlobalVariables.url_Odoo, GlobalVariables.url_checkin, dateTime_checkin);
                string time_session = Int16.Parse(DateTime.Now.ToString("HH")) < 12 ? "morning" : "afternoon";
                if (message == "success")
                {
                    GlobalVariables.list_rfid_checkin.Add(new_code);
                    Task.Run(() => WriteLogRFID_checkout(new_code));
                    // interval_rfid.Add(new_code);
                    infoEmp = await api.APIGetInfoEmployee(GlobalVariables.url_Odoo, GlobalVariables.url_api_Employee, new_code);
                    Update_Info_Now_Checkin(infoEmp, dateTime_checkin);
                    Update_Info_Recently_CheckINCheckOUT(infoEmp);
                    Program.mainForm.pictureBoxNowCheckin.Image = GlobalVariables.cam_check.stringToImage(image_checkin);
                    //  ShowInfo_Object.pictureBox_now_Checkin.Image = GlobalData.Cam1.stringToImage(image_checkin);
                    Program.mainForm.notice.ForeColor = Color.DarkGreen;
                    Program.mainForm.notice.Text = string.Format("Good {0} {1}! Hope you have a good day!", time_session, infoEmp.name.Split(' ').ToList().Last());
                    Program.mainForm.pictureBoxNowCheckout.Image = null;
                    Program.mainForm.textBoxNowCheckOut.Clear();
                    //Task.Run(() => speak_checkin(infoEmp));
                    Task.Run(() => speakGoogle(infoEmp, GlobalVariables.text_checkin));
                }
                else if (message.Contains("Cannot create new attendance"))
                {
                    Program.mainForm.pictureBoxNowCheckin.Image = null;
                    Program.mainForm.textBoxTimeNowCheckIn.Clear();
                    Program.mainForm.notice.ForeColor = Color.Crimson;
                    Program.mainForm.notice.Text = "You have checkin already!\r\nPlease check your last information";
                    infoEmp = await api.APIGetInfoEmployee(GlobalVariables.url_Odoo, GlobalVariables.url_api_Employee, new_code);
                    Update_Info_REcently_CheckINCheckOUT_haveAVATAR(infoEmp);
                }
                else if (message.Contains("Can not find employee"))
                {

                    if (Program.mainForm.pictureBoxInfo != null)
                    {
                        Program.mainForm.ClearAllinfo();
                    }
                    Program.mainForm.notice.ForeColor = Color.Crimson;
                    Program.mainForm.notice.Text = " Cannot find your information";
                    if(!(GlobalVariables.auto == "2"))
                    {
                        checkdialog.RFID_exist = new_code;
                        checkdialog.ShowDialog();
                    }
                    
                }

            }
            else
            {
                String image_checkout = GlobalVariables.cam_check.GetImage();
                Console.WriteLine(image_checkout);
                string dateTime_checkout = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string message = await api.APIUpdateCheckOut(new_code, image_checkout, GlobalVariables.url_Odoo, GlobalVariables.url_updatecheckout, dateTime_checkout);
                if (message == "success")
                {
                    infoEmp = await api.APIGetInfoEmployee(GlobalVariables.url_Odoo, GlobalVariables.url_api_Employee, new_code);
                    Program.mainForm.textBoxTimeNowCheckIn.Clear();
                    Program.mainForm.pictureBoxNowCheckin.Image = null;
                    Update_Info_Now_Checkout(infoEmp, dateTime_checkout);
                    updateImageCheckin(infoEmp);
                    Update_Info_Recently_CheckINCheckOUT(infoEmp);
                    Program.mainForm.pictureBoxNowCheckout.Image = GlobalVariables.cam_check.stringToImage(image_checkout);
                    Program.mainForm.notice.ForeColor = Color.DarkGreen;
                    Program.mainForm.notice.Text = string.Format("Goodbye {0}! Thanks for your hardwork!", infoEmp.name.Split(' ').ToList().Last());

                    Task.Run(() => speakGoogle(infoEmp, GlobalVariables.text_checkout));
                }
                else if (message.Contains("Can not find employee"))
                {
                    Program.mainForm.notice.ForeColor = Color.Crimson;
                    Program.mainForm.notice.Text = " Cannot find your information";
                    if (!(GlobalVariables.auto == "2"))
                    {
                        checkdialog.RFID_exist = new_code;
                        checkdialog.ShowDialog();
                    }
                }
                else
                {
                    Program.mainForm.notice.Text = "Api error";
                }

            }
        }
        private async Task showInfoAutoAsync(String new_code)
        {
            API_odoo api = new API_odoo();
            Signal = await api.APIGetSignalCheckInCheckOut(GlobalVariables.url_Odoo,GlobalVariables.url_getsignalinout,new_code);
            if (Signal.signalcheck.Equals("checkin"))
            {
                String image_checkin = GlobalVariables.cam_check.GetImage();

                Console.WriteLine(image_checkin);
                //String image_checkin = GlobalData.Cam1.GetImage();
                string dateTime_checkin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string message = await api.APICheckin(new_code, image_checkin, GlobalVariables.url_Odoo, GlobalVariables.url_checkin, dateTime_checkin);

                string time_session = Int16.Parse(DateTime.Now.ToString("HH")) < 12 ? "morning" : "afternoon";
                if (message == "success")
                {
                    // interval_rfid.Add(new_code);
                    infoEmp = await api.APIGetInfoEmployee(GlobalVariables.url_Odoo, GlobalVariables.url_api_Employee, new_code);
                    GlobalVariables.list_rfid_checkout.Remove(new_code);
                    GlobalVariables.list_rfid_checkin.Add(new_code);
                    Update_Info_Now_Checkin(infoEmp, dateTime_checkin);
                    Update_Info_Recently_CheckINCheckOUT(infoEmp);
                    Program.mainForm.pictureBoxNowCheckin.Image = GlobalVariables.cam_check.stringToImage(image_checkin);
                    //  ShowInfo_Object.pictureBox_now_Checkin.Image = GlobalData.Cam1.stringToImage(image_checkin);
                    Program.mainForm.notice.ForeColor = Color.DarkGreen;
                    Program.mainForm.notice.Text = string.Format("Good {0} {1}! Hope you have a good day!", time_session, infoEmp.name.Split(' ').ToList().Last());
                    Program.mainForm.pictureBoxNowCheckout.Image = null;
                    Program.mainForm.textBoxNowCheckOut.Clear();
                    //Task.Run(() => speak_checkin(infoEmp));
                    Task.Run(() => speakGoogle(infoEmp, GlobalVariables.text_checkin));
                }
                else if (message.Contains("Cannot create new attendance"))
                {
                    Program.mainForm.pictureBoxNowCheckin.Image = null;
                    Program.mainForm.textBoxTimeNowCheckIn.Clear();
                    Program.mainForm.notice.ForeColor = Color.Crimson;
                    Program.mainForm.notice.Text = "You have checkin already!\r\nPlease check your last information";
                    infoEmp = await api.APIGetInfoEmployee(GlobalVariables.url_Odoo, GlobalVariables.url_api_Employee, new_code);
                    Update_Info_REcently_CheckINCheckOUT_haveAVATAR(infoEmp);
                }
                else if (message.Contains("Can not find employee"))
                {

                    if (Program.mainForm.pictureBoxInfo != null)
                    {
                        Program.mainForm.ClearAllinfo();
                    }
                    Program.mainForm.notice.ForeColor = Color.Crimson;
                    Program.mainForm.notice.Text = " Cannot find your information";
                    checkdialog.RFID_exist = new_code;
                    checkdialog.ShowDialog();

                }
            }
            else if (Signal.signalcheck.Equals("checkout"))
            {
                String image_checkout = GlobalVariables.cam_check.GetImage();
                Console.WriteLine(image_checkout);
                string dateTime_checkout = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string message = await api.APICheckout(new_code, image_checkout, GlobalVariables.url_Odoo, GlobalVariables.url_checkout, dateTime_checkout);
                if (message == "success")
                {
                    infoEmp = await api.APIGetInfoEmployee(GlobalVariables.url_Odoo, GlobalVariables.url_api_Employee, new_code);
                    GlobalVariables.list_rfid_checkin.Remove(new_code);
                    GlobalVariables.list_rfid_checkout.Add(new_code);
                    Update_Info_Now_Checkout(infoEmp, dateTime_checkout);
                    Update_Info_Recently_CheckINCheckOUT(infoEmp);
                    Program.mainForm.pictureBoxNowCheckout.Image = GlobalVariables.cam_check.stringToImage(image_checkout);
                    Program.mainForm.notice.ForeColor = Color.DarkGreen;
                    Program.mainForm.notice.Text = string.Format("Goodbye {0}! Thanks for your hardwork!", infoEmp.name.Split(' ').ToList().Last());
                    Program.mainForm.textBoxTimeNowCheckIn.Clear();
                    Program.mainForm.pictureBoxNowCheckin.Image = null;
                    Task.Run(() => speakGoogle(infoEmp, GlobalVariables.text_checkout));
                }
                else if (message.Contains("Employee already checked-out"))
                {
                    Program.mainForm.pictureBoxNowCheckout.Image = null;
                    Program.mainForm.textBoxNowCheckOut.Clear();
                    Program.mainForm.notice.ForeColor = Color.Crimson;
                    Program.mainForm.notice.Text = "You have checkout already!\r\nPlease check your last information";
                    infoEmp = await api.APIGetInfoEmployee(GlobalVariables.url_Odoo, GlobalVariables.url_api_Employee, new_code);
                    Update_Info_REcently_CheckINCheckOUT_haveAVATAR(infoEmp);
                }
                else if (message.Contains("Can not find employee"))
                {
                    Program.mainForm.notice.ForeColor = Color.Crimson;
                    Program.mainForm.notice.Text = " Cannot find your information";
                    checkdialog.RFID_exist = new_code;
                    checkdialog.ShowDialog();
                }
                else
                {
                    Program.mainForm.notice.Text = "Api error";
                }

            }
            else
            {
                Program.mainForm.notice.ForeColor = Color.Crimson;
                Program.mainForm.notice.Text = " Cannot find your information";
                checkdialog.RFID_exist = new_code;
                checkdialog.ShowDialog();
            }
        }

        private async Task showInfoAsync(String new_code)
        {
            API_odoo api = new API_odoo();
            if (Program.mainForm.Ischeckin)
            {
                if (!GlobalVariables.list_rfid_checkin.Contains(new_code))
                {
                    String image_checkin = GlobalVariables.cam_check.GetImage();
                    Console.WriteLine(image_checkin);
                    string dateTime_checkin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string message = await api.APICheckin(new_code, image_checkin, GlobalVariables.url_Odoo, GlobalVariables.url_checkin, dateTime_checkin);

                    string time_session = Int16.Parse(DateTime.Now.ToString("HH")) < 12 ? "morning" : "afternoon";
                    if (message == "success")
                    {
                        infoEmp = await api.APIGetInfoEmployee(GlobalVariables.url_Odoo, GlobalVariables.url_api_Employee, new_code);                       
                        GlobalVariables.list_rfid_checkout.Remove(new_code);
                        GlobalVariables.list_rfid_checkin.Add(new_code);
                        Update_Info_Now_Checkin(infoEmp, dateTime_checkin);
                        Update_Info_Recently_CheckINCheckOUT(infoEmp);
                        Program.mainForm.pictureBoxNowCheckin.Image = GlobalVariables.cam_check.stringToImage(image_checkin);
                        Program.mainForm.notice.ForeColor = Color.DarkGreen;
                        Program.mainForm.notice.Text = string.Format("Good {0} {1}! Hope you have a good day!", time_session, infoEmp.name.Split(' ').ToList().Last());
                        Program.mainForm.pictureBoxNowCheckout.Image = null;
                        Program.mainForm.textBoxNowCheckOut.Clear();
                        //Task.Run(() => speak_checkin(infoEmp));
                        //Task.Run(() => speakGoogle(infoEmp, GlobalVariables.text_checkin));

                    }
                    else if (message.Contains("Cannot create new attendance"))
                    {   
                        Program.mainForm.pictureBoxNowCheckin.Image = null;
                        Program.mainForm.textBoxTimeNowCheckIn.Clear();
                        Program.mainForm.notice.ForeColor = Color.Crimson;
                        Program.mainForm.notice.Text = "You have checkin already!\r\nPlease check your last information";
                        infoEmp = await api.APIGetInfoEmployee(GlobalVariables.url_Odoo, GlobalVariables.url_api_Employee, new_code);
                        Update_Info_REcently_CheckINCheckOUT_haveAVATAR(infoEmp);
                    }
                    else if (message.Contains("Can not find employee"))
                    {                          
                        if (Program.mainForm.pictureBoxInfo != null)
                        {
                            Program.mainForm.ClearAllinfo();
                        }
                        Program.mainForm.notice.ForeColor = Color.Crimson;
                        Program.mainForm.notice.Text = " Cannot find your information";
                        checkdialog.RFID_exist = new_code;
                        checkdialog.ShowDialog();
                    }
                    else
                    {
                        if (Program.mainForm.pictureBoxInfo != null)
                        {
                            Program.mainForm.ClearAllinfo();
                        }
                        Program.mainForm.notice.Text = " Api error";
                    }
                }
                else
                {
                        Program.mainForm.notice.Text = "You have checked-in already";
                        GlobalVariables.list_rfid_checkin.Remove(new_code);
                        GlobalVariables.rfid_code.Remove(new_code);
                        GlobalVariables.interval_rfid.Remove(new_code);
                }
            }
            else
            {
                if (!GlobalVariables.list_rfid_checkout.Contains(new_code))
                {
                    String image_checkout = GlobalVariables.cam_check.GetImage();
                    Console.WriteLine(image_checkout);
                    string dateTime_checkout = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string message = await api.APICheckout(new_code, image_checkout, GlobalVariables.url_Odoo, GlobalVariables.url_checkout, dateTime_checkout);
                    if (message == "success")
                    {

                        infoEmp = await api.APIGetInfoEmployee(GlobalVariables.url_Odoo, GlobalVariables.url_api_Employee, new_code);
                        GlobalVariables.list_rfid_checkin.Remove(new_code);
                        GlobalVariables.list_rfid_checkout.Add(new_code);
                        Program.mainForm.textBoxTimeNowCheckIn.Clear();
                        Program.mainForm.pictureBoxNowCheckin.Image = null;
                        Update_Info_Now_Checkout(infoEmp, dateTime_checkout);
                        updateImageCheckin(infoEmp);
                        Update_Info_Recently_CheckINCheckOUT(infoEmp);
                        Program.mainForm.pictureBoxNowCheckout.Image = GlobalVariables.cam_check.stringToImage(image_checkout);
                        Program.mainForm.notice.ForeColor = Color.DarkGreen;
                        Program.mainForm.notice.Text = string.Format("Goodbye {0}! Thanks for your hardwork!", infoEmp.name.Split(' ').ToList().Last());

                        //Task.Run(() => speakGoogle(infoEmp, GlobalVariables.text_checkout));
                    }
                    else if (message.Contains("Employee already checked-out"))
                    {
                        Program.mainForm.pictureBoxNowCheckout.Image = null;
                        Program.mainForm.textBoxNowCheckOut.Clear();
                        Program.mainForm.notice.ForeColor = Color.Crimson;
                        Program.mainForm.notice.Text = "You have checkout already!\r\nPlease check your last information";
                        infoEmp = await api.APIGetInfoEmployee(GlobalVariables.url_Odoo, GlobalVariables.url_api_Employee, new_code);
                        Update_Info_REcently_CheckINCheckOUT_haveAVATAR(infoEmp);
                    }
                    else if (message.Contains("Can not find employee"))
                    {
                        Program.mainForm.notice.ForeColor = Color.Crimson;
                        Program.mainForm.notice.Text = " Cannot find your information";
                        checkdialog.RFID_exist = new_code;
                        checkdialog.ShowDialog();
                    }
                    else
                    {
                        Program.mainForm.notice.Text = "Api error";
                    }

                }
                else
                {
                        Program.mainForm.notice.Text = "You have checked-out already";
                        GlobalVariables.list_rfid_checkout.Remove(new_code);
                        GlobalVariables.rfid_code.Remove(new_code);
                        GlobalVariables.interval_rfid.Remove(new_code);
                }
            }

        }
        public void setAtenaPower(AxOPOSRFID OPOSRFID1, string strCSV)
        {
            //string strCSV = "";
            int lData;

            int lRet;
            OPOSRFID1.BinaryConversion = OposStatus.OposBcNone;

            lData = 2;
            lRet = OPOSRFID1.DirectIO(120, ref lData, ref strCSV);


            if (lRet != OposStatus.OposSuccess)
            {
                Console.WriteLine("Setting atena power is failed!");
            }

            OPOSRFID1.BinaryConversion = OposStatus.OposBcNibble;
        }

        private async Task speak_checkin(API_odoo.MyResponse infoEmp)
        {
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.Speak("Hello" + infoEmp.name);
        }

        private void speak_checkout(API_odoo.MyResponse infoEmp)
        {
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.Speak("Goodbye" + infoEmp.name);
        }
        private String gender_to_text(String gender)
        {
            if (gender.Equals("male"))
            {
                return " anh ";
            }
            else if(gender.Equals("female"))
            {
                return " chị ";
            }
            else
            {
                return " anh ";
            }
        }
        // text to speech with google translate
        private void speakGoogle(API_odoo.MyResponse infoEmp,String textread)
        {
            if (!Program.mainForm.mute)
            {
                String first_name = infoEmp.name.Split(' ').Last();
                String relatedLabel = textread + gender_to_text(infoEmp.gender) + first_name;


                var playThread = new Thread(() => PlayMp3FromUrl("https://translate.google.com/translate_tts?ie=UTF-8&tl=vi&client=tw-ob&q=" + HttpUtility.UrlEncode(relatedLabel)));
                playThread.IsBackground = true;
                playThread.Start();
            }
        }
        bool waiting = false;
        AutoResetEvent stop = new AutoResetEvent(false);
        public async Task PlayMp3FromUrl(string url)
        {
            Console.WriteLine(url);
            using (Stream ms = new MemoryStream())
            {
                using (Stream stream = WebRequest.Create(url)
                    .GetResponse().GetResponseStream())
                {
                    byte[] buffer = new byte[32768];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                }

                ms.Position = 0;
                using (WaveStream blockAlignedStream =
                    new BlockAlignReductionStream(
                        WaveFormatConversionStream.CreatePcmStream(
                            new Mp3FileReader(ms))))
                {
                    using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        waveOut.Init(blockAlignedStream);
                        waveOut.PlaybackStopped += (sender, e) =>
                        {
                            waveOut.Stop();
                        };

                        waveOut.Play();
                        waiting = true;
                        stop.WaitOne(10000);
                        waiting = false;
                    }
                }
            }
        }
            void Update_Info_Now_Checkin(API_odoo.MyResponse infoEmp, string dateTime_checkin)
        {
            Program.mainForm.textBoxName.Text = infoEmp.name;
            Program.mainForm.textBoxID.Text = infoEmp.id;
            Program.mainForm.textBoxDepartment.Text = infoEmp.department;

            if (infoEmp != null)
            {
                // DateTime temp = DateTime.Now;
                Program.mainForm.textBoxTimeNowCheckIn.Text = dateTime_checkin;
                Program.mainForm.beginTime = DateTime.Now;
            }
            // convert string to image
            if (infoEmp.avatar != "False")
            {
                string image = infoEmp.avatar;
                Char[] listTrim = { 'b', '\'' };
                image = image.Trim(listTrim);
                Program.mainForm.pictureBoxInfo.Image = GlobalVariables.cam_check.stringToImage(image);
            }

        }
        void Update_Info_Now_Checkout(API_odoo.MyResponse infoEmp, string dateTime_checkout)
        {
            Program.mainForm.textBoxName.Text = infoEmp.name;
            Program.mainForm.textBoxID.Text = infoEmp.id;
            Program.mainForm.textBoxDepartment.Text = infoEmp.department;
            if (infoEmp != null)
            {
                // DateTime temp = DateTime.Now;
                Program.mainForm.textBoxNowCheckOut.Text = dateTime_checkout;
                Program.mainForm.beginTime = DateTime.Now;
            }
            if (infoEmp.avatar != "False")
            {
                string image = infoEmp.avatar;
                Char[] listTrim = { 'b', '\'' };
                image = image.Trim(listTrim);
                Program.mainForm.pictureBoxInfo.Image = GlobalVariables.cam_check.stringToImage(image);
            }
            else
            {
                Image NoImageLoad = Image.FromFile("Resource_RFID/noimage.png");
                Program.mainForm.pictureBoxInfo.Image = NoImageLoad;
            }
            //  ShowInfo_Object.avatar.Image = GlobalData.Cam1.stringToImage(image);


        }
        void Update_Info_Recently_CheckINCheckOUT(API_odoo.MyResponse infoEmp)
        {

            string image_checkin = infoEmp.last_checkin_image;
            string image_checkout = infoEmp.last_checkout_image;
            Program.mainForm.textBoxTimeLastCheckIn.Text = infoEmp.last_checkin;
            Program.mainForm.textBoxTimeLastCheckOut.Text = infoEmp.last_checkout;
            if (image_checkin != "False")
            {
                Program.mainForm.pictureBoxLastCheckIn.Image = GlobalVariables.cam_check.stringToImage(image_checkin);

            }
            if (image_checkout != "False")
            {
                Program.mainForm.pictureBoxLastCheckOut.Image = GlobalVariables.cam_check.stringToImage(image_checkout);

            }

        }
        void updateImageCheckin(API_odoo.MyResponse infoEmp)
        {
            string image=infoEmp.checkin_image.ToString();
            if (image != "False")
            {
                Program.mainForm.pictureBoxNowCheckin.Image=GlobalVariables.cam_check.stringToImage(image);
            }
            if (infoEmp.checkin != "False")
            {
                Program.mainForm.textBoxTimeNowCheckIn.Text = infoEmp.checkin;
            }
        }        
        void updateImageCheckout(API_odoo.MyResponse infoEmp)
        {
            string image=infoEmp.checkout_image.ToString();
            if (image != "False")
            {
                Program.mainForm.pictureBoxNowCheckout.Image=GlobalVariables.cam_check.stringToImage(image);
            }
            if (infoEmp.checkout != "False")
            {
                Program.mainForm.textBoxNowCheckOut.Text = infoEmp.checkout;
            }
        }
        void Update_Info_REcently_CheckINCheckOUT_haveAVATAR(API_odoo.MyResponse infoEmp)
        {
            Program.mainForm.textBoxName.Text = infoEmp.name;
            Program.mainForm.textBoxID.Text = infoEmp.id;
            Program.mainForm.textBoxDepartment.Text = infoEmp.department;
            if (infoEmp.last_checkin != "False")
            {
                Program.mainForm.textBoxTimeLastCheckIn.Text = infoEmp.last_checkin;
            }
            if(infoEmp.last_checkout != "False")
            {
                Program.mainForm.textBoxTimeLastCheckOut.Text = infoEmp.last_checkout;
            }
            string image_checkin = infoEmp.last_checkin_image;
            string image_checkout = infoEmp.last_checkout_image;
            
            
            if (image_checkin != "False")
            {
                Program.mainForm.pictureBoxLastCheckIn.Image = GlobalVariables.cam_check.stringToImage(image_checkin);

            }
            if (image_checkout != "False")
            {
                Program.mainForm.pictureBoxLastCheckOut.Image = GlobalVariables.cam_check.stringToImage(image_checkout);
            }
            if (infoEmp.avatar != "False")
            {
                string image = infoEmp.avatar;
                Char[] listTrim = { 'b', '\'' };
                image = image.Trim(listTrim);
                Program.mainForm.pictureBoxInfo.Image = GlobalVariables.cam_check.stringToImage(image);
            }
        }

        //Enable Device
        public void OPOS_EnableDevice(AxOPOSRFID OPOSRFID1, string device_name)
        {
            int Result;
            int phase;
            string strData;

            
            // Open Device
            Result = OPOSRFID1.Open(device_name);
            if (Result != OposStatus.OposSuccess)
            {
                //MessageBox.Show("Can't open device with name is " + device_name, "Error enable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("err");
            }

            Result = OPOSRFID1.ClaimDevice(3000);
            if (Result != OposStatus.OposSuccess)
            {
                //MessageBox.Show("Can't claim device!", "Error enable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Err Claim");
                OPOSRFID1.Close();
            }

            OPOSRFID1.DeviceEnabled = true;
            Result = OPOSRFID1.ResultCode;
            if (Result != OposStatus.OposSuccess)
            {
                //MessageBox.Show("Can't enable device! ", "Error enable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Err Enable");
                OPOSRFID1.Close();
            }


            //    'DirectIOを用いて現在の位相状態を取得する
            phase = 0;
            strData = "";
            Result = OPOSRFID1.DirectIO(115, ref phase, ref strData);
            OPOSRFID1.BinaryConversion = OposStatus.OposBcNibble;
            //OPOSRFID1.BinaryConversion = OposStatus.OposBcNone;
            Result = OPOSRFID1.ResultCode;
            if (Result != OposStatus.OposSuccess)
            {
                OPOSRFID1.Close();
            }


            OPOSRFID1.ProtocolMask = OposStatus.RfidPrEpc1g2;
            Result = OPOSRFID1.ResultCode;
            if (Result != OposStatus.OposSuccess)
            {
                OPOSRFID1.Close();
            }
            else
            {
                //GlobalVariables.OPOSStatus = "ENABLE";
            }
        }
        //disable device
        public void OPOS_DisableDevice(AxOPOSRFID OPOSRFID1)
        {

            OPOSRFID1.DeviceEnabled = false;
            int result_disbale = OPOSRFID1.ResultCode;
      
            if(result_disbale != OposStatus.OposSuccess)
            {
                Console.WriteLine("Failed disable!");
            }
            
            
            int result_release = OPOSRFID1.ReleaseDevice();
        
            if(result_release != OposStatus.OposSuccess)
            {
                Console.WriteLine("Failed release!");
            }

            int result_close = OPOSRFID1.Close();

            if (result_close != OposStatus.OposSuccess)
            {
                Console.WriteLine("Failed close!");
                MessageBox.Show("Disable device failed! ", "Error disable", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if ((result_disbale == OposStatus.OposSuccess)
                && (result_release == OposStatus.OposSuccess)
                && (result_close == OposStatus.OposSuccess))
            { 
                //GlobalVariables.OPOSStatus = "DISABLE"; 
            }
        }

        //Scanning
        public void OPOS_StartReading(AxOPOSRFID OPOSRFID1, int readTimeInterval)
        {
            int Result;
            //OPOSRFID1.ClearInputProperties();
            OPOSRFID1.ReadTimerInterval = readTimeInterval;
            OPOSRFID1.DataEventEnabled = true;

            if (GlobalVariables.OPOSRFID1.TagCount > 0)
            {
                GlobalVariables.OPOSRFID1.ClearInputProperties();
            }


            PhaseChange(OPOSRFID1);
            Result = OPOSRFID1.StartReadTags(OposStatus.RfidRtId, "000000000000000000000000", "000000000000000000000000", 0, 0, 1000, "00000000");
            if (Result != OposStatus.OposSuccess)
            {
                //MessageBox.Show("Error start reading!", "Error enable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("read err");
            }
            else
            {
                GlobalVariables.OPOSStatus = "READING";
                //Program.mainForm.infoLog.Invoke(new Action(() => Program.mainForm.infoLog.Text += ">>> START READING RFID\r\n"));
            }


        }

        //Set phase lenght of code
        private void PhaseChange(AxOPOSRFID OPOSRFID1)
        {
            int Result;
            int intData;
            string strData;
            //'DirectIOを使用して位相の有効／無効を制御する
            //'位相を有効にするDirectIOを実行する
            intData = 0;
            strData = "";
            Result = OPOSRFID1.DirectIO(116, ref intData, ref strData);
            if (Result == OposStatus.OposEBusy)
            {
                Console.WriteLine("読み取り中です。StopReadTagsを実行してください");
            }
            else if (Result == OposStatus.OposEIllegal)
            {
                Console.WriteLine("共存できない機能を使用している可能性があります");
            }
            else if (Result != OposStatus.OposSuccess)
            {
                Console.WriteLine("位相設定失敗しました");
            }

        }

        //Stop read
        public void OPOS_StopReading(AxOPOSRFID OPOSRFID1)
        {
            int Result;
            Result = OPOSRFID1.StopReadTags("00000000");
            if (Result != OposStatus.OposSuccess)
            {
                Console.WriteLine("Err Stop");
            }
            else
            {
                //Program.mainForm.infoLog.Invoke(new Action(() => Program.mainForm.infoLog.Text += ">>> STOP READING RFID\r\n"));
                GlobalVariables.OPOSStatus = "STOPPING";
            }
        }


        //Call API
/*        public async Task RFIDtoJAN(HttpClient api_client, string code_value, IntPtr sub_wm)
        {
            string json = JsonSerializer.Serialize(new { rfid = code_value, api_key = GlobalVariables.api_key });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await api_client.PostAsync(GlobalVariables.sub_url, content);
            string resultContent = await result.Content.ReadAsStringAsync();

            MyJson objectJson = JsonSerializer.Deserialize<MyJson>(resultContent);
            if (objectJson.code == "00")
            {
                var jan1 = objectJson.data["jancode_1"];
                var jan2 = objectJson.data["jancode_2"];
                PressKeyJAN(jan1, jan2, sub_wm);
            }
            else
            {
                GlobalVariables.rfid_code.Remove(code_value);
                Console.WriteLine(objectJson.message);
            }

        }*/

        //Converte List
        private string ConvertTagIDCode(string code_value)
        {
            Dictionary<char, char> nibble_code = new Dictionary<char, char> { { ':', 'A' }, { ';', 'B' }, { '<', 'C' }, { '=', 'D' }, { '>', 'E' }, { '?', 'F' } };
            var stringBuilder = new StringBuilder();
            foreach (var character in code_value)
            {
                if (nibble_code.TryGetValue(character, out var value))
                {
                    stringBuilder.Append(value);
                }
                else
                {
                    stringBuilder.Append(character);
                }
            }
            return stringBuilder.ToString();
        }


        //Sendkeys to WEBPOS
        public static void PressKeyJAN(string jan1, string jan2, IntPtr sub_wm)
        {
            foreach (var c in jan2)
            {
                WindowAPI.PostMessage(sub_wm, (uint)GlobalVariables.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(GlobalVariables.Keys), "D" + c)), (int)(0));
            }
            WindowAPI.PostMessage(sub_wm, (uint)GlobalVariables.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(GlobalVariables.Keys), "Return")), (int)(0));

            foreach (var c in jan1)
            {
                WindowAPI.PostMessage(sub_wm, (uint)GlobalVariables.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(GlobalVariables.Keys), "D" + c)), (int)(0));
            }

            WindowAPI.PostMessage(sub_wm, (uint)GlobalVariables.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(GlobalVariables.Keys), "Return")), (int)(0));
            //Thread.Sleep(100);
        }

      

        //private DataEvent

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // OPOS
            // 
            this.Name = "OPOS";
            this.Load += new System.EventHandler(this.OPOS_Load);
            this.ResumeLayout(false);
        }

        private void OPOS_Load(object sender, EventArgs e)
        {

        }

        private IntPtr getPosScanScreen()
        {
            Process processes = Process.GetProcessesByName("AIR_START").First();
            IEnumerable<IntPtr> windows = CommonFunction.EnumerateProcessWindowHandles(processes);
            int defaultNumOfThread = windows.Count();
            IntPtr wnd = windows.First();
            return wnd;
        }

        public static void stopReading(AxOPOSRFID OPOSRFID1)
        {
            int Result;
            Result = OPOSRFID1.StopReadTags("00000000");
            if (Result != OposStatus.OposSuccess)
            {
                Console.WriteLine("Err Stop");
            }
        }    
    }
}
