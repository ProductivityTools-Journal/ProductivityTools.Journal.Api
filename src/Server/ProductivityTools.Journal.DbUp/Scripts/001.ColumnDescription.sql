IF OBJECT_ID ('dbo.usp_addorupdatedescription', 'P') IS NOT NULL
                DROP PROCEDURE dbo.usp_addorupdatedescription;
            GO

            CREATE PROCEDURE usp_addorupdatedescription
            @table nvarchar(128),  --table name
            @column nvarchar(128), --column name, NULL if description for table
            @descr sql_variant-- description text
            AS
            BEGIN
        
            SET NOCOUNT ON;

            DECLARE @c nvarchar(128) = NULL;

            IF @column IS NOT NULL
                SET @c = N'COLUMN';

            BEGIN TRY
            EXECUTE sp_updateextendedproperty  N'MS_Description', @descr, N'SCHEMA', N'jl', N'TABLE', @table, @c, @column;
                END TRY
            BEGIN CATCH
            EXECUTE sp_addextendedproperty N'MS_Description', @descr, N'SCHEMA', N'jl', N'TABLE', @table, @c, @column;
            END CATCH;
            END
            GO