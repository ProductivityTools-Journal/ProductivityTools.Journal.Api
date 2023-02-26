CREATE TYPE [j].TreeArray AS TABLE(TreeId INT NOT NULL)
GO
CREATE FUNCTION [j].GetTreePath (@TreeId INT)
                            RETURNS @TreePath TABLE (TreeId INT)
                            AS
                            BEGIN
	                            DECLARE @ParentId INT
	                            SELECT @TreeId=JournalId, @ParentId=ParentId FROM [j].[Journal] WHERE JournalId=@TreeId	
	                            WHILE (@TreeId!=@ParentId)
	                            BEGIN
		                            INSERT @TreePath(TreeId) VALUES(@TreeId)
		                            SELECT @TreeId=[JournalId], @ParentId=ParentID FROM [j].[Journal] WHERE [JournalId]=@ParentId
	                            END
	                            INSERT @TreePath(TreeId) VALUES(@TreeId)
	                            RETURN;
                            END
GO;
                            
CREATE PROCEDURE j.VerifyOwnership 
							@email VARCHAR(100),
							@TreeIds [j].[TreeArray] READONLY,
							@HasPermission BIT OUTPUT
						AS
						BEGIN
							SET @HasPermission=1
	
							DECLARE @TreeIdsTemp [j].[TreeArray]
							INSERT @TreeIdsTemp(TreeId)
							SELECT TreeId from @TreeIds

							DECLARE @TreeId INT
							DECLARE @recordCount INT
							WHILE exists (Select * From @TreeIdsTemp)
							BEGIN

								SELECT @TreeId = MIN(TreeId) from @TreeIdsTemp
								DELETE from @TreeIdsTemp Where TreeID = @TreeId

								select @recordCount=COUNT(1) from [j].Journal J
								inner join [j].JournalOwner [JO] on J.JournalId=[JO].JournalId
								inner join [j].[User] [U] ON U.UserId=[JO].UserId
								inner join [j].GetTreePath(@TreeId) TP ON TP.TreeId=J.JournalId
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