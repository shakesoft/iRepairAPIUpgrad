using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class RegistrationViewData
    {
        public RegistrationViewData()
        {
            Country = new HashSet<CountryOutput>();
            City = new HashSet<CityOutput>();
            Title = new HashSet<TitleOutput>();
            Purpose = new HashSet<PurposeOutput>();
        }
        public ICollection<CountryOutput> Country { get; set; }
        public ICollection<CityOutput> City { get; set; }
        public ICollection<TitleOutput> Title { get; set; }
        public ICollection<PurposeOutput> Purpose { get; set; }
    }
    public class ShareGuestRegistrationViewData
    {
        public ShareGuestRegistrationViewData()
        {
            Country = new HashSet<CountryOutput>();
            City = new HashSet<CityOutput>();
            Title = new HashSet<TitleOutput>();
        }
        public ICollection<CountryOutput> Country { get; set; }
        public ICollection<CityOutput> City { get; set; }
        public ICollection<TitleOutput> Title { get; set; }
    }
    public class CountryOutput
    {
        public string Nationality { get; set; }
        public Guid NationalityKey { get; set; }
    }
    public class CityOutput
    {
        public string City { get; set; }
        public Guid CityKey { get; set; }
    }
    public class TitleOutput
    {
        public string Title { get; set; }
        public Guid TitleKey { get; set; }
    }
    public class PurposeOutput
    {
        public string PurposeStay { get; set; }
        public Guid PurposeStayKey { get; set; }
    }
    public class Documentlist
    { 
        public DocSignOutput DocSignOutput { get; set; }
      
    }
    public class ReservationRateOutput
    {
        private int? _BillTo;
        private DateTime? _ChargeDate;
        private string _Covers;
        private string _Description;
        private double? _ForeignAmount;
        private double? _ForeignExchangeRate;
        private string _OverwriteReason;
        private DateTime? _OverwriteTime;
        private string _PostCodeName;
        private double? _Rate;
        private string _Ref1;
        private string _Ref2;
        private double? _Tax1;
        private double? _Tax2;
        private double? _Tax3;
        private double? _Total;
        private string _UserName;
        public int? AwardedPoint { get; set; }
        public int? BillTo
        {
            get
            {
                return this._BillTo;
            }
            set
            {
                this._BillTo = value;
            }
        }

       

        public DateTime? ChargeDate
        {
            get
            {
                return this._ChargeDate;
            }
            set
            {
                this._ChargeDate = value;
            }
        }
        public string ChargeDatedes { get; set; }



        public string Covers
        {
            get
            {
                return this._Covers;
            }
            set
            {
                this._Covers = value;
            }
        }

        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this._Description = value;
            }
        }
        public string ForeignAmountdes { get; set; }
        public double? ForeignAmount
        {
            get
            {
                return this._ForeignAmount;
            }
            set
            {
                this._ForeignAmount = value;
            }
        }
        public string ForeignCurrencyName { get; set; }
        public string ForeignExchangeRatedes { get; set; }

        public double? ForeignExchangeRate
        {
            get
            {
                return this._ForeignExchangeRate;
            }
            set
            {
                this._ForeignExchangeRate = value;
            }
        }


        public string OverwriteReason
        {
            get
            {
                return this._OverwriteReason;
            }
            set
            {
                this._OverwriteReason = value;
            }
        }

        public DateTime? OverwriteTime
        {
            get
            {
                return this._OverwriteTime;
            }
            set
            {
                this._OverwriteTime = value;
            }
        }

        public string OverwriteTimedes { get; set; }

        public string PostCodeName
        {
            get
            {
                return this._PostCodeName;
            }
            set
            {
                this._PostCodeName = value;
            }
        }
        public string Ratedes { get; set; }
        public double? Rate
        {
            get
            {
                return this._Rate;
            }
            set
            {
                this._Rate = value;
            }
        }

        public int? RedeemPoint { get; set; }

        public string lblRef1 { get; set; }
        public string lblRef2 { get; set; }
        public string Ref1
        {
            get
            {
                return this._Ref1;
            }
            set
            {
                this._Ref1 = value;
            }
        }

        public string Ref2
        {
            get
            {
                return this._Ref2;
            }
            set
            {
                this._Ref2 = value;
            }
        }

        public string Tax1des { get; set; }
        public double? Tax1
        {
            get
            {
                return this._Tax1;
            }
            set
            {
                this._Tax1 = value;
            }
        }
        public string Tax2des { get; set; }
        public double? Tax2
        {
            get
            {
                return this._Tax2;
            }
            set
            {
                this._Tax2 = value;
            }
        }
        public string Tax3des { get; set; }
        public double? Tax3
        {
            get
            {
                return this._Tax3;
            }
            set
            {
                this._Tax3 = value;
            }
        }
        public string Totaldes { get; set; }
        public double? Total
        {
            get
            {
                return this._Total;
            }
            set
            {
                this._Total = value;
            }
        }

        public string UserName
        {
            get
            {
                return this._UserName;
            }
            set
            {
                this._UserName = value;
            }
        }

    }
    public class RegistrationDetailData
    {
        public RegistrationDetailData()
        {
            ReservationRateOutputlst = new HashSet<ReservationRateOutput>();
        }
        public string txtReservationNo { get; set; }
        public string txtResDate { get; set; }
        public string txtResStatus { get; set; }
        public string radChkInDate { get; set; }
        public string radChkOutDate { get; set; }
        public string RadNumericTxtNight { get; set; }
        public string txtRateType { get; set; }
        public ICollection<ReservationRateOutput> ReservationRateOutputlst { get; set; }
        
    }
    public class FirstLoadInfo
    {
        public FirstLoadInfo()
        {
            ShareGuestlist = new HashSet<ShareGuestlist>();
            ReservationHistory = new HashSet<ReservationHistory>();
            PurposeStayKey = Guid.Empty;
            PurposeStay = "";
            PreCheckInMessage = "";
            PreCheckInCount="0";
            btnSaveText = "";
        }
        public RegistrationViewData ReservationDropdown { get; set; }
        public MainGuestSignature MainGuestSignature { get; set; }
        public RoomMaxPaxInfo RoomMaxPaxInfo { get; set; }
        public MainGuestInfo MainGuestInfo { get; set; }
        public ICollection<ShareGuestlist> ShareGuestlist { get; set; }
        public ICollection<ReservationHistory> ReservationHistory { get; set; }
        public bool btnSaveShow { get; set; }
        public bool btnChkOutShow { get; set; }
        public string ReservationKey { get; set; }
        public string GuestKey { get; set; }
        public string BookingNo { get; set; }
        public string BookDate { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public string RateType { get; set; }
        public string Roomtype { get; set; }
        public string Night { get; set; }
        public Guid PurposeStayKey { get; set; }//ddlPurposeSelectedValue
        public string PurposeStay { get; set; }//ddlPurposeSelectedText
        public string Request{get;set; }
        public int intPreCheckInCount { get; set; }
        public string PreCheckInMessage{get;set; }
        public string PreCheckInCount { get; set; }
        public string btnSaveText { get; set; }
    }
    public class FirstLoadScanInfo
    {
        public FirstLoadScanInfo()
        {
            ShareGuestlist = new HashSet<ShareGuestlist>();
            PurposeStayKey = Guid.Empty;
            PurposeStay = "";
            PreCheckInMessage = "";
            PreCheckInCount = "0";
            btnSaveText = "";
        }
        public RegistrationViewData ReservationDropdown { get; set; }
        public MainGuestSignature MainGuestSignature { get; set; }
        public RoomMaxPaxInfo RoomMaxPaxInfo { get; set; }
        public MainGuestInfo MainGuestInfo { get; set; }
        public ICollection<ShareGuestlist> ShareGuestlist { get; set; }
        public bool btnSaveShow { get; set; }
        public bool btnChkOutShow { get; set; }
        public string ReservationKey { get; set; }
        public string GuestKey { get; set; }
        public string BookingNo { get; set; }
        public string BookDate { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public string RateType { get; set; }
        public string Roomtype { get; set; }
        public string Night { get; set; }
        public Guid PurposeStayKey { get; set; }//ddlPurposeSelectedValue
        public string PurposeStay { get; set; }//ddlPurposeSelectedText
        public string Request { get; set; }
        public int intPreCheckInCount { get; set; }
        public string PreCheckInMessage { get; set; }
        public string PreCheckInCount { get; set; }
        public string btnSaveText { get; set; }
    }

    public class PdfScanInfo
    {
        public MainGuestInfo MainGuestInfo { get; set; }
        public string BookingNo { get; set; }
        public string BookDate { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public string RateType { get; set; }
        public string Roomtype { get; set; }
        public string Night { get; set; }
    }

    public class GuestDetailScreenInfo
    {
        public GuestDetailScreenInfo() {
            ShareGuestlist = new HashSet<ShareGuestlist>();
        }
        public MainGuestInfo MainGuestInfo { get; set; }
        public MainGuestSignature MainGuestSignature { get; set; }
        public ICollection<ShareGuestlist> ShareGuestlist { get; set; }
        public RoomMaxPaxInfo RoomMaxPaxInfo { get; set; }
        public string BookingNo { get; set; }
        public string BookDate { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public string RateType { get; set; }
        public string Roomtype { get; set; }
        public string Night { get; set; }
    }
    public class ShareGuestFirstLoadInfo
    {
        public ShareGuestFirstLoadInfo()
        {
            ShareGuestlist = new HashSet<ShareGuestlist>();
            tbGuestInfoVisible = true;
        }
        public ShareGuestRegistrationViewData ReservationDropdown { get; set; }
        public RoomMaxPaxInfo RoomMaxPaxInfo { get; set; }
        public MainGuestInfo MainGuestInfo { get; set; }
        public ICollection<ShareGuestlist> ShareGuestlist { get; set; }
        public string BookingNo { get; set; }
        public string BookDate { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public string RateType { get; set; }
        public string Roomtype { get; set; }
        public string Night { get; set; }
        public string Request { get; set; }
        public string btnAddGuestInfoText { get; set; }
        public string SharedGuestInfoHeaderText { get; set; }
        public bool tbGuestInfoVisible { get; set; }
        public bool btnAddGuestVisible { get; set; }
    }
    public class ShareGuestlist
    {
        public string No { get; set; }
        public string Name { get; set; }
        public Guid GuestKey { get; set; }
    }
    public class MainGuestSignature
    {
        public string imgGuestSign { get; set; }
        public bool GuestSign { get; set; }
    }
    public class RoomMaxPaxInfo
    {
        public string RoomDescription { get; set; }//litRoomType
        public string RoomTypePaxText { get; set; }//litRoomMaxPax
        public RoomTypePaxLabel RoomTypePaxLabel { get; set; }//placeholderRoomPax
        public string RoomTypePax { get; set; }//litRoomMaxPaxCount
    }
    public class RoomTypePaxLabel
    {
        public string imgPaxText { get; set; }
        public string imgPaxToolTip { get; set; }
    }
    public class MainGuestInfo
    {
        public MainGuestInfo()
        {
            Title = "";
            litIsNewText = "1";
        }
        public string Title { get; set; }//ddlTitleSelectedValue
        public Guid TitleKey { get; set; }//ddlTitleSelectedValue
        public string FirstName{ get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string NRIC { get; set; }
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Postal { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }//ddlCitySelectedText
        public Guid CityKey { get; set; }//ddlCitySelectedValue
        public string Nationality { get; set; }//ddlCountrySelectedText
        public Guid NationalityKey { get; set; }//ddlCountrySelectedValue
        public string CheckOutDate { get; set; }

        #region for addshareguest
        public string ReservationKey { get; set; }
        public string litIsNewText { get; set; }
        public string CheckInDate { get; set; }
        public string GuestKey { get; set; }
        #endregion
    }
    public class CGuest
    {
        #region Constructor
        public CGuest() { }
        #endregion

        #region Members
        private Guid _GuestKey;
        private char? _Gender;
        private DateTime? _DOB;
        private string _Tel = string.Empty;
        private string _Mobile = string.Empty;
        private string _Fax = string.Empty;
        private string _EMail = string.Empty;
        private string _Postal = string.Empty;
        private string _CountryKey;
        private string _NationalityKey;
        private DateTime? _PassportExpiry;
        private string _Title = string.Empty;
        private string _LastName = string.Empty;
        private string _FirstName = string.Empty;
        private string _Address = string.Empty;
        private string _City = string.Empty;
        //private CPayment _GuestPaymentInfo = new CPayment();
        private string _Passport = string.Empty;
        private string _Name = string.Empty;
        private string _NationalityName = string.Empty;

        private Guid _ReservationKey;
        private DateTime _CheckInDate;
        private DateTime _CheckOutDate;
        private int _GuestStay = 0;

        #endregion

        #region Properties
        public Guid GuestKey
        {
            get { return _GuestKey; }
            set { _GuestKey = value; }
        }
        public char? Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }
        public DateTime? DOB
        {
            get { return _DOB; }
            set { _DOB = value; }
        }
        public string Tel
        {
            get { return _Tel; }
            set { _Tel = value; }
        }
        public string Mobile
        {
            get { return _Mobile; }
            set { _Mobile = value; }
        }
        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }
        public string EMail
        {
            get { return _EMail; }
            set { _EMail = value; }
        }
        public string Postal
        {
            get { return _Postal; }
            set { _Postal = value; }
        }
        public string CountryKey
        {
            get { return _CountryKey; }
            set { _CountryKey = value; }
        }
        public string NationalityKey
        {
            get { return _NationalityKey; }
            set { _NationalityKey = value; }
        }
        public DateTime? PassportExpiry
        {
            get { return _PassportExpiry; }
            set { _PassportExpiry = value; }
        }
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        //public CPayment GuestPaymentInfo
        //{
        //    get { return _GuestPaymentInfo; }
        //    set { _GuestPaymentInfo = value; }
        //}
        public string Passport
        {
            get { return _Passport; }
            set { _Passport = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string NationalityName
        {
            get { return _NationalityName; }
            set { _NationalityName = value; }
        }

        public Guid ReservationKey
        {
            get { return _ReservationKey; }
            set { _ReservationKey = value; }
        }
        public DateTime CheckInDate
        {
            get { return _CheckInDate; }
            set { _CheckInDate = value; }
        }
        public DateTime CheckOutDate
        {
            get { return _CheckOutDate; }
            set { _CheckOutDate = value; }
        }
        public int GuestStay
        {
            get { return _GuestStay; }
            set { _GuestStay = value; }
        }
        public int? TenantId { get; set; }
        #endregion
    }
    public class ReservationHistory
    {
        public DateTime CheckInDate { get; set; }
        public string CheckInDatedes { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string CheckOutDatedes { get; set; }
        public string StatusDesc { get; set; }
        public  string DocNo { get; set; }
        public  int? Adult { get; set; }
        public  int? Child { get; set; }
        public  string RateCode { get; set; }
        public  string Unit { get; set; }
        public  string RoomTypeName { get; set; }
        public  decimal? Amount { get; set; }

    }
    public class ReservationDetailOutput
    {
        private int? _AdditionalBed;
        private int? _Adult;
        private Guid? _BookingAgentKey;
        private Guid? _BookingChannelKey;
        private Guid? _BookingKey;
        private Guid? _CampaignKey;
        private DateTime? _CancellationDate;
        private Guid? _CancellationKey;
        private Guid? _CancellationStaffKey;
        private string _CardName;
        private string _CardNo;
        private string _CCExpiry;
        private int? _ChargeBack;
        private string _CheckIn_Instruction;
        private DateTime? _CheckInDate;
        private Guid? _CheckInStaffKey;
        private string _CheckInTime;
        private string _CheckOut_Instruction;
        private DateTime? _CheckOutDate;
        private Guid? _CheckOutStaffKey;
        private string _CheckOutTime;
        private int? _Child;
        private int? _COA;
        private string _CompanyAddress;
        private string _CompanyCity;
        private string _CompanyContact;
        private Guid? _CompanyCountryKey;
        private Guid? _CompanyKey;
        private string _CompanyPostal;
        private string _CompanyTel;
        private string _ContactPerson;
        private Guid? _DebtorKey;
        private int? _Definite;
        private string _Department;
        private string _DepartmentAddress;
        private string _DepartmentCity;
        private Guid? _DepartmentCountryKey;
        private Guid? _DepartmentKey;
        private string _DepartmentPostal;
        private double? _DepositAmount;
        private DateTime? _DocDate;
        private string _DocNo;
        private int? _DoNotMoveRoom;
        private string _ETA;
        private string _ETD;
        private int? _Extra1;
        private int? _Extra10;
        private int? _Extra11;
        private int? _Extra12;
        private int? _Extra13;
        private int? _Extra14;
        private int? _Extra15;
        private int? _Extra16;
        private int? _Extra17;
        private int? _Extra18;
        private int? _Extra19;
        private int? _Extra2;
        private int? _Extra20;
        private int? _Extra21;
        private int? _Extra22;
        private int? _Extra23;
        private int? _Extra24;
        private int? _Extra3;
        private int? _Extra4;
        private int? _Extra5;
        private int? _Extra6;
        private int? _Extra7;
        private int? _Extra8;
        private int? _Extra9;
        private string _Flight;
        private double? _Folio_Limit;
        private double? _Folio_Limit2;
        private Guid? _GeoSourceKey;
        private Guid? _Group1Key;
        private Guid? _Group2Key;
        private Guid? _Group3Key;
        private Guid? _Group4Key;
        private string _GroupCity;
        private Guid? _GroupCountryKey;
        private string _GroupName;
        private Guid? _GroupNationalityKey;
        private Guid? _GroupRegionKey;
        private Guid? _GroupReservationKey;
        private int? _Guaranteed;
        private string _GuestAddress;
        private string _GuestCity;
        private Guid? _GuestCountryKey;
        private DateTime? _GuestDOB;
        private string _GuestEmail;
        private char? _GuestGender;
        private Guid? _GuestIdentityTypeKey;
        private Guid? _GuestKey;
        private string _GuestMobile;
        private string _GuestName;
        private Guid? _GuestNationalityKey;
        private string _GuestPassport;
        private DateTime? _GuestPassportExpiry;
        private string _GuestPostal;
        private Guid? _GuestRegionKey;
        private int? _GuestStatus;
        private int? _GuestStay;
        private string _GuestTel;
        private Guid? _LastModifiedStaff;
        private int? _LongStay;
        private string _LoyaltyCard;
        private string _LoyaltyCardNo;
        private double? _MainFolioBalance;
        private Guid? _MarketKey;
        private DateTime? _NightAuditAddItemDate;
        private DateTime? _NightAuditRoomChargeDate;
        private DateTime? _OrgCheckOutDate;
        private double? _OtherFolioBalance;
        private int? _PayCommission;
        private int? _PaymentStatus;
       // private Guid? _PaymentTypeKey;
        private DateTime? _PostDate;
        private string _Posting_Instruction;
        private char? _PrintRate;
        private Guid? _PromotionKey;
        private Guid? _RateKey;
        private Guid? _RateTypeKey;
        private int? _RecheckIn;
        private DateTime? _RecheckInDatetime;
        private string _ReCheckInReason;
        private Guid? _ReCheckInStaffKey;
        private int? _RecheckInVirtualRoom;
        private string _Ref1;
        private string _Ref2;
        private string _Remark;
        private char? _RemarkToHistory;
        private Guid? _ReportingRateTypeKey;
        private Guid _ReservationKey;
        private Guid? _ReservationStaffKey;
        private char _ReservationType;
        //private Guid? _RoomKey;
        private Guid? _RoomTypeKey;
        private Guid? _SecurityKey;
        private int _Seq;
        private int? _Sort;
        private Guid? _SourceKey;
        private int? _Status;
        private int? _Sync;
        private Guid? _TransferChargesToKey;
        private Guid? _TravelAgentKey;
        private Guid? _TravelReasonKey;
        private Guid? _TravelTypeKey;
        private string _TS;
        private int? _UseAllotment;
        private string _VoucherNo;
        private string _LanguageCode;
        private Char? _Subscribe;
        private int? _GuestDND;
        private string _GuestCOS;
        private string _GuestLanguageCode;
        private int? _PreCheckIn;
        private Guid? _PurposeOfStay;
        private string _CVV;
        private string _ReprotingRateCode;
        private Guid? _OrgRatePromotionKey;
        private string _GuestArrivedTime;
        private int? _ExpressCheckOut;
        private DateTime? _CutOffDate;
        private int? _CutOffDays;
        private DateTime? _DecisionDate;
        private DateTime? _FollowUpDate;
        private Guid? _GroupBlockingKey;
        private string _Title;
        private string _LastName;
        private string _FirstName;


        public ReservationDetailOutput()
        {
        }

        public ReservationDetailOutput(Guid pKey)
        {
            this._ReservationKey = pKey;
        }

        public string AccountName { get; set; }

        public int? AdditionalBed
        {
            get
            {
                return this._AdditionalBed;
            }
            set
            {
                this._AdditionalBed = value;
            }
        }

        public int? Adult
        {
            get
            {
                return this._Adult;
            }
            set
            {
                this._Adult = value;
            }
        }

        public Guid? BillingContactAccountKey { get; set; }

        public Guid? BookingAgentKey
        {
            get
            {
                return this._BookingAgentKey;
            }
            set
            {
                this._BookingAgentKey = value;
            }
        }

        public string BookingAgentName { get; set; }

        public Guid? BookingChannelKey
        {
            get
            {
                return this._BookingChannelKey;
            }
            set
            {
                this._BookingChannelKey = value;
            }
        }

        public Guid? BookingKey
        {
            get
            {
                return this._BookingKey;
            }
            set
            {
                this._BookingKey = value;
            }
        }

        public Guid? CampaignKey
        {
            get
            {
                return this._CampaignKey;
            }
            set
            {
                this._CampaignKey = value;
            }
        }

        public DateTime? CancellationDate
        {
            get
            {
                return this._CancellationDate;
            }
            set
            {
                this._CancellationDate = value;
            }
        }

        public Guid? CancellationKey
        {
            get
            {
                return this._CancellationKey;
            }
            set
            {
                this._CancellationKey = value;
            }
        }

        public Guid? CancellationStaffKey
        {
            get
            {
                return this._CancellationStaffKey;
            }
            set
            {
                this._CancellationStaffKey = value;
            }
        }

        public string CardName
        {
            get
            {
                return this._CardName;
            }
            set
            {
                this._CardName = value;
            }
        }

        public string CardNo
        {
            get
            {
                return this._CardNo;
            }
            set
            {
                this._CardNo = value;
            }
        }

        public string CCExpiry
        {
            get
            {
                return this._CCExpiry;
            }
            set
            {
                this._CCExpiry = value;
            }
        }

        public int? ChargeBack
        {
            get
            {
                return this._ChargeBack;
            }
            set
            {
                this._ChargeBack = value;
            }
        }

        public string CheckIn_Instruction
        {
            get
            {
                return this._CheckIn_Instruction;
            }
            set
            {
                this._CheckIn_Instruction = value;
            }
        }

        public DateTime? CheckInDate
        {
            get
            {
                return this._CheckInDate;
            }
            set
            {
                this._CheckInDate = value;
            }
        }

        public Guid? CheckInStaffKey
        {
            get
            {
                return this._CheckInStaffKey;
            }
            set
            {
                this._CheckInStaffKey = value;
            }
        }

        public string CheckInTime
        {
            get
            {
                return this._CheckInTime;
            }
            set
            {
                this._CheckInTime = value;
            }
        }

        public string CheckOut_Instruction
        {
            get
            {
                return this._CheckOut_Instruction;
            }
            set
            {
                this._CheckOut_Instruction = value;
            }
        }

        public DateTime? CheckOutDate
        {
            get
            {
                return this._CheckOutDate;
            }
            set
            {
                this._CheckOutDate = value;
            }
        }

        public Guid? CheckOutStaffKey
        {
            get
            {
                return this._CheckOutStaffKey;
            }
            set
            {
                this._CheckOutStaffKey = value;
            }
        }

        public string CheckOutTime
        {
            get
            {
                return this._CheckOutTime;
            }
            set
            {
                this._CheckOutTime = value;
            }
        }

        public int? Child
        {
            get
            {
                return this._Child;
            }
            set
            {
                this._Child = value;
            }
        }

        public int? COA
        {
            get
            {
                return this._COA;
            }
            set
            {
                this._COA = value;
            }
        }

        public string CompanyAddress
        {
            get
            {
                return this._CompanyAddress;
            }
            set
            {
                this._CompanyAddress = value;
            }
        }

        public string CompanyCity
        {
            get
            {
                return this._CompanyCity;
            }
            set
            {
                this._CompanyCity = value;
            }
        }

        public string CompanyContact
        {
            get
            {
                return this._CompanyContact;
            }
            set
            {
                this._CompanyContact = value;
            }
        }

        public Guid? CompanyCountryKey
        {
            get
            {
                return this._CompanyCountryKey;
            }
            set
            {
                this._CompanyCountryKey = value;
            }
        }

        public Guid? CompanyKey
        {
            get
            {
                return this._CompanyKey;
            }
            set
            {
                this._CompanyKey = value;
            }
        }

        public string CompanyName { get; set; }

        public string CompanyPostal
        {
            get
            {
                return this._CompanyPostal;
            }
            set
            {
                this._CompanyPostal = value;
            }
        }

        public string CompanyTel
        {
            get
            {
                return this._CompanyTel;
            }
            set
            {
                this._CompanyTel = value;
            }
        }

        public string ContactPerson
        {
            get
            {
                return this._ContactPerson;
            }
            set
            {
                this._ContactPerson = value;
            }
        }

        public Guid? DebtorKey
        {
            get
            {
                return this._DebtorKey;
            }
            set
            {
                this._DebtorKey = value;
            }
        }

        public int? Definite
        {
            get
            {
                return this._Definite;
            }
            set
            {
                this._Definite = value;
            }
        }

        public string Department
        {
            get
            {
                return this._Department;
            }
            set
            {
                this._Department = value;
            }
        }

        public string DepartmentAddress
        {
            get
            {
                return this._DepartmentAddress;
            }
            set
            {
                this._DepartmentAddress = value;
            }
        }

        public string DepartmentCity
        {
            get
            {
                return this._DepartmentCity;
            }
            set
            {
                this._DepartmentCity = value;
            }
        }

        public Guid? DepartmentCountryKey
        {
            get
            {
                return this._DepartmentCountryKey;
            }
            set
            {
                this._DepartmentCountryKey = value;
            }
        }

        public Guid? DepartmentKey
        {
            get
            {
                return this._DepartmentKey;
            }
            set
            {
                this._DepartmentKey = value;
            }
        }

        public string DepartmentPostal
        {
            get
            {
                return this._DepartmentPostal;
            }
            set
            {
                this._DepartmentPostal = value;
            }
        }

        public double? DepositAmount
        {
            get
            {
                return this._DepositAmount;
            }
            set
            {
                this._DepositAmount = value;
            }
        }

        public DateTime? DocDate
        {
            get
            {
                return this._DocDate;
            }
            set
            {
                this._DocDate = value;
            }
        }

        public string DocNo
        {
            get
            {
                return this._DocNo;
            }
            set
            {
                this._DocNo = value;
            }
        }

        public int? DoNotMoveRoom
        {
            get
            {
                return this._DoNotMoveRoom;
            }
            set
            {
                this._DoNotMoveRoom = value;
            }
        }

        public string ETA
        {
            get
            {
                return this._ETA;
            }
            set
            {
                this._ETA = value;
            }
        }

        public string ETD
        {
            get
            {
                return this._ETD;
            }
            set
            {
                this._ETD = value;
            }
        }

        public int? Extra1
        {
            get
            {
                return this._Extra1;
            }
            set
            {
                this._Extra1 = value;
            }
        }

        public int? Extra10
        {
            get
            {
                return this._Extra10;
            }
            set
            {
                this._Extra10 = value;
            }
        }

        public int? Extra11
        {
            get
            {
                return this._Extra11;
            }
            set
            {
                this._Extra11 = value;
            }
        }

        public int? Extra12
        {
            get
            {
                return this._Extra12;
            }
            set
            {
                this._Extra12 = value;
            }
        }

        public int? Extra13
        {
            get
            {
                return this._Extra13;
            }
            set
            {
                this._Extra13 = value;
            }
        }

        public int? Extra14
        {
            get
            {
                return this._Extra14;
            }
            set
            {
                this._Extra14 = value;
            }
        }

        public int? Extra15
        {
            get
            {
                return this._Extra15;
            }
            set
            {
                this._Extra15 = value;
            }
        }

        public int? Extra16
        {
            get
            {
                return this._Extra16;
            }
            set
            {
                this._Extra16 = value;
            }
        }

        public int? Extra17
        {
            get
            {
                return this._Extra17;
            }
            set
            {
                this._Extra17 = value;
            }
        }

        public int? Extra18
        {
            get
            {
                return this._Extra18;
            }
            set
            {
                this._Extra18 = value;
            }
        }

        public int? Extra19
        {
            get
            {
                return this._Extra19;
            }
            set
            {
                this._Extra19 = value;
            }
        }

        public int? Extra2
        {
            get
            {
                return this._Extra2;
            }
            set
            {
                this._Extra2 = value;
            }
        }

        public int? Extra20
        {
            get
            {
                return this._Extra20;
            }
            set
            {
                this._Extra20 = value;
            }
        }

        public int? Extra21
        {
            get
            {
                return this._Extra21;
            }
            set
            {
                this._Extra21 = value;
            }
        }

        public int? Extra22
        {
            get
            {
                return this._Extra22;
            }
            set
            {
                this._Extra22 = value;
            }
        }

        public int? Extra23
        {
            get
            {
                return this._Extra23;
            }
            set
            {
                this._Extra23 = value;
            }
        }

        public int? Extra24
        {
            get
            {
                return this._Extra24;
            }
            set
            {
                this._Extra24 = value;
            }
        }

        public int? Extra3
        {
            get
            {
                return this._Extra3;
            }
            set
            {
                this._Extra3 = value;
            }
        }

        public int? Extra4
        {
            get
            {
                return this._Extra4;
            }
            set
            {
                this._Extra4 = value;
            }
        }

        public int? Extra5
        {
            get
            {
                return this._Extra5;
            }
            set
            {
                this._Extra5 = value;
            }
        }

        public int? Extra6
        {
            get
            {
                return this._Extra6;
            }
            set
            {
                this._Extra6 = value;
            }
        }

        public int? Extra7
        {
            get
            {
                return this._Extra7;
            }
            set
            {
                this._Extra7 = value;
            }
        }

        public int? Extra8
        {
            get
            {
                return this._Extra8;
            }
            set
            {
                this._Extra8 = value;
            }
        }

        public int? Extra9
        {
            get
            {
                return this._Extra9;
            }
            set
            {
                this._Extra9 = value;
            }
        }

        public string Flight
        {
            get
            {
                return this._Flight;
            }
            set
            {
                this._Flight = value;
            }
        }

        public double? Folio_Limit
        {
            get
            {
                return this._Folio_Limit;
            }
            set
            {
                this._Folio_Limit = value;
            }
        }

        public double? Folio_Limit2
        {
            get
            {
                return this._Folio_Limit2;
            }
            set
            {
                this._Folio_Limit2 = value;
            }
        }

        public Guid? GeoSourceKey
        {
            get
            {
                return this._GeoSourceKey;
            }
            set
            {
                this._GeoSourceKey = value;
            }
        }

        public Guid? Group1Key
        {
            get
            {
                return this._Group1Key;
            }
            set
            {
                this._Group1Key = value;
            }
        }

        public Guid? Group2Key
        {
            get
            {
                return this._Group2Key;
            }
            set
            {
                this._Group2Key = value;
            }
        }

        public Guid? Group3Key
        {
            get
            {
                return this._Group3Key;
            }
            set
            {
                this._Group3Key = value;
            }
        }

        public Guid? Group4Key
        {
            get
            {
                return this._Group4Key;
            }
            set
            {
                this._Group4Key = value;
            }
        }

        public string GroupCity
        {
            get
            {
                return this._GroupCity;
            }
            set
            {
                this._GroupCity = value;
            }
        }

        public Guid? GroupCountryKey
        {
            get
            {
                return this._GroupCountryKey;
            }
            set
            {
                this._GroupCountryKey = value;
            }
        }

        public string GroupName
        {
            get
            {
                return this._GroupName;
            }
            set
            {
                this._GroupName = value;
            }
        }

        public Guid? GroupNationalityKey
        {
            get
            {
                return this._GroupNationalityKey;
            }
            set
            {
                this._GroupNationalityKey = value;
            }
        }

        public Guid? GroupRegionKey
        {
            get
            {
                return this._GroupRegionKey;
            }
            set
            {
                this._GroupRegionKey = value;
            }
        }

        public Guid? GroupReservationKey
        {
            get
            {
                return this._GroupReservationKey;
            }
            set
            {
                this._GroupReservationKey = value;
            }
        }

        public int? Guaranteed
        {
            get
            {
                return this._Guaranteed;
            }
            set
            {
                this._Guaranteed = value;
            }
        }

        public string GuestAddress
        {
            get
            {
                return this._GuestAddress;
            }
            set
            {
                this._GuestAddress = value;
            }
        }

        public string GuestCity
        {
            get
            {
                return this._GuestCity;
            }
            set
            {
                this._GuestCity = value;
            }
        }

        public Guid? GuestCountryKey
        {
            get
            {
                return this._GuestCountryKey;
            }
            set
            {
                this._GuestCountryKey = value;
            }
        }

        public DateTime? GuestDOB
        {
            get
            {
                return this._GuestDOB;
            }
            set
            {
                this._GuestDOB = value;
            }
        }

        public string GuestEmail
        {
            get
            {
                return this._GuestEmail;
            }
            set
            {
                this._GuestEmail = value;
            }
        }

        public char? GuestGender
        {
            get
            {
                return this._GuestGender;
            }
            set
            {
                this._GuestGender = value;
            }
        }

        public Guid? GuestIdentityTypeKey
        {
            get
            {
                return this._GuestIdentityTypeKey;
            }
            set
            {
                this._GuestIdentityTypeKey = value;
            }
        }

        public Guid? GuestKey
        {
            get
            {
                return this._GuestKey;
            }
            set
            {
                this._GuestKey = value;
            }
        }

        public string GuestMobile
        {
            get
            {
                return this._GuestMobile;
            }
            set
            {
                this._GuestMobile = value;
            }
        }

        public string GuestName
        {
            get
            {
                return this._GuestName;
            }
            set
            {
                this._GuestName = value;
            }
        }

        public Guid? GuestNationalityKey
        {
            get
            {
                return this._GuestNationalityKey;
            }
            set
            {
                this._GuestNationalityKey = value;
            }
        }

        public string GuestPassport
        {
            get
            {
                return this._GuestPassport;
            }
            set
            {
                this._GuestPassport = value;
            }
        }

        public DateTime? GuestPassportExpiry
        {
            get
            {
                return this._GuestPassportExpiry;
            }
            set
            {
                this._GuestPassportExpiry = value;
            }
        }

        public string GuestPostal
        {
            get
            {
                return this._GuestPostal;
            }
            set
            {
                this._GuestPostal = value;
            }
        }

        public Guid? GuestRegionKey
        {
            get
            {
                return this._GuestRegionKey;
            }
            set
            {
                this._GuestRegionKey = value;
            }
        }

        public int? GuestStatus
        {
            get
            {
                return this._GuestStatus;
            }
            set
            {
                this._GuestStatus = value;
            }
        }

        public int? GuestStay
        {
            get
            {
                return this._GuestStay;
            }
            set
            {
                this._GuestStay = value;
            }
        }

        public string GuestTel
        {
            get
            {
                return this._GuestTel;
            }
            set
            {
                this._GuestTel = value;
            }
        }

        public Guid? LastModifiedStaff
        {
            get
            {
                return this._LastModifiedStaff;
            }
            set
            {
                this._LastModifiedStaff = value;
            }
        }

        public int? LongStay
        {
            get
            {
                return this._LongStay;
            }
            set
            {
                this._LongStay = value;
            }
        }

        public string LoyaltyCard
        {
            get
            {
                return this._LoyaltyCard;
            }
            set
            {
                this._LoyaltyCard = value;
            }
        }

        public string LoyaltyCardNo
        {
            get
            {
                return this._LoyaltyCardNo;
            }
            set
            {
                this._LoyaltyCardNo = value;
            }
        }

        public Guid? MaidStatusKey { get; set; }

        public double? MainFolioBalance
        {
            get
            {
                return this._MainFolioBalance;
            }
            set
            {
                this._MainFolioBalance = value;
            }
        }

        public Guid? MarketKey
        {
            get
            {
                return this._MarketKey;
            }
            set
            {
                this._MarketKey = value;
            }
        }

        public DateTime? NightAuditAddItemDate
        {
            get
            {
                return this._NightAuditAddItemDate;
            }
            set
            {
                this._NightAuditAddItemDate = value;
            }
        }

        public DateTime? NightAuditRoomChargeDate
        {
            get
            {
                return this._NightAuditRoomChargeDate;
            }
            set
            {
                this._NightAuditRoomChargeDate = value;
            }
        }

        public DateTime? OrgCheckOutDate
        {
            get
            {
                return this._OrgCheckOutDate;
            }
            set
            {
                this._OrgCheckOutDate = value;
            }
        }

        public double? OtherFolioBalance
        {
            get
            {
                return this._OtherFolioBalance;
            }
            set
            {
                this._OtherFolioBalance = value;
            }
        }

        public int? PayCommission
        {
            get
            {
                return this._PayCommission;
            }
            set
            {
                this._PayCommission = value;
            }
        }

        public int? PaymentStatus
        {
            get
            {
                return this._PaymentStatus;
            }
            set
            {
                this._PaymentStatus = value;
            }
        }

       

        public DateTime? PostDate
        {
            get
            {
                return this._PostDate;
            }
            set
            {
                this._PostDate = value;
            }
        }

        public string Posting_Instruction
        {
            get
            {
                return this._Posting_Instruction;
            }
            set
            {
                this._Posting_Instruction = value;
            }
        }

        public char? PrintRate
        {
            get
            {
                return this._PrintRate;
            }
            set
            {
                this._PrintRate = value;
            }
        }

        public Guid? PromotionKey
        {
            get
            {
                return this._PromotionKey;
            }
            set
            {
                this._PromotionKey = value;
            }
        }

        public string RateCode { get; set; }

        public Guid? RateKey
        {
            get
            {
                return this._RateKey;
            }
            set
            {
                this._RateKey = value;
            }
        }

        public Guid? RateTypeKey
        {
            get
            {
                return this._RateTypeKey;
            }
            set
            {
                this._RateTypeKey = value;
            }
        }

        public bool ReCalculateRate { get; set; }

        public int? RecheckIn
        {
            get
            {
                return this._RecheckIn;
            }
            set
            {
                this._RecheckIn = value;
            }
        }

        public DateTime? RecheckInDatetime
        {
            get
            {
                return this._RecheckInDatetime;
            }
            set
            {
                this._RecheckInDatetime = value;
            }
        }

        public string ReCheckInReason
        {
            get
            {
                return this._ReCheckInReason;
            }
            set
            {
                this._ReCheckInReason = value;
            }
        }

        public Guid? ReCheckInStaffKey
        {
            get
            {
                return this._ReCheckInStaffKey;
            }
            set
            {
                this._ReCheckInStaffKey = value;
            }
        }

        public int? RecheckInVirtualRoom
        {
            get
            {
                return this._RecheckInVirtualRoom;
            }
            set
            {
                this._RecheckInVirtualRoom = value;
            }
        }

        public string Ref1
        {
            get
            {
                return this._Ref1;
            }
            set
            {
                this._Ref1 = value;
            }
        }

        public string Ref2
        {
            get
            {
                return this._Ref2;
            }
            set
            {
                this._Ref2 = value;
            }
        }

        public string Remark
        {
            get
            {
                return this._Remark;
            }
            set
            {
                this._Remark = value;
            }
        }

        public char? RemarkToHistory
        {
            get
            {
                return this._RemarkToHistory;
            }
            set
            {
                this._RemarkToHistory = value;
            }
        }

        public Guid? ReportingRateTypeKey
        {
            get
            {
                return this._ReportingRateTypeKey;
            }
            set
            {
                this._ReportingRateTypeKey = value;
            }
        }

        public Guid ReservationKey
        {
            get
            {
                return this._ReservationKey;
            }
            set
            {
                this._ReservationKey = value;
            }
        }

        public Guid? ReservationStaffKey
        {
            get
            {
                return this._ReservationStaffKey;
            }
            set
            {
                this._ReservationStaffKey = value;
            }
        }

        public char ReservationType
        {
            get
            {
                return this._ReservationType;
            }
            set
            {
                this._ReservationType = value;
            }
        }

        public string ReservedRoomStatusString { get; set; }

        public string ReservedRoomUnit { get; set; }


        public Guid? RoomStatusKey { get; set; }

        public Guid? RoomTypeKey
        {
            get
            {
                return this._RoomTypeKey;
            }
            set
            {
                this._RoomTypeKey = value;
            }
        }

        public string RoomTypeString { get; set; }

        public Guid? SecurityKey
        {
            get
            {
                return this._SecurityKey;
            }
            set
            {
                this._SecurityKey = value;
            }
        }

        public int Seq
        {
            get
            {
                return this._Seq;
            }
            set
            {
                this._Seq = value;
            }
        }

        public int? Sort
        {
            get
            {
                return this._Sort;
            }
            set
            {
                this._Sort = value;
            }
        }

        public Guid? SourceKey
        {
            get
            {
                return this._SourceKey;
            }
            set
            {
                this._SourceKey = value;
            }
        }

        public int? Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                this._Status = value;
            }
        }

        public string StatusString { get; set; }

        public int? Sync
        {
            get
            {
                return this._Sync;
            }
            set
            {
                this._Sync = value;
            }
        }

        public Guid? TransferChargesToKey
        {
            get
            {
                return this._TransferChargesToKey;
            }
            set
            {
                this._TransferChargesToKey = value;
            }
        }

        public Guid? TravelAgentKey
        {
            get
            {
                return this._TravelAgentKey;
            }
            set
            {
                this._TravelAgentKey = value;
            }
        }

        public Guid? TravelReasonKey
        {
            get
            {
                return this._TravelReasonKey;
            }
            set
            {
                this._TravelReasonKey = value;
            }
        }

        public Guid? TravelTypeKey
        {
            get
            {
                return this._TravelTypeKey;
            }
            set
            {
                this._TravelTypeKey = value;
            }
        }

        public string TS
        {
            get
            {
                return this._TS;
            }
            set
            {
                this._TS = value;
            }
        }

        public byte[] TSByte { get; set; }

        public int? UseAllotment
        {
            get
            {
                return this._UseAllotment;
            }
            set
            {
                this._UseAllotment = value;
            }
        }

        public string VoucherNo
        {
            get
            {
                return this._VoucherNo;
            }
            set
            {
                this._VoucherNo = value;
            }
        }
        public string LanguageCode
        {
            get
            {
                return this._LanguageCode;
            }
            set
            {
                this._LanguageCode = value;
            }
        }
        public Char? Subscribe
        {
            get
            {
                return this._Subscribe;
            }
            set
            {
                this._Subscribe = value;
            }
        }
        public int? GuestDND
        {
            get
            {
                return this._GuestDND;
            }
            set
            {
                this._GuestDND = value;
            }
        }
        public string GuestLanguageCode
        {
            get
            {
                return this._GuestLanguageCode;
            }
            set
            {
                this._GuestLanguageCode = value;
            }
        }
        public string GuestCos
        {
            get
            {
                return this._GuestCOS;
            }
            set
            {
                this._GuestCOS = value;
            }
        }
        public int? PreCheckIn
        {
            get
            {
                return this._PreCheckIn;
            }
            set
            {
                this._PreCheckIn = value;
            }
        }
        public Guid? PurposeOfStay
        {
            get
            {
                return this._PurposeOfStay;
            }
            set
            {
                this._PurposeOfStay = value;
            }
        }
        public string CVV
        {
            get
            {
                return this._CVV;
            }
            set
            {
                this._CVV = value;
            }
        }
        public Guid? OrgRatePromotionTypeKey
        {
            get
            {
                return this._OrgRatePromotionKey;
            }
            set
            {
                this._OrgRatePromotionKey = value;
            }
        }
        public string ReportingRateCode
        {
            get { return this._ReprotingRateCode; }
            set { this._ReprotingRateCode = value; }
        }
        public string GuestArrivedTime
        {
            get
            {
                return this._GuestArrivedTime;
            }
            set
            {
                this._GuestArrivedTime = value;
            }
        }
        public int? ExpressCheckOut
        {
            get
            {
                return this._ExpressCheckOut;
            }
            set
            {
                this._ExpressCheckOut = value;
            }
        }
        public DateTime? CutOffDate
        {
            get
            {
                return this._CutOffDate;
            }
            set
            {
                this._CutOffDate = value;
            }
        }
        public int? CutOffDays
        {
            get
            {
                return this._CutOffDays;
            }
            set
            {
                this._CutOffDays = value;
            }
        }
        public DateTime? FollowUpDate
        {
            get
            {
                return this._FollowUpDate;
            }
            set
            {
                this._FollowUpDate = value;
            }
        }
        public DateTime? DecisionDate
        {
            get
            {
                return this._DecisionDate;
            }
            set
            {
                this._DecisionDate = value;
            }
        }
        public Guid? GroupBlockingKey
        {
            get
            {
                return this._GroupBlockingKey;
            }
            set
            {
                this._GroupBlockingKey = value;
            }
        }
        public string Title
        {
            get
            {

                return this._Title;
            }
            set
            {
                this._Title = value;
            }
        }
        public string LastName
        {
            get
            {

                return this._LastName;
            }
            set
            {
                this._LastName = value;
            }
        }
        public string FirstName
        {
            get
            {

                return this._FirstName;
            }
            set
            {
                this._FirstName = value;
            }
        }
        public enum ReservationFilterType
        {
            CheckIn = 30,
            CheckOut = 40,
            Folio = 20,
            Reservation = 10,
            InHouse = 50,
            RecentCinCout = 60
        }

        public enum ReservationStatus
        {
            CancelA = -1,
            CancelB = -10,
            CheckIn = 2,
            CheckOut = 10,
            OnReserve = 1,
            Pending = 0
        }
    }
    public class DocSignInput
    {
        public byte[] imgSign { get; set; }
    }
    public class DocSignOutput
    {
        public string GuestSign { get; set; }
    }
    public class DocumentSign
    {
        public DocumentSign()
        {
            //
            // TODO: Add constructor logic here
            //
            Description = "Guest Signature";
            DocumentName = "Guest Signature";
        }

        public Guid DocumentKey { get; set; }
        public Guid? GuestKey { get; set; }
        public Guid? CompanyKey { get; set; }
        public Guid? LastModifiedStaff { get; set; }
        public int? Sort { get; set; }
        public int? Sync { get; set; }
        public int? Seq { get; set; }
        public string _TS { get; set; }
        public byte[] TSByte { get; set; }
        public byte[] DocumentStore { get; set; }
        public string Description { get; set; }
        public string DocumentName { get; set; }
        public Guid? ReservationKey { get; set; }
        public byte[] Signature { get; set; }
        public string GuestSign { get; set; }
    }

    public class CHistory
    {
        public CHistory()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public DateTime? ChangedDate { get; set; }
        public string Detail { get; set; }
        public Guid HistoryKey { get; set; }
        public Guid? LinkKey { get; set; }
        public string ModuleName { get; set; }
        public char? Operation { get; set; }
        public int Seq { get; set; }
        public int? Sort { get; set; }
        public Guid? SourceKey { get; set; }
        public Guid? StaffKey { get; set; }
        public string StaffName { get; set; }
        public int? Sync { get; set; }
        public string TableName { get; set; }
        public string TS { get; set; }
        public int? TenantId { get; set; }

    }
    public class MainGuestInfoViewData
    {
        public MainGuestInfoViewData()
        {
            GuestSign = false;
            TitleKey = Guid.Empty;
            Title = "";
            CityKey = Guid.Empty;
            City = "--";
            NationalityKey = Guid.Empty;
            Nationality = "--";
            PreCheckInCount = "0";
            litIsNewText = "1";
            Address2 = "";
        }
        public bool GuestSign { get; set; }
        public string Title { get; set; }//ddlTitleSelectedValue
        public Guid TitleKey { get; set; }//ddlTitleSelectedValue
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string NRIC { get; set; }
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Postal { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }//ddlCitySelectedText
        public Guid CityKey { get; set; }//ddlCitySelectedValue
        public string Nationality { get; set; }//ddlCountrySelectedText
        public Guid NationalityKey { get; set; }//ddlCountrySelectedValue
        public string PreCheckInCount { get; set; }
        public string GuestKey { get; set; }
        public string logintime { get; set; }
        public string BookingNo { get; set; }
        public string CheckOutDate { get; set; }
        #region for addshareguest
        public string ReservationKey { get; set; }
        public string litIsNewText { get; set; }
        public string CheckInDate { get; set; }
        #endregion
    }

    public class SignatureToImage
    {
        public Color BackgroundColor { get; set; }
        public Color PenColor { get; set; }
        public int CanvasWidth { get; set; }
        public int CanvasHeight { get; set; }
        public float PenWidth { get; set; }
        public float FontSize { get; set; }
        public string FontName { get; set; }

        /// <summary>
        /// Gets a new signature generator with the default options.
        /// </summary>
        public SignatureToImage()
        {
            // Default values
            BackgroundColor = Color.White;
            PenColor = Color.FromArgb(20, 83, 148);
            CanvasWidth = 400;
            CanvasHeight = 200;
            PenWidth = 2;
            FontSize = 24;
            FontName = "Journal";
        }

        /// <summary>
        /// Draws a signature based on the JSON provided by Signature Pad.
        /// </summary>
        /// <param name="json">JSON string of line drawing commands.</param>
        /// <returns>Bitmap image containing the signature.</returns>
        public Bitmap SigJsonToImage(string json)
        {
            var signatureImage = GetBlankCanvas();
            if (!string.IsNullOrWhiteSpace(json))
            {
                using (var signatureGraphic = Graphics.FromImage(signatureImage))
                {
                    signatureGraphic.SmoothingMode = SmoothingMode.AntiAlias;
                    var pen = new Pen(PenColor, PenWidth);
                    var serializer = new JavaScriptSerializer();
                    // Next line may throw System.ArgumentException if the string
                    // is an invalid json primitive for the SignatureLine structure
                    var lines = serializer.Deserialize<List<SignatureLine>>(json);
                    foreach (var line in lines)
                    {
                        signatureGraphic.DrawLine(pen, line.lx, line.ly, line.mx, line.my);
                    }
                }
            }
            return signatureImage;
        }

        /// <summary>
        /// Draws an approximation of a signature using a font.
        /// </summary>
        /// <param name="name">The string that will be drawn.</param>
        /// <param name="fontPath">Full path of font file to be used if default font is not installed on the system.</param>
        /// <returns>Bitmap image containing the user's signature.</returns>
        public Bitmap SigNameToImage(string name, string fontPath = null)
        {
            var signatureImage = GetBlankCanvas();
            if (!string.IsNullOrWhiteSpace(name))
            {
                Font font;
                // Need a reference to the font, be it the .ttf in the project or the system-installed font
                if (string.IsNullOrWhiteSpace(fontPath))
                {
                    // Path parameter not provided, try to use system-installed font
                    var installedFontCollection = new InstalledFontCollection();
                    if (installedFontCollection.Families.Any(f => f.Name == FontName))
                    {
                        font = new Font(FontName, FontSize);
                    }
                    else
                    {
                        throw new ArgumentException("The full path of the font file must be provided when the specified font is not installed on the system.", "fontPath");
                    }
                }
                else if (File.Exists(fontPath))
                {
                    try
                    {
                        // Temporarily install font while not affecting the system-installed collection
                        var collection = new PrivateFontCollection();
                        collection.AddFontFile(fontPath);
                        font = new Font(collection.Families.First(), FontSize);
                    }
                    catch (FileNotFoundException)
                    {
                        // Since the existence of the file has already been tested, this exception
                        // means the file is invalid or not supported when trying to load
                        throw new Exception("The specified font file \"" + fontPath + "\" is either invalid or not supported.");
                    }
                }
                else
                {
                    throw new FileNotFoundException("The specified font file \"" + fontPath + "\" does not exist or permission was denied.", fontPath);
                }
                using (var signatureGraphic = Graphics.FromImage(signatureImage))
                {
                    signatureGraphic.TextRenderingHint = TextRenderingHint.AntiAlias;
                    signatureGraphic.DrawString(name, font, new SolidBrush(PenColor), 0, 0);
                }
            }
            return signatureImage;
        }

        /// <summary>
        /// Get a blank bitmap using instance properties for dimensions and background color.
        /// </summary>
        /// <returns>Blank bitmap image.</returns>
        private Bitmap GetBlankCanvas()
        {
            var blankImage = new Bitmap(CanvasWidth, CanvasHeight);
            blankImage.MakeTransparent();
            using (var signatureGraphic = Graphics.FromImage(blankImage))
            {
                signatureGraphic.Clear(BackgroundColor);
            }
            return blankImage;
        }

        /// <summary>
        /// Line drawing commands as generated by the Signature Pad JSON export option.
        /// </summary>
        private class SignatureLine
        {
            public int lx { get; set; }
            public int ly { get; set; }
            public int mx { get; set; }
            public int my { get; set; }
        }

        
    }

    public class RequestGuestOutput
    {
        public Guid ReservationRequestKey { get; set; }
        public string ReservationRequestID { get; set; }
        public DateTime? RequestDate { get; set; }
        public string RequestDatedes { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string ResponseDatedes { get; set; }
        public string HotelResponse { get; set; }
        public string StatusDesc { get; set; }
        public string GuestRequest { get; set; }
        public string RequestTypeName { get; set; }
        public string Unit { get; set; }
        public string DocNo { get; set; }
        public string Name { get; set; }
    }

    public class RequestGuestDataEntryOutput
    {
        public RequestGuestDataEntryViewData RequestGuestDataEntryDropdown { get; set; }
        public ReservationRequestOutput ReservationRequestOutput { get; set; }

    }
    public class RequestGuestDataEntryViewData
    {
        public RequestGuestDataEntryViewData()
        {
            ddlRequestType = new HashSet<RequestTypeOutput>();
        }
        public ICollection<RequestTypeOutput> ddlRequestType { get; set; }
       
    }
    public class RequestTypeOutput
    {
        public string RequestTypeName { get; set; }
        public Guid RequestTypeKey { get; set; }
    }
   
    public class ReservationRequestInput
    {
        public Guid ReservationRequestKey { get; set; }
        public string ReservationRequestID { get; set; }
        public Guid? RequestTypeKey { get; set; }
        public int Status { get; set; }
        public Guid? ReservationKey { get; set; }
        public Guid? GuestKey { get; set; }
        public string GuestRequest { get; set; }
        public DateTime? RequestDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public int? TenantId { get; set; }
    }
    public class ReservationRequestOutput
    {
        public ReservationRequestOutput()
        {
            btnEdit = true;
        }
        public Guid ReservationRequestKey { get; set; }
        public string ReservationRequestID { get; set; }
        public Guid? RequestTypeKey { get; set; }
        public string RequestTypeName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public Guid? ReservationKey { get; set; }
        public Guid? GuestKey { get; set; }
        public string GuestRequest { get; set; }
        public DateTime? RequestDate { get; set; }
        public string HotelResponse { get; set; }
        public DateTime? ResponseDate { get; set; }
        public bool btnEdit { get; set; }
        public string FolioNo { get; set; }
        public string GuestName { get; set; }
    }
}
