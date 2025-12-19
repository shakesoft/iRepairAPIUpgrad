using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using BEZNgCore.Authorization.IrepairDal;
using BEZNgCore.Common;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.IrepairAppService.DAL;
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
    public class SupervisorModeAppService : BEZNgCoreAppServiceBase
    {
        private readonly IRepository<RoomStatus, Guid> _roomstatusRepository;
        private readonly IRepository<Room, Guid> _roomRepository;
       // private readonly IRepository<Control, Guid> _controlRepository;
        private readonly IRepository<MaidStatus, Guid> _maidstatusRepository;
        private readonly IRepository<Maid, Guid> _maidRepository;
        private readonly IRepository<Staff, Guid> _staffRepository;
        //private readonly MaidStatusRepository _maidStatusdalRepository;
        private readonly RoomRepository _roomdalRepository;
        private readonly IMaidStatusRepository _imaidstatusRepository;
        private readonly IRoomRepository _iroomRepository;
        RoomStatusDAL dalroomstatus;
        RoomDAL dalroom;
        public static string HouseKeepingMaidStatusInspectionRequired = "Inspection Required";
        public static string c = "Clean";
        public static string d = "Dirty";
        public SupervisorModeAppService(
            IRepository<RoomStatus, Guid> roomstatusRepository,
            IRepository<Room, Guid> roomRepository,
            //IRepository<Control, Guid> controlRepository,
            IRepository<MaidStatus, Guid> maidstatusRepository,
            IRepository<Maid, Guid> maidRepository,
            //MaidStatusRepository maidStatusdalRepository,
            IRepository<Staff, Guid> staffRepository,
            IMaidStatusRepository imaidstatusRepository,
            IRoomRepository iroomRepository,
            RoomRepository roomdalRepository
            )
        {
            _roomstatusRepository = roomstatusRepository;
            _roomRepository = roomRepository;
           // _controlRepository = controlRepository;
            _maidstatusRepository = maidstatusRepository;
            _maidRepository = maidRepository;
            // _maidStatusdalRepository = maidStatusdalRepository;
            _staffRepository = staffRepository;
            _imaidstatusRepository = imaidstatusRepository;
            _roomdalRepository = roomdalRepository;
            dalroomstatus = new RoomStatusDAL(_roomstatusRepository);
            _iroomRepository = iroomRepository;
            dalroom = new RoomDAL(_roomRepository);
        }
        [HttpGet]
        public ListResultDto<SupervisorModeViewData> GetSupervisorModeViewData()
        {

            List<SupervisorModeViewData> Alllst = new List<SupervisorModeViewData>();
            SupervisorModeViewData a = new SupervisorModeViewData();
            a.GetHotelFloors = dalroom.BindHotelFloorList();
            a.GetRoomStatuss = dalroomstatus.GetAllRoomStatus();
            //a.StaffList = _maidRepository.GetAll().Where(x => x.Active == 1)
            //           .OrderBy(x => x.Name)
            //           .Select(x => new DDLAttendantOutput
            //           {
            //               MaidKey = x.Id,
            //               Name = x.Name
            //           })
            //                            .ToList();
            a.StaffList = (from m in _maidRepository.GetAll()
                           join s in _staffRepository.GetAll()
                           on m.Id equals s.MaidKey
                           where (m.Active == 1 && s.Active == 1 && s.AnywhereAccess == 1 && s.PIN != null)
                           orderby m.Name
                           select new DDLAttendantOutput
                           {
                               MaidKey = m.Id,
                               Name = m.Name
                           }).ToList();
            //a.MaidStatusCount = GetBindMaidStatusListCount();
            a.MaidStatusCount = GetBindMaidStatusListCountQuery().Result;
            Alllst.Add(a);
            return new ListResultDto<SupervisorModeViewData>(Alllst);
        }

        public ListResultDto<SupervisorModeViewData> GetSupervisorModeViewDataIncludeAll()
        {

            List<SupervisorModeViewData> Alllst = new List<SupervisorModeViewData>();
            SupervisorModeViewData a = new SupervisorModeViewData();
            List<GetHotelFloor> f = new List<GetHotelFloor>();
            GetHotelFloor f1 = new GetHotelFloor();
            f1.Floor = "0";
            f1.btnFloor = "ALL";
            f.Add(f1);
            var hf = _roomRepository.GetAll().GroupBy(x => x.Floor)
                       .OrderBy(x => x.Key)
                       .Select(x => new GetHotelFloor
                       {
                           btnFloor = CommomData.GetNumber(x.Key.ToString()),
                           Floor = x.Key.ToString()
                       });
            a.GetHotelFloors = f.Concat(hf).ToList();
            List<GetRoomStatus> r = new List<GetRoomStatus>();
            GetRoomStatus r1 = new GetRoomStatus();
            r1.RoomStatusKey = Guid.Empty;
            r1.RoomStatus = "ALL";
            r.Add(r1);
            var rs = _roomstatusRepository.GetAll().OrderByDescending(s => s.Seq)
                                        .Select(s => new GetRoomStatus
                                        {
                                            RoomStatusKey = s.Id,
                                            RoomStatus = s.RoomStatusName
                                        });
            a.GetRoomStatuss = r.Concat(rs).ToList();
            List<DDLAttendantOutput> sl = new List<DDLAttendantOutput>();
            DDLAttendantOutput s1 = new DDLAttendantOutput();
            s1.MaidKey = Guid.Empty;
            s1.Name = "ALL";
            sl.Add(s1);
            var ss = (from m in _maidRepository.GetAll()
                      join s in _staffRepository.GetAll()
                      on m.Id equals s.MaidKey
                      where (m.Active == 1 && s.Active == 1 && s.AnywhereAccess == 1 && s.PIN != null)
                      orderby m.Name
                      select new DDLAttendantOutput
                      {
                          MaidKey = m.Id,
                          Name = m.Name
                      });
            a.StaffList = sl.Concat(ss).ToList();

            a.MaidStatusCount = GetBindMaidStatusListCountQuery().Result;
            Alllst.Add(a);
            return new ListResultDto<SupervisorModeViewData>(Alllst);
        }
        public async Task<List<MaidStatusListOutPut>> GetBindMaidStatusListCountQuery()
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
                var staffmaidkey = _staffRepository.GetAll().Where(x => x.Id == user.StaffKey).Select(x => x.MaidKey).FirstOrDefault();
                string maidKey = staffmaidkey.ToString();
                DateTime dtBusinessDate = DateTime.Now;
                if (!string.IsNullOrEmpty(maidKey))
                {
                    Task<List<GetMaidStatusOutput>> lstbd = _imaidstatusRepository.GetBusinessDate();
                    dtBusinessDate = lstbd.Result[0].BusinessDate;
                }
                string floorno = "";
                return _roomdalRepository.BindMaidStatusListCountSup(dtBusinessDate, maidKey, floorno);
            }

        }


        //public int GetBindTotoalRoomCount()//BindTotoalRoomCount();
        //{
        //    Task<List<GetDashRoomByMaidStatusKeyOutput>> dt = null;
        //    DateTime dtBusinessDate = DateTime.Now;
        //    var lst = _maidstatusRepository.GetAll().Where(x => x.MaidStatusName == HouseKeepingMaidStatusInspectionRequired).Select(x => x.Id).ToList();

        //    if (lst.Count > 0)
        //    {
        //        string maidStatusKey = lst[0].ToString();
        //        var d = _controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefault();
        //        if (d != null)
        //        {
        //            dtBusinessDate = d.Value;
        //        }
        //        string maidKey = ""; string floorNo = ""; string roomStatusKey = "";
        //        dt = _maidStatusdalRepository.GetDashRoomByMaidStatusKey(dtBusinessDate, maidStatusKey, maidKey, floorNo, roomStatusKey);
        //    }
        //    return dt.Result.Count;

        //}
        public async Task<List<MaidStatusListOutPut>> GetBindMaidStatusListCount()//BindMaidStatusListCount();
        {
            var lst = _maidstatusRepository.GetAll().Where(x => x.MaidStatusName == HouseKeepingMaidStatusInspectionRequired).Select(x => x.Id).ToList();
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
                Guid maidkey = new Guid(maidKey);
                List<MaidStatusListOutPut> MaidStatus = new List<MaidStatusListOutPut>();

                var MaidStatus1 = _roomRepository.GetAll().Where(x => x.MaidKey == maidkey)
                           .GroupBy(x => x.MaidKey)
                           .Select(x => new MaidStatusListOutPut
                           {
                               MaidStatus = "ALL",
                               RoomCount = x.Count()
                           }).ToList();
                var MaidStatus2 = (from p in _roomRepository.GetAll()
                                   where (p.MaidKey == maidkey)
                                   join e in _maidstatusRepository.GetAll() on p.MaidStatusKey equals e.Id into t
                                   from rt in t.DefaultIfEmpty()
                                   where (rt.MaidStatusName.Contains(c) || rt.MaidStatusName.Contains(d))
                                   group new { p.MaidStatusKey }
                                   by new { rt.MaidStatusName } into g
                                   orderby g.Key.MaidStatusName
                                   select new MaidStatusListOutPut
                                   {
                                       MaidStatus = g.Key.MaidStatusName,
                                       RoomCount = g.Count()

                                   }).ToList();

                MaidStatus = (from word in MaidStatus1 select word).Concat
                             (from word in MaidStatus2 select word).ToList();

                return MaidStatus;
            }
        }
        //public async Task<PagedResultDto<GetDashRoomByMaidStatusKeyOutput>> GetBindGrid(string SelectedRoomStatus = "ALL", string FloorNo = "0", string SelectedMaidStatus = "ALL", string MaidKey = "")
        //{
        //    List<GetDashRoomByMaidStatusKeyOutput> dt = new List<GetDashRoomByMaidStatusKeyOutput>();

        //    string strMaidStatusKey = "";
        //    if (SelectedMaidStatus != "ALL")
        //    {
        //        strMaidStatusKey = _maidstatusRepository.GetAll().Where(x => x.MaidStatusName == SelectedMaidStatus).Select(x => x.Id).FirstOrDefault().ToString();
        //    }
        //    string strRoomStatusKey = "";
        //    if (SelectedRoomStatus != "ALL")
        //    {
        //        strRoomStatusKey = _roomstatusRepository.GetAll().Where(x => x.RoomStatusName == SelectedRoomStatus).Select(x => x.Id).FirstOrDefault().ToString();
        //    }
        //    DateTime dtBusinessDate = DateTime.Now;
        //    var lst = _maidstatusRepository.GetAll().Where(x => x.MaidStatusName == HouseKeepingMaidStatusInspectionRequired).Select(x => x.Id).ToList();

        //    if (lst.Count > 0)
        //    {
        //        string maidStatusKey = lst[0].ToString();
        //        var d = _controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefault();
        //        if (d != null)
        //        {
        //            dtBusinessDate = d.Value;
        //        }

        //        dt = await _roomdalRepository.GetSupervisorRoomByMaidStatusKey(dtBusinessDate, MaidKey, FloorNo, strMaidStatusKey, strRoomStatusKey);
        //    }

        //    var Count = dt.Count;

        //return new PagedResultDto<GetDashRoomByMaidStatusKeyOutput>(
        //    Count,
        //    dt
        //);

        //}
        public async Task<PagedResultDto<GetSupervisorModeOutput>> GetBindGrid(string SelectedRoomStatus = "ALL", string FloorNo = "0", string MaidKey = "", string SelectedMaidStatus = "ALL")
        {
            string strMaidStatusKey = "";
            if (SelectedMaidStatus != "ALL")
            {
                strMaidStatusKey = _maidstatusRepository.GetAll().Where(x => x.MaidStatusName == SelectedMaidStatus).Select(x => x.Id).FirstOrDefault().ToString();
            }
            string strRoomStatusKey = "";
            if (SelectedRoomStatus != "ALL")
            {
                strRoomStatusKey = _roomstatusRepository.GetAll().Where(x => x.RoomStatusName == SelectedRoomStatus).Select(x => x.Id).FirstOrDefault().ToString();
            }
            DateTime dtBusinessDate = DateTime.Now;

            //var d = _controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefault();
            //if (d != null)
            //{
            //    dtBusinessDate = d.Value;
            //}
            List<GetSupervisorModeOutput> dt = new List<GetSupervisorModeOutput>();
            dt = await _roomdalRepository.GetSupervisorModeBindGrid(dtBusinessDate, MaidKey, FloorNo, strMaidStatusKey, strRoomStatusKey);
            var Count = dt.Count;

            return new PagedResultDto<GetSupervisorModeOutput>(
            Count,
            dt
            //DataTable dtRoom = BLL_Room.GetSupervisorRoomByMaidStatusKey(MaidKey, FloorNo, strMaidStatusKey, strRoomStatusKey);
            //List<GetDashRoomByMaidStatusKeyOutput> dt = new List<GetDashRoomByMaidStatusKeyOutput>();
            //string strRoomStatusKey = "";
            //if (SelectedRoomStatus != "ALL")
            //{
            //    strRoomStatusKey = _roomstatusRepository.GetAll().Where(x => x.RoomStatusName == SelectedRoomStatus).Select(x => x.Id).FirstOrDefault().ToString();
            //}
            //DateTime dtBusinessDate = DateTime.Now;
            //var lst = _maidstatusRepository.GetAll().Where(x => x.MaidStatusName == CommomData.HouseKeepingMaidStatusInspectionRequired).Select(x => x.Id).ToList();

            //if (lst.Count > 0)
            //{
            //    string maidStatusKey = lst[0].ToString();
            //    var d = _controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefault();
            //    if (d != null)
            //    {
            //        dtBusinessDate = d.Value;
            //    }

            //    dt = await _roomdalRepository.GetRoomByMaidStatusKey(dtBusinessDate, maidStatusKey, MaidKey, FloorNo, strRoomStatusKey);

            //}

            //var Count = dt.Count;

            //return new PagedResultDto<GetDashRoomByMaidStatusKeyOutput>(
            //Count,
            //dt
        );

        }

        public async Task<PagedResultDto<GetSupervisorModePaginOutput>> GetBindGridPagin(string SelectedRoomStatus = "ALL", string FloorNo = "0", string MaidKey = "", string SelectedMaidStatus = "ALL", int currentPage = 1, int pageSize = 10)
        {
            List<GetSupervisorModePaginOutput> Alllst = new List<GetSupervisorModePaginOutput>();
            GetSupervisorModePaginOutput a = new GetSupervisorModePaginOutput();
            string strMaidStatusKey = "";
            if (SelectedMaidStatus != "ALL")
            {
                strMaidStatusKey = _maidstatusRepository.GetAll().Where(x => x.MaidStatusName == SelectedMaidStatus).Select(x => x.Id).FirstOrDefault().ToString();
            }
            string strRoomStatusKey = "";
            if (SelectedRoomStatus != "ALL")
            {
                strRoomStatusKey = _roomstatusRepository.GetAll().Where(x => x.RoomStatusName == SelectedRoomStatus).Select(x => x.Id).FirstOrDefault().ToString();
            }
            DateTime dtBusinessDate = DateTime.Now;

            //var d = _controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefault();
            //if (d != null)
            //{
            //    dtBusinessDate = d.Value;
            //}
            List<GetSupervisorModeOutput> dt = new List<GetSupervisorModeOutput>();
            int TotalRowCount = 0;
            dt = _roomdalRepository.GetSupervisorModeBindGridPaginate(dtBusinessDate, MaidKey, FloorNo, strMaidStatusKey, strRoomStatusKey, currentPage, pageSize, out TotalRowCount);
            //var Count = dt.Count;
            if (dt.Count > 0)
            {
                a.SupervisorModeList = dt;
                a.PaginattionList = generatePager(TotalRowCount, pageSize, currentPage);
            }
            Alllst.Add(a);
            return new PagedResultDto<GetSupervisorModePaginOutput>(
            TotalRowCount,
            Alllst

        );

        }

        private List<PageListItem> generatePager(int totalRowCount, int pageSize, int currentPage)
        {
            int totalLinkInPage = 5;
            int totalPageCount = (int)Math.Ceiling((decimal)totalRowCount / pageSize);

            int startPageLink = Math.Max(currentPage - (int)Math.Floor((decimal)totalLinkInPage / 2), 1);
            int lastPageLink = Math.Min(startPageLink + totalLinkInPage - 1, totalPageCount);

            if ((startPageLink + totalLinkInPage - 1) > totalPageCount)
            {
                lastPageLink = Math.Min(currentPage + (int)Math.Floor((decimal)totalLinkInPage / 2), totalPageCount);
                startPageLink = Math.Max(lastPageLink - totalLinkInPage + 1, 1);
            }

            List<PageListItem> pageLinkContainer = new List<PageListItem>();


            if (startPageLink != 1)
                pageLinkContainer.Add(new PageListItem("&laquo;", "1", currentPage != 1));
            for (int i = startPageLink; i <= lastPageLink; i++)
            {
                pageLinkContainer.Add(new PageListItem(i.ToString(), i.ToString(), currentPage != i));
            }
            if (lastPageLink != totalPageCount)
                pageLinkContainer.Add(new PageListItem("&raquo;", totalPageCount.ToString(), currentPage != totalPageCount));

            return pageLinkContainer;
        }
    }
}
