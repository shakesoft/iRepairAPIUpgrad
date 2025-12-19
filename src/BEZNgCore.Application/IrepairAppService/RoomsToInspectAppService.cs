using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using BEZNgCore.Authorization.IrepairDal;
using BEZNgCore.Common;
using BEZNgCore.IrepairAppService.DAL;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Mvc;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{
    public class RoomsToInspectAppService : BEZNgCoreAppServiceBase
    {
        private readonly IRepository<Maid, Guid> _maidRepository;
        private readonly IRepository<RoomStatus, Guid> _roomstatusRepository;
        private readonly IRepository<Room, Guid> _roomRepository;
        private readonly IRepository<Control, Guid> _controlRepository;
        private readonly IRepository<MaidStatus, Guid> _maidstatusRepository;
        // private readonly IRepository<SupervisorCheckList, Guid> _supervisorchecklistRepository;
        private readonly IRepository<Staff, Guid> _staffRepository;
        private readonly MaidStatusRepository _maidStatusdalRepository;
        private readonly RoomRepository _roomdalRepository;
        RoomStatusDAL dalroomstatus;
        RoomDAL dalroom;
        // SupervisorCheckListDAL dalsupcl;
        public RoomsToInspectAppService(
            IRepository<RoomStatus, Guid> roomstatusRepository,
            IRepository<Room, Guid> roomRepository,
            IRepository<Control, Guid> controlRepository,
            IRepository<MaidStatus, Guid> maidstatusRepository,
            IRepository<Maid, Guid> maidRepository,
            MaidStatusRepository maidStatusdalRepository,
            IRepository<Staff, Guid> staffRepository,
            RoomRepository roomdalRepository//,
                                            // IRepository<SupervisorCheckList, Guid> supervisorchecklistRepository

            )
        {
            _roomstatusRepository = roomstatusRepository;
            _roomRepository = roomRepository;
            _controlRepository = controlRepository;
            _maidstatusRepository = maidstatusRepository;
            _maidRepository = maidRepository;
            _maidStatusdalRepository = maidStatusdalRepository;
            _staffRepository = staffRepository;
            _roomdalRepository = roomdalRepository;
            dalroomstatus = new RoomStatusDAL(_roomstatusRepository);
            dalroom = new RoomDAL(_roomRepository);
            // _supervisorchecklistRepository = supervisorchecklistRepository;
            // dalsupcl = new SupervisorCheckListDAL(_supervisorchecklistRepository);
        }
        [HttpGet]
        public ListResultDto<RTIViewData> GetRoomToInspectViewData()
        {

            List<RTIViewData> Alllst = new List<RTIViewData>();
            RTIViewData a = new RTIViewData();
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
            a.TotoalRoomCount = GetBindTotoalRoomCount();
            Alllst.Add(a);
            return new ListResultDto<RTIViewData>(Alllst);
        }

        public ListResultDto<RTIViewData> GetRoomToInspectViewDataIncludeAll()
        {

            List<RTIViewData> Alllst = new List<RTIViewData>();
            RTIViewData a = new RTIViewData();
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
            a.TotoalRoomCount = GetBindTotoalRoomCount();
            Alllst.Add(a);
            return new ListResultDto<RTIViewData>(Alllst);
        }
        public int GetBindTotoalRoomCount()//BindTotoalRoomCount();
        {
            int count = 0;
            Task<List<GetDashRoomByMaidStatusKeyOutput>> dt = null;
            DateTime dtBusinessDate = DateTime.Now;
            var lst = _maidstatusRepository.GetAll().Where(x => x.MaidStatusName == CommomData.HouseKeepingMaidStatusInspectionRequired).Select(x => x.Id).ToList();

            if (lst.Count > 0)
            {
                string maidStatusKey = lst[0].ToString();
                var d = _controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefault();
                if (d != null)
                {
                    dtBusinessDate = d.Value;
                }
                string maidKey = ""; string floorNo = ""; string roomStatusKey = "";
                dt = _maidStatusdalRepository.GetDashRoomByMaidStatusKey(dtBusinessDate, maidStatusKey, maidKey, floorNo, roomStatusKey);
                count = dt.Result.Count;
            }
            return count;

        }

        public async Task<PagedResultDto<GetDashRoomByMaidStatusKeyOutput>> GetBindGrid(string SelectedRoomStatus = "ALL", string FloorNo = "0", string MaidKey = "")
        {
            List<GetDashRoomByMaidStatusKeyOutput> dt = new List<GetDashRoomByMaidStatusKeyOutput>();
            string strRoomStatusKey = "";
            if (SelectedRoomStatus != "ALL")
            {
                strRoomStatusKey = _roomstatusRepository.GetAll().Where(x => x.RoomStatusName == SelectedRoomStatus).Select(x => x.Id).FirstOrDefault().ToString();
            }
            DateTime dtBusinessDate = DateTime.Now;
            var lst = _maidstatusRepository.GetAll().Where(x => x.MaidStatusName == CommomData.HouseKeepingMaidStatusInspectionRequired).Select(x => x.Id).ToList();

            if (lst.Count > 0)
            {
                string maidStatusKey = lst[0].ToString();
                //var d = _controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefault();
                //if (d != null)
                //{
                //    dtBusinessDate = d.Value;
                //}

                dt = await _roomdalRepository.GetRoomByMaidStatusKey(dtBusinessDate, maidStatusKey, MaidKey, FloorNo, strRoomStatusKey);

            }

            var Count = dt.Count;

            return new PagedResultDto<GetDashRoomByMaidStatusKeyOutput>(
            Count,
            dt
        );

        }

        public ListResultDto<LineSheetSupVewOutput> GetSupCheckList(string strRoomNo)
        {
            List<LineSheetSupVewOutput> lst = new List<LineSheetSupVewOutput>();
            LineSheetSupVewOutput s = new LineSheetSupVewOutput();
            List<LineSheetSupOutput> lstsup = new List<LineSheetSupOutput>();
            var room = dalroom.GetbyUnit(strRoomNo);
            if (room.Count > 0)
            {
                s.roomKey = room[0].Id.ToString();
                DataTable dtLS = _roomdalRepository.GetSupLinenItem(s.roomKey);
                foreach (DataRow dr in dtLS.Rows)
                {
                    LineSheetSupOutput o = new LineSheetSupOutput();
                    o.InspectionChecklistKey = (!DBNull.Value.Equals(dr["InspectionChecklistKey"])) ? (!string.IsNullOrEmpty(dr["InspectionChecklistKey"].ToString()) ? new Guid(dr["InspectionChecklistKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.ItemKey = (!DBNull.Value.Equals(dr["ItemKey"])) ? (!string.IsNullOrEmpty(dr["ItemKey"].ToString()) ? new Guid(dr["ItemKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.Combined = (!string.IsNullOrEmpty(dr["Description"].ToString())) ? dr["ItemCode"].ToString() + " - " + dr["Description"].ToString() : dr["ItemCode"].ToString();
                    o.Quantity = !DBNull.Value.Equals(dr["Quantity"]) ? dr["Quantity"].ToString() : "";
                    o.Checked = !DBNull.Value.Equals(dr["Checked"]) ? Convert.ToInt32(dr["Checked"]) : 0;
                    o.chkLinenItem = (o.Checked == 0) ? false : true;
                    lstsup.Add(o);
                }
            }
            s.SupCheckList = lstsup;
            lst.Add(s);
            return new ListResultDto<LineSheetSupVewOutput>(lst);
        }
        public async Task<string> SupCheckListSave(LineSheetSupVewOutput input)
        {
            string message = "";
            try
            {
                // Check whether this room valid to update 
                List<SupervisorCheckList> insert = ReadSupervisorLinenItem(input.SupCheckList);
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
                    //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                    Guid StaffKey = user.StaffKey;
                    for (int i = 0; i < insert.Count; i++)
                    {
                        if (insert[i].Id == Guid.Empty) //the key has 32 zeros; if want check whether it's empty, use this method
                        {
                            _maidStatusdalRepository.InsertSupLinenItem(insert[i], RoomKey, StaffKey);
                        }
                        else
                        {
                            _maidStatusdalRepository.UpdateSupLinenItem(insert[i], RoomKey, StaffKey);
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
        private List<SupervisorCheckList> ReadSupervisorLinenItem(ICollection<LineSheetSupOutput> supCheckList)
        {
            List<SupervisorCheckList> lstResBilling = new List<SupervisorCheckList>();
            string server = _roomdalRepository.GetDateAsync().Result;
            var timing = DateTime.Now - Convert.ToDateTime(server);
            var date = DateTime.Now - timing;
            //save all previous records
            foreach (LineSheetSupOutput item in supCheckList)
            {
                SupervisorCheckList tmp = new SupervisorCheckList();
                tmp.Id = new Guid(item.InspectionChecklistKey.ToString());
                tmp.ItemKey = item.ItemKey;
                tmp.Checked = item.Checked;
                tmp.DocDate = date;
                if (AbpSession.TenantId != null)
                {
                    tmp.TenantId = (int?)AbpSession.TenantId;
                }
                lstResBilling.Add(tmp);
            }

            return lstResBilling;
        }
    }
}
