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
    /// Interaction logic for UCSelectClass.xaml
    /// </summary>
    public partial class UCSelectClass : UserControl
    {
        SelectClassWindow selectClassWindow;
        public UCSelectClass(SelectClassWindow selectClassWindow)
        {
            InitializeComponent();
            this.selectClassWindow = selectClassWindow;
            String currYear = DateTime.Now.Year.ToString();
            String pathYear = Gobal.SUBJECT_PATH + "/" + currYear;
            if (!Directory.Exists(pathYear))
            {
                Directory.CreateDirectory(pathYear);
                for (int i = 1; i < 3; i++)
                {
                    String term = i + "";
                    String pathTerm = Gobal.SUBJECT_PATH + "/" + currYear + "/" + term;
                    if (!Directory.Exists(pathTerm))
                    {
                        Directory.CreateDirectory(pathTerm);
                    }
                }
            }
            String[] dirYear = Directory.GetDirectories(Gobal.SUBJECT_PATH);
            for (int i = 0; i < dirYear.Length; i++)
            {
                dirYear[i] = dirYear[i].Remove(0, (Gobal.SUBJECT_PATH + "\\").Length);
                cbYear.Items.Add(dirYear[i]);

            }
            cbYear.Text = currYear;
            String[] dirTerm = Directory.GetDirectories(Gobal.SUBJECT_PATH + "/" + currYear);

            for (int j = 0; j < dirTerm.Length; j++)
            {
                String s = Gobal.SUBJECT_PATH + "/" + currYear + "\\";
                dirTerm[j] = dirTerm[j].Remove(0, s.Length);
                cbTerm.Items.Add(dirTerm[j]);
            }
            cbTerm.SelectedItem = "1";
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gobal.YEAR = (String)cbYear.SelectedItem;
            Gobal.TERM = (String)cbTerm.SelectedItem;
            
            String pathIDSubject = Gobal.SUBJECT_PATH + "/" + Gobal.YEAR + "/" + Gobal.TERM;
            String[] dirpathIDSubject = Directory.GetDirectories(pathIDSubject);

            
            UCSparate ucSparate = new UCSparate();
            StackPanelMain.Children.Clear();
            for (int j = 0; j < dirpathIDSubject.Length; j++)
            {
                String s = pathIDSubject + "\\";
                dirpathIDSubject[j] = dirpathIDSubject[j].Remove(0, s.Length);
                UCSubjectItem ucSubjectItem = new UCSubjectItem(dirpathIDSubject[j], Gobal.YEAR, Gobal.TERM, selectClassWindow);

                StackPanelMain.Children.Add(ucSubjectItem);

            }
        }
    }
}
