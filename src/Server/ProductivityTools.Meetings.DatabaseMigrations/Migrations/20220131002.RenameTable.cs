using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(202201310002)]
    public class RenameTable : Migration
    {
        public override void Down()
        {

        }

        public override void Up()
        {
            Alter.Table("Meeting").InSchema("mt").ToSchema("j");
            Rename.Table("Meeting").InSchema("j").To("JournalItem");
        }
    }
}
