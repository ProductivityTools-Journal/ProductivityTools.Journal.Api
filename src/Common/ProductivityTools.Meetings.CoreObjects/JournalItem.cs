using System;
using System.Collections.Generic;

namespace ProductivityTools.Meetings.CoreObjects
{
    public class JournalItem
    {
        public int? JournalItemId { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
       

        public int? TreeId { get; set; }

        public List<JournalItemNotes> NotesList { get; set; }

        public JournalItem()
        {
            //pw: date provider
            this.Date = DateTime.Now;
        }
    }
}
