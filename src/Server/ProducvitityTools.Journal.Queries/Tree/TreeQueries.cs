using Microsoft.EntityFrameworkCore;
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
        List<ProductivityTools.Meetings.Database.Objects.Journal> GetUserJournals(string email);
        ProductivityTools.Meetings.Database.Objects.Journal GetTreeNode(int id);
        bool ValidateOnershipCall(string email, int[] treeIds);
        ProductivityTools.Meetings.Database.Objects.Journal GetJournalByPublicHash(string publicHash);
        List<ProductivityTools.Meetings.Database.Objects.Journal> GetChildJournals(int parentId);
        string GetJournalPath(int journalId);
        Dictionary<int, string> GetJournalPaths(IEnumerable<int> journalIds);
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
            ProductivityTools.Meetings.Database.Objects.Journal root = this.MeetingContext.Journal.Where(x => x.Name == "Root").First();
            return root;
        }

        public List<ProductivityTools.Meetings.Database.Objects.Journal> GetTree(string email, int parentId)
        {
            var result = this.MeetingContext.Journal.Where(x => x.ParentId == parentId && x.JournalId != x.ParentId && x.Deleted == false).ToList();
            return result;
        }

        public List<ProductivityTools.Meetings.Database.Objects.Journal> GetUserJournals(string email)
        {
            var sql = @"
                WITH UserTree AS (
                    SELECT j.JournalId, j.ParentId, j.Name, j.Deleted, j.PublicHash
                    FROM [j].[Journal] j
                    INNER JOIN [j].[JournalOwner] jo ON j.JournalId = jo.JournalId
                    INNER JOIN [j].[User] u ON jo.UserId = u.UserId
                    WHERE u.email = {0} AND j.Deleted = 0
                    UNION ALL
                    SELECT child.JournalId, child.ParentId, child.Name, child.Deleted, child.PublicHash
                    FROM [j].[Journal] child
                    INNER JOIN UserTree parent ON child.ParentId = parent.JournalId
                    WHERE child.JournalId != child.ParentId AND child.Deleted = 0
                )
                SELECT DISTINCT JournalId, ParentId, Name, Deleted, PublicHash FROM UserTree;";

            var result = this.MeetingContext.Journal
                .FromSqlRaw(sql, email)
                .AsNoTracking()
                .ToList();

            return result;
        }

        public ProductivityTools.Meetings.Database.Objects.Journal GetTreeNode(int id)
        {
            var result = this.MeetingContext.Journal.SingleOrDefault(x => x.JournalId == id);
            return result;
        }

        public bool ValidateOnershipCall(string email, int[] treeIds)
        {
            var r = DatabaseHelpers.ExecutVerifyOwnership(this.MeetingContext, email, treeIds);
            return r;
        }

        public ProductivityTools.Meetings.Database.Objects.Journal GetJournalByPublicHash(string publicHash)
        {
            if (string.IsNullOrEmpty(publicHash))
            {
                return null;
            }
            var result = this.MeetingContext.Journal
                .SingleOrDefault(x => x.PublicHash == publicHash && x.Deleted == false);
            return result;
        }

        public List<ProductivityTools.Meetings.Database.Objects.Journal> GetChildJournals(int parentId)
        {
            var result = this.MeetingContext.Journal
                .Where(x => x.ParentId == parentId && x.JournalId != x.ParentId && x.Deleted == false)
                .ToList();
            return result;
        }

        public string GetJournalPath(int journalId)
        {
            var names = new List<string>();
            var current = this.MeetingContext.Journal.SingleOrDefault(x => x.JournalId == journalId);
            while (current != null && current.Name != "Root")
            {
                names.Add(current.Name);
                if (!current.ParentId.HasValue || current.ParentId.Value == current.JournalId)
                {
                    break;
                }
                current = this.MeetingContext.Journal.SingleOrDefault(x => x.JournalId == current.ParentId.Value);
            }
            if (names.Count == 0 && current != null)
            {
                names.Add(current.Name);
            }
            names.Reverse();
            return string.Join(" / ", names);
        }

        public Dictionary<int, string> GetJournalPaths(IEnumerable<int> journalIds)
        {
            var result = new Dictionary<int, string>();
            var allJournals = this.MeetingContext.Journal.Where(x => x.Deleted == false).ToDictionary(x => x.JournalId);
            foreach (var id in journalIds.Distinct())
            {
                var names = new List<string>();
                int? currentId = id;
                while (currentId.HasValue && allJournals.TryGetValue(currentId.Value, out var current) && current.Name != "Root")
                {
                    names.Add(current.Name);
                    if (!current.ParentId.HasValue || current.ParentId.Value == current.JournalId)
                    {
                        break;
                    }
                    currentId = current.ParentId.Value;
                }
                if (names.Count == 0 && currentId.HasValue && allJournals.TryGetValue(currentId.Value, out var rootNode))
                {
                    names.Add(rootNode.Name);
                }
                names.Reverse();
                result[id] = string.Join(" / ", names);
            }
            return result;
        }
    }
}
