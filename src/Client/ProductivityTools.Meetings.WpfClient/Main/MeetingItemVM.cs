using ProductivityTools.Meetings.CoreObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ProductivityTools.Meetings.WpfClient;
using ProductivityTools.Meetings.ClientCaller;

namespace ProductivityTools.Meetings.WpfClient.Controls.MeetingItem
{
    public class MeetingItemVM
    {
        public JournalItem Meeting { get; set; }
        public ICommand EditMeetingCommand { get; }
        public ICommand SaveMeetingCommand { get; }


        public string GG { get; set; }

        public MeetingItemVM(JournalItem meeting)
        {
            this.Meeting = meeting;
            EditMeetingCommand = new CommandHandler(EditMeetingClick, () => true);
            SaveMeetingCommand = new CommandHandler(SaveMeetingClick, () => true);
        }

        private void EditMeetingClick()
        {
            EditMeeting editMeetingWindow = new EditMeeting(Meeting);
            editMeetingWindow.ShowDialog();
        }

        private async void SaveMeetingClick()
        {
            MeetingsClient client = new MeetingsClient(null);
            await client.SaveMeeting(this.Meeting);
        }
    }
}
