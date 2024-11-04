using ProductivityTools.Meetings.CoreObjects;
using System.Collections.Generic;

namespace ProductivityTools.Meetings.Services
{
    public interface IPageService
    {
        List<CoreObjects.Page> GetPages(string email, int? treeNodeId, bool drillDown);
        void DeletePage(string email, int pageId);
    }
}