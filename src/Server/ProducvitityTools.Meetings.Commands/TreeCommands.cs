using ProductivityTools.Meetings.Database;
using ProductivityTools.Meetings.Database.Objects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProducvitityTools.Meetings.Commands
{

    public interface ITreeCommands
    {
        void AddTreeNode(int parentId, string name);
        int Delete(IEnumerable<int> treeIds);
    }

    public class TreeCommands : ITreeCommands
    {
        MeetingContext MeetingContext;

        public TreeCommands(MeetingContext context)
        {
            this.MeetingContext = context;
        }

        public void AddTreeNode(int parentId, string name)
        {
            TreeNode tree = new TreeNode() { ParentId = parentId, Name = name };
            this.MeetingContext.Tree.Add(tree);
            this.MeetingContext.SaveChanges();
        }

        public int Delete(IEnumerable<int> treeIds)
        {
            var trees = this.MeetingContext.Tree.Where(x => treeIds.Contains(x.TreeId));
            foreach(var tree in trees)
            {
                tree.Deleted = true;
                MeetingContext.Update(tree);
            }

            this.MeetingContext.SaveChanges();
            return trees.Count();
        }
    }
}
