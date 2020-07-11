﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.CoreObjects
{
    public class NewTreeNodeRequest
    {
        public string Name { get; set; }
        public int Parent { get; set; }

        public NewTreeNodeRequest() { }

        public NewTreeNodeRequest(int parent, string name)
        {
            this.Name = name;
            this.Parent = parent;
        }
    }
}
