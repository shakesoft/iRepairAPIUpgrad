/****** SeparateDeviceTokenForNoti ******/
ALTER TABLE [dbo].[Staff]
    ADD [FirebaseToken_IdiRepair] [nvarchar](2000) NULL
/****** Msg Template sent Notification in iClean ******/
GO
SET IDENTITY_INSERT [dbo].[SqoopeMsgType] ON 
GO
INSERT [dbo].[SqoopeMsgType] ([MessageKey], [Code], [Description], [CreatedOn], [Active], [Seq], [MessageTemplate], [TenantId]) VALUES (N'918cbd78-917a-41ce-8fe2-2dd2eacf3791', N'iClean:NEW_WORKORDER', N'This message will trigger once New Work Order has been added.', CAST(N'2025-04-09T12:23:02.030' AS DateTime), 1, 9, N'#USERNAME# has added New WO##WO_NO#[#WO_DESC#]', 1)
GO
SET IDENTITY_INSERT [dbo].[SqoopeMsgType] OFF

