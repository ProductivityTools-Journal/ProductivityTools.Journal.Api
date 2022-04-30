using Microsoft.EntityFrameworkCore;
using ProductivityTools.Meetings.Database;
using ProductivityTools.Meetings.Database.Objects;
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

        private ValidateTreeOwners(string user, List<int> treeNodeIds)
        {
            foreach(int nodeId in treeNodeIds)
            {
                if
            }
        }

        public List<JournalItem> GetMeetings()
        {
            var result = this.MeetingContext.JournalItem
                .Include(x => x.NotesList)
                .ToList();
            return result;
        }

        public List<JournalItem> GetMeetings(List<int> treeNodeId)
        {
            var result = this.MeetingContext.JournalItem
                .Where(x => x.TreeId.HasValue && treeNodeId.Contains(x.TreeId.Value))
                .Include(x => x.NotesList)
                .OrderByDescending(x => x.Date).Take(50).ToList();
            return result;
        }

        public JournalItem GetMeeting(int id)
        {
            var result = this.MeetingContext.JournalItem
                .Include(x => x.NotesList)
                .SingleOrDefault(x => x.JournalItemId == id);
            return result;
        }
    }
}
