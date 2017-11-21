using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using GoogleDriveV3.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

namespace WhomIsThisFace.UC
{
    /// <summary>
    /// Interaction logic for UCHome.xaml
    /// </summary>
    public partial class UCHome : UserControl
    {
        static string[] Scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile, DriveService.Scope.DriveMetadata, DriveService.Scope.DriveAppdata, SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Drive API .NET Quickstart";
        DriveService service;
        String displayOnGoogleSite;
        public UCHome()
        {
            InitializeComponent();

            miGmail.Background = Brushes.LightCoral;

            miSheet.Background = Brushes.LightCoral;
            menuTop.Background = Brushes.LightCoral;

            miGmail.IsEnabled = false;
            miGmail.Background = Brushes.White;
            miGmail.Foreground = Brushes.LightCoral;

            Image Img = new Image();

            Uri uri = new Uri("/HowTo/Gmail.PNG", UriKind.Relative);
            BitmapImage imgSource = new BitmapImage(uri);
            Img.Height = 600;
            Img.Source = imgSource;
            StackPanelMain.Children.Clear();
            StackPanelMain.Children.Add(Img);


            if (MainWindow.Gid != 0) { 
                tbGid.Text = MainWindow.Gid+"";
                tbGid.IsEnabled = false;
                btnLoginSheet.IsEnabled = false;

                tbGetCodeGoogleSite.Visibility = Visibility.Visible;
                tbGetCodeGoogleSite.Text = "<div><img src=\"https://www.google.com/chart?chc=sites&amp;cht=d&amp;chdp=sites&amp;chl=%5B%5B%E0%B8%AA%E0%B9%80%E0%B8%9B%E0%B8%A3%E0%B8%94%E0%B8%8A%E0%B8%B5%E0%B8%95%E0%B8%82%E0%B8%AD%E0%B8%87+Google'%3D20'f%5Cv'a%5C%3D0'10'%3D499'0'dim'%5Cbox1'b%5CF6F6F6'fC%5CF6F6F6'eC%5C0'sk'%5C%5B%22123456%22'%5D'a%5CV%5C%3D12'f%5C%5DV%5Cta%5C%3D10'%3D0'%3D500'%3D597'dim'%5C%3D10'%3D10'%3D500'%3D597'vdim'%5Cbox1'b%5Cva%5CF6F6F6'fC%5CC8C8C8'eC%5C'a%5C%5Do%5CLauto'f%5C&amp;sig=cT7R2VyWDOu6M8oPZmRSx7Klr90\" data-origsrc=\"" + MainWindow.sheetsID + "\" data-type=\"spreadsheet\" data-props=\"align:left;borderTitle:123456;doctype:r;height:600;objectTitle:123456;showBorder:true;showBorderTitle:true;view:sheet;\" width=\"500\" height=\"600\" style=\"display:block;text-align:left;margin-right:auto;\"></div>";
                               
                btnCoppyToClipboard.Visibility = Visibility.Visible;
                lbHTML.Visibility = Visibility.Visible;
            }
            if (!MainWindow.Gmail.Equals(""))
            {
                tbGmail.Text = MainWindow.Gmail;
                tbGmail.IsEnabled = false;
                btnLogin.IsEnabled = false;
                btnDownloadTrainingImage.Visibility = Visibility.Visible;
                btnUploadTrainingImage.Visibility = Visibility.Visible;
                btnDownloadSheet.Visibility = Visibility.Visible;
                btnUploadSheet.Visibility = Visibility.Visible;
                btnOpenSheet.Visibility = Visibility.Visible;
                btnLoginSheet.Visibility = Visibility.Visible;
                

                lbGid.Visibility = Visibility.Visible;
                tbGid.Visibility = Visibility.Visible;
                service = MainWindow.Service;
            }
            else {
                btnDownloadTrainingImage.Visibility = Visibility.Collapsed;
                btnUploadTrainingImage.Visibility = Visibility.Collapsed;
                btnDownloadSheet.Visibility = Visibility.Collapsed;
                btnUploadSheet.Visibility = Visibility.Collapsed;
                btnLoginSheet.Visibility = Visibility.Collapsed;
                lbGid.Visibility = Visibility.Collapsed;
                tbGid.Visibility = Visibility.Collapsed;
                btnOpenSheet.Visibility = Visibility.Collapsed;

                tbGetCodeGoogleSite.Visibility = Visibility.Collapsed;
                btnCoppyToClipboard.Visibility = Visibility.Collapsed;
                lbHTML.Visibility = Visibility.Collapsed;
            }
            String sheetPath = Gobal.SUBJECT_ID_PATH + "/sheetID.txt";
            if (File.Exists(sheetPath))
            {
                string[] lines = System.IO.File.ReadAllLines(sheetPath, System.Text.Encoding.GetEncoding(874));
                MainWindow.sheetsID = lines[0];

          //      btnGetCodeGoogleSite.Visibility = Visibility.Visible;
          //      tbGetCodeGoogleSite.Visibility = Visibility.Visible;
            }
            else {
             //   btnGetCodeGoogleSite.Visibility = Visibility.Collapsed;
             //   tbGetCodeGoogleSite.Visibility = Visibility.Collapsed;
            }
        }

        private void btnUploadTrainingImage_Click(object sender, RoutedEventArgs e)
        {

            Thread t = new Thread(
            o =>
            {

                btnDownloadTrainingImage.Dispatcher.BeginInvoke(
                           (Action)(() => { btnDownloadTrainingImage.IsEnabled = false; btnDownloadTrainingImage.Content = "Uploading"; }));
                btnUploadTrainingImage.Dispatcher.BeginInvoke(
                   (Action)(() => { btnUploadTrainingImage.IsEnabled = false; btnUploadTrainingImage.Content = "Uploading"; }));

                string startPath = Gobal.TRAIN_PATH;
                String filename = Gobal.YEAR + "_" + Gobal.TERM + "_" + Gobal.SUBJECT_ID + "_" + "train.zip";
                string zipPath = Gobal.SUBJECT_ID_PATH + "/" + filename;
                if (File.Exists(zipPath))
                    File.Delete(zipPath);
                ZipFile.CreateFromDirectory(startPath, zipPath);


                List<Google.Apis.Drive.v3.Data.File> folders = Util.retrieveFiles(service, "mimeType='application/vnd.google-apps.folder'", "");
                if (folders == null) {

                    //popUP
                    messageDialog("No internet connection", "Please check your network.");


                    btnDownloadTrainingImage.Dispatcher.BeginInvoke(
                           (Action)(() => { btnDownloadTrainingImage.IsEnabled = true; btnDownloadTrainingImage.Content = "Download Training Image"; }));
                    btnUploadTrainingImage.Dispatcher.BeginInvoke(
                       (Action)(() => { btnUploadTrainingImage.IsEnabled = true; btnUploadTrainingImage.Content = "Upload Training Image"; }));
                    return;
                }
                Google.Apis.Drive.v3.Data.File workingFolder = null;
                Google.Apis.Drive.v3.Data.File workingFile = null;

                foreach (Google.Apis.Drive.v3.Data.File folder in folders)
                {
                    if (folder.Name.Equals("WTF"))
                    {
                        workingFolder = folder;
                        break;
                    }
                }
                if (workingFolder == null)
                    workingFolder = Util.CreateFolder(service, "WTF", "");


                List<Google.Apis.Drive.v3.Data.File> files = Util.retrieveFiles(service, null, workingFolder.Id);

                foreach (Google.Apis.Drive.v3.Data.File file in files)
                {
                    if (file.Name.Equals(filename))
                    {

                        workingFile = file;
                        break;
                    }
                }
                if (workingFile != null)
                {
                    Util.deleteFile(service, workingFile.Id);
                }
                workingFile = Util.InsertFile(service, filename, "Train", workingFolder.Id, "mimeType='application/vnd.google-apps.unknown'", zipPath);

                if (File.Exists(zipPath))
                    File.Delete(zipPath);

                btnDownloadTrainingImage.Dispatcher.BeginInvoke(
                           (Action)(() => { btnDownloadTrainingImage.IsEnabled = true; btnDownloadTrainingImage.Content = "Download Training Image"; }));
                btnUploadTrainingImage.Dispatcher.BeginInvoke(
                   (Action)(() => { btnUploadTrainingImage.IsEnabled = true; btnUploadTrainingImage.Content = "Upload Training Image"; }));
            });
            t.Start();

               
           
        }

        private void btnDownloadTrainingImage_Click(object sender, RoutedEventArgs e)
        {

            Thread t = new Thread(
            o =>
            {

                btnDownloadTrainingImage.Dispatcher.BeginInvoke(
                           (Action)(() => { btnDownloadTrainingImage.IsEnabled = false; btnDownloadTrainingImage.Content = "Downloading"; }));
                btnUploadTrainingImage.Dispatcher.BeginInvoke(
                   (Action)(() => { btnUploadTrainingImage.IsEnabled = false; btnUploadTrainingImage.Content = "Downloading"; }));

                string startPath = Gobal.TRAIN_PATH;
                String filename = Gobal.YEAR + "_" + Gobal.TERM + "_" + Gobal.SUBJECT_ID + "_" + "train.zip";
                string zipPath = Gobal.SUBJECT_ID_PATH + "/" + filename;


                List<Google.Apis.Drive.v3.Data.File> folders = Util.retrieveFiles(service, "mimeType='application/vnd.google-apps.folder'", "");
                if (folders == null)
                {

                    //popUP
                    messageDialog("No internet connection","Please check your network.");
                    










                    btnDownloadTrainingImage.Dispatcher.BeginInvoke(
                           (Action)(() => { btnDownloadTrainingImage.IsEnabled = true; btnDownloadTrainingImage.Content = "Download Training Image"; }));
                    btnUploadTrainingImage.Dispatcher.BeginInvoke(
                       (Action)(() => { btnUploadTrainingImage.IsEnabled = true; btnUploadTrainingImage.Content = "Upload Training Image"; }));
                    return;
                }
                Google.Apis.Drive.v3.Data.File workingFolder = null;
                Google.Apis.Drive.v3.Data.File workingFile = null;

                foreach (Google.Apis.Drive.v3.Data.File folder in folders)
                {
                    if (folder.Name.Equals("WTF"))
                    {

                        workingFolder = folder;
                        break;
                    }
                }
                if (workingFolder == null)
                {
                    //pop up
                    messageDialog("No data", "You never backup data.");
                }
                else
                {
                    List<Google.Apis.Drive.v3.Data.File> files = Util.retrieveFiles(service, null, workingFolder.Id);

                    foreach (Google.Apis.Drive.v3.Data.File file in files)
                    {
                        if (file.Name.Equals(filename))
                        {

                            workingFile = file;
                            break;
                        }
                    }
                    if (workingFile == null)
                    {
                        //pop up
                        messageDialog("No data", "You never backup data.");
                        
                    }
                    else
                    {
                        MemoryStream tmp = Util.DownloadFile(service, workingFile.Id);
                        if (File.Exists(zipPath))
                            File.Delete(zipPath);



                        using (FileStream f = new FileStream(zipPath, FileMode.Create, System.IO.FileAccess.Write))
                        {
                            byte[] bytes = tmp.ToArray();
                            f.Write(bytes, 0, bytes.Length);
                        }
                        String[] imageFiles = GetFiles(Gobal.TRAIN_PATH, "*.jpg", SearchOption.TopDirectoryOnly);
                        foreach (String imf in imageFiles)
                            File.Delete(imf);
                        ZipFile.ExtractToDirectory(zipPath, startPath);

                        if (File.Exists(zipPath))
                            File.Delete(zipPath);
                    }


                }

                btnDownloadTrainingImage.Dispatcher.BeginInvoke(
                           (Action)(() => { btnDownloadTrainingImage.IsEnabled = true; btnDownloadTrainingImage.Content = "Download Training Image"; }));
                btnUploadTrainingImage.Dispatcher.BeginInvoke(
                   (Action)(() => { btnUploadTrainingImage.IsEnabled = true; btnUploadTrainingImage.Content = "Upload Training Image"; }));

            });
            t.Start();
            

            


        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            UserCredential credential=null;
            if (tbGmail.Text.Length > 0)
            {

                Thread t = new Thread(
                o =>
                {
                    string gmail="";
                    tbGmail.Dispatcher.BeginInvoke(
                           (Action)(() => { gmail = tbGmail.Text; }));

                    using (var stream =
                    new FileStream("client_id.json", FileMode.Open, FileAccess.Read))
                    {
                        string credPath = System.Environment.GetFolderPath(
                            System.Environment.SpecialFolder.Personal);
                        credPath = System.IO.Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");


                        try
                        {
                            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                                GoogleClientSecrets.Load(stream).Secrets,
                                Scopes,
                                gmail,
                                CancellationToken.None,
                                new FileDataStore(credPath, true)).Result;
                            Console.WriteLine("Credential file saved to: " + credPath);

                            btnDownloadTrainingImage.Dispatcher.BeginInvoke(
                           (Action)(() => { btnDownloadTrainingImage.Visibility = Visibility.Visible; }));
                            btnUploadTrainingImage.Dispatcher.BeginInvoke(
                               (Action)(() => { btnUploadTrainingImage.Visibility = Visibility.Visible; }));
                            btnLogin.Dispatcher.BeginInvoke(
                               (Action)(() => { btnLogin.IsEnabled = false; }));
                            tbGmail.Dispatcher.BeginInvoke(
                               (Action)(() => { tbGmail.IsEnabled = false; }));



                            btnDownloadSheet.Dispatcher.BeginInvoke(
                               (Action)(() => { btnDownloadSheet.Visibility = Visibility.Visible; }));
                            btnUploadSheet.Dispatcher.BeginInvoke(
                               (Action)(() => { btnUploadSheet.Visibility = Visibility.Visible; }));

                            btnLoginSheet.Dispatcher.BeginInvoke(
                              (Action)(() => { btnLoginSheet.Visibility = Visibility.Visible; }));

                            lbGid.Dispatcher.BeginInvoke(
                              (Action)(() => { lbGid.Visibility = Visibility.Visible; }));

                            tbGid.Dispatcher.BeginInvoke(
                              (Action)(() => { tbGid.Visibility = Visibility.Visible; }));

                            btnOpenSheet.Dispatcher.BeginInvoke(
                              (Action)(() => { btnOpenSheet.Visibility = Visibility.Visible; }));

                            if (!MainWindow.sheetsID.Equals(""))
                            {

                                btnOpenSheet.Dispatcher.BeginInvoke(
                              (Action)(() => { btnOpenSheet.Visibility = Visibility.Visible; }));
                            }
                        }
                        catch { 
                            //pop up lost connect
                            messageDialog("No internet connection", "Please check your network.");
                            return;
                        }



                        

                    }





                    // Create Drive API service.
                    service = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = ApplicationName,
                    });

         
                    MainWindow.Service = service;
                    MainWindow.Gmail = gmail;
                    //     myTextBox.Dispatcher.BeginInvoke(
                    //       (Action)(() => { string value = myTextBox.Text; }));


                    string startPath = Gobal.TRAIN_PATH;
                    String filename = Gobal.YEAR + "_" + Gobal.TERM + "_" + Gobal.SUBJECT_ID + "_" + "List";
                    string fileUpload = Gobal.SUBJECT_ID_PATH + "/List.csv";



                    List<Google.Apis.Drive.v3.Data.File> folders = Util.retrieveFiles(service, "mimeType='application/vnd.google-apps.folder'", "");
                    if (folders == null)
                    {

                        //popUP
                        messageDialog("No internet connection", "Please check your network.");


                        return;
                    }

                    Google.Apis.Drive.v3.Data.File workingFolder = null;
                    Google.Apis.Drive.v3.Data.File workingFile = null;

                    foreach (Google.Apis.Drive.v3.Data.File folder in folders)
                    {
                        if (folder.Name.Equals("WTF"))
                        {
                            workingFolder = folder;
                            break;
                        }
                    }
                    if (workingFolder == null)
                        workingFolder = Util.CreateFolder(service, "WTF", "");


                    List<Google.Apis.Drive.v3.Data.File> files = Util.retrieveFiles(service, null, workingFolder.Id);

                    foreach (Google.Apis.Drive.v3.Data.File file in files)
                    {
                        if (file.Name.Equals(filename))
                        {

                            workingFile = file;
                            break;
                        }
                    }
                    if (workingFile == null)
                    {
                        workingFile = Util.InsertSheet(service, filename, "Train", workingFolder.Id, fileUpload);
                        MainWindow.sheetsID = workingFile.Id;
                        String fileSheetID = Gobal.SUBJECT_ID_PATH + "/sheetID.txt";
                        if (File.Exists(fileSheetID))
                        {
                            File.Delete(fileSheetID);
                        }
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileSheetID, true, Encoding.UTF8))
                        {
                            file.WriteLine(workingFile.Id);
                        }
                    }
                    



                    

                });
                t.Start();

                

                 }
            else { 
            
                //popup 
                messageDialog("Check your detail", "Please Enter your Gmail.");
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

        private void btnUploadSheet_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(
            o =>
            {

                btnDownloadSheet.Dispatcher.BeginInvoke(
                           (Action)(() => { btnDownloadSheet.IsEnabled = false; btnDownloadSheet.Content = "Uploading"; }));
                btnUploadSheet.Dispatcher.BeginInvoke(
                   (Action)(() => { btnUploadSheet.IsEnabled = false; btnUploadSheet.Content = "Uploading"; }));

                string startPath = Gobal.TRAIN_PATH;
                String filename = Gobal.YEAR + "_" + Gobal.TERM + "_" + Gobal.SUBJECT_ID + "_" + "List";
                string fileUpload = Gobal.SUBJECT_ID_PATH + "/List.csv";
  


                List<Google.Apis.Drive.v3.Data.File> folders = Util.retrieveFiles(service, "mimeType='application/vnd.google-apps.folder'", "");
                if (folders == null)
                {

                    //popUP
                    messageDialog("No internet connection", "Please check your network.");


                    btnDownloadSheet.Dispatcher.BeginInvoke(
                           (Action)(() => { btnDownloadSheet.IsEnabled = true; btnDownloadSheet.Content = "Download Sheet"; }));
                    btnUploadSheet.Dispatcher.BeginInvoke(
                       (Action)(() => { btnUploadSheet.IsEnabled = true; btnUploadSheet.Content = "Upload Sheet"; }));
                    return;
                }
                Google.Apis.Drive.v3.Data.File workingFolder = null;
                Google.Apis.Drive.v3.Data.File workingFile = null;

                foreach (Google.Apis.Drive.v3.Data.File folder in folders)
                {
                    if (folder.Name.Equals("WTF"))
                    {
                        workingFolder = folder;
                        break;
                    }
                }
                if (workingFolder == null)
                    workingFolder = Util.CreateFolder(service, "WTF", "");


                List<Google.Apis.Drive.v3.Data.File> files = Util.retrieveFiles(service, null, workingFolder.Id);

                foreach (Google.Apis.Drive.v3.Data.File file in files)
                {
                    if (file.Name.Equals(filename))
                    {

                        workingFile = file;
                        break;
                    }
                }
                if (workingFile != null)
                {
                    Util.deleteFile(service, workingFile.Id);
                }



                workingFile = Util.InsertSheet(service, filename, "Train", workingFolder.Id, fileUpload);
                MainWindow.sheetsID = workingFile.Id;
                btnOpenSheet.Dispatcher.BeginInvoke(
                           (Action)(() => { btnOpenSheet.Visibility = Visibility.Visible;}));
                String fileSheetID = Gobal.SUBJECT_ID_PATH+"/sheetID.txt";
                if (File.Exists(fileSheetID)) {
                    File.Delete(fileSheetID);
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileSheetID, true, Encoding.UTF8))
                {
                    file.WriteLine(workingFile.Id);
                }

                btnDownloadSheet.Dispatcher.BeginInvoke(
                           (Action)(() => { btnDownloadSheet.IsEnabled = true; btnDownloadSheet.Content = "Download Sheet"; }));
                btnUploadSheet.Dispatcher.BeginInvoke(
                   (Action)(() => { btnUploadSheet.IsEnabled = true; btnUploadSheet.Content = "Upload Sheet"; }));
            });
            t.Start();
        }

        private void btnDownloadSheet_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(
            o =>
            {

                btnDownloadSheet.Dispatcher.BeginInvoke(
                           (Action)(() => { btnDownloadSheet.IsEnabled = false; btnDownloadSheet.Content = "Downloading"; }));
                btnUploadSheet.Dispatcher.BeginInvoke(
                   (Action)(() => { btnUploadSheet.IsEnabled = false; btnUploadSheet.Content = "Downloading"; }));

                string startPath = Gobal.TRAIN_PATH;
                String filename = Gobal.YEAR + "_" + Gobal.TERM + "_" + Gobal.SUBJECT_ID + "_" + "List";
                string fileUpload = Gobal.SUBJECT_ID_PATH + "/List.csv";



                List<Google.Apis.Drive.v3.Data.File> folders = Util.retrieveFiles(service, "mimeType='application/vnd.google-apps.folder'", "");
                if (folders == null)
                {

                    //popUP
                    messageDialog("No internet connection", "Please check your network.");


                    btnDownloadSheet.Dispatcher.BeginInvoke(
                           (Action)(() => { btnDownloadSheet.IsEnabled = true; btnDownloadSheet.Content = "Download Sheet"; }));
                    btnUploadSheet.Dispatcher.BeginInvoke(
                       (Action)(() => { btnUploadSheet.IsEnabled = true; btnUploadSheet.Content = "Upload Sheet"; }));
                    return;
                }
                Google.Apis.Drive.v3.Data.File workingFolder = null;
                Google.Apis.Drive.v3.Data.File workingFile = null;

                foreach (Google.Apis.Drive.v3.Data.File folder in folders)
                {
                    if (folder.Name.Equals("WTF"))
                    {

                        workingFolder = folder;
                        break;
                    }
                }
                if (workingFolder == null)
                {
                    //pop up
                    messageDialog("No data", "You never backup data.");

                }
                else
                {
                    List<Google.Apis.Drive.v3.Data.File> files = Util.retrieveFiles(service, null, workingFolder.Id);

                    foreach (Google.Apis.Drive.v3.Data.File file in files)
                    {
                        if (file.Name.Equals(filename))
                        {

                            workingFile = file;
                            break;
                        }
                    }
                    if (workingFile == null)
                    {
                        //pop up
                        messageDialog("No data", "You never backup data.");

                    }
                    else
                    {
                        MemoryStream tmp = Util.DownloadSheet(service, workingFile.Id);




                        String ListTmp = Gobal.SUBJECT_ID_PATH + "/ListTmp.csv";
                        if (File.Exists(ListTmp))
                            File.Delete(ListTmp);
                        using (FileStream f = new FileStream(Gobal.SUBJECT_ID_PATH + "/ListTmp.csv", FileMode.Create, System.IO.FileAccess.Write))
                        {
                            byte[] bytes = tmp.ToArray();
                            f.Write(bytes, 0, bytes.Length);
                        }
                        
                        string[] lines = System.IO.File.ReadAllLines(Gobal.SUBJECT_ID_PATH + "/ListTmp.csv", System.Text.Encoding.UTF8);

                        if (File.Exists(fileUpload))
                            File.Delete(fileUpload);
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileUpload, true, Encoding.UTF8))
                        {
                            foreach (string line in lines)
                            {
                               // strOut += line + "\n";
                                file.WriteLine(line);
                            }

                            
                        }
                        File.Delete(ListTmp);
                    }


                }

                btnDownloadSheet.Dispatcher.BeginInvoke(
                           (Action)(() => { btnDownloadSheet.IsEnabled = true; btnDownloadSheet.Content = "Download Sheet"; }));
                btnUploadSheet.Dispatcher.BeginInvoke(
                   (Action)(() => { btnUploadSheet.IsEnabled = true; btnUploadSheet.Content = "Upload Sheet"; }));

            });
            t.Start();
        }

        private void btnLoginSheet_Click(object sender, RoutedEventArgs e)
        {
            UserCredential credentialSheet ;
            if (tbGid.Text.Length > 0)
            {

                Thread t = new Thread(
                o =>
                {
                    string gmail = "";
                    tbGmail.Dispatcher.BeginInvoke(
                           (Action)(() => { gmail = tbGmail.Text; }));

                    string gid = "";
                    tbGmail.Dispatcher.BeginInvoke(
                           (Action)(() => { gid = tbGid.Text; }));


                    using (var streamSheet =
                        new FileStream("client_id_spreadsheet.json", FileMode.Open, FileAccess.Read))
                    {
                        string credPathSheet = System.Environment.GetFolderPath(
                            System.Environment.SpecialFolder.Personal);
                        credPathSheet = System.IO.Path.Combine(credPathSheet, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                        try
                        {
                            credentialSheet = GoogleWebAuthorizationBroker.AuthorizeAsync(
                                GoogleClientSecrets.Load(streamSheet).Secrets,
                                Scopes,
                                gmail,
                                CancellationToken.None,
                                new FileDataStore(credPathSheet, true)).Result;
                            Console.WriteLine("Credential file saved to: " + credPathSheet);
                        }
                        catch {
                            //popup
                            messageDialog("No internet connection", "Please check your network.");
                            return;
                        }

                    }

                 
                    // Create Google Sheets API service.
                    MainWindow.SheetService = new SheetsService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credentialSheet,
                        ApplicationName = ApplicationName,
                    });

                    
                    try
                    {
                        String[] data = { "1"};
                       
                        Util.UpdateCells(MainWindow.SheetService, MainWindow.sheetsID, Int32.Parse(gid), 1, 0, data);
                        MainWindow.Gid = Int32.Parse(gid);
                        tbGetCodeGoogleSite.Dispatcher.BeginInvoke(
                           (Action)(() =>
                           {
                               tbGetCodeGoogleSite.Text = "<div><img src=\"https://www.google.com/chart?chc=sites&amp;cht=d&amp;chdp=sites&amp;chl=%5B%5B%E0%B8%AA%E0%B9%80%E0%B8%9B%E0%B8%A3%E0%B8%94%E0%B8%8A%E0%B8%B5%E0%B8%95%E0%B8%82%E0%B8%AD%E0%B8%87+Google'%3D20'f%5Cv'a%5C%3D0'10'%3D499'0'dim'%5Cbox1'b%5CF6F6F6'fC%5CF6F6F6'eC%5C0'sk'%5C%5B%22123456%22'%5D'a%5CV%5C%3D12'f%5C%5DV%5Cta%5C%3D10'%3D0'%3D500'%3D597'dim'%5C%3D10'%3D10'%3D500'%3D597'vdim'%5Cbox1'b%5Cva%5CF6F6F6'fC%5CC8C8C8'eC%5C'a%5C%5Do%5CLauto'f%5C&amp;sig=cT7R2VyWDOu6M8oPZmRSx7Klr90\" data-origsrc=\"" + MainWindow.sheetsID + "\" data-type=\"spreadsheet\" data-props=\"align:left;borderTitle:123456;doctype:r;height:600;objectTitle:123456;showBorder:true;showBorderTitle:true;view:sheet;\" width=\"500\" height=\"600\" style=\"display:block;text-align:left;margin-right:auto;\"></div>";
                               tbGetCodeGoogleSite.Visibility = Visibility.Visible;
                           }));
                        btnCoppyToClipboard.Dispatcher.BeginInvoke(
                           (Action)(() => { btnCoppyToClipboard.Visibility = Visibility.Visible; }));

                        lbHTML.Dispatcher.BeginInvoke(
                          (Action)(() => { lbHTML.Visibility = Visibility.Visible; }));
                       
                        btnLoginSheet.Dispatcher.BeginInvoke(
                           (Action)(() => { btnLoginSheet.IsEnabled = false; }));
                        tbGid.Dispatcher.BeginInvoke(
                           (Action)(() => { tbGid.IsEnabled = false; }));
                    }
                    catch { 
                        //gid wrong
                        messageDialog("Some thing wrong.", "Please check your network and Gid.");
                    }
                    

                });
                t.Start();



            }
            else
            {

                //popup 
                messageDialog("Check your detail", "Please Enter your Gid.");
            }
        }

        private void btnOpenSheet_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/spreadsheets/d/"+MainWindow.sheetsID);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Brush selected = Brushes.White;
            Brush unSelected = Brushes.LightCoral;
            MenuItem menuItem = (MenuItem)sender;

            if (menuItem.Header.Equals("Gmail") && miGmail.IsEnabled)
            {
                miGmail.IsEnabled = false;
                miSheet.IsEnabled = true;

                miGmail.Background = selected;
                miSheet.Background = unSelected;

                miGmail.Foreground = unSelected;
                miSheet.Foreground = selected;


                Image Img = new Image();
                
                Uri uri = new Uri("/HowTo/Gmail.PNG", UriKind.Relative);
                BitmapImage imgSource = new BitmapImage(uri);
                Img.Height = 600;
                Img.Source = imgSource;
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(Img);


            }
            else if (menuItem.Header.Equals("Sheet"))
            {
                miGmail.IsEnabled = true;
                miSheet.IsEnabled = false;

                miGmail.Background = unSelected;
                miSheet.Background = selected;

                miGmail.Foreground = selected;
                miSheet.Foreground = unSelected;

                Image Img = new Image();

                Uri uri = new Uri("/HowTo/Sheet.PNG", UriKind.Relative);
                BitmapImage imgSource = new BitmapImage(uri);
                Img.Height = 600;
                Img.Source = imgSource;
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(Img);
            }
            
        }

        private void btnCoppyToClipboard_Click(object sender, RoutedEventArgs e)
        {

            Clipboard.SetText(tbGetCodeGoogleSite.Text);

            
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
