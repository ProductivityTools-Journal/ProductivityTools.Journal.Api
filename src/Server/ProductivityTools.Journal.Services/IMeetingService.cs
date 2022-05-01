using ProductivityTools.Meetings.CoreObjects;
using System.Collections.Generic;

namespace ProductivityTools.Meetings.Services
{
    public interface IMeetingService
    {
        List<JournalItem> GetMeetings(string email, int? treeNodeId, bool drillDown);
        void DeleteMeeting(string email, int meetingId);
    }
}