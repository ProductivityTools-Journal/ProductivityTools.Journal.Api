using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProductivityTools.Meetings.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EchoController : Controller
    {
        [HttpGet]
        [Route("Date")]
        public string Date()
        {
            return DateTime.Now.ToString();
        }

        [HttpPost]
        [Route("Hello")]
        public string Hello(object s)
        {
            return string.Concat($"Hello {s.ToString()} Current date:{DateTime.Now}");
        }
    }
}