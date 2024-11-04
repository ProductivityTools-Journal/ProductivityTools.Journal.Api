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
        readonly IMapper Mapper;

        public PageService(IMeetingQueries meetingQueries, IJournalCommands meetingCommands, ITreeService treeService, IMapper mapper)
        {
            this.MeetingQueries = meetingQueries;
            this.TreeService = treeService;
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
    }
}
