/*
 Source Server Type    : SQL Server
 Source Server Version : 12002000
 Source Host           : recognitionoteserver.database.windows.net:1433
 Source Catalog        : reconitionNoteDB
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 12002000
 File Encoding         : 65001
*/


-- ----------------------------
-- Table structure for nota
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[nota]') AND type IN ('U'))
	DROP TABLE [dbo].[nota]
GO

CREATE TABLE [dbo].[nota] (
  [id_nota] int  NOT NULL,
  [titolo] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [data_nota] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [contenuto] nvarchar(4000) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [username] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL
)
GO


-- ----------------------------
-- Table structure for utente
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[utente]') AND type IN ('U'))
	DROP TABLE [dbo].[utente]
GO

CREATE TABLE [dbo].[utente] (
  [username] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [passw] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [personID] varchar(40) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO


-- ----------------------------
-- Primary Key structure for table nota
-- ----------------------------
ALTER TABLE [dbo].[nota] ADD CONSTRAINT [PK__nota__26991D8CFD4A7F84] PRIMARY KEY CLUSTERED ([id_nota])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Primary Key structure for table utente
-- ----------------------------
ALTER TABLE [dbo].[utente] ADD CONSTRAINT [PK__utente__F3DBC573CE73163F] PRIMARY KEY CLUSTERED ([username])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Foreign Keys structure for table nota
-- ----------------------------
ALTER TABLE [dbo].[nota] ADD CONSTRAINT [FK__nota__username__5EBF139D] FOREIGN KEY ([username]) REFERENCES [dbo].[utente] ([username]) ON DELETE CASCADE ON UPDATE CASCADE
GO

