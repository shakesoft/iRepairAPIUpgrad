using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BEZNgCore.Authorization.IrepairDal;
using BEZNgCore.IrepairAppService.DAL;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{
    public class MinibarCORoomAppService : BEZNgCoreAppServiceBase
    {
        private readonly IRepository<Room, Guid> _roomRepository;
        // private readonly IRepository<Control, Guid> _controlRepository;
        RoomDAL dalroom;
        //ControlDAL dalcontrol;
        private readonly RoomRepository _roomdalRepository;
        public MinibarCORoomAppService(
            IRepository<Room, Guid> roomRepository,
            // IRepository<Control, Guid> controlRepository,
            RoomRepository roomdalRepository)
        {
            _roomRepository = roomRepository;
            // _controlRepository = controlRepository;
            //  _roomdalRepository = roomdalRepository;
            dalroom = new RoomDAL(_roomRepository);
            // dalcontrol = new ControlDAL(_controlRepository);
            _roomdalRepository = roomdalRepository;
        }
        public ListResultDto<GetHotelFloor> GetBindHotelFloorList()//BindHotelFloorList();
        {
            var floor = dalroom.BindHotelFloorList();

            return new ListResultDto<GetHotelFloor>(floor);
        }
        public async Task<PagedResultDto<MinibarCORoomOutput>> GetBindHotelRoomButtonList(string floorNo = "0")//BindHotelRoomButtonList(floorno);
        {
            List<MinibarCORoomOutput> dt = new List<MinibarCORoomOutput>();

            // DateTime searchDate = dalcontrol.GetSystemdate();
            DateTime dtSearchDate = _roomdalRepository.GetBusinessDate();
            int floor = Convert.ToInt32(floorNo);
            dt = _roomdalRepository.GetChkOutRoomByDateAndFloor(dtSearchDate, floor);
            //for (int i = 0; i < dtRoom.Rows.Count; i++)
            //{
            //    strRoomNo = dtRoom.Rows[i]["Unit"].ToString();
            //    btnRoomNo = new HtmlAnchor();
            //    btnRoomNo.InnerText = strRoomNo;


            //    if (dtRoom.Rows[i]["Status"].ToString().Trim().Equals("2") && dtRoom.Rows[i]["RecheckInVirtualRoom"].ToString().Trim().Equals("1"))
            //    {
            //        btnRoomNo.Attributes.Add("class", "btn-room-occupied col-xs-3 col-sm-2 col-md-1");
            //        btnRoomNo.HRef = "javascript:OnRoomSelect('" + strRoomNo + "');";
            //    }
            //    else
            //    {
            //        btnRoomNo.Attributes.Add("class", "btn-room col-xs-3 col-sm-2 col-md-1");
            //        btnRoomNo.HRef = "javascript:OnNonOccupiedRoomSelect();";
            //    }

            //    pnRoomNoList.Controls.Add(btnRoomNo);
            //}
            var Count = dt.Count;
            return new PagedResultDto<MinibarCORoomOutput>(
               Count,
               dt
           );
        }

    }
}
