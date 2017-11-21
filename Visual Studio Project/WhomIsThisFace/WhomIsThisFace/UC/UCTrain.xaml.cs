using DirectShowLib;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
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
using WhomIsThisFace.Model;
namespace WhomIsThisFace.UC
{
    /// <summary>
    /// Interaction logic for UCTrain.xaml
    /// </summary>
    public partial class UCTrain : UserControl
    {


        private String pathTrain = Gobal.SUBJECT_ID_PATH + "/train";

        private Capture capture;

        private Image<Bgr, byte> currentFrame;
        private System.Drawing.Rectangle[] faceDetected;
        private Image<Bgr, byte> currentFaceDisp;

        private Student currentStudent;
        private Hashtable studentDatas;
        // private List<StudentData> studentDatas;
        private Image<Bgr, Byte>[] faceDetectedImage;

        private DispatcherTimer timer;
        private VideoWriter vw;
        int indexImage;
        private int SIZE_ARRAY_IMAGE = 5;




        public UCTrain()
        {
            InitializeComponent();
            loadData();
        }
        
        private void UC_Loaded(object sender, RoutedEventArgs e)
        {

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



        }

     
        private void UC_Unloaded(object sender, EventArgs e)
        {
            if (capture != null)
            {
                timer.Tick -= new EventHandler(timer_Tick);
                capture.Dispose();

            }
        }

        private void loadData()
        {
            // BtnNext.IsEnabled = false;
            BtnPrev.IsEnabled = false;
            indexImage = 0;
            studentDatas = new Hashtable();
            string[] lines = System.IO.File.ReadAllLines(Gobal.SUBJECT_ID_PATH + "/List.csv",System.Text.Encoding.GetEncoding(874));
        //    f = new StreamReader(txtKUFile.Text, System.Text.Encoding.GetEncoding(874))ว

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
                try
                {
            
                    String[] files = GetFiles(Gobal.TRAIN_PATH, s[1]+"*.jpg", SearchOption.TopDirectoryOnly);
                  
                    studentData.Image = new Image<Bgr, byte>[SIZE_ARRAY_IMAGE];
                    foreach(String file in files){
                        FileInfo finfo = new FileInfo(file);
                        int idx = Int32.Parse(finfo.Name.Split('.')[0].Split('-')[1]);
                        if (idx<SIZE_ARRAY_IMAGE)
                            studentData.Image[idx] = new Image<Bgr, byte>(file);
                    }
               
                }
                catch { }


                int myInt;
                bool isNumerical = int.TryParse(studentData.ID, out myInt);
                if (isNumerical) { 
                    String key = studentData.ID + ";" + studentData.StudentID + ";" + studentData.Name + ";" + studentData.LastName;
                    studentDatas.Add(key, studentData);
                    ListViewData.Items.Add(key);
                }
            }

        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                currentFrame = capture.QueryFrame();
                faceDetected = Process.Detector.DetectFace(currentFrame.Convert<Gray, byte>());
            /*    if (!BtnRecord.Content.Equals("Record"))
                {



                    //  vw.WriteFrame(currentFrame);

                    BtnRecord.Content = "Recording " + currentStudent.NumFrame + " Frames";

                    foreach (System.Drawing.Rectangle rect in faceDetected)
                    {
                        try
                        {

                            Image<Bgr, byte> test = currentFrame.Copy(rect).Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                            String imagePath = pathTrain + "/" + currentStudent.StudentID + "-" + currentStudent.NumFrame + ".jpg";
                            vw.WriteFrame(test);
                            currentStudent.NumFrame++;
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
                            test.Bitmap.Save(imagePath, jpegCodec, encoderParams);
                        }
                        catch { }
                    }


                }
             
             */
                DispFrame.Source = Display.Bitmap.ToBitmapSource(Process.Detector.DrawFace(currentFrame.Clone(), faceDetected,false));
            }
            catch { }

        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            
            if (indexImage <= SIZE_ARRAY_IMAGE - 1)
            {
                BtnNext.IsEnabled = true;
            }
            if (indexImage > 0)
            {
                
                indexImage--;
                BtnRecord.Content = "Set Image " + (indexImage + 1);
                if (indexImage == 0)
                    BtnPrev.IsEnabled = false;
                try { DispFaceListStudent.Source = Display.Bitmap.ToBitmapSource(currentStudent.Image[indexImage]); }
                catch { DispFaceListStudent.Source = Display.Bitmap.ToBitmapSource(new Image<Bgr, byte>(480, 320, new Bgr(245, 245, 245))); }
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            
            if (indexImage >= 0)
            {
                BtnPrev.IsEnabled = true;
            }
            if (indexImage < SIZE_ARRAY_IMAGE - 1)
            {
                
                indexImage++;
                BtnRecord.Content = "Set Image " + (indexImage+1);
                if (indexImage == SIZE_ARRAY_IMAGE - 1)
                    BtnNext.IsEnabled = false;
                try { DispFaceListStudent.Source = Display.Bitmap.ToBitmapSource(currentStudent.Image[indexImage]); }
                catch { DispFaceListStudent.Source = Display.Bitmap.ToBitmapSource(new Image<Bgr, byte>(480, 320, new Bgr(245, 245, 245))); }
            }
        }

        private void ListViewData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selected = (String)ListViewData.SelectedValue + "";


            currentStudent = (Student)studentDatas[selected];
          /*  if (currentStudent.Image != null)
                SIZE_ARRAY_IMAGE = currentStudent.Image.Count();*/
            /* if (currentStudent.Image == null)
             {
                 currentStudent.Image = new Image<Bgr, Byte>[SIZE_ARRAY_IMAGE];
             }*/

            try { DispFaceListStudent.Source = Display.Bitmap.ToBitmapSource(currentStudent.Image[indexImage]); }
            catch { DispFaceListStudent.Source = Display.Bitmap.ToBitmapSource(new Image<Bgr, byte>(480, 320, new Bgr(245, 245, 245))); }
        }

        private void BtnSetImageSlotN_Click(object sender, RoutedEventArgs e)
        {
            timer.Tick -= new EventHandler(timer_Tick);
            try
            {

                faceDetectedImage = new Image<Bgr, byte>[faceDetected.Count()];
                for (int i = 0; i < faceDetected.Count(); i++)
                {

                    faceDetectedImage[i] = currentFrame.Copy(faceDetected[i]).Convert<Bgr, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                }
                currentFaceDisp = faceDetectedImage[0].Clone();

                try
                {

                    String imagePath = pathTrain + "/" + currentStudent.StudentID + "-" + indexImage + ".jpg";
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
                    currentFaceDisp.Bitmap.Save(imagePath, jpegCodec, encoderParams);
                }
                catch { }

                currentStudent.Image[indexImage] = currentFaceDisp.Clone();

                DispFaceListStudent.Source = Display.Bitmap.ToBitmapSource(currentStudent.Image[indexImage]);
            }
            catch
            {
                DispFaceListStudent.Source = Display.Bitmap.ToBitmapSource(new Image<Bgr, byte>(480, 320, new Bgr(245, 245, 245)));
            }
            timer.Tick += new EventHandler(timer_Tick);
        }


/*
        private void BtnRecord_Click(object sender, RoutedEventArgs e)
        {
            indexImage = 1;
            BtnPrev_Click(null, null);
            if (BtnRecord.Content.Equals("Record"))
            {
                try
                {
                    String selected = (String)ListViewData.SelectedValue + "";

                    currentStudent = (Student)studentDatas[selected];

                    String filename = pathTrain + "/" + currentStudent.StudentID + ".avi";
                    if (File.Exists(filename))
                    {
                        string sMessageBoxText = "Do you want to Overide file right?";
                        string sCaption = "Ohhh user";

                        MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                        MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                        MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                        switch (rsltMessageBox)
                        {
                            case MessageBoxResult.Yes:
                                File.Delete(filename);
                                vw = new VideoWriter(filename, 30, 100, 100, true);
                                currentStudent.NumFrame = 0;

                                BtnRecord.Content = "Recording " + currentStudent.StudentID;
                                break;

                        }


                    }
                    else
                    {
                        vw = new VideoWriter(filename, 30, 100, 100, true);
                        currentStudent.NumFrame = 0;

                        BtnRecord.Content = "Recording " + currentStudent.StudentID;
                    }
                    //   vw = new VideoWriter(filename, 30, capture.Width, capture.Height, true);

                }
                catch
                {

                }

            }
            else
            {

                vw.Dispose();
                String path = pathTrain + "/" + currentStudent.StudentID + ".avi";
                if (File.Exists(path))
                {
                    Capture frame = new Capture(path);
                    List<Image<Bgr, Byte>> listFrame = new List<Image<Bgr, byte>>();
                    while (true)
                    {
                        Image<Bgr, Byte> tmp = frame.QueryFrame();
                        if (tmp == null) break;
                        listFrame.Add(tmp.Clone());
                    }
                    currentStudent.Image = listFrame.ToArray();
                    SIZE_ARRAY_IMAGE = listFrame.Count();


                }

                BtnRecord.Content = "Record";
            }
        }*/
        private void cbCamera_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (capture != null)
                capture.Dispose();
            if (((String)cbCamera.SelectedItem).Equals("load .avi"))
            {

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
                    catch
                    {

                    }
                }




            }
            else
            {
                capture = new Capture(Convert.ToInt32((String)cbCamera.SelectedItem));
                capture.FlipHorizontal = true;

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
    }
}
