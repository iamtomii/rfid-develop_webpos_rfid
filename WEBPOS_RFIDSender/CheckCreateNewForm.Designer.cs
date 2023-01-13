namespace WEBPOS_RFIDSender
{
    partial class CheckCreateNewForm
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
            this.noticecreate = new System.Windows.Forms.Label();
            this.buttonYes = new System.Windows.Forms.Button();
            this.buttonNo = new System.Windows.Forms.Button();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.inputtext = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // noticecreate
            // 
            this.noticecreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.noticecreate.AutoSize = true;
            this.noticecreate.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noticecreate.Location = new System.Drawing.Point(120, 64);
            this.noticecreate.Name = "noticecreate";
            this.noticecreate.Size = new System.Drawing.Size(431, 24);
            this.noticecreate.TabIndex = 0;
            this.noticecreate.Text = "RFID not exist! Do you want create new ?";
            this.noticecreate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.noticecreate.Click += new System.EventHandler(this.label1_Click);
            // 
            // buttonYes
            // 
            this.buttonYes.Location = new System.Drawing.Point(419, 204);
            this.buttonYes.Name = "buttonYes";
            this.buttonYes.Size = new System.Drawing.Size(79, 36);
            this.buttonYes.TabIndex = 1;
            this.buttonYes.Text = "YES";
            this.buttonYes.UseVisualStyleBackColor = true;
            this.buttonYes.Click += new System.EventHandler(this.buttonYes_Click);
            // 
            // buttonNo
            // 
            this.buttonNo.Location = new System.Drawing.Point(522, 204);
            this.buttonNo.Name = "buttonNo";
            this.buttonNo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonNo.Size = new System.Drawing.Size(80, 36);
            this.buttonNo.TabIndex = 2;
            this.buttonNo.Text = "NO";
            this.buttonNo.UseVisualStyleBackColor = true;
            this.buttonNo.Click += new System.EventHandler(this.buttonNo_Click);
            // 
            // textBoxID
            // 
            this.textBoxID.Location = new System.Drawing.Point(336, 146);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.Size = new System.Drawing.Size(179, 20);
            this.textBoxID.TabIndex = 3;
            this.textBoxID.TextChanged += new System.EventHandler(this.textBoxID_TextChanged);
            // 
            // inputtext
            // 
            this.inputtext.AutoSize = true;
            this.inputtext.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputtext.Location = new System.Drawing.Point(146, 144);
            this.inputtext.Name = "inputtext";
            this.inputtext.Size = new System.Drawing.Size(129, 19);
            this.inputtext.TabIndex = 4;
            this.inputtext.Text = "Input your ID :";
            // 
            // CheckCreateNewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 265);
            this.Controls.Add(this.inputtext);
            this.Controls.Add(this.textBoxID);
            this.Controls.Add(this.buttonNo);
            this.Controls.Add(this.buttonYes);
            this.Controls.Add(this.noticecreate);
            this.Name = "CheckCreateNewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create new rfid card";
            this.Load += new System.EventHandler(this.CheckCreateNewForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label noticecreate;
        private System.Windows.Forms.Button buttonYes;
        private System.Windows.Forms.Button buttonNo;
        private System.Windows.Forms.TextBox textBoxID;
        private System.Windows.Forms.Label inputtext;
    }
}