﻿using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ProductivityTools.Journal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomTokenController : Controller
    {


        [HttpGet]
        [Route("PublishBookNotes")]
        public async Task<string> GetTokenGAS()
        {
            var uid = "PublishBookNotes";
            string customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid);
            return customToken;
        }
    }
}
