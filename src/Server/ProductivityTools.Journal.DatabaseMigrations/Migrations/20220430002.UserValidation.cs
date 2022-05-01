using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.DatabaseMigrations.Migrations
{
    [Migration(20220430004)]
    public class USerValidation: Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Execute.Sql(@"CREATE TYPE [jl].TreeArray AS TABLE(TreeId INT NOT NULL)");
            Execute.Sql(@"CREATE FUNCTION [jl].GetTreePath (@TreeId INT)
                            RETURNS @TreePath TABLE (TreeId INT)
                            AS
                            BEGIN
	                            DECLARE @ParentId INT
	                            SELECT @TreeId=[TreeId], @ParentId=ParentId FROM [jl].[Tree] WHERE TreeId=@TreeId	
	                            WHILE (@TreeId!=@ParentId)
	                            BEGIN
		                            INSERT @TreePath(TreeId) VALUES(@TreeId)
		                            SELECT @TreeId=[TreeId], @ParentId=ParentID FROM [jl].[Tree] WHERE TreeId=@ParentId
	                            END
	                            INSERT @TreePath(TreeId) VALUES(@TreeId)
	                            RETURN;
                            END
                            ");
            Execute.Sql(@"CREATE PROCEDURE jl.VerifyOwnership 
							@email VARCHAR(100),
							@TreeIds [jl].[TreeArray] READONLY,
							@HasPermission BIT OUTPUT
						AS
						BEGIN
							SET @HasPermission=1
	
							DECLARE @TreeIdsTemp [jl].[TreeArray]
							INSERT @TreeIdsTemp(TreeId)
							SELECT TreeId from @TreeIds

							DECLARE @TreeId INT
							DECLARE @recordCount INT
							WHILE exists (Select * From @TreeIdsTemp)
							BEGIN

								SELECT @TreeId = MIN(TreeId) from @TreeIdsTemp
								DELETE from @TreeIdsTemp Where TreeID = @TreeId

								select @recordCount=COUNT(1) from [jl].Tree T
								inner join [jl].TreeOwner [TO] on T.TreeId=[TO].TreeId
								inner join [jl].[User] [U] ON U.UserId=[TO].UserId
								inner join [jl].GetTreePath(@TreeId) TP ON TP.TreeId=T.TreeId
								WHERE U.email=@email

								IF (@recordCount=0)
								BEGIN
									SET @HasPermission=0
									RETURN
								END
							END
							RETURN 1
						END;
						GO
						");
        }
    }
}
