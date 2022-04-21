using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20220425001)]
    public class AddDetailsType: Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Alter.Table("JournalItemNotes")
                     .InSchema("jl").AddColumn("NotesType").AsString(8).Nullable();
        }
    }
}
