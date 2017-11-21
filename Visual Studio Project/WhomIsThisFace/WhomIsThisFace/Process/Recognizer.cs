using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhomIsThisFace.Model;

namespace WhomIsThisFace.Process
{
    class Recognizer
    {
        private TrainingImage trainingImage;

        private FaceRecognizer recognizerEMGUCV;


        public Recognizer(TrainingImage trainingImage)
        {
            this.trainingImage = trainingImage;
            //  try
            //  {
            Train();
            //   }
            //  catch { };
        }


        public FaceRecognizer.PredictionResult Who(Image<Gray, Byte> face)
        {
            FaceRecognizer.PredictionResult result = recognizerEMGUCV.Predict(face);
            if (result.Label!=-1)
            result.Label =(int)trainingImage.mapToCount[result.Label];
            return result;

        }


        private void Train()
        {

           //  recognizerEMGUCV = new LBPHFaceRecognizer(1, 4, 8, 8, 50);
            recognizerEMGUCV = new LBPHFaceRecognizer(1, 8, 8, 8, 65);
       //     recognizerEMGUCV = new FisherFaceRecognizer(0, 3500);
      //    recognizerEMGUCV = new EigenFaceRecognizer(80, double.PositiveInfinity);
            recognizerEMGUCV.Train(trainingImage.ImageToArray(), trainingImage.LabelToArray());
        }

        public Boolean isEmpty()
        {
            return trainingImage.isEmpty();
        }
    }
}
