using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20220131004)]
    public class RenameTable1 : Migration
    {
        public override void Down()
        {

        }

        public override void Up()
        {
            Alter.Table("JournalItem").InSchema("j").ToSchema("jl");
        }
    }
}
