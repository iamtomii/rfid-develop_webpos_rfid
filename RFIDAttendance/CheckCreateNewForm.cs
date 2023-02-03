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
using RFIDAttendance.Common;
using System.Speech.Synthesis;
using System.IO;

namespace RFIDAttendance
{
    public partial class CheckCreateNewForm : Form
    {

        dialog dialogresult = new dialog();
        API_odoo.InfoResponse infoEmpbyid = new API_odoo.InfoResponse();
        public string RFID_exist { get; set; }
        public string PINCODE { get; set; }
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
            this.textBoxPINID.Visible = true;
            this.textBoxPINID.Text = null;
            this.inputtext.Visible= true;
            this.buttonYes.Visible= true;
            this.labeltatus.Text = "RFID : "+ RFID_exist;
            labelname.Text = null;
            labelphone.Text = null;
            labelid.Text = null;
            labeldeparment.Text = null;
            pictureBoxavatar.Image = null;

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
            if (inputString.Equals("False"))
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

        private async void textBoxID_TextChanged(object sender, EventArgs e)
        {
            textBoxPINID.Focus();

        }

        private async void buttonYes_Click(object sender, EventArgs e)
        {

            Close();
            API_odoo api = new API_odoo();
            String message = await api.APICreateNewRFIDEMployee(PINCODE, RFID_exist, GlobalVariables.url_Odoo, GlobalVariables.url_createnew);

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

        private void buttonNo_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void labelname_Click(object sender, EventArgs e)
        {

        }

        private void labelid_Click(object sender, EventArgs e)
        {

        }

        private void labeldeparment_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void idlabel_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxavatar_Click(object sender, EventArgs e)
        {

        }

        private async void buttonset_Click(object sender, EventArgs e)
        {
           
            this.PINCODE= textBoxPINID.Text;
            API_odoo api = new API_odoo();
            infoEmpbyid = await api.APIGetInfoEmployeebyID(GlobalVariables.url_Odoo, GlobalVariables.url_showinfo, PINCODE);
            if (infoEmpbyid.code == "ok")
            {
                labeltatus.Text = "RFID : " + RFID_exist;
                labelname.Text = infoEmpbyid.name;
                labelphone.Text = infoEmpbyid.phone;
                labelid.Text = infoEmpbyid.ID;
                labeldeparment.Text = infoEmpbyid.department;
                pictureBoxavatar.Image = stringToImage(infoEmpbyid.avatar);

            }
            else
            {
                labeltatus.Text = infoEmpbyid.code;
                labeltatus.ForeColor = Color.DarkOrange;
                labelname.Text = null;
                labelphone.Text = null;
                labelid.Text = null;
                labeldeparment.Text = null;
                pictureBoxavatar.Image = null;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
