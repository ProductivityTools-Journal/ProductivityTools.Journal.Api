﻿using System;
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

namespace ProductivityTools.Meetings.WpfClient
{
    /// <summary>
    /// Interaction logic for EditMeeting.xaml
    /// </summary>
    public partial class EditMeeting : Window
    {
        MeetingVM meeting;
        public EditMeeting()
        {
            InitializeComponent();
        }

        public EditMeeting(MeetingVM meeting) : this()
        {
            this.meeting = meeting;
            this.DataContext = this.meeting;
        }
    }
}
