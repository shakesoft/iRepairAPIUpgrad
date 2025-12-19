
/****** For Multi Tenancy update for each respective tenant id******/

update Control set TenantId=1
update GuestStatus set TenantId=1
update History set TenantId=1 where ModuleName='iClean' or ModuleName='iRepair'
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
update MWorkTimeSheet set TenantId=1
update MPriority set TenantId=1
update RoomBlock set TenantId=1
update MWorkTimeSheetNoteTemplate set TenantId=1
update MWorkNotes set TenantId=1

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