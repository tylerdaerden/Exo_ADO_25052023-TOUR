CREATE TABLE [dbo].[Student]
(
	[Id] INT NOT NULL IDENTITY,
	[FirstName] VARCHAR(50) NOT NULL,
	[LastName] VARCHAR(50) NOT NULL,
	[BirthDate] DATETIME2 NOT NULL,
	[YearResult] INT NOT NULL,
	[SectionID] INT NOT NULL,
	[Active] BIT DEFAULT 1,

	CONSTRAINT PK_Student PRIMARY KEY([Id]),
	CONSTRAINT CK_Student__YearResult CHECK ([YearResult] BETWEEN 0 AND 20),
	CONSTRAINT CK_Student__BirthDate CHECK ([BirthDate] >= '01-01-1930'),
	CONSTRAINT FK_Student__Section FOREIGN KEY([SectionID]) REFERENCES [Section]([Id])
)
