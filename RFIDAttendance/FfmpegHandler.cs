using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RFIDAttendance.StateModel;
using MediaToolkit;
using System.Threading;
using MediaToolkit.Model;
using System.IO;

namespace RFIDAttendance
{
    public class FfmpegHandler
    {
        public static async Task<bool> ExecuteFFMpegAsync(string arguments)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg.exe",
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();
                    int counttime = 0;
                    while (!process.HasExited)
                    {


                        if (counttime==30)
                        {
                            process.Close();
                            WriteLog("oke");
                            return false;
                        }
                        else
                        {
                            counttime++;
                        }
                        WriteLog(counttime.ToString());

                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return true;
        }
        private static void WriteLog(string data)
        {
            using (TextWriter writer = new StreamWriter("Log_data.txt", true))  // true is for append mode
            {
                writer.WriteLine(data);
            }
        }


        public static string ConvertFile(ConvertFileDetails convertFileDetails,
            EventHandler<ConvertProgressEventArgs> convertProgressEvent, EventHandler<ConversionCompleteEventArgs> conversionCompleteEvent)
        {
            try
            {
                using (var engine = new Engine())
                {
                    var inputFile = new MediaFile { Filename = convertFileDetails.InputFilePath };
                    var outputFile = new MediaFile { Filename = convertFileDetails.OutputFilePath };

                    
                    engine.ConvertProgressEvent += convertProgressEvent;
                    engine.ConversionCompleteEvent += conversionCompleteEvent;

                    engine.Convert(inputFile, outputFile);

                    Process.Start("explorer.exe", "/select, \"" + convertFileDetails.OutputFilePath + "\"");
                    return "Success";

                }
            }
            catch
            {
                return "Error";
            }

        }

    }
}
