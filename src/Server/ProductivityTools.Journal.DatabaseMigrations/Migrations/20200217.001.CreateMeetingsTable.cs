using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20200217)]
    public class CreateMeetingsTable : Migration
    {
        public override void Down()
        {
            Delete.Table("Meetings");
        }

        public override void Up()
        {
            Create.Schema("mt");

            Create.Table("Meeting")
                .InSchema("mt")
                .WithColumn("MeetingId").AsInt32().Identity().PrimaryKey()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("Subject").AsString(200).NotNullable()
                .WithColumn("BeforeNotes").AsString(20000).Nullable()
                .WithColumn("DuringNotes").AsString(20000).Nullable()
                .WithColumn("AfterNotes").AsString(20000).Nullable();
        }
    }
}
