using ProductivityTools.Meetings.CoreObjects;
using System.Collections.Generic;

namespace ProductivityTools.Meetings.Services
{
    public interface ITreeService
    {
        CoreObjects.Journal GetTree(string email);
        int AddTreeNode(string email, int parentId, string name);
        int Delete(string email, int treeId);
        List<int> GetFlatChildsId(string email,int parent);

        void MoveTree(int sourceId, int targetId);
    }
}