CREATE OR ALTER PROCEDURE j.VerifyOwnership 
    @email VARCHAR(100),
    @TreeIds [j].[TreeArray] READONLY,
    @HasPermission BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId INT;
    SELECT @UserId = UserId FROM [j].[User] WHERE email = @email;

    IF @UserId IS NULL
    BEGIN
        SET @HasPermission = 0;
        RETURN;
    END;

    ;WITH AncestorTree AS (
        SELECT t.TreeId AS RequestedTreeId, j.JournalId, j.ParentId
        FROM @TreeIds t
        INNER JOIN [j].[Journal] j ON t.TreeId = j.JournalId
        WHERE j.Deleted = 0
        
        UNION ALL
        
        SELECT a.RequestedTreeId, parent.JournalId, parent.ParentId
        FROM [j].[Journal] parent
        INNER JOIN AncestorTree a ON parent.JournalId = a.ParentId
        WHERE parent.JournalId != a.JournalId AND parent.Deleted = 0
    ),
    PermittedRequestedIds AS (
        SELECT DISTINCT a.RequestedTreeId
        FROM AncestorTree a
        INNER JOIN [j].[JournalOwner] jo ON a.JournalId = jo.JournalId
        WHERE jo.UserId = @UserId
    )
    SELECT @HasPermission = CASE 
        WHEN NOT EXISTS (
            SELECT 1 FROM @TreeIds t
            WHERE t.TreeId NOT IN (SELECT RequestedTreeId FROM PermittedRequestedIds)
        ) THEN 1
        ELSE 0
    END;
END;
GO
