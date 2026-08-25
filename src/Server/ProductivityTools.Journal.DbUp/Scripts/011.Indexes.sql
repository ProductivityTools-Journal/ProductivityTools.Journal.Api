-- 1. Composite index for Page list queries (filters on JournalId and Deleted, orders by Pinned DESC, Date DESC)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Page_JournalId_Deleted_Pinned_Date' AND object_id = OBJECT_ID('[j].[Page]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Page_JournalId_Deleted_Pinned_Date 
    ON [j].[Page] (JournalId, Deleted, Pinned, Date DESC);
END;
GO

-- 2. Index on User email for rapid ownership resolution
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_User_Email' AND object_id = OBJECT_ID('[j].[User]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX IX_User_Email 
    ON [j].[User] (email);
END;
GO

-- 3. Index on Journal for hierarchy traversal
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Journal_ParentId_Deleted' AND object_id = OBJECT_ID('[j].[Journal]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Journal_ParentId_Deleted 
    ON [j].[Journal] (ParentId, Deleted);
END;
GO

-- 4. Filtered index for fast Public Hash lookups on Page
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Page_PublicHash' AND object_id = OBJECT_ID('[j].[Page]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Page_PublicHash 
    ON [j].[Page] (PublicHash) 
    WHERE PublicHash IS NOT NULL AND Deleted = 0;
END;
GO

-- 5. Filtered index for fast Public Hash lookups on Journal
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Journal_PublicHash' AND object_id = OBJECT_ID('[j].[Journal]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Journal_PublicHash 
    ON [j].[Journal] (PublicHash) 
    WHERE PublicHash IS NOT NULL AND Deleted = 0;
END;
GO
