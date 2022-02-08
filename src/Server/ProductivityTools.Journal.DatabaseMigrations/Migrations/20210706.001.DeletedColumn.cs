using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20210706001)]
    public class DeleteColumn: Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Alter.Table("Meeting")
                .InSchema("mt").AddColumn("Deleted").AsBoolean().WithDefaultValue(false).NotNullable();

            Alter.Table("Tree")
                .InSchema("mt").AddColumn("Deleted").AsBoolean().WithDefaultValue(false).NotNullable();
        }
    }
}
