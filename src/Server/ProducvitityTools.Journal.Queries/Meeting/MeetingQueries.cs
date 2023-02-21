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

        public List<Page> GetPages()
        {
            //do not know when it is used, so throw it.
            throw new UnauthorizedAccessException();
            var result = this.MeetingContext.Pages
              //  .Include(x => x.NotesList)
                .ToList();
            return result;
        }

        public List<Page> GetPages(string email, List<int> treeNodeId)
        {
            treeNodeId.RemoveAll(x => x == 1);
            QueriesHelper.ValidateOnershipCall(this.MeetingContext, email, treeNodeId.ToArray());

            var result = this.MeetingContext.Pages
            .Where(x => x.JournalId.HasValue && treeNodeId.Contains(x.JournalId.Value))
           // .Include(x => x.NotesList)
            .OrderByDescending(x => x.Date).Take(50).ToList();
            return result;


        }

        public Page GetPage(string email, int pageId)
        {
            var result = this.MeetingContext.Pages
               // .Include(x => x.NotesList)
                .SingleOrDefault(x => x.PageId == pageId);
            QueriesHelper.ValidateOnershipCall(this.MeetingContext, email, new int[] { result.JournalId.Value });

            return result;
        }
    }
}
