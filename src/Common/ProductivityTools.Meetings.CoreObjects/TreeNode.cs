using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.CoreObjects
{
    public class TreeNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public List<TreeNode> Nodes { get; set; }

        public TreeNode(string name)
        {
            this.Name = name;
            this.Nodes = new List<TreeNode>();
        }
    }
}
