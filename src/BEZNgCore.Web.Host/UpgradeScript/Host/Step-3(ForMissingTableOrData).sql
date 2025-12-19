/****** if [LostFoundImage] table is missing ******/
GO
CREATE TABLE [dbo].[LostFoundImage](
	[LostFoundImageKey] [uniqueidentifier] NOT NULL,
	[LostFoundKey] [uniqueidentifier] NOT NULL,
	[LostFoundImage] [varchar](256) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[CreatedUser] [uniqueidentifier] NULL,
	[LostFoundImages] [image] NULL,
	[LostFoundImages2] [image] NULL,
	[LostFoundImages3] [image] NULL,
	[LostFoundImages4] [image] NULL,
	[LostFoundImages5] [image] NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_LostFoundImage] PRIMARY KEY CLUSTERED 
(
	[LostFoundImageKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[LostFoundImage] ADD  DEFAULT (newid()) FOR [LostFoundImageKey]
GO

/****** if [MaidStatus] Data is missing ******/
SET IDENTITY_INSERT [dbo].[MaidStatus] ON 
INSERT [dbo].[MaidStatus] ([MaidStatusKey], [MaidStatus], [Sort], [Sync], [Seq], [TenantId]) VALUES (N'5e1be1e8-b25e-4429-befa-85e9a0c63a00', N'Maintenance in the Room', 0, 0, 5, 1)
INSERT [dbo].[MaidStatus] ([MaidStatusKey], [MaidStatus], [Sort], [Sync], [Seq], [TenantId]) VALUES (N'42bf2df3-85e7-49d7-a5dc-8b19246decf0', N'Maintenance Required', 0, 0, 6, 1)
SET IDENTITY_INSERT [dbo].[MaidStatus] OFF

/***** for NotiMessage Permission******/
update generalprofile set profilevalue='True' where ProfileName = 'SqoopeIntegration'

/******if there is no ExternalAccess data******/
GO
SET IDENTITY_INSERT [dbo].[ExternalAccess] ON 

INSERT [dbo].[ExternalAccess] ([ExternalAccessKey], [Sort], [Sync], [Seq], [Start], [END], [Active]) VALUES (N'51301f8f-12b4-42f9-a2c2-73e6db7b658a', 1, NULL, 3, N'192.168.100.1', N'192.168.100.255', 1)
INSERT [dbo].[ExternalAccess] ([ExternalAccessKey], [Sort], [Sync], [Seq], [Start], [END], [Active]) VALUES (N'537b68aa-dc96-4345-b59a-83628f90df4f', 2, NULL, 5, N'192.168.20.1', N'192.168.20.255', 1)
INSERT [dbo].[ExternalAccess] ([ExternalAccessKey], [Sort], [Sync], [Seq], [Start], [END], [Active]) VALUES (N'd35fc4d7-e53c-475a-95f8-c3f76e28ef98', 0, 0, 1, N'192.168.10.1', N'192.168.10.255', 1)
SET IDENTITY_INSERT [dbo].[ExternalAccess] OFF
ALTER TABLE [dbo].[ExternalAccess] ADD  CONSTRAINT [DF_ExternalAccess_ExternalAccessKey]  DEFAULT (newid()) FOR [ExternalAccessKey]
GO
ALTER TABLE [dbo].[ExternalAccess] ADD  CONSTRAINT [DF_ExternalAccess_Sort]  DEFAULT ((0)) FOR [Sort]
GO
ALTER TABLE [dbo].[ExternalAccess] ADD  CONSTRAINT [DF_ExternalAccess_Sync]  DEFAULT ((0)) FOR [Sync]
GO
ALTER TABLE [dbo].[ExternalAccess] ADD  DEFAULT ((1)) FOR [Active]
GO



/******For Chancellor Only******/
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=187 and Active=1) where ToContactID=187 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=221 and Active=1) where ToContactID=221 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=253 and Active=1) where ToContactID=253 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=254) where ToContactID=254 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=276) where ToContactID=276 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=304) where ToContactID=304 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=308) where ToContactID=308 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=309) where ToContactID=309 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=311) where ToContactID=311 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=313) where ToContactID=313 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=315) where ToContactID=315 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=316 and Active=1) where ToContactID=316 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=317 and Active=1) where ToContactID=317 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=318) where ToContactID=318 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=319) where ToContactID=319 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=320 and Active=1) where ToContactID=320 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=321) where ToContactID=321 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=322 and Active=1) where ToContactID=322 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=323 and Active=1) where ToContactID=323 and year(createdOn)=2022 and month(createdOn)>1

update [SqoopeMsgLog] set ToStaffKey='BA3D9704-07B8-43E0-97D5-D58450C14D12' where ToContactID=324 and year(createdOn)=2022 and month(createdOn)>1

update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=325 and Active=1) where ToContactID=325 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=328) where ToContactID=328 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=339) where ToContactID=339 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=341) where ToContactID=341 and year(createdOn)=2022 and month(createdOn)>1

update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=342 and Active=1) where ToContactID=342 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=343) where ToContactID=343 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=344) where ToContactID=344 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=346) where ToContactID=346 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=357) where ToContactID=357 and year(createdOn)=2022 and month(createdOn)>1

update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=406 and Active=1) where ToContactID=406 and year(createdOn)=2022 and month(createdOn)>1
update [SqoopeMsgLog] set ToStaffKey=(select staffkey from Staff where Contact_Id=413 and Active=1) where ToContactID=413 and year(createdOn)=2022 and month(createdOn)>1
