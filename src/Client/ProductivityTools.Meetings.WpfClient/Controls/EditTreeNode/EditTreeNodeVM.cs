using ProductivityTools.Meetings.ClientCaller;
using ProductivityTools.Meetings.CoreObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ProductivityTools.Meetings.WpfClient.Controls
{
    public class EditTreeNodeVM
    {
        private Journal ParentTreeNode;


        public ICommand AddCommand { get; set; }
        public string NewTreeNodeName { get; set; }

        public EditTreeNodeVM(Journal treeNode)
        {
            this.ParentTreeNode = treeNode;
            this.AddCommand = new CommandHandler(Add, () => true);
        }

        private async void Add()
        {
            MeetingsClient client = new MeetingsClient(null);
            await client.NewTreeNode(this.ParentTreeNode.Id, this.NewTreeNodeName);
        }
    }
}
