using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhomIsThisFace.Display;

namespace WhomIsThisFace.Process
{
    class Detector
    {
        private static CascadeClassifier faceCascade = new CascadeClassifier("Cascades/haarcascade_frontalface_default.xml");
        private static CascadeClassifier eyesCascade = new CascadeClassifier("Cascades/haarcascade_mcs_eyepair_big.xml");
        private static CascadeClassifier mouthCascade = new CascadeClassifier("Cascades/mouth.xml");
        private static CascadeClassifier noseCascade = new CascadeClassifier("Cascades/nose.xml");

        public static Rectangle[] DetectFace(Image<Gray, Byte> gray)
        {

            List<System.Drawing.Rectangle> result = new List<Rectangle>();
            System.Drawing.Rectangle[] facesDetected = faceCascade.DetectMultiScale(gray, 1.2, 1, new System.Drawing.Size(0, 0), System.Drawing.Size.Empty);


            for (int i = 0; i < facesDetected.Length; i++)
            {
                Image<Gray, Byte> face = gray.Copy(facesDetected[i]);
                System.Drawing.Rectangle[] eyesDetected = eyesCascade.DetectMultiScale(face, 1.2, 5, new System.Drawing.Size(0, 0), System.Drawing.Size.Empty);
                System.Drawing.Rectangle[] mouthDetected = mouthCascade.DetectMultiScale(face, 1.2, 1, new System.Drawing.Size(50, 50), System.Drawing.Size.Empty);
                System.Drawing.Rectangle[] noseDetected = noseCascade.DetectMultiScale(face, 1.2, 5, new System.Drawing.Size(50, 50), System.Drawing.Size.Empty);
               
                if (eyesDetected.Length > 0 && noseDetected.Length > 0)
                {
                    for (int l = 0; l < eyesDetected.Length; l++)
                    {
                        for (int j = 0; j < noseDetected.Length; j++)
                        {
                            if (noseDetected[j].Y > eyesDetected[l].Y) {

                                result.Add(facesDetected[i]);
                                break;

                            }
                        }
                    }

                }
                
            

            }

            return result.ToArray();
            //System.Drawing.Rectangle[] facesDetected = faceCascade.DetectMultiScale(gray, 1.2, 10, new System.Drawing.Size(0, 0), System.Drawing.Size.Empty);

//            return facesDetected;
        }
        


        public static Image<Bgr, Byte> DrawFace(Image<Bgr, Byte> image, Rectangle[] facesDetected, Boolean found)
        {
            for (int i = 0; i < facesDetected.Length; i++)
            {
                if (!found)
                    image.Draw(facesDetected[i], new Bgr(System.Drawing.Color.Red), 2);
                else
                    image.Draw(facesDetected[i], new Bgr(System.Drawing.Color.LightGreen), 2);
            }

            return image;
        }
        public static Image<Gray, Byte> PreProcess(Image<Gray, Byte> image)
        {
            image._EqualizeHist();

            return image;
        }
        
    }
}
