using ProductivityTools.Meetings.CoreObjects;
using System.Collections.Generic;

namespace ProductivityTools.Meetings.Services
{
    public interface IMeetingService
    {
        List<JournalItem> GetMeetings(int? treeNodeId, bool drillDown);
        void DeleteMeeting(int meetingId);
    }
}