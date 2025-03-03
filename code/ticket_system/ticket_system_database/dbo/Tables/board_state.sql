CREATE TABLE [dbo].[board_state]
(
	state_id Integer NOT NULL PRIMARY KEY,
	board_id Integer NOT NULL,
	state_name VARCHAR(MAX) NOT NULL,
	position Integer NOT NULL, 
    CONSTRAINT [FK_board_state_To_project_board] FOREIGN KEY ([board_id]) REFERENCES [project_board]([board_id])
)
