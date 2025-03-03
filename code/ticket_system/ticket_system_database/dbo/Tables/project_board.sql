CREATE TABLE [dbo].[project_board]
(
	board_id Integer NOT NULL PRIMARY KEY,
	p_id Integer NOT NULL, 
    CONSTRAINT [FK_project_board_To_project] FOREIGN KEY ([p_id]) REFERENCES [project]([p_id])
)
