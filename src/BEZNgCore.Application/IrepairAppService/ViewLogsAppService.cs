using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BEZNgCore.Authorization.IrepairDal;
using BEZNgCore.CustomizeRepository;
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
    public class ViewLogsAppService : BEZNgCoreAppServiceBase
    {
        //private readonly IRepository<Staff, Guid> _staffRepository;
        //private readonly IRepository<Room, Guid> _roomRepository;
        private readonly IMaidStatusRepository _maidStatusdalRepository;
        //private readonly IRepository<MTechnician, int> _technicianRepository;
        public ViewLogsAppService(
            //IRepository<Staff, Guid> staffRepository,
            //IRepository<Room, Guid> roomRepository,
            IMaidStatusRepository maidStatusdalRepository//,
                                                         //IRepository<MTechnician, int> technicianRepository)
            )
        {
            //_staffRepository = staffRepository;
            //_roomRepository = roomRepository;
            _maidStatusdalRepository = maidStatusdalRepository;
            //_technicianRepository = technicianRepository;
        }
        #region iclean
        [HttpGet]
        public ListResultDto<ViewLogViewData> GetViewLogViewData()
        {
            List<ViewLogViewData> Alllst = new List<ViewLogViewData>();
            ViewLogViewData a = new ViewLogViewData();
            a.LogType = GetLogType();
            a.Attendant = GetAllAttendant();
            a.Room = GetAllRoom();
            //a.Attendant = _staffRepository.GetAll().Where(x => x.MaidKey != null && x.Active == 1).OrderBy(x => x.UserName)
            //    .Select(x => new MaidOutput
            //    {
            //        StaffKey = x.Id,
            //        UserName = x.UserName
            //    })
            //    .ToList();
            //a.Room = _roomRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Floor).ThenBy(x => x.Unit)
            //    .Select(x => new DDLRoomOutput
            //    {
            //        RoomKey = x.Id,
            //        Unit = x.Unit
            //    })
            //    .ToList();
            Alllst.Add(a);
            return new ListResultDto<ViewLogViewData>(Alllst);
        }

        protected List<MaidOutput> GetAllAttendant()
        {
            return _maidStatusdalRepository.GetAllAttendant();
        }
        protected List<DDLRoomOutput> GetAllRoom()
        {
            return _maidStatusdalRepository.GetAllRoom();
        }

        public ListResultDto<ViewLogViewData> GetViewLogViewDataIncludeAll()
        {
            List<ViewLogViewData> Alllst = new List<ViewLogViewData>();
            ViewLogViewData a = new ViewLogViewData();
            a.LogType = GetLogType();
            List<MaidOutput> sl = new List<MaidOutput>();
            MaidOutput s1 = new MaidOutput();
            s1.StaffKey = Guid.Empty;
            s1.UserName = "ALL";
            sl.Add(s1);
            List<MaidOutput> ss = GetAllAttendant();
            a.Attendant = sl.Concat(ss).ToList();
            List<DDLRoomOutput> rl = new List<DDLRoomOutput>();
            DDLRoomOutput r1 = new DDLRoomOutput();
            r1.RoomKey = Guid.Empty;
            r1.Unit = "ALL Rooms";
            rl.Add(r1);
            List<DDLRoomOutput> rr = GetAllRoom();
            //var rr = _roomRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Floor).ThenBy(x => x.Unit)
            //    .Select(x => new DDLRoomOutput
            //    {
            //        RoomKey = x.Id,
            //        Unit = x.Unit
            //    });
            a.Room = rl.Concat(rr).ToList();
            Alllst.Add(a);
            return new ListResultDto<ViewLogViewData>(Alllst);
        }
        public List<LogTypeOutput> GetLogType()
        {
            try
            {
                List<LogTypeOutput> r = new List<LogTypeOutput>();
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("LogType"));
                dt.Columns.Add(new DataColumn("Value"));

                DataRow drNew = dt.NewRow();
                drNew["LogType"] = "ALL Type";
                drNew["Value"] = "";
                dt.Rows.Add(drNew);

                drNew = dt.NewRow();
                drNew["LogType"] = "Staff";
                drNew["Value"] = "Staff";
                dt.Rows.Add(drNew);

                drNew = dt.NewRow();
                drNew["LogType"] = "Room";
                drNew["Value"] = "Room";
                dt.Rows.Add(drNew);

                drNew = dt.NewRow();
                drNew["LogType"] = "Room DND";
                drNew["Value"] = "RoomDND";
                dt.Rows.Add(drNew);

                drNew = dt.NewRow();
                drNew["LogType"] = "Room OPT";
                drNew["Value"] = "RoomOPT";
                dt.Rows.Add(drNew);

                drNew = dt.NewRow();
                drNew["LogType"] = "Room Assign";
                drNew["Value"] = "RoomAssign";
                dt.Rows.Add(drNew);

                drNew = dt.NewRow();
                drNew["LogType"] = "Minibar";
                drNew["Value"] = "Minibar";
                dt.Rows.Add(drNew);

                drNew = dt.NewRow();
                drNew["LogType"] = "Laundry";
                drNew["Value"] = "Laundry";
                dt.Rows.Add(drNew);

                foreach (DataRow row in dt.Rows)
                {
                    LogTypeOutput o = new LogTypeOutput();
                    o.LogType = row["LogType"].ToString();
                    o.Value = row["Value"].ToString();
                    r.Add(o);
                }

                return r;
            }
            catch
            {
                throw;
            }
        }

        //public ListResultDto<MaidOutput> GetBindDDLMaid()
        //{
        //    var v = _staffRepository.GetAll().Where(x => x.MaidKey != null && x.Active == 1).OrderBy(x => x.UserName)
        //        .Select(x => new MaidOutput
        //        {
        //            StaffKey = x.Id,
        //            UserName = x.UserName
        //        })
        //        .ToList();

        //    return new ListResultDto<MaidOutput>(v);
        //}
        //public ListResultDto<DDLRoomOutput> GetBindDDLRoom()
        //{
        //    var v = _roomRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Floor).ThenBy(x => x.Unit)
        //        .Select(x => new DDLRoomOutput
        //        {
        //            RoomKey = x.Id,
        //            Unit = x.Unit
        //        })
        //        .ToList();

        //    return new ListResultDto<DDLRoomOutput>(v);
        //}
        public async Task<PagedResultDto<ViewLogOutput>> GetBindGrid(string logType = "", string staffKey = "", string roomKey = "")
        {
            try
            {
                List<ViewLogOutput> dt = new List<ViewLogOutput>();

                dt = _maidStatusdalRepository.GetHistory(logType, staffKey, roomKey);

                var Count = dt.Count;

                return new PagedResultDto<ViewLogOutput>(
                Count,
                dt
            );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region irepair
        public ListResultDto<ViewLogIRViewData> GetViewLogIRViewData()
        {
            List<ViewLogIRViewData> Alllst = new List<ViewLogIRViewData>();
            ViewLogIRViewData a = new ViewLogIRViewData();
            a.LogType = GetLogTypeIR();
            List<TechnicianOutput> l = new List<TechnicianOutput>();
            TechnicianOutput t1 = new TechnicianOutput();
            t1.StaffKey = new Guid();
            t1.UserName = "ALL";
            l.Add(t1);
            //RIGHT JOIN  MTechnician tech ON tech.TechnicianKey = s.TechnicianKey
            List<TechnicianOutput> b = GetStaffAllTechnician();
            //var b = (from s in _staffRepository.GetAll()
            //        join t in _technicianRepository.GetAll()
            //        on s.TechnicianKey equals t.TechnicianKey into joined
            //        from t in joined.DefaultIfEmpty()
            //        where (s.Id!=null&&s.Active==1&&t.Active==1)
            //        orderby s.UserName
            //        select new TechnicianOutput
            //          {
            //              StaffKey = s.Id,
            //              UserName = s.UserName
            //          });
            a.Technician = l.Concat(b).ToList();

            Alllst.Add(a);
            return new ListResultDto<ViewLogIRViewData>(Alllst);
        }
        protected List<TechnicianOutput> GetStaffAllTechnician()
        {
            return _maidStatusdalRepository.GetStaffAllTechnician();
        }
        private List<LogTypeIROutput> GetLogTypeIR()
        {
            List<LogTypeIROutput> lst = new List<LogTypeIROutput>();
            lst.AddRange(new List<LogTypeIROutput>
        {
        new LogTypeIROutput("ALL Type",""),
        new LogTypeIROutput("Staff","Staff"),
        new LogTypeIROutput("Work Order","WO"),
        new LogTypeIROutput("Work Assign","WOAssign"),
        new LogTypeIROutput("Time Sheet","WTS"),
        new LogTypeIROutput("Block Room","BR")
        });

            return lst;
        }
        public async Task<PagedResultDto<ViewLogIROutput>> GetIRBindGrid(string logType = "", string staffKey = "00000000-0000-0000-0000-000000000000")
        {
            try
            {
                List<ViewLogIROutput> dt = new List<ViewLogIROutput>();

                dt = _maidStatusdalRepository.GetIRHistory(logType, staffKey);

                var Count = dt.Count;

                return new PagedResultDto<ViewLogIROutput>(
                Count,
                dt
            );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
