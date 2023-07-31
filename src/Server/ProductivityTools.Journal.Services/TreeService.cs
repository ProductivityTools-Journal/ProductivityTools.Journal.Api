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

        private List<CoreObjects.Journal> GetNodes(string email, int parent)
        {
            List<CoreObjects.Journal> result = new List<CoreObjects.Journal>();
            var dbTreeNodes = this.TreeQueries.GetTree(email, parent);
            foreach (var dbTreeNode in dbTreeNodes)
            {
                CoreObjects.Journal treeNode = this.Mapper.Map<CoreObjects.Journal>(dbTreeNode);
                treeNode.ParentId = parent;
                treeNode.Nodes = GetNodes(email, dbTreeNode.JournalId);
                if (this.TreeQueries.ValidateOnershipCall(email, new int[] { treeNode.Id }))
                {
                    result.Add(treeNode);
                }
            }
            result = result.OrderBy(x => x.Name).ToList();
            return result;
        }

        private List<int> GetIds(List<CoreObjects.Journal> nodes)
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

        public List<int> GetFlatChildsId(string email, int parent)
        {
            var nodes = GetNodes(email, parent);
            List<int> result = GetIds(nodes);
            return result;
        }

        public CoreObjects.Journal GetTree(string email)
        {
            var rootdb = TreeQueries.GetRoot();
            CoreObjects.Journal root = Mapper.Map<CoreObjects.Journal>(rootdb);
            root.Nodes = GetNodes(email, rootdb.JournalId);
            return root;
        }

        public List<CoreObjects.Journal> GetTreePaths(string email, List<int> treeIds)
        {
            var result= new List<CoreObjects.Journal>();
            foreach(var  id in treeIds)
            {
                var treePath=GetTreePath(email, id);
                result.Add(treePath);
            }
            return result;
        }

        //gets and returns path to given page
        private CoreObjects.Journal GetTreePath(string email, int treeId)
        {
            List<CoreObjects.Journal> flatPath = new List<CoreObjects.Journal>();
            var lowestElement = TreeQueries.GetTreeNode(treeId);
            this.TreeQueries.ValidateOnershipCall(email, new int[] { lowestElement.JournalId });
            while (lowestElement != null && lowestElement.Name != "Root")
            {
                flatPath.Add(Mapper.Map<CoreObjects.Journal>(lowestElement));
                lowestElement = TreeQueries.GetTreeNode(lowestElement.ParentId.Value);
            }
            flatPath.Reverse();
            CoreObjects.Journal result = null;
            CoreObjects.Journal resultIterator = null;
            foreach (var item in flatPath)
            {
                if (result == null)
                {
                    result = item;
                    resultIterator = item;
                    result.Parent = null;//I do not need it
                    //dbcontext if contains the value it will place it automatically
                }
                else
                {
                    resultIterator.Nodes.Add(item);
                    resultIterator = item;
                    item.Parent = null;
                }
            }
            return result;
        }

        public int AddTreeNode(string email, int parentId, string name)
        {
            var result = this.TreeCommands.AddTreeNode(parentId, name);
            if (parentId == 1)
            {
                int userId = -1;
                if (email == "pwujczyk@google.com")
                {
                    userId = 3;
                }
                if (email == "pwujczyk@gmail.com")
                {
                    userId = 1;
                }
                if (email == "malgorzata.wujczyk@gmail.com")
                {
                    userId = 2;
                }
                this.PermissionCommands.AddOwner(userId, result.JournalId);
            }
            return result.JournalId;
        }

        public int Delete(string email, int treeId)
        {
            List<CoreObjects.Journal> subTreeNodes = GetNodes(email, treeId);
            subTreeNodes.Add(this.Mapper.Map<CoreObjects.Journal>(this.TreeQueries.GetTreeNode(treeId)));
            var treesIds = subTreeNodes.Select(x => x.Id);
            int meetingRemoved = this.MeetingCommands.Delete(treesIds);
            int treeNodeRemoved = this.TreeCommands.Delete(treesIds);
            return meetingRemoved + treeNodeRemoved;
        }

        public void MoveTree(int sourceId, int targetId)
        {
            this.TreeCommands.Move(sourceId, targetId);
        }

        public CoreObjects.Journal RenameJournal(int journalId, string newName)
        {
            var r = this.TreeCommands.RenameJournal(journalId, newName);
            return this.Mapper.Map<CoreObjects.Journal>(r);
        }

    }
}
