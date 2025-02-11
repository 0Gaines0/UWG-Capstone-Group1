CREATE TABLE [dbo].[project]
(
	[p_id] INT NOT NULL PRIMARY KEY,
	[project_lead_id] INT NOT NULL,
	[p_title] VARCHAR(MAX) NOT NULL,
	[p_description] text NOT NULL, 
    CONSTRAINT [FK_project_To_employee] FOREIGN KEY ([project_lead_id]) REFERENCES [employee]([e_id])
)
