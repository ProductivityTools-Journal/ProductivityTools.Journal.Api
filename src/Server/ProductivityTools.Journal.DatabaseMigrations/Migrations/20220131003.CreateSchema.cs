using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20220131003)]
    public class CreateSchema : Migration
    {
        public override void Down()
        {

        }

        public override void Up()
        {
            Create.Schema("jl");
        }
    }
}
