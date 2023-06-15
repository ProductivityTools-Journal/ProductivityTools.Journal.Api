Update [PTJournal].[j].[Page] set Content=REPLACE(Content,'"type":"link","href":','"type":"a","url":') 
  Update [PTJournal].[j].[Page] set Content=REPLACE(Content,'"type":"paragraph"','"type":"p"') 

  Update [PTJournal].[j].[Page] set Content=REPLACE(Content,'"type":"headingOne"','"type":"h1"') 
  Update [PTJournal].[j].[Page] set Content=REPLACE(Content,'"type":"headingTwo"','"type":"h2"')
  Update [PTJournal].[j].[Page] set Content=REPLACE(Content,'"type":"headingThree"','"type":"h3"')
  Update [PTJournal].[j].[Page] set Content=REPLACE(Content,'"type":"orderedList"','"type":"ul"')
  Update [PTJournal].[j].[Page] set Content=REPLACE(Content,'"type":"unorderedList"','"type":"ul"')

  update [PTJournal].[j].[Page] set Content=dbo.UpdateList(Content)