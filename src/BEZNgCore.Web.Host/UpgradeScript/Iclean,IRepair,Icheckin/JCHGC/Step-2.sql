--for iclean and irepair
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

/****** For Multi Tenancy update for each respective tenant id******/
GO
 IF COL_LENGTH('Control','TenantId') IS NULL
 BEGIN
 ALTER TABLE Control  ADD TenantId int NULL
END
GO
update Control set TenantId=1
GO
 IF COL_LENGTH('GuestStatus','TenantId') IS NULL
 BEGIN
 ALTER TABLE GuestStatus  ADD TenantId int NULL
END
GO
update GuestStatus set TenantId=1
GO
 IF COL_LENGTH('History','TenantId') IS NULL
 BEGIN
 ALTER TABLE History  ADD TenantId int NULL
END
GO
update History set TenantId=1 where ModuleName='iClean' or ModuleName='iRepair' or ModuleName='iCheckIn'
GO
 IF COL_LENGTH('Maid','TenantId') IS NULL
 BEGIN
 ALTER TABLE Maid  ADD TenantId int NULL
END
GO
update Maid set TenantId=1
GO
 IF COL_LENGTH('MArea','TenantId') IS NULL
 BEGIN
 ALTER TABLE MArea  ADD TenantId int NULL
END
GO
update MArea set TenantId=1
GO
 IF COL_LENGTH('MWorkOrder','TenantId') IS NULL
 BEGIN
 ALTER TABLE MWorkOrder  ADD TenantId int NULL
END
GO
update MWorkOrder set TenantId=1
GO
 IF COL_LENGTH('Room','TenantId') IS NULL
 BEGIN
 ALTER TABLE Room  ADD TenantId int NULL
END
GO
update Room set TenantId=1
GO
 IF COL_LENGTH('RoomStatus','TenantId') IS NULL
 BEGIN
 ALTER TABLE RoomStatus  ADD TenantId int NULL
END
GO
update RoomStatus set TenantId=1
GO
 IF COL_LENGTH('RoomType','TenantId') IS NULL
 BEGIN
 ALTER TABLE RoomType  ADD TenantId int NULL
END
GO
update RoomType set TenantId=1
GO
 IF COL_LENGTH('RoomTypeGrouping','TenantId') IS NULL
 BEGIN
 ALTER TABLE RoomTypeGrouping  ADD TenantId int NULL
END
GO
update RoomTypeGrouping set TenantId=1
GO
 IF COL_LENGTH('Staff','TenantId') IS NULL
 BEGIN
 ALTER TABLE Staff  ADD TenantId int NULL
END
GO
update Staff set TenantId=1
GO
 IF COL_LENGTH('TravelType','TenantId') IS NULL
 BEGIN
 ALTER TABLE TravelType  ADD TenantId int NULL
END
GO
update TravelType set TenantId=1
GO
 IF COL_LENGTH('Title','TenantId') IS NULL
 BEGIN
 ALTER TABLE Title  ADD TenantId int NULL
END
GO
update Title set TenantId=1
GO
 IF COL_LENGTH('AttendantCheckList','TenantId') IS NULL
 BEGIN
 ALTER TABLE AttendantCheckList  ADD TenantId int NULL
END
GO
update AttendantCheckList set TenantId=1
GO
 IF COL_LENGTH('BillingCode','TenantId') IS NULL
 BEGIN
 ALTER TABLE BillingCode  ADD TenantId int NULL
END
GO
update BillingCode set TenantId=1
GO
 IF COL_LENGTH('Booking','TenantId') IS NULL
 BEGIN
 ALTER TABLE Booking  ADD TenantId int NULL
END
GO
update Booking set TenantId=1
GO
 IF COL_LENGTH('Campaign','TenantId') IS NULL
 BEGIN
 ALTER TABLE Campaign  ADD TenantId int NULL
END
GO
update Campaign set TenantId=1
GO
 IF COL_LENGTH('Cancellation','TenantId') IS NULL
 BEGIN
 ALTER TABLE Cancellation  ADD TenantId int NULL
END
GO
update Cancellation set TenantId=1
GO
 IF COL_LENGTH('CancellationRule','TenantId') IS NULL
 BEGIN
 ALTER TABLE CancellationRule  ADD TenantId int NULL
END
GO
update CancellationRule set TenantId=1
GO
 IF COL_LENGTH('CheckList','TenantId') IS NULL
 BEGIN
 ALTER TABLE CheckList  ADD TenantId int NULL
END
GO
update CheckList set TenantId=1
GO
 IF COL_LENGTH('Company','TenantId') IS NULL
 BEGIN
 ALTER TABLE Company  ADD TenantId int NULL
END
GO
update Company set TenantId=1
GO
 IF COL_LENGTH('Currency','TenantId') IS NULL
 BEGIN
 ALTER TABLE Currency  ADD TenantId int NULL
END
GO
update Currency set TenantId=1
GO
 IF COL_LENGTH('Department','TenantId') IS NULL
 BEGIN
 ALTER TABLE Department  ADD TenantId int NULL
END
GO
update Department set TenantId=1
GO
 IF COL_LENGTH('EventType','TenantId') IS NULL
 BEGIN
 ALTER TABLE EventType  ADD TenantId int NULL
END
GO
update EventType set TenantId=1
GO
 IF COL_LENGTH('GeneralProfile','TenantId') IS NULL
 BEGIN
 ALTER TABLE GeneralProfile  ADD TenantId int NULL
END
GO
update GeneralProfile set TenantId=1
GO
 IF COL_LENGTH('Group1','TenantId') IS NULL
 BEGIN
 ALTER TABLE Group1  ADD TenantId int NULL
END
GO
update Group1 set TenantId=1
GO
 IF COL_LENGTH('Group2','TenantId') IS NULL
 BEGIN
 ALTER TABLE Group2  ADD TenantId int NULL
END
GO
update Group2 set TenantId=1
GO
 IF COL_LENGTH('Group3','TenantId') IS NULL
 BEGIN
 ALTER TABLE Group3  ADD TenantId int NULL
END
GO
update Group3 set TenantId=1
GO
 IF COL_LENGTH('Group4','TenantId') IS NULL
 BEGIN
 ALTER TABLE Group4  ADD TenantId int NULL
END
GO
update Group4 set TenantId=1
GO
 IF COL_LENGTH('Guest','TenantId') IS NULL
 BEGIN
 ALTER TABLE Guest  ADD TenantId int NULL
END
GO
update Guest set TenantId=1
GO
 IF COL_LENGTH('GuestIdentityType','TenantId') IS NULL
 BEGIN
 ALTER TABLE GuestIdentityType  ADD TenantId int NULL
END
GO
update GuestIdentityType set TenantId=1
GO
 IF COL_LENGTH('Item','TenantId') IS NULL
 BEGIN
 ALTER TABLE Item  ADD TenantId int NULL
END
GO
update Item set TenantId=1
GO
 IF COL_LENGTH('LostFound','TenantId') IS NULL
 BEGIN
 ALTER TABLE LostFound  ADD TenantId int NULL
END
GO
update LostFound set TenantId=1
GO
 IF COL_LENGTH('LostFoundImage','TenantId') IS NULL
 BEGIN
 ALTER TABLE LostFoundImage  ADD TenantId int NULL
END
GO
update LostFoundImage set TenantId=1
GO
 IF COL_LENGTH('LostFoundStatus','TenantId') IS NULL
 BEGIN
 ALTER TABLE LostFoundStatus  ADD TenantId int NULL
END
GO
update LostFoundStatus set TenantId=1
GO
 IF COL_LENGTH('MaidStatus','TenantId') IS NULL
 BEGIN
 ALTER TABLE MaidStatus  ADD TenantId int NULL
END
GO
update MaidStatus set TenantId=1
GO
 IF COL_LENGTH('MTechnician','TenantId') IS NULL
 BEGIN
 ALTER TABLE MTechnician  ADD TenantId int NULL
END
GO
update MTechnician set TenantId=1
GO
 IF COL_LENGTH('MWorkOrderStatus','TenantId') IS NULL
 BEGIN
 ALTER TABLE MWorkOrderStatus  ADD TenantId int NULL
END
GO
update MWorkOrderStatus set TenantId=1
GO
 IF COL_LENGTH('MWorkType','TenantId') IS NULL
 BEGIN
 ALTER TABLE MWorkType  ADD TenantId int NULL
END
GO
update MWorkType set TenantId=1
GO
 IF COL_LENGTH('Nationality','TenantId') IS NULL
 BEGIN
 ALTER TABLE Nationality  ADD TenantId int NULL
END
GO
update Nationality set TenantId=1
GO
 IF COL_LENGTH('PaymentGroup','TenantId') IS NULL
 BEGIN
 ALTER TABLE PaymentGroup  ADD TenantId int NULL
END
GO
update PaymentGroup set TenantId=1
GO
 IF COL_LENGTH('PaymentType','TenantId') IS NULL
 BEGIN
 ALTER TABLE PaymentType  ADD TenantId int NULL
END
GO
update PaymentType set TenantId=1
GO
 IF COL_LENGTH('Postcode','TenantId') IS NULL
 BEGIN
 ALTER TABLE Postcode  ADD TenantId int NULL
END
GO
update Postcode set TenantId=1
GO
 IF COL_LENGTH('RateType','TenantId') IS NULL
 BEGIN
 ALTER TABLE RateType  ADD TenantId int NULL
END
GO
update RateType set TenantId=1
GO
 IF COL_LENGTH('RateTypeGrouping','TenantId') IS NULL
 BEGIN
 ALTER TABLE RateTypeGrouping  ADD TenantId int NULL
END
GO
update RateTypeGrouping set TenantId=1
GO
 IF COL_LENGTH('Region','TenantId') IS NULL
 BEGIN
 ALTER TABLE Region  ADD TenantId int NULL
END
GO
update Region set TenantId=1
GO
 IF COL_LENGTH('Report','TenantId') IS NULL
 BEGIN
 ALTER TABLE Report  ADD TenantId int NULL
END
GO
update Report set TenantId=1
GO
 IF COL_LENGTH('ReportBatch','TenantId') IS NULL
 BEGIN
 ALTER TABLE ReportBatch  ADD TenantId int NULL
END
GO
update ReportBatch set TenantId=1
GO
 IF COL_LENGTH('ReportSecurity','TenantId') IS NULL
 BEGIN
 ALTER TABLE ReportSecurity  ADD TenantId int NULL
END
GO
update ReportSecurity set TenantId=1
GO
 IF COL_LENGTH('Reservation','TenantId') IS NULL
 BEGIN
 ALTER TABLE Reservation  ADD TenantId int NULL
END
GO
update Reservation set TenantId=1
GO
 IF COL_LENGTH('ReservationAdditional','TenantId') IS NULL
 BEGIN
 ALTER TABLE ReservationAdditional  ADD TenantId int NULL
END
GO
update ReservationAdditional set TenantId=1
GO
 IF COL_LENGTH('ReservationGuest','TenantId') IS NULL
 BEGIN
 ALTER TABLE ReservationGuest  ADD TenantId int NULL
END
GO
update ReservationGuest set TenantId=1
GO
 IF COL_LENGTH('ReservationRate','TenantId') IS NULL
 BEGIN
 ALTER TABLE ReservationRate  ADD TenantId int NULL
END
GO
update ReservationRate set TenantId=1
GO
 IF COL_LENGTH('SecurityProfile','TenantId') IS NULL
 BEGIN
 ALTER TABLE SecurityProfile  ADD TenantId int NULL
END
GO
update SecurityProfile set TenantId=1
GO
 IF COL_LENGTH('SqoopeGroup','TenantId') IS NULL
 BEGIN
 ALTER TABLE SqoopeGroup  ADD TenantId int NULL
END
GO
update SqoopeGroup set TenantId=1
GO
 IF COL_LENGTH('SqoopeGroupLink','TenantId') IS NULL
 BEGIN
 ALTER TABLE SqoopeGroupLink  ADD TenantId int NULL
END
GO
update SqoopeGroupLink set TenantId=1
GO
 IF COL_LENGTH('SqoopeMsgGroupLink','TenantId') IS NULL
 BEGIN
 ALTER TABLE SqoopeMsgGroupLink  ADD TenantId int NULL
END
GO
update SqoopeMsgGroupLink set TenantId=1
GO
 IF COL_LENGTH('SqoopeMsgLog','TenantId') IS NULL
 BEGIN
 ALTER TABLE SqoopeMsgLog  ADD TenantId int NULL
END
GO
update SqoopeMsgLog set TenantId=1
GO
 IF COL_LENGTH('SqoopeMsgType','TenantId') IS NULL
 BEGIN
 ALTER TABLE SqoopeMsgType  ADD TenantId int NULL
END
GO
update SqoopeMsgType set TenantId=1
GO
 IF COL_LENGTH('StaffDepartment','TenantId') IS NULL
 BEGIN
 ALTER TABLE StaffDepartment  ADD TenantId int NULL
END
GO
update StaffDepartment set TenantId=1
GO
 IF COL_LENGTH('SupervisorCheckList','TenantId') IS NULL
 BEGIN
 ALTER TABLE SupervisorCheckList  ADD TenantId int NULL
END
GO
update SupervisorCheckList set TenantId=1
GO
 IF COL_LENGTH('TravelReason','TenantId') IS NULL
 BEGIN
 ALTER TABLE TravelReason  ADD TenantId int NULL
END
GO
update TravelReason set TenantId=1
GO
 IF COL_LENGTH('MWorkTimeSheet','TenantId') IS NULL
 BEGIN
 ALTER TABLE MWorkTimeSheet  ADD TenantId int NULL
END
GO
update MWorkTimeSheet set TenantId=1
GO
 IF COL_LENGTH('MPriority','TenantId') IS NULL
 BEGIN
 ALTER TABLE MPriority  ADD TenantId int NULL
END
GO
update MPriority set TenantId=1
GO
 IF COL_LENGTH('RoomBlock','TenantId') IS NULL
 BEGIN
 ALTER TABLE RoomBlock  ADD TenantId int NULL
END
GO
update RoomBlock set TenantId=1
GO
 IF COL_LENGTH('MWorkTimeSheetNoteTemplate','TenantId') IS NULL
 BEGIN
 ALTER TABLE MWorkTimeSheetNoteTemplate  ADD TenantId int NULL
END
GO
update MWorkTimeSheetNoteTemplate set TenantId=1
GO
 IF COL_LENGTH('MWorkNotes','TenantId') IS NULL
 BEGIN
 ALTER TABLE MWorkNotes  ADD TenantId int NULL
END
GO
update MWorkNotes set TenantId=1

GO
 IF COL_LENGTH('EmailHistory','TenantId') IS NULL
 BEGIN
 ALTER TABLE EmailHistory  ADD TenantId int NULL
END
GO
update EmailHistory set TenantId=1

GO
 IF COL_LENGTH('EmailList','TenantId') IS NULL
 BEGIN
 ALTER TABLE EmailList  ADD TenantId int NULL
END
GO
update EmailList set TenantId=1

GO
 IF COL_LENGTH('City','TenantId') IS NULL
 BEGIN
 ALTER TABLE City  ADD TenantId int NULL
END
GO
update City set TenantId=1

GO
 IF COL_LENGTH('Company','TenantId') IS NULL
 BEGIN
 ALTER TABLE Company  ADD TenantId int NULL
END
GO
update Company set TenantId=1

GO
 IF COL_LENGTH('ReservationBillingContact','TenantId') IS NULL
 BEGIN
 ALTER TABLE ReservationBillingContact  ADD TenantId int NULL
END
GO
update ReservationBillingContact set TenantId=1

GO
 IF COL_LENGTH('Request','TenantId') IS NULL
 BEGIN
 ALTER TABLE Request  ADD TenantId int NULL
END
GO
update Request set TenantId=1

GO
 IF COL_LENGTH('Postcode','TenantId') IS NULL
 BEGIN
 ALTER TABLE Postcode  ADD TenantId int NULL
END
GO
update Postcode set TenantId=1

GO
 IF COL_LENGTH('PaymentType','TenantId') IS NULL
 BEGIN
 ALTER TABLE PaymentType  ADD TenantId int NULL
END
GO
update PaymentType set TenantId=1

GO
 IF COL_LENGTH('BillingCode','TenantId') IS NULL
 BEGIN
 ALTER TABLE BillingCode  ADD TenantId int NULL
END
GO
update BillingCode set TenantId=1

GO
 IF COL_LENGTH('ReservationAdditional','TenantId') IS NULL
 BEGIN
 ALTER TABLE ReservationAdditional  ADD TenantId int NULL
END
GO
update ReservationAdditional set TenantId=1

GO
 IF COL_LENGTH('GuestAdditional','TenantId') IS NULL
 BEGIN
 ALTER TABLE GuestAdditional  ADD TenantId int NULL
END
GO
update GuestAdditional set TenantId=1

GO
 IF COL_LENGTH('ReservationRoom','TenantId') IS NULL
 BEGIN
 ALTER TABLE ReservationRoom  ADD TenantId int NULL
END
GO
update ReservationRoom set TenantId=1

GO
 IF COL_LENGTH('AllotmentLine','TenantId') IS NULL
 BEGIN
 ALTER TABLE AllotmentLine  ADD TenantId int NULL
END
GO
update AllotmentLine set TenantId=1

GO
 IF COL_LENGTH('AllotmentHdr','TenantId') IS NULL
 BEGIN
 ALTER TABLE AllotmentHdr  ADD TenantId int NULL
END
GO
update AllotmentHdr set TenantId=1

