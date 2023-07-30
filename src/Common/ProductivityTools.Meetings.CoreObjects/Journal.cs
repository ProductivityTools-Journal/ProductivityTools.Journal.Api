using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.CoreObjects
{
    public class Journal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        //not always filled
        public Journal Parent { get; set; }
        public List<Journal> Nodes { get; set; }

        public Journal(string name)
        {
            this.Name = name;
            this.Nodes = new List<Journal>();
        }
    }
}
