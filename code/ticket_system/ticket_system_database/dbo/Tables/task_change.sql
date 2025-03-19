CREATE TABLE [dbo].[task_change]
(
	[change_id] INT NOT NULL PRIMARY KEY, 
    [type] VARCHAR(MAX) NOT NULL,
	[changed_date] datetime NOT NULL,
	[previous_value] text NULL,
	[new_value] text NOT NULL,
	[assignee_id] int NOT NULL, 
    CONSTRAINT [FK_task_change_To_employee] FOREIGN KEY ([assignee_id]) REFERENCES [employee]([e_id])

)
