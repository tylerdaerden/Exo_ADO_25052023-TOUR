CREATE TRIGGER [TR_Student_Delete]
ON [Student]
INSTEAD OF DELETE
AS 
 BEGIN
	UPDATE [Student]
	 SET [Active] = 0
	 WHERE [Id] IN (SELECT [ID] FROM [deleted]);
 END