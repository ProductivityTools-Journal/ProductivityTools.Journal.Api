using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductivityTools.Meetings.Database;
using ProductivityTools.Meetings.Database.Objects;

namespace ProducvitityTools.Meetings.Commands
{
    public interface IMeetingCommands
    {
        int Save(JournalItem meeting);
        void Update(JournalItem meeting);
        void Delete(JournalItem meetingId);
        int Delete(IEnumerable<int> treeIds);
    }

    public class MeetingCommands : IMeetingCommands
    {
        MeetingContext MeetingContext;

        public MeetingCommands(MeetingContext context)
        {
            this.MeetingContext = context;
        }

        void IMeetingCommands.Update(JournalItem meeting)
        {
            MeetingContext.JournalItem.Attach(meeting);
            MeetingContext.Entry(meeting).State = EntityState.Modified;
            meeting.NotesList.ForEach(x =>
            {
                if (x.JournalItemNotesId == 0)
                {
                    MeetingContext.Entry(x).State = EntityState.Added;
                }
                else
                {
                    MeetingContext.Entry(x).State = EntityState.Modified;
                }
            });
            

            var ChangeTracker = MeetingContext.ChangeTracker;

            var addedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added).ToList();
            var modifiedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).ToList();
            var deletedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).ToList();

            MeetingContext.SaveChanges();
        }

        int IMeetingCommands.Save(JournalItem meeting)
        {
            if (meeting.JournalItemId == null)
            {
                MeetingContext.JournalItem.Add(meeting);
            }
            else
            {
                MeetingContext.JournalItem.Attach(meeting);
                MeetingContext.Entry(meeting).State = EntityState.Modified;
            }
            var ChangeTracker = MeetingContext.ChangeTracker;

            var addedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added).ToList();
            var modifiedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).ToList();
            var deletedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).ToList();


            MeetingContext.SaveChanges();
            return meeting.JournalItemId.Value;
        }

        void IMeetingCommands.Delete(JournalItem meeting)
        {
            MeetingContext.JournalItem.Remove(meeting);
            MeetingContext.SaveChanges();
        }

        int IMeetingCommands.Delete(IEnumerable<int> treeIds)
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
