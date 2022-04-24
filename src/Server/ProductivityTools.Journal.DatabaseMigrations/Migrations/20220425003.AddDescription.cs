using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20220425003)]
    public class AddDescriptoin: Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Execute.Sql(@"usp_addorupdatedescription N'JournalItemNotes', N'NotesType', N'Technical type of item notes, it could be empty or Slate' ");
        }
    }
}
