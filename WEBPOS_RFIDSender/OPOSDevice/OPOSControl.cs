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

namespace WEBPOS_RFIDSender.OposControl
{
    public class MyJson
    {
        public string code { get; set; }
        public Dictionary<string, string> data { get; set; }
        public bool isSuccess { get; set; }
        public string message { get; set; }
    }

    public class OPOS : System.Windows.Forms.UserControl
    {
        public OPOS()
        {
            GlobalVariables.OPOSRFID1.DataEvent += OPOSRFID1_DataEvent;
        }

        private void OPOSRFID1_DataEvent(object sender, _DOPOSRFIDEvents_DataEventEvent e)
        {
            WindowAPI.PostMessage(IntPtr.Zero, (uint)GlobalVariables.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(GlobalVariables.Keys), "Return")), (int)(0));

            int TagCount;
            string UserData;
            HttpClient api_client = new HttpClient();
            api_client.BaseAddress = new Uri(GlobalVariables.url_api);
            api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
                    if (GlobalVariables.rfid_code.Count <= 50)
                    {
                        Console.WriteLine("RFID CODE: " + new_code);
                        //infoLog.Text += String.Format("RFID CODE: {0}\n", new_code);
                        Program.mainForm.infoLog.Invoke(new Action(() => Program.mainForm.infoLog.AppendText(String.Format("{0}\r\n", new_code))));

                        GlobalVariables.rfid_code.Add(new_code);
                        
                        //call API
                    }
                    else
                    {
                        MessageBox.Show("1回のお支払いにつき50冊が上限です");
                    }

                }

                GlobalVariables.OPOSRFID1.NextTag();
            }
            GlobalVariables.OPOSRFID1.DataEventEnabled = true;
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
                Program.mainForm.infoLog.Invoke(new Action(() => Program.mainForm.infoLog.Text += ">>> START READING RFID\r\n"));
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
                Program.mainForm.infoLog.Invoke(new Action(() => Program.mainForm.infoLog.Text += ">>> STOP READING RFID\r\n"));
                GlobalVariables.OPOSStatus = "STOPPING";
            }
        }


        //Call API
        public async Task RFIDtoJAN(HttpClient api_client, string code_value, IntPtr sub_wm)
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

        }

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
