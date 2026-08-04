using System.Collections.Generic;
using ProductivityTools.Meetings.Database.Objects;

namespace ProducvitityTools.Meetings.Queries
{
    public interface IMeetingQueries
    {
        List<Page> GetPages();
        List<Page> GetPages(string email, List<int> treeNodeIds);
        Page GetPage(string email, int id);
        Page GetPageByPublicHash(string publicHash);
        List<Page> GetPagesByJournalIds(List<int> journalIds);
        List<Page> GetPagesWithoutPlainText(int count = 100);

        string GetServerName();
    }
}