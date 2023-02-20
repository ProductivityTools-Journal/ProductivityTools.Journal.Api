using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20230220001)]
    public class CreateJournalTables : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Create.Schema("j");

            Create.Table("Journal")
                          .InSchema("j")
                          .WithColumn("PageId").AsInt32().Identity().PrimaryKey()
                          .WithColumn("JournalId").AsInt32().NotNullable()
                          .WithColumn("Subject").AsString().NotNullable()
                          .WithColumn("Notes").AsString().NotNullable()
                          .WithColumn("Deleted").AsBoolean().NotNullable();

            Execute.Sql("INSERT INTO [j].[Journal] (Name,ParentId) VALUES ('Root',0)");

            Execute.Sql("UPDATE [j].[Journal] SET ParentId=(SELECT TOP 1 TreeId FROM [mt].[Tree])");

            Create.ForeignKey("FK_Journal_Journal")
                .FromTable("Journal").InSchema("j").ForeignColumn("ParentId")
                .ToTable("Journal").InSchema("j").PrimaryColumn("JournalId");


            Create.Table("User")
               .InSchema("j")
               .WithColumn("UserId").AsInt32().Identity().PrimaryKey()
               .WithColumn("email").AsString().NotNullable();


            Execute.Sql("INSERT INTO [j].[User] (email) VALUES ('pwujczyk@gmail.com')");
            Execute.Sql("INSERT INTO [j].[User] (email) VALUES ('gopara@gmail.com')");

            Create.Table("TreeOwner")
                .InSchema("j")
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("TreeId").AsInt32().NotNullable();

            Create.PrimaryKey("PK_TreeOwner").OnTable("TreeOwner").WithSchema("j").Columns(new[] { "UserId", "JournalId" });

            Create.ForeignKey("FK_TreeOnwers_Journal")
                .FromTable("TreeOwner").InSchema("j").ForeignColumn("JournalId")
                .ToTable("Tree").InSchema("j").PrimaryColumn("JournalId");

            Create.ForeignKey("FK_TreeOnwers_UserId")
                .FromTable("TreeOwner").InSchema("j").ForeignColumn("UserId")
                .ToTable("User").InSchema("j").PrimaryColumn("UserId");
        }
    }
}
