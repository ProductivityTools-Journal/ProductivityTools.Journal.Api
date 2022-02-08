using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20200605001)]
    public class CreateTreeTable : Migration
    {
        public override void Down()
        {
            Delete.Table("Tree");
        }

        public override void Up()
        {
            Create.Table("Tree")
                .InSchema("mt")
                .WithColumn("TreeId").AsInt32().Identity().PrimaryKey()
                .WithColumn("ParentId").AsInt32().NotNullable()
                .WithColumn("Name").AsString().NotNullable();

            Execute.Sql("INSERT INTO [mt].[Tree] (Name,ParentId) VALUES ('Root',0)");

            Execute.Sql("UPDATE [mt].[Tree] SET ParentId=(SELECT TOP 1 TreeId FROM [mt].[Tree])");

            Create.ForeignKey("FK_Tree_Tree")
                .FromTable("Tree").InSchema("mt").ForeignColumn("ParentId")
                .ToTable("Tree").InSchema("mt").PrimaryColumn("TreeId");
        }
    }
}
