ALTER TABLE [j].[Journal] ADD [Archived] BIT NOT NULL CONSTRAINT [DF_Journal_Archived] DEFAULT ((0))
GO
