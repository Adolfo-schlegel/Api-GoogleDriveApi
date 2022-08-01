using API_GoolgeDriveApi.Models;
using API_GoolgeDriveApi.Services.GoogleDriveService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_GoolgeDriveApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        Reply oR;
        IGoogleDriveService _googleDriveService;
        public FileController(IGoogleDriveService googleDriveService)
        {
            oR = new Reply();
            _googleDriveService = googleDriveService;
        }

        [HttpGet]
        [Route("CreateFile")]
        public Reply CreateFile(string descripcion, string filepath, string folder = "My Drive")
        {
            oR.message = "OK";
            oR.result = 1;
            oR.data = _googleDriveService.UploadFile(descripcion, filepath, folder);
            
            if(oR.data == null)
            {
                oR.result = 0;
                oR.message = "server error";
            }

            return oR;
        }

        [HttpGet]
        [Route("ListFilesInFolder")]
        public Reply ListFilesInFolder(string folder)
        {
            oR.message = "OK";
            oR.data = _googleDriveService.ListFilesInFolder(folder);
            oR.result = 1;
             
            if(oR.data == null)
            {
                oR.result = 0;
                oR.message = "server error";                
            }
            return oR;
        }

        [HttpGet]
        [Route("ListAllFiles")]
        public Reply ListAllFiles()
        {
            oR.message = "OK";
            oR.data = _googleDriveService.ListAllFiles();
            oR.result = 1;

            if(oR.data == null)
            {
                oR.result = 1;
                oR.message = "server error";                
            }
            return oR;
        }

        [HttpGet]
        [Route("ListTrashedFiles")]
        public Reply ListTrashedFiles()
        {
            oR.message = "OK";
            oR.data = _googleDriveService.ListTrashFiles();
            oR.result = 1;
            
            if(oR.data == null)
            {
                oR.result = 1;
                oR.message = "Server error";
            }
            return oR;
        }
    }
}
