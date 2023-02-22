using System;
using System.Collections.Generic;

namespace ProductivityTools.Meetings.CoreObjects
{
    public class Page
    {
        public int? PageId { get; set; }
        public int? JournalId { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }

        public string Notes { get; set; }
        public string NotesType { get; set; }
       
        /// <summary>
        /// It is used to say if the element is deleted
        /// </summary>
        public string Status { get; set; }

        //public List<JournalItemNotes> NotesList { get; set; }

        public Page()
        {
            //pw: date provider
            this.Date = DateTime.Now;
        }
    }
}
