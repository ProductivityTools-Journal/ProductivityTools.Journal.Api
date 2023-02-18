using System.Collections.Generic;
using ProductivityTools.Meetings.Database.Objects;

namespace ProducvitityTools.Meetings.Queries
{
    public interface IMeetingQueries
    {
        List<JournalItem> GetPages();
        List<JournalItem> GetPages(string email, List<int> treeNodeIds);
        JournalItem GetPage(string email, int id);
    }
}