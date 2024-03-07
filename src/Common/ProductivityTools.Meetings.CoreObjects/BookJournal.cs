using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.CoreObjects
{
    public class BookJournal
    {
        public int ParentJournalId { get; set; }
        public string JournalName { get; set; }
        public Page Page { get; set; }
    }
}
