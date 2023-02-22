using ProductivityTools.Meetings.CoreObjects;
using ProductivityTools.Meetings.WpfClient.Controls.MeetingItem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProductivityTools.Meetings.WpfClient.Controls
{
    public partial class EditMeeting : Window
    {
        public EditMeeting()
        {
            InitializeComponent();
        }

        public EditMeeting(CoreObjects.Page meeting) : this()
        {
            this.DataContext = new EditMeetingVM(meeting);
        }
    }
}
