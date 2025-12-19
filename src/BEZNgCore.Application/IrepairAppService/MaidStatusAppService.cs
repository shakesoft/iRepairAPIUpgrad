using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using BEZNgCore.Authorization.IrepairDal;
using BEZNgCore.Common;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{
    public class MaidStatusAppService : BEZNgCoreAppServiceBase
    {

        private readonly IMaidStatusRepository _maidstatusRepository;
        // private readonly IRoomRepository _roomRepository;
        private readonly IRepository<Staff, Guid> _staffRepository;
        private readonly ICommondalRepository _commondalRepository;
        public MaidStatusAppService(
            IMaidStatusRepository maidstatusRepository,
            // IRoomRepository roomRepository,
            IRepository<Staff, Guid> staffRepository,
            ICommondalRepository commondalRepository)
        {

            _maidstatusRepository = maidstatusRepository;
            //  _roomRepository = roomRepository;
            _staffRepository = staffRepository;
            _commondalRepository = commondalRepository;
        }
        [HttpGet]
        public ListResultDto<GetDashboardICViewData> GetDashboardViewData()
        {
            List<GetDashboardICViewData> lst = new List<GetDashboardICViewData>();
            GetDashboardICViewData a = new GetDashboardICViewData();
            a.RoomToInspect = GetRoomToInspect();
            a.AssignedTask = GetDashRoomByMaidKeyAsync().Result;
            lst.Add(a);
            return new ListResultDto<GetDashboardICViewData>(lst);
        }
        [HttpGet]
        public ListResultDto<NGetDashboardICViewData> GetDashboardViewDataNew()
        {
            List<NGetDashboardICViewData> lst = new List<NGetDashboardICViewData>();
            NGetDashboardICViewData a = new NGetDashboardICViewData();
            a.RoomToInspect = GetRoomToInspect();
            a.AssignedTask = GetDashRoomByMaidKeyAsync().Result;
            a.GuestRequest = _commondalRepository.GetOpenProgressCount();
            lst.Add(a);
            return new ListResultDto<NGetDashboardICViewData>(lst);
        }
        
        [HttpGet]
        public int GetRoomToInspect()
        {
            DateTime dtBusinessDate = DateTime.Now;
            Task<List<GetMaidStatusOutput>> lstmk = _maidstatusRepository.GetMaidStatusKeyByStatusAsync(CommomData.HouseKeepingMaidStatusInspectionRequired);
            string maidStatusKey = lstmk.Result[0].MaidStatusKey;
            Task<List<GetDashRoomByMaidStatusKeyOutput>> dt = null;
            if (!string.IsNullOrEmpty(maidStatusKey))
            {
                Task<List<GetMaidStatusOutput>> lstbd = _maidstatusRepository.GetBusinessDate();
                dtBusinessDate = lstbd.Result[0].BusinessDate;
                dt = _maidstatusRepository.GetDashRoomByMaidStatusKey(dtBusinessDate, maidStatusKey, "", "");
            }
            return dt.Result.Count;

        }

        public async Task<int> GetDashRoomByMaidKeyAsync()
        {
            Task<List<GetDashRoomByMaidKeyOutput>> dt = null;
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
                // var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                var staffmaidkey = _staffRepository.GetAll().Where(x => x.Id == user.StaffKey).Select(x => x.MaidKey).FirstOrDefault();
                string maidKey = staffmaidkey.ToString();
                string maidStatusKey = "", roomStatusKey = "", floor = "";
                DateTime dtBusinessDate = DateTime.Now;

                if (!string.IsNullOrEmpty(maidKey))
                {
                    Task<List<GetMaidStatusOutput>> lstbd = _maidstatusRepository.GetBusinessDate();
                    dtBusinessDate = lstbd.Result[0].BusinessDate;
                    dt = _maidstatusRepository.GetRoomByMaidKey(dtBusinessDate, maidKey, maidStatusKey, roomStatusKey, floor, "");
                    //  dt= _maidstatusRepository.GetRoomByMaidKey(dtBusinessDate, maidKey, maidStatusKey, roomStatusKey, floor, "");
                }
            }
            return dt.Result.Count;
        }
    }
}
