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
        int Save(Meeting meeting);
        void Update(Meeting meeting);
        void Delete(Meeting meetingId);
        int Delete(IEnumerable<int> treeIds);
    }

    public class MeetingCommands : IMeetingCommands
    {
        MeetingContext MeetingContext;

        public MeetingCommands(MeetingContext context)
        {
            this.MeetingContext = context;
        }

        void IMeetingCommands.Update(Meeting meeting)
        {//pw: not used
            MeetingContext.Meeting.Attach(meeting);
            MeetingContext.Entry(meeting).State = EntityState.Modified;

            var ChangeTracker = MeetingContext.ChangeTracker;

            var addedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added).ToList();
            var modifiedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).ToList();
            var deletedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).ToList();

            MeetingContext.SaveChanges();
        }

        int IMeetingCommands.Save(Meeting meeting)
        {
            if (meeting.MeetingId == null)
            {
                MeetingContext.Meeting.Add(meeting);
            }
            else
            {
                MeetingContext.Meeting.Attach(meeting);
                MeetingContext.Entry(meeting).State = EntityState.Modified;
            }
            var ChangeTracker = MeetingContext.ChangeTracker;

            var addedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added).ToList();
            var modifiedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).ToList();
            var deletedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).ToList();


            MeetingContext.SaveChanges();
            return meeting.MeetingId.Value;
        }

        void IMeetingCommands.Delete(Meeting meeting)
        {
            MeetingContext.Meeting.Remove(meeting);
            MeetingContext.SaveChanges();
        }

        int IMeetingCommands.Delete(IEnumerable<int> treeIds)
        {
            var meetings = MeetingContext.Meeting.Where(x => treeIds.Contains(x.TreeId.Value));
            foreach (var meeting in meetings)
            {
                meeting.Deleted = true;
            };

            MeetingContext.SaveChanges();

            return meetings.Count();
        }
    }
}
