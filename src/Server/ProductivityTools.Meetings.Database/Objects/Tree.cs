using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.Database.Objects
{
    public class TreeNode
    {
        public int TreeId { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }

        public TreeNode Parent { get; set; }

    }
}
