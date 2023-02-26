using ProductivityTools.Journal.Database;
using ProductivityTools.Meetings.Database;
using ProductivityTools.Meetings.Database.Objects;
using ProducvitityTools.Journal.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ProducvitityTools.Meetings.Queries
{
    public interface ITreeQueries
    {
        ProductivityTools.Meetings.Database.Objects.Journal GetRoot();
        List<ProductivityTools.Meetings.Database.Objects.Journal> GetTree(string email, int parentId);
        ProductivityTools.Meetings.Database.Objects.Journal GetTreeNode(int id);
        bool ValidateOnershipCall(string email, int[] treeIds);
    }

    class TreeQueries : ITreeQueries
    {
        MeetingContext MeetingContext;

        public TreeQueries(MeetingContext context)
        {
            this.MeetingContext = context;
        }

        public ProductivityTools.Meetings.Database.Objects.Journal GetRoot()
        {
            ProductivityTools.Meetings.Database.Objects.Journal root = this.MeetingContext.Tree.Where(x => x.Name == "Root").First();
            return root;
        }

        public List<ProductivityTools.Meetings.Database.Objects.Journal> GetTree(string email, int parentId)
        {
            var result = this.MeetingContext.Tree.Where(x => x.ParentId == parentId && x.JournalId != x.ParentId && x.Deleted == false).ToList();
            return result;
        }

        public ProductivityTools.Meetings.Database.Objects.Journal GetTreeNode(int id)
        {
            var result = this.MeetingContext.Tree.SingleOrDefault(x => x.JournalId == id);
            return result;
        }

        public bool ValidateOnershipCall(string email, int[] treeIds)
        {
            var r = DatabaseHelpers.ExecutVerifyOwnership(this.MeetingContext, email, treeIds);
            return r;
        }
    }
}
