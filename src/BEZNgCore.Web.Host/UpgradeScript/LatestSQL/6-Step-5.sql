GO
CREATE TABLE [dbo].[Dndphoto](
	[DndphotoKey] [uniqueidentifier] NOT NULL,
	[RoomKey] [uniqueidentifier] NULL,
	[LastModifiedStaff] [uniqueidentifier] NULL,
	[Sort] [int] NULL,
	[Sync] [int] NULL,
	[Seq] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[TS] [timestamp] NULL,
        [Document] [varchar](100) NULL,
	[Image] [varbinary](max) NULL,
 CONSTRAINT [PK_Dndphoto] PRIMARY KEY CLUSTERED 
(
	[DndphotoKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Dndphoto] ADD  CONSTRAINT [DF_Dndphoto_Sort]  DEFAULT (0) FOR [Sort]
GO
ALTER TABLE [dbo].[Dndphoto] ADD  CONSTRAINT [DF_Dndphotot_Sync]  DEFAULT (0) FOR [Sync]
GO
ALTER TABLE [dbo].[Dndphoto] ADD  DEFAULT (NULL) FOR [Image]
GO

ALTER TABLE [dbo].[Dndphoto]
    ADD [CreatedDate] [datetime] NULL
ALTER TABLE [dbo].[Dndphoto]
ADD CONSTRAINT DF_Dndphoto_CreatedDate DEFAULT (GETDATE()) FOR [CreatedDate];

UPDATE [dbo].[Dndphoto]
SET [CreatedDate] = GETDATE()
WHERE [CreatedDate] IS NULL;
