update maid set FirstName='Brillantez' where MaidKey='C2C6BB47-95CE-45C7-8DB4-DA6A99EBF5F7'
update maid set Active=1 where MaidKey='C2C6BB47-95CE-45C7-8DB4-DA6A99EBF5F7'
update MTechnician set Name='Brillantez' where Seqno=24
update MTechnician set TechnicianKey='8E8BB34D-F7BC-41B2-82F4-3D5DBA429038' where Seqno=24
update Staff set pin='Nzc4ODk5' where username='Brillantez'
update Staff set TechnicianKey='8E8BB34D-F7BC-41B2-82F4-3D5DBA429038' where username='Brillantez'
update Staff set MaidKey='C2C6BB47-95CE-45C7-8DB4-DA6A99EBF5F7' where username='Brillantez'
update Staff set Sec_Supervisor=10 where username='Brillantez'
update Staff set Sec_TechSupervisor=10 where username='Brillantez'
update AbpUsers set pin='Nzc4ODk5' where username='Brillantez'
GO
CREATE TABLE [dbo].[MPriority](
	[PriorityID] [int] IDENTITY(0,1) NOT NULL,
	[Priority] [varchar](200) NULL,
	[Sort] [int] NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_MPriority] PRIMARY KEY CLUSTERED 
(
	[PriorityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
GO
CREATE TABLE [dbo].[MWorkNotes](
	[MWorkNotesKey] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Details] [varchar](2000) NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[MWorkOrderKey] [uniqueidentifier] NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_MWorkNotes] PRIMARY KEY CLUSTERED 
(
	[MWorkNotesKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
GO
CREATE TABLE [dbo].[MWorkTimeSheetNoteTemplate](
	[Seqno] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Active] [int] NOT NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_MTimeSheetNoteTemplate] PRIMARY KEY CLUSTERED 
(
	[Seqno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
GO
CREATE TABLE [dbo].[SqoopeMsgLog](
	[SqoopeMsgLogKey] [uniqueidentifier] NOT NULL,
	[SqoopeMessageKey] [uniqueidentifier] NULL,
	[FromContactID] [int] NULL,
	[ToContactID] [int] NULL,
	[Msg] [nvarchar](500) NULL,
	[SqoopeMsgID] [int] NULL,
	[SqoopeMsgCreatedTS] [varchar](50) NULL,
	[SqoopeMsgCreatedOn] [datetime] NULL,
	[SqoopeMsgResCode] [nvarchar](50) NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
	[Seq] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NULL,
	[Read] [bit] NOT NULL,
	[Send] [bit] NOT NULL,
	[ToStaffKey] [uniqueidentifier] NULL,
	[FirebaseToken_Id] [nvarchar](2000) NULL,
PRIMARY KEY CLUSTERED 
(
	[SqoopeMsgLogKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO

GO
CREATE TABLE [dbo].[SqoopeGroup](
	[SqoopeGroupKey] [uniqueidentifier] NOT NULL,
	[Active] [int] NULL,
	[Sort] [int] NULL,
	[Sync] [int] NULL,
	[Seq] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[Description] [nvarchar](50) NULL,
	[Remark] [nvarchar](50) NULL,
	[MessageKey] [uniqueidentifier] NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_SqoopeGroup] PRIMARY KEY CLUSTERED 
(
	[SqoopeGroupKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
CREATE TABLE [dbo].[SqoopeGroupLink](
	[SqoopeLinkStaffkey] [uniqueidentifier] NOT NULL,
	[SqoopeGroupKey] [uniqueidentifier] NULL,
	[StaffKey] [uniqueidentifier] NULL,
	[Remark] [varchar](200) NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_SqoopeGroupLink] PRIMARY KEY CLUSTERED 
(
	[SqoopeLinkStaffkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
GO
CREATE TABLE [dbo].[SqoopeMsgGroupLink](
	[SqoopeMsgGroupLinkKey] [uniqueidentifier] NOT NULL,
	[SqoopeMessageKey] [uniqueidentifier] NOT NULL,
	[SqoopeGroupKey] [uniqueidentifier] NOT NULL,
	[Remark] [varchar](200) NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
	[Seq] [int] IDENTITY(1,1) NOT NULL,
	[Sort] [int] NULL,
	[TenantId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[SqoopeMsgGroupLinkKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO

GO
CREATE TABLE [dbo].[SqoopeMsgType](
	[MessageKey] [uniqueidentifier] NOT NULL,
	[Code] [varchar](50) NULL,
	[Description] [varchar](200) NULL,
	[CreatedOn] [datetime] NULL,
	[Active] [int] NULL,
	[Seq] [int] IDENTITY(1,1) NOT NULL,
	[MessageTemplate] [nvarchar](100) NOT NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_MessageType] PRIMARY KEY CLUSTERED 
(
	[MessageKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SqoopeGroup] ON 
GO
INSERT [dbo].[SqoopeGroup] ([SqoopeGroupKey], [Active], [Sort], [Sync], [Seq], [Code], [Description], [Remark], [MessageKey], [TenantId]) VALUES (N'4e0f4645-0e1c-47eb-abc3-3fee1f5ece2c', 1, 3, NULL, 3, N'Manager Group', N'Manager Group', N'', NULL, 1)
GO
INSERT [dbo].[SqoopeGroup] ([SqoopeGroupKey], [Active], [Sort], [Sync], [Seq], [Code], [Description], [Remark], [MessageKey], [TenantId]) VALUES (N'323564dd-8607-40fd-853e-6bb947461672', 1, 4, NULL, 4, N'Maintenance Group', N'Maintenance Group', N'', NULL, 1)
GO
INSERT [dbo].[SqoopeGroup] ([SqoopeGroupKey], [Active], [Sort], [Sync], [Seq], [Code], [Description], [Remark], [MessageKey], [TenantId]) VALUES (N'58044ce0-fd74-4cfa-91a9-6ff8c7ff8505', 1, 2, NULL, 2, N'Supervisor Group', N'Supervisor Group', N'', NULL, 1)
GO
INSERT [dbo].[SqoopeGroup] ([SqoopeGroupKey], [Active], [Sort], [Sync], [Seq], [Code], [Description], [Remark], [MessageKey], [TenantId]) VALUES (N'2c440923-621f-4a0c-990a-d5b04709c3b9', 1, 1, NULL, 1, N'Attendant Group', N'Attendant Group', N'', NULL, 1)
GO
INSERT [dbo].[SqoopeGroup] ([SqoopeGroupKey], [Active], [Sort], [Sync], [Seq], [Code], [Description], [Remark], [MessageKey], [TenantId]) VALUES (N'60041150-6d43-4d23-8314-da525c55eb0d', 1, 5, NULL, 5, N'Maintenance Supervisor Group', N'Maintenance Supervisor Group', N'', NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[SqoopeGroup] OFF
GO

INSERT INTO [dbo].[SqoopeGroupLink]
           ([SqoopeLinkStaffkey]
           ,[SqoopeGroupKey]
           ,[StaffKey]
           ,[Remark]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[TenantId])
     VALUES
           (N'249AEF06-AD6B-4AA9-8E9D-1A81F5DF217F'
           ,N'58044CE0-FD74-4CFA-91A9-6FF8C7FF8505'
           ,N'CB760BCC-4485-4532-8BD2-A3451DA11642'
           , N'Created'
           ,N'CB760BCC-4485-4532-8BD2-A3451DA11642'
           ,CAST(N'2022-12-02T12:05:40.320' AS DateTime)
           ,N'CB760BCC-4485-4532-8BD2-A3451DA11642'
           ,1)
GO
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'77af79b9-e94f-4423-a0b8-14148eee1085', N'75a27511-6f2c-45e8-a69d-7b2c6045633d', N'58044ce0-fd74-4cfa-91a9-6ff8c7ff8505', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-06T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 5, 5, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'b5a25ea8-9233-438d-a641-1ea711142e6e', N'87700a69-84bc-4a16-b47a-730c965a495c', N'4e0f4645-0e1c-47eb-abc3-3fee1f5ece2c', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-20T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 7, 7, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'7639a4c9-f1ad-44dc-87ba-348799a7599c', N'044b89b3-8832-4b1a-82b7-0000562d343b', N'60041150-6d43-4d23-8314-da525c55eb0d', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-20T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 13, 14, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'd3f21ab3-0f4f-4d0f-a6fd-3cd6db1bbaf8', N'ac3ae3ee-512e-4ad2-a4d3-ce6ff5709377', N'4e0f4645-0e1c-47eb-abc3-3fee1f5ece2c', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-12-21T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 16, 8, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'f6cdcc6a-a33e-4915-93cd-3fafbcf19b45', N'22d5ba5d-07b5-4506-af72-396aa29c1db4', N'58044ce0-fd74-4cfa-91a9-6ff8c7ff8505', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-06T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 6, 6, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'afe979bb-f5ee-45b2-9300-59a93076dcb8', N'02424b2c-b6ce-4dab-92a1-0000d998552b', N'323564dd-8607-40fd-853e-6bb947461672', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-20T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 11, 12, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'10c29b60-47e6-477b-871c-834b43dceaf7', N'044b89b3-8832-4b1a-82b7-0000562d343b', N'323564dd-8607-40fd-853e-6bb947461672', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-20T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 9, 10, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'84ba73db-7728-45ed-9c9a-901c988e6317', N'0f15326b-0f49-4a16-991b-00006cb55045', N'60041150-6d43-4d23-8314-da525c55eb0d', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-20T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 14, 15, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'4be7bb1e-493b-4fcf-88eb-9ca8b1e935c8', N'ff8df30e-f87b-49e2-8884-00006ea9a1f9', N'60041150-6d43-4d23-8314-da525c55eb0d', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-20T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 12, 13, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'd7885809-6b7c-49e9-b79c-9ffeb40c1e0a', N'ff8df30e-f87b-49e2-8884-00006ea9a1f9', N'323564dd-8607-40fd-853e-6bb947461672', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-20T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 8, 9, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'13bb130c-18cc-44e0-9a2e-cde0d94c581e', N'22d5ba5d-07b5-4506-af72-396aa29c1db4', N'2c440923-621f-4a0c-990a-d5b04709c3b9', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-06T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 2, 2, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'9663f497-667c-4d98-bf94-df6b3e96c04b', N'75a27511-6f2c-45e8-a69d-7b2c6045633d', N'2c440923-621f-4a0c-990a-d5b04709c3b9', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-06T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 1, 1, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'7d968e6d-34e0-49f2-a0e2-e48cb8041cd1', N'0f15326b-0f49-4a16-991b-00006cb55045', N'323564dd-8607-40fd-853e-6bb947461672', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-20T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 10, 11, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'47a3f37b-eaa5-495b-8955-f1962c70faa7', N'ac3ae3ee-512e-4ad2-a4d3-ce6ff5709377', N'58044ce0-fd74-4cfa-91a9-6ff8c7ff8505', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-06T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 3, 3, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'6a5435a0-f032-41f3-b3ba-f9343ddbdb0f', N'02424b2c-b6ce-4dab-92a1-0000d998552b', N'60041150-6d43-4d23-8314-da525c55eb0d', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-20T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 15, 16, 1)
GO
INSERT [dbo].[SqoopeMsgGroupLink] ([SqoopeMsgGroupLinkKey], [SqoopeMessageKey], [SqoopeGroupKey], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Seq], [Sort], [TenantId]) VALUES (N'f7fb7268-257e-4eb3-8ed3-fa6162b9f25a', N'87700a69-84bc-4a16-b47a-730c965a495c', N'58044ce0-fd74-4cfa-91a9-6ff8c7ff8505', N'', N'00000000-0000-0000-0000-000000000000', CAST(N'2015-10-06T00:00:00.000' AS DateTime), N'ff451dfd-918d-418b-817e-dc5187637666', CAST(N'2016-02-11T14:33:36.023' AS DateTime), 4, 4, 1)
GO
SET IDENTITY_INSERT [dbo].[SqoopeMsgGroupLink] OFF
GO
SET IDENTITY_INSERT [dbo].[SqoopeMsgType] ON 
GO
INSERT [dbo].[SqoopeMsgType] ([MessageKey], [Code], [Description], [CreatedOn], [Active], [Seq], [MessageTemplate], [TenantId]) VALUES (N'044b89b3-8832-4b1a-82b7-0000562d343b', N'iRepair:ASSIGN_WORKORDER', N'This message will trigger once this Work is assigned to Technician.', CAST(N'2015-10-08T13:59:38.217' AS DateTime), 1, 6, N'#USERNAME# has assigned WO##WO_NO#[#WO_DESC#] to #TECHNICIAN#', 1)
GO
INSERT [dbo].[SqoopeMsgType] ([MessageKey], [Code], [Description], [CreatedOn], [Active], [Seq], [MessageTemplate], [TenantId]) VALUES (N'0f15326b-0f49-4a16-991b-00006cb55045', N'iRepair:UPDATE_WORKORDER_STATUS', N'This message will trigger once this Work status is updated.', CAST(N'2015-10-08T13:59:38.217' AS DateTime), 1, 7, N'#USERNAME# has updated WO##WO_NO#[#WO_DESC#] Status to #WO_STATUS#', 1)
GO
INSERT [dbo].[SqoopeMsgType] ([MessageKey], [Code], [Description], [CreatedOn], [Active], [Seq], [MessageTemplate], [TenantId]) VALUES (N'ff8df30e-f87b-49e2-8884-00006ea9a1f9', N'iRepair:NEW_WORKORDER', N'This message will trigger once New Work Order has been added.', CAST(N'2015-10-08T13:59:38.217' AS DateTime), 1, 5, N'#USERNAME# has added New WO##WO_NO#[#WO_DESC#]', 1)
GO
INSERT [dbo].[SqoopeMsgType] ([MessageKey], [Code], [Description], [CreatedOn], [Active], [Seq], [MessageTemplate], [TenantId]) VALUES (N'02424b2c-b6ce-4dab-92a1-0000d998552b', N'iRepair:INFORM_BLOCKROOM_STATUS', N'This message will trigger once Block Room is added/updated.', CAST(N'2015-10-08T13:59:38.217' AS DateTime), 1, 8, N'In WO##WO_NO#, #USERNAME# has added/updated Block Room##ROOM_NO# #BLOCK_DATE# as #BLOCK_STATUS#', 1)
GO
INSERT [dbo].[SqoopeMsgType] ([MessageKey], [Code], [Description], [CreatedOn], [Active], [Seq], [MessageTemplate], [TenantId]) VALUES (N'22d5ba5d-07b5-4506-af72-396aa29c1db4', N'iClean:Assign_Attendant', N'This message will trigger after supervisor has assigned the room to Attendant.', CAST(N'2015-10-06T10:54:03.350' AS DateTime), 1, 4, N'Room##ROOM_NO# #ROOM_ASSIGNMENT_INFO#', 1)
GO
INSERT [dbo].[SqoopeMsgType] ([MessageKey], [Code], [Description], [CreatedOn], [Active], [Seq], [MessageTemplate], [TenantId]) VALUES (N'87700a69-84bc-4a16-b47a-730c965a495c', N'iClean:Checked_Clean', N'This message will trigger after supervisor has updated the room status as CLEAN.', CAST(N'2015-10-06T10:54:03.347' AS DateTime), 1, 2, N'Room##ROOM_NO# is CLEAN', 1)
GO
INSERT [dbo].[SqoopeMsgType] ([MessageKey], [Code], [Description], [CreatedOn], [Active], [Seq], [MessageTemplate], [TenantId]) VALUES (N'75a27511-6f2c-45e8-a69d-7b2c6045633d', N'iClean:Checked_Dirty', N'This message will trigger after supervisor has updated the room status as DIRTY.', CAST(N'2015-10-06T10:54:03.350' AS DateTime), 1, 3, N'Room##ROOM_NO# is DIRTY', 1)
GO
INSERT [dbo].[SqoopeMsgType] ([MessageKey], [Code], [Description], [CreatedOn], [Active], [Seq], [MessageTemplate], [TenantId]) VALUES (N'ac3ae3ee-512e-4ad2-a4d3-ce6ff5709377', N'iClean:InspectionRequired', N'This message will trigger after attendant has completed housekeeping in the room.', CAST(N'2015-10-06T10:54:03.347' AS DateTime), 1, 1, N'Room##ROOM_NO# requires inspection', 1)
GO
SET IDENTITY_INSERT [dbo].[SqoopeMsgType] OFF
GO
ALTER TABLE [dbo].[SqoopeGroup] ADD  CONSTRAINT [DF_SqoopeGroup_SqoopeGroupKey]  DEFAULT (newid()) FOR [SqoopeGroupKey]
GO
ALTER TABLE [dbo].[SqoopeGroup] ADD  CONSTRAINT [DF_SqoopeGroup_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[SqoopeGroup] ADD  CONSTRAINT [DF_SqoopeGroup_Sort]  DEFAULT ((0)) FOR [Sort]
GO
ALTER TABLE [dbo].[SqoopeGroup] ADD  CONSTRAINT [DF_SqoopeGroup_Sync]  DEFAULT ((0)) FOR [Sync]
GO
ALTER TABLE [dbo].[SqoopeGroupLink] ADD  CONSTRAINT [DF_SqoopeGroupLink_SqoopeLinkStaffkey]  DEFAULT (newid()) FOR [SqoopeLinkStaffkey]
GO
ALTER TABLE [dbo].[SqoopeGroupLink] ADD  CONSTRAINT [DF_SqoopeGroupLink_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[SqoopeMsgGroupLink] ADD  DEFAULT (newid()) FOR [SqoopeMsgGroupLinkKey]
GO
ALTER TABLE [dbo].[SqoopeMsgGroupLink] ADD  CONSTRAINT [DF_SqoopeMsgGroupLink_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[SqoopeMsgGroupLink] ADD  CONSTRAINT [DF_SqoopeMsgGroupLink_Sort]  DEFAULT ((0)) FOR [Sort]
GO
ALTER TABLE [dbo].[SqoopeMsgType] ADD  CONSTRAINT [DF_MessageType_MessageKey]  DEFAULT (newid()) FOR [MessageKey]
GO
ALTER TABLE [dbo].[SqoopeMsgType] ADD  CONSTRAINT [DF_MessageType_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[SqoopeMsgType] ADD  CONSTRAINT [DF_MessageType_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[SqoopeMsgType] ADD  DEFAULT ('#ROOM_NO#') FOR [MessageTemplate]
GO
