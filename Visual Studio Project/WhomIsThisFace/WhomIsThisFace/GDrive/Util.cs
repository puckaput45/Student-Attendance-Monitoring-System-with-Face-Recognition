using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveV3.Helpers
{
    public partial class Util
    {

        

        public static List<File> retrieveFiles(DriveService service, String mimeType, String folderId)
        {
            List<File> result = new List<File>();

           
            string pageToken = null;
            do
            {
                FilesResource.ListRequest request = service.Files.List();
                if (!String.IsNullOrEmpty(mimeType))
                {
                    request.Q = mimeType;
                }
                
                request.Spaces = "drive";
                request.Fields = "nextPageToken, files(id, name, parents)";
                request.PageToken = pageToken;
                FileList tmp = null;
                try
                {
                    tmp = request.Execute();
                }
                catch {
                    return null;
                }
                foreach (var file in tmp.Files)
                {
                    if (folderId.Equals("")) {
                        result.Add(file);
                    }
                    else { 
                        if (file.Parents != null) { 
                            foreach (String parent in file.Parents) {
                                if (parent.Equals(folderId))
                                { 
                                    result.Add(file);
                                }
                            }
                        }
                        Console.WriteLine(String.Format(
                                "Found file: %s (%s)", file.Name, file.Id));
                    }
                }
                pageToken = tmp.NextPageToken;
            } while (pageToken != null);
            return result;
        }

       


        public static File InsertFile(DriveService service, String title, String description, String parentId, String mimeType, String filename)
        {
            // File's metadata.
            File body = new File();
            body.Name = title;
            body.Description = description;
            body.MimeType = mimeType;
           // body.Shared = true;
            //Type Folder Test
            //  body.MimeType = "application/vnd.google-apps.video";


            // Set the parent folder.
            if (!String.IsNullOrEmpty(parentId))
            {
                body.Parents = new List<string> { parentId };
            }

            // Load the File's content and put it into a memory stream
            byte[] byteArray = System.IO.File.ReadAllBytes(filename);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);

            try
            {
                // When we add a file, we create an Insert request then call the Upload method on the request.
                // (If we were updating an existing file, we would use the Update function)
                FilesResource.CreateMediaUpload request = service.Files.Create(body, stream, mimeType);
                request.Upload();

                // Set the file object to the response of the upload
                File file = request.ResponseBody;

                // Uncomment the following line to print the File ID.
                // Console.WriteLine("File ID: " + file.Id);

                // return the file object so the caller has a reference to it.
                return file;
            }
            catch (Exception e)
            {
                // May want to log this or do something with it other than just dumping to the console.
                Console.WriteLine("An error occurred: " + e.Message);
                return null;
            }
        }




        public static File InsertSheet(DriveService service, String title, String description, String parentId, String filename)
        {
            // File's metadata.
  
            // body.Shared = true;
            //Type Folder Test
            //  body.MimeType = "application/vnd.google-apps.video";


            var fileMetadata = new File();
            fileMetadata.Name = title;
            fileMetadata.MimeType = "application/vnd.google-apps.spreadsheet";

            if (!String.IsNullOrEmpty(parentId))
            {
                fileMetadata.Parents = new List<string> { parentId };
            }

            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(filename,
                                    System.IO.FileMode.Open))
            {
                request = service.Files.Create(
                    fileMetadata, stream, "text/csv");
                request.Fields = "id";
                try
                {
                    request.Upload();
                    File file = request.ResponseBody;

                    return file;
                }
                catch (Exception e)
                {
                    // May want to log this or do something with it other than just dumping to the console.
                    Console.WriteLine("An error occurred: " + e.Message);
                    return null;
                }
            }


        }
        public static System.IO.MemoryStream DownloadSheet(DriveService service,  String fileId)
        {
            
            var request = service.Files.Export(fileId, "text/csv");
            var stream = new System.IO.MemoryStream();
            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
           
            request.Download(stream);
            return stream;


        }


        public static File UpdateFile(DriveService service, String fileId, String title, String description, String parentId, String mimeType, String filename)
        {
            // File's metadata.
            File body = service.Files.Get(fileId).Execute();
       //     body.Name = title;
         //   body.Description = description;
           // body.MimeType = mimeType;

            //body.ModifiedTime
            // Load the File's content and put it into a memory stream
            byte[] byteArray = System.IO.File.ReadAllBytes(filename);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);

            try
            {
                // When we add a file, we create an Insert request then call the Upload method on the request.
                // (If we were updating an existing file, we would use the Update function)
                FilesResource.UpdateMediaUpload request = service.Files.Update(body, body.Id, stream, body.MimeType);
                request.Upload();

                // Set the file object to the response of the upload
                File file = request.ResponseBody;

                // Uncomment the following line to print the File ID.
                // Console.WriteLine("File ID: " + file.Id);

                // return the file object so the caller has a reference to it.
                return file;
            }
            catch (Exception e)
            {
                // May want to log this or do something with it other than just dumping to the console.
                Console.WriteLine("An error occurred: " + e.Message);
                return null;
            }
        }




        public static File CreateFolder(DriveService service, String name, String parentId)
        {
            var fileMetadata = new File();
            fileMetadata.Name = name;
            fileMetadata.MimeType = "application/vnd.google-apps.folder";
            var request = service.Files.Create(fileMetadata);

            if (!String.IsNullOrEmpty(parentId))
            {
                request.Fields = parentId;
            }
            
            var file = request.Execute();
            Console.WriteLine("Folder ID: " + file.Id);
            return file;
        }
        public static System.IO.MemoryStream DownloadFile(DriveService service, String fileId)
        {

            var request = service.Files.Get(fileId);
            var stream = new System.IO.MemoryStream();

            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
            request.MediaDownloader.ProgressChanged +=
                (IDownloadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            {
                                Console.WriteLine(progress.BytesDownloaded);
                                break;
                            }
                        case DownloadStatus.Completed:
                            {
                                Console.WriteLine("Download complete.");
                                break;
                            }
                        case DownloadStatus.Failed:
                            {
                                Console.WriteLine("Download failed.");
                                break;
                            }
                    }
                };
            request.Download(stream);

            

            return stream;
        }





      public static void deleteFile(DriveService service, String fileId) {
            try {
                service.Files.Delete(fileId).Execute();
            } catch  {
            }
      }

      public static IList<IList<Object>> retrieveData(SheetsService service, String spreadsheetId, String range)
      {
          SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

          ValueRange response = request.Execute();
          IList<IList<Object>> values = response.Values;

          return values;
      }

      public static void UpdateCells(SheetsService service, String spreadsheetId,int gid, int row, int col, String[] value)
      {
         
          List<Request> requests = new List<Request>();
          List<RowData> rowDatas = new List<RowData>();
          for (int i = 0; i < value.Length; i++) {
              List<CellData> valuess = new List<CellData>();
              ExtendedValue extendedValue = new ExtendedValue();
              extendedValue.StringValue = value[i];
              CellData cd = new CellData();
              cd.UserEnteredValue = extendedValue;

              valuess.Add(cd);
          
              RowData rowData = new RowData();
              rowData.Values = valuess;
              rowDatas.Add(rowData);
          }
          GridCoordinate gridCoordinate = new GridCoordinate();
          gridCoordinate.SheetId = gid;
          gridCoordinate.RowIndex = row;
          gridCoordinate.ColumnIndex = col;
          
          

          UpdateCellsRequest updateCellsRequest = new UpdateCellsRequest();
          updateCellsRequest.Rows = rowDatas;
          updateCellsRequest.Start = gridCoordinate;
          updateCellsRequest.Fields = "userEnteredValue";

          Request request = new Request();
          request.UpdateCells = updateCellsRequest;

          requests.Add(request);

          BatchUpdateSpreadsheetRequest batchUpdateRequest = new BatchUpdateSpreadsheetRequest();

          batchUpdateRequest.Requests = requests;
          service.Spreadsheets.BatchUpdate(batchUpdateRequest, spreadsheetId).Execute();

      }
        

    }


}
