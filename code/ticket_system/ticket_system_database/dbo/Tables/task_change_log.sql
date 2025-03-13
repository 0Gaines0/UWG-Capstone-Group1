CREATE TABLE [dbo].[task_change_log]
(
	[task_id] INT NOT NULL PRIMARY KEY,
	[change_id] INT NOT NULL, 
    CONSTRAINT [FK_task_change_log_To_task] FOREIGN KEY ([task_id]) REFERENCES [task]([task_id]), 
    CONSTRAINT [FK_task_change_log_To_task_change] FOREIGN KEY ([change_id]) REFERENCES [task_change]([change_id])

)
