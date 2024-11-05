using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using ProductivityTools.Journal.Images;
using Google.Cloud.Storage.V1;

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
                var path=imageManager.UploadImageToStorage(s,base.UserEmail,FileName,"image/jpg");

                return path;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private const string MimeType = "image/png";
        private const string FileName = "CM-Logo.png";

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(string fileName)
        {
            var ookies = Request.Cookies;
            string value = string.Empty;
            ookies.TryGetValue("SomeCookie", out value);
            //var token = GetValue(context, "token", "Please provide filename");
            //string userEmail = "pwujczyk@gmail.com";//await ValidateBearer(token);
            //string user = userEmail.Replace("@", "-").Replace(".", "-");
            //var fileName = GetValue(context, "fileName", "Please provide filename");
            ////await context.Response.WriteAsync(fileName);
            ////await context.Response.WriteAsync(userEmail);
            //var fullName = $"{user}/{fileName}";
            //var bucketName = string.Format($"ptjournalimages");
            //var client = StorageClient.Create();
            //context.Response.ContentType = "image/jpeg";
            //using (var stream = new MemoryStream())
            //{
            //    client.DownloadObject(bucketName, fullName, stream);
            //    await context.Response.BodyWriter.WriteAsync(stream.ToArray());
            //}
            //var image = _fileService.GetImageAsByteArray();
            FileStream file = new FileStream("lmx.png", FileMode.Open);

            return File(file, MimeType, FileName);
        }
    }
}
