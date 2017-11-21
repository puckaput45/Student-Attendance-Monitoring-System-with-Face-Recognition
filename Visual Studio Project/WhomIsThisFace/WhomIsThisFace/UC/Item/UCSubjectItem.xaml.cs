using System;
using System.Collections.Generic;
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

namespace WhomIsThisFace.UC.Item
{
    /// <summary>
    /// Interaction logic for SubjectItem.xaml
    /// </summary>
    public partial class UCSubjectItem : UserControl
    {
        String subjectID;
        String year;
        String term;
        SelectClassWindow selectClassWindow;
        public UCSubjectItem(String subjectID, String year, String term, SelectClassWindow selectClassWindow)
        {
            InitializeComponent();
            this.subjectID = subjectID;
            this.year = year;
            this.term = term;
            this.selectClassWindow = selectClassWindow;

            tbIDSubject.Text = subjectID;
            
        }

        private void btnSubjectItem_Click(object sender, RoutedEventArgs e)
        {
            Gobal.SUBJECT_ID = subjectID;
            Gobal.SUBJECT_ID_PATH = Environment.CurrentDirectory + "/subject/" + Gobal.YEAR + "/" + Gobal.TERM + "/" + Gobal.SUBJECT_ID;
            Gobal.TRAIN_PATH = Gobal.SUBJECT_ID_PATH + "/train";
            Gobal.TEST_PATH = Gobal.SUBJECT_ID_PATH + "/test";
            Gobal.RESULT_PATH = Gobal.SUBJECT_ID_PATH + "/result";
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            selectClassWindow.Close();
            
        }
    }
}
