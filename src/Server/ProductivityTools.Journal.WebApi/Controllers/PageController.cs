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
    public class PageController : JlController 
    {
        private readonly IMapper mapper;
        IMeetingQueries MeetingQueries;
        IJournalCommands MeetingCommands;
        IPageService MeetingService;         
        ITreeService TreeService;
        private readonly IConfiguration configuration;
        private IHttpContextAccessor _httpContextAccessor;
       // UserManager<IdentityUser> _userManager;

        public PageController(IMeetingQueries meetingQueries, IJournalCommands meetingCommands, IPageService meetingService,ITreeService treeService, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.MeetingQueries = meetingQueries;
            this.mapper = mapper;
            this.MeetingService = meetingService;
            this.MeetingCommands = meetingCommands;
            this.TreeService = treeService;
            this.configuration = configuration;
           // this._userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("UserEmail")]
        public object UserEmailProvider(MeetingListRequest request)
        {
            return "pawel";
        }

        [HttpPost]
        [Route("Date2")]
        public object GetDate2()
        {

            return "pawel2";
        }

        [HttpGet]
        [Route("echo")]
        public string echo(string name)
        {
            return $"Welcome request performed at {DateTime.Now} with param {name} on server {System.Environment.MachineName} to Application Jounral";
        }

        [HttpPost]
        [Route("Date")]
        public object GetDate()
        {
            var cookieOptions = new CookieOptions();
            cookieOptions.SameSite = SameSiteMode.None;
            cookieOptions.Expires = DateTime.Now.AddDays(1);
            //cookieOptions.Path = "/";
            Response.Cookies.Append("Date", "SomeValue2", cookieOptions);
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
        [Route("SaveBookJournal")]
        //add validation
        public Page SaveBookJournal(BookJournal bookJournal)
        {
            
            Database.Objects.Page dbPage = this.mapper.Map<Database.Objects.Page>(bookJournal.Page);
            var journalId = TreeService.AddIfDoesNotExists(bookJournal.Email, bookJournal.ParentJournalId, bookJournal.JournalName);
            dbPage.JournalId = journalId;
            Database.Objects.Page savedMeeting = MeetingCommands.Save(dbPage);
            var result = this.mapper.Map<CoreObjects.Page>(savedMeeting);
            return result;
        }

        [HttpPost]
        [Authorize]
        [AuthenticatedUsers]
        [Route(Consts.ListName)]
        public List<CoreObjects.Page> GetList(MeetingListRequest meetingListRequest)
        {
            //var x=_userManager.GetUserAsync(HttpContext.User);

            SaveToLog("Request started");
            List<CoreObjects.Page> result = this.MeetingService.GetPages(UserEmail, meetingListRequest.Id, meetingListRequest.DrillDown).ToList();
            SaveToLog("Meetings mapped");
            return result;
        }



        [HttpPost]
        [Route(Consts.MeetingName)]
        [Authorize]
        public CoreObjects.Page Get(JournalId meetingId)
        {
            var partresult = MeetingQueries.GetPage(UserEmail, meetingId.Id.Value);
            CoreObjects.Page result = this.mapper.Map<CoreObjects.Page>(partresult);
            SaveToLog("Meetings mapped");
            return result;
        }


        [HttpPost]
        [Route(Consts.AddMeetingName)]
        //add validation
        public CoreObjects.Page Save(CoreObjects.Page meeting)
        {
            Database.Objects.Page dbMeeting = this.mapper.Map<Database.Objects.Page>(meeting);
            Database.Objects.Page savedMeeting = MeetingCommands.Save(dbMeeting);
            var result = this.mapper.Map<CoreObjects.Page>(savedMeeting);
            return result;
        }

      

        [HttpPost]
        [Route(Consts.UpdateMeetingName)]
        public void Update(CoreObjects.Page meeting)
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
