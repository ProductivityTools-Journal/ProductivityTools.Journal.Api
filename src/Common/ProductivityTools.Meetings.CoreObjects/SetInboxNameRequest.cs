using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.CoreObjects
{
    public class SetInboxNameRequest
    {
        public int JournalId { get; set; }
        public string InboxName { get; set; }
    }
}
