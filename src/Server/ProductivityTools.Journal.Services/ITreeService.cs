using ProductivityTools.Meetings.CoreObjects;
using System.Collections.Generic;

namespace ProductivityTools.Meetings.Services
{
    public interface ITreeService
    {
        CoreObjects.Journal GetTree(string email);
        //List<CoreObjects.Journal> GetTreePaths(string email, List<int> treeIds);
        int AddTreeNode(string email, int parentId, string name);
        int Delete(string email, int treeId);
        List<int> GetFlatChildsId(string email,int parent);


        void MoveTree(int sourceId, int targetId);

        CoreObjects.Journal RenameJournal(int journalId, string newName);
        int? AddIfDoesNotExists(int parentId, string journalName);
    }
}