CREATE TABLE [dbo].[task]
(
	[task_id] INT NOT NULL PRIMARY KEY,
	[description] text,
	[priority] INT NOT NULL,
	[createdDate] DATETIME NOT NULL,
	[state_id] INT NOT NULL,
	[assignee_id] INT NULL,
    CONSTRAINT [FK_task_To_board_state] FOREIGN KEY (state_id) REFERENCES board_state(state_id),
	CONSTRAINT [FK_task_To_employee] FOREIGN KEY (assignee_id) REFERENCES employee(e_id),
)
