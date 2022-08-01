using API_GoolgeDriveApi.Models;
using API_GoolgeDriveApi.Services.GoogleDriveService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_GoolgeDriveApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FolderController : Controller
    {
        Reply oR;        
        IGoogleDriveService _googleDriveService;
        public FolderController(IGoogleDriveService googleDriveService)
        {
            _googleDriveService = googleDriveService;
            oR = new Reply();
        }

        [HttpGet]
        [Route("CreateFolder")]
        public Reply CreateFolder(string name)
        {
            oR.message = "OK";
            oR.data = _googleDriveService.CreateFolder(name);
            oR.result = 1;

            if (oR.data == null)
            {
                oR.result = 0;
                oR.message = "server error";
            }
            return oR;
        }

        [HttpGet]
        [Route("ListFolders")]
        public Reply ListFolders()
        {
            oR.message = "OK";
            oR.data = _googleDriveService.ListFolders();
            oR.result = 1;

            if (oR.data == null)
            {
                oR.result = 0;
                oR.message = "server error";
            }
            return oR;
        }
    }
}
