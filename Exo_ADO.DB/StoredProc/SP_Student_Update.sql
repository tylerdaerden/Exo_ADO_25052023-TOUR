CREATE PROCEDURE [dbo].[SP_Student_Update]
	@id INT,
	@section_id INT,
	@year_result INT
AS
	UPDATE [Student]
	SET
		[SectionID] = @section_id,
		[YearResult] = @year_result
	WHERE [Id] = @id;
