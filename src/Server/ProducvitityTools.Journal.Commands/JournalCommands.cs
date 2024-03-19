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
        Page Save(Page meeting);
        void Update(Page meeting);
        void Delete(Page meetingId);
        int Delete(IEnumerable<int> treeIds);
    }

    public class JournalCommands : IJournalCommands
    {
        MeetingContext MeetingContext;

        public JournalCommands(MeetingContext context)
        {
            this.MeetingContext = context;
        }

        void IJournalCommands.Update(Page meeting)
        {
            MeetingContext.Pages.Attach(meeting);
            MeetingContext.Entry(meeting).State = EntityState.Modified;
            //meeting.NotesList.ForEach(x =>
            //{
            //    switch (x.Status)
            //    {
            //        case "New": MeetingContext.Entry(x).State = EntityState.Added; break;
            //        case "Deleted": MeetingContext.Entry(x).State = EntityState.Deleted; break;

            //        default:
            //            MeetingContext.Entry(x).State = EntityState.Modified;
            //            break;
            //    }
            //});


            var ChangeTracker = MeetingContext.ChangeTracker;

            var addedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added).ToList();
            var modifiedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).ToList();
            var deletedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).ToList();

            MeetingContext.SaveChanges();
        }

        Page IJournalCommands.Save(Page page)
        {
            if (page.PageId == null)
            {
                MeetingContext.Pages.Add(page);
            }
            else
            {
                MeetingContext.Pages.Attach(page);
                MeetingContext.Entry(page).State = EntityState.Modified;
            }
            var ChangeTracker = MeetingContext.ChangeTracker;

            var addedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added).ToList();
            var modifiedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).ToList();
            var deletedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).ToList();


            MeetingContext.SaveChanges();
            return page;
        }

        void IJournalCommands.Delete(Page meeting)
        {
            MeetingContext.Pages.Remove(meeting);
            MeetingContext.SaveChanges();
        }

        int IJournalCommands.Delete(IEnumerable<int> treeIds)
        {
            var meetings = MeetingContext.Pages.Where(x => treeIds.Contains(x.JournalId.Value));
            foreach (var meeting in meetings)
            {
                meeting.Deleted = true;
            };

            MeetingContext.SaveChanges();

            return meetings.Count();
        }
    }
}
