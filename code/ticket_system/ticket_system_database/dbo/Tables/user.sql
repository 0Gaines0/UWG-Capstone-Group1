CREATE TABLE [dbo].[user] (
    [Id]       INT            NOT NULL,
    [user_id]  NVARCHAR (50)  NOT NULL,
    [username] NVARCHAR (MAX) NOT NULL,
    [password] NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_user_id_key] UNIQUE NONCLUSTERED ([user_id] ASC)
);

