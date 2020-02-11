IF EXISTS (select * from sys.objects o inner join sys.schemas s on s.schema_id = o.schema_id where s.name = 'dbo' and o.name = 'CreateErrorLogUser' and o.type = 'P') 
BEGIN
	DROP PROCEDURE [dbo].[CreateErrorLogUser]
END
GO

CREATE PROCEDURE [dbo].[CreateErrorLogUser]
	@DatabaseName NVARCHAR(100),
	@AppUser NVARCHAR(100)
AS
BEGIN
	DECLARE @SqlStatement NVARCHAR(500),
			@SchemaName NVARCHAR(100) = 'dbo'

	-- Create App Pool User

	IF NOT EXISTS (SELECT LoginName FROM SYSLOGINS WHERE NAME = @AppUser)
	BEGIN
		SET @SqlStatement = 'CREATE LOGIN [' + @AppUser + '] FROM WINDOWS WITH DEFAULT_DATABASE=[' + @DatabaseName + '], DEFAULT_LANGUAGE=[us_english]'
		EXEC sp_executesql @SqlStatement
	END

	IF NOT EXISTS (SELECT [Name] FROM SYSUSERS WHERE [Name] = @AppUser)
	BEGIN
		SET @SqlStatement = 'CREATE USER [' + @AppUser + '] FOR LOGIN [' + @AppUser + '] WITH DEFAULT_SCHEMA = ' + @SchemaName
		EXEC sp_executesql @SqlStatement
	END

	SET @SqlStatement = 'GRANT CONNECT TO [' + @AppUser + ']'
	EXEC sp_executesql @SqlStatement

	SET @SqlStatement = 'GRANT SELECT, INSERT, UPDATE ON ' + @SchemaName + '.Logs TO [' + @AppUser + ']'
	EXEC sp_executesql @SqlStatement
END
GO