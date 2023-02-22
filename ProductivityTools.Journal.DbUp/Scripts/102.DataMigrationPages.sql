
insert into [PTJournal].[j].[Page]([JournalId],[Subject],[Date],[Notes],[NotesType],Deleted)
SELECT journal.TreeId,journal.[Subject],journal.[Date],notes.Notes,notes.[NotesType],journal.Deleted 
FROM [PTMeetings].[jl].[JournalItemNotes] notes
inner join  [PTMeetings].[jl].[JournalItem] journal on notes.JournalItemId=journal.JournalItemId