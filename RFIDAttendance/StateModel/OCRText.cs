using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using RFIDAttendance.Common;
using RFIDAttendance.WinAPI;
using System.IO;

namespace RFIDAttendance.StateModel
{
    class OCRText
    {
        public static string GetWorkingTitle()
        {
            Rect rc;
            Process check_processes;
            IEnumerable<IntPtr> check_windows;
            check_processes = Process.GetProcessesByName("AIR_START").First();
            check_windows = EnumerateProcessWindowHandles(check_processes);
            IntPtr wnd = check_windows.First();

            string plainText = "";
            try
            {
                WindowAPI.GetWindowRect(wnd, out rc);

                Bitmap capture = new Bitmap(500, 450, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics gfxBmp = Graphics.FromImage(capture);
                IntPtr hdcBitmap = gfxBmp.GetHdc();


                WindowAPI.PrintWindow(wnd, hdcBitmap, 0);

                gfxBmp.ReleaseHdc(hdcBitmap);
                gfxBmp.Dispose();

                Rectangle rect = new System.Drawing.Rectangle(105, 65, 150, 30);
                Bitmap image = capture.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                plainText = detectTitleText(image);
                

            }
            catch (Exception e)
            {
                MainForm.WriteLog("OCRText >> " + e.Message);
                plainText = ""; 
            }

            return plainText;

        }

        public static IEnumerable<IntPtr> EnumerateProcessWindowHandles(Process process)
        {
            var handles = new List<IntPtr>();
            foreach (ProcessThread thread in process.Threads)
                WindowAPI.EnumThreadWindows(thread.Id, (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);
            return handles;
        }



        public static List<Image> getTileStatic_Image(string path_img)
        {
            List<Image> titleImage = new List<Image>();
            string[] fileArray = Directory.GetFiles(path_img, "*.jpg");
            foreach(string fname in fileArray)
            {
                Image img = Bitmap.FromFile(fname);
                titleImage.Add(img);
            }
            return titleImage;         
        }

        public static bool matchTitle(Image img01, Image img02)
        {
            Bitmap Image01 = (Bitmap)(img01);
            Bitmap Image02 = (Bitmap)(img02);
            int result = 0;

            for (int x = 0; x < Image01.Width; x++)
            {
                for (int y = 0; y < Image01.Height; y++)
                {
                    Color color1 = Image01.GetPixel(x, y);
                    Color color2 = Image02.GetPixel(x, y);
                    int diff =
                       Math.Abs(color1.R - color2.R) +
                       Math.Abs(color1.G - color2.G) +
                       Math.Abs(color1.B - color2.B);

                    result += diff;
                }
            }

            if(result == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string detectTitleText(Image src)
        {
            int index = -1;
            string title_working = "";
            for(int i=0; i < GlobalVariables.staticTitle.Count; i++)
            {
                if(matchTitle(src, GlobalVariables.staticTitle[i]))
                {
                    index = i;
                    break;
                }
            }

            switch (index)
            {
                case 0:
                    title_working = "会員/客層入力";
                    break;
                case 1:
                    title_working = "商品登録";
                    break;
                case 2:
                    title_working = "個数修正";
                    break;
                case 3:
                    title_working = "現金入力";
                    break;
                default:
                    break;
            }

            return title_working;
        }
    }
}
