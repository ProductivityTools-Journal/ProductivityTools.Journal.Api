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
    public class BookController : JlController
    {

        private readonly IMapper mapper;
        IMeetingQueries MeetingQueries;
        IJournalCommands MeetingCommands;
        IMeetingService MeetingService;
        ITreeService TreeService;
        private readonly IConfiguration configuration;
        private IHttpContextAccessor _httpContextAccessor;
        // UserManager<IdentityUser> _userManager;

        public BookController(IMeetingQueries meetingQueries, IJournalCommands meetingCommands, IMeetingService meetingService, ITreeService treeService, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
    }
}
