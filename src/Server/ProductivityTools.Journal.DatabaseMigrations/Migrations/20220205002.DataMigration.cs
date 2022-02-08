using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20220205002)]
    public class DataMigration: Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Execute.Sql(@"
  
                with tab as(
                    select JournalItemId, BeforeNotes as Notes,'Before' as Type, Date from jl.JournalItem
                    where BeforeNotes is not null
                        union all
                    select JournalItemId, DuringNotes as Notes,'During' as Type, Date from jl.JournalItem
                    where DuringNotes is not null
                        union all
                    select JournalItemId, AfterNotes as Notes,'After' as Type, Date from jl.JournalItem
                    where AfterNotes is not null
                )
                INSERT INTO [jl].[JournalItemNotes](JournalItemId,Notes,[Type])
                SELECT JournalItemId,Notes,Type from tab order by Date asc");
        }
    }
}
