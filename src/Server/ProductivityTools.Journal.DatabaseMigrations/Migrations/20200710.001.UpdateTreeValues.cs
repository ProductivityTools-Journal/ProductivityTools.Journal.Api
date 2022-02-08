using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(202007010001)]
    public class UpdateTreeValues : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Execute.Sql("UPDATE [PTMeetings].[mt].[Meeting] SET TreeId=1");
            Alter.Table("Meeting")
                .InSchema("mt").AlterColumn("TreeId").AsInt32();
        }
    }
}
