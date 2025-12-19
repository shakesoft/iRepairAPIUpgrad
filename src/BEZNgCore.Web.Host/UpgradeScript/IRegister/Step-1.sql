ALTER TABLE [dbo].[AbpUsers]
 ADD [CarPlate] [nvarchar](20) NULL,
	[CheckInDate] [datetime2](7) NULL,
	[CheckOutDate] [datetime2](7) NULL,
	[Company] [nvarchar](60) NULL,
	[CompanyName] [nvarchar](50) NULL,
	[ETA] [nvarchar](10) NULL,
	[ETD] [nvarchar](10) NULL,
	[MeetingId] [uniqueidentifier] NULL,
	[Notes] [nvarchar](200) NULL,
	[Passport] [nvarchar](60) NULL,
	[Photo] [varbinary](max) NULL,
	[RegistrationKey] [uniqueidentifier] NULL,
	[RoomNumber] [nvarchar](20) NULL,
	[Status] [int] NULL,
	[Temperature] [nvarchar](10) NULL,
	[Title] [nvarchar](20) NULL,
	[UnitNumber] [nvarchar](50) NULL,
	[WorkPassID] [nvarchar](60) NULL
GO
 IF COL_LENGTH('Registration','TenantId') IS NULL
 BEGIN
 ALTER TABLE Registration  ADD TenantId int NULL
END
GO
Update Registration SET TenantId=1
GO
 IF COL_LENGTH('RegistrationLog','TenantId') IS NULL
 BEGIN
 ALTER TABLE RegistrationLog  ADD TenantId int NULL
END
GO
Update RegistrationLog SET TenantId=1

GO
 IF COL_LENGTH('RegistrationVisitor','TenantId') IS NULL
 BEGIN
 ALTER TABLE RegistrationVisitor  ADD TenantId int NULL
END
GO
Update RegistrationVisitor SET TenantId=1

GO
 IF COL_LENGTH('Module_Security','TenantId') IS NULL
 BEGIN
 ALTER TABLE Module_Security  ADD TenantId int NULL
END
GO
Update Module_Security SET TenantId=1

GO

IF COL_LENGTH('AbpUsers','WorkPassID') IS NULL
 BEGIN
 ALTER TABLE AbpUsers  ADD [CheckInDate] [datetime2](7) NULL,
	[CheckOutDate] [datetime2](7) NULL,
	[Company] [nvarchar](60) NULL,
	[CompanyName] [nvarchar](50) NULL,
	[ETA] [nvarchar](10) NULL,
	[ETD] [nvarchar](10) NULL,
	[Notes] [nvarchar](200) NULL,
	[Passport] [nvarchar](60) NULL,
	[RegistrationKey] [uniqueidentifier] NULL,
	[RoomNumber] [nvarchar](20) NULL,
	[Status] [int] NULL,
	[Temperature] [nvarchar](10) NULL,
	[Title] [nvarchar](20) NULL,
	[UnitNumber] [nvarchar](50) NULL,
	[WorkPassID] [nvarchar](60) NULL
END

GO
DELETE Registration where FirstName =''
GO
INSERT INTO [dbo].[AbpUsers]
           (
		   ShouldChangePasswordOnNextLogin,
		   [CreationTime]          
           ,[IsDeleted]        
           ,[UserName]
           ,[TenantId]     
		   ,EmailAddress
		   ,IsPhoneNumberConfirmed
           ,[Name]
           ,[Surname]
           ,[Password]        
           ,[AccessFailedCount]
           ,[IsLockoutEnabled]          
           ,[IsActive]
           ,[NormalizedUserName]
		   ,NormalizedEmailAddress
		   ,IsTwoFactorEnabled
		   ,IsEmailConfirmed
		   ,SecurityStamp
		   ,ConcurrencyStamp
		   ,[CheckInDate],
			[CheckOutDate],
			[Company],
			[CompanyName],
			[ETA],
			[ETD],
			[Notes],
			[Passport],
			[RegistrationKey],
			[RoomNumber],
			[Status],
			[Temperature],
			[Title],
			[UnitNumber],
			[WorkPassID]
       )
     SELECT
           0
		   ,getdate()          
           ,0           
           ,LOWER(REPLACE( FirstName, ' ', ''))
           ,1  
		   ,ISNULL(EMail,LOWER(REPLACE(FirstName, ' ', ''))+'@company.com')
		   ,0
           ,FirstName
           ,ISNULL(LastName,FirstName)
           ,'QnJpbGxhbnRlei4xOA=='        
           ,0
           ,1           
           ,1
           ,UPPER(REPLACE(FirstName, ' ', ''))
		   ,UPPER(ISNULL(EMail,LOWER(REPLACE(FirstName, ' ', ''))+'@company.com'))
		   ,0
		   ,0
		   ,newid()
		   ,newid()
		   ,[CheckInDate],
			[CheckOutDate],
			[Company],
			[CompanyName],
			[ETA],
			[ETD],
			[Notes],
			[Passport],
			[RegistrationKey],
			[RoomNumber],
			[Status],
			[Temperature],
			[Title],
			[UnitNumber],
			[WorkPassID]
		    FROM Registration
GO
