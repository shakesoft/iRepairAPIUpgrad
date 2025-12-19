using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.IRepairIAppService;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{

    public class MainAppService : BEZNgCoreAppServiceBase, IMainAppService
    {

        private readonly IRepository<Staff, Guid> _staffRepository;
        private readonly IRepository<MTechnician, int> _technicianRepository;
        private readonly IMworkorderdalRepository _mwordorderdalRepository;
        //  private readonly IRepository<History, Guid> _historyRepository;
        public MainAppService(
            IRepository<Staff, Guid> staffRepository,
            IRepository<MTechnician, int> technicianRepository,
            IMworkorderdalRepository mwordorderdalRepository)
        //  IRepository<History, Guid> historyRepository)
        {
            _staffRepository = staffRepository;
            _technicianRepository = technicianRepository;
            _mwordorderdalRepository = mwordorderdalRepository;
            // _historyRepository = historyRepository;
        }
        //public int GetDashRoomByMaidKey()
        //{
        //    var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
        //    var staffmaidkey = _staffRepository.GetAll().Where(x => x.Id == user.Result.StaffKey).Select(x => x.MaidKey).FirstOrDefault();
        //    string maidKey = staffmaidkey.ToString();
        //    string maidStatusKey = "", roomStatusKey = "", floor = "";
        //    DateTime dtBusinessDate = DateTime.Now;
        //    Task<List<GetDashRoomByMaidKeyOutput>> dt = null;
        //    if (!string.IsNullOrEmpty(maidKey))
        //    {
        //        Task<List<GetMaidStatusOutput>> lstbd = _mwordorderdalRepository.GetBusinessDate();
        //        dtBusinessDate = lstbd.Result[0].BusinessDate;
        //        dt = _mwordorderdalRepository.GetRoomByMaidKey(dtBusinessDate, maidKey, maidStatusKey, roomStatusKey, floor, "");
        //        //  dt= _maidstatusRepository.GetRoomByMaidKey(dtBusinessDate, maidKey, maidStatusKey, roomStatusKey, floor, "");
        //    }
        //    return dt.Result.Count;
        //}
        [HttpGet]
        public ListResultDto<GetDashboardViewData> GetDashboardViewData()
        {
            List<GetDashboardViewData> lst = new List<GetDashboardViewData>();
            GetDashboardViewData a = new GetDashboardViewData();
            a.GetAssignedTasks = GetAssignedTasksAsync().Result;
            a.GetUnassignedTechWorkOrderCount = GetUnassignedTechWorkOrderCount();
            lst.Add(a);
            return new ListResultDto<GetDashboardViewData>(lst);
        }
        [HttpGet]
        public async Task<int> GetAssignedTasksAsync()
        {
            int litMyTaskCount = 0;
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
                var TechnicianID = (from s in _staffRepository.GetAll()
                             .Where(s => s.Id == user.StaffKey).DefaultIfEmpty()
                                    join t in _technicianRepository.GetAll() on s.TechnicianKey equals t.TechnicianKey into tmp
                                    from final in tmp.DefaultIfEmpty()
                                    select new { final.Id }).FirstOrDefault();
                DataTable dt = _mwordorderdalRepository.GetWOByTechnician(TechnicianID.Id, -1, "ALL");
                litMyTaskCount = dt.Rows.Count;
            }
            return litMyTaskCount;
        }
        [HttpGet]
        public int GetUnassignedTechWorkOrderCount()
        {
            int litMyTaskCount = 0;
            DataTable dt = _mwordorderdalRepository.GetUnassignedTechWorkOrderCount();
            litMyTaskCount = Convert.ToInt32(dt.Rows[0]["noOfWork"]);
            return litMyTaskCount;
        }
        [HttpGet]
        public async Task<PagedResultDto<HistoryDto>> GetTodayHistory()
        {
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
                Guid staffkey = user.StaffKey;

                List<HistoryDto> dt = new List<HistoryDto>();

                dt = _mwordorderdalRepository.GetTodayHistory(staffkey);

                var Count = dt.Count;

                return new PagedResultDto<HistoryDto>(
                Count,
                dt
                );
            }
        }

        //public async Task<PagedResultDto<GetHistoryForViewDto>> GetTodayHistory()
        //{
        //    var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
        //    Guid staff = user.Result.StaffKey;
        //    var filteredHistory = _historyRepository.GetAll().Where(x => x.ModuleName.Equals("iRepair") && x.StaffKey == staff && x.ChangedDate >= DateTime.Now.AddHours(-24));

        //    var pagedAndFilteredHistory = from s in filteredHistory
        //                                  orderby s.ChangedDate descending
        //                                  select s;
        //    var totalCount = filteredHistory.Count();

        //    var dbList = pagedAndFilteredHistory.ToList();
        //    var results = new List<GetHistoryForViewDto>();

        //    foreach (var o in dbList)
        //    {
        //        var res = new GetHistoryForViewDto()
        //        {
        //            History = new HistoryDto
        //            {
        //               HistoryDes= GetDateTimeToDisplay(o.ChangedDate)+ " =>"+ o.Detail

        //            }
        //        };

        //        results.Add(res);
        //    }

        //    return new PagedResultDto<GetHistoryForViewDto>(
        //        totalCount,
        //        results
        //    );

        //}
        //private static string GetDateTimeToDisplay(object inputDate)
        //{
        //    try
        //    {
        //        string strReturnValue = "";
        //        if (inputDate != null && inputDate.ToString() != "")
        //        {
        //            strReturnValue = Convert.ToDateTime(inputDate).ToString("dd/MM/yyyy HH:mm:ss");
        //        }
        //        return strReturnValue;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
