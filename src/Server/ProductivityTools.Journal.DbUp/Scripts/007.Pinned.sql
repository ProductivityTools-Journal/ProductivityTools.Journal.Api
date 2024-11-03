-- before that query ParentId was not null. And Root has JournalId=1 and ParentId=1
-- this created infinite reference in after quering for a tree as parent id always referenced 

  ALTER TABLE [j].[Page] ADD Pinned BIT NOT NULL DEFAULT 0
  
