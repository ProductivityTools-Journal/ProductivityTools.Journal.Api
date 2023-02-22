using ProductivityTools.Meetings.CoreObjects;
using ProductivityTools.Meetings.WpfClient.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProductivityTools.Meetings.WpfClient.Controls
{
    public partial class EditTreeNode : Window
    {
        private Journal ParentTreeNode;

        public EditTreeNode()
        {
            InitializeComponent();
        }

        public EditTreeNode(Journal treeNode) : this()
        {
            this.DataContext = new EditTreeNodeVM(treeNode);
        }
    }
}
