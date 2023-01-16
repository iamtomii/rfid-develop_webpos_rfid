using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using WEBPOS_RFIDSender.Common;
using OpenCvSharp;

namespace WEBPOS_RFIDSender
{
    public partial class showinfo : Form
    {
        dialog dialogresult = new dialog();
        public String Name { get; set; }
        public String ID { get; set; }
        public String deparment { get; set; }
        public String  avatar { get; set; }
        public String RFID { get; set; }
        public String PIN { get; set; }
        public showinfo()
        {
            InitializeComponent();
        }

        private void showinfo_Load(object sender, EventArgs e)
        {
            labelname.Text= Name;
            labelid.Text= ID;
            labeldeparment.Text= deparment;
            pictureBoxavatar.Image = stringToImage(avatar);
        }
        public System.Drawing.Image stringToImage(string inputString)
        {
            Char[] listTrim = { 'b', '\'' };
            inputString = inputString.Trim(listTrim);
            System.Drawing.Image image = new Bitmap(100, 1000);
            if (string.IsNullOrEmpty(inputString))
            {
                return image;
            }
            byte[] bytes = Convert.FromBase64String(inputString);
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                image = System.Drawing.Image.FromStream(ms, true, true);
                return image;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void labelid_Click(object sender, EventArgs e)
        {

        }

        private void labeldeparment_Click(object sender, EventArgs e)
        {

        }

        private async void buttonyes_Click(object sender, EventArgs e)
        {   
            Close();
            API_odoo api = new API_odoo();
            String message = await api.APICreateNewRFIDEMployee(PIN, RFID, GlobalVariables.url_Odoo, GlobalVariables.url_createnew);

            JObject obj = JObject.Parse(message);
            if (!(obj.ContainsKey("debug")))
            {
                Console.WriteLine(obj["name"].ToString());
                String name = obj["name"].ToString();
                dialogresult.tatustext = "RFID is created with employee " + name;
                dialogresult.ShowDialog();




            }
            else
            {
                Console.WriteLine(obj.ToString());
                dialogresult.tatustext = "Can not create! \n" + obj["message"].ToString();
                dialogresult.ShowDialog();

            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonno_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
