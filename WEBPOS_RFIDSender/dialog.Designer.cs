namespace WEBPOS_RFIDSender
{
    partial class dialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dialog));
            this.buttonok = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tatus = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonok
            // 
            this.buttonok.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonok.Location = new System.Drawing.Point(434, 160);
            this.buttonok.Name = "buttonok";
            this.buttonok.Size = new System.Drawing.Size(75, 34);
            this.buttonok.TabIndex = 1;
            this.buttonok.Text = "OK";
            this.buttonok.UseVisualStyleBackColor = true;
            this.buttonok.Click += new System.EventHandler(this.buttonok_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tatus);
            this.panel1.Location = new System.Drawing.Point(36, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(494, 100);
            this.panel1.TabIndex = 2;
            // 
            // tatus
            // 
            this.tatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tatus.AutoSize = true;
            this.tatus.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tatus.Location = new System.Drawing.Point(19, 38);
            this.tatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tatus.Name = "tatus";
            this.tatus.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tatus.Size = new System.Drawing.Size(322, 19);
            this.tatus.TabIndex = 3;
            this.tatus.Text = "\"RFID is created with employee Name";
            this.tatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(556, 206);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonok);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "dialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "dialog";
            this.Load += new System.EventHandler(this.dialog_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonok;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label tatus;
    }
}