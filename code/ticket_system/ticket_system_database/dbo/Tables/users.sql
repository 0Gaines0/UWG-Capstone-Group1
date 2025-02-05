CREATE TABLE [dbo].[users] (
    [Id]       INT            NOT NULL,
    [userId]  NVARCHAR (50)  NOT NULL,
    [username] NVARCHAR (MAX) NOT NULL,
    [password] NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_user_id_key] UNIQUE NONCLUSTERED ([userId] ASC)
);

