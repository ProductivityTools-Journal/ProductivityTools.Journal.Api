using System.Collections.Generic;
using ProductivityTools.Meetings.Database.Objects;

namespace ProducvitityTools.Meetings.Queries
{
    public interface IMeetingQueries
    {
        List<JournalItem> GetMeetings();
        List<JournalItem> GetMeetings(string email, List<int> treeNodeIds);
        JournalItem GetMeeting(string email, int id);
    }
}