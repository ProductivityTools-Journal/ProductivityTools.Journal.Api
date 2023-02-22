using ProductivityTools.Meetings.ClientCaller;
using ProductivityTools.Meetings.CoreObjects;
using ProductivityTools.Meetings.WpfClient.Controls.MeetingItem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ProductivityTools.Meetings.WpfClient.Controls
{
    public class EditMeetingVM
    {
        public Page Meeting { get; set; }
        public ICommand SaveMeetingCommand { get; }
        public ICommand DeleteMeetingCommand { get; }

        public EditMeetingVM(Page meeting)
        {
            this.Meeting = meeting;
            SaveMeetingCommand = new CommandHandler(SaveMeetingClick, () => true);
            DeleteMeetingCommand = new CommandHandler(DeleteMeetingClick, () => true);
        }

        private async void SaveMeetingClick()
        {
            MeetingsClient client = new MeetingsClient(null);
            int meetingId=await client.SaveMeeting(this.Meeting);
            this.Meeting.PageId = meetingId;
        }

        private async void DeleteMeetingClick()
        {
            MeetingsClient client = new MeetingsClient(null);
            await client.DeleteMeeting(new JournalId() { Id = this.Meeting.PageId });
        }
    }
}
