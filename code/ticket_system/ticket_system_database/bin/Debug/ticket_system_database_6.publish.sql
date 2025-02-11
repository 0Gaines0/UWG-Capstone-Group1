﻿/*
Deployment script for ticket_system

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "ticket_system"
:setvar DefaultFilePrefix "ticket_system"
:setvar DefaultDataPath "C:\Users\Jgain\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"
:setvar DefaultLogPath "C:\Users\Jgain\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Creating Foreign Key [dbo].[FK_group_To_employee]...';


GO
ALTER TABLE [dbo].[group] WITH NOCHECK
    ADD CONSTRAINT [FK_group_To_employee] FOREIGN KEY ([manager_id]) REFERENCES [dbo].[employee] ([e_id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_group_member_To_employee]...';


GO
ALTER TABLE [dbo].[group_member] WITH NOCHECK
    ADD CONSTRAINT [FK_group_member_To_employee] FOREIGN KEY ([e_id]) REFERENCES [dbo].[employee] ([e_id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_group_member_To_group]...';


GO
ALTER TABLE [dbo].[group_member] WITH NOCHECK
    ADD CONSTRAINT [FK_group_member_To_group] FOREIGN KEY ([g_id]) REFERENCES [dbo].[group] ([g_id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_project_To_employee]...';


GO
ALTER TABLE [dbo].[project] WITH NOCHECK
    ADD CONSTRAINT [FK_project_To_employee] FOREIGN KEY ([project_lead_id]) REFERENCES [dbo].[employee] ([e_id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_project_group_To_group]...';


GO
ALTER TABLE [dbo].[project_group] WITH NOCHECK
    ADD CONSTRAINT [FK_project_group_To_group] FOREIGN KEY ([g_id]) REFERENCES [dbo].[group] ([g_id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_project_group_To_project]...';


GO
ALTER TABLE [dbo].[project_group] WITH NOCHECK
    ADD CONSTRAINT [FK_project_group_To_project] FOREIGN KEY ([p_id]) REFERENCES [dbo].[project] ([p_id]);


GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[group] WITH CHECK CHECK CONSTRAINT [FK_group_To_employee];

ALTER TABLE [dbo].[group_member] WITH CHECK CHECK CONSTRAINT [FK_group_member_To_employee];

ALTER TABLE [dbo].[group_member] WITH CHECK CHECK CONSTRAINT [FK_group_member_To_group];

ALTER TABLE [dbo].[project] WITH CHECK CHECK CONSTRAINT [FK_project_To_employee];

ALTER TABLE [dbo].[project_group] WITH CHECK CHECK CONSTRAINT [FK_project_group_To_group];

ALTER TABLE [dbo].[project_group] WITH CHECK CHECK CONSTRAINT [FK_project_group_To_project];


GO
PRINT N'Update complete.';


GO
