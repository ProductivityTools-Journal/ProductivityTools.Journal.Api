using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20220425004)]
    public class AddDescriptoinType: Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Execute.Sql(@"usp_addorupdatedescription N'JournalItemNotes', N'Type', N'It is a type of notes, it could be for example email, before meeting notes, during notes'");
        }
    }
}
