using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WhomIsThisFace.Display
{
    class Bitmap
    {
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap
                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                DeleteObject(ptr); //release the HBitmap
                return bs;
            }
        }

        public static BitmapSource ToBitmapSource(IImage image, List<DrawTextData> drawTextData)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {

                Graphics g = Graphics.FromImage(source);
                foreach (DrawTextData i in drawTextData)
                {
                    RectangleF rectf = new RectangleF(i.FaceFound.X - 5, i.FaceFound.Y - 20, 500, 50);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.DrawString(i.Message, new Font("Tahoma", 10), System.Drawing.Brushes.LightGreen, rectf);
                    g.Flush();
                }


                IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap
                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                DeleteObject(ptr); //release the HBitmap
                return bs;
            }
        }
        public static void SaveToJpeg(IImage image, String imagePath)
        {

            //Converst to jpg
            double quality = 100;
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(
                System.Drawing.Imaging.Encoder.Quality,
                (long)quality
                );

            var jpegCodec = (from codec in ImageCodecInfo.GetImageEncoders()
                             where codec.MimeType == "image/jpeg"
                             select codec).Single();
            image.Bitmap.Save(imagePath, jpegCodec, encoderParams);
        }

        public static Image<Gray, Byte> LBP(Image<Gray, Byte> image, int radius, int neighbors)
        {
            Image<Gray, Byte> result = new Image<Gray, byte>(image.Width, image.Height);
            result.SetZero();
            //result = new Image<Gray, byte>();//Mat::zeros(src.rows-2*radius, src.cols-2*radius, CV_32SC1);
            for (int n = 0; n < neighbors; n++)
            {
                // sample points
                double x = 1.0 * radius * Math.Cos(2.0 * Math.PI * n / neighbors);
                double y = 1.0 * radius * -Math.Sin(2.0 * Math.PI * n / neighbors);
                // relative indices
                int fx = (int)Math.Floor(x);
                int fy = (int)Math.Floor(y);
                int cx = (int)Math.Ceiling(x);
                int cy = (int)Math.Ceiling(y);
                // fractional part
                double ty = y - fy;
                double tx = x - fx;
                // set interpolation weights
                double w1 = (1 - tx) * (1 - ty);
                double w2 = tx * (1 - ty);
                double w3 = (1 - tx) * ty;
                double w4 = tx * ty;
                // iterate through your data
                for (int i = radius; i < image.Height - radius; i++)
                {
                    for (int j = radius; j < image.Width - radius; j++)
                    {

                        double t = w1 * image.Data[i + fy, j + fx, 0] + w2 * image.Data[i + fy, j + cx, 0] + w3 * image.Data[i + cy, j + fx, 0] + w4 * image.Data[i + cy, j + cx, 0];
                        // we are dealing with floating point precision, so add some little tolerance
                        int tmp = (((t > image.Data[i, j, 0]) && (Math.Abs(t - image.Data[i, j, 0]) > Double.Epsilon)) ? 1 : 0) << n;
                        if (tmp > 0)
                        {
                            if (n > 5)
                            {

                                int xxx = (byte)tmp;
                            }
                        }
                        result.Data[i - radius, j - radius, 0] += (byte)tmp;
                    }
                }
            }
            return result;



        }

    }
    public class DrawTextData
    {
        public System.Drawing.Rectangle FaceFound { get; set; }
        public String Message { get; set; }

        public DrawTextData(System.Drawing.Rectangle FaceFound, String Message)
        {
            this.FaceFound = FaceFound;
            this.Message = Message;
        }

        
    }



}
