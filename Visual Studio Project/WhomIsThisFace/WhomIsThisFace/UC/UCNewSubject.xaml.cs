using Microsoft.Win32;
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

namespace WhomIsThisFace.UC
{
    /// <summary>
    /// Interaction logic for UCNewSubject.xaml
    /// </summary>
    public partial class UCNewSubject : UserControl
    {
        public UCNewSubject()
        {
            InitializeComponent();
            cbTerm.Items.Add("1");
            cbTerm.Items.Add("2");
            cbTerm.SelectedItem = "1";
            tbYear.Text = DateTime.Now.Year.ToString();
        }

  
        private void btnOpenfile_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "CSV Files (.csv)|*.csv";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                // Open the selected file to read.
               // System.IO.Stream fileStream = openFileDialog1.OpenFile();
                tbFileName.Text = openFileDialog1.FileName;
               /* using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    // Read the first line from the file and write it the textbox.
                    //  tbResults.Text = reader.ReadLine();
                }
                fileStream.Close();*/
            }
        }



    }
}
