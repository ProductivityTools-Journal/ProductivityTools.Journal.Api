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

        public string GetServerName()
        {
            string server = this.MeetingContext.Database.SqlQuery<string>($"select @@SERVERNAME as value").Single();
            return server;
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

            IQueryable<Page> queryPinned = this.MeetingContext.Pages
            .Where(x => x.JournalId.HasValue && treeNodeId.Contains(x.JournalId.Value) && x.Pinned == true && x.Deleted == false)
            // .Include(x => x.NotesList)
            .OrderByDescending(x => x.Date);
            var queryStringPinned = queryPinned.ToQueryString();
            List<Page> pined = queryPinned.ToList();

            IQueryable<Page> query = this.MeetingContext.Pages
            .Where(x => x.JournalId.HasValue && treeNodeId.Contains(x.JournalId.Value) && x.Pinned==false && x.Deleted == false)
            // .Include(x => x.NotesList)
            .OrderByDescending(x => x.Date).Take(50);
            var queryString = query.ToQueryString();
            List<Page> pages = query.ToList();
            pined.AddRange(pages);
            return pined;


        }

        public Page GetPage(string email, int pageId)
        {
            var result = this.MeetingContext.Pages
               // .Include(x => x.NotesList)
                .SingleOrDefault(x => x.PageId == pageId);
            QueriesHelper.ValidateOnershipCall(this.MeetingContext, email, new int[] { result.JournalId.Value });

            return result;
        }

        public Page GetPageByPublicHash(string publicHash)
        {
            if (string.IsNullOrEmpty(publicHash))
            {
                return null;
            }
            var result = this.MeetingContext.Pages
                .SingleOrDefault(x => x.PublicHash == publicHash && x.Deleted == false);
            return result;
        }

        public List<Page> GetPagesByJournalIds(List<int> journalIds)
        {
            var pined = this.MeetingContext.Pages
                .Where(x => x.JournalId.HasValue && journalIds.Contains(x.JournalId.Value) && x.Pinned == true && x.Deleted == false)
                .Select(x => new Page
                {
                    PageId = x.PageId,
                    JournalId = x.JournalId,
                    Date = x.Date,
                    Subject = x.Subject,
                    Content = x.Content,
                    ContentType = x.ContentType,
                    Pinned = x.Pinned,
                    Deleted = x.Deleted,
                    PublicHash = x.PublicHash,
                    PlainText = x.PlainText
                })
                .OrderByDescending(x => x.Date)
                .ToList();

            var pages = this.MeetingContext.Pages
                .Where(x => x.JournalId.HasValue && journalIds.Contains(x.JournalId.Value) && x.Pinned == false && x.Deleted == false)
                .Select(x => new Page
                {
                    PageId = x.PageId,
                    JournalId = x.JournalId,
                    Date = x.Date,
                    Subject = x.Subject,
                    Content = x.Content,
                    ContentType = x.ContentType,
                    Pinned = x.Pinned,
                    Deleted = x.Deleted,
                    PublicHash = x.PublicHash,
                    PlainText = x.PlainText
                })
                .OrderByDescending(x => x.Date)
                .ToList();

            pined.AddRange(pages);
            return pined;
        }

        public List<Page> GetPagesWithoutPlainText(int count = 1000)
        {
            var result = this.MeetingContext.Pages
                .Where(x => (x.PlainText == null || x.PlainText == "") && x.Content != null)
                .OrderByDescending(x => x.PageId)
                .Take(count)
                .ToList();
            return result;
        }
    }
}
