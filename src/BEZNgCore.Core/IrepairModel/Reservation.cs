using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("Reservation")]
    public class Reservation : Entity<Guid>, IMayHaveTenant
    {
        [Column("ReservationKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual DateTime? DocDate { get; set; }
        public virtual Guid? RateTypeKey { get; set; }

        [StringLength(1, MinimumLength = 0)]
        public virtual string ReservationType { get; set; }
        public virtual Guid? TravelAgentKey { get; set; }
        public virtual Guid? DebtorKey { get; set; }
        public virtual Guid? CompanyKey { get; set; }
        public virtual Guid? GuestKey { get; set; }
        public virtual int? Status { get; set; }
        public virtual Guid? RoomTypeKey { get; set; }
        public virtual Guid? RoomKey { get; set; }
        public virtual int? AdditionalBed { get; set; }
        public virtual DateTime CheckInDate { get; set; }
        [StringLength(5, MinimumLength = 0)]
        public virtual string CheckInTime { get; set; }
        public virtual DateTime CheckOutDate { get; set; }
        [StringLength(5, MinimumLength = 0)]
        public virtual string CheckOutTime { get; set; }
        [StringLength(30, MinimumLength = 0)]
        public virtual string Flight { get; set; }
        public virtual Guid? PromotionKey { get; set; }
        public virtual Guid? RateKey { get; set; }
        public virtual Guid? SourceKey { get; set; }
        public virtual Guid? MarketKey { get; set; }
        public virtual Guid? BookingChannelKey { get; set; }
        public virtual Guid? GeoSourceKey { get; set; }
        public virtual Guid? SecurityKey { get; set; }
        public virtual DateTime? PostDate { get; set; }
        public virtual Guid? PaymentTypeKey { get; set; }
        [StringLength(60, MinimumLength = 0)]
        public virtual string CardNo { get; set; }
        public virtual int? Adult { get; set; }
        public virtual int? Child { get; set; }
        public virtual Guid? LastModifiedStaff { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string ETA { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string ETD { get; set; }
        public virtual Guid? BookingKey { get; set; }
        public virtual Guid? CampaignKey { get; set; }
        public virtual Guid? TravelReasonKey { get; set; }
        public virtual Guid? TravelTypeKey { get; set; }
        public virtual int? PaymentStatus { get; set; }
        public virtual decimal? DepositAmount { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Ref1 { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Ref2 { get; set; }
        public virtual Guid? CancellationKey { get; set; }
        public virtual Guid? GroupReservationKey { get; set; }
        [StringLength(60, MinimumLength = 0)]
        public virtual string VoucherNo { get; set; }
        [StringLength(30, MinimumLength = 0)]
        public virtual string CCExpiry { get; set; }
        public virtual int? COA { get; set; }
        public virtual Guid? DepartmentKey { get; set; }
        public virtual Guid? BookingAgentKey { get; set; }
        public virtual int? Guaranteed { get; set; }
        public virtual DateTime? NightAuditRoomChargeDate { get; set; }
        public virtual DateTime? NightAuditAddItemDate { get; set; }
        public virtual DateTime? CancellationDate { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Department { get; set; }
        [StringLength(120, MinimumLength = 0)]
        public virtual string DepartmentAddress { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string DepartmentPostal { get; set; }
        public virtual Guid? DepartmentCountryKey { get; set; }
        public virtual Guid? ReservationStaffKey { get; set; }
        public virtual Guid? CheckInStaffKey { get; set; }
        public virtual Guid? CheckOutStaffKey { get; set; }
        public virtual Guid? CancellationStaffKey { get; set; }
        public virtual Guid? Group1Key { get; set; }
        public virtual Guid? Group2Key { get; set; }
        public virtual Guid? Group3Key { get; set; }
        public virtual Guid? Group4Key { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string ReCheckInReason { get; set; }
        public virtual Guid? ReCheckInStaffKey { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string CompanyContact { get; set; }
        public virtual DateTime? OrgCheckOutDate { get; set; }
        public virtual Guid? ReportingRateTypeKey { get; set; }
        public virtual int? RecheckIn { get; set; }
        public virtual decimal? Folio_Limit { get; set; }
        public virtual int? Extra1 { get; set; }
        public virtual int? Extra2 { get; set; }
        public virtual int? Extra3 { get; set; }
        public virtual int? Extra4 { get; set; }
        public virtual int? Extra5 { get; set; }
        public virtual int? Extra6 { get; set; }
        public virtual int? Extra7 { get; set; }
        public virtual int? Extra8 { get; set; }
        public virtual int? Extra9 { get; set; }
        public virtual int? Extra10 { get; set; }
        public virtual int? Extra11 { get; set; }
        public virtual int? Extra12 { get; set; }
        public virtual int? Extra13 { get; set; }
        public virtual int? Extra14 { get; set; }
        public virtual int? Extra15 { get; set; }
        public virtual int? Extra16 { get; set; }
        public virtual int? Extra17 { get; set; }
        public virtual int? Extra18 { get; set; }
        public virtual int? Extra19 { get; set; }
        public virtual int? Extra20 { get; set; }
        public virtual int? Extra21 { get; set; }
        public virtual int? Extra22 { get; set; }
        public virtual int? Extra23 { get; set; }
        public virtual int? Extra24 { get; set; }
        public virtual int? Definite { get; set; }
        [StringLength(1, MinimumLength = 0)]
        public virtual string PrintRate { get; set; }
        public virtual Guid? GroupNationalityKey { get; set; }
        public virtual Guid? GroupRegionKey { get; set; }
        public virtual Guid? GroupCountryKey { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string CompanyPostal { get; set; }
        public virtual Guid? CompanyCountryKey { get; set; }
        public virtual decimal? MainFolioBalance { get; set; }
        public virtual decimal? OtherFolioBalance { get; set; }
        public virtual int? PayCommission { get; set; }
        public virtual int? ChargeBack { get; set; }
        [StringLength(1, MinimumLength = 0)]
        public virtual string RemarkToHistory { get; set; }
        public virtual int? GuestStay { get; set; }
        [StringLength(30, MinimumLength = 0)]
        public virtual string LoyaltyCardNo { get; set; }
        [StringLength(30, MinimumLength = 0)]
        public virtual string LoyaltyCard { get; set; }
        public virtual int? UseAllotment { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string CompanyTel { get; set; }
        public virtual DateTime? GuestPassportExpiry { get; set; }
        [StringLength(1, MinimumLength = 0)]
        public virtual string GuestGender { get; set; }
        public virtual DateTime? GuestDob { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string GuestTel { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string GuestMobile { get; set; }
        public virtual Guid? GuestNationalityKey { get; set; }
        public virtual Guid? GuestRegionKey { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string GuestPostal { get; set; }
        public virtual Guid? GuestCountryKey { get; set; }
        public virtual decimal? Folio_Limit2 { get; set; }
        public virtual int? GuestStatus { get; set; }
        public virtual int? RecheckInVirtualRoom { get; set; }
        public virtual DateTime? RecheckInDatetime { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string GuestEmail { get; set; }
        public virtual int? DoNotMoveRoom { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string GroupName { get; set; }
        [StringLength(512, MinimumLength = 0)]
        public virtual string CheckIn_Instruction { get; set; }
        [StringLength(512, MinimumLength = 0)]
        public virtual string CheckOut_Instruction { get; set; }
        [StringLength(512, MinimumLength = 0)]
        public virtual string Posting_Instruction { get; set; }
        [StringLength(120, MinimumLength = 0)]
        public virtual string CompanyAddress { get; set; }
        [StringLength(120, MinimumLength = 0)]
        public virtual string GuestAddress { get; set; }
        [StringLength(512, MinimumLength = 0)]
        public virtual string Remark { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ContactPerson { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string DepartmentCity { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string GroupCity { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string CompanyCity { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string GuestCity { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string CardName { get; set; }
        public virtual Guid? GuestIdentityTypeKey { get; set; }
        public virtual string GuestPassport { get; set; }
        public virtual Guid? TransferChargesToKey { get; set; }
        [StringLength(15, MinimumLength = 0)]
        public virtual string DocNo { get; set; }
        public virtual int? LongStay { get; set; }
        public virtual DateTime? CheckInDateOriginal { get; set; }
        public virtual DateTime? CheckOutDateOriginal { get; set; }
        public virtual DateTime? Cutoff_Date { get; set; }
        public virtual int? Cutoff_Days { get; set; }
        public virtual DateTime? Follow_Up_Date { get; set; }
        public virtual DateTime? Decision_Date { get; set; }
        public virtual Guid? BBQPitKey { get; set; }
        [StringLength(12, MinimumLength = 0)]
        public virtual string CheckOutDocNo { get; set; }
        public virtual Guid? PurposeStayKey { get; set; }
        public virtual int? PreCheckInCount { get; set; }
        public virtual int? GuestDND { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string GuestPhoneCOS { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string GuestLanguageCode { get; set; }
        [StringLength(30, MinimumLength = 0)]
        public virtual string CVV { get; set; }
        public virtual Guid? OrgRatePromotionTypeKey { get; set; }
        [StringLength(5, MinimumLength = 0)]
        public virtual string GuestArrived { get; set; }
        public virtual int? ExpressCheckOut { get; set; }
        [StringLength(60, MinimumLength = 0)]
        public virtual string GDS { get; set; }
        public virtual Guid? GroupBlockingKey { get; set; }
        [StringLength(256, MinimumLength = 0)]
        public virtual string GroupAddress { get; set; }
        [StringLength(15, MinimumLength = 0)]
        public virtual string GroupPostalCode { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string GroupPhone { get; set; }
        public virtual Guid? CommCodeKey { get; set; }
        public virtual int? CommPayable { get; set; }
        public virtual DateTime? RoomListDate { get; set; }
        public virtual Guid? CancellationRuleKey { get; set; }
        public virtual int? Fixed { get; set; }
        public virtual int? Elastic { get; set; }
        public virtual int? GroupStatus { get; set; }
        public virtual Guid? RoomToCharge { get; set; }
        public virtual int? DoNotContact { get; set; }
        public virtual int? Inventory { get; set; }
        public virtual Guid? BillTo2Key { get; set; }
        public virtual Guid? BillTo3Key { get; set; }
        public virtual Guid? BillTo4Key { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string GuestDND_old { get; set; }
        public virtual Guid? Users { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string Computer { get; set; }
        public virtual DateTime? Access { get; set; }
        [StringLength(60, MinimumLength = 0)]
        public virtual string tGuestPassport { get; set; }
        public virtual Guid? CompanyDepartmentKey { get; set; }
    }
}
