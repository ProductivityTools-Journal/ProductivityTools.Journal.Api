using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20220205001)]
    public class m20220205001 : Migration
    {
        public override void Down()
        {
            Delete.Table("Tree");
        }

        public override void Up()
        {
            Rename.Column("MeetingId").OnTable("JournalItem").InSchema("jl").To("JournalItemId");


            Create.Table("JournalItemNotes")
                .InSchema("jl")
                .WithColumn("JournalItemNotesId").AsInt32().Identity().PrimaryKey()
                .WithColumn("JournalItemId").AsInt32().NotNullable()
                .WithColumn("Notes").AsString().NotNullable()
                .WithColumn("Type").AsString().NotNullable();


            Create.ForeignKey("FK_JournalItemNotes_JournalItem")
                .FromTable("JournalItemNotes").InSchema("jl").ForeignColumn("JournalItemId")
                .ToTable("JournalItem").InSchema("jl").PrimaryColumn("JournalItemId");
        }
    }
}
