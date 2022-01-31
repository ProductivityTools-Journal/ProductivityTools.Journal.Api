using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(202201310005)]
    public class DeleteSchema : Migration
    {
        public override void Down()
        {

        }

        public override void Up()
        {
            Delete.Schema("j");

        }
    }
}
