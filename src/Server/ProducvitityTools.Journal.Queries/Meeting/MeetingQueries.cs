using Microsoft.EntityFrameworkCore;
using ProductivityTools.Journal.Database;
using ProductivityTools.Meetings.Database;
using ProductivityTools.Meetings.Database.Objects;
using ProducvitityTools.Journal.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProducvitityTools.Meetings.Queries
{
    public class MeetingQueries : IMeetingQueries
    {
        MeetingContext MeetingContext;

        public MeetingQueries(MeetingContext context)
        {
            this.MeetingContext = context;
        }

        //private ValidateTreeOwners(string user, List<int> treeNodeIds)
        //{
        //    foreach(int nodeId in treeNodeIds)
        //    {
        //        if
        //    }
        //}




        public List<JournalItem> GetPages()
        {
            //do not know when it is used, so throw it.
            throw new UnauthorizedAccessException();
            var result = this.MeetingContext.JournalItem
                .Include(x => x.NotesList)
                .ToList();
            return result;
        }

        public List<JournalItem> GetPages(string email, List<int> treeNodeId)
        {
            treeNodeId.RemoveAll(x => x == 1);
            QueriesHelper.ValidateOnershipCall(this.MeetingContext, email, treeNodeId.ToArray());

            var result = this.MeetingContext.JournalItem
            .Where(x => x.TreeId.HasValue && treeNodeId.Contains(x.TreeId.Value))
            .Include(x => x.NotesList)
            .OrderByDescending(x => x.Date).Take(50).ToList();
            return result;


        }

        public JournalItem GetPage(string email, int pageId)
        {
            var result = this.MeetingContext.JournalItem
                .Include(x => x.NotesList)
                .SingleOrDefault(x => x.JournalItemId == pageId);
            QueriesHelper.ValidateOnershipCall(this.MeetingContext, email, new int[] { result.TreeId.Value });

            return result;
        }
    }
}
