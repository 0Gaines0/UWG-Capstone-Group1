CREATE TABLE [dbo].[group]
(
	[g_id] INT NOT NULL PRIMARY KEY,
	[manager_id] INT NOT NULL,
	[g_name] VARCHAR(MAX) NOT NULL,
	[g_description] VARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_group_To_employee] FOREIGN KEY ([manager_id]) REFERENCES [employee]([e_id])
)
