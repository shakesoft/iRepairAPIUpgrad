using Abp.Domain.Entities;
using BEZNgCore.IrepairControl;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("Staff")]
    public class Staff : Entity<Guid>, IMayHaveTenant
    {
        [Column("StaffKey")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }

        public virtual int? Active { get; set; }

        [StringLength(StaffConsts.MaxUserNameLength, MinimumLength = StaffConsts.MinUserNameLength)]
        public virtual string UserName { get; set; }

        [StringLength(StaffConsts.MaxPasswordLength, MinimumLength = StaffConsts.MinPasswordLength)]
        public virtual string Password { get; set; }

        [StringLength(StaffConsts.MaxFullNameLength, MinimumLength = StaffConsts.MinFullNameLength)]
        public virtual string FullName { get; set; }

        [StringLength(StaffConsts.MaxJobPositionLength, MinimumLength = StaffConsts.MinJobPositionLength)]
        public virtual string JobPosition { get; set; }

        public virtual int? Administrator { get; set; }

        public virtual Guid? SecurityProfileKey { get; set; }

        public virtual int? Sort { get; set; }

        public virtual int? Sync { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }

        [Timestamp]
        public virtual byte[] TS { get; set; }

        public virtual int? Sec_Guest { get; set; }

        public virtual int? Sec_Company { get; set; }

        public virtual int? Sec_BillingCodeSetup { get; set; }

        public virtual int? Sec_PostCodeSetup { get; set; }

        public virtual int? Sec_PaymentTypeSetup { get; set; }

        public virtual int? Sec_RoomSetup { get; set; }

        public virtual int? Sec_PriceSetup { get; set; }

        public virtual int? Sec_RoomAllotmentSetup { get; set; }

        public virtual int? Sec_ItemSetup { get; set; }

        public virtual int? Sec_GroupingSetup { get; set; }

        public virtual int? Sec_CancellationSetup { get; set; }

        public virtual int? Sec_AnnouncementSetup { get; set; }

        public virtual int? Sec_ARTransactions { get; set; }

        public virtual int? Sec_ARPayment { get; set; }

        public virtual int? Sec_ARAdjustment { get; set; }

        public virtual int? Sec_RoomAvailability { get; set; }

        public virtual int? Sec_SearchRate { get; set; }

        public virtual int? Sec_Reservation { get; set; }

        public virtual int? Sec_Folio { get; set; }

        public virtual int? Sec_FrontDeskCheckIn { get; set; }

        public virtual int? Sec_FrontDeskCheckOut { get; set; }

        public virtual int? Sec_NewReservation { get; set; }

        public virtual int? Sec_AssignRoom { get; set; }

        public virtual int? Sec_CheckIn { get; set; }

        public virtual int? Sec_CancelReservation { get; set; }

        public virtual int? Sec_MoveReservation { get; set; }

        public virtual int? Sec_CopyReservation { get; set; }

        public virtual int? Sec_ConfirmReservation { get; set; }

        public virtual int? Sec_ARAllocation { get; set; }

        public virtual int? Sec_CheckOut { get; set; }

        public virtual int? Sec_CitySetup { get; set; }

        public virtual int? Sec_BlockRoom { get; set; }

        public virtual int? Sec_ReCheckIn { get; set; }

        public virtual int? Sec_ConfirmRoomChange { get; set; }

        public virtual int? Sec_CompanyFinance { get; set; }

        public virtual int? Sec_Holiday_Setup { get; set; }

        public virtual int? Sec_ActivatePABX { get; set; }

        public virtual int? Sec_ChangeBusinessDate { get; set; }

        public virtual int? Sec_Cashier { get; set; }

        public virtual int? SEC_Override_EndShift { get; set; }

        public virtual int? Sec_ARCN { get; set; }

        public virtual int? Sec_ARDN { get; set; }

        public virtual int? Sec_PaymentGroupSetup { get; set; }

        public virtual Guid? StaffDepartmentKey { get; set; }

        public virtual int? Sec_ShowShiftBalance { get; set; }

        public virtual int? Sec_ChangeARCompany { get; set; }

        public virtual int? Sec_ShowDayEndBalance { get; set; }

        public virtual int? Sec_CloseOthersShift { get; set; }

        public virtual int? Sec_BatchPosting { get; set; }

        public virtual int? Sec_SourceSetup { get; set; }

        public virtual int? Sec_VoidPosting { get; set; }

        public virtual int? Sec_TransferPosting { get; set; }

        public virtual int? Sec_ManualPosting { get; set; }

        public virtual int? Sec_ConsolidatePosting { get; set; }

        public virtual int? Sec_TitleSetup { get; set; }

        public virtual int? Sec_InactivateGuest { get; set; }

        public virtual int? Sec_NightAudit { get; set; }

        [StringLength(StaffConsts.MaxMPasswordLength, MinimumLength = StaffConsts.MinMPasswordLength)]
        public virtual string MPassword { get; set; }

        public virtual int? Sec_OverBookingSetup { get; set; }

        public virtual int? MAdministrator { get; set; }

        public virtual int? Sec_ChangeLedger { get; set; }

        public virtual int? Sec_ForeignCurrencySetup { get; set; }

        public virtual int? Sec_VirtualFolioPosting { get; set; }

        public virtual int? Sec_SystemBalancing { get; set; }

        public virtual int? Sec_EventBanquet { get; set; }

        public virtual int? Sec_FacilitySetup { get; set; }

        public virtual int? Sec_ChangeRate { get; set; }

        public virtual int? Sec_FNBSetup { get; set; }

        public virtual int? Sec_EventBookingTypeSetup { get; set; }

        public virtual int? Sec_EventInventorySetup { get; set; }

        public virtual int? Sec_Concierges { get; set; }

        public virtual int? Sec_CompanyCRM { get; set; }

        public virtual int? Sec_LockupSetup { get; set; }

        public virtual int? Sec_CompanyType_Setup { get; set; }

        public virtual int? Sec_Payout { get; set; }

        public virtual int? Sec_ARTransfer { get; set; }

        public virtual int? Sec_Split_Bill { get; set; }

        public virtual int? Sec_NewPendingReservation { get; set; }

        public virtual int? Sec_HMM { get; set; }

        public virtual int? Sec_CanUncheckIn { get; set; }

        public virtual Guid? UserTemplateKey { get; set; }

        [StringLength(StaffConsts.MaxPINLength, MinimumLength = StaffConsts.MinPINLength)]
        public virtual string PIN { get; set; }

        public virtual Guid? MaidKey { get; set; }

        public virtual Guid? TechnicianKey { get; set; }

        public virtual int? Sec_Supervisor { get; set; }


        [StringLength(StaffConsts.MaxUserNameLength, MinimumLength = StaffConsts.MinUserNameLength)]
        public virtual string Contact_Id { get; set; }

        public virtual int? Sec_TechSupervisor { get; set; }

        public virtual int? OverwriteRate { get; set; }

        public virtual int? Donotmove { get; set; }

        public virtual int? Sec_UnCancelReservation { get; set; }

        public virtual int? Sec_UserSecurity { get; set; }

        public virtual int? Sec_UserTemplate { get; set; }

        public virtual int? Sec_PMTDecrypt { get; set; }

        public virtual int? Sec_Maid { get; set; }

        public virtual int? Sec_CAttendant { get; set; }

        public virtual int? Sec_AAttendant { get; set; }

        public virtual int? Sec_PromoModule { get; set; }

        public virtual int? Sec_BatchCheckOut { get; set; }

        public virtual Guid? BreakfastStaff { get; set; }

        public virtual int? AnywhereAccess { get; set; }

        public virtual int? Sec_Overbooking { get; set; }

        public virtual int? Sec_ReservationPaymentAdd { get; set; }

        public virtual int? Sec_ReservationPaymentShow { get; set; }

        public virtual int? Sec_ReservationPaymentDelete { get; set; }

        public virtual int? Sec_NegativePosting { get; set; }

        public virtual int? Sec_MoveFitToGroup { get; set; }

        [StringLength(StaffConsts.MaxComputerNameLength, MinimumLength = StaffConsts.MinComputerNameLength)]
        public virtual string ComputerName { get; set; }

        [StringLength(StaffConsts.MaxEftposIPLength, MinimumLength = StaffConsts.MinEftposIPLength)]
        public virtual string EftposIP { get; set; }

        [StringLength(StaffConsts.MaxEftposIDLength, MinimumLength = StaffConsts.MinEftposIDLength)]
        public virtual string EftposID { get; set; }

        [StringLength(StaffConsts.MaxEftposSNLength, MinimumLength = StaffConsts.MinEftposSNLength)]
        public virtual string EftposSN { get; set; }

        public virtual int? Sec_Stats { get; set; }

        public virtual int? Sec_VirtualCheckIn { get; set; }

        public virtual int? Sec_GroupOverbook { get; set; }

        public virtual int? Sec_OverrideCredit { get; set; }

        public virtual int? Sec_PassportDecrypt { get; set; }

        [StringLength(2000, MinimumLength = 0)]
        public virtual string FirebaseToken_Id { get; set; }

        public virtual int? Sec_JobPosition { get; set; }
        public virtual int? Sec_IPSetUp { get; set; }
        public virtual int? Sec_IPViewLog { get; set; }
        public virtual int? Sec_IPAssignTasks { get; set; }
        public virtual int? Sec_IPBlockRoom { get; set; }

        public virtual int? Sec_SupervisorB { get; set; }
        public virtual int? Sec_SupervisorMode { get; set; }
        public virtual int? Sec_Rooms { get; set; }
        public virtual int? Sec_MiniBar { get; set; }
        public virtual int? Sec_MiniBarCo { get; set; }
        public virtual int? Sec_Laundry { get; set; }
        public virtual int? Sec_LostFound { get; set; }
        public virtual int? Sec_WOEntry { get; set; }
        public virtual int? Sec_ViewLogs { get; set; }
        public virtual int? Sec_RoomstoInspect { get; set; }
        public virtual int? Sec_GuestRequest { get; set; }
        public virtual int? Sec_AllowCleanDirectly { get; set; }

        [StringLength(2000, MinimumLength = 0)]
        public virtual string FirebaseToken_IdiRepair { get; set; }
        public virtual int? Sec_OverrideRestriction { get; set; }

        public virtual int? ChangeCleanStatus { get; set; }

    }
}

