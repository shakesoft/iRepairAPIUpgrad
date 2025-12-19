using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("ReservationGuest")]
    public class ReservationGuest : Entity<Guid>, IMayHaveTenant
    {
        [Column("ReservationGuestKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid? ReservationKey { get; set; }
        public virtual Guid? ReservationRoomKey { get; set; }
        public virtual Guid? GuestKey { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual int? GuestStay { get; set; }
        public virtual DateTime? CheckInDate { get; set; }
        public virtual DateTime? CheckOutDate { get; set; }
        public virtual Guid? ShareGuestKey { get; set; }
        public virtual DateTime? X_CheckInDate { get; set; }
        public virtual DateTime? X_CheckOutDate { get; set; }
        public virtual int? ReservationStatus { get; set; }
        public virtual DateTime? X_BillCheckInDate { get; set; }
        public virtual DateTime? X_BillCheckOutDate { get; set; }
        public virtual DateTime? X_Bill2CheckInDate { get; set; }
        public virtual DateTime? X_Bill2CheckOutDate { get; set; }
        public virtual DateTime? X_Bill3CheckInDate { get; set; }
        public virtual DateTime? X_Bill3CheckOutDate { get; set; }
        public virtual DateTime? X_Bill4CheckInDate { get; set; }
        public virtual DateTime? X_Bill4CheckOutDate { get; set; }
        public virtual DateTime? X_Bill5CheckInDate { get; set; }
        public virtual DateTime? X_Bill5CheckOutDate { get; set; }
        public virtual DateTime? X_Bill6aCheckInDate { get; set; }
        public virtual DateTime? X_Bill6aCheckOutDate { get; set; }
        public virtual DateTime? X_Bill6bCheckInDate { get; set; }
        public virtual DateTime? X_Bill6bCheckOutDate { get; set; }
        public virtual DateTime? X_Bill7CheckInDate { get; set; }
        public virtual DateTime? X_Bill7CheckOutDate { get; set; }
        public virtual DateTime? X_SHNBill1CheckInDate { get; set; }
        public virtual DateTime? X_SHNBill1CheckOutDate { get; set; }
        public virtual DateTime? X_SHNBill2CheckInDate { get; set; }
        public virtual DateTime? X_SHNBill2CheckOutDate { get; set; }
        public virtual DateTime? X_SHNBill3CheckInDate { get; set; }
        public virtual DateTime? X_SHNBill3CheckOutDate { get; set; }
        public virtual DateTime? X_SHNBill4CheckInDate { get; set; }
        public virtual DateTime? X_SHNBill4CheckOutDate { get; set; }
        public virtual DateTime? X_SHNBill5CheckInDate { get; set; }
        public virtual DateTime? X_SHNBill5CheckOutDate { get; set; }
        public virtual DateTime? X_SHNBill6CheckInDate { get; set; }
        public virtual DateTime? X_SHNBill6CheckOutDate { get; set; }
        public virtual DateTime? X_SHNBill7CheckInDate { get; set; }
        public virtual DateTime? X_SHNBill7CheckOutDate { get; set; }
    }
}
