CREATE TABLE [dbo].[task_comment] (
    [comment_id] INT NOT NULL IDENTITY PRIMARY KEY,
    [task_id] INT NOT NULL,
    [commenter_id] INT NOT NULL,
    [comment_text] NVARCHAR(MAX) NOT NULL,
    [commented_at] DATETIME2 NOT NULL,

    CONSTRAINT [FK_task_comment_task_task_id] 
        FOREIGN KEY ([task_id]) REFERENCES [task] ([task_id]) ON DELETE NO ACTION,

    CONSTRAINT [FK_task_comment_Employees_commenter_id] 
        FOREIGN KEY ([commenter_id]) REFERENCES [employee] ([e_id]) ON DELETE CASCADE
);
