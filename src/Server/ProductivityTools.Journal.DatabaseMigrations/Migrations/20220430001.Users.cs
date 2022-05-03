using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20220430002)]
    public class Users1 : Migration
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

            Create.Table("TreeOwner")
                .InSchema("jl")
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("TreeId").AsInt32().NotNullable();

            Create.PrimaryKey("PK_TreeOwner").OnTable("TreeOwner").WithSchema("jl").Columns(new[] { "UserId", "TreeId" });

            Create.ForeignKey("FK_TreeOnwers_Tree")
                .FromTable("TreeOwner").InSchema("jl").ForeignColumn("TreeId")
                .ToTable("Tree").InSchema("jl").PrimaryColumn("TreeId");

            Create.ForeignKey("FK_TreeOnwers_UserId")
                .FromTable("TreeOwner").InSchema("jl").ForeignColumn("UserId")
                .ToTable("User").InSchema("jl").PrimaryColumn("UserId");
        }
    }
}
