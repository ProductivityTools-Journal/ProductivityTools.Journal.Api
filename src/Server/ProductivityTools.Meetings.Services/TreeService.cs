using AutoMapper;
using ProductivityTools.Meetings.CoreObjects;
using ProducvitityTools.Meetings.Commands;
using ProducvitityTools.Meetings.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace ProductivityTools.Meetings.Services
{
    public class TreeService : ITreeService
    {
        private readonly ITreeQueries TreeQueries;
        private readonly ITreeCommands TreeCommands;
        private readonly IMeetingCommands MeetingCommands;

        readonly IMapper Mapper;

        public TreeService(ITreeQueries treeQueries, ITreeCommands treeCommands, IMeetingCommands meetingCommands, IMapper mapper)
        {
            this.TreeQueries = treeQueries;
            this.TreeCommands = treeCommands;
            this.MeetingCommands = meetingCommands;
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
            this.TreeCommands.AddTreeNode(parentId, name);
        }

        public int Delete(int treeId)
        {
            List<TreeNode> subTreeNodes = GetNodes(treeId);
            subTreeNodes.Add(this.Mapper.Map < TreeNode >(this.TreeQueries.GetTreeNode(treeId)));
            var treesIds = subTreeNodes.Select(x => x.Id);
            int meetingRemoved=this.MeetingCommands.Delete(treesIds);
            int treeNodeRemoved=this.TreeCommands.Delete(treesIds);
            return meetingRemoved + treeNodeRemoved;
        }
    }
}
