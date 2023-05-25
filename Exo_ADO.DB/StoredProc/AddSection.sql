CREATE PROCEDURE AddSection
	@sectionName VARCHAR(50),
	@sectionId INT

AS
BEGIN
	INSERT INTO Section (Id, SectionName)
	VALUES (@sectionId, @sectionName)
END