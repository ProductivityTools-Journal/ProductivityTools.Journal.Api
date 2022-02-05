using Auth0.AuthenticationApi;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        public MeetingsController(IMeetingQueries meetingQueries, IMeetingCommands meetingCommands, IMeetingService meetingService, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.MeetingQueries = meetingQueries;
            this.mapper = mapper;
            this.MeetingService = meetingService;
            this.MeetingCommands = meetingCommands;
            this.configuration = configuration;
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
        [Route(Consts.ListName)]
        public async Task<List<Meeting>> GetList(MeetingListRequest meetingListRequest)
        {
            //var xx = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            //try
            //{
            //    //var apiClient = new AuthenticationApiClient("productivitytools-meeting-dev.eu.auth0.com");
            //    //var userInfod = await apiClient.GetUserInfoAsync(accessToken);


            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}

            //var userInfoUrl = "https://www.googleapis.com/oauth2/v1/userinfo";
            //var hc = new HttpClient();
            //hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //var response = hc.GetAsync(userInfoUrl).Result;
            //dynamic userInfo = response.Content.ReadAsStringAsync().Result;

            SaveToLog("Request started");

            //var partresult = this.MeetingService.GetMeetings(treeNodeId);
            List<Meeting> result = this.MeetingService.GetMeetings(meetingListRequest.Id, meetingListRequest.DrillDown).OrderByDescending(x => x.Date).ToList();
            SaveToLog("Meetings mapped");
            return result;
        }

        [HttpPost]
        [Route(Consts.MeetingName)]
        public Meeting Get(MeetingId meetingId)
        {
            SaveToLog("Request started");
            var remotesecret = meetingId.Secret.ToString();
            string s = Environment.GetEnvironmentVariable("MeetingsSecret", EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrEmpty(s))
            {
                if (remotesecret != s)
                {
                    throw new Exception("Wrong secret");
                }
            }
            var partresult = MeetingQueries.GetMeeting(meetingId.Id.Value);
            Meeting result = this.mapper.Map<Meeting>(partresult);
            SaveToLog("Meetings mapped");
            return result;
        }


        [HttpPost]
        [Route(Consts.AddMeetingName)]
        //add validation
        public int Save(Meeting meeting)
        {
            Database.Objects.JournalItem dbMeeting = this.mapper.Map<Database.Objects.JournalItem>(meeting);
            int meetingId = MeetingCommands.Save(dbMeeting);
            return meetingId;
        }

        [HttpPost]
        [Route(Consts.UpdateMeetingName)]
        public void Update(Meeting meeting)
        {
            Database.Objects.JournalItem dbMeeting = this.mapper.Map<Database.Objects.JournalItem>(meeting);
            MeetingCommands.Update(dbMeeting);
        }

        [HttpPost]
        [Route(Consts.DeleteMeetingName)]
        public void Delete(MeetingId meeting)
        {
            MeetingService.DeleteMeeting(meeting.Id.Value);
        }
    }
}
