using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.CoreObjects
{
    public class MoveTreeRequest
    {
        public int SourceId {get;set;}
        public int ParentTargetId{ get; set; }
    }
}
