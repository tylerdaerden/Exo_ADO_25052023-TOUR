CREATE VIEW [dbo].[V_Student]
AS 
SELECT [Id], [FirstName], [LastName], [BirthDate], [YearResult], [SectionID] 
FROM [Student]
WHERE [Active] = 1