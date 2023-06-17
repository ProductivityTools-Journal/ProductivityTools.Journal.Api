using Auth0.AuthenticationApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

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

                // combining GUID to create unique name before saving in wwwroot
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileName;

                // getting full path inside wwwroot/images
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", FileName);

                // copying file
                file.CopyTo(new FileStream(imagePath, FileMode.Create));

                return "File Uploaded Successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
