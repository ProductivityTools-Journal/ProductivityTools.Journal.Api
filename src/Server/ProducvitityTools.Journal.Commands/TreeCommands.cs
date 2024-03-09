using Microsoft.EntityFrameworkCore.Query;
using ProductivityTools.Journal.Database;
using ProductivityTools.Meetings.Database;
using ProductivityTools.Meetings.Database.Objects;
using System.Collections;

using System.Collections.Generic;
using System.Linq;

namespace ProducvitityTools.Meetings.Commands
{

    public interface ITreeCommands
    {
        Journal AddTreeNode(int parentId, string name);
        int Delete(IEnumerable<int> treeIds);
        void Move(int sourceId, int targetId);

        Journal RenameJournal(int journalId, string newName);
        int? CheckIfTreeNodeExists(int parentId, string name);
    }

    public class TreeCommands : ITreeCommands
    {
        MeetingContext MeetingContext;

        public TreeCommands(MeetingContext context)
        {
            this.MeetingContext = context;
        }

        public Journal AddTreeNode(int parentId, string name)
        {
            Journal tree = new Journal() { ParentId = parentId, Name = name };
            this.MeetingContext.Journal.Add(tree);
            this.MeetingContext.SaveChanges();
            return tree;
        }

        public int Delete(IEnumerable<int> treeIds)
        {
            var trees = this.MeetingContext.Journal.Where(x => treeIds.Contains(x.JournalId));
            foreach (var tree in trees)
            {
                tree.Deleted = true;
                MeetingContext.Update(tree);
            }

            this.MeetingContext.SaveChanges();
            return trees.Count();
        }

        public void Move(int source, int target)
        {
            var sourceElement = this.MeetingContext.Journal.Where(x => x.JournalId == source).FirstOrDefault();
            sourceElement.ParentId = target;
            MeetingContext.Update(sourceElement);
            MeetingContext.SaveChanges();
        }

        public Journal RenameJournal(int journalId, string newName)
        {
            var sourceElement = this.MeetingContext.Journal.Where(x => x.JournalId == journalId).FirstOrDefault();
            sourceElement.Name = newName;
            MeetingContext.Update(sourceElement);
            MeetingContext.SaveChanges();
            return sourceElement;
        }

        public int? CheckIfTreeNodeExists(int parentId, string name)
        {
            var journals = this.MeetingContext.Journal.FirstOrDefault(x => x.ParentId == parentId && x.Name == name);
            return journals?.JournalId;
        }
    }
}
