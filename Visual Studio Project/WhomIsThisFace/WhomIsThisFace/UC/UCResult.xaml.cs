using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
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
using WhomIsThisFace.UC.Item;

namespace WhomIsThisFace.UC
{
    /// <summary>
    /// Interaction logic for UCTestMethod.xaml
    /// </summary>
    public partial class UCResult : UserControl
    {
        private List<UCResultItem> items;
        public UCResult()
        {
            InitializeComponent();
            

            DirectoryInfo directory = new DirectoryInfo(Gobal.RESULT_PATH);
            DirectoryInfo[] dirs = directory.GetDirectories("*");

            foreach (DirectoryInfo dir in dirs) {
                cbResultDate.Items.Add(dir.Name);
                cbResultDate.SelectedItem = dir.Name;
            }

            loadData();
        }
        private void loadData()
        {
            items = new List<UCResultItem>();
            String currDatePath = Gobal.RESULT_PATH + "/" + cbResultDate.SelectedItem;
            DirectoryInfo directory = new DirectoryInfo(currDatePath);
            FileInfo[] files = directory.GetFiles("*.txt");
            StackPanelMain.Children.Clear();
            foreach (FileInfo f in files)
            {
                string[] lines = System.IO.File.ReadAllLines(currDatePath + "/" + f.Name);

                Image<Bgr, Byte> trainingImage = new Image<Bgr, Byte>(Gobal.TRAIN_PATH + "/" + f.Name.Split('.')[0] + "-0.jpg");
                Image<Bgr, Byte> resultImage = new Image<Bgr, Byte>(currDatePath + "/" + f.Name.Split('.')[0] + ".jpg");


                String text = lines[0];
                String[] split = text.Split('\t');
                //   resultImage._EqualizeHist(); 
                UCResultItem ucResultItem = new UCResultItem(trainingImage, resultImage, split[1] + " " + split[2] + " " + split[3]);

                StackPanelMain.Children.Add(ucResultItem);
            }



        }
        private void cbResultDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadData();
        }
        System.Diagnostics.Process process;
        private void btnOpenLocalSheet_Click(object sender, RoutedEventArgs e)
        {
            
            String filenameS = Gobal.SUBJECT_ID_PATH + "/List.csv";
            String filenameD = Gobal.SUBJECT_ID_PATH + "/List_TMP.csv";
            System.IO.File.Copy(filenameS, filenameD, true);
            process = System.Diagnostics.Process.Start(Gobal.SUBJECT_ID_PATH + "/List_TMP.csv");

            
    
        }

        private void btnSaveLocalSheet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String filenameS = Gobal.SUBJECT_ID_PATH + "/List.csv";
                String filenameD = Gobal.SUBJECT_ID_PATH + "/List_TMP.csv";

                string[] lines = System.IO.File.ReadAllLines(filenameD, System.Text.Encoding.GetEncoding(874));
                int numRow = lines.Length;
                if (File.Exists(filenameS))
                    File.Delete(filenameS);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filenameS, true, Encoding.UTF8))
                {

                    for (int i = 0; i < numRow; i++)
                    {
                        String line = lines[i].Replace("\t",",");
                        
                        file.WriteLine(line);
                    }
                }
               // System.IO.File.Copy(filenameD, filenameS, true);
            }
            catch { 
                messageDialog("Can't access file", "Please close file.");
            }
        }

        public void messageDialog(String title, String message)
        {
            string sMessageBoxText = message;
            string sCaption = title;

            MessageBoxButton btnMessageBox = MessageBoxButton.OK;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

        }
  
    }
}
