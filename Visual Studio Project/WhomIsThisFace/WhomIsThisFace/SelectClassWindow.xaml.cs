using MahApps.Metro.Controls;
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
using System.Windows.Shapes;
using WhomIsThisFace.UC;

namespace WhomIsThisFace
{
    /// <summary>
    /// Interaction logic for SelectClassWindow.xaml
    /// </summary>
    public partial class SelectClassWindow : MetroWindow
    {
        UCNewSubject ucNewSubject;
        public SelectClassWindow()
        {
            InitializeComponent();
            UCSelectClass ucSelectClass = new UCSelectClass(this);
            StackPanelMain.Children.Add(ucSelectClass);
            btnADDSubject.Visibility = Visibility.Collapsed;
            btnCancelSubject.Visibility = Visibility.Collapsed;
        }

        private void btnNewSubject_Click(object sender, RoutedEventArgs e)
        {
            ucNewSubject = new UCNewSubject();
            StackPanelMain.Children.Clear();
            StackPanelMain.Children.Add(ucNewSubject);

            btnADDSubject.Visibility = Visibility.Visible;
            btnCancelSubject.Visibility = Visibility.Visible;

            btnNewSubject.Visibility = Visibility.Collapsed;
        }

        private void btnADDSubject_Click(object sender, RoutedEventArgs e)
        {
            btnADDSubject.Visibility = Visibility.Collapsed;
            btnCancelSubject.Visibility = Visibility.Collapsed;
            btnNewSubject.Visibility = Visibility.Visible;
           

            String currYear = DateTime.Now.Year.ToString();
            String pathYear = Gobal.SUBJECT_PATH + "/" + currYear;
            String term = (String)ucNewSubject.cbTerm.SelectedItem;
            String subjectID = ucNewSubject.tbSubjectID.Text;
            if (subjectID.Length > 0&&File.Exists(ucNewSubject.tbFileName.Text)) {
                String pathIDSubject = Gobal.SUBJECT_PATH + "/" + currYear + "/" + term + "/" + subjectID;
                
                if (!Directory.Exists(pathIDSubject))
                {
                    Directory.CreateDirectory(pathIDSubject);
                    Directory.CreateDirectory(pathIDSubject + "/train");
                    Directory.CreateDirectory(pathIDSubject + "/result");
                }
                String[] split = ucNewSubject.tbFileName.Text.Split('.');
                String fileName = split[split.Length - 1];
                File.Copy(ucNewSubject.tbFileName.Text, pathIDSubject + "/" + "List." + fileName);

            }
            StackPanelMain.Children.Clear();
            UCSelectClass ucSelectClass = new UCSelectClass(this);
            StackPanelMain.Children.Add(ucSelectClass);


            
        }

        private void btnCancelSubject_Click(object sender, RoutedEventArgs e)
        {
            btnADDSubject.Visibility = Visibility.Collapsed;
            btnCancelSubject.Visibility = Visibility.Collapsed;
            btnNewSubject.Visibility = Visibility.Visible;
            StackPanelMain.Children.Clear();
            UCSelectClass ucSelectClass = new UCSelectClass(this);
            StackPanelMain.Children.Add(ucSelectClass);
        }

       
        


    }
}
