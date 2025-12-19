using Abp.Json;
using Abp.OpenIddict.Applications;
using Abp.OpenIddict.Authorizations;
using Abp.OpenIddict.EntityFrameworkCore;
using Abp.OpenIddict.Scopes;
using Abp.OpenIddict.Tokens;
using Abp.Zero.EntityFrameworkCore;
using BEZNgCore.Authorization.Delegation;
using BEZNgCore.Authorization.Roles;
using BEZNgCore.Authorization.Users;
using BEZNgCore.Authorization.Users;
using BEZNgCore.Chat;
using BEZNgCore.Editions;
using BEZNgCore.ExtraProperties;
using BEZNgCore.Friendships;
using BEZNgCore.IrepairModel;
using BEZNgCore.MultiTenancy;
using BEZNgCore.MultiTenancy.Accounting;
using BEZNgCore.MultiTenancy.Payments;
using BEZNgCore.Storage;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Org.BouncyCastle.Utilities.Collections;
using PayPalCheckoutSdk.Orders;
using System.Reflection.Metadata;
using Twilio.TwiML.Voice;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static QRCoder.PayloadGenerator;

namespace BEZNgCore.EntityFrameworkCore;

public class BEZNgCoreDbContext : AbpZeroDbContext<Tenant, Role, User, BEZNgCoreDbContext>, IOpenIddictDbContext
{
    /* Define an IDbSet for each entity of the application */

    public virtual DbSet<OpenIddictApplication> Applications { get; }

    public virtual DbSet<OpenIddictAuthorization> Authorizations { get; }

    public virtual DbSet<OpenIddictScope> Scopes { get; }

    public virtual DbSet<OpenIddictToken> Tokens { get; }

    public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

    public virtual DbSet<Friendship> Friendships { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

    public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

    public virtual DbSet<SubscriptionPaymentProduct> SubscriptionPaymentProducts { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<UserDelegation> UserDelegations { get; set; }

    public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

	public virtual DbSet<UserAccountLink> UserAccountLinks { get; set; }

    #region Extend Objects
    public virtual DbSet<Control> Control { get; set; }
    public virtual DbSet<Currency> Currency { get; set; }
    public virtual DbSet<IrepairModel.Document> Document { get; set; }
    public virtual DbSet<GeneralProfile> GeneralProfile { get; set; }
    public virtual DbSet<Guest> Guest { get; set; }
    public virtual DbSet<History> History { get; set; }
    public virtual DbSet<IrepairModel.Item> Item { get; set; }
    public virtual DbSet<Maid> Maid { get; set; }
    public virtual DbSet<MaidStatus> MaidStatus { get; set; }
    public virtual DbSet<MArea> MArea { get; set; }
    public virtual DbSet<MPriority> MPriority { get; set; }
    public virtual DbSet<MTechnician> MTechnician { get; set; }
    public virtual DbSet<MWorkNotes> MWorkNotes { get; set; }
    public virtual DbSet<MWorkOrder> MWorkOrder { get; set; }
    public virtual DbSet<MWorkOrderStatus> MWorkOrderStatus { get; set; }
    public virtual DbSet<MWorkTimeSheet> MWorkTimeSheet { get; set; }
    public virtual DbSet<MWorkTimeSheetNoteTemplate> MWorkTimeSheetNoteTemplate { get; set; }
    public virtual DbSet<MWorkType> MWorkType { get; set; }
    public virtual DbSet<Nationality> Nationality { get; set; }
    public virtual DbSet<PurposeStay> PurposeStay { get; set; }
    public virtual DbSet<RateType> RateType { get; set; }
    public virtual DbSet<Report> Report { get; set; }
    public virtual DbSet<ReportBatch> ReportBatch { get; set; }
    public virtual DbSet<Reservation> Reservation { get; set; }
    public virtual DbSet<ReservationGuest> ReservationGuest { get; set; }
    public virtual DbSet<ReservationRate> ReservationRate { get; set; }
    public virtual DbSet<IrepairModel.Room> Room { get; set; }
    public virtual DbSet<RoomBlock> RoomBlock { get; set; }
    public virtual DbSet<RoomStatus> RoomStatus { get; set; }
    public virtual DbSet<RoomType> RoomType { get; set; }
    public virtual DbSet<SqoopeGroup> SqoopeGroup { get; set; }
    public virtual DbSet<SqoopeGroupLink> SqoopeGroupLink { get; set; }
    public virtual DbSet<SqoopeMsgGroupLink> SqoopeMsgGroupLink { get; set; }
    public virtual DbSet<SqoopeMsgLog> SqoopeMsgLog { get; set; }
    public virtual DbSet<SqoopeMsgType> SqoopeMsgType { get; set; }
    public virtual DbSet<Staff> Staff { get; set; }
    public virtual DbSet<Title> Titles { get; set; }
    #region iclean
    public virtual DbSet<AttendantCheckList> AttendantCheckList { get; set; }
    public virtual DbSet<GuestStatus> GuestStatus { get; set; }
    public virtual DbSet<SupervisorCheckList> SupervisorCheckList { get; set; }
    public virtual DbSet<LostFound> LostFound { get; set; }
    public virtual DbSet<LostFoundImage> LostFoundImage { get; set; }
    public virtual DbSet<LostFoundStatus> LostFoundStatus { get; set; }
    public virtual DbSet<ReportSecurity> ReportSecurity { get; set; }
    #endregion

    #region icheckin
    public virtual DbSet<EmailHistory> EmailHistory { get; set; }
    public virtual DbSet<EmailList> EmailList { get; set; }
    public virtual DbSet<City> City { get; set; }
    public virtual DbSet<Company> Company { get; set; }
    public virtual DbSet<ReservationBillingContact> ReservationBillingContact { get; set; }
    public virtual DbSet<Request> Request { get; set; }
    public virtual DbSet<Postcode> Postcode { get; set; }
    public virtual DbSet<PaymentType> PaymentType { get; set; }
    public virtual DbSet<BillingCode> BillingCode { get; set; }
    public virtual DbSet<ReservationAdditional> ReservationAdditional { get; set; }
    public virtual DbSet<GuestAdditional> GuestAdditional { get; set; }
    public virtual DbSet<ReservationRoom> ReservationRoom { get; set; }
    public virtual DbSet<AllotmentLine> AllotmentLine { get; set; }
    public virtual DbSet<AllotmentHdr> AllotmentHdr { get; set; }
    #endregion


    #endregion
    public BEZNgCoreDbContext(DbContextOptions<BEZNgCoreDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BinaryObject>(b => { b.HasIndex(e => new { e.TenantId }); });

        modelBuilder.Entity<SubscribableEdition>(b =>
        {
            b.Property(e => e.MonthlyPrice).HasPrecision(18, 2);
            b.Property(e => e.AnnualPrice).HasPrecision(18, 2);
        });

        modelBuilder.Entity<SubscriptionPayment>(x =>
        {
            x.Property(u => u.ExtraProperties)
                .HasConversion(
                    d => d.ToJsonString(false, false),
                    s => s.FromJsonString<ExtraPropertyDictionary>()
                )
                .Metadata.SetValueComparer(new ValueComparer<ExtraPropertyDictionary>(
                    (c1, c2) => c1.ToJsonString(false, false) == c2.ToJsonString(false, false),
                    c => c.ToJsonString(false, false).GetHashCode(),
                    c => c.ToJsonString(false, false).FromJsonString<ExtraPropertyDictionary>()
                ));
        });

        modelBuilder.Entity<SubscriptionPaymentProduct>(x =>
        {
            x.Property(u => u.ExtraProperties)
                .HasConversion(
                    d => d.ToJsonString(false, false),
                    s => s.FromJsonString<ExtraPropertyDictionary>()
                )
                .Metadata.SetValueComparer(new ValueComparer<ExtraPropertyDictionary>(
                    (c1, c2) => c1.ToJsonString(false, false) == c2.ToJsonString(false, false),
                    c => c.ToJsonString(false, false).GetHashCode(),
                    c => c.ToJsonString(false, false).FromJsonString<ExtraPropertyDictionary>()
                ));

            x.Property(e => e.Amount).HasPrecision(18, 2);
            x.Property(e => e.TotalAmount).HasPrecision(18, 2);
        });

        modelBuilder.Entity<ChatMessage>(b =>
        {
            b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
            b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
            b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
            b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
        });

        modelBuilder.Entity<Friendship>(b =>
        {
            b.HasIndex(e => new { e.TenantId, e.UserId });
            b.HasIndex(e => new { e.TenantId, e.FriendUserId });
            b.HasIndex(e => new { e.FriendTenantId, e.UserId });
            b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
        });

        modelBuilder.Entity<Tenant>(b =>
        {
            b.HasIndex(e => new { e.SubscriptionEndDateUtc });
            b.HasIndex(e => new { e.CreationTime });
        });

        modelBuilder.Entity<SubscriptionPayment>(b =>
        {
            b.HasIndex(e => new { e.Status, e.CreationTime });
            b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
        });

        modelBuilder.Entity<UserDelegation>(b =>
        {
            b.HasIndex(e => new { e.TenantId, e.SourceUserId });
            b.HasIndex(e => new { e.TenantId, e.TargetUserId });
        });

		modelBuilder.Entity<UserAccountLink>(b =>
		{
			b.HasIndex(e => new { e.UserAccountId, e.LinkedUserAccountId }).IsUnique();
		});

        modelBuilder.ConfigureOpenIddict();
    }
}