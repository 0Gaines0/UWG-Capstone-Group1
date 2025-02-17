CREATE TABLE [dbo].[employee]
(
	[e_id] INT NOT NULL PRIMARY KEY,
	[f_name] VARCHAR(MAX) NOT NULL,
	[l_name] VARCHAR(MAX) NOT NULL,
	[username] VARCHAR(MAX) NOT NULL,
	[hashed_password] VARCHAR(MAX) NOT NULL,
	[is_active] BIT NOT NULL,
	[is_manager] BIT NOT NULL,
	[is_admin] BIT NOT NULL,
	[email] VARCHAR(MAX) NOT NULL
)
