using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.CoreObjects
{
    public class MeetingListRequest
    {
        public int? Id { get; set; }
        public bool DrillDown { get; set; }
        public bool FullSync { get; set; }
    }
}
