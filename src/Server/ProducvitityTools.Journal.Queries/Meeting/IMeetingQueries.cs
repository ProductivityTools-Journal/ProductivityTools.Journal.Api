using System.Collections.Generic;
using ProductivityTools.Meetings.Database.Objects;

namespace ProducvitityTools.Meetings.Queries
{
    public interface IMeetingQueries
    {
        List<Page> GetPages();
        List<Page> GetPages(string email, List<int> treeNodeIds);
        Page GetPage(string email, int id);
    }
}