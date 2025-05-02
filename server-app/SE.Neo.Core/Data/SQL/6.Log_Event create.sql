IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log_Event]') AND type in (N'U'))
DROP TABLE [dbo].[Log_Event]
GO

CREATE TABLE [dbo].[Log_Event](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](850) NULL,
	[Level] [nvarchar](250) NULL,
	[Timestamp] [datetime] NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [xml] NULL,
	[User_Name] [nvarchar](100) NULL,
	[Source] [int] NULL,
 CONSTRAINT [PK_Log_Event] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Log_Event]  WITH CHECK ADD CHECK  (((1)<=[Source] AND [Source]<=(4)))
GO

CREATE NONCLUSTERED INDEX [IX_Log_Event_Level] ON [dbo].[Log_Event]
(
	[Level] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Log_Event_User_Name] ON [dbo].[Log_Event]
(
	[User_Name] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Log_Event_Source] ON [dbo].[Log_Event]
(
	[Source] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Log_Event_Timestamp] ON [dbo].[Log_Event]
(
	[Timestamp] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Log_Event_Message] ON [dbo].[Log_Event]
(
	[Message] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO
