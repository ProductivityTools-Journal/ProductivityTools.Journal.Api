using ProductivityTools.Meetings.CoreObjects;
using System.Collections.Generic;

namespace ProductivityTools.Meetings.Services
{
    public interface IMeetingService
    {
        List<CoreObjects.Page> GetMeetings(string email, int? treeNodeId, bool drillDown);
        void DeletePage(string email, int pageId);
    }
}