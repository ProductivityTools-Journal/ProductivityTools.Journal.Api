using ProductivityTools.Journal.Database;
using ProductivityTools.Meetings.Database;
using ProductivityTools.Meetings.Database.Objects;
using System.Collections;

using System.Collections.Generic;
using System.Linq;

namespace ProducvitityTools.Meetings.Commands
{

    public interface IPermissionCommands
    {
        void AddOwner(int userId, int treeID);
    }

    public class PermissionCommands : IPermissionCommands
    {
        MeetingContext MeetingContext;

        public PermissionCommands(MeetingContext context)
        {
            this.MeetingContext = context;
        }

        public void AddOwner(int userId, int treeID)
        {
            JournalOwner to = new JournalOwner();
            to.JournalId = treeID;
            to.UserId = userId;
            this.MeetingContext.JournalOwner.Add(to);
            this.MeetingContext.SaveChanges();
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
    }
}
