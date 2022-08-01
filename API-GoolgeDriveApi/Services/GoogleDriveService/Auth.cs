using API_GoolgeDriveApi.Services.GoogleDriveService.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;

namespace API_GoolgeDriveApi.Services.GoogleDriveService
{
    public class Auth : IAuth
    {
        private string pathOAuth = @"C:\Users\adolf\OneDrive\Escritorio\Datos personales\GoogleCloudOAuth\client_secret_380759970163-n8ujjmtngo9ngfs9c0kre39h2a187jel.apps.googleusercontent.com.json";
        static string[] Scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile};
        static string ApplicationName = "API-GoogleDriveApi";
        UserCredential? credential;

        public Auth(string path)
        {
            using (var stream = new FileStream(pathOAuth, FileMode.Open, FileAccess.Read))
            {
                string credPath = @"C:\Users\adolf\OneDrive\Escritorio\Datos personales\GoogleCloudOAuth\Creadentials";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Creadenciales guardadas en: " + credPath);                    
            }
        }
        public DriveService MakeService()
        {
            var service = new DriveService(new Google.Apis.Services.BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
            
            return service;
        }               
    }
}
