using ProductivityTools.Meetings.CoreObjects;
using System.Collections.Generic;

namespace ProductivityTools.Meetings.Services
{
    public interface IPageService
    {
        List<CoreObjects.Page> GetPages(string email, int? treeNodeId, bool drillDown);
        void DeletePage(string email, int pageId);
        string GetPublicHash(string email, int pageId);
        CoreObjects.Page GetPublicPage(string publicHash);
        List<CoreObjects.Page> GetPublicPages(string publicHash);
    }
}