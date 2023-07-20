using Microsoft.EntityFrameworkCore;
using ProductivityTools.Journal.Database;
using ProductivityTools.Meetings.Database;
using ProductivityTools.Meetings.Database.Objects;
using ProducvitityTools.Journal.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Xml.Linq;

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
            var query = this.MeetingContext.Pages
            .Where(x => x.JournalId.HasValue && treeNodeId.Contains(x.JournalId.Value))
            // .Include(x => x.NotesList)
            .OrderByDescending(x => x.Date).Take(50);
            var queryString = query.ToQueryString();
            //DECLARE @__p_1 int = 50;

//            SELECT TOP(@__p_1) [p].[PageId], [p].[Content], [p].[ContentType], [p].[Date], [p].[Deleted], [p].[JournalId], [p].[Subject]
//FROM[j].[Page] AS[p]
//WHERE([p].[JournalId] IS NOT NULL) AND[p].[JournalId] IN(2, 119, 5, 120, 41, 9, 146, 58, 12, 84, 17, 44, 111, 89, 90, 34, 107, 25, 67, 81, 27, 70, 101, 103, 112, 74, 45, 75, 105, 14, 140, 22, 86, 144, 51, 154, 145, 46, 83, 53, 63, 43, 23, dsa125, 126, 100, 47, 129, 91, 38, 128, 20, 21, 24, 39, 115, 139, 85, 136, 73, 138, 69, 141, 19, 55, 13, 123, 124, 11, 15, 26, 59, 4, 118, 42, 92, 8, 147, 127, 65, 68, 72, 66, 80, 56, 57, 16, 137, 153, 135, 114, 30, 99, 28, 64, 151, 150, 79, 131, 18, 29, 82, 148, 98, 52, 116, 117, 54)
//ORDER BY[p].[Date] DESC
            var result = query.ToList();
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
