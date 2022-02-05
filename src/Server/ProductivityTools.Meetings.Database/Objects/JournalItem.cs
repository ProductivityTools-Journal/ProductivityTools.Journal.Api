using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.Database.Objects
{

    public class JournalItem
    {
        public int? JournalItemId { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        //public string BeforeNotes { get; set; }
        //public string DuringNotes { get; set; }
        //public string AfterNotes { get; set; }
        public int? TreeId { get; set; }
        public bool Deleted { get; set; }

        public List<JournalItemNotes> Notes { get; set; }
    }
}
