using ProductivityTools.Meetings.CoreObjects;
using System.Collections.Generic;

namespace ProductivityTools.Meetings.Services
{
    public interface ITreeService
    {
        List<TreeNode> GetTree();
        int AddTreeNode(int parentId, string name);
        int Delete(int treeId);
        List<int> GetFlatChildsId(int parent);

        void MoveTree(int sourceId, int targetId);
    }
}