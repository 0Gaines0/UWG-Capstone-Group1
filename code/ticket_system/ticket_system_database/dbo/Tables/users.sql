CREATE TABLE [dbo].[users] (
    [Id]       INT PRIMARY KEY NOT NULL,
    [userId]  NVARCHAR (50)  NOT NULL,
    [username] NVARCHAR (MAX) NOT NULL,
    [password] NVARCHAR (MAX) NOT NULL,
);

