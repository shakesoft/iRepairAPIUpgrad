using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using BEZNgCore.Authorization.IrepairDal;
using BEZNgCore.IrepairAppService.DAL;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{
    public class MinibarRoomAppService : BEZNgCoreAppServiceBase
    {
        private readonly IRepository<Room, Guid> _roomRepository;
        //private readonly IRepository<Control, Guid> _controlRepository;
       // private readonly IRepository<Reservation, Guid> _reservationRepository;
       // private readonly IRepository<Guest, Guid> _guestRepository;
        private readonly IRepository<Item, Guid> _itemRepository;
        private readonly IRepository<Currency, Guid> _currencyRepository;
        private readonly IRepository<History, Guid> _historyRepository;
        private readonly IRepository<GeneralProfile, Guid> _generalprofileRepository;
        private readonly IRepository<ReservationRate, Guid> _reservationrateRepository;

        RoomDAL dalroom;
        //ControlDAL dalcontrol;
        // ReservationDAL dalreservation;
        ItemDAL dalitem;
        CurrencyDAL dalcurrency;
        HistoryDAL dalhistory;
        GeneralProfileDAL dalgeneralprofile;
        ReservationRateDAL dalreservationrate;
        private readonly RoomRepository _roomdalRepository;
        public MinibarRoomAppService(
            IRepository<Room, Guid> roomRepository,
            //IRepository<Control, Guid> controlRepository,
            // IRepository<Reservation, Guid> reservationRepository,
            //IRepository<Guest, Guid> guestRepository,
            IRepository<Item, Guid> itemRepository,
            IRepository<Currency, Guid> currencyRepository,
            IRepository<History, Guid> historyRepository,
            IRepository<GeneralProfile, Guid> generalprofileRepository,
            IRepository<ReservationRate, Guid> reservationrateRepository,
        RoomRepository roomdalRepository)
        {
            _roomRepository = roomRepository;
            //_controlRepository = controlRepository;
            // _reservationRepository = reservationRepository;
            //_guestRepository = guestRepository;
            _itemRepository = itemRepository;
            _currencyRepository = currencyRepository;
            _historyRepository = historyRepository;
            _generalprofileRepository = generalprofileRepository;
            _reservationrateRepository = reservationrateRepository;
            //_roomdalRepository = roomdalRepository;
            dalroom = new RoomDAL(_roomRepository);
            //dalcontrol = new ControlDAL(_controlRepository);
            // dalreservation = new ReservationDAL(_reservationRepository, _guestRepository);
            dalitem = new ItemDAL(_itemRepository);
            dalcurrency = new CurrencyDAL(_currencyRepository);
            dalhistory = new HistoryDAL(_historyRepository);
            dalgeneralprofile = new GeneralProfileDAL(_generalprofileRepository);
            dalreservationrate = new ReservationRateDAL(_reservationrateRepository);
            _roomdalRepository = roomdalRepository;
        }
        [HttpGet]
        public ListResultDto<MinibarItemViewData> GetMinibarItemViewData(string roomNo)
        {
            List<MinibarItemViewData> Alllst = new List<MinibarItemViewData>();

            //string roomKey = dalroom.GetRoomKeyByRoomNo(roomNo);
            //List<ReservationOutput> rst = dalreservation.GetReservationByRoomKey(roomKey);
            string roomKey = "";
            DataTable dtt = _roomdalRepository.GetHotelRoomByRoomNo(roomNo);
            if (dtt.Rows.Count > 0)
            {
                roomKey = dtt.Rows[0]["RoomKey"].ToString();
            }
            List<ReservationOutput> rst = _roomdalRepository.GetReservationByRoomKey(roomKey);
            if (rst.Count == 0)
                throw new UserFriendlyException("This room is vacant. No action is required.");
            List<MinibarItemAddOutput> lst = new List<MinibarItemAddOutput>();
            DataTable dt = _roomdalRepository.GetMinibarItemByResKey(rst[0].ReservationKey.ToString());
            foreach (DataRow dr in dt.Rows)
            {
                MinibarItemAddOutput o = new MinibarItemAddOutput();
                o.ItemKey = (!DBNull.Value.Equals(dr["ItemKey"])) ? (!string.IsNullOrEmpty(dr["RoomKey"].ToString()) ? new Guid(dr["ItemKey"].ToString()) : Guid.Empty) : Guid.Empty;//Guid.Parse(dr["ItemKey"].ToString());
                o.PostDate = Convert.IsDBNull(dr["PostDate"]) ? null : (DateTime?)(dr["PostDate"]);// Convert.ToDateTime(dr["PostDate"].ToString());
                o.UserName = !DBNull.Value.Equals(dr["UserName"]) ? dr["UserName"].ToString() : "";// resRate.UserName;
                o.Description = !DBNull.Value.Equals(dr["Description"]) ? dr["Description"].ToString() : "";// resRate.UserName;
                o.PostDateDes = o.PostDate.Value.ToString("d/M/yyyy HH:mm");
                lst.Add(o);
            }
            MinibarItemViewData a = new MinibarItemViewData();
            a.ReservationKey = rst[0].ReservationKey;
            a.DocNo = rst[0].DocNo;
            a.CheckInDate = rst[0].CheckInDate;
            a.CheckOutDate = rst[0].CheckOutDate;
            a.GuestKey = rst[0].GuestKey;
            a.GuestName = rst[0].GuestName;
            a.RoomKey = rst[0].RoomKey;
            a.AddedMinibaritems = lst;
            a.MinibarItems = dalitem.GetMinibarItem();
            Alllst.Add(a);
            return new ListResultDto<MinibarItemViewData>(Alllst);
        }
        public ListResultDto<GetHotelFloor> GetBindHotelFloorList()//BindHotelFloorList();
        {
            var floor = dalroom.BindHotelFloorList();

            return new ListResultDto<GetHotelFloor>(floor);
        }
        public async Task<PagedResultDto<RoomStatusHotelRoomOutput>> GetBindHotelRoomButtonList(string floorNo = "0")//BindHotelRoomButtonList(floorno);
        {
            List<RoomStatusHotelRoomOutput> dt = new List<RoomStatusHotelRoomOutput>();

            //DateTime searchDate = dalcontrol.GetSystemdate();
            DateTime dtSearchDate = _roomdalRepository.GetBusinessDate();
            string roomStatusKey = ""; string guestStatus = ""; string[] list = null;
            int floor = Convert.ToInt32(floorNo);

            dt = _roomdalRepository.GetHotelRoomByDateAndFloor(dtSearchDate, floor, roomStatusKey, guestStatus, list);


            //DataTable dtRoom = _roomdalRepository.GetHotelRoomByDateAndFloor(dtSearchDate, Convert.ToInt32(floorNo));
            //if (dtRoom.Rows[i]["RoomStatus"].ToString().Trim().ToLower().Equals("occupied"))
            //{
            //    btnRoomNo.Attributes.Add("class", "btn-room-occupied col-xs-3 col-sm-2 col-md-1");
            //    btnRoomNo.HRef = "javascript:OnRoomSelect('" + strRoomNo + "');";
            //"MinibarItem.aspx?room=" + arrParameter[1]
            //}
            //else
            //{
            //    btnRoomNo.Attributes.Add("class", "btn-room col-xs-3 col-sm-2 col-md-1");
            //    btnRoomNo.HRef = "javascript:OnNonOccupiedRoomSelect();";
            //alert('This room is vacant. No action is required.');
            //}
            var Count = dt.Count;
            return new PagedResultDto<RoomStatusHotelRoomOutput>(
               Count,
               dt
           );
        }

        #region Bind All Grid Item & Selected GridItem(MinibarItem)
        public async Task<ListResultDto<ReservationOutput>> GetBindRoomInfo(string roomNo)
        {
            //string roomKey = dalroom.GetRoomKeyByRoomNo(roomNo);
            // var lst = dalreservation.GetReservationByRoomKey(roomKey);
            string roomKey = "";
            DataTable dtt = _roomdalRepository.GetHotelRoomByRoomNo(roomNo);
            if (dtt.Rows.Count > 0)
            {
                roomKey = dtt.Rows[0]["RoomKey"].ToString();
            }
            var lst = _roomdalRepository.GetReservationByRoomKey(roomKey);
            //if (dtRes.Rows.Count > 0)
            //{
            //    Guest guest = BLL_Guest.GetGuestInfoByGuestKey(strProperty, dtRes.Rows[0]["GuestKey"].ToString());

            //    litFolioNo.Text = dtRes.Rows[0]["DocNo"].ToString();
            //    litGuestName.Text = guest.Name;
            //    litCheckIn.Text = DisplayHelper.GetShortDateToDisplay(dtRes.Rows[0]["CheckInDate"]);
            //    litCheckOut.Text = DisplayHelper.GetShortDateToDisplay(dtRes.Rows[0]["CheckOutDate"]);

            //    BindGridItemAdded(strProperty, dtRes.Rows[0]["ReservationKey"].ToString());
            //}
            //else
            //{
            //    grdItem.Enabled = false;
            //    grdItemSelected.Enabled = false;
            //    btnAddItem.Enabled = false;
            //    btnReset.Enabled = false;

            //    throw new Exception("This room is vacant. No action is required.");
            //}
            return new ListResultDto<ReservationOutput>(lst);
        }
        public ListResultDto<MinibarItemAddOutput> GetBindGridItemAdded(string resKey)
        {
            try
            {
                Guid ReservationKey = new Guid(resKey);
                var lst = (from resRate in _reservationrateRepository.GetAll()
                           where (resRate.ReservationKey == ReservationKey)
                           join i in _itemRepository.GetAll() on resRate.ItemKey equals i.Id into t
                           from rt in t.DefaultIfEmpty()
                           where (rt.Minibar == 1)
                           orderby resRate.Seq descending
                           select new MinibarItemAddOutput
                           {
                               ItemKey = rt.Id,
                               PostDate = resRate.PostDate,
                               UserName = resRate.UserName,
                               Description = resRate.Description
                           }).ToList();
                return new ListResultDto<MinibarItemAddOutput>(lst);
                //SELECT resRate.* FROM ReservationRate resRate
                //LEFT JOIN Item i ON  resRate.ItemKey = i.ItemKey
                //WHERE  i.Minibar = 1 AND  resRate.ReservationKey = @ResKey
                //ORDER BY  resRate.Seq DESC
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        public ListResultDto<ItemOutput> GetBindGridItem()
        {
            var lst = dalitem.GetMinibarItem();
            return new ListResultDto<ItemOutput>(lst);
        }
        public ListResultDto<ItemSelectedOutput> GetBindGridItemSelected()
        {
            var lst = dalitem.GetMinibarSelectedItem();
            return new ListResultDto<ItemSelectedOutput>(lst);
        }
        #endregion

        #region Button Click - New - Add Item / Cancel(popupAddItemQty)
        public string GetBindItemInfo(string itemKey)
        {
            string Description = "";
            var item = dalitem.GetItemByItemKey(itemKey);
            if (item != null)
            {
                Description = item.Description;
            }
            return Description;
        }
        public ListResultDto<ItemSelectedOutput> GetBindEditItemInfo(string itemKey)
        {
            var lst = dalitem.GetMinibarSelectedItem();
            return new ListResultDto<ItemSelectedOutput>(lst);

            //for (int i = 0; i < dtAddedItem.Rows.Count; i++)
            //{
            //    if (dtAddedItem.Rows[i]["ItemKey"].ToString().Equals(itemKey))
            //    {
            //        litItemDesc.Text = dtAddedItem.Rows[i]["Description"].ToString();
            //        txtQty.Text = dtAddedItem.Rows[i]["Qty"].ToString();
            //        break;
            //    }
            //}

        }
        public ListResultDto<ItemSelectedOutput> GetbtnAddItem(string qty, string itemKey)
        {

            int intRowNo = 1;
            var item = dalitem.GetItemByItemKey(itemKey);
            List<ItemSelectedOutput> dtAddedItem = new List<ItemSelectedOutput>();
            if (IsExistInTable(dtAddedItem, itemKey))
            {
                for (int i = 0; i < dtAddedItem.Count; i++)
                {
                    if (dtAddedItem[i].ItemKey.ToString().Equals(itemKey))
                    {
                        dtAddedItem[i].Qty = Convert.ToInt32(dtAddedItem[i].Qty) + Convert.ToInt32(qty);
                        break;
                    }
                }
            }
            else
            {
                if (dtAddedItem.Count > 0)
                {
                    intRowNo += dtAddedItem.Count;
                }
                ItemSelectedOutput o = new ItemSelectedOutput();
                o.No = intRowNo;
                o.ItemKey = new Guid(itemKey);
                o.Description = item.Description;
                o.Qty = Convert.ToInt32(qty);
                o.SalesPrice = item.SalesPrice;
                o.PostCodeKey = item.PostCodeKey;
                dtAddedItem.Add(o);
            }

            return new ListResultDto<ItemSelectedOutput>(dtAddedItem);


        }

        private bool IsExistInTable(List<ItemSelectedOutput> dtAddedItem, string itemKey)
        {
            try
            {
                bool blnExist = false;
                for (int i = 0; i < dtAddedItem.Count; i++)
                {
                    if (dtAddedItem[i].ItemKey.ToString().Equals(itemKey))
                    {
                        return true;
                    }
                }


                return blnExist;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<string> btnAddItemClick(PostSelectedItems input)//string roomNo, List<ItemSelectedOutput> dtAddedItem)
        {
            string message = "";
            try
            {

                string roomKey = input.roomKey;//dalroom.GetRoomKeyByRoomNo(roomNo);

                if (input.ItemSelected.Count > 0)
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
                        string voucherNo = "";
                        //int intSuccessful = dalreservation.GenerateReservationRateToInsert(roomKey, dtAddedItem);
                        try
                        {
                            int intSuccessful = 0;
                            string type = (string.IsNullOrEmpty(voucherNo) ? "minibar" : "laundry");
                            List<ReservationRate> listResRate = new List<ReservationRate>();
                            ReservationRate resRate;
                            string strReservationKey = input.ReservationKey; //dalreservation.GetReservationKeyByRoomKey(roomKey);
                                                                             // DateTime dtBusinessDate = dalcontrol.GetSystemdate();
                            DateTime dtBusinessDate = _roomdalRepository.GetBusinessDate();
                            string strCurrencyKey = dalcurrency.GetCurrencyKey();

                            foreach (ItemSelectedOutput dr in input.ItemSelected)
                            {
                                double dbSubTotalSalesPrice = Convert.ToDouble(dr.SalesPrice) * Convert.ToInt32(dr.Qty);
                                SalesPrice pricing = GetSalesPrice(dbSubTotalSalesPrice, dr.PostCodeKey.ToString());
                                resRate = new ReservationRate();
                                resRate.ReservationKey = new Guid(strReservationKey);
                                resRate.ChargeDate = dtBusinessDate;
                                resRate.Description = GetReservationRateDescription(dr.No.ToString(), dr.Description.ToString(), dr.Qty.ToString(), voucherNo);
                                resRate.ItemKey = new Guid(dr.ItemKey.ToString());
                                resRate.Rate = (decimal?)pricing.salesPrice;
                                resRate.Tax1 = (decimal?)pricing.salesTax1;
                                resRate.Tax2 = (decimal?)pricing.salesTax2;
                                resRate.Tax3 = (decimal?)pricing.salesTax3;
                                resRate.Total = (decimal?)pricing.salesTotal;
                                resRate.BillTo = 2;
                                resRate.PostCodeKey = new Guid(dr.PostCodeKey.ToString());
                                resRate.StaffKey = user.StaffKey;//new Guid(BLL_Staff.GetLoginUserStaffKey());
                                resRate.UserName = user.UserName;
                                resRate.RoomKey = new Guid(roomKey);
                                resRate.ForeignCurrencyKey = !string.IsNullOrEmpty(strCurrencyKey) ? new Guid(strCurrencyKey) : (Guid?)null;
                                resRate.ForeignAmount = (decimal?)pricing.salesPrice;
                                resRate.SecondaryAmount = (decimal?)pricing.salesTotal;
                                resRate.ForeignExchangeRate = 1;
                                resRate.SecondaryExchangeRate = 1;

                                listResRate.Add(resRate);
                            }

                            if (listResRate.Count > 0)
                            {
                                foreach (ReservationRate res in listResRate)
                                {
                                    int Sort = 1;
                                    List<ReservationRate> l = dalreservationrate.check(res.ReservationKey);
                                    if (l.Count > 0)
                                    {
                                        Sort = l.Max(x => x.Sort).Value + 1;
                                    }
                                    ReservationRate r = new ReservationRate();
                                    r.Id = Guid.NewGuid();
                                    r.ReservationKey = res.ReservationKey;
                                    r.ChargeDate = res.ChargeDate;
                                    r.Description = res.Description;
                                    r.ItemKey = res.ItemKey;
                                    r.Rate = res.Rate;
                                    r.Tax1 = res.Tax1.HasValue ? res.Tax1.Value : 0.0m;
                                    r.Tax2 = res.Tax2.HasValue ? res.Tax2.Value : 0.0m;
                                    r.Tax3 = res.Tax3.HasValue ? res.Tax3.Value : 0.0m;
                                    r.Total = res.Total == null ? 0.0m : res.Total;
                                    r.BillTo = res.BillTo;
                                    r.PostCodeKey = res.PostCodeKey;
                                    r.StaffKey = res.StaffKey;
                                    r.UserName = res.UserName;
                                    r.RoomKey = res.RoomKey;
                                    r.ShiftKey = res.ShiftKey;
                                    r.ShiftNo = res.ShiftNo;
                                    r.ForeignCurrencyKey = res.ForeignCurrencyKey;
                                    r.ForeignAmount = res.ForeignAmount;
                                    r.SecondaryAmount = res.SecondaryAmount == null ? 0.0m : res.SecondaryAmount;
                                    r.ForeignExchangeRate = res.ForeignExchangeRate;
                                    r.SecondaryExchangeRate = res.SecondaryExchangeRate;
                                    r.Sort = Sort;
                                    r.PostDate = DateTime.Now;
                                    // r.TenantId = 1;
                                    r.TenantId = (int?)AbpSession.TenantId;
                                    r.AdditionalBed = 0;
                                    r.RedemptPoint = 0;
                                    r.AwardedPoint = 0;
                                    r.Consolidated = 0;
                                    r.Void = 0;
                                    r.Status = 0;
                                    r.Overwrite = 0;
                                    r.Sync = 0;
                                    r.OverwriteTime = DateTime.Now;
                                    intSuccessful = dalreservationrate.InsertReservationRate(r);
                                    if (intSuccessful == 1)
                                    {
                                        CreateOrEditHistoryDto j = new CreateOrEditHistoryDto();
                                        j.StaffKey = user.StaffKey;
                                        j.Operation = "I";
                                        j.ModuleName = "iClean";
                                        if (type.Equals("minibar"))
                                            j.TableName = "Minibar";
                                        else
                                            j.TableName = "Laundry";
                                        j.Detail = "(iClean) " + user.UserName + " added " + res.Description;
                                        j.Sort = 0;
                                        j.Sync = 0;
                                        j.ChangedDate = DateTime.Now;
                                        j.Id = null;

                                        var history = ObjectMapper.Map<History>(j);
                                        if (AbpSession.TenantId != null)
                                        {
                                            history.TenantId = (int?)AbpSession.TenantId;
                                        }
                                        history.SourceKey = res.ReservationKey;
                                        intSuccessful = dalhistory.SaveAsync(history).Result;
                                    }

                                }


                            }

                            if (intSuccessful == 1)
                            {
                                message = "Selected Mini Bar Item(s) has been added.";
                                // GetBindRoomInfo(roomNo);
                                //// Show successful msg

                                //litMessage.Text = DisplayHelper.GetSuccessfulMessageBox("Selected Mini Bar Item(s) has been added.");
                                //Session[SessionHelper.SessionTempAddedItem] = "";
                                //grdItemSelected.Rebind();

                                //BindRoomInfo(roomNo);
                            }
                            else
                            {
                                throw new UserFriendlyException("Fail to add the record.");

                            }
                        }
                        catch (Exception ex)
                        {
                            throw new UserFriendlyException(ex.Message);
                        }
                    }
                }
                else
                {
                    throw new UserFriendlyException("Please select the Item to proceed.");

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            return message;
        }
        #region Get the Price(Price, Total , Tax1,2,3 for inputPrice) by GST Inclusive
        public SalesPrice GetSalesPrice(double inputPrice, string postCodeKey)
        {
            try
            {
                SalesPrice price = new SalesPrice();
                bool blnGSTInclusive = dalgeneralprofile.IsGSTInclusive();
                if (blnGSTInclusive)
                {
                    Task<List<SalesPrice>> lstbd = _roomdalRepository.GetGSTInclusiveAmt(inputPrice, postCodeKey);
                    price = lstbd.Result[0];
                }
                else
                {
                    Task<List<SalesPrice>> lstbd = _roomdalRepository.GetGSTExclusiveAmt(inputPrice, postCodeKey);
                    price = lstbd.Result[0];
                }

                return price;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static string GetReservationRateDescription(string no, string itemDesc, string qty, string voucherNo)
        {
            string strReturnValue = "";

            try
            {
                if (!string.IsNullOrEmpty(voucherNo))
                    strReturnValue = "Laundry Voucher#" + voucherNo + ": " + no + ") " + itemDesc.Trim() + " x " + qty;
                else
                    strReturnValue = "MiniBar Charge: " + no + ") " + itemDesc.Trim() + " x " + qty;

            }
            catch (Exception ex)
            {
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
                //throw ex;
            }
            return strReturnValue;
        }
        #endregion

        #endregion



    }
}
