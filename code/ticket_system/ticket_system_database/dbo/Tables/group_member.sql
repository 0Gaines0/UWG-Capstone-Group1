CREATE TABLE [dbo].[group_member]
(
	[g_id] INT NOT NULL,
	[e_id] INT NOT NULL, 
    CONSTRAINT [PK_group_member] PRIMARY KEY ([g_id],[e_id]), 
    CONSTRAINT [FK_group_member_To_group] FOREIGN KEY ([g_id]) REFERENCES [group]([g_id]),
	CONSTRAINT [FK_group_member_To_employee] FOREIGN KEY ([e_id]) REFERENCES [employee]([e_id]),

)
