using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.Database.Objects
{

    public class JournalItemNotes
    {
        public int JournalItemNotesId { get; set; }
        public string Notes { get; set; }
        public string Type { get; set; }

        public int JournalItemId { get; set; }
        public JournalItem JournalItem { get; set; }

    }
}
