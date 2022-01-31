using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(202201310001)]
    public class CreateSchema1 : Migration
    {
        public override void Down()
        {

        }

        public override void Up()
        {
            Create.Schema("j");
        }
    }
}
