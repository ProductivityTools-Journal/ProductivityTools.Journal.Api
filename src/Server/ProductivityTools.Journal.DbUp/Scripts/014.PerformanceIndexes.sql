-- 1. Index on JournalOwner for rapid ownership lookups by UserId and JournalId
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_JournalOwner_UserId_JournalId' AND object_id = OBJECT_ID('[j].[JournalOwner]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_JournalOwner_UserId_JournalId 
    ON [j].[JournalOwner] (UserId, JournalId);
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_JournalOwner_JournalId_UserId' AND object_id = OBJECT_ID('[j].[JournalOwner]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_JournalOwner_JournalId_UserId 
    ON [j].[JournalOwner] (JournalId, UserId);
END;
GO

-- 2. Covering index on Journal for hierarchy traversal (avoids Key Lookups during recursive CTEs)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Journal_ParentId_Deleted_Archived_Covering' AND object_id = OBJECT_ID('[j].[Journal]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Journal_ParentId_Deleted_Archived_Covering 
    ON [j].[Journal] (ParentId, Deleted, Archived)
    INCLUDE (JournalId, Name, PublicHash, InboxName);
END;
GO
