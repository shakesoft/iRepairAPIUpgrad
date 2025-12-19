using Abp.Application.Services.Dto;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.UI;
using BEZNgCore.Common;
using System.Data;
using Abp.Runtime.Session;
using System.Drawing;
using System.IO;
using BEZNgCore.Configuration;
using Microsoft.AspNetCore.Hosting;
using IronPdf;
using QRCoder;
//using Twilio.Rest.Preview.Sync.Service;
using System.Net.Mail;
using System.Net;

namespace BEZNgCore.IrepairAppService
{
    public class IciRegistrationAppService : BEZNgCoreAppServiceBase
    {

        private readonly IWebHostEnvironment _env;
        private readonly IAppConfigurationAccessor _configurationAccessor;
        private readonly IRegistrationdalRepository _registrationdalRepository;
        public IciRegistrationAppService(
            IWebHostEnvironment env,
            IAppConfigurationAccessor configurationAccessor,
            IRegistrationdalRepository registrationdalRepository)
        {
            _env = env;
            _configurationAccessor = configurationAccessor;
            _registrationdalRepository = registrationdalRepository;
        }
        [HttpGet]
        public ListResultDto<RegistrationViewData> GetRegistrationViewData()
        {
            List<RegistrationViewData> Alllst = new List<RegistrationViewData>();
            RegistrationViewData a = new RegistrationViewData();
            a.Title = GetAllTitle();
            a.City = GetAllCity();
            a.Country = GetAllCountry();
            a.Purpose = GetAllPurpose();
            Alllst.Add(a);
            return new ListResultDto<RegistrationViewData>(Alllst);
        }


        [HttpGet]
        protected List<TitleOutput> GetAllTitle()
        {

            return _registrationdalRepository.GenerateDDLTitle();
        }
        [HttpGet]
        protected List<CityOutput> GetAllCity()
        {

            return _registrationdalRepository.GenerateDDLCity();
        }
        [HttpGet]
        protected List<CountryOutput> GetAllCountry()
        {

            return _registrationdalRepository.GenerateDDLCountry();
        }
        [HttpGet]
        protected List<PurposeOutput> GetAllPurpose()
        {

            return _registrationdalRepository.GenerateDDLPurposeofStay();
        }

        [HttpGet]
        public async Task<ListResultDto<FirstLoadInfo>> GetRegistrationDetailViewData()
        {
            List<FirstLoadInfo> Alllst = new List<FirstLoadInfo>();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //    if (user.Result != null)
                //    {
                string folioNumber = user.PIN;
                string ReservationKey = "";
                string strGuestKey = user.StaffKey.ToString();
                FirstLoadInfo a = new FirstLoadInfo();

                #region ReservationDropdown
                RegistrationViewData ri = new RegistrationViewData();
                ri.Title = GetAllTitle();
                ri.City = GetAllCity();
                ri.Country = GetAllCountry();
                ri.Purpose = GetAllPurpose();
                a.ReservationDropdown = ri;
                #endregion

                DataTable dt = _registrationdalRepository.GetReservationByFolioNumber(folioNumber);
                if (dt.Rows.Count > 0)
                {
                    DateTime dtCheckIndate = Convert.ToDateTime(dt.Rows[0]["CheckInDate"]);
                    DateTime dtCheckOutdate = Convert.ToDateTime(dt.Rows[0]["CheckOutDate"]);
                    TimeSpan tsDay = dtCheckOutdate - dtCheckIndate;
                    int intNoOfNight = tsDay.Days;
                    if (dt.Rows[0]["Status"].ToString() == "2")
                    {
                        a.btnSaveShow = false;
                        a.btnChkOutShow = true;
                    }
                    else if (dt.Rows[0]["Status"].ToString() == "1")
                    {
                        a.btnSaveShow = true;
                        a.btnChkOutShow = false;
                    }
                    else
                    {
                        a.btnSaveShow = false;
                        a.btnChkOutShow = false;
                    }
                    ReservationKey = dt.Rows[0]["ReservationKey"].ToString();
                    a.ReservationKey = ReservationKey;
                    a.GuestKey = dt.Rows[0]["GuestKey"].ToString();
                    a.BookingNo = folioNumber;
                    a.BookDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["DocDate"]);
                    a.CheckInDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["CheckInDate"]);
                    a.CheckOutDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["CheckOutDate"]);
                    a.RateType = dt.Rows[0]["RateDescription"].ToString();
                    a.Roomtype = dt.Rows[0]["RoomDescription"].ToString();
                    a.Night = tsDay.Days.ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[0]["PurposeStayKey"].ToString()))
                        a.PurposeStayKey = new Guid(dt.Rows[0]["PurposeStayKey"].ToString());

                    a.Request = dt.Rows[0]["Remark"].ToString();

                    int intPreCheckInCount = 0;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["PreCheckInCount"].ToString()))
                        intPreCheckInCount = Convert.ToInt32(dt.Rows[0]["PreCheckInCount"]);

                    if (intPreCheckInCount > 0)
                    {
                        a.PreCheckInMessage = "Thank you, Guest! <br/> Your online Check-In process is completed.";
                        a.PreCheckInCount = intPreCheckInCount.ToString();
                    }


                    #region BindMainGuestSignature


                    DocumentSign doc = _registrationdalRepository.BindMainGuestSignature(ReservationKey, strGuestKey);
                    MainGuestSignature mgs = new MainGuestSignature();
                    if (doc.Signature != null)
                    {
                        var varGuestSign = Convert.ToBase64String(doc.Signature);
                        mgs.imgGuestSign = string.Format("data:image/png;base64,{0}", varGuestSign);
                        mgs.GuestSign = true;
                    }
                    else
                    {
                        mgs.GuestSign = false;
                    }
                    a.MainGuestSignature = mgs;


                    #endregion

                    #region BindRoomMaxPaxInfo

                    if (dt.Rows.Count > 0)
                    {
                        RoomMaxPaxInfo rmp = new RoomMaxPaxInfo();
                        rmp.RoomDescription = dt.Rows[0]["RoomDescription"].ToString();

                        rmp.RoomTypePaxText = GetRoomTypePaxTextByRoomTypeSeq(dt.Rows[0]["RoomSeq"].ToString());

                        rmp.RoomTypePaxLabel = GetRoomTypePaxLabelByRoomTypeSeq(dt.Rows[0]["RoomSeq"].ToString());

                        int intRoomMaxPax = GetRoomTypePaxByRoomTypeSeq(dt.Rows[0]["RoomSeq"].ToString());

                        rmp.RoomTypePax = intRoomMaxPax.ToString();
                        a.RoomMaxPaxInfo = rmp;

                    }


                    #endregion
                    #region BindMainGuestInfo

                    CGuest guest = _registrationdalRepository.GetGuestInfoByGuestKey(dt.Rows[0]["GuestKey"].ToString());
                    if (guest != null)
                    {
                        MainGuestInfo mgi = new MainGuestInfo();
                        mgi.GuestKey = dt.Rows[0]["GuestKey"].ToString();
                        mgi.ReservationKey = ReservationKey;
                        mgi.Title = guest.Title.ToString();
                        mgi.CheckInDate = a.CheckInDate;
                        mgi.CheckOutDate = a.CheckOutDate;
                        mgi.FirstName = (!string.IsNullOrEmpty(guest.FirstName)) ? guest.FirstName.Trim() : "";
                        mgi.Lastname = (!string.IsNullOrEmpty(guest.LastName)) ? guest.LastName.Trim() : "";
                        mgi.Email = guest.EMail.Trim();
                        mgi.NRIC = (!string.IsNullOrEmpty(guest.Passport)) ? guest.Passport.Trim() : "";
                        mgi.DOB = (guest.DOB.HasValue ? guest.DOB.Value : (DateTime?)null);
                        if (guest.Address != null)
                        {
                            mgi.Address = (guest.Address.Trim().Length > 60 ? guest.Address.Trim().Substring(0, 60) : guest.Address.Trim());
                            mgi.Address2 = (guest.Address.Trim().Length > 60 ? guest.Address.Trim().Substring(60) : "");
                        }
                        else
                        {
                            mgi.Address = "";
                            mgi.Address2 = "";
                        }
                        mgi.Postal = guest.Postal;
                        mgi.Mobile = guest.Mobile;
                        if (guest.City == null || guest.City.Equals("--") || string.IsNullOrEmpty(guest.City))
                        {
                            mgi.City = "--";
                            mgi.CityKey = Guid.Empty;
                        }
                        else
                        {
                            if (guest.City != null)
                                mgi.City = guest.City;
                            mgi.CityKey = GetCityKey(guest.City);
                        }
                        if (guest.CountryKey == null || guest.CountryKey == Guid.Empty.ToString())
                        {
                            mgi.Nationality = "--";
                            mgi.NationalityKey = Guid.Empty;
                        }
                        else
                        {
                            mgi.Nationality = GetNationality(guest.CountryKey);
                            mgi.NationalityKey = new Guid(guest.CountryKey);
                        }
                        a.MainGuestInfo = mgi;
                    }
                    else
                    {
                        // show error msg
                    }

                    #endregion

                    #region grdSharedGuest
                    DataTable dtr = _registrationdalRepository.GetReservationGuestByReservationKey(ReservationKey);
                    List<ShareGuestlist> slst = new List<ShareGuestlist>();
                    if (dtr.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtr.Rows)
                        {
                            ShareGuestlist o = new ShareGuestlist();
                            o.No = !DBNull.Value.Equals(dr["No"]) ? dr["No"].ToString() : "";
                            o.Name = !DBNull.Value.Equals(dr["Name"]) ? dr["Name"].ToString() : "";
                            o.GuestKey = (!DBNull.Value.Equals(dr["GuestKey"])) ? (!string.IsNullOrEmpty(dr["GuestKey"].ToString()) ? new Guid(dr["GuestKey"].ToString()) : Guid.Empty) : Guid.Empty;

                            slst.Add(o);
                        }
                    }
                    a.ShareGuestlist = slst;
                    #endregion
                    #region grdReservationHistory
                    a.ReservationHistory = _registrationdalRepository.GetReservationByGuestKey(dt.Rows[0]["GuestKey"].ToString());
                    #endregion
                }
                else
                {
                    //go login page
                }

                Alllst.Add(a);
            }
            //else
            //{
            //    throw new UserFriendlyException("Session has expired.");
            //}

            return new ListResultDto<FirstLoadInfo>(Alllst);
        }

        [HttpGet]
        public async Task<ListResultDto<ShareGuestFirstLoadInfo>> GetAddSharedGuestData(bool blnNew = true, string GuestKey = "")
        {
            List<ShareGuestFirstLoadInfo> Alllst = new List<ShareGuestFirstLoadInfo>();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {

                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //if (user.Result != null)
                //{
                string folioNumber = user.PIN;
                string ReservationKey = "";
                string CheckInDate = "";
                string CheckOutDate = "";
                if (string.IsNullOrEmpty(GuestKey))
                {
                    GuestKey = user.StaffKey.ToString();
                }

                ShareGuestFirstLoadInfo a = new ShareGuestFirstLoadInfo();
                int RoomMaxPaxCount = 0;
                #region ReservationDropdown
                ShareGuestRegistrationViewData ri = new ShareGuestRegistrationViewData();
                ri.Title = GetAllTitle();
                ri.City = GetAllCity();
                ri.Country = GetAllCountry();
                a.ReservationDropdown = ri;
                #endregion

                #region Reservationdetail
                DataTable dt = _registrationdalRepository.GetReservationByFolioNumber(folioNumber);
                if (dt.Rows.Count > 0)
                {
                    ReservationKey = dt.Rows[0]["ReservationKey"].ToString();
                    DateTime dtCheckIndate = Convert.ToDateTime(dt.Rows[0]["CheckInDate"]);
                    DateTime dtCheckOutdate = Convert.ToDateTime(dt.Rows[0]["CheckOutDate"]);
                    TimeSpan tsDay = dtCheckOutdate - dtCheckIndate;
                    int intNoOfNight = tsDay.Days;
                    a.BookingNo = folioNumber;
                    a.BookDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["DocDate"]);
                    CheckInDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["CheckInDate"]);
                    CheckOutDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["CheckOutDate"]);
                    a.CheckInDate = CheckInDate;
                    a.CheckOutDate = CheckOutDate;
                    a.RateType = dt.Rows[0]["RateDescription"].ToString();
                    a.Roomtype = dt.Rows[0]["RoomDescription"].ToString();
                    a.Night = tsDay.Days.ToString();


                    #region BindRoomMaxPaxInfo

                    if (dt.Rows.Count > 0)
                    {
                        RoomMaxPaxInfo rmp = new RoomMaxPaxInfo();
                        rmp.RoomDescription = dt.Rows[0]["RoomDescription"].ToString();

                        rmp.RoomTypePaxText = GetRoomTypePaxTextByRoomTypeSeq(dt.Rows[0]["RoomSeq"].ToString());

                        rmp.RoomTypePaxLabel = GetRoomTypePaxLabelByRoomTypeSeq(dt.Rows[0]["RoomSeq"].ToString());

                        int intRoomMaxPax = GetRoomTypePaxByRoomTypeSeq(dt.Rows[0]["RoomSeq"].ToString());
                        RoomMaxPaxCount = intRoomMaxPax;
                        rmp.RoomTypePax = intRoomMaxPax.ToString();
                        a.RoomMaxPaxInfo = rmp;

                    }


                    #endregion

                }
                else
                {
                    //go login page
                }
                #endregion

                #region BindAddEditGuestPanel

                if (blnNew)
                {
                    a.SharedGuestInfoHeaderText = "Add Shared Guest's Information";
                    a.btnAddGuestInfoText = "Add Shared Guest";
                    MainGuestInfo mgi = new MainGuestInfo();
                    mgi.ReservationKey = ReservationKey;
                    mgi.litIsNewText = "1";
                    mgi.CheckInDate = CheckInDate;
                    mgi.CheckOutDate = CheckOutDate;
                    mgi.FirstName = "";
                    mgi.Lastname = "";
                    mgi.Email = "";
                    mgi.NRIC = "";
                    mgi.DOB = null;
                    mgi.Address = "";
                    mgi.Address2 = "";
                    mgi.Postal = "";
                    mgi.Mobile = "";
                    #region BindCityCountry
                    CGuest guest = _registrationdalRepository.GetGuestInfoByGuestKey(GuestKey);
                    if (guest != null)
                    {
                        if (guest.City == null || guest.City.Equals("--") || string.IsNullOrEmpty(guest.City))
                        {
                            mgi.City = "--";
                            mgi.CityKey = Guid.Empty;
                        }
                        else
                        {
                            if (guest.City != null)
                                mgi.City = guest.City;
                            mgi.CityKey = GetCityKey(guest.City);
                        }
                        if (guest.NationalityKey == null || guest.NationalityKey == Guid.Empty.ToString())
                        {
                            mgi.Nationality = "--";
                            mgi.NationalityKey = Guid.Empty;
                        }
                        else
                        {
                            mgi.Nationality = GetNationality(guest.NationalityKey);
                            mgi.NationalityKey = new Guid(guest.NationalityKey);
                        }


                    }
                    #endregion

                    a.MainGuestInfo = mgi;
                }
                else
                {
                    a.SharedGuestInfoHeaderText = "Edit Shared Guest's Information";
                    a.btnAddGuestInfoText = "Update";
                    #region BindEditGuestInfo
                    CGuest guest = _registrationdalRepository.GetGuestInfoByGuestKey(GuestKey);
                    if (guest != null)
                    {


                        if (guest != null)
                        {
                            MainGuestInfo mgi = new MainGuestInfo();
                            mgi.GuestKey = GuestKey;
                            mgi.ReservationKey = ReservationKey;
                            mgi.litIsNewText = "0";
                            mgi.CheckInDate = CheckInDate;
                            mgi.CheckOutDate = CheckOutDate;
                            if (!string.IsNullOrEmpty(guest.Title))
                            {
                                if (guest.Title.Trim().ToString() != "")
                                {
                                    mgi.Title = guest.Title.ToString();
                                }
                            }

                            mgi.FirstName = (!string.IsNullOrEmpty(guest.FirstName)) ? guest.FirstName.Trim() : "";
                            mgi.Lastname = (!string.IsNullOrEmpty(guest.LastName)) ? guest.LastName.Trim() : "";
                            mgi.Email = guest.EMail.Trim();
                            mgi.NRIC = (!string.IsNullOrEmpty(guest.Passport)) ? guest.Passport.Trim() : "";
                            mgi.DOB = (guest.DOB.HasValue ? guest.DOB.Value : (DateTime?)null);
                            if (guest.Address != null)
                            {
                                mgi.Address = (guest.Address.Trim().Length > 60 ? guest.Address.Trim().Substring(0, 60) : guest.Address.Trim());
                                mgi.Address2 = (guest.Address.Trim().Length > 60 ? guest.Address.Trim().Substring(60) : "");
                            }
                            else
                            {
                                mgi.Address = "";
                                mgi.Address2 = "";
                            }
                            mgi.Postal = guest.Postal;
                            mgi.Mobile = guest.Mobile;
                            if (guest.City == null || guest.City.Equals("--") || string.IsNullOrEmpty(guest.City))
                            {
                                mgi.City = "--";
                                mgi.CityKey = Guid.Empty;
                            }
                            else
                            {
                                if (guest.City != null)
                                    mgi.City = guest.City;
                                mgi.CityKey = GetCityKey(guest.City);
                            }
                            if (guest.NationalityKey == null || guest.NationalityKey == Guid.Empty.ToString())
                            {
                                mgi.Nationality = "--";
                                mgi.NationalityKey = Guid.Empty;
                            }
                            else
                            {
                                mgi.Nationality = GetNationality(guest.NationalityKey);
                                mgi.NationalityKey = new Guid(guest.NationalityKey);
                            }
                            a.MainGuestInfo = mgi;
                        }

                    }
                    else
                    {
                        // show error msg
                    }
                    #endregion

                }


                #endregion

                #region shareguestlist


                DataTable dtr = _registrationdalRepository.GetReservationGuestByReservationKey(ReservationKey);
                List<ShareGuestlist> slst = new List<ShareGuestlist>();
                if (dtr.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtr.Rows)
                    {
                        ShareGuestlist o = new ShareGuestlist();
                        o.No = !DBNull.Value.Equals(dr["No"]) ? dr["No"].ToString() : "";
                        o.Name = !DBNull.Value.Equals(dr["Name"]) ? dr["Name"].ToString() : "";
                        o.GuestKey = (!DBNull.Value.Equals(dr["GuestKey"])) ? (!string.IsNullOrEmpty(dr["GuestKey"].ToString()) ? new Guid(dr["GuestKey"].ToString()) : Guid.Empty) : Guid.Empty;

                        slst.Add(o);
                    }
                }
                a.ShareGuestlist = slst;
                if (RoomMaxPaxCount > (dtr.Rows.Count + 1))
                {
                    a.btnAddGuestVisible = true;
                }
                else
                {
                    a.btnAddGuestVisible = false;
                    a.tbGuestInfoVisible = false;
                }


                #endregion

                Alllst.Add(a);
            }
            //else
            //{
            //    throw new UserFriendlyException("Session has expired.");
            //}

            return new ListResultDto<ShareGuestFirstLoadInfo>(Alllst);
        }

        [HttpPost]
        public async Task<string> btnSave(MainGuestInfoViewData a)//SetupViewData input)MainGuestInfoViewData a
        {
            string message = "";
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //if (user.Result != null)
                //{
                try
                {

                    if (a.GuestSign == true)//a.MainGuestSignature.GuestSign
                    {
                        CGuest guest = new CGuest();
                        string strPreCheckInCount = (Convert.ToInt32(a.PreCheckInCount) + 1).ToString();
                        guest.GuestKey = Guid.Parse(a.GuestKey);
                        guest.Title = a.Title;//a.MainGuestInfo.Title;
                        guest.Passport = a.NRIC.Trim();// a.MainGuestInfo.NRIC.Trim();
                        guest.DOB = a.DOB;// a.MainGuestInfo.DOB;
                        guest.Address = CommomData.GetCleanSQLString(a.Address) + (!string.IsNullOrEmpty(CommomData.GetCleanSQLString(a.Address2.Trim())) ? ", " : "") + CommomData.GetCleanSQLString(a.Address2.Trim());
                        guest.Postal = CommomData.GetCleanSQLString(a.Postal);
                        guest.CountryKey = a.NationalityKey.ToString();
                        guest.NationalityKey = a.NationalityKey.ToString();
                        guest.City = a.City;
                        guest.Mobile = CommomData.GetCleanSQLString(a.Mobile);
                        if (AbpSession.TenantId != null)
                        {
                            guest.TenantId = (int)AbpSession.TenantId;
                        }


                        int intSuccess = _registrationdalRepository.UpdateMainGuestInfo(guest);
                        int intSuccessful = _registrationdalRepository.UpdateReservationAndMainGuestInfo(a.ReservationKey, strPreCheckInCount);

                        if (intSuccessful == 1 && intSuccess == 1)
                        {
                            InsertMainGuestHistoryInfo(a.ReservationKey, strPreCheckInCount);
                            //int success= _registrationdalRepository.InsertMainGuestHistoryInfo(a.ReservationKey, strPreCheckInCount);
                            message = "Your check-in information has been updated.";
                        }
                        else
                        {
                            //message = "Update fail";
                            throw new UserFriendlyException("Update fail");
                        }

                    }
                    else
                    {

                        message = "Please sign to check-in online.";
                        //throw new UserFriendlyException("Please sign to check-in online.");
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }

                //else
                //{
                //    throw new UserFriendlyException("Session has expired.");
                //}
            }
            return message;
        }

        [HttpPost]
        public async Task<string> btnChkOut(MainGuestInfoViewData a)//SetupViewData input)MainGuestInfoViewData a
        {
            string message = "";
            List<FirstLoadInfo> Alllst = new List<FirstLoadInfo>();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //if (user.Result != null)
                //{
                try
                {

                    double bill = 0;


                    var sysdate = _registrationdalRepository.GetBusinessDate();
                    var chkOutDate = Convert.ToDateTime(a.CheckOutDate);
                    var lst = _registrationdalRepository.GetChkOutBillingContactBy(a.ReservationKey);
                    for (int i = 0; i < lst.Rows.Count; i++)
                    {
                        bill += Convert.ToDouble(lst.Rows[i]["Balance"].ToString());
                    }
                    if (chkOutDate == sysdate)
                    {
                        if (bill == 0)
                        {

                            CGuest guest = new CGuest();
                            string strGuestKey = a.GuestKey.Trim();
                            guest.GuestKey = Guid.Parse(strGuestKey);
                            guest.Title = a.Title;
                            guest.Passport = a.NRIC.Trim();
                            guest.DOB = a.DOB;
                            guest.Address = CommomData.GetCleanSQLString(a.Address) + (!string.IsNullOrEmpty(CommomData.GetCleanSQLString(a.Address2.Trim())) ? ", " : "") + CommomData.GetCleanSQLString(a.Address2.Trim());
                            guest.Postal = CommomData.GetCleanSQLString(a.Postal);
                            guest.CountryKey = a.NationalityKey.ToString();
                            guest.NationalityKey = a.NationalityKey.ToString();
                            guest.City = a.City;
                            guest.Mobile = CommomData.GetCleanSQLString(a.Mobile);
                            if (AbpSession.TenantId != null)
                            {
                                guest.TenantId = (int)AbpSession.TenantId;
                            }
                            int intSuccess = _registrationdalRepository.UpdateMainGuestInfo(guest);
                            int intSuccessful = _registrationdalRepository.UpdateChkOutReservation(a.ReservationKey);
                            if (intSuccess == 1 && intSuccessful == 1)
                            {

                                InsertChkOutHistoryInfo(a.ReservationKey, a.BookingNo, a.logintime);
                                message = "Check out successful.";
                                // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "open", "var cfm = confirm('Online Check-Out is Successful.Do you wish to logout?'); if (cfm == true) { alert('Logout Successful'); window.location.href = 'Index.aspx'; } ", true);
                            }

                        }
                        else
                        {
                            InsertChkOutHistoryInfo(a.ReservationKey, a.BookingNo, a.logintime);
                            message = "Please proceed to the Front Desk for payment of remaining balances.";
                        }
                    }
                    else
                    {
                        InsertChkOutHistoryInfo(a.ReservationKey, a.BookingNo, a.logintime);
                        message = "Please proceed to Front Desk for early check out.";
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

                //else
                //{
                //    throw new UserFriendlyException("Session has expired.");
                //}
            }
            return message;
        }


        [HttpPost]
        public async Task<string> btnSaveNew(MainGuestInfoViewData a)//SetupViewData input)MainGuestInfoViewData a
        {
            string message = "";
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //if (user.Result != null)
                //{
                try
                {

                    if (a.GuestSign == true)//a.MainGuestSignature.GuestSign
                    {
                        CGuest guest = new CGuest();
                        string strPreCheckInCount = (Convert.ToInt32(a.PreCheckInCount) + 1).ToString();
                        guest.GuestKey = Guid.Parse(a.GuestKey);
                        guest.Title = a.Title;//a.MainGuestInfo.Title;
                        guest.Passport = a.NRIC.Trim();// a.MainGuestInfo.NRIC.Trim();
                        guest.DOB = a.DOB;// a.MainGuestInfo.DOB;
                        guest.Address = CommomData.GetCleanSQLString(a.Address) + (!string.IsNullOrEmpty(CommomData.GetCleanSQLString(a.Address2.Trim())) ? ", " : "") + CommomData.GetCleanSQLString(a.Address2.Trim());
                        guest.Postal = CommomData.GetCleanSQLString(a.Postal);
                        guest.CountryKey = a.NationalityKey.ToString();
                        guest.NationalityKey = a.NationalityKey.ToString();
                        guest.City = a.City;
                        guest.Mobile = CommomData.GetCleanSQLString(a.Mobile);
                        if (AbpSession.TenantId != null)
                        {
                            guest.TenantId = (int)AbpSession.TenantId;
                        }


                        int intSuccess = _registrationdalRepository.UpdateMainGuestInfo(guest);
                        int intSuccessful = _registrationdalRepository.UpdateReservationAndMainGuestInfo(a.ReservationKey, strPreCheckInCount);

                        if (intSuccessful == 1 && intSuccess == 1)
                        {
                            InsertMainGuestHistoryInfo(a.ReservationKey, strPreCheckInCount);
                            #region update doc
                            int Successful = UpdateScreenShoot(user);
                            #endregion
                            if(Successful>0)
                            {
                                //int success= _registrationdalRepository.InsertMainGuestHistoryInfo(a.ReservationKey, strPreCheckInCount);
                                message = "Your check-in information has been updated.";
                            }
                            else
                            {
                                message = "Doc Save Fail";
                            }
                            
                        }
                        else
                        {
                            //message = "Update fail";
                            throw new UserFriendlyException("Update fail");
                        }

                    }
                    else
                    {

                        message = "Please sign to check-in online.";
                        //throw new UserFriendlyException("Please sign to check-in online.");
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }

                //else
                //{
                //    throw new UserFriendlyException("Session has expired.");
                //}
            }
            return message;
        }

        private int UpdateScreenShoot(Authorization.Users.User user)
        {
            int intSuccessful = 0;
            string foliono = user.PIN;
            string strGuestKey = user.StaffKey.ToString();
            string ReservationKey = GetReservationKey(foliono).ToString();
            GuestDetailScreenInfo ga = new GuestDetailScreenInfo();
            ga = GetGuestDetailScreenInfo(foliono, strGuestKey);
            string screenimage = ScreenShootImg(ga);


            byte[] imageBytes = ConvertHtmlToImageBytes(screenimage);

            DocumentSign document = new DocumentSign();
            document.ReservationKey = new Guid(ReservationKey);
            document.DocumentStore = imageBytes;
          
             intSuccessful=_registrationdalRepository.UpdateScreenShootImage(document);
            return intSuccessful;
        }

        [HttpPost]
        public async Task<string> btnChkOutNew(MainGuestInfoViewData a)//SetupViewData input)MainGuestInfoViewData a
        {
            string message = "";
            List<FirstLoadInfo> Alllst = new List<FirstLoadInfo>();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //if (user.Result != null)
                //{
                try
                {

                    double bill = 0;


                    var sysdate = _registrationdalRepository.GetBusinessDate();
                    var chkOutDate = Convert.ToDateTime(a.CheckOutDate);
                    var lst = _registrationdalRepository.GetChkOutBillingContactBy(a.ReservationKey);
                    for (int i = 0; i < lst.Rows.Count; i++)
                    {
                        bill += Convert.ToDouble(lst.Rows[i]["Balance"].ToString());
                    }
                    if (chkOutDate == sysdate)
                    {
                        if (bill == 0)
                        {

                            CGuest guest = new CGuest();
                            string strGuestKey = a.GuestKey.Trim();
                            guest.GuestKey = Guid.Parse(strGuestKey);
                            guest.Title = a.Title;
                            guest.Passport = a.NRIC.Trim();
                            guest.DOB = a.DOB;
                            guest.Address = CommomData.GetCleanSQLString(a.Address) + (!string.IsNullOrEmpty(CommomData.GetCleanSQLString(a.Address2.Trim())) ? ", " : "") + CommomData.GetCleanSQLString(a.Address2.Trim());
                            guest.Postal = CommomData.GetCleanSQLString(a.Postal);
                            guest.CountryKey = a.NationalityKey.ToString();
                            guest.NationalityKey = a.NationalityKey.ToString();
                            guest.City = a.City;
                            guest.Mobile = CommomData.GetCleanSQLString(a.Mobile);
                            if (AbpSession.TenantId != null)
                            {
                                guest.TenantId = (int)AbpSession.TenantId;
                            }
                            int intSuccess = _registrationdalRepository.UpdateMainGuestInfo(guest);
                            int intSuccessful = _registrationdalRepository.UpdateChkOutReservation(a.ReservationKey);
                            if (intSuccess == 1 && intSuccessful == 1)
                            {

                                InsertChkOutHistoryInfo(a.ReservationKey, a.BookingNo, a.logintime);
                                #region update doc
                                int Successful = UpdateScreenShoot(user);
                                #endregion
                                if(Successful>0)
                                {
                                    message = "Check out successful.";
                                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "open", "var cfm = confirm('Online Check-Out is Successful.Do you wish to logout?'); if (cfm == true) { alert('Logout Successful'); window.location.href = 'Index.aspx'; } ", true);
                                }
                                else
                                {
                                    message = "Doc Save Fail";
                                }

                            }

                        }
                        else
                        {
                            InsertChkOutHistoryInfo(a.ReservationKey, a.BookingNo, a.logintime);
                            message = "Please proceed to the Front Desk for payment of remaining balances.";
                        }
                    }
                    else
                    {
                        InsertChkOutHistoryInfo(a.ReservationKey, a.BookingNo, a.logintime);
                        message = "Please proceed to Front Desk for early check out.";
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

                //else
                //{
                //    throw new UserFriendlyException("Session has expired.");
                //}
            }
            return message;
        }
        [HttpPost]
        public async Task<string> AddSharedGuestOrUpdate(MainGuestInfoViewData a)//SetupViewData input)MainGuestInfoViewData a
        {
            string message = "";
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //if (user.Result != null)
                //{
                try
                {
                    string strMessage = "";

                    CGuest guest = new CGuest();

                    bool blnIsNew = (a.litIsNewText.Equals("1"));

                    guest.ReservationKey = new Guid(a.ReservationKey);
                    guest.Title = a.Title;
                    guest.FirstName = a.FirstName.Trim();
                    guest.LastName = a.Lastname.Trim();
                    guest.EMail = a.Email.Trim();
                    guest.Passport = a.NRIC.Trim();
                    guest.DOB = a.DOB;
                    guest.Address = CommomData.GetCleanSQLString(a.Address) + (!string.IsNullOrEmpty(CommomData.GetCleanSQLString(a.Address2.Trim())) ? ", " : "") + CommomData.GetCleanSQLString(a.Address2.Trim());
                    guest.Postal = CommomData.GetCleanSQLString(a.Postal);
                    guest.CountryKey = a.NationalityKey.ToString();
                    guest.NationalityKey = a.NationalityKey.ToString();
                    guest.City = a.City.ToString();
                    guest.Mobile = CommomData.GetCleanSQLString(a.Mobile);
                    guest.CheckInDate = Convert.ToDateTime(a.CheckInDate);
                    guest.CheckOutDate = Convert.ToDateTime(a.CheckOutDate);
                    if (AbpSession.TenantId != null)
                    {
                        guest.TenantId = (int)AbpSession.TenantId;
                    }
                    int intSuccessful = 0;

                    if (blnIsNew)
                    {
                        guest.GuestKey = Guid.NewGuid();
                        intSuccessful = _registrationdalRepository.AddReservationGuest(guest);
                        InsertSharedGuestHistoryInfo(a.ReservationKey, guest, false, true);
                    }
                    else
                    {
                        guest.GuestKey = new Guid(a.GuestKey);
                        intSuccessful = _registrationdalRepository.UpdateReservationGuest(guest);
                        InsertSharedGuestHistoryInfo(a.ReservationKey, guest, false, false);
                    }


                    if (intSuccessful == 1)
                    {
                        //DisplayAlertMsg("Shared Guest's info. has been saved.");
                        message = "Shared Guest's info. has been saved.";
                    }
                    else
                    {
                        //DisplayAlertMsg("Fail to save the record.");
                        throw new UserFriendlyException("Fail to save the record.");
                    }


                }
                catch (Exception ex)
                {

                    throw new UserFriendlyException(ex.Message.ToString());
                }


                //else
                //{
                //    throw new UserFriendlyException("Session has expired.");
                //}
            }
            return message;
        }

        [HttpPost]
        public async Task<string> btnSignUpSave(MainGuestInfoViewData a)
        {
            string message = "";

            CGuest guest = new CGuest();
            guest.GuestKey = Guid.NewGuid();
            guest.Title = a.Title;
            guest.FirstName = a.FirstName.Trim();
            guest.LastName = a.Lastname.Trim();
            guest.EMail = a.Email.Trim();
            guest.DOB = a.DOB;
            guest.Mobile = CommomData.GetCleanSQLString(a.Mobile);
            guest.Address = CommomData.GetCleanSQLString(a.Address);
            guest.Postal = CommomData.GetCleanSQLString(a.Postal);



            int intSuccessful = _registrationdalRepository.AddGuest(guest);
            if (intSuccessful > 0)
            {
                //Response.Redirect("Index.aspx");
                message = "Success";
            }
            else
            {
                throw new UserFriendlyException("Fail");
            }

            return message;
        }

        [HttpPost]
        public async Task<string> RemoveGuest(string GuestKey)
        {
            string message = "";
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {

                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //if (user.Result != null)
                //{
                string foliono = user.PIN;
                CGuest guest = new CGuest();
                guest.GuestKey = new Guid(GuestKey);
                guest.ReservationKey = GetReservationKey(foliono);

                int intSuccessful = _registrationdalRepository.RemoveReservationGuest(guest);
                if (intSuccessful == 1)
                {
                    InsertRemoveSharedGuestHistoryInfo(guest.ReservationKey.ToString(), GuestKey);
                    message = "Shared Guest has been removed.";
                }
                else
                {
                    throw new UserFriendlyException("Fail to update the record.");
                }

            }
            //else
            //{
            //    throw new UserFriendlyException("Session has expired.");
            //}

            return message;
        }

        [HttpPost]
        public async Task<string> btnSaveDoc(DocSignInput sig)
        {
            string message = "";
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //if (user.Result != null)
                //{
                try
                {
                    string folioNumber = user.PIN;
                    string strReservationKey = GetReservationKey(folioNumber).ToString();
                    string strGuestKey = user.StaffKey.ToString();
                    if (sig.imgSign != null)
                    {
                        DocumentSign docExisting = _registrationdalRepository.BindMainGuestSignature(strReservationKey, strGuestKey);
                        //var sigToImg = new SignatureToImage();
                        //var signatureImage = sigToImg.SigJsonToImage(sig.Sig);
                        //byte[] imgSign = ConvertImageToByte(signatureImage);
                        DocumentSign document = new DocumentSign();
                        document.DocumentKey = Guid.NewGuid();
                        document.Description = "Guest Signature";
                        document.DocumentName = "Guest Signature";
                        document.DocumentStore = sig.imgSign;
                        document.GuestKey = new Guid(strGuestKey);
                        document.ReservationKey = new Guid(strReservationKey);
                        document.Signature = sig.imgSign;
                        int intSuccessful = 0;

                        if (!string.IsNullOrEmpty(docExisting.DocumentName))
                        {
                            intSuccessful = _registrationdalRepository.UpdateGuestDocument(document);
                            InsertGuestSignatureLog(document, false);
                            message = "success";
                        }
                        else
                        {
                            intSuccessful = _registrationdalRepository.InsertGuestDocument(document);
                            InsertGuestSignatureLog(document, true);
                            message = "success";
                        }
                    }


                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }

                //else
                //{
                //    throw new UserFriendlyException("Session has expired.");
                //}
            }
            return message;
        }

        [HttpGet]
        public async Task<ListResultDto<Documentlist>> GetDocumentData()
        {
            List<Documentlist> Alllst = new List<Documentlist>();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //    if (user.Result != null)
                //    {
                string folioNumber = user.PIN;
                string ReservationKey = GetReservationKey(folioNumber).ToString();
                string strGuestKey = user.StaffKey.ToString();
                Documentlist a = new Documentlist();

                #region BindMainGuestSignature


                DocumentSign doc = _registrationdalRepository.BindMainGuestSignature(ReservationKey, strGuestKey);
                DocSignOutput d = new DocSignOutput();
                d.GuestSign = Convert.ToBase64String(doc.Signature);
                a.DocSignOutput = d;





                #endregion



                Alllst.Add(a);
            }

            return new ListResultDto<Documentlist>(Alllst);
        }
      

        [HttpGet]
        public async Task<ListResultDto<Documentlist>> GetScreenShootImageData()
        {
            List<Documentlist> Alllst = new List<Documentlist>();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //    if (user.Result != null)
                //    {
                string folioNumber = user.PIN;
                string ReservationKey = GetReservationKey(folioNumber).ToString();
                string strGuestKey = user.StaffKey.ToString();
                Documentlist a = new Documentlist();

                #region BindMainGuestSignature


                DocumentSign doc = _registrationdalRepository.BindMainGuestSignature(ReservationKey, strGuestKey);
                DocSignOutput d = new DocSignOutput();
                d.GuestSign = Convert.ToBase64String(doc.DocumentStore);
                a.DocSignOutput = d;





                #endregion



                Alllst.Add(a);
            }

            return new ListResultDto<Documentlist>(Alllst);
        }

        [HttpGet]
        public async Task<ListResultDto<FirstLoadScanInfo>> GetRegistrationScanViewData()
        {
            List<FirstLoadScanInfo> Alllst = new List<FirstLoadScanInfo>();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //    if (user.Result != null)
                //    {
                string folioNumber = user.PIN;
                string ReservationKey = "";
                string strGuestKey = user.StaffKey.ToString();
                FirstLoadScanInfo a = new FirstLoadScanInfo();

                #region ReservationDropdown
                RegistrationViewData ri = new RegistrationViewData();
                ri.Title = GetAllTitle();
                ri.City = GetAllCity();
                ri.Country = GetAllCountry();
                ri.Purpose = GetAllPurpose();
                a.ReservationDropdown = ri;
                #endregion

                DataTable dt = _registrationdalRepository.GetReservationByFolioNumber(folioNumber);
                if (dt.Rows.Count > 0)
                {
                    DateTime dtCheckIndate = Convert.ToDateTime(dt.Rows[0]["CheckInDate"]);
                    DateTime dtCheckOutdate = Convert.ToDateTime(dt.Rows[0]["CheckOutDate"]);
                    TimeSpan tsDay = dtCheckOutdate - dtCheckIndate;
                    int intNoOfNight = tsDay.Days;
                    if (dt.Rows[0]["Status"].ToString() == "2")
                    {
                        a.btnSaveShow = false;
                        a.btnChkOutShow = true;
                    }
                    else if (dt.Rows[0]["Status"].ToString() == "1")
                    {
                        a.btnSaveShow = true;
                        a.btnChkOutShow = false;
                    }
                    else
                    {
                        a.btnSaveShow = false;
                        a.btnChkOutShow = false;
                    }
                    ReservationKey = dt.Rows[0]["ReservationKey"].ToString();
                    a.ReservationKey = ReservationKey;
                    a.GuestKey = dt.Rows[0]["GuestKey"].ToString();
                    a.BookingNo = folioNumber;
                    a.BookDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["DocDate"]);
                    a.CheckInDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["CheckInDate"]);
                    a.CheckOutDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["CheckOutDate"]);
                    a.RateType = dt.Rows[0]["RateDescription"].ToString();
                    a.Roomtype = dt.Rows[0]["RoomDescription"].ToString();
                    a.Night = tsDay.Days.ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[0]["PurposeStayKey"].ToString()))
                        a.PurposeStayKey = new Guid(dt.Rows[0]["PurposeStayKey"].ToString());

                    a.Request = dt.Rows[0]["Remark"].ToString();

                    int intPreCheckInCount = 0;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["PreCheckInCount"].ToString()))
                        intPreCheckInCount = Convert.ToInt32(dt.Rows[0]["PreCheckInCount"]);

                    if (intPreCheckInCount > 0)
                    {
                        a.PreCheckInMessage = "Thank you, Guest! <br/> Your online Check-In process is completed.";
                        a.PreCheckInCount = intPreCheckInCount.ToString();
                    }


                    #region BindMainGuestSignature


                    DocumentSign doc = _registrationdalRepository.BindMainGuestSignature(ReservationKey, strGuestKey);
                    MainGuestSignature mgs = new MainGuestSignature();
                    if (doc.Signature != null)
                    {
                        var varGuestSign = Convert.ToBase64String(doc.Signature);
                        mgs.imgGuestSign = string.Format("data:image/png;base64,{0}", varGuestSign);
                        mgs.GuestSign = true;
                    }
                    else
                    {
                        mgs.GuestSign = false;
                    }
                    a.MainGuestSignature = mgs;


                    #endregion

                    #region BindRoomMaxPaxInfo

                    if (dt.Rows.Count > 0)
                    {
                        RoomMaxPaxInfo rmp = new RoomMaxPaxInfo();
                        rmp.RoomDescription = dt.Rows[0]["RoomDescription"].ToString();

                        rmp.RoomTypePaxText = GetRoomTypePaxTextByRoomTypeSeq(dt.Rows[0]["RoomSeq"].ToString());

                        rmp.RoomTypePaxLabel = GetRoomTypePaxLabelByRoomTypeSeq(dt.Rows[0]["RoomSeq"].ToString());

                        int intRoomMaxPax = GetRoomTypePaxByRoomTypeSeq(dt.Rows[0]["RoomSeq"].ToString());

                        rmp.RoomTypePax = intRoomMaxPax.ToString();
                        a.RoomMaxPaxInfo = rmp;

                    }


                    #endregion
                    #region BindMainGuestInfo

                    CGuest guest = _registrationdalRepository.GetGuestInfoByGuestKey(dt.Rows[0]["GuestKey"].ToString());
                    if (guest != null)
                    {
                        MainGuestInfo mgi = new MainGuestInfo();
                        mgi.GuestKey = dt.Rows[0]["GuestKey"].ToString();
                        mgi.ReservationKey = ReservationKey;
                        mgi.Title = guest.Title.ToString();
                        mgi.CheckInDate = a.CheckInDate;
                        mgi.CheckOutDate = a.CheckOutDate;
                        mgi.FirstName = (!string.IsNullOrEmpty(guest.FirstName)) ? guest.FirstName.Trim() : "";
                        mgi.Lastname = (!string.IsNullOrEmpty(guest.LastName)) ? guest.LastName.Trim() : "";
                        mgi.Email = guest.EMail.Trim();
                        mgi.NRIC = (!string.IsNullOrEmpty(guest.Passport)) ? guest.Passport.Trim() : "";
                        mgi.DOB = (guest.DOB.HasValue ? guest.DOB.Value : (DateTime?)null);
                        if (guest.Address != null)
                        {
                            mgi.Address = (guest.Address.Trim().Length > 60 ? guest.Address.Trim().Substring(0, 60) : guest.Address.Trim());
                            mgi.Address2 = (guest.Address.Trim().Length > 60 ? guest.Address.Trim().Substring(60) : "");
                        }
                        else
                        {
                            mgi.Address = "";
                            mgi.Address2 = "";
                        }
                        mgi.Postal = guest.Postal;
                        mgi.Mobile = guest.Mobile;
                        if (guest.City == null || guest.City.Equals("--") || string.IsNullOrEmpty(guest.City))
                        {
                            mgi.City = "--";
                            mgi.CityKey = Guid.Empty;
                        }
                        else
                        {
                            if (guest.City != null)
                                mgi.City = guest.City;
                            mgi.CityKey = GetCityKey(guest.City);
                        }
                        if (guest.CountryKey == null || guest.CountryKey == Guid.Empty.ToString())
                        {
                            mgi.Nationality = "--";
                            mgi.NationalityKey = Guid.Empty;
                        }
                        else
                        {
                            mgi.Nationality = GetNationality(guest.CountryKey);
                            mgi.NationalityKey = new Guid(guest.CountryKey);
                        }
                        a.MainGuestInfo = mgi;
                    }
                    else
                    {
                        // show error msg
                    }

                    #endregion

                    #region grdSharedGuest
                    DataTable dtr = _registrationdalRepository.GetReservationGuestByReservationKey(ReservationKey);
                    List<ShareGuestlist> slst = new List<ShareGuestlist>();
                    if (dtr.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtr.Rows)
                        {
                            ShareGuestlist o = new ShareGuestlist();
                            o.No = !DBNull.Value.Equals(dr["No"]) ? dr["No"].ToString() : "";
                            o.Name = !DBNull.Value.Equals(dr["Name"]) ? dr["Name"].ToString() : "";
                            o.GuestKey = (!DBNull.Value.Equals(dr["GuestKey"])) ? (!string.IsNullOrEmpty(dr["GuestKey"].ToString()) ? new Guid(dr["GuestKey"].ToString()) : Guid.Empty) : Guid.Empty;

                            slst.Add(o);
                        }
                    }
                    a.ShareGuestlist = slst;
                    #endregion

                }
                else
                {
                    //go login page
                }

                Alllst.Add(a);
            }
            //else
            //{
            //    throw new UserFriendlyException("Session has expired.");
            //}

            return new ListResultDto<FirstLoadScanInfo>(Alllst);
        }

        [HttpGet]
        public async Task<ListResultDto<RegistrationDetailData>> GetRegistrationDetailData()
        {
            List<RegistrationDetailData> Alllst = new List<RegistrationDetailData>();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //    if (user.Result != null)
                //    {
                string folioNumber = user.PIN;
                string _ReservationKey = GetReservationKey(folioNumber).ToString();
                RegistrationDetailData a = new RegistrationDetailData();
                #region GenerateReservationInfo

                ReservationDetailOutput res = _registrationdalRepository.GetReservationByReservationKey(_ReservationKey);

                a.txtReservationNo = res.DocNo;
                a.txtResDate = res.DocDate.HasValue ? res.DocDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt").ToUpper() : "";
                a.txtResStatus = res.StatusString;

                a.radChkInDate = res.CheckInDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt").ToUpper();
                a.radChkOutDate = res.CheckOutDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt").ToUpper();

                a.RadNumericTxtNight = GetDateDifference(res.CheckInDate, res.CheckOutDate).ToString();

                a.txtRateType = res.RateCode;
                #endregion
                #region
                string PMTDecrypt = "";
                a.ReservationRateOutputlst = _registrationdalRepository.GetTransactionByReservationKey(_ReservationKey, PMTDecrypt);
                #endregion

                Alllst.Add(a);
            }
            //else
            //{
            //    throw new UserFriendlyException("Session has expired.");
            //}

            return new ListResultDto<RegistrationDetailData>(Alllst);
        }
        
        [HttpPost]
        public async Task<string> btnEmailSend()
        {
            string message = "";
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //if (user.Result != null)
                //{
                string foliono = user.PIN;
                string ReservationKey = GetReservationKey(foliono).ToString();
                string Email = user.EmailAddress;
                string LastName = user.Surname;
                string FirstName = user.Name;
                string Title = "";
                string mailfrom = _configurationAccessor.Configuration["BrillantezEmail"];
                string ccmail = _configurationAccessor.Configuration["CCMail"];
                string mailPassword = _configurationAccessor.Configuration["BrillantezPassword"];//"3TjUJu7wYEakKjQ";//"Net#75Server";//"3TjUJu7wYEakKjQ";
                //string scanurl = _configurationAccessor.Configuration["BarCodeURL"];//"http://localhost:62319/RegistrationScan.aspx?Email=";// GetSiteRoot() + "/SendMailDesign.aspx";//http://localhost:62319/SendMailDesign.aspx";//"http://localhost:62319/Test.aspx"//"http://localhost:62319/RegistrationScan.aspx?Email=";
                PdfScanInfo a = new PdfScanInfo();
                a = GetPdfScanInfo(foliono);
                string pdfgape = HPageForP(a);
                string HotelName = _configurationAccessor.Configuration["HotelName"];
                EmailList el = GetFormData(Email, foliono, ReservationKey, Title, FirstName, LastName, HotelName);
                EmailHistory eh = new EmailHistory();
                eh.SentDateTime = DateTime.Now;
                eh.ReservationKey = new Guid(ReservationKey);
                eh.Sort = 0;
                eh.From = mailfrom;
                eh.To = Email;
                eh.Subject = "Guest Information";
                eh.Content = EmailForm(Title, FirstName, LastName, HotelName);
                eh.Sent = 0;
                int success = _registrationdalRepository.InsertHistory(eh);
                #region create filename
                string fileName = ToSafeFileName(foliono);
                String pathName = _env.ContentRootPath;//AppDomain.CurrentDomain.BaseDirectory;//HostingEnvironment.MapPath("~/PDFs/")//HttpContext.Current.Server.MapPath("~/PDFs/");
                //if (!System.IO.Directory.Exists(pathName))
                //{
                //    Directory.CreateDirectory(pathName);
                //}
                string fileDirectory = "PdfFiles"; // the name of the file directory relative to the application base directory

                string applicationPath = Path.Combine(pathName, fileDirectory); /*AppDomain.CurrentDomain.BaseDirectory
*/
                // Create the directory if it doesn't already exist
                if (!Directory.Exists(applicationPath))
                {
                    Directory.CreateDirectory(applicationPath);
                }
                string file = CreateFileWithUniqueName(applicationPath, fileName);
                string filepath2 = Path.Combine(applicationPath, file + ".pdf");
                HtmlToPdf Renderer = new HtmlToPdf();

                // Convert HTML string to PDF byte array
                byte[] PDF = Renderer.RenderHtmlAsPdf(pdfgape).BinaryData;

                // Save the PDF byte array to file
                File.WriteAllBytes(filepath2, PDF);
                // IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
                //var pdf = Renderer.RenderUrlAsPdf(scanurl + Email + "&BookingNo=" + foliono);
                // pdf.SaveAs(filepath2);
                #endregion
                #region gmail
                //MailMessage mail = new MailMessage();
                //        SmtpClient smtpServer = new SmtpClient(Commondata.mailserver);
                //        mail.From = new MailAddress(Commondata.mailfrom, "Guest Information");
                //        mail.To.Add(hdEmail.Value);
                //        mail.Subject = "Guest Information";
                //        mail.Body = EmailForm();
                //        mail.CC.Add(Commondata.ccmail);
                //        mail.Headers.Add("Sender", Commondata.mailfrom);
                //        System.Net.Mail.Attachment attachment;
                //        attachment = new System.Net.Mail.Attachment(filepath2);
                //        mail.Attachments.Add(attachment);
                //        smtpServer.Port = Commondata.mailPort;
                //        smtpServer.Credentials = new System.Net.NetworkCredential(Commondata.mailfrom, Commondata.mailPassword);
                //        smtpServer.EnableSsl = true;
                //        smtpServer.Send(mail);
                #endregion
                #region office365
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient(CommomData.mailserver, CommomData.mailPort);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(mailfrom, mailPassword);
                MailAddress fromEmail = new MailAddress(mailfrom);

                //mailMessage = new MailMessage(fromEmail, new MailAddress(memberEmail, memberName));
                //mailMessage = new MailMessage();
                mailMessage.From = fromEmail;
                mailMessage.To.Add(Email);
                mailMessage.Body = EmailForm(Title, FirstName, LastName, HotelName);
                mailMessage.Subject = "Guest Information";
                mailMessage.IsBodyHtml = true;
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(filepath2);
                mailMessage.Attachments.Add(attachment);
                //  smtpClient.Send(mailMessage);
                #endregion

                int rowAffected = _registrationdalRepository.InsertGuestEmailList(el);
                int successfull = _registrationdalRepository.UpdateEmailHistory(eh);
                if (rowAffected > 0 && successfull > 0)
                {
                    message = "success";
                }
                else
                {
                    message = "fail";
                }

            }
            return message;
        }

        [HttpPost]
        public async Task<string> btnEmailSendNew()
        {
            string message = "";
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //if (user.Result != null)
                //{
                string foliono = user.PIN;
                string strGuestKey = user.StaffKey.ToString();
                string ReservationKey = GetReservationKey(foliono).ToString();
                string Email = user.EmailAddress;
                string LastName = user.Surname;
                string FirstName = user.Name;
                string Title = "";
                string mailfrom = _configurationAccessor.Configuration["BrillantezEmail"];
                string ccmail = _configurationAccessor.Configuration["CCMail"];
                string mailPassword = _configurationAccessor.Configuration["BrillantezPassword"];//"3TjUJu7wYEakKjQ";//"Net#75Server";//"3TjUJu7wYEakKjQ";
                                                                                                 //string scanurl = _configurationAccessor.Configuration["BarCodeURL"];//"http://localhost:62319/RegistrationScan.aspx?Email=";// GetSiteRoot() + "/SendMailDesign.aspx";//http://localhost:62319/SendMailDesign.aspx";//"http://localhost:62319/Test.aspx"//"http://localhost:62319/RegistrationScan.aspx?Email=";
                                                                                                 //PdfScanInfo a = new PdfScanInfo();
                                                                                                 //a = GetPdfScanInfo(foliono);
                                                                                                 // string pdfgape = HPageForP(a);
                GuestDetailScreenInfo ga = new GuestDetailScreenInfo();
                ga = GetGuestDetailScreenInfo(foliono, strGuestKey);
                string screenimage = ScreenShootImg(ga);
                byte[] imageBytes = ConvertHtmlToImageBytes(screenimage);
                DocumentSign document = new DocumentSign();
                document.ReservationKey = new Guid(ReservationKey);
                document.DocumentStore = imageBytes;
                int intSuccessful = 0;
                intSuccessful = _registrationdalRepository.UpdateScreenShootImage(document);
                string HotelName = _configurationAccessor.Configuration["HotelName"];
                EmailList el = GetFormData(Email, foliono, ReservationKey, Title, FirstName, LastName, HotelName);
                EmailHistory eh = new EmailHistory();
                eh.SentDateTime = DateTime.Now;
                eh.ReservationKey = new Guid(ReservationKey);
                eh.Sort = 0;
                eh.From = mailfrom;
                eh.To = Email;
                eh.Subject = "Guest Information";
                eh.Content = EmailForm(Title, FirstName, LastName, HotelName);
                eh.Sent = 0;
                int success = _registrationdalRepository.InsertHistory(eh);
                // #region create filename
                // string fileName = ToSafeFileName(foliono);
                // String pathName = _env.ContentRootPath;//AppDomain.CurrentDomain.BaseDirectory;//HostingEnvironment.MapPath("~/PDFs/")//HttpContext.Current.Server.MapPath("~/PDFs/");
                // //if (!System.IO.Directory.Exists(pathName))
                // //{
                // //    Directory.CreateDirectory(pathName);
                // //}
                // string fileDirectory = "PdfFiles"; // the name of the file directory relative to the application base directory

                // string applicationPath = Path.Combine(pathName, fileDirectory); /*AppDomain.CurrentDomain.BaseDirectory*/

                // // Create the directory if it doesn't already exist
                // if (!Directory.Exists(applicationPath))
                // {
                //     Directory.CreateDirectory(applicationPath);
                // }
                // string file = CreateFileWithUniqueName(applicationPath, fileName);
                // string filepath2 = Path.Combine(applicationPath, file + ".pdf");
                // HtmlToPdf Renderer = new HtmlToPdf();

                // // Convert HTML string to PDF byte array
                //// byte[] PDF = Renderer.RenderHtmlAsPdf(pdfgape).BinaryData;
                // byte[] PDF = Renderer.RenderHtmlAsPdf(screenimage).BinaryData;
                // // Save the PDF byte array to file
                // System.IO.File.WriteAllBytes(filepath2, PDF);
                // // IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
                // //var pdf = Renderer.RenderUrlAsPdf(scanurl + Email + "&BookingNo=" + foliono);
                // // pdf.SaveAs(filepath2);
                // #endregion
                #region gmail
                //MailMessage mail = new MailMessage();
                //        SmtpClient smtpServer = new SmtpClient(Commondata.mailserver);
                //        mail.From = new MailAddress(Commondata.mailfrom, "Guest Information");
                //        mail.To.Add(hdEmail.Value);
                //        mail.Subject = "Guest Information";
                //        mail.Body = EmailForm();
                //        mail.CC.Add(Commondata.ccmail);
                //        mail.Headers.Add("Sender", Commondata.mailfrom);
                //        System.Net.Mail.Attachment attachment;
                //        attachment = new System.Net.Mail.Attachment(filepath2);
                //        mail.Attachments.Add(attachment);
                //        smtpServer.Port = Commondata.mailPort;
                //        smtpServer.Credentials = new System.Net.NetworkCredential(Commondata.mailfrom, Commondata.mailPassword);
                //        smtpServer.EnableSsl = true;
                //        smtpServer.Send(mail);
                #endregion
                //#region office365
                //MailMessage mailMessage = new MailMessage();
                //SmtpClient smtpClient = new SmtpClient(CommomData.mailserver, CommomData.mailPort);
                //smtpClient.EnableSsl = true;
                //smtpClient.Credentials = new NetworkCredential(mailfrom, mailPassword);
                //MailAddress fromEmail = new MailAddress(mailfrom);

                ////mailMessage = new MailMessage(fromEmail, new MailAddress(memberEmail, memberName));
                ////mailMessage = new MailMessage();
                //mailMessage.From = fromEmail;
                //mailMessage.To.Add(Email);
                //mailMessage.Body = EmailForm(Title, FirstName, LastName, HotelName);
                //mailMessage.Subject = "Guest Information";
                //mailMessage.IsBodyHtml = true;
                //System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(filepath2);
                //mailMessage.Attachments.Add(attachment);
                ////  smtpClient.Send(mailMessage);
                //#endregion

                int rowAffected = _registrationdalRepository.InsertGuestEmailList(el);
                int successfull = _registrationdalRepository.UpdateEmailHistory(eh);
                if (rowAffected > 0 && successfull > 0)
                {
                    message = "success";
                }
                else
                {
                    message = "fail";
                }

            }
            return message;
        }
        [HttpPost]
        public async Task<string> btnSaveScreenShoot(DocSignInput sig)
        {
            string message = "";
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //if (user.Result != null)
                //{
                try
                {
                    string folioNumber = user.PIN;
                    string strReservationKey = GetReservationKey(folioNumber).ToString();
                    string strGuestKey = user.StaffKey.ToString();
                    if (sig.imgSign != null)
                    {
                        DocumentSign document = new DocumentSign();
                        document.ReservationKey = new Guid(strReservationKey);
                        document.DocumentStore = sig.imgSign;
                        int intSuccessful = 0;
                        intSuccessful = _registrationdalRepository.UpdateScreenShootImage(document);
                        if (intSuccessful > 0)
                        {
                            message = "success";
                        }
                    }


                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }

                //else
                //{
                //    throw new UserFriendlyException("Session has expired.");
                //}
            }
            return message;
        }
      
        private byte[] ConvertHtmlToImageBytes(string html)
        {
            var renderer = new HtmlToPdf();
            var pdf = renderer.RenderHtmlAsPdf(html);

            return pdf.BinaryData;
        }

       
        [HttpPost]
        public async Task<string> ConvertImageBytetoString(DocSignInput sig)
        {
            return Convert.ToBase64String(sig.imgSign);
            
        }
       
        [HttpPost]
        public async Task<string> btnEmailSendTestQR()
        {
            string message = "";
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                //if (user.Result != null)
                //{
                string foliono = user.PIN;
                string ReservationKey = GetReservationKey(foliono).ToString();
                string Email = user.EmailAddress;
                string LastName = user.Surname;
                string FirstName = user.Name;
                string Title = "";
                string mailfrom = _configurationAccessor.Configuration["BrillantezEmail"];
                string ccmail = _configurationAccessor.Configuration["CCMail"];
                string mailPassword = _configurationAccessor.Configuration["BrillantezPassword"];//"3TjUJu7wYEakKjQ";//"Net#75Server";//"3TjUJu7wYEakKjQ";
                //string scanurl = _configurationAccessor.Configuration["BarCodeURL"];//"http://localhost:62319/RegistrationScan.aspx?Email=";// GetSiteRoot() + "/SendMailDesign.aspx";//http://localhost:62319/SendMailDesign.aspx";//"http://localhost:62319/Test.aspx"//"http://localhost:62319/RegistrationScan.aspx?Email=";
                PdfScanInfo a = new PdfScanInfo();
                a = GetPdfScanInfo(foliono);
                string pdfgape = HPageForP(a);
                string t = pdfgape;
            }
            return message;
        }
        [HttpGet]
        public async Task<ListResultDto<RequestGuestOutput>> GetGuestRequestDataBindGrid(DateTime? requestDate = null, DateTime? toDate = null, int cancelReq = -1, int openReq = 1, int inPReq = 2, int completeReq = 10)
        {
            List<RequestGuestOutput> Alllst = new List<RequestGuestOutput>();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                DateTime defrequestDate = DateTime.Now.AddDays(-7);
                DateTime deftoDate = DateTime.Now;
                if (requestDate != null)
                {
                    defrequestDate = requestDate.Value;
                }
                if (toDate != null)
                {
                    deftoDate = toDate.Value;
                }

                DataTable dtr = _registrationdalRepository.GetReservationRequestByGuestKey("", cancelReq, openReq, inPReq, completeReq, defrequestDate, deftoDate);
                if (dtr.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtr.Rows)
                    {
                        DateTime? nullable;
                        RequestGuestOutput o = new RequestGuestOutput();
                        o.ReservationRequestKey = (!DBNull.Value.Equals(dr["ReservationRequestKey"])) ? (!string.IsNullOrEmpty(dr["ReservationRequestKey"].ToString()) ? new Guid(dr["ReservationRequestKey"].ToString()) : Guid.Empty) : Guid.Empty;
                        o.ReservationRequestID = !DBNull.Value.Equals(dr["ReservationRequestID"]) ? dr["ReservationRequestID"].ToString() : "";
                        o.Name = !DBNull.Value.Equals(dr["Name"]) ? dr["Name"].ToString() : "";
                        o.DocNo = !DBNull.Value.Equals(dr["DocNo"]) ? dr["DocNo"].ToString() : "";
                        o.StatusDesc = !DBNull.Value.Equals(dr["StatusDesc"]) ? dr["StatusDesc"].ToString() : "";
                        o.RequestTypeName = !DBNull.Value.Equals(dr["RequestTypeName"]) ? dr["RequestTypeName"].ToString() : "";
                        o.Unit = !DBNull.Value.Equals(dr["Unit"]) ? dr["Unit"].ToString() : "";
                        o.GuestRequest = !DBNull.Value.Equals(dr["GuestRequest"]) ? dr["GuestRequest"].ToString() : "";
                        o.RequestDate = (DateTime)(Convert.IsDBNull(dr["RequestDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["RequestDate"])).Value);
                        o.RequestDatedes = o.RequestDate.Value.ToString("dd/MM/yyyy h.mmtt");
                        o.HotelResponse = !DBNull.Value.Equals(dr["HotelResponse"]) ? dr["HotelResponse"].ToString() : "";
                        o.ResponseDate = !DBNull.Value.Equals(dr["ResponseDate"]) ? (DateTime)(Convert.IsDBNull(dr["ResponseDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["ResponseDate"])).Value) : null;
                        o.ResponseDatedes = o.ResponseDate == null ? "" : o.ResponseDate.Value.ToString("dd/MM/yyyy h.mmtt");

                        Alllst.Add(o);
                    }
                }

            }


            return new ListResultDto<RequestGuestOutput>(Alllst);
        }

        [HttpGet]
        public async Task<ListResultDto<RequestGuestDataEntryOutput>> GetRequestGuestDataEntry(string ReservationRequestKey = null)
        {
            List<RequestGuestDataEntryOutput> Alllst = new List<RequestGuestDataEntryOutput>();
            RequestGuestDataEntryOutput a = new RequestGuestDataEntryOutput();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                RequestGuestDataEntryViewData b = new RequestGuestDataEntryViewData();
                b.ddlRequestType = GetAllRequestType();
                a.RequestGuestDataEntryDropdown = b;
                if (!string.IsNullOrEmpty(ReservationRequestKey))
                {
                    ReservationRequestOutput op = new ReservationRequestOutput();
                    op = _registrationdalRepository.GetReservationRequestNew(ReservationRequestKey);
                    op.FolioNo = user.PIN;
                    op.GuestName = GetGuestName(user.StaffKey.ToString());
                    a.ReservationRequestOutput = op;
                }
                else
                {
                    ReservationRequestOutput op = new ReservationRequestOutput();
                    op.FolioNo = user.PIN;
                    op.GuestName = GetGuestName(user.StaffKey.ToString());
                    op.Status = 1;
                    op.StatusName = "Open request";
                    a.ReservationRequestOutput = op;
                }
                Alllst.Add(a);
            }


            return new ListResultDto<RequestGuestDataEntryOutput>(Alllst);
        }
        protected List<RequestTypeOutput> GetAllRequestType()
        {

            return _registrationdalRepository.GetAllRequestType();
        }
        [HttpPost]
        public async Task<string> AddRequestGuestOrUpdate(ReservationRequestInput a)
        {
            string message = "";
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                try
                {
                    string folioNumber = user.PIN;
                    string strReservationKey = GetReservationKey(folioNumber).ToString();
                    //DateTime? sysDate = _DbContext.Controls.Select(p => p.SystemDate).FirstOrDefault();
                    //int curr = DateTime.Now.Hour;
                    //int min = DateTime.Now.Minute;
                    //sysDate = sysDate.Value.AddHours(curr).AddMinutes(min);
                    DateTime? sysDate = DateTime.Now;
                    int success = 0;
                    if (a.ReservationRequestKey.ToString() == Guid.Empty.ToString())
                    {
                        a.ReservationRequestKey = Guid.NewGuid();
                        a.ReservationRequestID = _registrationdalRepository.GetReservationRequestID();
                        a.Status = 1;//open request
                        a.ReservationKey = strReservationKey == "" ? new Guid?() : Guid.Parse(strReservationKey);
                        a.GuestKey = user.StaffKey.ToString() == "" ? new Guid?() : user.StaffKey;
                        a.RequestDate = sysDate;
                        a.CreatedBy = user.StaffKey;
                        if (AbpSession.TenantId != null)
                        {
                            a.TenantId = (int)AbpSession.TenantId;
                        }
                        success = _registrationdalRepository.AddRequestGuest(a);

                    }
                    else//edit
                    {
                        Guid key = Guid.Parse(a.ReservationRequestKey.ToString());
                        //var tmp = _DbContext.ReservationRequests.Where(x => x.ReservationRequestKey == key).SingleOrDefault();
                        a.ReservationKey = strReservationKey == "" ? new Guid?() : Guid.Parse(strReservationKey);
                        a.GuestKey = user.StaffKey.ToString() == "" ? new Guid?() : user.StaffKey;
                        a.RequestDate = sysDate;
                        a.ModifiedBy = user.StaffKey;
                        if (AbpSession.TenantId != null)
                        {
                            a.TenantId = (int)AbpSession.TenantId;
                        }
                        success = _registrationdalRepository.UpdateRequestGuest(a);
                    }
                    if (success > 0)
                    {
                        message = "success";
                    }
                    else
                    {
                        message = "fail";
                    }


                }
                catch (Exception ex)
                {

                    throw new UserFriendlyException(ex.Message.ToString());
                }


                //else
                //{
                //    throw new UserFriendlyException("Session has expired.");
                //}
            }
            return message;
        }
        private PdfScanInfo GetPdfScanInfo(string folioNumber)
        {
            PdfScanInfo a = new PdfScanInfo();


            DataTable dt = _registrationdalRepository.GetReservationByFolioNumber(folioNumber);
            if (dt.Rows.Count > 0)
            {
                DateTime dtCheckIndate = Convert.ToDateTime(dt.Rows[0]["CheckInDate"]);
                DateTime dtCheckOutdate = Convert.ToDateTime(dt.Rows[0]["CheckOutDate"]);
                TimeSpan tsDay = dtCheckOutdate - dtCheckIndate;
                int intNoOfNight = tsDay.Days;

                a.BookingNo = folioNumber;
                a.BookDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["DocDate"]);
                a.CheckInDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["CheckInDate"]);
                a.CheckOutDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["CheckOutDate"]);
                a.RateType = dt.Rows[0]["RateDescription"].ToString();
                a.Roomtype = dt.Rows[0]["RoomDescription"].ToString();
                a.Night = tsDay.Days.ToString();

                #region BindMainGuestInfo

                CGuest guest = _registrationdalRepository.GetGuestInfoByGuestKey(dt.Rows[0]["GuestKey"].ToString());
                if (guest != null)
                {
                    MainGuestInfo mgi = new MainGuestInfo();
                    mgi.GuestKey = dt.Rows[0]["GuestKey"].ToString();
                    mgi.Title = guest.Title.ToString();
                    mgi.CheckInDate = a.CheckInDate;
                    mgi.CheckOutDate = a.CheckOutDate;
                    mgi.FirstName = (!string.IsNullOrEmpty(guest.FirstName)) ? guest.FirstName.Trim() : "";
                    mgi.Lastname = (!string.IsNullOrEmpty(guest.LastName)) ? guest.LastName.Trim() : "";
                    mgi.Email = guest.EMail.Trim();
                    mgi.NRIC = (!string.IsNullOrEmpty(guest.Passport)) ? guest.Passport.Trim() : "";
                    mgi.DOB = (guest.DOB.HasValue ? guest.DOB.Value : (DateTime?)null);
                    if (guest.Address != null)
                    {
                        mgi.Address = (guest.Address.Trim().Length > 60 ? guest.Address.Trim().Substring(0, 60) : guest.Address.Trim());
                        mgi.Address2 = (guest.Address.Trim().Length > 60 ? guest.Address.Trim().Substring(60) : "");
                    }
                    else
                    {
                        mgi.Address = "";
                        mgi.Address2 = "";
                    }
                    mgi.Postal = guest.Postal;
                    mgi.Mobile = guest.Mobile;
                    if (guest.City == null || guest.City.Equals("--") || string.IsNullOrEmpty(guest.City))
                    {
                        mgi.City = "--";
                        mgi.CityKey = Guid.Empty;
                    }
                    else
                    {
                        if (guest.City != null)
                            mgi.City = guest.City;
                        mgi.CityKey = GetCityKey(guest.City);
                    }
                    if (guest.CountryKey == null || guest.CountryKey == Guid.Empty.ToString())
                    {
                        mgi.Nationality = "--";
                        mgi.NationalityKey = Guid.Empty;
                    }
                    else
                    {
                        mgi.Nationality = GetNationality(guest.CountryKey);
                        mgi.NationalityKey = new Guid(guest.CountryKey);
                    }
                    a.MainGuestInfo = mgi;
                }
                else
                {
                    // show error msg
                }

                #endregion


            }
            else
            {
                //go login page
            }



            return a;
        }

        private GuestDetailScreenInfo GetGuestDetailScreenInfo(string folioNumber, string strGuestKey)
        {
            GuestDetailScreenInfo a = new GuestDetailScreenInfo();

            string ReservationKey = "";
            DataTable dt = _registrationdalRepository.GetReservationByFolioNumber(folioNumber);
            if (dt.Rows.Count > 0)
            {
                ReservationKey = dt.Rows[0]["ReservationKey"].ToString();
                DateTime dtCheckIndate = Convert.ToDateTime(dt.Rows[0]["CheckInDate"]);
                DateTime dtCheckOutdate = Convert.ToDateTime(dt.Rows[0]["CheckOutDate"]);
                TimeSpan tsDay = dtCheckOutdate - dtCheckIndate;
                int intNoOfNight = tsDay.Days;

                a.BookingNo = folioNumber;
                a.BookDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["DocDate"]);
                a.CheckInDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["CheckInDate"]);
                a.CheckOutDate = CommomData.GetDateToDisplayIc(dt.Rows[0]["CheckOutDate"]);
                a.RateType = dt.Rows[0]["RateDescription"].ToString();
                a.Roomtype = dt.Rows[0]["RoomDescription"].ToString();
                a.Night = tsDay.Days.ToString();
                #region BindRoomMaxPaxInfo

                if (dt.Rows.Count > 0)
                {
                    RoomMaxPaxInfo rmp = new RoomMaxPaxInfo();
                    rmp.RoomDescription = dt.Rows[0]["RoomDescription"].ToString();

                    rmp.RoomTypePaxText = GetRoomTypePaxTextByRoomTypeSeq(dt.Rows[0]["RoomSeq"].ToString());

                    rmp.RoomTypePaxLabel = GetRoomTypePaxLabelByRoomTypeSeq(dt.Rows[0]["RoomSeq"].ToString());

                    int intRoomMaxPax = GetRoomTypePaxByRoomTypeSeq(dt.Rows[0]["RoomSeq"].ToString());

                    rmp.RoomTypePax = intRoomMaxPax.ToString();
                    a.RoomMaxPaxInfo = rmp;

                }


                #endregion
                #region BindMainGuestInfo

                CGuest guest = _registrationdalRepository.GetGuestInfoByGuestKey(dt.Rows[0]["GuestKey"].ToString());
                if (guest != null)
                {
                    MainGuestInfo mgi = new MainGuestInfo();
                    mgi.GuestKey = dt.Rows[0]["GuestKey"].ToString();
                    mgi.Title = guest.Title.ToString();
                    mgi.CheckInDate = a.CheckInDate;
                    mgi.CheckOutDate = a.CheckOutDate;
                    mgi.FirstName = (!string.IsNullOrEmpty(guest.FirstName)) ? guest.FirstName.Trim() : "";
                    mgi.Lastname = (!string.IsNullOrEmpty(guest.LastName)) ? guest.LastName.Trim() : "";
                    mgi.Email = guest.EMail.Trim();
                    mgi.NRIC = (!string.IsNullOrEmpty(guest.Passport)) ? guest.Passport.Trim() : "";
                    mgi.DOB = (guest.DOB.HasValue ? guest.DOB.Value : (DateTime?)null);
                    if (guest.Address != null)
                    {
                        mgi.Address = (guest.Address.Trim().Length > 60 ? guest.Address.Trim().Substring(0, 60) : guest.Address.Trim());
                        mgi.Address2 = (guest.Address.Trim().Length > 60 ? guest.Address.Trim().Substring(60) : "");
                    }
                    else
                    {
                        mgi.Address = "";
                        mgi.Address2 = "";
                    }
                    mgi.Postal = guest.Postal;
                    mgi.Mobile = guest.Mobile;
                    if (guest.City == null || guest.City.Equals("--") || string.IsNullOrEmpty(guest.City))
                    {
                        mgi.City = "--";
                        mgi.CityKey = Guid.Empty;
                    }
                    else
                    {
                        if (guest.City != null)
                            mgi.City = guest.City;
                        mgi.CityKey = GetCityKey(guest.City);
                    }
                    if (guest.CountryKey == null || guest.CountryKey == Guid.Empty.ToString())
                    {
                        mgi.Nationality = "--";
                        mgi.NationalityKey = Guid.Empty;
                    }
                    else
                    {
                        mgi.Nationality = GetNationality(guest.CountryKey);
                        mgi.NationalityKey = new Guid(guest.CountryKey);
                    }
                    a.MainGuestInfo = mgi;
                }
                else
                {
                    // show error msg
                }

                #endregion
                #region sign
                DocumentSign doc = _registrationdalRepository.BindMainGuestSignature(ReservationKey, strGuestKey);
                MainGuestSignature mgs = new MainGuestSignature();
                if (doc.Signature != null)
                {
                    var varGuestSign = Convert.ToBase64String(doc.Signature);
                    mgs.imgGuestSign = string.Format("data:image/png;base64,{0}", varGuestSign);
                    mgs.GuestSign = true;
                }
                else
                {
                    mgs.GuestSign = false;
                }
                a.MainGuestSignature = mgs;
                #endregion
                #region share guest
                DataTable dtr = _registrationdalRepository.GetReservationGuestByReservationKey(ReservationKey);
                List<ShareGuestlist> slst = new List<ShareGuestlist>();
                if (dtr.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtr.Rows)
                    {
                        ShareGuestlist o = new ShareGuestlist();
                        o.No = !DBNull.Value.Equals(dr["No"]) ? dr["No"].ToString() : "";
                        o.Name = !DBNull.Value.Equals(dr["Name"]) ? dr["Name"].ToString() : "";
                        o.GuestKey = (!DBNull.Value.Equals(dr["GuestKey"])) ? (!string.IsNullOrEmpty(dr["GuestKey"].ToString()) ? new Guid(dr["GuestKey"].ToString()) : Guid.Empty) : Guid.Empty;

                        slst.Add(o);
                    }
                }
                a.ShareGuestlist = slst;
                #endregion

            }
            else
            {
                //go login page
            }



            return a;
        }
        private string ToSafeFileName(string s)
        {
            return s
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
        }
        private string HPageForP(PdfScanInfo a)
        {
            string vCardText = "BEGIN:GUESTINFO\r\nVERSION:2.1\r\nN:";
            vCardText += "NAME:" + a.MainGuestInfo.FirstName + a.MainGuestInfo.Lastname + "\r\n";
            vCardText += "EMAIL;PREF;INTERNET:" + a.MainGuestInfo.Email + "\r\n";
            vCardText += "TEL;WORK;VOICE:" + a.MainGuestInfo.Mobile + "\r\n";
            vCardText += "CHECKINDATE:" + a.CheckInDate + "\r\n";
            vCardText += "END:GUESTINFO";
            string QRCodeImage = GenerateQRCode(vCardText);
            string MainGuestInfoDOB = a.MainGuestInfo.DOB == null ? "" : a.MainGuestInfo.DOB.Value.ToString("dd/MM/yyyy");
            string html = "<html><head id=\"Head1\"><title>iCheckIn</title></head><body>" +
                "<div style=\"width:1170px;padding-right: 15px;padding-left: 15px;margin-right: auto;margin-left: auto;margin-top:-10px;\">" +
                "<img src=\"data:image/png;base64,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\" alt=\"My Image\" alt=\"logo\" title=\"D'Resort\" style=\"width: 250px\"></div>" +
                "<div style=\"width: 1170px;padding-right: 15px;padding-left: 15px;margin-right: auto;margin-left: auto;\">" +
                "<div style=\"border-bottom: 1px solid transparent;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;font-weight: 500;line-height: 1.1;font-weight: bold;font-size: x-large;color:orange;\">Guest Registration </div>" +
                "<table style=\"border-collapse: collapse; border: 1px solid black;width:100%;border-color:orange;\"><tr><th style=\"text-align: left;background-color:orange;color:white;padding: 10px 15px;border-bottom: 1px solid transparent;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;font-weight: 500;line-height: 1.1;\">Your Reservation Details</th>" +
                "<th style=\"text-align: left;background-color:orange;color:white;padding: 10px 15px;border-bottom: 1px solid transparent;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;font-weight: 500;line-height: 1.1;\">OR COde</th></tr><tr><td style=\"width:50%;\"><table><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Booking No.  :</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>" + a.BookingNo + "</span></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Book Date :</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>" + a.BookDate + "</span></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Check in  :</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>" + a.CheckInDate + "</span></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Check out  :</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>" + a.CheckOutDate + "</span></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Rate Type  :</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>" + a.RateType + "</span></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Room Type  :</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>" + a.Roomtype + "</span></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Night(s)  :</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>" + a.Night + "</span></td></tr></table></td>" +
                "<td style=\"width:50%;\"><table><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;\"><p><img style=\"display: inline-block;max-width: 100%;height: auto;padding: 4px;line-height: 1.42857143;background-color: #fff;border: 1px solid #ddd;border-radius: 4px;transition: all .2s ease-in-out;\" " +
                "src=\"data:image/png;base64," + QRCodeImage + "\" width=\"304\" height=\"236\"/></p></td></tr></table></td></tr></table><br />" +
                "<table style=\"border-collapse: collapse; border: 1px solid black;width:100%;border-color:orange;\"><tr><th colspan=\"2\" style=\"text-align: left;background-color:orange;color:white;padding: 10px 15px;border-bottom: 1px solid transparent;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;font-weight: 500;line-height: 1.1;\">Guest Information </th></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Salutation : </td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><select name=\"ddlTitle\" id=\"ddlTitle\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;width:100px;\"><option selected=\"selected\" value=\"" + a.MainGuestInfo.Title + "\">" + a.MainGuestInfo.Title + "</option></select></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">First Name:*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtName\" value=\"" + a.MainGuestInfo.FirstName + "\" maxlength=\"40\" id=\"txtName\" style=\"cursor: not-allowed;opacity: 1;display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #eee;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"First Name\" type=\"text\" /></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Last Name:*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtLastname\" value=\"" + a.MainGuestInfo.Lastname + "\" id=\"txtLastname\" style=\"cursor: not-allowed;opacity: 1;display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #eee;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"Last Name\" type=\"text\" /></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Email:*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtEmail\" value=\"" + a.MainGuestInfo.Email + "\" maxlength=\"50\" id=\"txtEmail\" style=\"cursor: not-allowed;opacity: 1;display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #eee;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"Email\" type=\"text\" /></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Passport/ID :*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtNRIC\" type=\"text\" value=\"" + a.MainGuestInfo.NRIC + "\" id=\"txtNRIC\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" /></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Date of Birth:*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">" + MainGuestInfoDOB + "</td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Address:*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">" + a.MainGuestInfo.Address + "" + a.MainGuestInfo.Address2 + "</td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Postal Code:*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtPostal\" type=\"text\" value=\"" + a.MainGuestInfo.Postal + "\" maxlength=\"10\" id=\"txtPostal\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"Postal Code\" /></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">City:</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><select name=\"ddlCity\" id=\"ddlCity\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\"><option selected=\"selected\" value=\"" + a.MainGuestInfo.CityKey + "\">" + a.MainGuestInfo.City + "</option></select></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Country:</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><select name=\"ddlCountry\" id=\"ddlCountry\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\"><option selected=\"selected\" value=\"" + a.MainGuestInfo.NationalityKey + "\">" + a.MainGuestInfo.Nationality + "</option></select></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Mobile:*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtMobile\" type=\"text\" value=\"" + a.MainGuestInfo.Mobile + "\" maxlength=\"20\" id=\"txtMobile\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"Mobile\" /></td></tr>" +
                "</table></div><br/></body></html>";

            return html;
        }
        private string ScreenShootImg(GuestDetailScreenInfo a)
        {
            string vCardText = "BEGIN:GUESTINFO\r\nVERSION:2.1\r\nN:";
            vCardText += "NAME:" + a.MainGuestInfo.FirstName + a.MainGuestInfo.Lastname + "\r\n";
            vCardText += "EMAIL;PREF;INTERNET:" + a.MainGuestInfo.Email + "\r\n";
            vCardText += "TEL;WORK;VOICE:" + a.MainGuestInfo.Mobile + "\r\n";
            vCardText += "CHECKINDATE:" + a.CheckInDate + "\r\n";
            vCardText += "END:GUESTINFO";
            string QRCodeImage = GenerateQRCode(vCardText);
            string MainGuestInfoDOB = a.MainGuestInfo.DOB == null ? "" : a.MainGuestInfo.DOB.Value.ToString("dd/MM/yyyy");
            string shareguestresult = "";
            if(a.ShareGuestlist.Count>0)
            {
                foreach(ShareGuestlist shareguest in a.ShareGuestlist)
                {
                    shareguestresult += "<tr><td style=\"width:10%;padding: 10px 15px;font-size: 15px;font-family:Helvetica,Arial,sans-serif;\">"+shareguest.No+"</td><td style=\"font-size: 15px;font-family:Helvetica,Arial,sans-serif;\">"+shareguest.Name+"</td><td style=\"width:10%;color: #428bca;\">Remove </td></tr>";
                }
            }

            string html = "<html><head id=\"Head1\"><title>iCheckIn</title></head><body>" +
                "<div style=\"width:1170px;padding-right: 15px;padding-left: 15px;margin-right: auto;margin-left: auto;margin-top:-10px;\">" +
                "<img src=\"data:image/png;base64,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\" alt=\"My Image\" alt=\"logo\" title=\"D'Resort\" style=\"width: 250px\">" +
                "</div><div style=\"width: 1170px;padding-right: 15px;padding-left: 15px;margin-right: auto;margin-left: auto;\">" +
                "<div style=\"border-bottom: 1px solid transparent;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;font-weight: 500;line-height: 1.1;font-weight: bold;font-size: x-large;color:orange;\">Guest Registration </div>" +
                "<table><tr><table style=\"width:100%;border-collapse: collapse;border: 1px solid white;\"><tr><td style=\"border-collapse: collapse;border: 1px solid white;width:68%;\"><table style=\"width:100%;border-collapse:collapse;border:1px solid orange;\"\">" +
                "<tr ><th colspan=\"2\" style=\"width:68%;text-align: left;border-collapse: collapse;border: 1px solid orange;background-color:orange;color:white;padding: 10px 15px;border-bottom: 1px solid transparent;font-size: 16px;font-family:Helvetica,Arial,sans-serif;font-weight: 500;line-height: 1.3;\">Guest Information </th><tr/>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Salutation : </td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><select name=\"ddlTitle\" id=\"ddlTitle\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;width:100px;\"><option selected=\"selected\" value=\"" + a.MainGuestInfo.Title + "\">" + a.MainGuestInfo.Title + "</option></select></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">First Name:*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtName\" value=\"" + a.MainGuestInfo.FirstName + "\" maxlength=\"40\" id=\"txtName\" style=\"cursor: not-allowed;opacity: 1;display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #eee;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"First Name\" type=\"text\" /></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Last Name:*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtLastname\" value=\"" + a.MainGuestInfo.Lastname + "\" id=\"txtLastname\" style=\"cursor: not-allowed;opacity: 1;display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #eee;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"Last Name\" type=\"text\" /></td></tr>" +
                "<tr> <td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Email:*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtEmail\" value=\"" + a.MainGuestInfo.Email + "\" maxlength=\"50\" id=\"txtEmail\" style=\"cursor: not-allowed;opacity: 1;display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #eee;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"Email\" type=\"text\" /></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Passport/ID :*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtNRIC\" type=\"text\" value=\"" + a.MainGuestInfo.NRIC + "\" id=\"txtNRIC\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" /></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Date of Birth:*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtDob\" type=\"text\" value=\""+MainGuestInfoDOB+"\" style =\"display: block;width: 120px;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" /></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Address:*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtadd1\" type=\"text\" value=\"" + a.MainGuestInfo.Address + "\" placeholder=\"Address\"  id=\"txtadd1\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\"/></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;width:10%;\">&nbsp;</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtadd2\" type=\"text\" value=\"" + a.MainGuestInfo.Address2 + "\" placeholder=\"Address\"  id=\"txtadd2\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\"/></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Postal Code:*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtPostal\" type=\"text\" value=\"" + a.MainGuestInfo.Postal + "\" maxlength=\"10\" id=\"txtPostal\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"Postal Code\" /></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">City:</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><select name=\"ddlCity\" id=\"ddlCity\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\"><option selected=\"selected\" value=\"" + a.MainGuestInfo.CityKey + "\">" + a.MainGuestInfo.City + "</option></select></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Country:</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">\r\n\t<select name=\"ddlCountry\" id=\"ddlCountry\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\"><option selected=\"selected\" value=\""+ a.MainGuestInfo.NationalityKey + "\">" + a.MainGuestInfo.Nationality + "</option></select></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Mobile:*</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><input name=\"txtMobile\" type=\"text\" value=\"" + a.MainGuestInfo.Mobile + "\" maxlength=\"20\" id=\"txtMobile\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"Mobile\" /></td></tr></table></td>" +
                "<td style=\"width:2%;\"></td>" +
                "<td style=\"border-collapse:collapse;border: 1px solid white;width:30%;\"><table style=\"width:100%;border-collapse:collapse;border:1px solid #ddd;\">" +
                "<tr><th colspan=\"2\"  style=\"width:100%;text-align: left;border-collapse: collapse;border: 1px solid #ddd;background-color: #f5f5f5;color: #428bca;padding: 10px 15px;border-bottom: 1px solid transparent;font-size: 16px;font-family:Helvetica,Arial,sans-serif;font-weight: 500;line-height: 1.3;\">Your Reservation Details</th><tr/>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Booking No.  :</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>" + a.BookingNo + "</span></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Book Date :</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>" + a.BookDate + "</span></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Check in  :</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>" + a.CheckInDate + "</span></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Check out  :</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>" + a.CheckOutDate + "</span></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Rate Type  :</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>" + a.RateType + "</span></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Room Type  :</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>" + a.Roomtype + "</span></td></tr>" +
                "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Night(s)  :</td>" +
                "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>" + a.Night + "</span></td></tr></table>" +
                "<table><tr><td><span style=\"display:inline-block;height:365px;width:400px;\"><img src=\"data:image/png;base64,"+QRCodeImage+ "\" width=\"304\" height=\"236\"></span></td><tr/></table></td></tr></table></tr></table>" +
                "<table style=\"width:63.2%;border-collapse: collapse;border: 1px solid orange;margin-top:10px;\"><tr><th  style=\"width:100%;text-align: left;border-collapse: collapse;border: 1px solid orange;background-color:orange;color:white;padding: 10px 15px;border-bottom: 1px solid transparent;font-size: 16px;font-family:Helvetica,Arial,sans-serif;font-weight: 500;line-height: 1.3;\"><span>Shared Guest(s) </span><span style=\"float: right\"><a>Manage Shared Guests <span class=\"glyphicon glyphicon-cog\" aria-hidden=\"true\"></span></a></span></th></tr>" +
                "<tr><td style=\"padding: 10px 15px;font-size: 15px;font-family:Helvetica,Arial,sans-serif;\">Max. Occupancy for "+a.RoomMaxPaxInfo.RoomDescription+" is "+a.RoomMaxPaxInfo.RoomTypePaxText+"</td></tr><tr> <td style=\"padding: 10px 15px;\"><table style=\"width:100%;border-collapse: collapse;border: 1px solid #ddd;\">" +shareguestresult+
                "</table></td></tr></table><br/><table style=\"width:68%;border-collapse: collapse;border: 1px solid white;\"><tr><td><span style=\"font-weight:bold;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">(Guest Signature)</span><span style=\"padding-left: 0px;color: #428bca;\">Please click here to sign</span></td></tr>" +
                "<tr><td style=\"border-top-left-radius: 3px;border-top-right-radius: 3px;\"><p><img id=\"imgGuestSign\" src=\""+a.MainGuestSignature.imgGuestSign + "\" style=\"border-color:Gray;border-width:1px;border-style:Solid;height:200px;width:415px;\"></p></td></tr></table></div><br/></body></html>";
            return html;
        }
        private string GenerateQRCode(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            // Convert the QR code image to a byte array
            MemoryStream ms = new MemoryStream();
            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] qrCodeBytes = ms.ToArray();

            // Convert the byte array to a base64 string
            string base64String = Convert.ToBase64String(qrCodeBytes);

            // Return the base64 string
            return base64String;

        }

        //private string HPageForP(PdfScanInfo a)
        //{


        //    string html = "<html><head><title>iCheckIn</title></head><body>";
        //    html += "<div style=\"width: 1170px; padding - right: 15px; padding - left: 15px; margin - right: auto; margin - left: auto; margin - top:-10px;\">";
        //    html += "<img src=\"data: image / png; base64,iVBORw0KGgoAAAANSUhEUgAABT4AAAL2CAYAAABoson2AAAACXBIWXMAAFxGAABcRgEUlENBAAAgAElEQVR42uzdu6ssWX7g + zHbzD + gjDTKETSjpMcWZBtyRdJQ0CAQCYMYgZxEyNHDCBCCppmBdDRCaBhSjxmQHKUxEno4AYVkSmmMVVwuSVNGQ7GrUtX16L7dUt4T1bGlXafOOfnYEbF + a63Pho + lVp29M + P5jbVW / Ifz + fwfAAAAAABK4kMAAAAAAIRPAAAAAADhEwAAAABA + AQAAAAAED4BAAAAAIRPAAAAAED4BAAAAAAQPgEAAAAAhE8AAAAAAOETAAAAAED4BAAAAACETwAAAAAA4RMAAAAAQPgEAAAAABA + AQAAAACETwAAAABA + AQAAAAAED4BAAAAAIRPAAAAAADhEwAAAABA + AQAAAAAED4BAAAAAOETAAAAAED4BACu9f / +n681MLK1 / exrc9sBE1g6rwGA8AkA / HuQOcPIWvvZ15a2AybQOK8BgPAJAAifCJ / CJ8InACB8AoDwCcKn8InwCQAInwAgfILwKXwifAKA8AkACJ8In8InCJ8AIHwCAMInwqfwCcInAAifAIDwifApfCJ8AgDCJwAInyB8Cp8InwCA8AkAwicIn8InwicACJ8AgPCJ8Cl8gvAJAMInACB8InwKnyB8AoDwCQAInwifwifCJwAgfAKA8AnCp / CJ8AkACJ8AIHyC8Cl8InwCAMInAAifCJ / CJwifACB8AgDCJ8Kn8AnCJwAInwCA8InwKXwifAIAwicACDJiAcKn8InwCQAInwAgfILwKXwifAIAwicACJ8In / Yz4RPhEwCETwBg4iCzemH3wkk0YAT7bhuzn31t9sL6hYNtghF0x +/ tC3PnNQAQPgEAYYbxHF / YiDCv3dfmfaTysIFnj6bujtv2KwAQPgGA68PMoh8FKixwa4SZbHTnw1tvz19YvrB6oXmivdL + pf +/ ZW8 + 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\" style=\"width: 250px\"></div>";
        //    html += "<div style=\"width: 1170px;padding-right: 15px;padding-left: 15px;margin-right: auto;margin-left: auto;\">";
        //    html += "<div style=\"border-bottom: 1px solid transparent;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;font-weight: 500;line-height: 1.1;font-weight: bold;font-size: x-large;color:orange;\">Guest Registration </div>";
        //    html += "<table style=\"border-collapse: collapse; border: 1px solid black;width:100%;border-color:orange;\">";
        //    html += "<tr><th style=\"text-align: left;background-color:orange;color:white;padding: 10px 15px;border-bottom: 1px solid transparent;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;font-weight: 500;line-height: 1.1;\">Your Reservation Details</th>";
        //    html += "<th style=\"text-align: left;background-color:orange;color:white;padding: 10px 15px;border-bottom: 1px solid transparent;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;font-weight: 500;line-height: 1.1;\">OR COde</th></tr>";
        //    html += "<tr><td style=\"width:50%;\"><table><tr>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Booking No.  :</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>147139</span></td>";
        //    html += "</tr><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Book Date :</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>12 Apr 2019</span></td>";
        //    html += "</tr><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Check in  :</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>25 Sep 2019</span></td>";
        //    html += "</tr><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Check out  :</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>27 Sep 2019</span></td>";
        //    html += "</tr><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Rate Type  :</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>($120NETT)TA ROOM ONLY</span></td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>Premier Queen</span></td></tr>";
        //    html += "<tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Night(s)  :</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\"><span>2</span></td></tr>";
        //    html += "</table></td><td style=\"width:50%;\" >< table >< tr >< td style = \"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;\">";
        //    html += "<p><img style=\"display: inline-block;max-width: 100%;height: auto;padding: 4px;line-height: 1.42857143;background-color: #fff;border: 1px solid #ddd;border-radius: 4px;transition: all .2s ease-in-out;\" src = \"data:image/png;base64,\" width=\"304\" height=\"236\"/></p></td></tr></table></td>";
        //    html += "</tr></table></td></tr></table><br /><table style=\"border - collapse: collapse; border: 1px solid black; width: 100 %; border - color:orange;\"><tr>";
        //    html += "<th colspan=\"2\" style=\"text-align: left;background-color:orange;color:white;padding: 10px 15px;border-bottom: 1px solid transparent;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;font-weight: 500;line-height: 1.1;\">Guest Information </th>";
        //    html += "</tr><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Salutation : </td>";
        //    html += "<td style=\"padding: 10px 15px; border - top - left - radius: 3px; border - top - right - radius: 3px; font - size: 16px; font - family:Helvetica,Arial,sans - serif;\"><select name=\"ddlTitle\" id=\"ddlTitle\" style=\"display: block; width: 100 %; height: 34px; padding: 6px 12px; font - size: 14px; line - height: 1.42857143; color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;width:100px;\">";
        //    html += "<option selected=\"selected\">Mr</option></select></td></tr><tr>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">First Name:*</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">";
        //    html += "<input name=\"txtName\" value=\"nanda\" maxlength=\"40\" id=\"txtName\" style=\"cursor: not-allowed;opacity: 1;display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #eee;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"First Name\" type=\"text\" /></td>";
        //    html += "</tr><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Last Name:*</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">";
        //    html += "<input name=\"txtLastname\" value=\"han\" id=\"txtLastname\" style=\"cursor: not-allowed;opacity: 1;display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #eee;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"Last Name\" type=\"text\" />";
        //    html += "</td></tr><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Email:*</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">";
        //    html += "<input name=\"txtEmail\" value=\"brillantezhan@gmail.com\" id=\"txtEmail\" style=\"cursor: not - allowed; opacity: 1; display: block; width: 100 %; height: 34px; padding: 6px 12px; font - size: 14px; line - height: 1.42857143; color: #555;background-color: #eee;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"Email\" type=\"text\" />";
        //    html += "</td></tr><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Passport/ID :*</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">";
        //    html += "<input name=\"txtNRIC\" type = \"text\" value = \"l5lOMxMCt3OQYo4oCZM+1w==\" id = \"txtNRIC\" style = \"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" />";
        //    html += "</tr><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Date of Birth:*</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">11/12/1989</td>";
        //    html += "</tr><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Address:*</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">aaaaaa</td>";
        //    html += "</tr><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Postal Code:*</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">";
        //    html += "<input name=\"txtPostal\" type = \"text\" value = \"644444\" id = \"txtPostal\" style = \"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"Postal Code\" />";
        //    html += "</tr><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">City:</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">";
        //    html += "<select name=\"ddlCity\" id = \"ddlCity\" style = \"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\">";
        //    html += "<option selected=\"selected\" >--</option>";
        //    html += "</select></td></tr><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Country:</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">";
        //    html += "<select name=\"ddlCountry\" id=\"ddlCountry\" style=\"display: block;width: 100%;height: 34px;padding: 6px 12px;font-size: 14px;line-height: 1.42857143;color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\">";
        //    html += "<option selected=\"selected\">Singapore</option>";
        //    html += "</select></td></tr><tr><td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">Mobile:*</td>";
        //    html += "<td style=\"padding: 10px 15px;border-top-left-radius: 3px;border-top-right-radius: 3px;font-size: 16px;font-family:Helvetica,Arial,sans-serif;\">";
        //    html += "<input name=\"txtMobile\" type=\"text\" value=\"65999999999\" id=\"txtMobile\" style=\"display: block; width: 100 %; height: 34px; padding: 6px 12px; font - size: 14px; line - height: 1.42857143; color: #555;background-color: #fff;background-image: none;border: 1px solid #ccc;border-radius: 4px;box-shadow: inset 0 1px 1px rgba(0,0,0,.075);transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;\" placeholder=\"Mobile\" />";
        //    html += "</td> </tr></table> </div><br/></body></html>";
        //    return html;
        //}
        private EmailList GetFormData(string Email, string FolioNumber, string ReservationKey, string Title, string FirstName, string LastName, string HotelName)
        {
            EmailList el = new EmailList();
            el.mail_from = _configurationAccessor.Configuration["BrillantezEmail"];//CommomData.mailfrom;
            el.mail_to = Email;
            el.mail_subject = "Guest Information";
            el.mail_body = EmailForm(Title, FirstName, LastName, HotelName);
            el.mail_status = 1;
            el.date_sent = DateTime.Now;
            el.date_tosend = DateTime.Now;
            el.doc_no = FolioNumber;
            el.sourcekey = new Guid(ReservationKey);
            return el;
        }
        //private Bitmap ConvertPixelDataToBitmap(byte[] pixelData, int width, int height)
        //{
        //    Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        //    BitmapData bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
        //        ImageLockMode.WriteOnly, bitmap.PixelFormat);
        //    IntPtr bitmapPtr = bitmapData.Scan0;
        //    Marshal.Copy(pixelData, 0, bitmapPtr, pixelData.Length);
        //    bitmap.UnlockBits(bitmapData);
        //    return bitmap;
        //}
        #region CreateFileWithUniqueName
        public static string CreateFileWithUniqueName(string folder, string fileName)
        {
            string str = "";
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            try
            {
                var fileBase = Path.GetFileNameWithoutExtension(fileName);
                var ext = Path.GetExtension(fileName);
                string[] files = Directory.GetFiles(folder);
                foreach (string file in files)
                {
                    DataRow dr = dt.NewRow();
                    dr["Name"] = Path.GetFileName(file);
                    dt.Rows.Add(dr);
                }

                DataRow[] dr_data = dt.Select("Name like'" + fileBase + "%'");

                if (dr_data.Length > 1)
                {
                    for (int i = dr_data.Length; i >= 1; i--)
                    {
                        var check_ext = Path.GetExtension(dr_data[i - 1]["Name"].ToString());
                        var check_fileBase = Path.GetFileNameWithoutExtension(dr_data[i - 1]["Name"].ToString());

                        if (check_fileBase.Contains('_'))
                        {
                            int index = check_fileBase.LastIndexOf("_");
                            check_fileBase = check_fileBase.Substring(index);
                            string[] s = check_fileBase.Split('_');
                            if (s.Length > 0)
                            {
                                int count = Convert.ToInt32(s[1]) + 1;
                                str = fileBase + "_" + count + ext;
                                break;
                            }
                        }
                        else
                        {
                            str = fileBase + "_1" + ext;
                        }

                    }
                }
                else if (dr_data.Length == 1)
                {
                    str = fileBase + "_1" + ext;
                }
                else
                {
                    str = fileBase + ext;
                }
            }
            catch { }
            return str;
        }
        #endregion
        private string EmailForm(string Title, string FirstName, string LastName, string HotelName)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Dear" + "&nbsp;&nbsp;&nbsp;" + Title + "&nbsp;&nbsp;&nbsp;" + FirstName + "&nbsp;&nbsp;&nbsp;" + LastName);
            builder.Append("<br></br>");
            builder.Append("<p>&nbsp;&nbsp;&nbsp;Please see the following attachment for guest information.</p>");
            builder.Append("<br></br>");
            builder.Append("<br></br>");
            builder.Append("<br></br>");
            builder.Append("<br></br>");
            builder.Append("<br></br>");
            builder.Append("Thanks & Best Regards");
            builder.Append("<br></br>");
            builder.Append(HotelName);
            return builder.ToString();
        }
        private byte[] ConvertImageToByte(Bitmap img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        private static int GetDateDifference(DateTime? dtChkInDate, DateTime? dtChkOutDate)
        {
            if (dtChkInDate.HasValue && dtChkOutDate.HasValue)
            {
                return dtChkOutDate.Value.Subtract(dtChkInDate.Value).Days;
            }
            else return 0;
        }

        private void InsertGuestSignatureLog(DocumentSign document, bool blnAdd)
        {
            try
            {
                List<CHistory> listHistory = new List<CHistory>();
                CHistory history = new CHistory();
                history.HistoryKey = Guid.NewGuid();
                history.SourceKey = document.ReservationKey;
                history.TableName = "Document";
                if (blnAdd)
                {
                    history.Operation = 'I';
                    history.Detail = "(iCheckIn) Guest signed at online check-in.";
                }
                else
                {
                    history.Operation = 'U';
                    history.Detail = "(iCheckIn) Guest updated signature.";
                }
                if (AbpSession.TenantId != null)
                {
                    history.TenantId = (int)AbpSession.TenantId;
                }

                listHistory.Add(history);
                _registrationdalRepository.InsertHistoryList(listHistory);

            }
            catch (Exception ex)
            {
                // write to log
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
        }

        private void InsertRemoveSharedGuestHistoryInfo(string resKey, string guestKey)
        {
            try
            {
                List<CHistory> listHistory = new List<CHistory>();
                CGuest guest = _registrationdalRepository.GetGuestInfoByGuestKey(guestKey);
                if (guest != null && !string.IsNullOrEmpty(guest.Name))
                {
                    CHistory history = new CHistory();
                    history.SourceKey = new Guid(resKey);
                    history.Operation = 'D';
                    history.TableName = "Reservation";
                    history.Detail = "(iCheckIn) Remove Shared Guest: " + guest.Name;
                    if (AbpSession.TenantId != null)
                    {
                        history.TenantId = (int)AbpSession.TenantId;
                    }
                    _registrationdalRepository.InsertHistoryList(listHistory);
                }
            }
            catch (Exception ex)
            {
                // LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
        }

        private void InsertChkOutHistoryInfo(string resKey, string folio, string logintime)
        {
            try
            {

                List<CHistory> listHistory = new List<CHistory>();
                CHistory history = new CHistory();
                history.HistoryKey = Guid.NewGuid();
                history.SourceKey = new Guid(resKey);
                history.Operation = 'U';
                history.TableName = "Reservation";
                history.Detail = "(iCheckIn) Folio #" + folio + " Login Time:" + logintime + " CheckOut Time:" + DateTime.Now.ToString("MM / dd / yyyy h: mm tt") + " has check out from online check out";

                if (AbpSession.TenantId != null)
                {
                    history.TenantId = (int)AbpSession.TenantId;
                }

                listHistory.Add(history);
                _registrationdalRepository.InsertHistoryList(listHistory);
            }
            catch (Exception ex)
            {
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
        }

        private void InsertMainGuestHistoryInfo(string resKey, string preCheckInCount)
        {
            try
            {
                List<CHistory> listHistory = new List<CHistory>();
                CHistory history = new CHistory();
                history.HistoryKey = Guid.NewGuid();
                history.SourceKey = new Guid(resKey);
                history.Operation = 'U';
                history.TableName = "Reservation";
                history.Detail = "(iCheckIn) Update Pre-CheckInCount : " + preCheckInCount;
                if (AbpSession.TenantId != null)
                {
                    history.TenantId = (int)AbpSession.TenantId;
                }

                listHistory.Add(history);
                _registrationdalRepository.InsertHistoryList(listHistory);
            }
            catch (Exception ex)
            {
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
        }
        private void InsertSharedGuestHistoryInfo(string reservationKey, CGuest guest, bool blnMainGuest, bool blnAdd)
        {
            List<CHistory> listHistory = new List<CHistory>();
            try
            {
                char cOperation = 'U';
                if (blnAdd)
                    cOperation = 'I';

                string strDetailPrefix = "";
                if (blnMainGuest)
                {
                    if (blnAdd)
                        strDetailPrefix = "(iCheckIn)Add MainGuest : ";
                    else
                        strDetailPrefix = "(iCheckIn)Update MainGuest : ";
                }
                else
                {
                    if (blnAdd)
                        strDetailPrefix = "(iCheckIn)Add SharedGuest : ";
                    else
                        strDetailPrefix = "(iCheckIn)Update SharedGuest : ";
                }

                CHistory history;
                if (!blnMainGuest)
                {
                    history = new CHistory();
                    history.HistoryKey = Guid.NewGuid();
                    history.Operation = cOperation;
                    history.TableName = "Guest";
                    history.SourceKey = new Guid(reservationKey);
                    history.Detail = strDetailPrefix + "Guest : " + guest.Title + " " + guest.FirstName + " " + guest.LastName + " , Email : " + guest.EMail + " , NRIC : " + guest.Passport;
                    if (AbpSession.TenantId != null)
                    {
                        history.TenantId = (int)AbpSession.TenantId;
                    }
                    listHistory.Add(history);
                }
                history = new CHistory();
                history.HistoryKey = Guid.NewGuid();
                history.Operation = cOperation;
                history.TableName = "Guest";
                history.SourceKey = new Guid(reservationKey);
                history.Detail = strDetailPrefix + "DOB : " + Convert.ToDateTime(guest.DOB).ToString("dd-MMM-yyyy") + " , Address : " + guest.Address + " , Postal : " + guest.Postal + " , City : " + guest.City + " , Mobile : " + guest.Mobile;
                if (AbpSession.TenantId != null)
                {
                    history.TenantId = (int)AbpSession.TenantId;
                }

                listHistory.Add(history);
                _registrationdalRepository.InsertHistoryList(listHistory);

            }
            catch (Exception ex)
            {
                //throw ex;
            }


        }

        private int GetRoomTypePaxByRoomTypeSeq(string seq)
        {
            try
            {
                int intMaxPax = 0;

                //if (propertyName.ToLower().Equals(EProperty.DownTown.ToString().ToLower()))
                //{
                if (seq == "53" || seq == "54" || seq == "57" || seq == "60")
                {

                    //imgPax.ToolTip = "4 Adults / 2 Adults 2 Children";
                    intMaxPax = 4;
                }
                else if (seq == "58")
                {
                    //imgPax.ToolTip = "2 Adults 2 Children";
                    intMaxPax = 4;
                }
                else if (seq == "59")
                {
                    //imgPax.ToolTip = "2 Adults 2 Children";
                    intMaxPax = 3;
                }
                else if (seq == "63")
                {

                    //imgPax.ToolTip = "2 Adults";
                    intMaxPax = 2;
                }
                else
                {

                    //imgPax.ToolTip = "2 Adults";
                    intMaxPax = 2;
                }
                //}
                //else if (propertyName.ToLower().Equals(EProperty.Sentosa.ToString().ToLower()))
                //{
                //    if (seq == "47")
                //    {
                //        //imgPax.ToolTip = "4 Adults / 2 Adults 2 Children";
                //        intMaxPax = 4;
                //    }
                //    else if (seq == "48")
                //    {
                //        //imgPax.ToolTip = "2 Adults";
                //        intMaxPax = 2;
                //    }
                //    else
                //    {

                //        //imgPax.ToolTip = "2 Adults";
                //        intMaxPax = 2;
                //    }

                //}
                //else if (propertyName.ToLower().Equals(EProperty.Banyu_Biru.ToString().ToLower()))
                //{
                //    if (seq == "49")
                //    {

                //        //imgPax.ToolTip = "6 Adults / 4 Adults + 2 Children";
                //        intMaxPax = 6;
                //    }
                //    else
                //    {

                //        intMaxPax = 2;
                //    }
                //}

                return intMaxPax;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private RoomTypePaxLabel GetRoomTypePaxLabelByRoomTypeSeq(string seq)
        {
            try
            {
                RoomTypePaxLabel imgPax = new RoomTypePaxLabel();
                string strImgAdult4OrAdult2AndChild2 = " <img src='images/adult4.png'> | <img src='images/adult2child2.png'>";
                string strImgAdult2AndChild2 = "<img src='images/adult2child2.png'>";
                string strImgAdult3 = " <img src='images/adult3.png'>";
                string strImgAdult2 = " <img src='images/adult2.png'>";

                //if (propertyName.ToLower().Equals(EProperty.DownTown.ToString().ToLower()))
                //{
                if (seq == "53" || seq == "54" || seq == "57" || seq == "60")
                {
                    imgPax.imgPaxText = strImgAdult4OrAdult2AndChild2;
                    imgPax.imgPaxToolTip = "4 Adults / 2 Adults 2 Children";
                }
                else if (seq == "58")
                {
                    imgPax.imgPaxText = strImgAdult2AndChild2;
                    imgPax.imgPaxToolTip = "2 Adults 2 Children";
                }
                else if (seq == "59")
                {
                    imgPax.imgPaxText = strImgAdult3;
                    imgPax.imgPaxToolTip = "3 Adults";
                }
                else if (seq == "63")
                {
                    imgPax.imgPaxText = strImgAdult2;
                    imgPax.imgPaxToolTip = "2 Adults";
                }
                else
                {
                    imgPax.imgPaxText = strImgAdult2;
                    imgPax.imgPaxToolTip = "2 Adults";
                }
                //}
                //else if (propertyName.ToLower().Equals(EProperty.Sentosa.ToString().ToLower()))
                //{
                //    if (seq == "47")
                //    {
                //        imgPax.Text = strImgAdult4OrAdult2AndChild2Orange;
                //        imgPax.ToolTip = "4 Adults / 2 Adults 2 Children";
                //    }
                //    else if (seq == "48")
                //    {
                //        imgPax.Text = strImgAdult2Orange;
                //        imgPax.ToolTip = "2 Adults";
                //    }

                //    else if (seq == "53" || seq == "54" || seq == "57" || seq == "60")
                //    {
                //        imgPax.Text = strImgAdult4OrAdult2AndChild2Orange;
                //        imgPax.ToolTip = "4 Adults / 2 Adults 2 Children";
                //    }
                //    else if (seq == "58")
                //    {
                //        imgPax.Text = strImgAdult2AndChild2Orange;
                //        imgPax.ToolTip = "2 Adults 2 Children";
                //    }
                //    else if (seq == "63")
                //    {
                //        imgPax.Text = strImgAdult3Orange;
                //        imgPax.ToolTip = "3 Adults";
                //    }
                //    else
                //    {
                //        imgPax.Text = strImgAdult2Orange;
                //        imgPax.ToolTip = "2 Adults";
                //    }
                //}
                //else if (propertyName.ToLower().Equals(EProperty.Banyu_Biru.ToString().ToLower()))
                //{
                //    if (seq == "49")
                //    {
                //        imgPax.Text = strImgAdult6OrAdult4AndChild2Blue;
                //        imgPax.ToolTip = "6 Adults / 4 Adults + 2 Children";
                //    }
                //    else if (seq == "53" || seq == "54" || seq == "57" || seq == "60")
                //    {
                //        imgPax.Text = strImgAdult4OrAdult2AndChild2;
                //        imgPax.ToolTip = "4 Adults / 2 Adults 2 Children";
                //    }
                //    else if (seq == "58")
                //    {
                //        imgPax.Text = strImgAdult2AndChild2;
                //        imgPax.ToolTip = "2 Adults 2 Children";
                //    }
                //    else if (seq == "63")
                //    {
                //        imgPax.Text = strImgAdult3;
                //        imgPax.ToolTip = "3 Adults";
                //    }
                //    else
                //    {
                //        imgPax.Text = strImgAdult2;
                //        imgPax.ToolTip = "2 Adults";
                //    }
                //}

                return imgPax;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetRoomTypePaxTextByRoomTypeSeq(string seq)
        {

            try
            {
                string strRetrunValue = "";

                //if (propertyName.ToLower().Equals(EProperty.DownTown.ToString().ToLower()))
                //{
                if (seq == "53" || seq == "54" || seq == "57" || seq == "60")
                {
                    strRetrunValue = "4 Adults / 2 Adults 2 Children";
                }
                else if (seq == "58")
                {
                    strRetrunValue = "2 Adults 2 Children";
                }
                else if (seq == "59")
                {
                    strRetrunValue = "3 Adults";
                }
                else if (seq == "63")
                {
                    strRetrunValue = "2 Adults";
                }
                else
                {
                    strRetrunValue = "2 Adults";
                }
                //}
                //else if (propertyName.ToLower().Equals(EProperty.Sentosa.ToString().ToLower()))
                //{
                //    if (seq == "47")
                //    {
                //        strRetrunValue = "4 Adults / 2 Adults 2 Children";
                //    }
                //    else if (seq == "48")
                //    {
                //        strRetrunValue = "2 Adults";
                //    }
                //    else
                //    {
                //        strRetrunValue = "2 Adults";
                //    }
                //}
                //else if (propertyName.ToLower().Equals(EProperty.Banyu_Biru.ToString().ToLower()))
                //{
                //    if (seq == "49")
                //    {
                //        strRetrunValue = "6 Adults / 4 Adults + 2 Children";
                //    }
                //    else if (seq == "53" || seq == "54" || seq == "57" || seq == "60")
                //    {
                //        strRetrunValue = "4 Adults / 2 Adults 2 Children";
                //    }
                //    else if (seq == "58")
                //    {
                //        strRetrunValue = "2 Adults 2 Children";
                //    }
                //    else if (seq == "63")
                //    {
                //        strRetrunValue = "3 Adults";
                //    }
                //    else
                //    {
                //        strRetrunValue = "2 Adults";
                //    }
                //}

                return strRetrunValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected Guid GetCityKey(string City)
        {
            return _registrationdalRepository.GetCityKey(City);

        }
        protected string GetNationality(string CountryKey)
        {
            return _registrationdalRepository.GetNationality(CountryKey);

        }
        protected Guid GetReservationKey(string docno)
        {
            return _registrationdalRepository.GetReservationKey(docno);

        }
        protected string GetGuestName(string GuestKey)
        {
            return _registrationdalRepository.GetGuestName(GuestKey);

        }
    }



}
