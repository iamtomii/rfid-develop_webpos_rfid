
namespace WEBPOS_RFIDSender
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.notice = new System.Windows.Forms.Label();
            this.mainTitle = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.pictureBoxmute = new System.Windows.Forms.PictureBox();
            this.hightLight_Btn = new System.Windows.Forms.Panel();
            this.pictureBoxCheckin = new System.Windows.Forms.PictureBox();
            this.textBoxDepartment = new System.Windows.Forms.TextBox();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBoxInfo = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBoxNowCheckOut = new System.Windows.Forms.TextBox();
            this.textBoxTimeLastCheckOut = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBoxNowCheckout = new System.Windows.Forms.PictureBox();
            this.pictureBoxLastCheckOut = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.textBoxTimeNowCheckIn = new System.Windows.Forms.TextBox();
            this.textBoxTimeLastCheckIn = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBoxNowCheckin = new System.Windows.Forms.PictureBox();
            this.pictureBoxLastCheckIn = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panelInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxmute)).BeginInit();
            this.hightLight_Btn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCheckin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInfo)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNowCheckout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLastCheckOut)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNowCheckin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLastCheckIn)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel1.Controls.Add(this.notice);
            this.panel1.Controls.Add(this.mainTitle);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1784, 130);
            this.panel1.TabIndex = 6;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // notice
            // 
            this.notice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.notice.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.notice.ForeColor = System.Drawing.SystemColors.ControlText;
            this.notice.Location = new System.Drawing.Point(786, 36);
            this.notice.Name = "notice";
            this.notice.Size = new System.Drawing.Size(969, 60);
            this.notice.TabIndex = 61;
            this.notice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.notice.Click += new System.EventHandler(this.notice_Click);
            // 
            // mainTitle
            // 
            this.mainTitle.AutoSize = true;
            this.mainTitle.Font = new System.Drawing.Font("Impact", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainTitle.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.mainTitle.Location = new System.Drawing.Point(300, 36);
            this.mainTitle.Name = "mainTitle";
            this.mainTitle.Size = new System.Drawing.Size(382, 60);
            this.mainTitle.TabIndex = 51;
            this.mainTitle.Text = "Employee Manage";
            this.mainTitle.Click += new System.EventHandler(this.mainTitle_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(49, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(143, 124);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 50;
            this.pictureBox2.TabStop = false;
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.Color.GhostWhite;
            this.panelInfo.Controls.Add(this.pictureBoxmute);
            this.panelInfo.Controls.Add(this.hightLight_Btn);
            this.panelInfo.Controls.Add(this.textBoxDepartment);
            this.panelInfo.Controls.Add(this.textBoxID);
            this.panelInfo.Controls.Add(this.textBoxName);
            this.panelInfo.Controls.Add(this.label4);
            this.panelInfo.Controls.Add(this.label3);
            this.panelInfo.Controls.Add(this.label2);
            this.panelInfo.Controls.Add(this.label1);
            this.panelInfo.Controls.Add(this.pictureBoxInfo);
            this.panelInfo.Location = new System.Drawing.Point(12, 144);
            this.panelInfo.MaximumSize = new System.Drawing.Size(600, 800);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(473, 735);
            this.panelInfo.TabIndex = 7;
            // 
            // pictureBoxmute
            // 
            this.pictureBoxmute.Location = new System.Drawing.Point(185, 696);
            this.pictureBoxmute.Name = "pictureBoxmute";
            this.pictureBoxmute.Size = new System.Drawing.Size(39, 36);
            this.pictureBoxmute.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxmute.TabIndex = 9;
            this.pictureBoxmute.TabStop = false;
            this.pictureBoxmute.Click += new System.EventHandler(this.pictureBox1_Click_1);
            // 
            // hightLight_Btn
            // 
            this.hightLight_Btn.BackColor = System.Drawing.Color.DarkTurquoise;
            this.hightLight_Btn.Controls.Add(this.pictureBoxCheckin);
            this.hightLight_Btn.Location = new System.Drawing.Point(120, 542);
            this.hightLight_Btn.Name = "hightLight_Btn";
            this.hightLight_Btn.Size = new System.Drawing.Size(165, 117);
            this.hightLight_Btn.TabIndex = 8;
            // 
            // pictureBoxCheckin
            // 
            this.pictureBoxCheckin.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.pictureBoxCheckin.BackColor = System.Drawing.Color.Wheat;
            this.pictureBoxCheckin.Location = new System.Drawing.Point(16, 16);
            this.pictureBoxCheckin.Name = "pictureBoxCheckin";
            this.pictureBoxCheckin.Size = new System.Drawing.Size(134, 90);
            this.pictureBoxCheckin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCheckin.TabIndex = 0;
            this.pictureBoxCheckin.TabStop = false;
            this.pictureBoxCheckin.Click += new System.EventHandler(this.pictureBoxCheckin_Click);
            // 
            // textBoxDepartment
            // 
            this.textBoxDepartment.BackColor = System.Drawing.Color.PowderBlue;
            this.textBoxDepartment.Enabled = false;
            this.textBoxDepartment.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDepartment.Location = new System.Drawing.Point(171, 417);
            this.textBoxDepartment.Name = "textBoxDepartment";
            this.textBoxDepartment.ReadOnly = true;
            this.textBoxDepartment.Size = new System.Drawing.Size(198, 26);
            this.textBoxDepartment.TabIndex = 7;
            this.textBoxDepartment.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxID
            // 
            this.textBoxID.BackColor = System.Drawing.Color.PowderBlue;
            this.textBoxID.Enabled = false;
            this.textBoxID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxID.Location = new System.Drawing.Point(171, 372);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.ReadOnly = true;
            this.textBoxID.Size = new System.Drawing.Size(198, 26);
            this.textBoxID.TabIndex = 6;
            this.textBoxID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxID.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // textBoxName
            // 
            this.textBoxName.BackColor = System.Drawing.Color.PowderBlue;
            this.textBoxName.Enabled = false;
            this.textBoxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxName.Location = new System.Drawing.Point(171, 330);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.ReadOnly = true;
            this.textBoxName.Size = new System.Drawing.Size(198, 26);
            this.textBoxName.TabIndex = 5;
            this.textBoxName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxName.WordWrap = false;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DarkGreen;
            this.label4.Location = new System.Drawing.Point(147, 503);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 24);
            this.label4.TabIndex = 4;
            this.label4.Text = "CHECK-IN";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(33, 417);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 24);
            this.label3.TabIndex = 3;
            this.label3.Text = "Department";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(33, 374);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "ID";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 332);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // pictureBoxInfo
            // 
            this.pictureBoxInfo.BackColor = System.Drawing.Color.MintCream;
            this.pictureBoxInfo.Location = new System.Drawing.Point(99, 20);
            this.pictureBoxInfo.Name = "pictureBoxInfo";
            this.pictureBoxInfo.Size = new System.Drawing.Size(254, 264);
            this.pictureBoxInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxInfo.TabIndex = 0;
            this.pictureBoxInfo.TabStop = false;
            this.pictureBoxInfo.Click += new System.EventHandler(this.pictureBoxInfo_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.GhostWhite;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Location = new System.Drawing.Point(503, 144);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1281, 735);
            this.panel2.TabIndex = 8;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.AliceBlue;
            this.panel3.Controls.Add(this.textBoxNowCheckOut);
            this.panel3.Controls.Add(this.textBoxTimeLastCheckOut);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.pictureBoxNowCheckout);
            this.panel3.Controls.Add(this.pictureBoxLastCheckOut);
            this.panel3.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel3.Location = new System.Drawing.Point(665, 9);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(525, 677);
            this.panel3.TabIndex = 11;
            // 
            // textBoxNowCheckOut
            // 
            this.textBoxNowCheckOut.BackColor = System.Drawing.Color.PowderBlue;
            this.textBoxNowCheckOut.Enabled = false;
            this.textBoxNowCheckOut.Location = new System.Drawing.Point(134, 613);
            this.textBoxNowCheckOut.Name = "textBoxNowCheckOut";
            this.textBoxNowCheckOut.ReadOnly = true;
            this.textBoxNowCheckOut.Size = new System.Drawing.Size(310, 32);
            this.textBoxNowCheckOut.TabIndex = 7;
            this.textBoxNowCheckOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxNowCheckOut.TextChanged += new System.EventHandler(this.textBoxNowCheckOut_TextChanged);
            // 
            // textBoxTimeLastCheckOut
            // 
            this.textBoxTimeLastCheckOut.BackColor = System.Drawing.Color.PowderBlue;
            this.textBoxTimeLastCheckOut.Enabled = false;
            this.textBoxTimeLastCheckOut.Location = new System.Drawing.Point(134, 287);
            this.textBoxTimeLastCheckOut.Name = "textBoxTimeLastCheckOut";
            this.textBoxTimeLastCheckOut.ReadOnly = true;
            this.textBoxTimeLastCheckOut.Size = new System.Drawing.Size(310, 32);
            this.textBoxTimeLastCheckOut.TabIndex = 6;
            this.textBoxTimeLastCheckOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(36, 621);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 24);
            this.label10.TabIndex = 5;
            this.label10.Text = "Time";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(48, 295);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 24);
            this.label9.TabIndex = 4;
            this.label9.Text = "Time";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(227, 336);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 24);
            this.label8.TabIndex = 3;
            this.label8.Text = "Check Out";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(180, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(160, 24);
            this.label7.TabIndex = 2;
            this.label7.Text = "Last Check Out";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // pictureBoxNowCheckout
            // 
            this.pictureBoxNowCheckout.BackColor = System.Drawing.Color.MintCream;
            this.pictureBoxNowCheckout.Location = new System.Drawing.Point(134, 363);
            this.pictureBoxNowCheckout.Name = "pictureBoxNowCheckout";
            this.pictureBoxNowCheckout.Size = new System.Drawing.Size(310, 237);
            this.pictureBoxNowCheckout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxNowCheckout.TabIndex = 1;
            this.pictureBoxNowCheckout.TabStop = false;
            // 
            // pictureBoxLastCheckOut
            // 
            this.pictureBoxLastCheckOut.BackColor = System.Drawing.Color.MintCream;
            this.pictureBoxLastCheckOut.Location = new System.Drawing.Point(134, 38);
            this.pictureBoxLastCheckOut.Name = "pictureBoxLastCheckOut";
            this.pictureBoxLastCheckOut.Size = new System.Drawing.Size(310, 237);
            this.pictureBoxLastCheckOut.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLastCheckOut.TabIndex = 0;
            this.pictureBoxLastCheckOut.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.AliceBlue;
            this.panel5.Controls.Add(this.textBoxTimeNowCheckIn);
            this.panel5.Controls.Add(this.textBoxTimeLastCheckIn);
            this.panel5.Controls.Add(this.label12);
            this.panel5.Controls.Add(this.label11);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.pictureBoxNowCheckin);
            this.panel5.Controls.Add(this.pictureBoxLastCheckIn);
            this.panel5.Location = new System.Drawing.Point(88, 9);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(523, 677);
            this.panel5.TabIndex = 10;
            // 
            // textBoxTimeNowCheckIn
            // 
            this.textBoxTimeNowCheckIn.BackColor = System.Drawing.Color.PowderBlue;
            this.textBoxTimeNowCheckIn.Enabled = false;
            this.textBoxTimeNowCheckIn.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTimeNowCheckIn.Location = new System.Drawing.Point(104, 618);
            this.textBoxTimeNowCheckIn.Name = "textBoxTimeNowCheckIn";
            this.textBoxTimeNowCheckIn.ReadOnly = true;
            this.textBoxTimeNowCheckIn.Size = new System.Drawing.Size(310, 32);
            this.textBoxTimeNowCheckIn.TabIndex = 12;
            this.textBoxTimeNowCheckIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTimeLastCheckIn
            // 
            this.textBoxTimeLastCheckIn.BackColor = System.Drawing.Color.PowderBlue;
            this.textBoxTimeLastCheckIn.Enabled = false;
            this.textBoxTimeLastCheckIn.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTimeLastCheckIn.Location = new System.Drawing.Point(104, 292);
            this.textBoxTimeLastCheckIn.Name = "textBoxTimeLastCheckIn";
            this.textBoxTimeLastCheckIn.ReadOnly = true;
            this.textBoxTimeLastCheckIn.Size = new System.Drawing.Size(310, 32);
            this.textBoxTimeLastCheckIn.TabIndex = 11;
            this.textBoxTimeLastCheckIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxTimeLastCheckIn.TextChanged += new System.EventHandler(this.textBoxTimeLastCheckIn_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(21, 621);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 24);
            this.label12.TabIndex = 10;
            this.label12.Text = "Time";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(186, 336);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(96, 24);
            this.label11.TabIndex = 9;
            this.label11.Text = "Check in";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(21, 300);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 24);
            this.label6.TabIndex = 8;
            this.label6.Text = "Time";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(172, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 24);
            this.label5.TabIndex = 7;
            this.label5.Text = "Last Check in";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // pictureBoxNowCheckin
            // 
            this.pictureBoxNowCheckin.BackColor = System.Drawing.Color.MintCream;
            this.pictureBoxNowCheckin.Location = new System.Drawing.Point(104, 363);
            this.pictureBoxNowCheckin.Name = "pictureBoxNowCheckin";
            this.pictureBoxNowCheckin.Size = new System.Drawing.Size(310, 237);
            this.pictureBoxNowCheckin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxNowCheckin.TabIndex = 6;
            this.pictureBoxNowCheckin.TabStop = false;
            // 
            // pictureBoxLastCheckIn
            // 
            this.pictureBoxLastCheckIn.BackColor = System.Drawing.Color.MintCream;
            this.pictureBoxLastCheckIn.Location = new System.Drawing.Point(104, 38);
            this.pictureBoxLastCheckIn.Name = "pictureBoxLastCheckIn";
            this.pictureBoxLastCheckIn.Size = new System.Drawing.Size(310, 237);
            this.pictureBoxLastCheckIn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLastCheckIn.TabIndex = 5;
            this.pictureBoxLastCheckIn.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1784, 891);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RFID Reader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxmute)).EndInit();
            this.hightLight_Btn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCheckin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInfo)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNowCheckout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLastCheckOut)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNowCheckin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLastCheckIn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label notice;
        private System.Windows.Forms.Label mainTitle;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Panel hightLight_Btn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.PictureBox pictureBoxLastCheckIn;
        public System.Windows.Forms.PictureBox pictureBoxCheckin;
        public System.Windows.Forms.TextBox textBoxDepartment;
        public System.Windows.Forms.TextBox textBoxID;
        public System.Windows.Forms.TextBox textBoxName;
        public System.Windows.Forms.PictureBox pictureBoxInfo;
        public System.Windows.Forms.TextBox textBoxNowCheckOut;
        public System.Windows.Forms.TextBox textBoxTimeLastCheckOut;
        public System.Windows.Forms.PictureBox pictureBoxNowCheckout;
        public System.Windows.Forms.PictureBox pictureBoxLastCheckOut;
        public System.Windows.Forms.PictureBox pictureBoxNowCheckin;
        public System.Windows.Forms.TextBox textBoxTimeNowCheckIn;
        public System.Windows.Forms.TextBox textBoxTimeLastCheckIn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.PictureBox pictureBoxmute;
    }
}

