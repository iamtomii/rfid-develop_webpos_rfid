using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WEBPOS_RFIDSender.Common;
using System.Speech.Synthesis;

namespace WEBPOS_RFIDSender
{
    public partial class CheckCreateNewForm : Form
    {

        public string RFID_exist { get; set; }
        public CheckCreateNewForm(string RFID_exist)
        {
            this.RFID_exist= RFID_exist;

        }
        public CheckCreateNewForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void CheckCreateNewForm_Load(object sender, EventArgs e)
        {
            this.textBoxID.Visible = true;
            this.textBoxID.Text = null;
            this.noticecreate.Text = "RFID not exist! Do you want create new ?";
            this.inputtext.Visible= true;
            this.buttonYes.Visible= true;
            this.buttonNo.Text = "NO";
        }

        private void textBoxID_TextChanged(object sender, EventArgs e)
        {

        }

        private async void buttonYes_Click(object sender, EventArgs e)
        {
            String text=textBoxID.Text;
            Console.WriteLine(text);
            API_odoo api = new API_odoo();
            String message = await api.APICreateNewRFIDEMployee(text, RFID_exist,GlobalVariables.url_Odoo,GlobalVariables.url_createnew);

            JObject obj = JObject.Parse(message);

            if (!(obj.ContainsKey("debug")))
            {
                Console.WriteLine(obj["name"].ToString());
                String name = obj["name"].ToString();
                noticecreate.Text = "RFID is created with employee " + name;

                inputtext.Visible = false;
                buttonYes.Visible = false;
                textBoxID.Visible = false;

                buttonNo.Text = "OK";




            }
            else
            {
                Console.WriteLine(obj.ToString());
                String error = "Can not create! \n" + obj["message"].ToString();
                noticecreate.Text = error;
                inputtext.Visible = false;
                buttonYes.Visible = false;
                textBoxID.Visible = false;
                buttonNo.Text = "OK";

            }


        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
