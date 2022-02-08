using ProductivityTools.Meetings.Database;
using ProductivityTools.Meetings.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ProducvitityTools.Meetings.Queries
{
    public interface ITreeQueries
    {
        TreeNode GetRoot();
        List<TreeNode> GetTree(int parentId);
        TreeNode GetTreeNode(int id);   
    }

    class TreeQueries : ITreeQueries
    {
        MeetingContext MeetingContext;

        public TreeQueries(MeetingContext context)
        {
            this.MeetingContext = context;
        }

        public TreeNode GetRoot()
        {
            TreeNode root = this.MeetingContext.Tree.Where(x => x.Name == "Root").First();
            return root;
        }

        public List<TreeNode> GetTree(int parentId)
        {
            var result = this.MeetingContext.Tree.Where(x => x.ParentId == parentId && x.TreeId != x.ParentId && x.Deleted==false).ToList();
            return result;
        }

        public TreeNode GetTreeNode(int id)
        {
            var result = this.MeetingContext.Tree.SingleOrDefault(x => x.TreeId == id);
            return result;
        }
    }
}
