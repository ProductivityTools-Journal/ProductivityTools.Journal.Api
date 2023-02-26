
SET IDENTITY_INSERT PTJournal.j.[User] on
INSERT INTO PTJournal.j.[User](UserId,email)
SELECT  [UserId],[email]
FROM [PTMeetings].[jl].[User]
SET IDENTITY_INSERT PTJournal.j.[User] off


SET IDENTITY_INSERT PTJournal.j.[Journal] on
INSERT INTO PTJournal.j.[Journal]([JournalId],[ParentId],[Name],[Deleted])
SELECT  [TreeId],[ParentId],[Name],[Deleted]
FROM [PTMeetings].[jl].[Tree]
SET IDENTITY_INSERT PTJournal.j.[Journal] OFF
