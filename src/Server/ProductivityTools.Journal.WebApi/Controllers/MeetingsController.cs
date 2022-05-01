using Auth0.AuthenticationApi;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductivityTools.Journal.WebApi;
using ProductivityTools.Meetings.CoreObjects;
using ProductivityTools.Meetings.Services;
using ProducvitityTools.Meetings.Commands;
using ProducvitityTools.Meetings.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductivityTools.Meetings.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly IMapper mapper;
        IMeetingQueries MeetingQueries;
        IMeetingCommands MeetingCommands;
        IMeetingService MeetingService;
        private readonly IConfiguration configuration;
        private IHttpContextAccessor _httpContextAccessor;
       // UserManager<IdentityUser> _userManager;

        public MeetingsController(IMeetingQueries meetingQueries, IMeetingCommands meetingCommands, IMeetingService meetingService, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.MeetingQueries = meetingQueries;
            this.mapper = mapper;
            this.MeetingService = meetingService;
            this.MeetingCommands = meetingCommands;
            this.configuration = configuration;
           // this._userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("Date")]
        public object GetDate()
        {
            return DateTime.Now;
        }

        [HttpPost]
        [Route("List2")]
        public string Get2(object name)
        {
            return $"Welcome {name.ToString()}";
        }

        [HttpPost]
        [Route("List3")]
        public string Get3(object name)
        {
            var remotesecret = name.ToString();
            string s = Environment.GetEnvironmentVariable("MeetingsSecret", EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrEmpty(s))
            {
                if (remotesecret != s)
                {
                    throw new Exception("Wrong secret");
                }
            }
            return $"Welcome {name.ToString()} secret checked= {s}";
        }

        private void SaveToLog(string message)
        {
            //using (EventLog eventLog = new EventLog("Application"))
            //{
            //    eventLog.Source = "Application";
            //    eventLog.WriteEntry(message, EventLogEntryType.Information, 101, 1);
            //}
        }
        



        [HttpPost]
        [Authorize]
        [AuthenticatedUsers]
        [Route(Consts.ListName)]
        public List<JournalItem> GetList(MeetingListRequest meetingListRequest)
        {
            //var x=_userManager.GetUserAsync(HttpContext.User);

            SaveToLog("Request started");
            List<JournalItem> result = this.MeetingService.GetMeetings(UserEmail, meetingListRequest.Id, meetingListRequest.DrillDown).OrderByDescending(x => x.Date).ToList();
            SaveToLog("Meetings mapped");
            return result;
        }

        private string UserEmail
        {
            get
            {
                var x1 = HttpContext.User;
                var identity = (ClaimsIdentity)HttpContext.User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var email = claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                return email;
            }
        }

        [HttpPost]
        [Route(Consts.MeetingName)]
        [Authorize]
        public JournalItem Get(MeetingId meetingId)
        {
            var partresult = MeetingQueries.GetMeeting(UserEmail, meetingId.Id.Value);
            JournalItem result = this.mapper.Map<JournalItem>(partresult);
            SaveToLog("Meetings mapped");
            return result;
        }


        [HttpPost]
        [Route(Consts.AddMeetingName)]
        //add validation
        public int Save(JournalItem meeting)
        {
            Database.Objects.JournalItem dbMeeting = this.mapper.Map<Database.Objects.JournalItem>(meeting);
            int meetingId = MeetingCommands.Save(dbMeeting);
            return meetingId;
        }

        [HttpPost]
        [Route(Consts.UpdateMeetingName)]
        public void Update(JournalItem meeting)
        {
            Database.Objects.JournalItem dbMeeting = this.mapper.Map<Database.Objects.JournalItem>(meeting);
            MeetingCommands.Update(dbMeeting);
        }

        [HttpPost]
        [Route(Consts.DeleteMeetingName)]
        public void Delete(MeetingId meeting)
        {
            MeetingService.DeleteMeeting(UserEmail, meeting.Id.Value);
        }
    }
}
