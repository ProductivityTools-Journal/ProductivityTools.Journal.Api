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
        private readonly IJournalCommands MeetingCommands;
        private readonly IPermissionCommands PermissionCommands;

        readonly IMapper Mapper;

        public TreeService(ITreeQueries treeQueries, ITreeCommands treeCommands, IJournalCommands meetingCommands, IPermissionCommands permissionCommands, IMapper mapper)
        {
            this.TreeQueries = treeQueries;
            this.TreeCommands = treeCommands;
            this.MeetingCommands = meetingCommands;
            this.PermissionCommands = permissionCommands;
            this.Mapper = mapper;
        }

        private List<TreeNode> GetNodes(string email, int parent)
        {
            List<TreeNode> result = new List<TreeNode>();
            var dbTreeNodes = this.TreeQueries.GetTree(email,parent);
            foreach (var dbTreeNode in dbTreeNodes)
            {
                TreeNode treeNode = this.Mapper.Map<TreeNode>(dbTreeNode);
                treeNode.ParentId = parent;
                treeNode.Nodes = GetNodes(email,dbTreeNode.TreeId);
                if (this.TreeQueries.ValidateOnershipCall(email, new int[] { treeNode.Id }))
                {
                    result.Add(treeNode);
                }
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

        public List<int> GetFlatChildsId(string email,int parent)
        {
            var nodes = GetNodes(email,parent);
            List<int> result = GetIds(nodes); 
            return result;
        }

        public List<TreeNode> GetTree(string email)
        {
            var rootdb = TreeQueries.GetRoot();
            TreeNode root = Mapper.Map<TreeNode>(rootdb);
            root.Nodes = GetNodes(email,rootdb.TreeId);
            List<TreeNode> result = new List<TreeNode>();
            result.Add(root);
            return result;
        }

        public int AddTreeNode(string email,int parentId, string name)
        {
            var result=this.TreeCommands.AddTreeNode(parentId, name);
            if(parentId==1)
            {
                int userId = -1;
                if (email == "pwujczyk@google.com")
                {
                    userId = 3;
                }
                if (email == "pwujczyk@gmail.com")
                {
                    userId =1;
                }
                if (email == "grzegorz.opara@gmail.com")
                {
                    userId = 2;
                }
                this.PermissionCommands.AddOwner(userId,result.TreeId);
            }
            return result.TreeId;
        }

        public int Delete(string email, int treeId)
        {
            List<TreeNode> subTreeNodes = GetNodes(email, treeId);
            subTreeNodes.Add(this.Mapper.Map < TreeNode >(this.TreeQueries.GetTreeNode(treeId)));
            var treesIds = subTreeNodes.Select(x => x.Id);
            int meetingRemoved=this.MeetingCommands.Delete(treesIds);
            int treeNodeRemoved=this.TreeCommands.Delete(treesIds);
            return meetingRemoved + treeNodeRemoved;
        }

        public void MoveTree(int sourceId, int targetId)
        {
            this.TreeCommands.Move(sourceId, targetId);
        }

    }
}
