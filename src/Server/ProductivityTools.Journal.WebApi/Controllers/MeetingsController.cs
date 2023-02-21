using Auth0.AuthenticationApi;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductivityTools.Journal.WebApi;
using ProductivityTools.Journal.WebApi.Controllers;
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
    public class MeetingsController : JlController 
    {
        private readonly IMapper mapper;
        IMeetingQueries MeetingQueries;
        IJournalCommands MeetingCommands;
        IMeetingService MeetingService;
        private readonly IConfiguration configuration;
        private IHttpContextAccessor _httpContextAccessor;
       // UserManager<IdentityUser> _userManager;

        public MeetingsController(IMeetingQueries meetingQueries, IJournalCommands meetingCommands, IMeetingService meetingService, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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



        [HttpPost]
        [Route(Consts.MeetingName)]
        [Authorize]
        public JournalItem Get(JournalId meetingId)
        {
            var partresult = MeetingQueries.GetPage(UserEmail, meetingId.Id.Value);
            JournalItem result = this.mapper.Map<JournalItem>(partresult);
            SaveToLog("Meetings mapped");
            return result;
        }


        [HttpPost]
        [Route(Consts.AddMeetingName)]
        //add validation
        public JournalItem Save(JournalItem meeting)
        {
            Database.Objects.Page dbMeeting = this.mapper.Map<Database.Objects.Page>(meeting);
            Database.Objects.Page savedMeeting = MeetingCommands.Save(dbMeeting);
            var result = this.mapper.Map<JournalItem>(savedMeeting);
            return result;
        }

        [HttpPost]
        [Route(Consts.UpdateMeetingName)]
        public void Update(JournalItem meeting)
        {
            Database.Objects.Page dbMeeting = this.mapper.Map<Database.Objects.Page>(meeting);
            MeetingCommands.Update(dbMeeting);
        }

        [HttpPost]
        [Route(Consts.DeletePageName)]
        public void Delete(PageId page)
        {
            MeetingService.DeletePage(UserEmail, page.Id);
        }
    }
}
