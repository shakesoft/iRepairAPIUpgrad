INSERT INTO [dbo].[AbpUsers]
           ([AccessFailedCount]
           ,[AuthenticationSource]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorUserId]
           ,[DeleterUserId]
           ,[DeletionTime]
           ,[EmailAddress]
           ,[EmailConfirmationCode]
           ,[IsActive]
           ,[IsDeleted]
           ,[IsEmailConfirmed]
           ,[IsLockoutEnabled]
           ,[IsPhoneNumberConfirmed]
           ,[IsTwoFactorEnabled]
           ,[LastModificationTime]
           ,[LastModifierUserId]
           ,[LockoutEndDateUtc]
           ,[Name]
           ,[NormalizedEmailAddress]
           ,[NormalizedUserName]
           ,[Password]
           ,[PasswordResetCode]
           ,[PhoneNumber]
           ,[ProfilePictureId]
           ,[SecurityStamp]
           ,[ShouldChangePasswordOnNextLogin]
           ,[Surname]
           ,[TenantId]
           ,[UserName]
           ,[SignInToken]
           ,[SignInTokenExpireTimeUtc]
           ,[GoogleAuthenticatorKey]
           ,[PIN]
           ,[StaffKey])
     SELECT
       0
	   ,NULL
       ,NULL
       ,getdate()
       ,NULL
       ,NULL
       ,NULL          
       ,UPPER(LOWER(REPLACE(UserName, ' ', '')) + '@company.com')
       ,NULL 
       ,1
       ,0
       ,1
       ,1
       ,0
       ,1
       ,NULL
       ,NULL
       ,NULL 
       ,REPLACE(UserName, ' ', '')
       ,UPPER(LOWER(REPLACE(UserName, ' ', '')) + '@company.com')
       ,REPLACE(UserName, ' ', '')
       ,ISNULL(Password,'AQAAAAEAACcQAAAAEJXLncV5WokLtv+TqyV7qXlGlos6wAK52lZ9j+BPTO8B8TKa7MlLElz8jzI6SpYcTw==') 
       ,NULL
       ,NULL
       ,NULL 
       ,newid()
       ,0
       ,REPLACE(UserName, ' ', '')
       ,1
       ,REPLACE(UserName, ' ', '')
       ,NULL
       ,NULL
       ,NULL
       ,PIN
       ,staffkey
       FROM Staff where UserName is not null

ALTER TABLE [dbo].[Control]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[GuestStatus]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[History]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Maid]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[MArea]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[MWorkOrder]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Room]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[RoomStatus]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[RoomType]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[RoomTypeGrouping]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Staff]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[TravelType]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Title]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[AttendantCheckList]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[BillingCode]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Booking]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Campaign]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Cancellation]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[CancellationRule]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[CheckList]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Company]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Currency]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Department]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[EventType]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[GeneralProfile]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Group1]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Group2]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Group3]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Group4]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Guest]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[GuestIdentityType]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Item]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[LostFound]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[LostFoundImage]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[LostFoundStatus]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[MaidStatus]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[MTechnician]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[MWorkOrderStatus]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[MWorkType]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Nationality]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[PaymentGroup]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[PaymentType]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Postcode]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[RateType]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[RateTypeGrouping]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Region]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Report]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[ReportBatch]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[ReportSecurity]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[Reservation]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[ReservationAdditional]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[ReservationGuest]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[ReservationRate]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[SecurityProfile]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[SqoopeGroup]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[SqoopeGroupLink]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[SqoopeMsgGroupLink]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[SqoopeMsgLog]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[SqoopeMsgType]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[StaffDepartment]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[SupervisorCheckList]
ADD [TenantId] [int] NULL;

ALTER TABLE [dbo].[TravelReason]
ADD [TenantId] [int] NULL;

/****** For Multi Tenancy update for each respective tenant id******/

update Control set TenantId=1
update GuestStatus set TenantId=1
update History set TenantId=1 where ModuleName='iClean'
update Maid set TenantId=1
update MArea set TenantId=1
update MWorkOrder set TenantId=1
update Room set TenantId=1
update RoomStatus set TenantId=1
update RoomType set TenantId=1
update RoomTypeGrouping set TenantId=1
update Staff set TenantId=1
update TravelType set TenantId=1
update Title set TenantId=1
update AttendantCheckList set TenantId=1
update BillingCode set TenantId=1
update Booking set TenantId=1
update Campaign set TenantId=1
update Cancellation set TenantId=1
update CancellationRule set TenantId=1
update CheckList set TenantId=1
update Company set TenantId=1
update Currency set TenantId=1
update Department set TenantId=1
update EventType set TenantId=1
update GeneralProfile set TenantId=1
update Group1 set TenantId=1
update Group2 set TenantId=1
update Group3 set TenantId=1
update Group4 set TenantId=1
update Guest set TenantId=1
update GuestIdentityType set TenantId=1
update Item set TenantId=1
update LostFound set TenantId=1
update LostFoundImage set TenantId=1
update LostFoundStatus set TenantId=1
update MaidStatus set TenantId=1
update MTechnician set TenantId=1
update MWorkOrderStatus set TenantId=1
update MWorkType set TenantId=1
update Nationality set TenantId=1
update PaymentGroup set TenantId=1
update PaymentType set TenantId=1
update Postcode set TenantId=1
update RateType set TenantId=1
update RateTypeGrouping set TenantId=1
update Region set TenantId=1
update Report set TenantId=1
update ReportBatch set TenantId=1
update ReportSecurity set TenantId=1
update Reservation set TenantId=1
update ReservationAdditional set TenantId=1
update ReservationGuest set TenantId=1
update ReservationRate set TenantId=1
update SecurityProfile set TenantId=1
update SqoopeGroup set TenantId=1
update SqoopeGroupLink set TenantId=1
update SqoopeMsgGroupLink set TenantId=1
update SqoopeMsgLog set TenantId=1
update SqoopeMsgType set TenantId=1
update StaffDepartment set TenantId=1
update SupervisorCheckList set TenantId=1
update TravelReason set TenantId=1


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





