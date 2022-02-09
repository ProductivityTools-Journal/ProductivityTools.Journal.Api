using System;

namespace ProductivityTools.Meetings.CoreObjects
{
    public class JournalItemNotes
    {
        public int? JournalItemNotesId { get; set; }
        public string Notes{ get; set; }
        public string Type { get; set; }

        /// <summary>
        /// It is used to say if the element is deleted
        /// </summary>
        public string Status { get; set; }
    }
}
