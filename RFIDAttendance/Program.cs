using Microsoft.Win32;
using RFIDAttendance.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace RFIDAttendance
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
            try
            {

                mainForm.ShowDialog();
            }
            catch (Exception e)
            {
                WriteLogE(e);
            }

        }
        private static void WriteLogE(Exception exception)
        {
            using (TextWriter writer = new StreamWriter("Log_data.txt", true))
            {
                writer.WriteLine(
                    "=>{0} An Error occurred: {1}  Message: {2}{3}",
                    DateTime.Now,
                    exception.StackTrace,
                    exception.Message,
                    Environment.NewLine
                    );
            }

        }
    }
}
