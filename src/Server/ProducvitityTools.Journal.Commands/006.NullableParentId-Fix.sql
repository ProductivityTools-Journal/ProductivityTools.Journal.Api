-- before that query ParentId was not null. And Root has JournalId=1 and ParentId=1
-- this created infinite reference in after quering for a tree as parent id always referenced 

  ALTER TABLE [j].[Journal] ALTER COLUMN ParentId INT NULL
  UPDATE [j].[Journal]  SET ParentId=NULL WHERE JournalId=1 and ParentId=1
