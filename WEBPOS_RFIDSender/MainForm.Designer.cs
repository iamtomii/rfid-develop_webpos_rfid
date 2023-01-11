
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.startBtn = new System.Windows.Forms.Button();
            this.stopBtn = new System.Windows.Forms.Button();
            this.statusLB = new System.Windows.Forms.Label();
            this.statusContent = new System.Windows.Forms.TextBox();
            this.infoLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // startBtn
            // 
            this.startBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.startBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startBtn.ForeColor = System.Drawing.Color.Green;
            this.startBtn.Location = new System.Drawing.Point(335, 26);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(73, 40);
            this.startBtn.TabIndex = 0;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = false;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // stopBtn
            // 
            this.stopBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.stopBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.stopBtn.Location = new System.Drawing.Point(414, 26);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(73, 40);
            this.stopBtn.TabIndex = 1;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = false;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // statusLB
            // 
            this.statusLB.AutoSize = true;
            this.statusLB.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLB.Location = new System.Drawing.Point(20, 32);
            this.statusLB.Name = "statusLB";
            this.statusLB.Size = new System.Drawing.Size(66, 25);
            this.statusLB.TabIndex = 2;
            this.statusLB.Text = "Status";
            // 
            // statusContent
            // 
            this.statusContent.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusContent.Location = new System.Drawing.Point(92, 32);
            this.statusContent.Multiline = true;
            this.statusContent.Name = "statusContent";
            this.statusContent.ReadOnly = true;
            this.statusContent.Size = new System.Drawing.Size(118, 29);
            this.statusContent.TabIndex = 3;
            this.statusContent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.statusContent.TextChanged += new System.EventHandler(this.statusContent_TextChanged);
            // 
            // infoLog
            // 
            this.infoLog.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLog.Location = new System.Drawing.Point(15, 81);
            this.infoLog.Multiline = true;
            this.infoLog.Name = "infoLog";
            this.infoLog.ReadOnly = true;
            this.infoLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.infoLog.Size = new System.Drawing.Size(472, 344);
            this.infoLog.TabIndex = 4;
            this.infoLog.TextChanged += new System.EventHandler(this.infoLog_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OldLace;
            this.ClientSize = new System.Drawing.Size(507, 450);
            this.Controls.Add(this.infoLog);
            this.Controls.Add(this.statusContent);
            this.Controls.Add(this.statusLB);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.startBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "RFID Reader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.Label statusLB;
        private System.Windows.Forms.TextBox statusContent;
        public System.Windows.Forms.TextBox infoLog;
    }
}

