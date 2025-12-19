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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.OpenXmlFormats;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{
    public class MyTaskAppService : BEZNgCoreAppServiceBase
    {
        private readonly IRepository<Staff, Guid> _staffRepository;
        private readonly IRepository<GuestStatus, Guid> _gueststatusRepository;
        private readonly IRepository<Room, Guid> _roomRepository;
        private readonly IRepository<MTechnician, int> _technicianRepository;
        private readonly IMworkorderdalRepository _mwordorderdalRepository;
        private readonly IRepository<RoomStatus, Guid> _roomstatusRepository;
        private readonly IRepository<MaidStatus, Guid> _maidstatusRepository;
        private readonly IRepository<Maid, Guid> _maidRepository;
        private readonly IRepository<Control, Guid> _controlRepository;
        //private readonly IRepository<History, Guid> _historyRepository;

        private readonly MaidStatusRepository _maidStatusdalRepository;
        private readonly RoomRepository _roomdalRepository;

        //private readonly ISetupdalRepository _setupdalRepository;
        public static string mr = "Maintenance Required";
        public static string mir = "Maintenance in the Room";
        RoomStatusDAL dalroomstatus;
        RoomDAL dalroom;
        MaidStatusDAL dalmaidstatus;
        StaffDAL dalstaff;
        public MyTaskAppService(
            IRepository<Staff, Guid> staffRepository,
            IRepository<GuestStatus, Guid> gueststatusRepository,
            IRepository<Room, Guid> roomRepository,
            IRepository<MTechnician, int> technicianRepository,
            IMworkorderdalRepository mwordorderdalRepository,
            IRepository<RoomStatus, Guid> roomstatusRepository,
            IRepository<MaidStatus, Guid> maidstatusRepository,
            IRepository<Maid, Guid> maidRepository,
            IRepository<Control, Guid> controlRepository,
        //IRepository<History, Guid> historyRepository,
        MaidStatusRepository maidStatusdalRepository,
            RoomRepository roomdalRepository
            //ISetupdalRepository setupdalRepository
            )
        {
            _staffRepository = staffRepository;
            _gueststatusRepository = gueststatusRepository;
            _roomRepository = roomRepository;
            _technicianRepository = technicianRepository;
            _mwordorderdalRepository = mwordorderdalRepository;
            _roomstatusRepository = roomstatusRepository;
            _maidstatusRepository = maidstatusRepository;
            _maidRepository = maidRepository;
             _controlRepository = controlRepository;
            //_historyRepository = historyRepository;
            _maidStatusdalRepository = maidStatusdalRepository;
            _roomdalRepository = roomdalRepository;
            dalroomstatus = new RoomStatusDAL(_roomstatusRepository);
            dalroom = new RoomDAL(_roomRepository);
            dalmaidstatus = new MaidStatusDAL(_maidstatusRepository);
            dalstaff = new StaffDAL(_staffRepository);
           // _setupdalRepository = setupdalRepository;
        }
        #region irepair
        [HttpGet]
        public ListResultDto<GetMytaskIRViewData> GetMyTaskIRViewData()
        {
            List<GetMytaskIRViewData> Alllst = new List<GetMytaskIRViewData>();
            GetMytaskIRViewData a = new GetMytaskIRViewData();
            a.GetWOStatusOutput = BindWOStatusOutputAsync().Result;
            a.GetRoomStatuss = dalroomstatus.GetAllRoomStatus();
            List<DDLMPriorityOutput> sl = new List<DDLMPriorityOutput>();
            DDLMPriorityOutput s1 = new DDLMPriorityOutput();
            s1.strPriorityStatus = "0";
            s1.strPriorityDesc = "ALL";
            sl.Add(s1);
            List<DDLMPriorityOutput> ss = BindPriorityButtonList();
            a.GetPriorityStatusOutPuts = sl.Concat(ss).ToList();

            Alllst.Add(a);
            return new ListResultDto<GetMytaskIRViewData>(Alllst);
        }
        public async Task<PagedResultDto<GetMyTaskDataOutput>> GetBindGridIR(string strWOStatus = "0", string strRStatus = "ALL", string strPriority = "0")
        {
            List<GetMyTaskDataOutput> dt = new List<GetMyTaskDataOutput>();
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
                Guid TechnicianKey = _staffRepository.GetAll().Where(x => x.Id == user.StaffKey).Select(x => x.TechnicianKey.Value).FirstOrDefault();
                int TechnicalID = _technicianRepository.GetAll().Where(x => x.TechnicianKey == TechnicianKey).OrderBy(x => x.Name).Select(x => x.Id).FirstOrDefault();
                try
                {
                    dt = _mwordorderdalRepository.GetWorkOrderByTechnician(TechnicalID, Convert.ToInt32(strWOStatus), strRStatus, Convert.ToInt32(strPriority));

                    var Count = dt.Count;

                    return new PagedResultDto<GetMyTaskDataOutput>(
                    Count,
                    dt
                );
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private List<DDLMPriorityOutput> BindPriorityButtonList()
        {
            //    List<PriorityStatusOutput> lst = new List<PriorityStatusOutput>();
            //    lst.AddRange(new List<PriorityStatusOutput>
            //{
            //new PriorityStatusOutput("0","ALL" ),
            //new PriorityStatusOutput("1","Low" ),
            //new PriorityStatusOutput("2","Medium"),
            //new PriorityStatusOutput("5","High" )
            //});

            //    return lst;
            return _maidStatusdalRepository.GetDDLPriority();
            
        }

        private async Task<List<WOStatusOutput>> BindWOStatusOutputAsync()
        {
            List<WOStatusOutput> dt = new List<WOStatusOutput>();
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
                Guid TechnicianKey = _staffRepository.GetAll().Where(x => x.Id == user.StaffKey).Select(x => x.TechnicianKey.Value).FirstOrDefault();
                int TechnicalID = _technicianRepository.GetAll().Where(x => x.TechnicianKey == TechnicianKey).OrderBy(x => x.Name).Select(x => x.Id).FirstOrDefault();
                dt = _mwordorderdalRepository.GetWorkOrderStatusCountByTechnician(TechnicalID);
                return dt;
            }
        }
        #endregion
        #region iclean
        [HttpGet]
        public async Task<ListResultDto<GetMytaskViewData>> GetMyTaskViewDataAsync()
        {
            var lst = dalmaidstatus.GetMaidStatusKey(CommomData.HouseKeepingMaidStatusInspectionRequired);// _maidstatusRepository.GetAll().Where(x => x.MaidStatusName == CommomData.HouseKeepingMaidStatusInspectionRequired).Select(x => x.Id).ToList();
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
                var staffmaidkey = dalstaff.GetMaidKey(user.StaffKey);
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
                                   where (!rt.MaidStatusName.Contains(mr) && !rt.MaidStatusName.Contains(mir))
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
                List<GetMytaskViewData> Alllst = new List<GetMytaskViewData>();
                GetMytaskViewData a = new GetMytaskViewData();
                a.GetHotelFloors = dalroom.BindHotelFloorList();
                a.GetRoomStatuss = dalroomstatus.GetAllRoomStatus();
                a.MaidStatusListOutPuts = MaidStatus;
                
                a.GetGuestStatuss = _gueststatusRepository.GetAll().OrderBy(s => s.Sort)
                           .Select(s => new GetGuestStatus
                           {
                               GuestStatusKey = s.Id,
                               StatusCode = s.StatusCode,
                               Status = s.Status
                           }).ToList();
                var attmaid = _maidRepository.GetAll()
                .Where(x => x.Id == maidkey)
                .Select(x => new { x.Name, x.StartDate })
                .FirstOrDefault();
                if (attmaid != null)
                {
                    if (!string.IsNullOrEmpty(attmaid.Name))
                        a.Attendant = attmaid.Name;
                    if (attmaid.StartDate.HasValue)
                        a.StartTime = attmaid.StartDate.Value.ToString("HH:mm");
                }
                DateTime dtBusinessDate = DateTime.Now;
                var contdata = _controlRepository.GetAll()
               .Select(x => new { x.SystemDate })
               .FirstOrDefault();
                if(contdata != null)
                {
                    if(contdata.SystemDate.HasValue)
                        dtBusinessDate=contdata.SystemDate.Value;
                }
                List<HouseKeeping> hp = _maidStatusdalRepository.LoadHouseKeepingList_New(dtBusinessDate,maidKey);
                int sumPlus = 0;
                if (hp.Count > 0)
                            {
                                foreach (var dr in hp)
                                {
                                    sumPlus += Convert.ToInt32(dr.Services);
                                }
                            }
                int assigned = sumPlus;
                a.Assigned = assigned+" min ("+ assigned / 60 + "hour "+ assigned % 60 + " min)";
                string[] hourmin = a.StartTime.Split(":");
                int starttotalmin = Convert.ToInt32(hourmin[0]) * 60 + Convert.ToInt32(hourmin[1]);
                int exptime = starttotalmin + assigned;
                var time = TimeSpan.FromMinutes(exptime);
                // a.ExpectedEndTime = "+\"{0:00}:{1:00}\", (int)time.TotalHours, time.Minutes+";
                string formattedmin = exptime % 60 < 10 ? "0" + exptime % 60 : (exptime % 60).ToString();
                a.ExpectedEndTime = ""+ exptime/60+":"+ formattedmin;

                a.StartTime = FormatTime(a.StartTime);
                a.ExpectedEndTime = FormatTime(a.ExpectedEndTime);
                Alllst.Add(a);

                return new ListResultDto<GetMytaskViewData>(Alllst);
            }
        }
        private string FormatTime(string time)
        {
            string[] parts = time.Split(':');
            int hour = int.Parse(parts[0]);
            string minute = parts[1];

            string formattedHour = hour < 10 ? "0" + hour : hour.ToString();
            return formattedHour + minute;
        }
        public async Task<ListResultDto<GetMytaskViewData>> GetMyTaskViewDataIncludeAllAsync()
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
                var lst = dalmaidstatus.GetMaidStatusKey(CommomData.HouseKeepingMaidStatusInspectionRequired);// _maidstatusRepository.GetAll().Where(x => x.MaidStatusName == CommomData.HouseKeepingMaidStatusInspectionRequired).Select(x => x.Id).ToList();
                                                                                                              //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                var staffmaidkey = dalstaff.GetMaidKey(user.StaffKey);
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
                                   where (!rt.MaidStatusName.Contains(mr) && !rt.MaidStatusName.Contains(mir))
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
                List<GetMytaskViewData> Alllst = new List<GetMytaskViewData>();
                GetMytaskViewData a = new GetMytaskViewData();
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
                a.MaidStatusListOutPuts = MaidStatus;
                List<GetGuestStatus> l = new List<GetGuestStatus>();
                GetGuestStatus t1 = new GetGuestStatus();
                t1.GuestStatusKey = Guid.Empty;
                t1.StatusCode = 90;
                t1.Status = "ALL";
                l.Add(t1);
                var b = _gueststatusRepository.GetAll().OrderBy(s => s.Sort)
                           .Select(s => new GetGuestStatus
                           {
                               GuestStatusKey = s.Id,
                               StatusCode = s.StatusCode,
                               Status = s.Status
                           });
                a.GetGuestStatuss = l.Concat(b).ToList();
                var attmaid = _maidRepository.GetAll()
                .Where(x => x.Id == maidkey)
                .Select(x => new { x.Name, x.StartDate })
                .FirstOrDefault();
                if (attmaid != null)
                {
                    if (!string.IsNullOrEmpty(attmaid.Name))
                        a.Attendant = attmaid.Name;
                    if (attmaid.StartDate.HasValue)
                        a.StartTime = attmaid.StartDate.Value.ToString("HH:mm");
                }
                DateTime dtBusinessDate = DateTime.Now;
                var contdata = _controlRepository.GetAll()
               .Select(x => new { x.SystemDate })
               .FirstOrDefault();
                if (contdata != null)
                {
                    if (contdata.SystemDate.HasValue)
                        dtBusinessDate = contdata.SystemDate.Value;
                }
                List<HouseKeeping> hp = _maidStatusdalRepository.LoadHouseKeepingList_New(dtBusinessDate, maidKey);
                int sumPlus = 0;
                if (hp.Count > 0)
                {
                    foreach (var dr in hp)
                    {
                        sumPlus += Convert.ToInt32(dr.Services);
                    }
                }
                int assigned = sumPlus;
                a.Assigned = assigned + " min (" + assigned / 60 + "hour " + assigned % 60 + " min)";
                string[] hourmin = a.StartTime.Split(":");
                int starttotalmin = Convert.ToInt32(hourmin[0]) * 60 + Convert.ToInt32(hourmin[1]);
                int exptime = starttotalmin + assigned;
                var time = TimeSpan.FromMinutes(exptime);
                // a.ExpectedEndTime = "+\"{0:00}:{1:00}\", (int)time.TotalHours, time.Minutes+";
                string formattedmin = exptime % 60 < 10 ? "0" + exptime % 60 : (exptime % 60).ToString();
                a.ExpectedEndTime = "" + exptime / 60 + ":" + formattedmin;

                a.StartTime = FormatTime(a.StartTime);
                a.ExpectedEndTime = FormatTime(a.ExpectedEndTime);
                Alllst.Add(a);
                return new ListResultDto<GetMytaskViewData>(Alllst);
            }
        }
        public async Task<PagedResultDto<GetDashRoomByMaidKeyOutput>> GetBindGrid(string MaidStatus = "ALL", string RoomStatus = "ALL", string GuestStatus = "ALL", string FloorNo = "0", bool HasStartedTask = false)
        {
            try
            {
                string strMaidStatusKey = ""; string strRoomStatusKey = "";


                string strGuestStatus = GuestStatus;
                if (MaidStatus != "ALL")
                {
                    strMaidStatusKey = dalmaidstatus.GetMaidStatusKey(MaidStatus).ToString();
                }
                if (RoomStatus != "ALL")
                {
                    strRoomStatusKey = dalroomstatus.GetRoomStatusKey(RoomStatus).ToString();
                }

                DateTime dtBusinessDate = DateTime.Now;

                List<GetDashRoomByMaidKeyOutput> dt = new List<GetDashRoomByMaidKeyOutput>();
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
                    var staffmaidkey = dalstaff.GetMaidKey(user.StaffKey);
                    string MaidKey = staffmaidkey.ToString();
                    if (!string.IsNullOrEmpty(MaidKey))
                    {
                        //Task<List<GetMaidStatusOutput>> lstbd = _maidStatusdalRepository.GetBusinessDate();
                        //dtBusinessDate = lstbd.Result[0].BusinessDate;

                        Task<List<MaidHasStartedTaskOutput>> lsta = _roomdalRepository.GetRoomCountByMaidKey(dtBusinessDate, MaidKey, FloorNo);
                        foreach (MaidHasStartedTaskOutput room in lsta.Result)
                        {
                            if (room.MaidStatus.Equals(CommomData.HouseKeepingMaidStatusMaidInRoom))
                                HasStartedTask = true;
                        }
                        dt = await _maidStatusdalRepository.GetMyTaskBindGrid(dtBusinessDate, MaidKey, strMaidStatusKey, strRoomStatusKey, FloorNo, strGuestStatus, HasStartedTask, user.StaffKey);
                        #region MaidHasStartedTask save in session or call MaidHasStartedTask(MaidstatusAppService)
                        ////Default => S_HASSTARTEDTASK => ession[SessionHelper.SessionMaidHasStartedTask] = "false";=>MaidHasStartedTask(maidKey)=>room["MaidStatus"].Equals('Attendant') return true

                        #endregion
                    }
                    var Count = dt.Count;

                    return new PagedResultDto<GetDashRoomByMaidKeyOutput>(
                    Count,
                    dt
                );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public ListResultDto<ShowLogOutput> GetShowLog(string roomkey, string staffkey = "")
        //{


        //    Guid groomkey = string.IsNullOrEmpty(roomkey) ? Guid.Empty : new Guid(roomkey);
        //    string ModuleName = "iClean";
        //    string TableName = "Room";
        //    if (!string.IsNullOrEmpty(staffkey))
        //    {
        //        Guid gstaffkey = new Guid(staffkey);
        //        var h = _historyRepository.GetAll().Where(x => x.ModuleName == ModuleName
        //        && x.TableName == TableName
        //        && x.StaffKey == gstaffkey
        //        && x.SourceKey == groomkey
        //        ).OrderByDescending(x => x.ChangedDate).Select(x => new ShowLogOutput
        //        {
        //            //ChangedDate = x.ChangedDate,
        //            //Detail = x.Detail
        //            GetDateTimeToDisplay = GetDateTimeToDisplay(x.ChangedDate) + "=>  " + x.Detail
        //        }).Take(4).ToList();
        //        return new ListResultDto<ShowLogOutput>(h);
        //    }
        //    else
        //    {
        //        var h = _historyRepository.GetAll().Where(x => x.ModuleName == ModuleName
        //       && x.TableName == TableName
        //       && x.SourceKey == groomkey
        //       ).OrderByDescending(x => x.ChangedDate).Select(x => new ShowLogOutput
        //       {
        //           GetDateTimeToDisplay = GetDateTimeToDisplay(x.ChangedDate) + "=>  " + x.Detail
        //       }).Take(4).ToList();
        //        return new ListResultDto<ShowLogOutput>(h);
        //    }

        //}

        public ListResultDto<ShowLogOutput> GetShowLog(string roomkey, string staffkey = "", int pageSize = 100)
        {

            try
            {
                List<ShowLogOutput> dt = new List<ShowLogOutput>();

                dt = _maidStatusdalRepository.GetRoomHistory(staffkey, roomkey, pageSize);

                var Count = dt.Count;

                return new PagedResultDto<ShowLogOutput>(
                Count,
                dt
            );
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        public static string GetDateTimeToDisplay(object inputDate)
        {
            try
            {
                string strReturnValue = "";
                if (inputDate != null && inputDate != "")
                {
                    strReturnValue = Convert.ToDateTime(inputDate).ToString("dd/MM/yyyy HH:mm:ss");
                }
                return strReturnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ListResultDto<LineSheetVewOutput> GetCheckList(string strRoomNo)
        {
            List<LineSheetVewOutput> lst = new List<LineSheetVewOutput>();
            LineSheetVewOutput s = new LineSheetVewOutput();
            List<LineSheetOutput> lstl = new List<LineSheetOutput>();
            var room = dalroom.GetbyUnit(strRoomNo);
            if (room.Count > 0)
            {
                s.roomKey = room[0].Id.ToString();
                DateTime date = DateTime.Now;
                DataTable dtLS = _roomdalRepository.GetLinenItem(s.roomKey, date);
                foreach (DataRow dr in dtLS.Rows)
                {
                    LineSheetOutput o = new LineSheetOutput();
                    o.LinenChecklistKey = (!DBNull.Value.Equals(dr["LinenChecklistKey"])) ? (!string.IsNullOrEmpty(dr["LinenChecklistKey"].ToString()) ? new Guid(dr["LinenChecklistKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.ItemKey = (!DBNull.Value.Equals(dr["ItemKey"])) ? (!string.IsNullOrEmpty(dr["ItemKey"].ToString()) ? new Guid(dr["ItemKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.Combined = (!string.IsNullOrEmpty(dr["Description"].ToString())) ? dr["ItemCode"].ToString() + " - " + dr["Description"].ToString() : dr["ItemCode"].ToString();
                    o.Quantity = (!DBNull.Value.Equals(dr["Quantity"])) ? Convert.ToInt32(dr["Quantity"].ToString()) : 0;

                    lstl.Add(o);
                }
            }
            s.AttendantCheckList = lstl;
            lst.Add(s);
            return new ListResultDto<LineSheetVewOutput>(lst);
        }

        public async Task<string> CheckListSave(LineSheetVewOutput input)
        {
            string message = "";
            try
            {
                // Check whether this room valid to update 

                List<AttendantCheckList> insert = ReadAttendantLinenItem(input.AttendantCheckList);
                //List<AttendantCheckList> getSR = ReadAttendantLinenItem(input.AttendantCheckList);
                Guid RoomKey = new Guid(input.roomKey);
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
                    Guid StaffKey = user.StaffKey;
                    //var getAllQtyCount = (from note in insert
                    //                      where note.Quantity >= 1 && note.ItemKey.ToString() != _maidStatusdalRepository.GetServiceRefuseKey(Guid.Parse(note.ItemKey.ToString())).Result
                    //                      select note).Count();

                    for (int j = 0; j < insert.Count(); j++)
                    {
                        if (insert[j].Id == Guid.Empty) //the key has 16 zeros; if want check whether it's empty, use this method
                        {
                            _maidStatusdalRepository.InsertAttLinenItem(insert[j], RoomKey, StaffKey);
                        }
                        else
                        {
                            _maidStatusdalRepository.UpdateAttLinenItem(insert[j], RoomKey, StaffKey);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
            return message;
        }
        public List<AttendantCheckList> ReadAttendantLinenItem(ICollection<LineSheetOutput> attendantCheckList)
        {
            List<AttendantCheckList> lstResBilling = new List<AttendantCheckList>();
            string server = _roomdalRepository.GetDateAsync().Result;
            var timing = DateTime.Now - Convert.ToDateTime(server);
            var date = DateTime.Now - timing;
            //save all previous records
            foreach (LineSheetOutput item in attendantCheckList)
            {
                AttendantCheckList tmp = new AttendantCheckList();
                tmp.Id = new Guid(item.LinenChecklistKey.ToString());
                tmp.ItemKey = item.ItemKey;
                tmp.Quantity = item.Quantity;
                tmp.DocDate = date;
                if (AbpSession.TenantId != null)
                {
                    tmp.TenantId = (int?)AbpSession.TenantId;
                }
                lstResBilling.Add(tmp);
            }

            return lstResBilling;
        }

        #endregion
    }
}
