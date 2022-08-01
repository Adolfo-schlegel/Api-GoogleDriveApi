namespace API_GoolgeDriveApi.Services.GoogleDriveService.Interfaces
{
    public interface IGoogleDriveService
    {
        public List<string> ListFilesInFolder(string folder);
        public List<string> ListAllFiles();
        public List<string> ListTrashFiles();
        public List<string> ListFolders();
        public string UploadFile(string folder, string descripcion, string filepath);
        public string CreateFolder(string name);
    }
}
