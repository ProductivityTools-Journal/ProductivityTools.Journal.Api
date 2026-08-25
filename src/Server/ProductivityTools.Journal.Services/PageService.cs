using AutoMapper;
using ProductivityTools.Meetings.CoreObjects;
using ProducvitityTools.Meetings.Commands;
using ProducvitityTools.Meetings.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductivityTools.Meetings.Services
{
    class PageService : IPageService
    {
        IMeetingQueries MeetingQueries;
        IJournalCommands MeetingCommand;
        ITreeService TreeService;
        ITreeQueries TreeQueries;
        readonly IMapper Mapper;

        public PageService(IMeetingQueries meetingQueries, IJournalCommands meetingCommands, ITreeService treeService, ITreeQueries treeQueries, IMapper mapper)
        {
            this.MeetingQueries = meetingQueries;
            this.TreeService = treeService;
            this.TreeQueries = treeQueries;
            this.MeetingCommand = meetingCommands;
            this.Mapper = mapper;
        }

        public List<CoreObjects.Page> GetPages(string email, int? treeNodeId, bool drillDown)
        {
            if (treeNodeId.HasValue)
            {
                return GetPagesInternal(email, treeNodeId.Value, drillDown);

            }
            else
            {
                if (drillDown)
                {
                    return this.Mapper.Map<List<CoreObjects.Page>>(this.MeetingQueries.GetPages());
                }
                else
                {
                    return new List<CoreObjects.Page>();
                }
            }
        }

        public List<CoreObjects.Page> GetPagesInternal(string email, int treeNodeId, bool drillDown)
        {
            var trees = new List<int>() { treeNodeId };
            if (drillDown)
            {
                trees.AddRange(this.TreeService.GetFlatChildsId(email,treeNodeId));
            }

            //var result = new List<Meeting>();
            var dbResult = this.MeetingQueries.GetPages(email, trees).ToList();
            var result = this.Mapper.Map<List<CoreObjects.Page>>(dbResult);
            return result;
        }

        public void DeletePage(string email, int meetingId)
        {
            var meeting = this.MeetingQueries.GetPage(email,meetingId);
            this.MeetingCommand.Delete(meeting);
        }

        public string GetPublicHash(string email, int pageId)
        {
            var page = this.MeetingQueries.GetPage(email, pageId);
            if (!string.IsNullOrEmpty(page.PublicHash))
            {
                return page.PublicHash;
            }
            var hash = Guid.NewGuid().ToString("N");
            page.PublicHash = hash;
            this.MeetingCommand.Update(page);
            return hash;
        }

        public CoreObjects.Page GetPublicPage(string publicHash)
        {
            var dbPage = this.MeetingQueries.GetPageByPublicHash(publicHash);
            if (dbPage == null)
            {
                return null;
            }
            var result = this.Mapper.Map<CoreObjects.Page>(dbPage);
            if (dbPage.JournalId.HasValue)
            {
                result.Path = this.TreeQueries.GetJournalPath(dbPage.JournalId.Value);
            }
            return result;
        }

        public List<CoreObjects.Page> GetPublicPages(string publicHash)
        {
            var rootJournal = this.TreeQueries.GetJournalByPublicHash(publicHash);
            if (rootJournal == null)
            {
                return new List<CoreObjects.Page>();
            }

            var journalIds = new List<int>() { rootJournal.JournalId };
            journalIds.AddRange(this.TreeQueries.GetFlatChildsIdPublic(rootJournal.JournalId));

            var journalPaths = this.TreeQueries.GetJournalPaths(journalIds);
            var dbPages = this.MeetingQueries.GetPagesByJournalIds(journalIds);
            var result = this.Mapper.Map<List<CoreObjects.Page>>(dbPages);
            foreach (var page in result)
            {
                if (page.JournalId.HasValue && journalPaths.TryGetValue(page.JournalId.Value, out var path))
                {
                    page.Path = path;
                }
            }
            return result;
        }
    }
}
