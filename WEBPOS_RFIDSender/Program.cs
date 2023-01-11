using System;
using System.Windows.Forms;


namespace WEBPOS_RFIDSender
{
    static class Program
    {
        public static MainForm mainForm = new MainForm();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
            mainForm.ShowDialog();
        }

        
    }
}
