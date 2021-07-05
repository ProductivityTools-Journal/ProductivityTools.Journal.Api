using AutoMapper;
using ProductivityTools.Meetings.CoreObjects;
using ProducvitityTools.Meetings.Queries;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace ProductivityTools.Meetings.Services
{
    public class TreeService : ITreeService
    {
        readonly ITreeQueries TreeQueries;
        readonly IMapper Mapper;

        public TreeService(ITreeQueries treeQueries, IMapper mapper)
        {
            this.TreeQueries = treeQueries;
            this.Mapper = mapper;
        }

        private List<TreeNode> GetNodes(int parent)
        {
            List<TreeNode> result = new List<TreeNode>();
            var dbTreeNodes = this.TreeQueries.GetTree(parent);
            foreach (var dbTreeNode in dbTreeNodes)
            {
                TreeNode treeNode = this.Mapper.Map<TreeNode>(dbTreeNode);
                treeNode.Nodes = GetNodes(dbTreeNode.TreeId);
                result.Add(treeNode);
            }
            return result;
        }

        private List<int> GetIds(List<TreeNode> nodes)
        {
            List<int> result = new List<int>();

            foreach (var subnode in nodes)
            {
                result.Add(subnode.Id);
                var subtree = GetIds(subnode.Nodes);
                result.AddRange(subtree);
            }
            return result;
        }

        public List<int> GetFlatChildsId(int parent)
        {
            var nodes = GetNodes(parent);
            List<int> result = GetIds(nodes);
            return result;
        }

        public List<TreeNode> GetTree()
        {
            var rootdb = TreeQueries.GetRoot();
            TreeNode root = Mapper.Map<TreeNode>(rootdb);
            root.Nodes = GetNodes(rootdb.TreeId);
            List<TreeNode> result = new List<TreeNode>();
            result.Add(root);
            return result;
        }

        public void AddTreeNode(int parentId, string name)
        {
            this.TreeQueries.AddTreNode(parentId, name);
        }

        public int RemoveTreeNodeWithAllItems(int treeId)
        {
            return 1;
        }
    }
}
