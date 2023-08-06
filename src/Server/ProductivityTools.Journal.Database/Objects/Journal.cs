using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProductivityTools.Meetings.Database.Objects
{
    public class Journal
    {
        public int JournalId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public Journal Parent { get; set; }

    }
}
