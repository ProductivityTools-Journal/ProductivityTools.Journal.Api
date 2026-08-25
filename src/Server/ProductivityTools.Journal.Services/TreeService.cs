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
        private readonly IMeetingQueries MeetingQueries;

        readonly IMapper Mapper;

        public TreeService(ITreeQueries treeQueries, ITreeCommands treeCommands, IJournalCommands meetingCommands, IPermissionCommands permissionCommands, IMeetingQueries meetingQueries, IMapper mapper)
        {
            this.TreeQueries = treeQueries;
            this.TreeCommands = treeCommands;
            this.MeetingCommands = meetingCommands;
            this.PermissionCommands = permissionCommands;
            this.MeetingQueries = meetingQueries;
            this.Mapper = mapper;
        }

        private List<CoreObjects.Journal> GetNodes(string email, int parent)
        {
            var userJournals = this.TreeQueries.GetUserJournals(email);
            return BuildHierarchy(userJournals, parent);
        }

        private List<CoreObjects.Journal> BuildHierarchy(List<Database.Objects.Journal> userJournals, int parent)
        {
            var nodesByParent = userJournals
                .Select(x => this.Mapper.Map<CoreObjects.Journal>(x))
                .ToLookup(x => x.ParentId);

            List<CoreObjects.Journal> GetChildren(int parentId)
            {
                var children = nodesByParent[parentId].OrderBy(x => x.Name).ToList();
                foreach (var child in children)
                {
                    child.Nodes = GetChildren(child.Id);
                }
                return children;
            }

            return GetChildren(parent);
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
            var userJournals = this.TreeQueries.GetUserJournals(email);
            var nodes = BuildHierarchy(userJournals, parent);
            List<int> result = GetIds(nodes);
            return result;
        }

        public CoreObjects.Journal GetTree(string email)
        {
            var rootdb = TreeQueries.GetRoot();
            CoreObjects.Journal root = Mapper.Map<CoreObjects.Journal>(rootdb);
            var userJournals = this.TreeQueries.GetUserJournals(email);
            root.Nodes = BuildHierarchy(userJournals, rootdb.JournalId);
            return root;
        }

        //public List<CoreObjects.Journal> GetTreePaths(string email, List<int> treeIds)
        //{
        //    var result= new List<CoreObjects.Journal>();
        //    foreach(var  id in treeIds)
        //    {
        //        var treePath=GetTreePath(email, id);
        //        result.Add(treePath);
        //    }
        //    return result;
        //}

        ////gets and returns path to given page
        //private CoreObjects.Journal GetTreePath(string email, int treeId)
        //{
        //    List<CoreObjects.Journal> flatPath = new List<CoreObjects.Journal>();
        //    var lowestElement = TreeQueries.GetTreeNode(treeId);
        //    this.TreeQueries.ValidateOnershipCall(email, new int[] { lowestElement.JournalId });
        //    while (lowestElement != null && lowestElement.Name != "Root")
        //    {
        //        flatPath.Add(Mapper.Map<CoreObjects.Journal>(lowestElement));
        //        lowestElement = TreeQueries.GetTreeNode(lowestElement.ParentId.Value);
        //    }
        //    flatPath.Reverse();
        //    CoreObjects.Journal result = null;
        //    CoreObjects.Journal resultIterator = null;
        //    foreach (var item in flatPath)
        //    {
        //        if (result == null)
        //        {
        //            result = item;
        //            resultIterator = item;
        //            result.Parent = null;//I do not need it
        //            //dbcontext if contains the value it will place it automatically
        //        }
        //        else
        //        {
        //            resultIterator.Nodes.Add(item);
        //            resultIterator = item;
        //            item.Parent = null;
        //        }
        //    }
        //    return result;
        //}

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
                    userId = 4;
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
        public int AddIfDoesNotExists(string email, int parentId, string journalName)
        {
            var jounnalId = this.TreeCommands.CheckIfTreeNodeExists(parentId, journalName);
            if (jounnalId == null)
            {
                jounnalId = this.AddTreeNode(email, parentId, journalName);
            }
            return jounnalId.Value;
        }

        public string GetPublicHash(string email, int journalId)
        {
            var hasPermission = this.TreeQueries.ValidateOnershipCall(email, new int[] { journalId });
            if (!hasPermission)
            {
                throw new UnauthorizedAccessException();
            }
            return this.TreeCommands.GetPublicHash(journalId);
        }
    }
}
