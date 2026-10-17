using ProductivityTools.Meetings.CoreObjects;
using System.Collections.Generic;

namespace ProductivityTools.Meetings.Services
{
    public interface ITreeService
    {
        CoreObjects.Journal GetTree(string email, bool includeArchived = false);
        //List<CoreObjects.Journal> GetTreePaths(string email, List<int> treeIds);
        int AddTreeNode(string email, int parentId, string name);
        int Delete(string email, int treeId);
        List<int> GetFlatChildsId(string email, int parent, bool includeArchived = false);


        void MoveTree(int sourceId, int targetId);

        CoreObjects.Journal RenameJournal(int journalId, string newName);
        int AddIfDoesNotExists(string email, int parentId, string journalName);
        string GetPublicHash(string email, int journalId);
        CoreObjects.Journal SetInboxName(string email, int journalId, string inboxName);
        CoreObjects.Journal RemoveInboxName(string email, int journalId);
        CoreObjects.Journal ArchiveJournal(string email, int journalId, bool archived = true);
    }
}