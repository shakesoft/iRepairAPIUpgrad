using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BEZNgCore.Migrations
{
    /// <inheritdoc />
    public partial class AllTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PIN",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StaffKey",
                table: "AbpUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AllotmentHdr",
                columns: table => new
                {
                    AllotmentHdrKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    Allotment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartyKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDay = table.Column<int>(type: "int", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    GroupAllotmentHdrKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllotmentHdr", x => x.AllotmentHdrKey);
                });

            migrationBuilder.CreateTable(
                name: "AllotmentLine",
                columns: table => new
                {
                    AllotmentLineKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    AllotmentHdrKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoomTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MondayQty = table.Column<int>(type: "int", nullable: true),
                    TuesdayQty = table.Column<int>(type: "int", nullable: true),
                    WednesdayQty = table.Column<int>(type: "int", nullable: true),
                    ThursdayQty = table.Column<int>(type: "int", nullable: true),
                    FridayQty = table.Column<int>(type: "int", nullable: true),
                    SaturdayQty = table.Column<int>(type: "int", nullable: true),
                    SundayQty = table.Column<int>(type: "int", nullable: true),
                    HolidayQty = table.Column<int>(type: "int", nullable: true),
                    HolidayEveQty = table.Column<int>(type: "int", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllotmentLine", x => x.AllotmentLineKey);
                });

            migrationBuilder.CreateTable(
                name: "AttendantCheckList",
                columns: table => new
                {
                    LinenChecklistKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ItemKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RoomKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stop = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendantCheckList", x => x.LinenChecklistKey);
                });

            migrationBuilder.CreateTable(
                name: "BillingCode",
                columns: table => new
                {
                    BillingCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    PostcodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MappedGroup = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingCode", x => x.BillingCodeKey);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    CityKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ShortCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityKey);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    AccNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    IATA = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    RegistrationNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EMail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mailing_Postal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Mailing_CountryKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Billing_Postal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Billing_CountryKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StopCredit = table.Column<int>(type: "int", nullable: true),
                    Terms = table.Column<int>(type: "int", nullable: true),
                    Billing = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Web = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Group1Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group2Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group3Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group4Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SourceKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommissionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayCommission = table.Column<int>(type: "int", nullable: true),
                    ChargeBack = table.Column<int>(type: "int", nullable: true),
                    LastModifiedStaff = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Mailing_RegionKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Billing_RegionKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Contract_Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company_Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GST = table.Column<int>(type: "int", nullable: true),
                    ExtraField1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExtraField2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExtraField3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExtraField4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExtraField5 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExtraField6 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExtraField7 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExtraField8 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExtraField9 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExtraField10 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExtraField11 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExtraField12 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EventTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Shortcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Mailing_Address = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Billing_Address = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(77)", maxLength: 77, nullable: true),
                    Billing_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mailing_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PMTSurchageOptOut = table.Column<int>(type: "int", nullable: true),
                    OnlinePwd = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InterfaceCommission = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Users = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Computer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Access = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommPayable = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyKey);
                });

            migrationBuilder.CreateTable(
                name: "Control",
                columns: table => new
                {
                    ControlKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    EMail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    City = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Postal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CountryKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuestPrefix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    CompanyPrefix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    TravelAgentPrefix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ContactPrefix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    FolioPrefix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    SystemDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PMSStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicenseKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Logo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LogoFileName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OEM = table.Column<int>(type: "int", nullable: true),
                    MaintenanceKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EventManagerKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Control", x => x.ControlKey);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    CurrencyKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BaseCurrency = table.Column<int>(type: "int", nullable: true),
                    SecondaryCurrency = table.Column<int>(type: "int", nullable: true),
                    ExchangeRate = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.CurrencyKey);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    DocumentKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    GuestKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedStaff = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DocumentStore = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Document = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReservationKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Signature = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.DocumentKey);
                });

            migrationBuilder.CreateTable(
                name: "EmailHistory",
                columns: table => new
                {
                    EmailHistoryKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    StaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReservationKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    To = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Sent = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailHistory", x => x.EmailHistoryKey);
                });

            migrationBuilder.CreateTable(
                name: "EmailList",
                columns: table => new
                {
                    mail_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    mac_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    batch_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    mail_from = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    mail_to = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    mail_subject = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    mail_body = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    mail_status = table.Column<int>(type: "int", nullable: false),
                    mail_retry = table.Column<int>(type: "int", nullable: false),
                    sent_count = table.Column<int>(type: "int", nullable: false),
                    sent_repeat = table.Column<int>(type: "int", nullable: false),
                    mail_priority = table.Column<int>(type: "int", nullable: false),
                    date_tosend = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_sent = table.Column<DateTime>(type: "datetime2", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    user_created = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    user_modified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    error_msg = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    doc_no = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    sourcekey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    reportschedulekey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailList", x => x.mail_id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralProfile",
                columns: table => new
                {
                    GeneralProfileKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    SetupKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProfileName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProfileValue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LastModifiedStaff = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DataType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralProfile", x => x.GeneralProfileKey);
                });

            migrationBuilder.CreateTable(
                name: "Guest",
                columns: table => new
                {
                    GuestKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    AccNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    CarNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EMail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Postal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CountryKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NationalityKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Interest = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Terms = table.Column<int>(type: "int", nullable: true),
                    Group1Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group2Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group3Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group4Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SourceKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DefaultCompany = table.Column<int>(type: "int", nullable: true),
                    Company1Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Company1Relation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Company1Department = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Company1Occupation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Company2Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Company2Relation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Company2Department = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Company2Occupation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Company3Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Company3Relation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Company3Department = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Company3Occupation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Company4Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Company4Relation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Company4Department = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Company4Occupation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LastModifiedStaff = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    PassportExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Extra1 = table.Column<int>(type: "int", nullable: true),
                    Extra2 = table.Column<int>(type: "int", nullable: true),
                    Extra3 = table.Column<int>(type: "int", nullable: true),
                    Extra4 = table.Column<int>(type: "int", nullable: true),
                    Extra5 = table.Column<int>(type: "int", nullable: true),
                    Extra6 = table.Column<int>(type: "int", nullable: true),
                    Extra7 = table.Column<int>(type: "int", nullable: true),
                    Extra8 = table.Column<int>(type: "int", nullable: true),
                    Extra9 = table.Column<int>(type: "int", nullable: true),
                    Extra10 = table.Column<int>(type: "int", nullable: true),
                    Extra11 = table.Column<int>(type: "int", nullable: true),
                    Extra12 = table.Column<int>(type: "int", nullable: true),
                    Extra13 = table.Column<int>(type: "int", nullable: true),
                    Extra14 = table.Column<int>(type: "int", nullable: true),
                    Extra15 = table.Column<int>(type: "int", nullable: true),
                    Extra16 = table.Column<int>(type: "int", nullable: true),
                    Extra17 = table.Column<int>(type: "int", nullable: true),
                    Extra18 = table.Column<int>(type: "int", nullable: true),
                    Extra19 = table.Column<int>(type: "int", nullable: true),
                    Extra20 = table.Column<int>(type: "int", nullable: true),
                    Extra21 = table.Column<int>(type: "int", nullable: true),
                    Extra22 = table.Column<int>(type: "int", nullable: true),
                    Extra23 = table.Column<int>(type: "int", nullable: true),
                    Extra24 = table.Column<int>(type: "int", nullable: true),
                    RegionKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GuestStay = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(110)", maxLength: 110, nullable: true),
                    ShortCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Guest = table.Column<string>(type: "nvarchar(123)", maxLength: 123, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GuestIdentityTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Passport = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Subscribe = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PropertyKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrgGuestKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrgAccNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    DoNotContact = table.Column<int>(type: "int", nullable: true),
                    OldGuestStay = table.Column<int>(type: "int", nullable: true),
                    Users = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Computer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Access = table.Column<DateTime>(type: "datetime2", nullable: true),
                    tPassport = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    X_Company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    X_Dorm = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    X_Sector = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guest", x => x.GuestKey);
                });

            migrationBuilder.CreateTable(
                name: "GuestAdditional",
                columns: table => new
                {
                    GuestAdditionalKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    GuestKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    GLKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestAdditional", x => x.GuestAdditionalKey);
                });

            migrationBuilder.CreateTable(
                name: "GuestStatus",
                columns: table => new
                {
                    GuestStatusKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ts = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestStatus", x => x.GuestStatusKey);
                });

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    HistoryKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    LinkKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModuleName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Operation = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SourceKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Detail = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    OldValue = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SourceLink = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.HistoryKey);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ItemKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    SalesPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LastModifiedStaff = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    GLKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HKg = table.Column<int>(type: "int", nullable: true),
                    Rgn = table.Column<int>(type: "int", nullable: true),
                    Cfm = table.Column<int>(type: "int", nullable: true),
                    Calculator = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    To_GH = table.Column<int>(type: "int", nullable: true),
                    Fm_GH = table.Column<int>(type: "int", nullable: true),
                    Flag1 = table.Column<int>(type: "int", nullable: true),
                    Flag2 = table.Column<int>(type: "int", nullable: true),
                    Flag3 = table.Column<int>(type: "int", nullable: true),
                    Flag4 = table.Column<int>(type: "int", nullable: true),
                    Flag5 = table.Column<int>(type: "int", nullable: true),
                    ChargeDateOffSet = table.Column<int>(type: "int", nullable: true),
                    News = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SalesPrice01 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalesPrice02 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalesPrice03 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalesPrice04 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalesPrice05 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalesPrice06 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalesPrice07 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalesPrice08 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalesPrice09 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalesPrice10 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Online = table.Column<int>(type: "int", nullable: true),
                    Minibar = table.Column<int>(type: "int", nullable: true),
                    Laundry = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Lookup = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ItemKey);
                });

            migrationBuilder.CreateTable(
                name: "LostFound",
                columns: table => new
                {
                    LostFoundKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    LostFoundStatusKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Area = table.Column<int>(type: "int", nullable: true),
                    Owner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OwnerFolio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OwnerRoomKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OwnerContactNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Founder = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FounderFolio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FounderRoomKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FounderContactNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Instruction = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    AutoReference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LostFound", x => x.LostFoundKey);
                });

            migrationBuilder.CreateTable(
                name: "LostFoundImage",
                columns: table => new
                {
                    LostFoundImageKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    LostFoundKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LostFoundImage = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LostFoundImages = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LostFoundImages2 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LostFoundImages3 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LostFoundImages4 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LostFoundImages5 = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LostFoundImage", x => x.LostFoundImageKey);
                });

            migrationBuilder.CreateTable(
                name: "LostFoundStatus",
                columns: table => new
                {
                    LostFoundStatusKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    LostFoundStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LostFoundStatus", x => x.LostFoundStatusKey);
                });

            migrationBuilder.CreateTable(
                name: "Maid",
                columns: table => new
                {
                    MaidKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ts = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(101)", maxLength: 101, nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Floor = table.Column<int>(type: "int", nullable: true),
                    Section = table.Column<int>(type: "int", nullable: true),
                    Mon = table.Column<int>(type: "int", nullable: true),
                    Tue = table.Column<int>(type: "int", nullable: true),
                    Wed = table.Column<int>(type: "int", nullable: true),
                    Thur = table.Column<int>(type: "int", nullable: true),
                    Fri = table.Column<int>(type: "int", nullable: true),
                    Sat = table.Column<int>(type: "int", nullable: true),
                    Sun = table.Column<int>(type: "int", nullable: true),
                    Min = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Max = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoomTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maid", x => x.MaidKey);
                });

            migrationBuilder.CreateTable(
                name: "MaidStatus",
                columns: table => new
                {
                    MaidStatusKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    MaidStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaidStatus", x => x.MaidStatusKey);
                });

            migrationBuilder.CreateTable(
                name: "MArea",
                columns: table => new
                {
                    Seqno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Floor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Block = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Active = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MArea", x => x.Seqno);
                });

            migrationBuilder.CreateTable(
                name: "MPriority",
                columns: table => new
                {
                    PriorityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MPriority", x => x.PriorityID);
                });

            migrationBuilder.CreateTable(
                name: "MTechnician",
                columns: table => new
                {
                    Seqno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Contractor = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    OPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Pager = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TechnicianKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MTechnician", x => x.Seqno);
                });

            migrationBuilder.CreateTable(
                name: "MWorkNotes",
                columns: table => new
                {
                    MWorkNotesKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MWorkOrderKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MWorkNotes", x => x.MWorkNotesKey);
                });

            migrationBuilder.CreateTable(
                name: "MWorkOrder",
                columns: table => new
                {
                    Seqno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    MWorkOrderKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SqoopeWorkOrderID = table.Column<int>(type: "int", nullable: true),
                    Room = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RoomKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MArea = table.Column<int>(type: "int", nullable: true),
                    MWorkType = table.Column<int>(type: "int", nullable: true),
                    MWorkOrderStatus = table.Column<int>(type: "int", nullable: true),
                    MTechnician = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ScheduledFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduledTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignedOff = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Cancelled = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EnteredBy = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    EnteredStaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EnteredDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedBy = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CompletedStaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignedOffBy = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    SignedOffStaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SignedOffDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledBy = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CancelledStaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CancelledDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdateBy = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    LastUpdateStaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StaffName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReportedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MWorkOrder", x => x.Seqno);
                });

            migrationBuilder.CreateTable(
                name: "MWorkOrderStatus",
                columns: table => new
                {
                    Seqno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Active = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MWorkOrderStatus", x => x.Seqno);
                });

            migrationBuilder.CreateTable(
                name: "MWorkTimeSheet",
                columns: table => new
                {
                    Seqno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Hdr_Seqno = table.Column<int>(type: "int", nullable: true),
                    MTechnician = table.Column<int>(type: "int", nullable: true),
                    WDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MWorkTimeSheet", x => x.Seqno);
                });

            migrationBuilder.CreateTable(
                name: "MWorkTimeSheetNoteTemplate",
                columns: table => new
                {
                    Seqno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Active = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MWorkTimeSheetNoteTemplate", x => x.Seqno);
                });

            migrationBuilder.CreateTable(
                name: "MWorkType",
                columns: table => new
                {
                    Seqno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Active = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MWorkType", x => x.Seqno);
                });

            migrationBuilder.CreateTable(
                name: "Nationality",
                columns: table => new
                {
                    NationalityKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PassportCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    FlashCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IDCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationality", x => x.NationalityKey);
                });

            migrationBuilder.CreateTable(
                name: "PaymentType",
                columns: table => new
                {
                    PaymentTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    PostCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyAR = table.Column<int>(type: "int", nullable: true),
                    CompanyKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Adjustment = table.Column<int>(type: "int", nullable: true),
                    CompulsoryCCNo = table.Column<int>(type: "int", nullable: true),
                    CompulsoryRef = table.Column<int>(type: "int", nullable: true),
                    CompulsoryName = table.Column<int>(type: "int", nullable: true),
                    CompulsoryVoucherNo = table.Column<int>(type: "int", nullable: true),
                    PaymentGroupKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ARPayment = table.Column<int>(type: "int", nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ExternalAR = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ExternalARAccNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SurchargePostCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SurchargePercent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SurchargeThreshold = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Payment = table.Column<int>(type: "int", nullable: true),
                    Payout = table.Column<int>(type: "int", nullable: true),
                    ExtPaymentTravelClick = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ExtPaymentSiteminder = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ARC_Sent = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    CreditCardFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Offline = table.Column<int>(type: "int", nullable: true),
                    EftposCardType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ARSurcharge = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentType", x => x.PaymentTypeKey);
                });

            migrationBuilder.CreateTable(
                name: "Postcode",
                columns: table => new
                {
                    PostcodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    BillingCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    GLCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Tax1 = table.Column<int>(type: "int", nullable: true),
                    Tax2 = table.Column<int>(type: "int", nullable: true),
                    Tax3 = table.Column<int>(type: "int", nullable: true),
                    Tax1Min = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax2Min = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax3Min = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax1Max = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax2Max = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax3Max = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax1Zero = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax2Zero = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax3Zero = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayCommission = table.Column<int>(type: "int", nullable: true),
                    GL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CurrencyKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Tax1Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Tax2Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Tax3Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ARC_Sent = table.Column<int>(type: "int", nullable: true),
                    FlashCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postcode", x => x.PostcodeKey);
                });

            migrationBuilder.CreateTable(
                name: "PurposeStay",
                columns: table => new
                {
                    PurposeStayKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    PurposeStay = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurposeStay", x => x.PurposeStayKey);
                });

            migrationBuilder.CreateTable(
                name: "RateType",
                columns: table => new
                {
                    RateTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    NoOfNight = table.Column<int>(type: "int", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    Override = table.Column<int>(type: "int", nullable: true),
                    LastModifiedStaff = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    ApplicableForAll = table.Column<int>(type: "int", nullable: true),
                    PostCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RateTypeGroupingKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PrintRate = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    RateCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    RateTypeKeyBase = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RateLinkAmount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RateLinkType = table.Column<int>(type: "int", nullable: true),
                    RateLinkRound = table.Column<int>(type: "int", nullable: true),
                    BAR = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ChannelSet = table.Column<int>(type: "int", nullable: true),
                    AdvanceDay = table.Column<int>(type: "int", nullable: false),
                    Online = table.Column<int>(type: "int", nullable: false),
                    MaxAdvanceDay = table.Column<int>(type: "int", nullable: false),
                    ChannelSet01 = table.Column<int>(type: "int", nullable: false),
                    ChannelSet02 = table.Column<int>(type: "int", nullable: false),
                    ChannelSet03 = table.Column<int>(type: "int", nullable: false),
                    ChannelSet04 = table.Column<int>(type: "int", nullable: false),
                    ChannelSet05 = table.Column<int>(type: "int", nullable: false),
                    ChannelSet06 = table.Column<int>(type: "int", nullable: false),
                    ChannelSet07 = table.Column<int>(type: "int", nullable: false),
                    ChannelSet08 = table.Column<int>(type: "int", nullable: false),
                    ChannelSet09 = table.Column<int>(type: "int", nullable: false),
                    ChannelSet10 = table.Column<int>(type: "int", nullable: false),
                    ChannelSet11 = table.Column<int>(type: "int", nullable: false),
                    ChannelSet12 = table.Column<int>(type: "int", nullable: false),
                    Promo = table.Column<int>(type: "int", nullable: false),
                    Group1Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group2Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group3Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group4Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TermsConditions = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    CancellationPolicy = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    ExtraAdultRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FirstNight = table.Column<int>(type: "int", nullable: false),
                    CommCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommPayable = table.Column<int>(type: "int", nullable: true),
                    CancellationRuleKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateType", x => x.RateTypeKey);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    ReportKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ParentReportKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RptName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ReportKey);
                });

            migrationBuilder.CreateTable(
                name: "ReportBatch",
                columns: table => new
                {
                    ReportBatchKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Report1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Report2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Report3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Report4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Report5 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Report6 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Report7 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Report8 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Report9 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Report10 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    BatchName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportBatch", x => x.ReportBatchKey);
                });

            migrationBuilder.CreateTable(
                name: "ReportSecurity",
                columns: table => new
                {
                    ReportSecurityKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ReportKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sec_Report = table.Column<int>(type: "int", nullable: true),
                    StaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportSecurity", x => x.ReportSecurityKey);
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    RequestKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    GuestKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReservationKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedStaff = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    PrintToGuestArrival = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CompanyKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.RequestKey);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    ReservationKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RateTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReservationType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    TravelAgentKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DebtorKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GuestKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    RoomTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoomKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdditionalBed = table.Column<int>(type: "int", nullable: true),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckInTime = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutTime = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Flight = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PromotionKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RateKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SourceKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MarketKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingChannelKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GeoSourceKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SecurityKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CardNo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Adult = table.Column<int>(type: "int", nullable: true),
                    Child = table.Column<int>(type: "int", nullable: true),
                    LastModifiedStaff = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    ETA = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ETD = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BookingKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CampaignKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TravelReasonKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TravelTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentStatus = table.Column<int>(type: "int", nullable: true),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Ref1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Ref2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CancellationKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GroupReservationKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VoucherNo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    CCExpiry = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    COA = table.Column<int>(type: "int", nullable: true),
                    DepartmentKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingAgentKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Guaranteed = table.Column<int>(type: "int", nullable: true),
                    NightAuditRoomChargeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NightAuditAddItemDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DepartmentAddress = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    DepartmentPostal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DepartmentCountryKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReservationStaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CheckInStaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CheckOutStaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CancellationStaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group1Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group2Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group3Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group4Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReCheckInReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReCheckInStaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyContact = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrgCheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReportingRateTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecheckIn = table.Column<int>(type: "int", nullable: true),
                    Folio_Limit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Extra1 = table.Column<int>(type: "int", nullable: true),
                    Extra2 = table.Column<int>(type: "int", nullable: true),
                    Extra3 = table.Column<int>(type: "int", nullable: true),
                    Extra4 = table.Column<int>(type: "int", nullable: true),
                    Extra5 = table.Column<int>(type: "int", nullable: true),
                    Extra6 = table.Column<int>(type: "int", nullable: true),
                    Extra7 = table.Column<int>(type: "int", nullable: true),
                    Extra8 = table.Column<int>(type: "int", nullable: true),
                    Extra9 = table.Column<int>(type: "int", nullable: true),
                    Extra10 = table.Column<int>(type: "int", nullable: true),
                    Extra11 = table.Column<int>(type: "int", nullable: true),
                    Extra12 = table.Column<int>(type: "int", nullable: true),
                    Extra13 = table.Column<int>(type: "int", nullable: true),
                    Extra14 = table.Column<int>(type: "int", nullable: true),
                    Extra15 = table.Column<int>(type: "int", nullable: true),
                    Extra16 = table.Column<int>(type: "int", nullable: true),
                    Extra17 = table.Column<int>(type: "int", nullable: true),
                    Extra18 = table.Column<int>(type: "int", nullable: true),
                    Extra19 = table.Column<int>(type: "int", nullable: true),
                    Extra20 = table.Column<int>(type: "int", nullable: true),
                    Extra21 = table.Column<int>(type: "int", nullable: true),
                    Extra22 = table.Column<int>(type: "int", nullable: true),
                    Extra23 = table.Column<int>(type: "int", nullable: true),
                    Extra24 = table.Column<int>(type: "int", nullable: true),
                    Definite = table.Column<int>(type: "int", nullable: true),
                    PrintRate = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    GroupNationalityKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GroupRegionKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GroupCountryKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyPostal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CompanyCountryKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MainFolioBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherFolioBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayCommission = table.Column<int>(type: "int", nullable: true),
                    ChargeBack = table.Column<int>(type: "int", nullable: true),
                    RemarkToHistory = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    GuestStay = table.Column<int>(type: "int", nullable: true),
                    LoyaltyCardNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LoyaltyCard = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    UseAllotment = table.Column<int>(type: "int", nullable: true),
                    CompanyTel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GuestPassportExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GuestGender = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    GuestDob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GuestTel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GuestMobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GuestNationalityKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GuestRegionKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GuestPostal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GuestCountryKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Folio_Limit2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GuestStatus = table.Column<int>(type: "int", nullable: true),
                    RecheckInVirtualRoom = table.Column<int>(type: "int", nullable: true),
                    RecheckInDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GuestEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DoNotMoveRoom = table.Column<int>(type: "int", nullable: true),
                    GroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CheckIn_Instruction = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CheckOut_Instruction = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Posting_Instruction = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CompanyAddress = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    GuestAddress = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DepartmentCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GroupCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompanyCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GuestCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CardName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GuestIdentityTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GuestPassport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferChargesToKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    LongStay = table.Column<int>(type: "int", nullable: true),
                    CheckInDateOriginal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutDateOriginal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cutoff_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cutoff_Days = table.Column<int>(type: "int", nullable: true),
                    Follow_Up_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Decision_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BBQPitKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CheckOutDocNo = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    PurposeStayKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PreCheckInCount = table.Column<int>(type: "int", nullable: true),
                    GuestDND = table.Column<int>(type: "int", nullable: true),
                    GuestPhoneCOS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GuestLanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CVV = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    OrgRatePromotionTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GuestArrived = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ExpressCheckOut = table.Column<int>(type: "int", nullable: true),
                    GDS = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    GroupBlockingKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GroupAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    GroupPostalCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    GroupPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CommCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommPayable = table.Column<int>(type: "int", nullable: true),
                    RoomListDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationRuleKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Fixed = table.Column<int>(type: "int", nullable: true),
                    Elastic = table.Column<int>(type: "int", nullable: true),
                    GroupStatus = table.Column<int>(type: "int", nullable: true),
                    RoomToCharge = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DoNotContact = table.Column<int>(type: "int", nullable: true),
                    Inventory = table.Column<int>(type: "int", nullable: true),
                    BillTo2Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BillTo3Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BillTo4Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GuestDND_old = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Users = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Computer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Access = table.Column<DateTime>(type: "datetime2", nullable: true),
                    tGuestPassport = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    CompanyDepartmentKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.ReservationKey);
                });

            migrationBuilder.CreateTable(
                name: "ReservationAdditional",
                columns: table => new
                {
                    ReservationAdditionalKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ReservationKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    GLKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NoOfDay = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationAdditional", x => x.ReservationAdditionalKey);
                });

            migrationBuilder.CreateTable(
                name: "ReservationBillingContact",
                columns: table => new
                {
                    ReservationBillingContactKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    ReservationKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Billing = table.Column<int>(type: "int", nullable: true),
                    AccountType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    AccountKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvoiceNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Invoice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationBillingContact", x => x.ReservationBillingContactKey);
                });

            migrationBuilder.CreateTable(
                name: "ReservationGuest",
                columns: table => new
                {
                    ReservationGuestKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ReservationKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReservationRoomKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GuestKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    GuestStay = table.Column<int>(type: "int", nullable: true),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShareGuestKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    X_CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReservationStatus = table.Column<int>(type: "int", nullable: true),
                    X_BillCheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_BillCheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_Bill2CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_Bill2CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_Bill3CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_Bill3CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_Bill4CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_Bill4CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_Bill5CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_Bill5CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_Bill6aCheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_Bill6aCheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_Bill6bCheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_Bill6bCheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_Bill7CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_Bill7CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_SHNBill1CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_SHNBill1CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_SHNBill2CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_SHNBill2CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_SHNBill3CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_SHNBill3CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_SHNBill4CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_SHNBill4CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_SHNBill5CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_SHNBill5CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_SHNBill6CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_SHNBill6CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_SHNBill7CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X_SHNBill7CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationGuest", x => x.ReservationGuestKey);
                });

            migrationBuilder.CreateTable(
                name: "ReservationRate",
                columns: table => new
                {
                    ReservationRateKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ReservationKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChargeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax1 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax3 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Overwrite = table.Column<int>(type: "int", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    OverwriteReason = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    OverwriteStaff = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OverwriteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BillTo = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Ref1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ref2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Covers = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoomKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Void = table.Column<int>(type: "int", nullable: true),
                    ARTransKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BillToName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InvoiceNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VoucherNo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    ShiftKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShiftNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    VoidSourceKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Consolidated = table.Column<int>(type: "int", nullable: true),
                    ForeignCurrencyKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ForeignAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SecondaryAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CardName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ForeignExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SecondaryExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdditionalBed = table.Column<byte>(type: "tinyint", nullable: true),
                    RedemptPoint = table.Column<int>(type: "int", nullable: true),
                    AwardedPoint = table.Column<int>(type: "int", nullable: true),
                    RateTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CardCVV = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    BatchInvoicePrint = table.Column<int>(type: "int", nullable: true),
                    OrgSourcePostCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrgSourceDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OrgSourcePostDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationRate", x => x.ReservationRateKey);
                });

            migrationBuilder.CreateTable(
                name: "ReservationRoom",
                columns: table => new
                {
                    ReservationRoomKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ReservationKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoomKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoomTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    AdditionalBed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Alloted = table.Column<int>(type: "int", nullable: true),
                    AllotmentLineKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Override = table.Column<int>(type: "int", nullable: true),
                    OldRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OldAdditional = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OverrideStaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OverrideStaffName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    OverrideTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OldRoomKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OverrideReason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PointsValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SourceKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TravelAgentKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RateCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RateTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldRateTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group1Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group2Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group3Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Group4Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrgRoomTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldRoomTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrgRatePromotionTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RatePromotionTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Adult = table.Column<int>(type: "int", nullable: false),
                    Child = table.Column<int>(type: "int", nullable: true),
                    GroupBlockReservationKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommCodeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommPayable = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationRoom", x => x.ReservationRoomKey);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    RoomKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    RoomTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MaxPax = table.Column<int>(type: "int", nullable: true),
                    InterconnectRoomKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Tv = table.Column<int>(type: "int", nullable: true),
                    Shower = table.Column<int>(type: "int", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ts = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Floor = table.Column<int>(type: "int", nullable: true),
                    CleaningTime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LinenDays = table.Column<int>(type: "int", nullable: true),
                    Bed = table.Column<int>(type: "int", nullable: true),
                    PhoneExt = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RoomStatusKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaidStatusKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaidKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LinenChange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Extra1 = table.Column<int>(type: "int", nullable: true),
                    Extra2 = table.Column<int>(type: "int", nullable: true),
                    Extra3 = table.Column<int>(type: "int", nullable: true),
                    Extra4 = table.Column<int>(type: "int", nullable: true),
                    Extra5 = table.Column<int>(type: "int", nullable: true),
                    Extra6 = table.Column<int>(type: "int", nullable: true),
                    Extra7 = table.Column<int>(type: "int", nullable: true),
                    Extra8 = table.Column<int>(type: "int", nullable: true),
                    Extra9 = table.Column<int>(type: "int", nullable: true),
                    Extra10 = table.Column<int>(type: "int", nullable: true),
                    Extra11 = table.Column<int>(type: "int", nullable: true),
                    Extra12 = table.Column<int>(type: "int", nullable: true),
                    Extra13 = table.Column<int>(type: "int", nullable: true),
                    Extra14 = table.Column<int>(type: "int", nullable: true),
                    Extra15 = table.Column<int>(type: "int", nullable: true),
                    Extra16 = table.Column<int>(type: "int", nullable: true),
                    Extra17 = table.Column<int>(type: "int", nullable: true),
                    Extra18 = table.Column<int>(type: "int", nullable: true),
                    Extra19 = table.Column<int>(type: "int", nullable: true),
                    Extra20 = table.Column<int>(type: "int", nullable: true),
                    Extra21 = table.Column<int>(type: "int", nullable: true),
                    Extra22 = table.Column<int>(type: "int", nullable: true),
                    Extra23 = table.Column<int>(type: "int", nullable: true),
                    Extra24 = table.Column<int>(type: "int", nullable: true),
                    HMMNotes = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Tower = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneExt2 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PhoneExt3 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PhoneExt4 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DND = table.Column<int>(type: "int", nullable: true),
                    ARC_Sent = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.RoomKey);
                });

            migrationBuilder.CreateTable(
                name: "RoomBlock",
                columns: table => new
                {
                    RoomBlockKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    RoomKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BlockDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    BlockStaff = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    UnblockStaff = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    BlockTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UnblockTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    MWorkOrderNo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomBlock", x => x.RoomBlockKey);
                });

            migrationBuilder.CreateTable(
                name: "RoomStatus",
                columns: table => new
                {
                    RoomStatusKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    RoomStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ts = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomStatus", x => x.RoomStatusKey);
                });

            migrationBuilder.CreateTable(
                name: "RoomType",
                columns: table => new
                {
                    RoomTypeKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    RoomType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ts = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DefaultRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhysicalRoomType = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    RoomTypeGroupingKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DefaultPoint = table.Column<short>(type: "smallint", nullable: true),
                    IsMember = table.Column<int>(type: "int", nullable: false),
                    LastRoom = table.Column<int>(type: "int", nullable: false),
                    LeftRoom = table.Column<int>(type: "int", nullable: false),
                    ArcSent = table.Column<int>(type: "int", nullable: true),
                    Online = table.Column<int>(type: "int", nullable: true),
                    MinRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Fs = table.Column<int>(type: "int", nullable: true),
                    Lt = table.Column<int>(type: "int", nullable: true),
                    Ds = table.Column<int>(type: "int", nullable: true),
                    LiveImgUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomType", x => x.RoomTypeKey);
                });

            migrationBuilder.CreateTable(
                name: "SqoopeGroup",
                columns: table => new
                {
                    SqoopeGroupKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MessageKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqoopeGroup", x => x.SqoopeGroupKey);
                });

            migrationBuilder.CreateTable(
                name: "SqoopeGroupLink",
                columns: table => new
                {
                    SqoopeLinkStaffkey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    SqoopeGroupKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqoopeGroupLink", x => x.SqoopeLinkStaffkey);
                });

            migrationBuilder.CreateTable(
                name: "SqoopeMsgGroupLink",
                columns: table => new
                {
                    SqoopeMsgGroupLinkKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    SqoopeMessageKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SqoopeGroupKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sort = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqoopeMsgGroupLink", x => x.SqoopeMsgGroupLinkKey);
                });

            migrationBuilder.CreateTable(
                name: "SqoopeMsgLog",
                columns: table => new
                {
                    SqoopeMsgLogKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    SqoopeMessageKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FromContactId = table.Column<int>(type: "int", nullable: true),
                    ToContactId = table.Column<int>(type: "int", nullable: true),
                    Msg = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SqoopeMsgId = table.Column<int>(type: "int", nullable: true),
                    SqoopeMsgCreatedTS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SqoopeMsgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SqoopeMsgResCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Read = table.Column<bool>(type: "bit", nullable: false),
                    Send = table.Column<bool>(type: "bit", nullable: false),
                    ToStaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirebaseToken_Id = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqoopeMsgLog", x => x.SqoopeMsgLogKey);
                });

            migrationBuilder.CreateTable(
                name: "SqoopeMsgType",
                columns: table => new
                {
                    MessageKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageTemplate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqoopeMsgType", x => x.MessageKey);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StaffKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JobPosition = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Administrator = table.Column<int>(type: "int", nullable: true),
                    SecurityProfileKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Sec_Guest = table.Column<int>(type: "int", nullable: true),
                    Sec_Company = table.Column<int>(type: "int", nullable: true),
                    Sec_BillingCodeSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_PostCodeSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_PaymentTypeSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_RoomSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_PriceSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_RoomAllotmentSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_ItemSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_GroupingSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_CancellationSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_AnnouncementSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_ARTransactions = table.Column<int>(type: "int", nullable: true),
                    Sec_ARPayment = table.Column<int>(type: "int", nullable: true),
                    Sec_ARAdjustment = table.Column<int>(type: "int", nullable: true),
                    Sec_RoomAvailability = table.Column<int>(type: "int", nullable: true),
                    Sec_SearchRate = table.Column<int>(type: "int", nullable: true),
                    Sec_Reservation = table.Column<int>(type: "int", nullable: true),
                    Sec_Folio = table.Column<int>(type: "int", nullable: true),
                    Sec_FrontDeskCheckIn = table.Column<int>(type: "int", nullable: true),
                    Sec_FrontDeskCheckOut = table.Column<int>(type: "int", nullable: true),
                    Sec_NewReservation = table.Column<int>(type: "int", nullable: true),
                    Sec_AssignRoom = table.Column<int>(type: "int", nullable: true),
                    Sec_CheckIn = table.Column<int>(type: "int", nullable: true),
                    Sec_CancelReservation = table.Column<int>(type: "int", nullable: true),
                    Sec_MoveReservation = table.Column<int>(type: "int", nullable: true),
                    Sec_CopyReservation = table.Column<int>(type: "int", nullable: true),
                    Sec_ConfirmReservation = table.Column<int>(type: "int", nullable: true),
                    Sec_ARAllocation = table.Column<int>(type: "int", nullable: true),
                    Sec_CheckOut = table.Column<int>(type: "int", nullable: true),
                    Sec_CitySetup = table.Column<int>(type: "int", nullable: true),
                    Sec_BlockRoom = table.Column<int>(type: "int", nullable: true),
                    Sec_ReCheckIn = table.Column<int>(type: "int", nullable: true),
                    Sec_ConfirmRoomChange = table.Column<int>(type: "int", nullable: true),
                    Sec_CompanyFinance = table.Column<int>(type: "int", nullable: true),
                    Sec_Holiday_Setup = table.Column<int>(type: "int", nullable: true),
                    Sec_ActivatePABX = table.Column<int>(type: "int", nullable: true),
                    Sec_ChangeBusinessDate = table.Column<int>(type: "int", nullable: true),
                    Sec_Cashier = table.Column<int>(type: "int", nullable: true),
                    SEC_Override_EndShift = table.Column<int>(type: "int", nullable: true),
                    Sec_ARCN = table.Column<int>(type: "int", nullable: true),
                    Sec_ARDN = table.Column<int>(type: "int", nullable: true),
                    Sec_PaymentGroupSetup = table.Column<int>(type: "int", nullable: true),
                    StaffDepartmentKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sec_ShowShiftBalance = table.Column<int>(type: "int", nullable: true),
                    Sec_ChangeARCompany = table.Column<int>(type: "int", nullable: true),
                    Sec_ShowDayEndBalance = table.Column<int>(type: "int", nullable: true),
                    Sec_CloseOthersShift = table.Column<int>(type: "int", nullable: true),
                    Sec_BatchPosting = table.Column<int>(type: "int", nullable: true),
                    Sec_SourceSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_VoidPosting = table.Column<int>(type: "int", nullable: true),
                    Sec_TransferPosting = table.Column<int>(type: "int", nullable: true),
                    Sec_ManualPosting = table.Column<int>(type: "int", nullable: true),
                    Sec_ConsolidatePosting = table.Column<int>(type: "int", nullable: true),
                    Sec_TitleSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_InactivateGuest = table.Column<int>(type: "int", nullable: true),
                    Sec_NightAudit = table.Column<int>(type: "int", nullable: true),
                    MPassword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sec_OverBookingSetup = table.Column<int>(type: "int", nullable: true),
                    MAdministrator = table.Column<int>(type: "int", nullable: true),
                    Sec_ChangeLedger = table.Column<int>(type: "int", nullable: true),
                    Sec_ForeignCurrencySetup = table.Column<int>(type: "int", nullable: true),
                    Sec_VirtualFolioPosting = table.Column<int>(type: "int", nullable: true),
                    Sec_SystemBalancing = table.Column<int>(type: "int", nullable: true),
                    Sec_EventBanquet = table.Column<int>(type: "int", nullable: true),
                    Sec_FacilitySetup = table.Column<int>(type: "int", nullable: true),
                    Sec_ChangeRate = table.Column<int>(type: "int", nullable: true),
                    Sec_FNBSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_EventBookingTypeSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_EventInventorySetup = table.Column<int>(type: "int", nullable: true),
                    Sec_Concierges = table.Column<int>(type: "int", nullable: true),
                    Sec_CompanyCRM = table.Column<int>(type: "int", nullable: true),
                    Sec_LockupSetup = table.Column<int>(type: "int", nullable: true),
                    Sec_CompanyType_Setup = table.Column<int>(type: "int", nullable: true),
                    Sec_Payout = table.Column<int>(type: "int", nullable: true),
                    Sec_ARTransfer = table.Column<int>(type: "int", nullable: true),
                    Sec_Split_Bill = table.Column<int>(type: "int", nullable: true),
                    Sec_NewPendingReservation = table.Column<int>(type: "int", nullable: true),
                    Sec_HMM = table.Column<int>(type: "int", nullable: true),
                    Sec_CanUncheckIn = table.Column<int>(type: "int", nullable: true),
                    UserTemplateKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PIN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    MaidKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TechnicianKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sec_Supervisor = table.Column<int>(type: "int", nullable: true),
                    Contact_Id = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Sec_TechSupervisor = table.Column<int>(type: "int", nullable: true),
                    OverwriteRate = table.Column<int>(type: "int", nullable: true),
                    Donotmove = table.Column<int>(type: "int", nullable: true),
                    Sec_UnCancelReservation = table.Column<int>(type: "int", nullable: true),
                    Sec_UserSecurity = table.Column<int>(type: "int", nullable: true),
                    Sec_UserTemplate = table.Column<int>(type: "int", nullable: true),
                    Sec_PMTDecrypt = table.Column<int>(type: "int", nullable: true),
                    Sec_Maid = table.Column<int>(type: "int", nullable: true),
                    Sec_CAttendant = table.Column<int>(type: "int", nullable: true),
                    Sec_AAttendant = table.Column<int>(type: "int", nullable: true),
                    Sec_PromoModule = table.Column<int>(type: "int", nullable: true),
                    Sec_BatchCheckOut = table.Column<int>(type: "int", nullable: true),
                    BreakfastStaff = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnywhereAccess = table.Column<int>(type: "int", nullable: true),
                    Sec_Overbooking = table.Column<int>(type: "int", nullable: true),
                    Sec_ReservationPaymentAdd = table.Column<int>(type: "int", nullable: true),
                    Sec_ReservationPaymentShow = table.Column<int>(type: "int", nullable: true),
                    Sec_ReservationPaymentDelete = table.Column<int>(type: "int", nullable: true),
                    Sec_NegativePosting = table.Column<int>(type: "int", nullable: true),
                    Sec_MoveFitToGroup = table.Column<int>(type: "int", nullable: true),
                    ComputerName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    EftposIP = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    EftposID = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    EftposSN = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    Sec_Stats = table.Column<int>(type: "int", nullable: true),
                    Sec_VirtualCheckIn = table.Column<int>(type: "int", nullable: true),
                    Sec_GroupOverbook = table.Column<int>(type: "int", nullable: true),
                    Sec_OverrideCredit = table.Column<int>(type: "int", nullable: true),
                    Sec_PassportDecrypt = table.Column<int>(type: "int", nullable: true),
                    FirebaseToken_Id = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Sec_JobPosition = table.Column<int>(type: "int", nullable: true),
                    Sec_IPSetUp = table.Column<int>(type: "int", nullable: true),
                    Sec_IPViewLog = table.Column<int>(type: "int", nullable: true),
                    Sec_IPAssignTasks = table.Column<int>(type: "int", nullable: true),
                    Sec_IPBlockRoom = table.Column<int>(type: "int", nullable: true),
                    Sec_SupervisorB = table.Column<int>(type: "int", nullable: true),
                    Sec_SupervisorMode = table.Column<int>(type: "int", nullable: true),
                    Sec_Rooms = table.Column<int>(type: "int", nullable: true),
                    Sec_MiniBar = table.Column<int>(type: "int", nullable: true),
                    Sec_MiniBarCo = table.Column<int>(type: "int", nullable: true),
                    Sec_Laundry = table.Column<int>(type: "int", nullable: true),
                    Sec_LostFound = table.Column<int>(type: "int", nullable: true),
                    Sec_WOEntry = table.Column<int>(type: "int", nullable: true),
                    Sec_ViewLogs = table.Column<int>(type: "int", nullable: true),
                    Sec_RoomstoInspect = table.Column<int>(type: "int", nullable: true),
                    Sec_GuestRequest = table.Column<int>(type: "int", nullable: true),
                    Sec_AllowCleanDirectly = table.Column<int>(type: "int", nullable: true),
                    FirebaseToken_IdiRepair = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Sec_OverrideRestriction = table.Column<int>(type: "int", nullable: true),
                    ChangeCleanStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.StaffKey);
                });

            migrationBuilder.CreateTable(
                name: "SupervisorCheckList",
                columns: table => new
                {
                    InspectionChecklistKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Group4 = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Checked = table.Column<int>(type: "int", nullable: false),
                    RoomKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupervisorCheckList", x => x.InspectionChecklistKey);
                });

            migrationBuilder.CreateTable(
                name: "Title",
                columns: table => new
                {
                    TitleKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    Sync = table.Column<int>(type: "int", nullable: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Title", x => x.TitleKey);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllotmentHdr");

            migrationBuilder.DropTable(
                name: "AllotmentLine");

            migrationBuilder.DropTable(
                name: "AttendantCheckList");

            migrationBuilder.DropTable(
                name: "BillingCode");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Control");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "EmailHistory");

            migrationBuilder.DropTable(
                name: "EmailList");

            migrationBuilder.DropTable(
                name: "GeneralProfile");

            migrationBuilder.DropTable(
                name: "Guest");

            migrationBuilder.DropTable(
                name: "GuestAdditional");

            migrationBuilder.DropTable(
                name: "GuestStatus");

            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "LostFound");

            migrationBuilder.DropTable(
                name: "LostFoundImage");

            migrationBuilder.DropTable(
                name: "LostFoundStatus");

            migrationBuilder.DropTable(
                name: "Maid");

            migrationBuilder.DropTable(
                name: "MaidStatus");

            migrationBuilder.DropTable(
                name: "MArea");

            migrationBuilder.DropTable(
                name: "MPriority");

            migrationBuilder.DropTable(
                name: "MTechnician");

            migrationBuilder.DropTable(
                name: "MWorkNotes");

            migrationBuilder.DropTable(
                name: "MWorkOrder");

            migrationBuilder.DropTable(
                name: "MWorkOrderStatus");

            migrationBuilder.DropTable(
                name: "MWorkTimeSheet");

            migrationBuilder.DropTable(
                name: "MWorkTimeSheetNoteTemplate");

            migrationBuilder.DropTable(
                name: "MWorkType");

            migrationBuilder.DropTable(
                name: "Nationality");

            migrationBuilder.DropTable(
                name: "PaymentType");

            migrationBuilder.DropTable(
                name: "Postcode");

            migrationBuilder.DropTable(
                name: "PurposeStay");

            migrationBuilder.DropTable(
                name: "RateType");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "ReportBatch");

            migrationBuilder.DropTable(
                name: "ReportSecurity");

            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "ReservationAdditional");

            migrationBuilder.DropTable(
                name: "ReservationBillingContact");

            migrationBuilder.DropTable(
                name: "ReservationGuest");

            migrationBuilder.DropTable(
                name: "ReservationRate");

            migrationBuilder.DropTable(
                name: "ReservationRoom");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "RoomBlock");

            migrationBuilder.DropTable(
                name: "RoomStatus");

            migrationBuilder.DropTable(
                name: "RoomType");

            migrationBuilder.DropTable(
                name: "SqoopeGroup");

            migrationBuilder.DropTable(
                name: "SqoopeGroupLink");

            migrationBuilder.DropTable(
                name: "SqoopeMsgGroupLink");

            migrationBuilder.DropTable(
                name: "SqoopeMsgLog");

            migrationBuilder.DropTable(
                name: "SqoopeMsgType");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "SupervisorCheckList");

            migrationBuilder.DropTable(
                name: "Title");

            migrationBuilder.DropColumn(
                name: "PIN",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "StaffKey",
                table: "AbpUsers");
        }
    }
}
