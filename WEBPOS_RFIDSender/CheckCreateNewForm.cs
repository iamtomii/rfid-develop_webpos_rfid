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
        showinfo showifo = new showinfo();
        dialog dialogresult = new dialog();
        API_odoo.InfoResponse infoEmpbyid = new API_odoo.InfoResponse();
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

        private async void textBoxID_TextChanged(object sender, EventArgs e)
        {
            textBoxID.Focus();

        }

        private async void buttonYes_Click(object sender, EventArgs e)
        {
            Close();
            String text=textBoxID.Text;
            Console.WriteLine(text);
            API_odoo api = new API_odoo();
            infoEmpbyid = await api.APIGetInfoEmployeebyID(GlobalVariables.url_Odoo,GlobalVariables.url_showinfo,text);
            if (infoEmpbyid.code == "ok")
            {
                showifo.Name = infoEmpbyid.name;
                showifo.ID = infoEmpbyid.ID;
                showifo.deparment = infoEmpbyid.department;
                showifo.avatar= infoEmpbyid.avatar;
                showifo.RFID = RFID_exist;
                showifo.PIN = text;
                showifo.ShowDialog();
                
            }
            else
            {
                String tatustext = infoEmpbyid.code;
                dialogresult.tatustext= tatustext;
                dialogresult.ShowDialog();
            }
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
