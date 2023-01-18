using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WEBPOS_RFIDSender
{

    public partial class dialog : Form
    {
        public String tatustext { get; set; }
        public dialog()
        {
            InitializeComponent();
        }

        private void tatus_Click(object sender, EventArgs e)
        {

        }

        private void dialog_Load(object sender, EventArgs e)
        {
            
            tatus.Text= tatustext;
        }

        private void buttonyes_Click(object sender, EventArgs e)
        {

        }

        private void buttonok_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
