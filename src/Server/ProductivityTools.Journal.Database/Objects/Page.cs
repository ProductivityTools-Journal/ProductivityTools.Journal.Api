using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.Database.Objects
{

    public class Page
    {
        public int? PageId { get; set; }
        public int? JournalId { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }

        public string Notes { get; set; }
        public string NotesType { get; set; }
        public bool Deleted { get; set; }

       // public List<JournalItemNotes> NotesList { get; set; }
    }
}
