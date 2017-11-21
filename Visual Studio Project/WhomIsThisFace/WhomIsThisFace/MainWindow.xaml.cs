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
using MahApps.Metro.Controls;
using WhomIsThisFace.UC;
using DirectShowLib;
using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;

namespace WhomIsThisFace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        bool[] currentMenu;
        public static String Gmail="";
        public static DriveService Service;
        public static SheetsService SheetService;
        public static String sheetsID = "";
        public static int Gid = 0;
        public MainWindow()
        {
            InitializeComponent();
            currentMenu = new bool[4];

            miHome.Background = Brushes.LightSkyBlue;
            miTest.Background = Brushes.LightSkyBlue;
            miTrain.Background = Brushes.LightSkyBlue;
            miResult.Background = Brushes.LightSkyBlue;
            menuTop.Background = Brushes.LightSkyBlue;


            miHome.IsEnabled = false;
            miHome.Background = Brushes.White;
            miHome.Foreground = Brushes.LightSkyBlue;

            UCHome ucHome = new UCHome();
            UCSparate ucSparate = new UCSparate();
            StackPanelMain.Children.Clear();
            StackPanelMain.Children.Add(ucSparate);
            StackPanelMain.Children.Add(ucHome);

            tbSubjectID.Text = Gobal.SUBJECT_ID +" "+Gobal.YEAR+"/"+Gobal.TERM;

            


        }
        
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Brush selected = Brushes.White;
            Brush unSelected = Brushes.LightSkyBlue;
            MenuItem menuItem = (MenuItem)sender;

            if (menuItem.Header.Equals("Home") && miHome.IsEnabled) 
            {
                miHome.IsEnabled=false;
                miTest.IsEnabled = true;
                miTrain.IsEnabled = true;
                miResult.IsEnabled = true;

                miHome.Background = selected;
                miTest.Background = unSelected;
                miTrain.Background = unSelected;
                miResult.Background = unSelected;

                miHome.Foreground = unSelected;
                miTest.Foreground = selected;
                miTrain.Foreground = selected;
                miResult.Foreground = selected;

                UCHome ucHome = new UCHome();
                UCSparate ucSparate = new UCSparate();
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(ucSparate);
                StackPanelMain.Children.Add(ucHome);
            }
            else if (menuItem.Header.Equals("Test"))
            {
                miHome.IsEnabled = true;
                miTest.IsEnabled = false;
                miTrain.IsEnabled = true;
                miResult.IsEnabled = true;

                miHome.Background = unSelected;
                miTest.Background = selected;
                miTrain.Background = unSelected;
                miResult.Background = unSelected;

                miHome.Foreground = selected;
                miTest.Foreground = unSelected;
                miTrain.Foreground = selected;
                miResult.Foreground = selected;

                UCTest ucTest = new UCTest();
                UCSparate ucSparate = new UCSparate();
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(ucSparate);
                StackPanelMain.Children.Add(ucTest);
            }
            else if (menuItem.Header.Equals("Train"))
            {
                miHome.IsEnabled = true;
                miTest.IsEnabled = true;
                miTrain.IsEnabled = false;
                miResult.IsEnabled = true;

                miHome.Background = unSelected;
                miTest.Background = unSelected;
                miTrain.Background = selected;
                miResult.Background = unSelected;

                miHome.Foreground = selected;
                miTest.Foreground = selected;
                miTrain.Foreground = unSelected;
                miResult.Foreground = selected;

                UCTrain ucTrain = new UCTrain();
                UCSparate ucSparate = new UCSparate();
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(ucSparate);
                StackPanelMain.Children.Add(ucTrain);
            }
            else if (menuItem.Header.Equals("Result"))
            {
                miHome.IsEnabled = true;
                miTest.IsEnabled = true;
                miTrain.IsEnabled = true;
                miResult.IsEnabled = false;

                miHome.Background = unSelected;
                miTest.Background = unSelected;
                miTrain.Background = unSelected;
                miResult.Background = selected;

                miHome.Foreground = selected;
                miTest.Foreground = selected;
                miTrain.Foreground = selected;
                miResult.Foreground = unSelected;

                UCResult ucResult = new UCResult();
                UCSparate ucSparate = new UCSparate();
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(ucSparate);
                StackPanelMain.Children.Add(ucResult);
            }
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            
            SelectClassWindow selectClassWindow = new SelectClassWindow();
            selectClassWindow.Show();
            this.Close();
        }

        private void btnSheet_Click(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.sheetsID.Equals(""))
                System.Diagnostics.Process.Start("https://docs.google.com/spreadsheets/d/" + MainWindow.sheetsID);
        }

    }
}
