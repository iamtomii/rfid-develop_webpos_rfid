using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WEBPOS_RFIDSender.WinAPI;

namespace WEBPOS_RFIDSender.Common
{
    class CommonFunction
    {
        public static IntPtr getPOSWindow()
        {
            Process webpos = Process.GetProcessesByName("AIR_START").First();
            IntPtr mainWnd = webpos.MainWindowHandle;
            return mainWnd;
        }
        public static Process processes;
        public static IEnumerable<IntPtr> windows;

        public static int defaultNumOfThread;

        public static bool GetWebPOSScreen()
        {
            try
            {
                processes = Process.GetProcessesByName("AIR_START").First();
                windows = EnumerateProcessWindowHandles(processes);
                defaultNumOfThread = windows.Count();
                GlobalVariables.mainWnd = windows.First();
                StringBuilder title = new StringBuilder(256);
                WindowAPI.GetWindowText(windows.First(), title, 256);
                Console.WriteLine(title);

            }
            catch (Exception)
            {
                return false;
            }

            if (GlobalVariables.mainWnd.Equals(IntPtr.Zero))
            {
                return false;
            }
            return true;
        }

        public static IEnumerable<IntPtr> EnumerateProcessWindowHandles(Process process)
        {
            var handles = new List<IntPtr>();
            foreach (ProcessThread thread in process.Threads)
                WindowAPI.EnumThreadWindows(thread.Id, (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);
            return handles;
        }

        public static void readConfigFile()
        {
            Dictionary<string, string> dataInFile = getDictionaryConfig("SENDER_CONFIG.ini");
            List<string> listKey = new List<string>(new string[] { "api_key", "url_api", "sub_url", "device_name", "vesca_emoney", "vesca_credit" });
            foreach (string key in listKey)
            {
                if (!dataInFile.ContainsKey(key))
                {
                    MessageBox.Show("Not found key: " + key + "!Please check key name in config file!",
                                   "Not found key", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            }


            GlobalVariables.api_key = dataInFile["api_key"];
            GlobalVariables.url_api = dataInFile["url_api"];
            GlobalVariables.sub_url = dataInFile["sub_url"];
            GlobalVariables.device_name = dataInFile["device_name"];
            GlobalVariables.rT = (int)Int64.Parse(dataInFile["rT"]);


            GlobalVariables.webpos_local_data_url = dataInFile["webpos_local_data_url"];

            GlobalVariables.input_id_emp = dataInFile["input_id_emp"];
            GlobalVariables.id_emp = dataInFile["id_emp"];
            GlobalVariables.sleep_key = Int32.Parse(dataInFile["sleep_key"]);

            //gate
            GlobalVariables.ept_gate = dataInFile["ept_gate"];
            GlobalVariables.ept_token = dataInFile["ept_token"];
            GlobalVariables.ept_unload = dataInFile["ept_unload"];

            GlobalVariables.webpos_app_path = dataInFile["webpos_app_path"];
            GlobalVariables.url_Odoo = dataInFile["url_Odoo"];
            GlobalVariables.url_checkin = dataInFile["url_checkin"];
            GlobalVariables.url_createnew = dataInFile["url_createnew"];
            GlobalVariables.url_checkout = dataInFile["url_checkout"];
            GlobalVariables.url_api_Employee = dataInFile["url_api_Employee"];
            GlobalVariables.url_camera = dataInFile["url_camera"];
            GlobalVariables.path_ImageError = dataInFile["path_ImageError"];

            string[] titles = dataInFile["state_title"].Split(',');
            foreach(string title in titles)
            {
                string[] sub_data = title.Split(':');
                string key = sub_data[0];
                string value = sub_data[1];

                GlobalVariables.state_titile.Add(key, value);
            }
        }

        public static Dictionary<string, string> getDictionaryConfig(string path)
        {
            Dictionary<string, string> Config = new Dictionary<string, string>();
            List<string> result = readDataFile(path);
            foreach (string line in result)
            {
                if (!line.Contains("="))
                {
                    MessageBox.Show("Config file have Incorrect syntax!",
                                   "Format Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
                string[] temp = line.Split('=');
                Config[temp[0].Trim()] = temp[1].Trim();
            }
            return Config;
        }

        public static List<string> readDataFile(string path)
        {
            List<string> result = new List<string>();
            if (!File.Exists(path))
            {
                MessageBox.Show("Error. Can not found file!\nPlease add config file! ",
                                   "Can not found file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            else
                try
                {
                    StreamReader sr = new StreamReader(path);
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        result.Add(line);
                        line = sr.ReadLine();
                    }
                    sr.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error. Can not read file!" + "\n" + e,
                                   "Can not read file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            return result;
        }
    }
}
