using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBPOS_RFIDSender
{
    class CameraController
    {
        private VideoCapture capture = null;

        //private bool captureInProgress;
        // Declare required methods
        public void StreamVideo(string IPorURLCamera)
        {

            bool isIntPort = int.TryParse(IPorURLCamera, out int portInt);
            if (!isIntPort)
            {
                capture = new VideoCapture(IPorURLCamera);
                capture.Open(IPorURLCamera);
            }
            else
            {
                capture = new VideoCapture(portInt);
                capture.Open(portInt);
            }


        }

        public string GetImage()
        {
            Mat currentFrame = new Mat();
            string base64String;
            currentFrame = capture.RetrieveMat();

            //try
            //{
            //    currentFrame = capture.QueryFrame();
            //    int i = 3;

            //    while (i > 0)
            //    {
            //        if (currentFrame == null)
            //        {
            //            currentFrame = capture.QueryFrame();
            //            i--;
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //}
            //catch(Exception e){
            //    Console.WriteLine("CAMERA EXCEPTION: "+e);

            //}

            if (!currentFrame.Empty())
            {
                Bitmap saveImage = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(currentFrame);

                System.IO.MemoryStream stream = new MemoryStream();
                saveImage.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);

                byte[] imageBytes = stream.ToArray();
                base64String = Convert.ToBase64String(imageBytes);
                //DateTime time1 = DateTime.Now;
                //  saveImage.Save(time1.ToString("yyyyMMdd-HH:mm:ss") + ".png");
            }
            else
            {

                Image saveImageLoad = Image.FromFile("Resource_RFID/error.png");
                byte[] imageBytes = imgToByteArray(saveImageLoad);
                base64String = Convert.ToBase64String(imageBytes);
            }

            return base64String;

        }
        public byte[] imgToByteArray(Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }
        public Image GetImageBitmap()
        {

            Bitmap frame = new Bitmap(100, 100);
            Mat currentFrame = capture.RetrieveMat();
            if (currentFrame != null)
            {
                frame = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(currentFrame);
            }

            return frame;

        }

        public Image stringToImage(string inputString)
        {
            Image image = new Bitmap(100, 1000);

            if (string.IsNullOrEmpty(inputString))
            {
                return image;
            }

            byte[] bytes = Convert.FromBase64String(inputString);

            // byte[] bytes = Convert.FromBase64String(inputString);
            // Image image = new Bitmap(200, 200);


            //  byte[] imageBytes = Convert.FromBase64String(inputString);
            //Convert byte[] to Image
            /*using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                image = Image.FromStream(ms, true, true);
                return image;
            }*/

            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                image = Image.FromStream(ms, true, true);
                return image;
            }




        }

        public string getSaveFolder(string parent_dict, string check_dict, string date_dict)
        {
            if (!Directory.Exists(parent_dict))
            {
                Directory.CreateDirectory(parent_dict);
            }

            string combinedPath = Path.Combine(parent_dict, check_dict);
            if (!Directory.Exists(combinedPath))
            {
                Directory.CreateDirectory(combinedPath);
            }

            string combinedPath1 = Path.Combine(combinedPath, date_dict);
            if (!Directory.Exists(combinedPath1))
            {
                Directory.CreateDirectory(combinedPath1);

            }
            return combinedPath1;
        }

        public void queyFrame()
        {
            while (true)
            {
                try
                {

                    capture.RetrieveMat();
                }
                catch
                {
                    Console.WriteLine("Trying to get frame camera...");
                }


            }
        }
    }
}
