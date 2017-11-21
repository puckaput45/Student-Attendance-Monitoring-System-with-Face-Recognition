using DirectShowLib;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using GoogleDriveV3.Helpers;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WhomIsThisFace.Display;
using WhomIsThisFace.Model;
using WhomIsThisFace.Process;
using WhomIsThisFace.UC.Item;

namespace WhomIsThisFace.UC
{
    /// <summary>
    /// Interaction logic for UCTest.xaml
    /// </summary>
    public partial class UCTest : UserControl
    {

        private Hashtable studentDatas;
        private Hashtable mapStudentData;

        
        private DispatcherTimer timer;
        private System.Drawing.Rectangle[] faceDetected;
        private Capture capture;
        private Found[] found;
        private String[,] check;
        private Image<Bgr, byte> currentFrame;
        Recognizer recognizer;
        
        private int currCol;
        private int numRow = 0;

        private String currDate = DateTime.Now.ToString("yyyy/MM/dd");
        private String currDatePath = Gobal.RESULT_PATH + "/" + DateTime.Now.ToString("yyyy_MM_dd") + "/";

    //    private String currDate = "22/22/22";
      //  private String currDatePath = Gobal.RESULT_PATH + "/" + "22_22_22" + "/";

        private DispatcherTimer countdown;
        public UCTest()
        {
            InitializeComponent();
            loadData();
            loadDataList();
           
            

        }
        Hashtable mapImage;
        private void loadDataList()
        {
            mapImage = new Hashtable();  
            DirectoryInfo directory = new DirectoryInfo(Gobal.TRAIN_PATH);
            FileInfo[] files = directory.GetFiles("*0.jpg");
            foreach (FileInfo f in files)
            {
                String sid = f.Name.Split('.')[0].Split('-')[0];

                Image<Gray, Byte> trainingImage = new Image<Gray, Byte>(f.FullName).Convert<Gray, Byte>();

                trainingImage = Process.Detector.PreProcess(trainingImage);

                UCResultItem ucResultItem = new UCResultItem(trainingImage, f.Name.Split('.')[0]);
                mapImage.Add(sid, ucResultItem);
                StackPanelMain.Children.Add(ucResultItem);
            }

            for (int i = 0; i < found.Length; i++)
            {
                if (found[i] != null)
                    if (found[i].Distance < double.MaxValue)
                    {
                        Student s = (Student)mapStudentData[i + ""];
                        UCResultItem item = (UCResultItem)mapImage[s.StudentID];
                        item.RPic.Source = Display.Bitmap.ToBitmapSource(found[i].Image);
                        item.Name.Content = found[i].Distance ;

                    }


            }
        }
        private void loadData()
        {
            // BtnNext.IsEnabled = false;
            studentDatas = new Hashtable();
            mapStudentData = new Hashtable();
            string[] lines = System.IO.File.ReadAllLines(Gobal.SUBJECT_ID_PATH + "/List.csv", System.Text.Encoding.GetEncoding(874));
            numRow = lines.Length;
            String[] lastCol = lines[0].Split(',');
            if(lastCol[lastCol.Length - 1].Equals("")){
                currCol = lastCol.Length - 1;
                check = new String[lines.Length, currCol + 1];
                check[0, currCol] = currDate;
               // Util.UpdateCells(MainWindow.SheetService, MainWindow.sheetsID, MainWindow.Gid, 0, currCol, currDate);
                for (int i = 1; i < lines.Length; i++)
                {
                    check[i, currCol] = "0";
                }
                for (int i = 0; i < lines.Length; i++)
                {
                    String tmpLine = lines[i].Replace("\t", ",");
                    String[] s = tmpLine.Split(',');
                    for (int j = 0; j < s.Length; j++)
                    {
                        if (s[j]!=null&&!s[j].Equals(""))
                            check[i, j] = s[j].Trim();
                    }
                }
            }
            else { 
                if (!lastCol[lastCol.Length - 1].Equals(currDate))
                {
                    currCol = lastCol.Length;
                    check = new String[lines.Length,currCol+1];
                
                    check[0,currCol] = currDate;
                 //   Util.UpdateCells(MainWindow.SheetService, MainWindow.sheetsID, MainWindow.Gid, 0, currCol, currDate);
                    for (int i = 1; i < lines.Length; i++) {
                        check[i, currCol] = "0"; 
                    }
                }
                else {
                    currCol = lastCol.Length-1;
                    check = new String[lines.Length, currCol+1];
               
                }
                for (int i = 0; i < lines.Length; i++)
                {
                    String[] s = lines[i].Split(',');
                    for (int j = 0; j < s.Length; j++)
                    {
                        check[i, j] = s[j].Trim();
                    }
                
                }
            }
            //data = new String[lines.Length][];
            
            foreach (string line in lines)
            {
                String tmp = line.Replace("\t", ",");
                String[] s = tmp.Split(',');
                for (int i = 0; i < s.Length; i++)
                    s[i] = s[i].Trim();
                Student studentData = null;
                try
                {
                    studentData = new Student(s[0], s[1], s[2].Split(' ')[0], s[2].Split(' ')[1]);
                }
                catch { }
                if (studentData == null) continue;
                String key = studentData.ID + ";" + studentData.StudentID + ";" + studentData.Name + ";" + studentData.LastName;
                studentDatas.Add(studentData.ID, key);
                mapStudentData.Add(studentData.ID, studentData);
            }

            found = new Found[studentDatas.Count + 1];
            for (int i = 0; i < found.Length; i++)
            {
                found[i] = new Found() ;
            }
            DirectoryInfo directory = new DirectoryInfo(currDatePath);

            if (!Directory.Exists(currDatePath))
            {
                Directory.CreateDirectory(currDatePath);
            }
            FileInfo[] files = directory.GetFiles("*.txt");
            foreach(FileInfo file in files){
                string[] line = System.IO.File.ReadAllLines(file.FullName);
                String[] split = line[0].Split('\t');
                found[Int32.Parse(split[0])] = new Found(float.Parse(split[4], CultureInfo.InvariantCulture.NumberFormat), new Image<Gray, byte>(currDatePath + "" + split[1]+".jpg"));
            }
            
        }


        TrainingImage trainingImage;

        private void UC_Loaded(object sender, RoutedEventArgs e)
        {

            
            try
            {
                trainingImage = new TrainingImage();

                recognizer = new Recognizer(trainingImage);
                labelStatus.Visibility = Visibility.Hidden;
                DsDevice[] captureDevices;

                // Get the set of directshow devices that are video inputs.
                captureDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

                for (int idx = 0; idx < captureDevices.Length; idx++)
                {
                    cbCamera.Items.Add(idx + "");
                    cbCamera.SelectedItem = idx + "";
                }
                cbCamera.Items.Add("load .avi");

                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(150000); // 150000 x 100 ns  = 1.5 E-4 s 15 ms
                //timer.Interval = 15;
                timer.Tick += new EventHandler(timer_Tick);
                //timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                timer.Start();

                countdown = new DispatcherTimer();
                countdown.Tick += countdown_Trick;
                countdown.Interval = TimeSpan.FromSeconds(2);
                countdown.Start();
           
            }
            catch { }

            

            

        }

        private void UC_Unloaded(object sender, EventArgs e)
        {
            if(timer!=null)
                timer.Tick -= new EventHandler(timer_Tick);
            if (countdown!=null)
                countdown.Tick -= countdown_Trick;
            if (capture != null)
            {
                
                capture.Dispose();

            }
        }

        List<FaceObject>[] faceObjects = new List<FaceObject>[2];

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                currentFrame = capture.QueryFrame();

                Image<Gray, Byte> gray = currentFrame.Convert<Gray, byte>();

                faceDetected = Process.Detector.DetectFace(gray);
                double[] distance = new double[found.Length + 1];

                List<DrawTextData> faceFound = new List<DrawTextData>();
                faceObjects[0] = new List<FaceObject>();

                for (int i = 0; i < faceDetected.Length; i++)
                {

                    Image<Gray, Byte> face = gray.Copy(faceDetected[i]).Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    face = Process.Detector.PreProcess(face);

                    FaceRecognizer.PredictionResult tmp = recognizer.Who(face);
                    FaceRecognizer.PredictionResult tmp2 = recognizer.Who(face.Copy().Flip(FLIP.HORIZONTAL).Convert<Gray, Byte>());
                    FaceRecognizer.PredictionResult who = tmp.Distance < tmp2.Distance ? tmp : tmp2;

                    if (who.Label > -1)
                    {
                        FaceObject facObj = new FaceObject(faceDetected[i], face, who.Distance, who.Label, CurrentMillis.Millis + "");
                        facObj.Label = who.Label;
                        facObj.Distance = who.Distance;
                        facObj.Rect = faceDetected[i];
                        facObj.DateTime = CurrentMillis.Millis + "";
                        faceObjects[0].Add(facObj);


                    }


                }

                if (faceObjects[1] != null)
                {

                    foreach (FaceObject fobj1 in faceObjects[1])
                    {
                        int idxMin = -1;
                        double d2pMin = Double.MaxValue;
                        for (int i = 0; i < faceObjects[0].Count; i++)
                        {

                            double d2p = Math.Sqrt((fobj1.Rect.X - faceObjects[0][i].Rect.X) * (fobj1.Rect.X - faceObjects[0][i].Rect.X) + (fobj1.Rect.Y - faceObjects[0][i].Rect.Y) * (fobj1.Rect.Y - faceObjects[0][i].Rect.Y));
                            if (d2p < d2pMin) { d2pMin = d2p; idxMin = i; }

                        }
                        Student s = (Student)mapStudentData[fobj1.Label + ""];
                        String text = s.ID + " " + s.StudentID + " " + s.Name + " " + s.LastName;


                        if (d2pMin > 50f)
                        {

                            if (found[fobj1.Label] != null)
                            {
                                if (found[fobj1.Label].Distance > fobj1.Distance)
                                {

                                    found[fobj1.Label] = new Found(fobj1.Distance, fobj1.Image.Clone());
                                    Image<Gray, byte> test = found[fobj1.Label].Image.Clone();

                                    String imagePath = currDatePath + s.StudentID + ".jpg";
                                    Display.Bitmap.SaveToJpeg(found[fobj1.Label].Image.Clone().Convert<Gray, Byte>(), imagePath);

                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(currDatePath + s.StudentID + ".txt"))
                                    {
                                        String textOut = s.ID + "\t" + s.StudentID + "\t" + s.Name + "\t" + s.LastName + "\t" + fobj1.Distance;

                                        file.WriteLine(textOut);
                                    }

                                    
                                }
                            }
                        }
                        else
                        {

                            if (faceObjects[0][idxMin].Distance > fobj1.Distance)
                            {
                                faceObjects[0][idxMin].Distance = fobj1.Distance;
                                faceObjects[0][idxMin].Label = fobj1.Label;
                                faceObjects[0][idxMin].Image = fobj1.Image.Clone();
                            }
                            faceObjects[0][idxMin].DateTime = fobj1.DateTime;
                        }
                        distance[fobj1.Label] = fobj1.Distance;
                        faceFound.Add(new DrawTextData(fobj1.Rect, text + " " + fobj1.Distance));
                    }
                }

                // StackPanelMain.Children.Clear();
                for (int i = 0; i < found.Length; i++)
                {
                    if (found[i] != null)
                        if (found[i].Distance < double.MaxValue)
                        {
                            Student s = (Student)mapStudentData[i + ""];
                            UCResultItem item = (UCResultItem)mapImage[s.StudentID];
                            item.RPic.Source = Display.Bitmap.ToBitmapSource(found[i].Image);
                            item.Name.Content = found[i].Distance + " " + distance[i];
                            mark(i);
                            //StackPanelMain.Children[2]
                            // StackPanelMain.Children.Add(item);


                        }


                }
                faceObjects[1] = new List<FaceObject>(faceObjects[0]);

                DispFrame.Source = Display.Bitmap.ToBitmapSource(Process.Detector.DrawFace(currentFrame.Clone(), faceDetected, (faceFound.Count > 0)), faceFound);
            }
            catch { 
            
            }

        }
        /*
            List<FaceObject> faceObjects = new List<FaceObject>();
            void timer_Tick(object sender, EventArgs e)
            {
                try
                {
                    currentFrame = capture.QueryFrame();
  
                    Image<Gray, Byte> gray = currentFrame.Convert<Gray, byte>();

                    faceDetected = Process.Detector.DetectFace(gray);
                    if (faceDetected.Length<=0)
                    {
                        DispFrame.Source = Display.Bitmap.ToBitmapSource(currentFrame.Clone());
                        return;
                    }
                    double[] distance = new double[found.Length + 1];

                    List<DrawTextData> faceFound = new List<DrawTextData>();
                    faceObjects = new List<FaceObject>();

                    for (int i = 0; i < faceDetected.Length; i++)
                    {
                    
                        Image<Gray,Byte> face = gray.Copy(faceDetected[i]).Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                        face = Process.Detector.PreProcess(face);
                
                        FaceRecognizer.PredictionResult tmp = recognizer.Who(face);
                        FaceRecognizer.PredictionResult tmp2 = recognizer.Who(face.Copy().Flip(FLIP.HORIZONTAL).Convert<Gray, Byte>());
                        FaceRecognizer.PredictionResult who = tmp.Distance < tmp2.Distance ? tmp : tmp2;
                    
                        if (who.Label > -1) {
                            FaceObject facObj = new FaceObject(faceDetected[i], face, who.Distance, who.Label, CurrentMillis.Millis + "");
                            facObj.Label = who.Label;
                            facObj.Distance = who.Distance;
                            facObj.Rect = faceDetected[i];
                            facObj.DateTime = CurrentMillis.Millis+"";
                            faceObjects.Add(facObj);
                        
                        }

                    
                    }

               
                        foreach (FaceObject fobj1 in faceObjects)
                        {
     
                            Student s = (Student)mapStudentData[fobj1.Label + ""];
                            String text = s.ID + " " + s.StudentID + " " + s.Name + " " + s.LastName;

                                if (found[fobj1.Label] != null) { 
                                    if (found[fobj1.Label].Distance > fobj1.Distance)
                                    {

                                        found[fobj1.Label] = new Found(fobj1.Distance, fobj1.Image.Clone());
                                        Image<Gray, byte> test = found[fobj1.Label].Image.Clone();


                                        String imagePath = currDatePath + s.StudentID + ".jpg";
                                        Display.Bitmap.SaveToJpeg(found[fobj1.Label].Image.Clone().Convert<Gray, Byte>(), imagePath);

                                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(currDatePath + s.StudentID + ".txt"))
                                        {
                                            String textOut = s.ID + "\t" + s.StudentID + "\t" + s.Name + "\t" + s.LastName + "\t" + fobj1.Distance;

                                            file.WriteLine(textOut);
                                        }


                                    }
                                }

                            distance[fobj1.Label] = fobj1.Distance;
                            faceFound.Add(new DrawTextData(fobj1.Rect, text + " " + fobj1.Distance));


                        

                        }
                


                    for (int i = 0; i < found.Length; i++)
                    {
                        if (found[i]!=null)
                            if (found[i].Distance < double.MaxValue)
                            {
                                Student s = (Student)mapStudentData[i + ""];
                                UCResultItem item = (UCResultItem)mapImage[s.StudentID];
                                item.RPic.Source = Display.Bitmap.ToBitmapSource(found[i].Image);
                                item.Name.Content =found[i].Distance + " " + distance[i];

                            }
                        
                
                    }


                    DispFrame.Source = Display.Bitmap.ToBitmapSource(Process.Detector.DrawFace(currentFrame.Clone(), faceDetected, (faceFound.Count>0)), faceFound);
                }
                catch { }

            }
            */

        class Found {
            public double Distance {get; set;}
            public Image<Gray, byte> Image{get; set;}

            public Found() {
                Distance = double.MaxValue;
            }
            public Found(double Distance, Image<Gray, byte> Image)
            {
                this.Distance = Distance;
                this.Image = Image;
            }
        }

         public class FaceObject
         {
             public System.Drawing.Rectangle Rect { get; set; }
             public String DateTime { get; set; }
             public Image<Gray, byte> Image { get; set; }
             
             public int Label { get; set; }
             
             public double Distance { get; set; }
             public double DistanceOriginal { get; set; }
             public Image<Gray, byte> ImageOriginal { get; set; }
             public int LabelOriginal { get; set; }
             public FaceObject(Image<Gray, byte> Image)
             {
             }
             public FaceObject(System.Drawing.Rectangle Rect, Image<Gray, byte> Image, double Distance, int Label, String DateTime)
             {
                 this.Rect = Rect;
                 this.Distance = Distance;
                 this.Image = Image;
                 this.DateTime = DateTime;
                 this.Label = Label;
                 this.ImageOriginal = Image.Clone();
                 this.DistanceOriginal = Distance;
                 this.LabelOriginal = Label;
             }
         }
         static class CurrentMillis
         {
             private static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
             /// <summary>Get extra long current timestamp</summary>
             public static long Millis { get { return (long)((DateTime.UtcNow - Jan1St1970).TotalMilliseconds); } }
         }

         private void cbCamera_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             if (capture != null)
                 capture.Dispose();
             if (((String)cbCamera.SelectedItem).Equals("load .avi")) {

                 OpenFileDialog openFileDialog1 = new OpenFileDialog();

                 // Set filter options and filter index.
                 openFileDialog1.Filter = "AVI Files (.avi)|*.avi";
                 openFileDialog1.FilterIndex = 1;

                 openFileDialog1.Multiselect = true;

                 // Call the ShowDialog method to show the dialog box.
                 bool? userClickedOK = openFileDialog1.ShowDialog();

                 // Process input if the user clicked OK.
                 if (userClickedOK == true)
                 {
                     try
                     {
                         capture = new Capture(openFileDialog1.FileName);
                     }
                     catch { 
                     
                     }
                 }




             }
             else { 
                 capture = new Capture(Convert.ToInt32((String)cbCamera.SelectedItem));
                 capture.FlipHorizontal=true;
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

         public void mark(int label) {
             check[label,currCol] = "1";
             //Util.UpdateCells(MainWindow.Service, MainWindow.sheetsID, 1, 1, "มา");
              
             
         }


         void countdown_Trick(object sender, EventArgs e)
         {
             String ListTmp = Gobal.SUBJECT_ID_PATH + "/List.csv";
             if (File.Exists(ListTmp))
                 File.Delete(ListTmp);


             String[] rowUpdate = new String[numRow];
             using (System.IO.StreamWriter file = new System.IO.StreamWriter(ListTmp, true, Encoding.UTF8))
             {
                 
                 for (int i = 0; i < numRow; i++)
                 {
                     String line = check[i,0];
                     for (int j = 1; j <= currCol; j++)
                     {
                         line += "," + check[i, j];
                     }
                     file.WriteLine(line);
                     rowUpdate[i] = check[i, currCol];
                 }
             }
             Thread t = new Thread(
                o =>
                {
                    try
                    {
                        Util.UpdateCells(MainWindow.SheetService, MainWindow.sheetsID, MainWindow.Gid, 0, currCol, rowUpdate);
                    }
                    catch { 
                    
                    }
                });
             t.Start();
         }

    }
}
