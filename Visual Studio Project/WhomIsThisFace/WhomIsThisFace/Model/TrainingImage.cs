using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhomIsThisFace.Model
{
    class TrainingImage
    {
        private List<Image<Gray, byte>> Image;

        private List<int> Label;

        private Hashtable mapToString;

        private Hashtable mapToInt;
        public Hashtable mapToCount;

        public TrainingImage()
        {
            resetTrainImg();
            
            try { loadTrainImg(); }
            catch { }

        }

        public void addTrainImg(Image<Gray, byte> image, int label)
        {
            Image.Add(image);
            Label.Add(label);
        }

        public void removeTrainImg(int index)
        {
            Image.RemoveAt(index);
            Label.RemoveAt(index);
        }
 
        public void resetTrainImg()
        {
            mapToString = new Hashtable();
            mapToInt = new Hashtable();
            mapToCount = new Hashtable();
            Label = new List<int>();
            Image = new List<Image<Gray, byte>>();
        }

        public void loadTrainImg()
        {

            try
            {
            //    string[] lines = System.IO.File.ReadAllLines(Gobal.SUBJECT_ID_PATH + "/List.csv");
                string[] lines = System.IO.File.ReadAllLines(Gobal.SUBJECT_ID_PATH + "/List.csv", System.Text.Encoding.GetEncoding(874));
                
                foreach (string line in lines)
                {
                    String tmpLine = line.Replace("\t", ",");
                    String[] s = tmpLine.Split(',');
                    for (int i = 0; i < s.Length; i++)
                        s[i] = s[i].Trim();

                    Student studentData = null;
                    try
                    {
                        studentData = new Student(s[0], s[1], s[2].Split(' ')[0], s[2].Split(' ')[1]);
                    }
                    catch { }
                    if (studentData == null) continue;
                    int myInt;
                    bool isNumerical = int.TryParse(studentData.ID, out myInt);
                    if (isNumerical)
                    {
                        mapToString.Add(Convert.ToInt32(s[0]), s[1]);
                        mapToInt.Add(s[1], Convert.ToInt32(s[0]));
                    }
                    

                    
                    
                }
            }
            catch { }
            int count = 1;

            String[] files = GetFiles(Gobal.TRAIN_PATH, "*.jpg", SearchOption.TopDirectoryOnly);
            foreach (String s in files)
            {

                FileInfo f = new FileInfo(s);
                String name = "";
                if (f.Name.Contains('-'))
                    name = f.Name.Split('.')[0].Split('-')[0];
                else name = f.Name.Split('.')[0];
                if (!mapToInt.Contains(name)) continue;

                String imagePath = Gobal.TRAIN_PATH + "/" + f.Name;
                //X1
                Image<Bgr, byte> originalImage = new Image<Bgr, byte>(imagePath);
             
                Image<Gray, Byte> gray = originalImage.Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                gray = Process.Detector.PreProcess(gray);

                int label = (int)mapToInt[name];



                mapToCount.Add(count, label);
                //mapToCount.Add(label, label);
                addTrainImg(gray, count++);
              //  addTrainImg(gray, label);
                
            }

        }
        public String[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            string[] searchPatterns = searchPattern.Split('|');
            List<string> files = new List<string>();
            foreach (string sp in searchPatterns)
                files.AddRange(System.IO.Directory.GetFiles(path, sp, searchOption));
            files.Sort();
            return files.ToArray();
        }

        public Boolean isEmpty()
        {
            return Label.Count == 0;
        }

        public Image<Gray, byte>[] ImageToArray()
        {
            return Image.ToArray();
        }
        public int[] LabelToArray()
        {
            return Label.ToArray();
        }

        public int Size()
        {
            return Label.Count;
        }
    }
}
