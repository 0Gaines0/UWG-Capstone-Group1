CREATE TABLE [dbo].[project_group]
(
	[p_id] INT NOT NULL,
	[g_id] INT NOT NULL, 
    CONSTRAINT [PK_project_group] PRIMARY KEY ([p_id], [g_id]), 
    CONSTRAINT [FK_project_group_To_project] FOREIGN KEY ([p_id]) REFERENCES [project]([p_id]),
	CONSTRAINT [FK_project_group_To_group] FOREIGN KEY ([g_id]) REFERENCES [group]([g_id])
)
