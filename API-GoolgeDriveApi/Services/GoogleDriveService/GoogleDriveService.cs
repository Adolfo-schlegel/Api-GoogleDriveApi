using API_GoolgeDriveApi.Services.GoogleDriveService.Interfaces;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.StaticFiles;

namespace API_GoolgeDriveApi.Services.GoogleDriveService
{
    public class GoogleDriveService : IGoogleDriveService
    {
        IAuth _auth;
        public GoogleDriveService(IAuth auth)
        {
            _auth = auth;
        }       
        
        public List<string> ListFolders()
        {
            try
            {
                
                DriveService service = _auth.MakeService();

                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.Fields = "nextPageToken, files(id, name)";
                listRequest.Q = $"mimeType = 'application/vnd.google-apps.folder' and trashed = false";

                IList<Google.Apis.Drive.v3.Data.File> Folders = listRequest.Execute().Files;

                return new List<string>(Folders.Select(f => "ID: " + f.Id + " - Name: " + f.Name).ToList());
            }
            catch(Exception)
            {
                return null;
            }
        }

        public List<string> ListFilesInFolder(string folder)
        {
            try
            {
                DriveService service = _auth.MakeService();

                FilesResource.ListRequest listRequest = service.Files.List();                
                listRequest.Q = $"mimeType != 'application/vnd.google-apps.folder' and trashed = false and '{Find_IdFile_ForName(folder)[0]}' in parents";

                // List files.
                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

                return new List<string>(files.Select(f => "ID: " + f.Id + " - Name: " + f.Name).ToList());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<string> ListAllFiles()
        {
            try
            {
                DriveService service = _auth.MakeService();

                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.Q = $"mimeType != 'application/vnd.google-apps.folder' and trashed = false";

                // List files.
                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

                return new List<string>(files.Select(f => "ID: " + f.Id + " - Name: " + f.Name).ToList());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<string> ListTrashFiles()
        {
            try
            {
                DriveService service = _auth.MakeService();

                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.Q = $"mimeType != 'application/vnd.google-apps.folder' and trashed = true";

                // List files.
                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

                return new List<string>(files.Select(f => "ID: " + f.Id + " - Name: " + f.Name).ToList());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string UploadFile(string descripcion, string filepath, string folder = "My Drive")
        {
            try
            {
                DriveService service = _auth.MakeService();

                FilesResource.CreateMediaUpload request;

                //file metadata
                var driveFile = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(filepath),
                    Description = descripcion,
                    MimeType = GetMimeType(filepath),
                    Parents = Find_IdFile_ForName(folder),
                };

                using (var stream = System.IO.File.OpenRead(filepath))
                {
                    //create a new file with metadata and stream
                    request = service.Files.Create(driveFile, stream, driveFile.MimeType);
                    request.Fields = "id";
                    var response = request.Upload();

                    try
                    {
                        var file = request.ResponseBody;
                        return file.Id;
                    }
                    catch (Exception)
                    {
                        return "The name folder not exist";
                    }
                };                               
            }
            catch(Exception)
            {
                return null;
            }            
        }

        public string CreateFolder(string name)
        {
            try
            {
                DriveService service = _auth.MakeService();

                var driveFolder = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = name,
                    MimeType = "application/vnd.google-apps.folder"
                };

                var request = service.Files.Create(driveFolder);

                var response = request.Execute();

                return response.Id;
            }
            catch(Exception)
            {
                return null;
            }
        }

        //tools
        private string GetMimeType(string FileName)
        {
            string? contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(FileName, out contentType);
            return contentType ?? "application/octet-stream";
        }

        private List<string> Find_IdFile_ForName(string name)
        {
            try
            {
                DriveService service = _auth.MakeService();

                FilesResource.ListRequest request = service.Files.List();
                request.Q = $"name = '{name}'";

                IList<Google.Apis.Drive.v3.Data.File> files = request.Execute().Files;

                return new List<string>(files.Select(f=> f.Id).ToList());
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
