using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20220205003)]
    public class RenameColumns: Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Rename.Column("BeforeNotes").OnTable("JournalItem").InSchema("jl").To("BeforeNotes_old");
            Rename.Column("AfterNotes").OnTable("JournalItem").InSchema("jl").To("AfterNotes_old");
            Rename.Column("DuringNotes").OnTable("JournalItem").InSchema("jl").To("DuringNotes_old");
        }
    }
}
