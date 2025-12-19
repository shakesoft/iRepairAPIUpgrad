using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using BEZNgCore.Authorization.IrepairDal;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.IrepairAppService.DAL;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{
    public class HouskeepingGuestRequestAppService : BEZNgCoreAppServiceBase
    {
        private readonly ICommondalRepository _commondalRepository;

       
        public HouskeepingGuestRequestAppService(
             ICommondalRepository commondalRepository)
        {
            _commondalRepository = commondalRepository;
        }
        [HttpGet]
        public ListResultDto<GeHGuestRequestViewData> GetHGuestRequestViewData()
        {
           
            List<GeHGuestRequestViewData> Alllst = new List<GeHGuestRequestViewData>();
            GeHGuestRequestViewData a = new GeHGuestRequestViewData();
            List<GuestRequestStatusOutput> sl = new List<GuestRequestStatusOutput>();
            a.GuestRequestStatus = BindGuestRequestStatusList();
            List<RequestTypeOutput> rol = new List<RequestTypeOutput>();
            RequestTypeOutput s1 = new RequestTypeOutput();
            s1.RequestTypeKey = Guid.Empty;
            s1.RequestTypeName = "ALL";
            rol.Add(s1);
            List<RequestTypeOutput> ss = GetAllRequestType();
            a.ddlRequestType = rol.Concat(ss).ToList();
            DateTime dtBusinessDate = DateTime.Today;
            a.toDate = dtBusinessDate;
            a.requestDate = dtBusinessDate.AddDays(-7);
            Alllst.Add(a);
            return new ListResultDto<GeHGuestRequestViewData>(Alllst);
        }
        private List<GuestRequestStatusOutput> BindGuestRequestStatusList()
        {
            List<GuestRequestStatusOutput> lst = new List<GuestRequestStatusOutput>();
            lst.AddRange(new List<GuestRequestStatusOutput>
            {
            new GuestRequestStatusOutput("-2","All","All" ),
            new GuestRequestStatusOutput("-1","Cancelled request","CANCELLED" ),
            new GuestRequestStatusOutput("1","Open request","OPEN" ),
            new GuestRequestStatusOutput("2","In Progress request", "IN PROGRESS"),
            new GuestRequestStatusOutput("10","Completed request","COMPLETED" )
            });

            return lst;
        }
        protected List<RequestTypeOutput> GetAllRequestType()
        {

            return _commondalRepository.GetAllRequestType();
        }

        [HttpGet]
        public async Task<ListResultDto<HRequestGuestOutput>> GetGuestRequestDataBindGrid(DateTime? requestDate = null, DateTime? toDate = null, int status = -2, string requestTypeKey = "")
        {
            List<HRequestGuestOutput> Alllst = new List<HRequestGuestOutput>();
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

                DataTable dtr = _commondalRepository.GetReservationRequestByGuestKey(defrequestDate, deftoDate,status,requestTypeKey);
                if (dtr.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtr.Rows)
                    {
                        DateTime? nullable;
                        HRequestGuestOutput o = new HRequestGuestOutput();
                        o.ReservationRequestKey = (!DBNull.Value.Equals(dr["ReservationRequestKey"])) ? (!string.IsNullOrEmpty(dr["ReservationRequestKey"].ToString()) ? new Guid(dr["ReservationRequestKey"].ToString()) : Guid.Empty) : Guid.Empty;
                        o.ReservationRequestID = !DBNull.Value.Equals(dr["ReservationRequestID"]) ? dr["ReservationRequestID"].ToString() : "";
                        o.Name = !DBNull.Value.Equals(dr["Name"]) ? dr["Name"].ToString() : "";
                        o.DocNo = !DBNull.Value.Equals(dr["DocNo"]) ? dr["DocNo"].ToString() : "";
                        o.StatusCode = !DBNull.Value.Equals(dr["StatusDesc"]) ? dr["StatusDesc"].ToString() : "";
                        o.StatusDesc = o.StatusCode == "Cancelled request" ? "CANCELLED" : o.StatusCode == "Open request" ? "OPEN" : o.StatusCode == "In Progress request" ? "IN PROGRESS" : "COMPLETED";
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


            return new ListResultDto<HRequestGuestOutput>(Alllst);
        }

        [HttpGet]
        public async Task<ListResultDto<HRequestGuestDataEntryOutput>> GetRequestGuestDataEntry(string ReservationRequestKey = null)
        {
            List<HRequestGuestDataEntryOutput> Alllst = new List<HRequestGuestDataEntryOutput>();
            HRequestGuestDataEntryOutput a = new HRequestGuestDataEntryOutput();
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
                HRequestGuestDataEntryViewData b = new HRequestGuestDataEntryViewData();
                List<GuestRequestStatusOutput> sl = new List<GuestRequestStatusOutput>();
                b.GuestRequestStatus = GuestBoxBindGuestRequestStatusList();
                List<RequestTypeOutput> rol = new List<RequestTypeOutput>();
                b.ddlRequestType = GetAllRequestType();
                a.RequestGuestDataEntryDropdown = b;
                
                    HReservationRequestOutput op = new HReservationRequestOutput();
                    op = _commondalRepository.GetReservationRequestNew(ReservationRequestKey);
                    a.ReservationRequestOutput = op;
                
                Alllst.Add(a);
            }


            return new ListResultDto<HRequestGuestDataEntryOutput>(Alllst);
        }
        private List<GuestRequestStatusOutput> GuestBoxBindGuestRequestStatusList()
        {
            List<GuestRequestStatusOutput> lst = new List<GuestRequestStatusOutput>();
            lst.AddRange(new List<GuestRequestStatusOutput>
            {
            new GuestRequestStatusOutput("-1","Cancelled request","CANCELLED" ),
            new GuestRequestStatusOutput("1","Open request","OPEN" ),
            new GuestRequestStatusOutput("2","In Progress request", "IN PROGRESS"),
            new GuestRequestStatusOutput("10","Completed request","COMPLETED" )
            });

            return lst;
        }
        //protected string GetGuestName(string GuestKey)
        //{
        //    return _commondalRepository.GetGuestName(GuestKey);

        //}UpdateAutoComplete

        [HttpPost]
        public async Task<string> UpdateRequestGuest(HReservationRequestInput a)
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
                      //  Guid key = Guid.Parse(a.ReservationRequestKey.ToString());
                        
                        a.ModifiedBy = user.StaffKey;
                        //if (AbpSession.TenantId != null)
                        //{
                        //    a.TenantId = (int)AbpSession.TenantId;
                        //}
                        int success = _commondalRepository.UpdateRequestGuest(a);
                    
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

        [HttpPost]
        public async Task<string> CompleteRequestGuest(string ReservationRequestKey = null)

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



                    HReservationRequestInput a=new HReservationRequestInput();
                    Guid key = Guid.Parse(ReservationRequestKey.ToString());
                    a.ReservationRequestKey = key;
                    a.ModifiedBy = user.StaffKey;
                    //if (AbpSession.TenantId != null)
                    //{
                    //    a.TenantId = (int)AbpSession.TenantId;
                    //}
                    int success = _commondalRepository.UpdateAutoComplete(a);

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


            }
            return message;
        }

        [HttpPost]
        public async Task<string> CancelledRequestGuest(string ReservationRequestKey = null)

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



                    HReservationRequestInput a = new HReservationRequestInput();
                    Guid key = Guid.Parse(ReservationRequestKey.ToString());
                    a.ReservationRequestKey = key;
                    a.ModifiedBy = user.StaffKey;
                    a.Status = -1;
                    //if (AbpSession.TenantId != null)
                    //{
                    //    a.TenantId = (int)AbpSession.TenantId;
                    //}
                    int success = _commondalRepository.UpdateGuestRequestStatus(a);

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


            }
            return message;
        }
        [HttpPost]
        public async Task<string> OpenRequestGuest(string ReservationRequestKey = null)

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



                    HReservationRequestInput a = new HReservationRequestInput();
                    Guid key = Guid.Parse(ReservationRequestKey.ToString());
                    a.ReservationRequestKey = key;
                    a.ModifiedBy = user.StaffKey;
                    a.Status = 1;
                    //if (AbpSession.TenantId != null)
                    //{
                    //    a.TenantId = (int)AbpSession.TenantId;
                    //}
                    int success = _commondalRepository.UpdateGuestRequestStatus(a);

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


            }
            return message;
        }
        [HttpPost]
        public async Task<string> ProgressRequestGuest(string ReservationRequestKey = null)

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



                    HReservationRequestInput a = new HReservationRequestInput();
                    Guid key = Guid.Parse(ReservationRequestKey.ToString());
                    a.ReservationRequestKey = key;
                    a.ModifiedBy = user.StaffKey;
                    a.Status = 2;
                    //if (AbpSession.TenantId != null)
                    //{
                    //    a.TenantId = (int)AbpSession.TenantId;
                    //}
                    int success = _commondalRepository.UpdateGuestRequestStatus(a);

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


            }
            return message;
        }
        [HttpPost]
        public async Task<string> OtherStatusRequestGuest(string ReservationRequestKey = null,string status="1")

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



                    HReservationRequestInput a = new HReservationRequestInput();
                    Guid key = Guid.Parse(ReservationRequestKey.ToString());
                    a.ReservationRequestKey = key;
                    a.ModifiedBy = user.StaffKey;
                    a.Status =Convert.ToInt32(status);
                    //if (AbpSession.TenantId != null)
                    //{
                    //    a.TenantId = (int)AbpSession.TenantId;
                    //}
                    int success = _commondalRepository.UpdateGuestRequestStatus(a);

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


            }
            return message;
        }
        [HttpPost]
        public async Task<string> SaveRequestGuest(HSReservationRequestInput a)
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
                    //  Guid key = Guid.Parse(a.ReservationRequestKey.ToString());

                    a.ModifiedBy = user.StaffKey;
                    //if (AbpSession.TenantId != null)
                    //{
                    //    a.TenantId = (int)AbpSession.TenantId;
                    //}
                    int success = _commondalRepository.SaveRequestGuest(a);

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

        [HttpGet]
        public async Task <ListResultDto<GetDashboardGuestViewData>> GuestRequestCount()
        {
            List<GetDashboardGuestViewData> lst = new List<GetDashboardGuestViewData>();
            GetDashboardGuestViewData a = new GetDashboardGuestViewData();
            
            
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
                a.GuestRequest = _commondalRepository.GetOpenProgressCount();
                lst.Add(a);
            }
            return new ListResultDto<GetDashboardGuestViewData>(lst);
        }
    }
}
