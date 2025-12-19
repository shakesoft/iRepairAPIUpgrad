using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class StaffDto
    {
        #region Constructor
        public StaffDto() { }
        #endregion


        #region Properties
        public Guid StaffKey { get; set; }
        

        public string UserName { get; set; }

        //public Guid? UserTemplateKey { get; set; }

        public Guid? MaidKey { get; set; }

        public Guid? TechnicianKey { get; set; }

        public int? TechnicianID { get; set; }

        public int? Active { get; set; }

        //public int? Administrator { get; set; }

        //public string FullName { get; set; }

        //public string JobPosition { get; set; }

        //public int? Sync { get; set; }

        //public byte[] TS { get; set; }

        //public int? MAdministrator { get; set; }

        public int? Sec_Supervisor { get; set; }

        public int? Sec_TechSupervisor { get; set; }

        //public string MPassword { get; set; }

        //public string Password { get; set; }

        //public int? Sec_ActivatePABX { get; set; }

        //public int? Sec_AnnouncementSetup { get; set; }

        //public int? Sec_ARAdjustment { get; set; }

        //public int? Sec_ARAllocation { get; set; }

        //public int? Sec_ARCN { get; set; }

        //public int? Sec_ARDN { get; set; }

        //public int? Sec_ARPayment { get; set; }

        //public int? Sec_ARTransactions { get; set; }

        //public int? Sec_ARTransfer { get; set; }

        //public int? Sec_AssignRoom { get; set; }

        //public int? Sec_BatchPosting { get; set; }

        //public int? Sec_BillingCodeSetup { get; set; }

        public int? Sec_BlockRoom { get; set; }

        //public int? Sec_CancellationSetup { get; set; }

        //public int? Sec_CancelReservation { get; set; }

        //public int? Sec_CanUncheckIn { get; set; }

        //public int? Sec_Cashier { get; set; }

        //public int? Sec_ChangeARCompany { get; set; }

        //public int? Sec_ChangeBusinessDate { get; set; }

        //public int? Sec_ChangeLedger { get; set; }

        //public int? Sec_ChangeRate { get; set; }

        //public int? Sec_CheckIn { get; set; }

        //public int? Sec_CheckOut { get; set; }

        //public int? Sec_CitySetup { get; set; }

        //public int? Sec_CloseOthersShift { get; set; }

        //public int? Sec_Company { get; set; }

        //public int? Sec_CompanyCRM { get; set; }

        //public int? Sec_CompanyFinance { get; set; }

        //public int? Sec_CompanyType_Setup { get; set; }

        //public int? Sec_Concierges { get; set; }

        //public int? Sec_ConfirmReservation { get; set; }

        //public int? Sec_ConfirmRoomChange { get; set; }

        //public int? Sec_ConsolidatePosting { get; set; }

        //public int? Sec_CopyReservation { get; set; }

        //public int? Sec_EventBanquet { get; set; }

        //public int? Sec_EventBookingTypeSetup { get; set; }

        //public int? Sec_EventInventorySetup { get; set; }

        //public int? Sec_FacilitySetup { get; set; }

        //public int? Sec_FNBSetup { get; set; }

        //public int? Sec_Folio { get; set; }

        //public int? Sec_ForeignCurrencySetup { get; set; }

        //public int? Sec_FrontDeskCheckIn { get; set; }

        //public int? Sec_FrontDeskCheckOut { get; set; }

        //public int? Sec_GroupingSetup { get; set; }

        //public int? Sec_Guest { get; set; }

        //public int? Sec_HMM { get; set; }

        //public int? Sec_Holiday_Setup { get; set; }

        //public int? Sec_InactivateGuest { get; set; }

        //public int? Sec_ItemSetup { get; set; }

        //public int? Sec_LockupSetup { get; set; }

        //public int? Sec_ManualPosting { get; set; }

        //public int? Sec_MoveReservation { get; set; }

        //public int? Sec_NewPendingReservation { get; set; }

        //public int? Sec_NewReservation { get; set; }

        //public int? Sec_NightAudit { get; set; }

        //public int? Sec_OverBookingSetup { get; set; }

        //public int? SEC_Override_EndShift { get; set; }

        //public int? Sec_PaymentGroupSetup { get; set; }

        //public int? Sec_PaymentTypeSetup { get; set; }

        //public int? Sec_Payout { get; set; }

        //public int? Sec_PostCodeSetup { get; set; }

        //public int? Sec_PriceSetup { get; set; }

        //public int? Sec_ReCheckIn { get; set; }

        //public int? Sec_Reservation { get; set; }

        //public int? Sec_RoomAllotmentSetup { get; set; }

        //public int? Sec_RoomAvailability { get; set; }

        //public int? Sec_RoomSetup { get; set; }

        //public int? Sec_SearchRate { get; set; }

        //public int? Sec_ShowDayEndBalance { get; set; }

        //public int? Sec_ShowShiftBalance { get; set; }

        //public int? Sec_SourceSetup { get; set; }

        //public int? Sec_Split_Bill { get; set; }

        //public int? Sec_SystemBalancing { get; set; }

        //public int? Sec_TitleSetup { get; set; }

        //public int? Sec_TransferPosting { get; set; }

        //public int? Sec_VirtualFolioPosting { get; set; }

        //public int? Sec_VoidPosting { get; set; }

        //public Guid? SecurityProfileKey { get; set; }

        //public int Seq { get; set; }

        //public int? Sort { get; set; }

        public int? Contact_ID { get; set; }

        // public Guid? StaffDepartmentKey { get; set; }

        public string FirebaseToken_Id { get; set; }
        #endregion
    }
}
