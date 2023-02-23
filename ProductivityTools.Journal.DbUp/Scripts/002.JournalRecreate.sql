
CREATE TABLE [j].[Journal](
	[JournalId] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Journal] PRIMARY KEY ([JournalId])
 )

ALTER TABLE [j].[Journal] ADD  CONSTRAINT [DF_Journal_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [j].[Journal]  WITH CHECK ADD  CONSTRAINT [FK_Journal_Journal] FOREIGN KEY([ParentId])
REFERENCES [j].[Journal] ([JournalId])
GO

ALTER TABLE [j].[Journal] CHECK CONSTRAINT [FK_Journal_Journal]
GO





CREATE TABLE [j].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[email] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId]))
GO





CREATE TABLE [j].[JournalOwner](
	[UserId] [int] NOT NULL,
	[JournalId] [int] NOT NULL,
 CONSTRAINT [PK_JournalOwner] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[JournalId] ASC
))
GO

ALTER TABLE [j].[JournalOwner]  WITH CHECK ADD  CONSTRAINT [FK_JournalOwner_Journal] FOREIGN KEY([JournalId])
REFERENCES [j].[Journal] ([JournalId])
GO

ALTER TABLE [j].[JournalOwner]  WITH CHECK ADD  CONSTRAINT [FK_JournalOwner_UserId] FOREIGN KEY([UserId])
REFERENCES [j].[User] ([UserId])
GO




CREATE TABLE [j].[Page](
	[PageId] [int] IDENTITY(1,1) NOT NULL,
	[JournalId] [int] NOT NULL,
	[Subject] [nvarchar](200) NOT NULL,
	[Date] Date NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[ContentType] [nvarchar](8) NULL,
	[Deleted] BIT NOT NULL,
 CONSTRAINT [PK_Page] PRIMARY KEY CLUSTERED ([PageId]))

ALTER TABLE [j].[Page] WITH CHECK ADD  CONSTRAINT [FK_Page_Journal] FOREIGN KEY([JournalId])
REFERENCES [j].[Journal] ([JournalId])
GO



EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Technical type of item notes, it could be empty or Slate' , @level0type=N'SCHEMA',@level0name=N'j', @level1type=N'TABLE',@level1name=N'Page', @level2type=N'COLUMN',@level2name=N'ContentType'
GO


