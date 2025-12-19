
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

    --if missing COLUMN
    ALTER TABLE RoomType
DROP COLUMN LiveImgUrl;

 ALTER TABLE [dbo].[MPriority]
    ADD [Active] [int] DEFAULT 1 NOT NULL;
	
	--if Missing TABLE
	GO

/****** Object:  Table [dbo].[HousekeepingOptOutReason]    Script Date: 24/10/2023 5:06:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HousekeepingOptOutReason](
	[HousekeepingOptOutReasonKey] [uniqueidentifier] NOT NULL,
	[Sort] [int] NULL,
	[Active] [int] NULL,
	[HousekeepingOptOutReasonCode] [nvarchar](10) NOT NULL,
	[Reason] [nvarchar](200) NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_HousekeepingOptOutReason] PRIMARY KEY CLUSTERED 
(
	[HousekeepingOptOutReasonKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[ReservationOptOut]    Script Date: 24/10/2023 5:07:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ReservationOptOut](
	[ReservationOptOutKey] [uniqueidentifier] NOT NULL,
	[ReservationOptOutCode] [nvarchar](50) NULL,
	[ReservationOptOutReason] [nvarchar](200) NULL,
	[AttendantID] [uniqueidentifier] NULL,
	[ReservationKey] [uniqueidentifier] NULL,
	[OptOut] [datetime] NULL,
	[Unit] [nvarchar](10) NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_ReservationOptOut] PRIMARY KEY CLUSTERED 
(
	[ReservationOptOutKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[HousekeepingDirtyReason]    Script Date: 24/10/2023 12:16:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HousekeepingDirtyReason](
	[HousekeepingDirtyReasonKey] [uniqueidentifier] NOT NULL,
	[Sort] [int] NULL,
	[Active] [int] NULL,
	[HousekeepingDirtyReasonCode] [nvarchar](10) NOT NULL,
	[Reason] [nvarchar](200) NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_HousekeepingDirtyReason] PRIMARY KEY CLUSTERED 
(
	[HousekeepingDirtyReasonKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT INTO [dbo].[HousekeepingDirtyReason]
           ([HousekeepingDirtyReasonKey]
           ,[Sort]
           ,[Active]
           ,[HousekeepingDirtyReasonCode]
           ,[Reason]
           ,[TenantId])
     VALUES
           (newid()
           ,1
           ,1
           ,'Dirty'
           ,'Room Dirty'
           ,1)
GO
/****** iRepair Access Control ******/
ALTER TABLE Staff  ADD [Sec_JobPosition] [int] NULL
ALTER TABLE Staff  ADD [Sec_IPSetUp] [int] NULL
ALTER TABLE Staff  ADD [Sec_IPViewLog] [int] NULL
ALTER TABLE Staff  ADD [Sec_IPAssignTasks] [int] NULL
ALTER TABLE Staff  ADD [Sec_IPBlockRoom] [int] NULL

/****** Upload 5 Images for LostFound,WorkOrder ******/
ALTER TABLE [dbo].[Document]
ADD [LostFoundKey] [uniqueidentifier] NULL,
[MWorkOrderKey] [uniqueidentifier] NULL,
[Signature] [varbinary](max) NULL 

/****** iClean Button Access Control ******/
ALTER TABLE Staff  ADD [Sec_SupervisorB] [int] NULL
ALTER TABLE Staff  ADD [Sec_SupervisorMode] [int] NULL
ALTER TABLE Staff  ADD [Sec_Rooms] [int] NULL
ALTER TABLE Staff  ADD [Sec_MiniBar] [int] NULL
ALTER TABLE Staff  ADD [Sec_MiniBarCo] [int] NULL
ALTER TABLE Staff  ADD [Sec_Laundry] [int] NULL
ALTER TABLE Staff  ADD [Sec_LostFound] [int] NULL
ALTER TABLE Staff  ADD [Sec_WOEntry] [int] NULL
ALTER TABLE Staff  ADD [Sec_ViewLogs] [int] NULL
ALTER TABLE Staff  ADD [Sec_RoomstoInspect] [int] NULL
ALTER TABLE Staff  ADD [Sec_GuestRequest] [int] NULL

/****** Add housekeeping Access rights to allow rooms to be clean directly ******/
ALTER TABLE Staff  ADD [Sec_AllowCleanDirectly] [int] NULL
