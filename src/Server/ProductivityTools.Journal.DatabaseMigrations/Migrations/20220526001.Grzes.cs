using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20220526001)]
    public class GrzesValidation: Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Execute.Sql(@"update  [jl].[User] set email='grzegorz.opara@gmail.com' where email like '%opara%'");
        }
    }
}
