using Google.Apis.Drive.v3;

namespace API_GoolgeDriveApi.Services.GoogleDriveService.Interfaces
{
    public interface IAuth
    {        
        public DriveService MakeService();
    }
}
