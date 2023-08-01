# ProductivityTools.Journal.Api
Group of application which helps to write notes from meetings 

[Column Descriptions](docs/ColumnDescription.MD)



## User validation

In each method which returns information to database application made additionall call which validates if user is owner of the particular tree in which journal item exists. If user owns the parent tree the condition is also meet. 

**jl.VerifyOwnership**

SQL function takes paramters
- email
- TreeIds - it is a list of TreeIds for which journalItems should be returned

In function validation creates copy of the TreeIds table and iterate through it (deleting records after processing).

Foreach record it perform select which validates if particular TreeId or any of the parent of this TreeId (GetTreePath) is added to the TreeOwner table.

```SQL
CREATE FUNCTION jl.VerifyOwnership (@email VARCHAR(100),@TreeIds [jl].[TreeArray] READONLY)
RETURNS BIT
AS
BEGIN
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
		inner join [jl].TreeOwners [TO] on T.TreeId=[TO].TreeId
		inner join [jl].[User] [U] ON U.UserId=[TO].UserId
		inner join [jl].GetTreePath(@TreeId) TP ON TP.TreeId=T.TreeId
		WHERE U.email=@email

		IF (@recordCount=0) RETURN 0
	END
	RETURN 1
END;
GO
```

In function custom type is used

```SQL
CREATE TYPE [jl].TreeArray AS TABLE(TreeId INT NOT NULL)
```

**GetTreePath**

Function for given TreeId returns all parents until root node.
```SQL
CREATE FUNCTION [jl].GetTreePath (@TreeId INT)
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
```
