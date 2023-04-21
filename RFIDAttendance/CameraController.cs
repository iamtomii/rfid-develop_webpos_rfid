using OpenCvSharp;
using OpenCvSharp.Extensions;
using RFIDAttendance.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFIDAttendance
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
        private bool captureInProgress = false;
        public void StreamAndSaveVideo(string outputFilename, int fps)
        {
            // Mở kết nối tới camera
            //StreamVideo1();
            //capture = new VideoCapture(GlobalVariables.url_camera);
            //capture.Open(GlobalVariables.url_camera);
            // Tạo đối tượng VideoWriter để ghi video
            var writer = new VideoWriter(outputFilename, FourCC.XVID, fps, new OpenCvSharp.Size(GlobalVariables.sizevideo_width, GlobalVariables.sizevideo_height));

            // Bắt đầu stream video và lưu lại từng khung hình vào file video
            Mat frame = new Mat();
            while (capture.IsOpened())
            {
                capture.Read(frame);
                if (frame.Empty())
                {
                    break;
                }
                Mat resizedFrame = new Mat();
                Cv2.Resize(frame, resizedFrame, new OpenCvSharp.Size(GlobalVariables.sizevideo_width, GlobalVariables.sizevideo_height));
                string text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Cv2.PutText(resizedFrame, text, new OpenCvSharp.Point(10, 50), HersheyFonts.HersheyScriptSimplex, 0.2, Scalar.White, 1, LineTypes.AntiAlias, false);
                Program.mainForm.gifusermanual.Image = resizedFrame.ToBitmap();
                writer.Write(resizedFrame);
            }

            // Giải phóng tài nguyên
            writer.Release();
            capture.Release();
        }




        public void StreamVideo1()
        {
            // Set the captureInProgress flag to true
            captureInProgress = true;

            // Create a window to display the camera stream
            //Cv2.NamedWindow("Camera", WindowMode.Normal);

            // Start the capture loop in a separate thread
            Task.Run(() => {
                while (captureInProgress)
                {
                    // Read a frame from the camera
                    try
                    {
                        using (Mat frame = new Mat())
                        {
                            capture.Read(frame);

                            // Check if the frame is empty
                            if (frame.Empty())
                            {
                                // If the frame is empty, break out of the loop
                                break;
                            }

                            // Display the frame in the 'Camera' window

                            //Cv2.ImShow("Camera", frame);
                            Cv2.WaitKey(1);
                        }
                    } catch(Exception ex) {MainForm.WriteLogE(ex); }

                }
            });
        }
    public void StopStream()
    {
        // Set the captureInProgress flag to false to stop the capture loop
        captureInProgress = false;

        // Release the VideoCapture object
        if (capture != null)
        {
            capture.Release();
            capture.Dispose();
            capture = null;
        }

        // Close the 'Camera' window
        Cv2.DestroyWindow("Camera");
    }

        public string GetImage()
        {
            Mat currentFrame = new Mat();
            string base64String;
            currentFrame = capture.RetrieveMat();

            if (!currentFrame.Empty())
            {
                Bitmap saveImage = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(currentFrame);

                System.IO.MemoryStream stream = new MemoryStream();
                saveImage.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);

                byte[] imageBytes = stream.ToArray();
                base64String = Convert.ToBase64String(imageBytes);
            }
            else
            {

                Image saveImageLoad = Image.FromFile(GlobalVariables.path_ImageError);
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
            if (!currentFrame.Empty())
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
            if (inputString.Equals("False"))
            {
                return image;
            }

            byte[] bytes = Convert.FromBase64String(inputString);
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                image = Image.FromStream(ms, true, true);
                return image;
            }
        }
        public String VideoToBase64(string pathvideo)
        {
            try
            {
                byte[] videobytes = File.ReadAllBytes(pathvideo);
                string base64String = Convert.ToBase64String(videobytes);

                return base64String;
            }
            catch(Exception e)
            {
                Console.WriteLine("Message: "+e);
                return null;
            }
        }
        
        public void Base64toVideo(String base64String, string file_name_out = "output.mp4")
        {
            try
            {
                byte[] decodeBytes = Convert.FromBase64String(base64String);
                File.WriteAllBytes(file_name_out, decodeBytes);
            }
            catch(Exception e)
            {
                Console.WriteLine("Decode: " + e);
            }
        }
    }
}
