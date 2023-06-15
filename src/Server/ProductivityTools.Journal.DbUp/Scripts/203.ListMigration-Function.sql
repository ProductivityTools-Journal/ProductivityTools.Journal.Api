CREATE FUNCTION UpdateList(@value AS NVARCHAR(Max))
RETURNS NVARCHAR(Max)
AS
BEGIN
DECLARE @ROW NVARCHAR(Max)
--SET @ROW = '[{"type":"title","children":[{"text":"Customer Affinity Routes"}]},{"type":"paragraph","children":[{"text":""},{"type":"link","href":"https://docs.gdit#","children":[{"text":"document"}]},{"text":""}]},{"type":"unorderedList","children":[{"type":"list-item","children":[{"text":"Kiedyś affinity routes przekazywało casa do TSE który posiadał doświadczenie z klientem"}]},{"type":"list-item","children":[{"text":"Przejście na vectora popsuło affinity"}]},{"type":"list-item","children":[{"text":"od 2022.12.06 w wektorze został naprawiony affinity routing i teraz następuje nauka"}]},{"type":"list-item","children":[{"text":"Complexity routing również wpłynął na affinity routing, ponieważ zmieniła się liczba TSG i TSR"}]},{"type":"list-item","children":[{"text":"Affintiy routing bazuje na parze customerId,TSGId"}]},{"type":"list-item","children":[{"text":"Propozycja jest aby wprowadzić bardzo granualne treningi w depuuty (appengine zamiast serverless)"}]},{"type":"list-item","children":[{"text":"Propozycja jest aby z kluczowymi klientami pracowali tylko najbardziej tenured ludzie"}]}]},{"type":"paragraph","children":[{"text":"Dokument opisuje jeszcz dużo analizy i mówi dużo o affinity routing. "}]}]'
SET @ROW = @value

DECLARE @SEARCHPHRASE NVARCHAR(Max)
SET @SEARCHPHRASE = '"type":"list-item","children":'

DECLARE @LISTITEMPOS INT
SELECT @LISTITEMPOS=CHARINDEX(@SEARCHPHRASE, @ROW)

DECLARE @NEWFORMAT VARCHAR(200)
SET @NEWFORMAT = '"type":"li","children":[{"type":"lic","children":'

--PRINT @LISTITEMPOS
IF @LISTITEMPOS>0
	BEGIN
		DECLARE @BEFORE NVARCHAR(Max)
		SET @BEFORE = SUBSTRING(@ROW, 0, @LISTITEMPOS)
        --PRINT @BEFORE
		SET @BEFORE=@BEFORE+@NEWFORMAT
		--PRINT @BEFORE

		DECLARE @CLOSINGELEMENT INT
		SELECT @CLOSINGELEMENT = CHARINDEX('"}]}', @ROW, @LISTITEMPOS+LEN(@SEARCHPHRASE))
		--PRINT @CLOSINGELEMENT

        DECLARE @CONTENT NVARCHAR(Max)
		SELECT @CONTENT = SUBSTRING(@ROW, @LISTITEMPOS + LEN(@SEARCHPHRASE), 4 + @CLOSINGELEMENT - (@LISTITEMPOS + LEN(@SEARCHPHRASE)))
        --PRINT @CONTENT

		SET @BEFORE=@BEFORE+@CONTENT+']}'

		DECLARE @RESTDOCUMENT NVARCHAR(Max)
		SELECT @RESTDOCUMENT = SUBSTRING(@ROW, @CLOSINGELEMENT + 4, LEN(@ROW) - @CLOSINGELEMENT - 3)
        --PRINT 'RESTDOCUMENT'
		--PRINT @RESTDOCUMENT

		SET @BEFORE=@BEFORE+@RESTDOCUMENT
		--PRINT @BEFORE
		RETURN @BEFORE
	END
RETURN @ROW
end



	 -- Update [PTJournal].[j].[Page] set Content = REPLACE(Content, '"type":"list-item","children":', '"type":"li","children":[{"type":"lic","children":') where PageId=2993