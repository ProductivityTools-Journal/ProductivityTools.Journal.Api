using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductivityTools.Meetings.Database;
using ProductivityTools.Meetings.Database.Objects;

namespace ProducvitityTools.Meetings.Commands
{
    public interface IJournalCommands
    {
        JournalItem Save(JournalItem meeting);
        void Update(JournalItem meeting);
        void Delete(JournalItem meetingId);
        int Delete(IEnumerable<int> treeIds);
    }

    public class MeetingCommands : IJournalCommands
    {
        MeetingContext MeetingContext;

        public MeetingCommands(MeetingContext context)
        {
            this.MeetingContext = context;
        }

        void IJournalCommands.Update(JournalItem meeting)
        {
            MeetingContext.JournalItem.Attach(meeting);
            MeetingContext.Entry(meeting).State = EntityState.Modified;
            meeting.NotesList.ForEach(x =>
            {
                switch (x.Status)
                {
                    case "New": MeetingContext.Entry(x).State = EntityState.Added; break;
                    case "Deleted": MeetingContext.Entry(x).State = EntityState.Deleted; break;

                    default:
                        MeetingContext.Entry(x).State = EntityState.Modified;
                        break;
                }
            });


            var ChangeTracker = MeetingContext.ChangeTracker;

            var addedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added).ToList();
            var modifiedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).ToList();
            var deletedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).ToList();

            MeetingContext.SaveChanges();
        }

        JournalItem IJournalCommands.Save(JournalItem journal)
        {
            if (journal.JournalItemId == null)
            {
                MeetingContext.JournalItem.Add(journal);
            }
            else
            {
                MeetingContext.JournalItem.Attach(journal);
                MeetingContext.Entry(journal).State = EntityState.Modified;
            }
            var ChangeTracker = MeetingContext.ChangeTracker;

            var addedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added).ToList();
            var modifiedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).ToList();
            var deletedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).ToList();


            MeetingContext.SaveChanges();
            return journal;
        }

        void IJournalCommands.Delete(JournalItem meeting)
        {
            MeetingContext.JournalItem.Remove(meeting);
            MeetingContext.SaveChanges();
        }

        int IJournalCommands.Delete(IEnumerable<int> treeIds)
        {
            var meetings = MeetingContext.JournalItem.Where(x => treeIds.Contains(x.TreeId.Value));
            foreach (var meeting in meetings)
            {
                meeting.Deleted = true;
            };

            MeetingContext.SaveChanges();

            return meetings.Count();
        }
    }
}
