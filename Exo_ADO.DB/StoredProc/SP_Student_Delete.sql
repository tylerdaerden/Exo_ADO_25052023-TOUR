CREATE PROCEDURE [dbo].[SP_Student_Delete]
	@id INTEGER
AS
	UPDATE [Student]
	SET
		[Active] = 0
	WHERE [Id] = @id;