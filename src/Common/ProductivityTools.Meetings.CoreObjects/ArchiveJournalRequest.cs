using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.CoreObjects
{
    public class ArchiveJournalRequest
    {
        public int JournalId { get; set; }
        public bool Archived { get; set; } = true;
    }
}
