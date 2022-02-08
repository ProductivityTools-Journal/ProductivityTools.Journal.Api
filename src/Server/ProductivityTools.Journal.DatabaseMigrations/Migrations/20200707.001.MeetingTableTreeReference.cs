using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20200707001)]
    public class MeetingTableTreeReferenc : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Alter.Table("Meeting")
                .InSchema("mt").AddColumn("TreeId").AsInt32().Nullable();
            Create.ForeignKey().FromTable("Meeting").InSchema("mt").ForeignColumn("TreeId")
                .ToTable("Tree").InSchema("mt").PrimaryColumn("TreeId");
        }
    }
}
