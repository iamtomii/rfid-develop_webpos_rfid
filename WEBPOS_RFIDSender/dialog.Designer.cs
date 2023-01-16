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
            this.buttonok = new System.Windows.Forms.Button();
            this.tatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonok
            // 
            this.buttonok.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonok.Location = new System.Drawing.Point(363, 131);
            this.buttonok.Name = "buttonok";
            this.buttonok.Size = new System.Drawing.Size(75, 34);
            this.buttonok.TabIndex = 1;
            this.buttonok.Text = "OK";
            this.buttonok.UseVisualStyleBackColor = true;
            this.buttonok.Click += new System.EventHandler(this.buttonok_Click);
            // 
            // tatus
            // 
            this.tatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tatus.AutoSize = true;
            this.tatus.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tatus.Location = new System.Drawing.Point(81, 66);
            this.tatus.Name = "tatus";
            this.tatus.Size = new System.Drawing.Size(318, 19);
            this.tatus.TabIndex = 2;
            this.tatus.Text = "\"RFID is created with employee Name";
            this.tatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tatus.Click += new System.EventHandler(this.tatus_Click);
            // 
            // dialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 177);
            this.Controls.Add(this.tatus);
            this.Controls.Add(this.buttonok);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "dialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "dialog";
            this.Load += new System.EventHandler(this.dialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonok;
        private System.Windows.Forms.Label tatus;
    }
}