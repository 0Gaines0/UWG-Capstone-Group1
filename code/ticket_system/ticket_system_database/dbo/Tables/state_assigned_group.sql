CREATE TABLE [dbo].[state_assigned_group]
(
	[state_id] INT NOT NULL , 
    [group_id] INT NOT NULL, 
    CONSTRAINT [FK_state_assigned_group_board_state] FOREIGN KEY (state_id) REFERENCES board_state(state_id), 
    CONSTRAINT [FK_state_assigned_group_to_group] FOREIGN KEY (group_id) REFERENCES [group]([g_id]), 
    CONSTRAINT [PK_state_assigned_group] PRIMARY KEY (state_id,group_id)
     
)
