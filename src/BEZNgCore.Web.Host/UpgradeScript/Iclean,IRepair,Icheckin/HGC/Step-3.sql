
/****** Notification Script ******/
 ALTER TABLE [dbo].[SqoopeMsgLog]
    ADD [Read] [bit]
    DEFAULT 0 NOT NULL;
ALTER TABLE [dbo].[SqoopeMsgLog]
    ADD [Send] [bit]
    DEFAULT 0 NOT NULL;
ALTER TABLE [dbo].[SqoopeMsgLog]
    ADD [ToStaffKey] [uniqueidentifier] NULL
ALTER TABLE [dbo].[SqoopeMsgLog]
    ADD [FirebaseToken_Id] [nvarchar](2000) NULL
ALTER TABLE [dbo].[Staff]
    ADD [FirebaseToken_Id] [nvarchar](2000) NULL
ALTER TABLE [dbo].[Roomtype]
    ADD [LiveImgUrl] [nvarchar](1000) NULL

    --if cannot run from code
    ALTER TABLE RoomType
DROP COLUMN LiveImgUrl;