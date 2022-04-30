using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20220430001)]
    public class Users : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Alter.Table("Tree").InSchema("mt").ToSchema("jl");

            Create.Table("User")
                .InSchema("jl")
                .WithColumn("UserId").AsInt32().Identity().PrimaryKey()
                .WithColumn("email").AsString().NotNullable();


            Execute.Sql("INSERT INTO [jl].[User] (email) VALUES ('pwujczyk@gmail.com')");
            Execute.Sql("INSERT INTO [jl].[User] (email) VALUES ('gopara@gmail.com')");

            Create.Table("TreeOwners")
                .InSchema("jl")
                .WithColumn("UserId").AsInt32().Identity()
                .WithColumn("TreeId").AsInt32().NotNullable();

            Create.PrimaryKey("PK_TreeOwners").OnTable("TreeOwners").WithSchema("jl").Columns(new[] { "UserId", "TreeId" });

            Create.ForeignKey("FK_TreeOnwers_Tree")
                .FromTable("TreeOwners").InSchema("jl").ForeignColumn("TreeId")
                .ToTable("Tree").InSchema("jl").PrimaryColumn("TreeId");

            Create.ForeignKey("FK_TreeOnwers_UserId")
                .FromTable("TreeOwners").InSchema("jl").ForeignColumn("UserId")
                .ToTable("User").InSchema("jl").PrimaryColumn("UserId");
        }
    }
}
