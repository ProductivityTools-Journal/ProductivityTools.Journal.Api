using Microsoft.AspNetCore.Mvc;
using ProducvitityTools.Meetings.Queries;
using System;

namespace ProductivityTools.Journal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebugController : Controller
    {
        private readonly IMeetingQueries MeetingQueries;

        public DebugController(IMeetingQueries context)
        {
            this.MeetingQueries = context;
        }

        [HttpGet]
        [Route("Date")]
        public string Date()
        {
            return DateTime.Now.ToString();
        }

        [HttpGet]
        [Route("AppName")]
        public string AppName()
        {
            return "PTJournal";
        }

        [HttpGet]
        [Route("Hello")]
        public string Hello(string name)
        {
            return string.Concat($"Hello {name.ToString()} Current date:{DateTime.Now}");
        }

        [HttpGet]
        [Route("ServerName")]
        public string ServerName()
        {
            string server = this.MeetingQueries.GetServerName();
            return server;
        }
    }
}
