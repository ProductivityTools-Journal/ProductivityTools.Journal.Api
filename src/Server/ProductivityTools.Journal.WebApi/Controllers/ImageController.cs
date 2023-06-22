using Auth0.AuthenticationApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using ProductivityTools.Journal.Images;

namespace ProductivityTools.Journal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : JlController
    {
        [HttpPost]
        [Route("Upload")]
        public string Upload([FromForm] IFormFile file)
        {
            try
            {
                // getting file original name
                string FileName = file.FileName;
                Stream s = file.OpenReadStream();
                ImageManager imageManager = new ImageManager();
                var path=imageManager.UploadImageToStorage(s,base.UserId,FileName,"image/jpg");

                return path;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
