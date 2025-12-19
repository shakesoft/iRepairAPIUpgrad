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
