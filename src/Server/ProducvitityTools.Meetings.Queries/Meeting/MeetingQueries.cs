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

        public List<JournalItem> GetMeetings()
        {
            var result = this.MeetingContext.JournalItem.ToList();
            return result;
        }

        public List<JournalItem> GetMeetings(List<int> treeNodeId)
        {
            var result = this.MeetingContext.JournalItem
                .Where(x=> x.TreeId.HasValue &&  treeNodeId.Contains(x.TreeId.Value))
                .OrderByDescending(x=>x.Date).Take(50).ToList();
            return result;
        }

        public JournalItem GetMeeting(int id)
        {
            var result = this.MeetingContext.JournalItem.SingleOrDefault(x => x.MeetingId == id);
            return result;
        }
    }
}
