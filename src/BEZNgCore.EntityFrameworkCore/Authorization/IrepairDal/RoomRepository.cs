using Abp.Data;
using Abp.EntityFrameworkCore;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.EntityFrameworkCore;
using BEZNgCore.EntityFrameworkCore.Repositories;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BEZNgCore.Authorization.IrepairDal
{
    public class RoomRepository : BEZNgCoreRepositoryBase<Room, Guid>, IRoomRepository
    {
        private readonly IActiveTransactionProvider _transactionProvider;
        ConnectionManager _connectionManager;
        public RoomRepository(IDbContextProvider<BEZNgCoreDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
            _connectionManager = new ConnectionManager(dbContextProvider, _transactionProvider);
        }


        #region interfaceimplement & related function 

        public async Task<List<GetHotelRoomFloorOutPut>> GetHotelRoomFloor()
        {
            var result = new List<GetHotelRoomFloorOutPut>();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetHotelRoomFloorQuery(), CommandType.Text, MultiTenancySide))
            {
                using (var dr = await command.ExecuteReaderAsync())
                {

                    while (dr.Read())
                    {
                        GetHotelRoomFloorOutPut g = new GetHotelRoomFloorOutPut();
                        g.Floor = Convert.ToInt32(dr["Floor"]);
                        result.Add(g);
                    }
                    dr.Close();
                }
            }
            return result;

        }

        public async Task<List<GetDashRoomByMaidKeyOutput>> GetRoomByMaidKey()
        {
            var result = new List<GetDashRoomByMaidKeyOutput>();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetHotelRoomFloorQuery(), CommandType.Text, MultiTenancySide))
            {
                using (var dr = await command.ExecuteReaderAsync())
                {

                    while (dr.Read())
                    {
                        GetDashRoomByMaidKeyOutput g = new GetDashRoomByMaidKeyOutput();
                        g.Floor = Convert.ToInt32(dr["Floor"]);
                        result.Add(g);
                    }
                    dr.Close();

                }
            }
            return result;
        }

        public async Task<List<MaidHasStartedTaskOutput>> GetRoomCountByMaidKey(DateTime dtBusinessDate, string maidKey, string floorNo)
        {
            var result = new List<MaidHasStartedTaskOutput>();
            bool blnFilterByFloorNo = false;
            if (!string.IsNullOrEmpty(floorNo) && !floorNo.Equals("0"))
                blnFilterByFloorNo = true;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetRoomCountByMaidKeyQuery(blnFilterByFloorNo, dtBusinessDate, maidKey, floorNo), CommandType.Text, MultiTenancySide))
            {
                using (var dr = await command.ExecuteReaderAsync())
                {

                    while (dr.Read())
                    {
                        MaidHasStartedTaskOutput g = new MaidHasStartedTaskOutput();
                        g.MaidStatusKey = (!DBNull.Value.Equals(dr["MaidStatusKey"])) ? (!string.IsNullOrEmpty(dr["MaidStatusKey"].ToString()) ? new Guid(dr["MaidStatusKey"].ToString()) : Guid.Empty) : Guid.Empty;
                        g.MaidStatus = !DBNull.Value.Equals(dr["MaidStatus"]) ? dr["MaidStatus"].ToString() : "";
                        g.RoomCount = !DBNull.Value.Equals(dr["RoomCount"]) ? Convert.ToInt32(dr["RoomCount"]) : 0;
                        result.Add(g);
                    }
                    dr.Close();

                }
            }
            return result;
        }

        public async Task<List<GetDashRoomByMaidStatusKeyOutput>> GetSupervisorRoomByMaidStatusKey(DateTime dtBusinessDate, string maidKey, string floorNo, string maidStatusKey, string roomStatusKey)
        {
            DataTable dt = new DataTable();
            var result = new List<GetDashRoomByMaidStatusKeyOutput>();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            try
            {
                bool blnFilterByMaidStatus = false;
                if (!string.IsNullOrEmpty(maidStatusKey) && !maidStatusKey.Equals(Guid.Empty.ToString()))
                    blnFilterByMaidStatus = true;
                bool blnFilterByMaidKey = false;
                if (!string.IsNullOrEmpty(maidKey) && !maidKey.Equals(Guid.Empty.ToString()))
                    blnFilterByMaidKey = true;
                bool blnFilterByFloorNo = false;
                if (!string.IsNullOrEmpty(floorNo) && !floorNo.Equals("0"))
                    blnFilterByFloorNo = true;
                bool blnFilterByRoomStatusKey = false;
                if (!string.IsNullOrEmpty(roomStatusKey) && !roomStatusKey.Equals(Guid.Empty.ToString()))
                    blnFilterByRoomStatusKey = true;
                SqlParameter[] parameters = new SqlParameter[]
              {

                new SqlParameter("@date", SqlDbType.DateTime)
                {
                    Value = dtBusinessDate
                },
                new SqlParameter("@MaidKey", SqlDbType.UniqueIdentifier)
                {
                    Value = blnFilterByMaidKey==true?Guid.Parse(maidKey):DBNull.Value
                },
                new SqlParameter("@MaidStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value = blnFilterByMaidStatus==true?Guid.Parse(maidStatusKey):DBNull.Value
                },
                new SqlParameter("@RoomStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value = blnFilterByRoomStatusKey==true?Guid.Parse(roomStatusKey):DBNull.Value
                },
                new SqlParameter("@FloorNo", SqlDbType.VarChar)
                {
                    Value = blnFilterByFloorNo==true?floorNo.Trim():DBNull.Value
                }
              };

                using (var command = _connectionManager.CreateCommandSP(GetSupervisorRoomByMaidStatusKeyQuery(blnFilterByMaidKey, blnFilterByFloorNo, blnFilterByRoomStatusKey, blnFilterByMaidStatus), CommandType.Text, MultiTenancySide, parameters))
                {
                    using (var dr = await command.ExecuteReaderAsync())
                    {

                        while (dr.Read())
                        {
                            GetDashRoomByMaidStatusKeyOutput g = new GetDashRoomByMaidStatusKeyOutput();
                            g.RoomKey = (!DBNull.Value.Equals(dr["RoomKey"])) ? (!string.IsNullOrEmpty(dr["RoomKey"].ToString()) ? new Guid(dr["RoomKey"].ToString()) : Guid.Empty) : Guid.Empty;
                            g.Unit = !DBNull.Value.Equals(dr["Unit"]) ? dr["Unit"].ToString() : "";
                            g.MaidStatusKey = (!DBNull.Value.Equals(dr["MaidStatusKey"])) ? (!string.IsNullOrEmpty(dr["MaidStatusKey"].ToString()) ? new Guid(dr["MaidStatusKey"].ToString()) : Guid.Empty) : Guid.Empty;
                            g.MaidStatus = !DBNull.Value.Equals(dr["MaidStatus"]) ? dr["MaidStatus"].ToString() : "";
                            g.RoomStatusKey = (!DBNull.Value.Equals(dr["RoomStatusKey"])) ? (!string.IsNullOrEmpty(dr["RoomStatusKey"].ToString()) ? new Guid(dr["RoomStatusKey"].ToString()) : Guid.Empty) : Guid.Empty;
                            g.RoomStatus = !DBNull.Value.Equals(dr["RoomStatus"]) ? dr["RoomStatus"].ToString() : "";
                            g.RoomType = !DBNull.Value.Equals(dr["RoomType"]) ? dr["RoomType"].ToString() : "";
                            g.InterconnectRoom = !DBNull.Value.Equals(dr["InterconnectRoom"]) ? dr["InterconnectRoom"].ToString() : "";
                            g.Floor = !DBNull.Value.Equals(dr["Floor"]) ? Convert.ToInt32(dr["Floor"]) : 0;
                            g.LinenChange = !DBNull.Value.Equals(dr["LinenChange"]) ? dr["LinenChange"].ToString() : "";
                            g.DND = !DBNull.Value.Equals(dr["DND"]) ? Convert.ToInt32(dr["DND"]) : 0;
                            g.CleaningTime = !DBNull.Value.Equals(dr["CleaningTime"]) ? dr["CleaningTime"].ToString() : "";
                            g.LinenDays = !DBNull.Value.Equals(dr["LinenDays"]) ? Convert.ToInt32(dr["LinenDays"]) : 0;
                            g.Bed = !DBNull.Value.Equals(dr["Bed"]) ? Convert.ToInt32(dr["Bed"]) : 0;
                            g.Maid = !DBNull.Value.Equals(dr["Maid"]) ? dr["Maid"].ToString() : "";
                            g.HMMNotes = !DBNull.Value.Equals(dr["HMMNotes"]) ? dr["HMMNotes"].ToString() : "";
                            g.GuestArrived = !DBNull.Value.Equals(dr["GuestArrived"]) ? dr["GuestArrived"].ToString() : "";
                            g.Status = !DBNull.Value.Equals(dr["Status"]) ? Convert.ToInt32(dr["Status"]) : 0;
                            g.PreCheckInCount = !DBNull.Value.Equals(dr["PreCheckInCount"]) ? Convert.ToInt32(dr["PreCheckInCount"]) : 0;

                            result.Add(g);
                        }
                        dr.Close();
                    }


                }

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<GetSupervisorModeOutput>> GetSupervisorModeBindGrid(DateTime dtBusinessDate, string maidKey, string floorNo, string maidStatusKey, string roomStatusKey)
        {
            var result = new List<GetSupervisorModeOutput>();
            DataTable dt = new DataTable();
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                bool blnFilterByMaidStatus = false;
                if (!string.IsNullOrEmpty(maidStatusKey) && !maidStatusKey.Equals(Guid.Empty.ToString()))
                    blnFilterByMaidStatus = true;
                bool blnFilterByMaidKey = false;
                if (!string.IsNullOrEmpty(maidKey) && !maidKey.Equals(Guid.Empty.ToString()))
                    blnFilterByMaidKey = true;
                bool blnFilterByFloorNo = false;
                if (!string.IsNullOrEmpty(floorNo) && !floorNo.Equals("0"))
                    blnFilterByFloorNo = true;
                bool blnFilterByRoomStatusKey = false;
                if (!string.IsNullOrEmpty(roomStatusKey) && !roomStatusKey.Equals(Guid.Empty.ToString()))
                    blnFilterByRoomStatusKey = true;
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@date", SqlDbType.DateTime)
                {
                    Value = dtBusinessDate
                },
                new SqlParameter("@MaidStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value = blnFilterByMaidStatus==true?Guid.Parse(maidStatusKey):DBNull.Value
                },
                new SqlParameter("@MaidKey", SqlDbType.UniqueIdentifier)
                {
                    Value =blnFilterByMaidKey==true? Guid.Parse(maidKey) :DBNull.Value
                },
                new SqlParameter("@FloorNo", SqlDbType.VarChar)
                {
                    Value =blnFilterByFloorNo==true?floorNo.Trim():DBNull.Value
                },
                new SqlParameter("@RoomStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value =blnFilterByRoomStatusKey==true? Guid.Parse(roomStatusKey):DBNull.Value
                }
                 };

                using (var command = _connectionManager.CreateCommandSP(GetSupervisorRoomByMaidStatusKeyQuery(blnFilterByMaidKey, blnFilterByFloorNo, blnFilterByRoomStatusKey, blnFilterByMaidStatus), CommandType.Text, MultiTenancySide, parameters))
                {
                    using (var dr = await command.ExecuteReaderAsync())
                    {

                        while (dr.Read())
                        {
                            GetSupervisorModeOutput g = new GetSupervisorModeOutput();
                            g.RoomKey = (!DBNull.Value.Equals(dr["RoomKey"])) ? (!string.IsNullOrEmpty(dr["RoomKey"].ToString()) ? new Guid(dr["RoomKey"].ToString()) : Guid.Empty) : Guid.Empty;
                            g.Unit = !DBNull.Value.Equals(dr["Unit"]) ? dr["Unit"].ToString() : "";
                            g.RoomStatus = !DBNull.Value.Equals(dr["RoomStatus"]) ? dr["RoomStatus"].ToString() : "";
                            g.RoomType = !DBNull.Value.Equals(dr["RoomType"]) ? dr["RoomType"].ToString() : "";
                            g.MaidStatus = !DBNull.Value.Equals(dr["MaidStatus"]) ? dr["MaidStatus"].ToString() : "";
                            g.InterconnectRoom = !DBNull.Value.Equals(dr["InterconnectRoom"]) ? dr["InterconnectRoom"].ToString() : "";
                            g.LinenChange = !DBNull.Value.Equals(dr["LinenChange"]) ? dr["LinenChange"].ToString() : "";
                            g.DND = !DBNull.Value.Equals(dr["DND"]) ? Convert.ToInt32(dr["DND"]) : 0;
                            g.Maid = !DBNull.Value.Equals(dr["Maid"]) ? dr["Maid"].ToString() : "";
                            g.HMMNotes = !DBNull.Value.Equals(dr["HMMNotes"]) ? dr["HMMNotes"].ToString() : "";
                            g.GuestArrived = !DBNull.Value.Equals(dr["GuestArrived"]) ? dr["GuestArrived"].ToString() == "99:99" ? "" : dr["GuestArrived"].ToString() : "";
                            g.GuestArrivedOrign = !DBNull.Value.Equals(dr["GuestArrived"]) ? dr["GuestArrived"].ToString() : "";
                            g.Status = !DBNull.Value.Equals(dr["Status"]) ? Convert.ToInt32(dr["Status"]) : 0;
                            g.PreCheckInCount = !DBNull.Value.Equals(dr["PreCheckInCount"]) ? Convert.ToInt32(dr["PreCheckInCount"]) : 0;
                            g.Adult = !DBNull.Value.Equals(dr["Adult"]) ? Convert.ToInt32(dr["Adult"]) : 0;
                            g.Child = !DBNull.Value.Equals(dr["Child"]) ? Convert.ToInt32(dr["Child"]) : 0;
                            g.GuestStatus = !DBNull.Value.Equals(dr["GuestStatus"]) ? dr["GuestStatus"].ToString() : "";
                            g.ReservationKey = (!DBNull.Value.Equals(dr["ReservationKey"])) ? (!string.IsNullOrEmpty(dr["ReservationKey"].ToString()) ? new Guid(dr["ReservationKey"].ToString()) : Guid.Empty) : Guid.Empty;
                            #region MobileUI
                            if (g.ReservationKey != Guid.Empty)
                            {
                                g.Items = GetItemCode(g.ReservationKey);
                            }
                            else { g.Items = ""; }
                            g.Pax = GetPax(g.Adult.ToString(), g.Child.ToString());
                            g.GetGuestArrived = GetGuestArrived(g.GuestArrivedOrign.ToString(), g.Status.ToString());
                            g.PreCheckInCountDes = g.PreCheckInCount.ToString() != "1" ? "" : "(Pre Check-In)";
                            g.LinenChangeDes = g.LinenChange.Equals("Y") ? "Yes" : "No";
                            g.RoomStatusDes = GetHoldReason(dtBusinessDate, g.RoomStatus, g.RoomKey);
                            g.RoomStatusColor = GetRoomStatus(g.RoomStatus);
                            g.AttendantStatusColor = GetMaidStatus(g.MaidStatus);
                            g.GetRoomDNDButton = GetRoomDNDButton(g.MaidStatus, g.DND, g.Unit);
                            g.GetRoomCleanButton = GetRoomCleanButton(g.Unit, g.DND);
                            g.GetRoomDirtyButton = GetRoomDirtyButton(g.Unit, g.DND);
                            g.GetHistoryLog = GetHistoryLog(g.RoomKey, "");
                            #endregion
                            #region DndImg
                            g.ContentType=!DBNull.Value.Equals(dr["Document"]) ? dr["Document"].ToString() : "";
                            g.Data = !DBNull.Value.Equals(dr["Image"]) ? dr["Image"] as byte[] : Array.Empty<byte>();
                            var base64Image = Convert.ToBase64String(g.Data);
                            g.imageSrc = $"data:{g.ContentType};base64,{base64Image}";
                            #endregion

                            result.Add(g);
                        }
                        dr.Close();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetSupervisorModeOutput> GetSupervisorModeBindGridPaginate(DateTime dtBusinessDate, string maidKey, string floorNo, string maidStatusKey, string roomStatusKey, int currentPage, int pageSize, out int totalRowCount)
        {

            totalRowCount = 0;
            var result = new List<GetSupervisorModeOutput>();
            DataTable dt = new DataTable();
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                bool blnFilterByMaidStatus = false;
                if (!string.IsNullOrEmpty(maidStatusKey) && !maidStatusKey.Equals(Guid.Empty.ToString()))
                    blnFilterByMaidStatus = true;
                bool blnFilterByMaidKey = false;
                if (!string.IsNullOrEmpty(maidKey) && !maidKey.Equals(Guid.Empty.ToString()))
                    blnFilterByMaidKey = true;
                bool blnFilterByFloorNo = false;
                if (!string.IsNullOrEmpty(floorNo) && !floorNo.Equals("0"))
                    blnFilterByFloorNo = true;
                bool blnFilterByRoomStatusKey = false;
                if (!string.IsNullOrEmpty(roomStatusKey) && !roomStatusKey.Equals(Guid.Empty.ToString()))
                    blnFilterByRoomStatusKey = true;
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@date", SqlDbType.DateTime)
                {
                    Value = dtBusinessDate
                },
                new SqlParameter("@MaidStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value = blnFilterByMaidStatus==true?Guid.Parse(maidStatusKey):DBNull.Value
                },
                new SqlParameter("@MaidKey", SqlDbType.UniqueIdentifier)
                {
                    Value =blnFilterByMaidKey==true? Guid.Parse(maidKey) :DBNull.Value
                },
                new SqlParameter("@FloorNo", SqlDbType.VarChar)
                {
                    Value =blnFilterByFloorNo==true?floorNo.Trim():DBNull.Value
                },
                new SqlParameter("@RoomStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value =blnFilterByRoomStatusKey==true? Guid.Parse(roomStatusKey):DBNull.Value
                }
            };
                using (var command = _connectionManager.CreateCommandSP(GetSupervisorRoomByMaidStatusKeyQuery(blnFilterByMaidKey, blnFilterByFloorNo, blnFilterByRoomStatusKey, blnFilterByMaidStatus), CommandType.Text, MultiTenancySide, parameters))
                {
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                    {
                        adapter.Fill(ds);
                    }
                    if (ds.Tables.Count == 1)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            totalRowCount = ds.Tables[0].Rows.Count;
                            dt = ds.Tables[0].Select().Skip(pageSize * (currentPage - 1)).Take(pageSize).CopyToDataTable();

                        }

                    }

                }
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        GetSupervisorModeOutput g = new GetSupervisorModeOutput();
                        g.RoomKey = (!DBNull.Value.Equals(dr["RoomKey"])) ? (!string.IsNullOrEmpty(dr["RoomKey"].ToString()) ? new Guid(dr["RoomKey"].ToString()) : Guid.Empty) : Guid.Empty;
                        g.Unit = !DBNull.Value.Equals(dr["Unit"]) ? dr["Unit"].ToString() : "";
                        g.RoomStatus = !DBNull.Value.Equals(dr["RoomStatus"]) ? dr["RoomStatus"].ToString() : "";
                        g.RoomType = !DBNull.Value.Equals(dr["RoomType"]) ? dr["RoomType"].ToString() : "";
                        g.MaidStatus = !DBNull.Value.Equals(dr["MaidStatus"]) ? dr["MaidStatus"].ToString() : "";
                        g.InterconnectRoom = !DBNull.Value.Equals(dr["InterconnectRoom"]) ? dr["InterconnectRoom"].ToString() : "";
                        g.LinenChange = !DBNull.Value.Equals(dr["LinenChange"]) ? dr["LinenChange"].ToString() : "";
                        g.DND = !DBNull.Value.Equals(dr["DND"]) ? Convert.ToInt32(dr["DND"]) : 0;
                        g.Maid = !DBNull.Value.Equals(dr["Maid"]) ? dr["Maid"].ToString() : "";
                        g.HMMNotes = !DBNull.Value.Equals(dr["HMMNotes"]) ? dr["HMMNotes"].ToString() : "";
                        g.GuestArrived = !DBNull.Value.Equals(dr["GuestArrived"]) ? dr["GuestArrived"].ToString() == "99:99" ? "" : dr["GuestArrived"].ToString() : "";
                        g.GuestArrivedOrign = !DBNull.Value.Equals(dr["GuestArrived"]) ? dr["GuestArrived"].ToString() : "";
                        g.Status = !DBNull.Value.Equals(dr["Status"]) ? Convert.ToInt32(dr["Status"]) : 0;
                        g.PreCheckInCount = !DBNull.Value.Equals(dr["PreCheckInCount"]) ? Convert.ToInt32(dr["PreCheckInCount"]) : 0;
                        g.Adult = !DBNull.Value.Equals(dr["Adult"]) ? Convert.ToInt32(dr["Adult"]) : 0;
                        g.Child = !DBNull.Value.Equals(dr["Child"]) ? Convert.ToInt32(dr["Child"]) : 0;
                        g.GuestStatus = !DBNull.Value.Equals(dr["GuestStatus"]) ? dr["GuestStatus"].ToString() : "";
                        g.ReservationKey = (!DBNull.Value.Equals(dr["ReservationKey"])) ? (!string.IsNullOrEmpty(dr["ReservationKey"].ToString()) ? new Guid(dr["ReservationKey"].ToString()) : Guid.Empty) : Guid.Empty;
                        #region MobileUI
                        //if (g.ReservationKey != Guid.Empty)
                        //{
                        //    g.Items = GetItemCode(g.ReservationKey);
                        //}
                        //else { g.Items = ""; }
                        g.Items= !DBNull.Value.Equals(dr["ItemCodes"]) ? dr["ItemCodes"].ToString() : "";
                        g.Pax = GetPax(g.Adult.ToString(), g.Child.ToString());
                        g.GetGuestArrived = GetGuestArrived(g.GuestArrivedOrign.ToString(), g.Status.ToString());
                        g.PreCheckInCountDes = g.PreCheckInCount.ToString() != "1" ? "" : "(Pre Check-In)";
                        g.LinenChangeDes = g.LinenChange.Equals("Y") ? "Yes" : "No";
                        g.RoomStatusDes = GetHoldReason(dtBusinessDate, g.RoomStatus, g.RoomKey);
                        g.RoomStatusColor = GetRoomStatus(g.RoomStatus);
                        g.AttendantStatusColor = GetMaidStatus(g.MaidStatus);
                        g.GetRoomDNDButton = GetRoomDNDButton(g.MaidStatus, g.DND, g.Unit);
                        g.GetRoomCleanButton = GetRoomCleanButton(g.Unit, g.DND);
                        g.GetRoomDirtyButton = GetRoomDirtyButton(g.Unit, g.DND);
                        g.GetHistoryLog = GetHistoryLog(g.RoomKey, "");
                        #endregion
                        #region DndImg
                        g.ContentType = !DBNull.Value.Equals(dr["Document"]) ? dr["Document"].ToString() : "";
                        g.Data = !DBNull.Value.Equals(dr["Image"]) ? dr["Image"] as byte[] : Array.Empty<byte>();
                        var base64Image = Convert.ToBase64String(g.Data);
                        g.imageSrc = $"data:{g.ContentType};base64,{base64Image}";
                        #endregion
                        result.Add(g);
                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }





        }
        public async Task<string> GetDateAsync()
        {
            string dt = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly("SELECT GETDATE() as sysdate", CommandType.Text, MultiTenancySide))
            {
                using (var dr = await command.ExecuteReaderAsync())
                {
                    while (dr.Read())
                    {
                        dt = !DBNull.Value.Equals(dr["sysdate"]) ? dr["sysdate"].ToString() : "";

                    }
                    dr.Close();
                }
            }
            return dt;
        }
        public async Task<string> staffmaidkey(Guid staffKey)
        {
            string dt = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly("SELECT maidkey from staff where StaffKey='" + staffKey + "'", CommandType.Text, MultiTenancySide))
            {
                using (var dr = await command.ExecuteReaderAsync())
                {
                    while (dr.Read())
                    {
                        dt = !DBNull.Value.Equals(dr["maidkey"]) ? dr["maidkey"].ToString() : "";

                    }
                    dr.Close();

                }
            }
            return dt;
        }
        public async Task<string> getAllowCleanDirectly(Guid staffKey)
        {
            string dt = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly("SELECT Sec_AllowCleanDirectly from staff where StaffKey='" + staffKey + "'", CommandType.Text, MultiTenancySide))
            {
                using (var dr = await command.ExecuteReaderAsync())
                {
                    while (dr.Read())
                    {
                        dt = !DBNull.Value.Equals(dr["Sec_AllowCleanDirectly"]) ? dr["Sec_AllowCleanDirectly"].ToString() : "0";

                    }
                    dr.Close();

                }
            }
            return dt;
        }
        #region Display Maid Status Helper
        //MaidStatus
        //Inspection Required
        //Dirty
        //Maid In Room
        //Clean
        public static string GetMaidStatus(object status)
        {
            string strReturnValue = status.ToString();
            try
            {
                switch (status.ToString())
                {
                    case "Dirty":
                        strReturnValue = "#c9302c";// "<span style=\"color: #c9302c\"><b>" + status + "</b></span>";
                        break;
                    case "Clean":
                        strReturnValue = "#5cb85c";// "<span style=\"color: #5cb85c\"><b>" + status + "</b></span>";
                        break;
                    default:
                        strReturnValue = "#0000ff";// "<span class=\"text-blue\"><b>" + status + "</b></span>";
                        break;
                }
                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }
        #endregion


        public DataTable GetContactListByMsgCode(string msgCode)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetContactListByMsgCodeQuery(msgCode), CommandType.Text, MultiTenancySide))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    dt = ds.Tables[0];
                }

            }
            return dt;

        }
        public DataTable GetIRContactListByMsgCode(string msgCode)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetIRContactListByMsgCodeQuery(msgCode), CommandType.Text, MultiTenancySide))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    dt = ds.Tables[0];
                }

            }
            return dt;

        }
        public DataTable GetLaundryItemByResKey(string resKey)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetLaundryItemByResKeyQuery(resKey), CommandType.Text, MultiTenancySide))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    dt = ds.Tables[0];
                }

            }
            return dt;

        }
        public DataTable GetMinibarItemByResKey(string resKey)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetMinibarItemByResKeyQuery(resKey), CommandType.Text, MultiTenancySide))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    dt = ds.Tables[0];
                }

            }
            return dt;

        }
        public async Task<List<GetDashRoomByMaidStatusKeyOutput>> GetRoomByMaidStatusKey(DateTime dtBusinessDate, string maidStatusKey, string maidKey, string floorNo, string roomStatusKey)
        {
            var result = new List<GetDashRoomByMaidStatusKeyOutput>();
            DataTable dt = new DataTable();
            try
            {
                bool blnFilterByMaidKey = false;
                if (!string.IsNullOrEmpty(maidKey) && !maidKey.Equals(Guid.Empty.ToString()))
                    blnFilterByMaidKey = true;
                bool blnFilterByFloorNo = false;
                if (!string.IsNullOrEmpty(floorNo) && !floorNo.Equals("0"))
                    blnFilterByFloorNo = true;
                bool blnFilterByRoomStatusKey = false;
                if (!string.IsNullOrEmpty(roomStatusKey) && !roomStatusKey.Equals(Guid.Empty.ToString()))
                    blnFilterByRoomStatusKey = true;
                SqlParameter[] parameters = new SqlParameter[]
              {

                new SqlParameter("@date", SqlDbType.DateTime)
                {
                    Value = dtBusinessDate
                },
                new SqlParameter("@MaidKey", SqlDbType.UniqueIdentifier)
                {
                    Value = blnFilterByMaidKey==true?Guid.Parse(maidKey):DBNull.Value
                },
                new SqlParameter("@MaidStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(maidStatusKey)
                },
                new SqlParameter("@RoomStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value = blnFilterByRoomStatusKey==true?Guid.Parse(roomStatusKey):DBNull.Value
                },
                new SqlParameter("@FloorNo", SqlDbType.VarChar)
                {
                    Value = blnFilterByFloorNo==true?floorNo.Trim():DBNull.Value
                }
              };
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                using (var command = _connectionManager.CreateCommandSP(GetRoomByMaidStatusKeyQuery(blnFilterByMaidKey, blnFilterByFloorNo, blnFilterByRoomStatusKey), CommandType.Text, MultiTenancySide, parameters))
                {
                    using (var dr = await command.ExecuteReaderAsync())
                    {

                        while (dr.Read())
                        {
                            GetDashRoomByMaidStatusKeyOutput g = new GetDashRoomByMaidStatusKeyOutput();
                            g.RoomKey = (!DBNull.Value.Equals(dr["RoomKey"])) ? (!string.IsNullOrEmpty(dr["RoomKey"].ToString()) ? new Guid(dr["RoomKey"].ToString()) : Guid.Empty) : Guid.Empty;
                            g.Unit = !DBNull.Value.Equals(dr["Unit"]) ? dr["Unit"].ToString() : "";
                            g.MaidStatusKey = (!DBNull.Value.Equals(dr["MaidStatusKey"])) ? (!string.IsNullOrEmpty(dr["MaidStatusKey"].ToString()) ? new Guid(dr["MaidStatusKey"].ToString()) : Guid.Empty) : Guid.Empty;
                            g.MaidStatus = !DBNull.Value.Equals(dr["MaidStatus"]) ? dr["MaidStatus"].ToString() : "";
                            g.GuestStatus = !DBNull.Value.Equals(dr["GuestStatus"]) ? dr["GuestStatus"].ToString() : "";
                            g.RoomStatusKey = (!DBNull.Value.Equals(dr["RoomStatusKey"])) ? (!string.IsNullOrEmpty(dr["RoomStatusKey"].ToString()) ? new Guid(dr["RoomStatusKey"].ToString()) : Guid.Empty) : Guid.Empty;
                            g.RoomStatus = !DBNull.Value.Equals(dr["RoomStatus"]) ? dr["RoomStatus"].ToString() : "";
                            g.RoomType = !DBNull.Value.Equals(dr["RoomType"]) ? dr["RoomType"].ToString() : "";
                            g.InterconnectRoom = !DBNull.Value.Equals(dr["InterconnectRoom"]) ? dr["InterconnectRoom"].ToString() : "";
                            g.Floor = !DBNull.Value.Equals(dr["Floor"]) ? Convert.ToInt32(dr["Floor"]) : 0;
                            g.LinenChange = !DBNull.Value.Equals(dr["LinenChange"]) ? dr["LinenChange"].ToString() : "";
                            g.DND = !DBNull.Value.Equals(dr["DND"]) ? Convert.ToInt32(dr["DND"]) : 0;
                            g.CleaningTime = !DBNull.Value.Equals(dr["CleaningTime"]) ? dr["CleaningTime"].ToString() : "";
                            g.LinenDays = !DBNull.Value.Equals(dr["LinenDays"]) ? Convert.ToInt32(dr["LinenDays"]) : 0;
                            g.Bed = !DBNull.Value.Equals(dr["Bed"]) ? Convert.ToInt32(dr["Bed"]) : 0;
                            g.Maid = !DBNull.Value.Equals(dr["Maid"]) ? dr["Maid"].ToString() : "";
                            g.HMMNotes = !DBNull.Value.Equals(dr["HMMNotes"]) ? dr["HMMNotes"].ToString() : "";
                            g.GuestArrived = !DBNull.Value.Equals(dr["GuestArrived"]) ? dr["GuestArrived"].ToString() == "99:99" ? "" : dr["GuestArrived"].ToString() : "";
                            g.GuestArrivedOrign = !DBNull.Value.Equals(dr["GuestArrived"]) ? dr["GuestArrived"].ToString() : "";
                            g.Status = !DBNull.Value.Equals(dr["Status"]) ? Convert.ToInt32(dr["Status"]) : 0;
                            g.PreCheckInCount = !DBNull.Value.Equals(dr["PreCheckInCount"]) ? Convert.ToInt32(dr["PreCheckInCount"]) : 0;
                            g.Adult = !DBNull.Value.Equals(dr["Adult"]) ? Convert.ToInt32(dr["Adult"]) : 0;
                            g.Child = !DBNull.Value.Equals(dr["Child"]) ? Convert.ToInt32(dr["Child"]) : 0;
                            g.Group1 = !DBNull.Value.Equals(dr["Group1"]) ? dr["Group1"].ToString() : "";
                            g.Group2 = !DBNull.Value.Equals(dr["Group2"]) ? dr["Group2"].ToString() : "";
                            g.Group3 = !DBNull.Value.Equals(dr["Group3"]) ? dr["Group3"].ToString() : "";
                            g.Group4 = !DBNull.Value.Equals(dr["Group4"]) ? dr["Group4"].ToString() : "";
                            //g.ETA = !DBNull.Value.Equals(dr["ETA"]) ? dr["ETA"].ToString() : "";
                            g.ETA = GetETA(dtBusinessDate, g.RoomKey);
                            g.ETD = !DBNull.Value.Equals(dr["ETD"]) ? dr["ETD"].ToString() : "";
                            g.ReservationKey = (!DBNull.Value.Equals(dr["ReservationKey"])) ? (!string.IsNullOrEmpty(dr["ReservationKey"].ToString()) ? new Guid(dr["ReservationKey"].ToString()) : Guid.Empty) : Guid.Empty;
                            #region MobileUI
                            if (g.ReservationKey != Guid.Empty)
                            {
                                g.Items = GetItemCode(g.ReservationKey);
                            }
                            else { g.Items = ""; }
                            g.GetGuestArrived = GetGuestArrived(g.GuestArrivedOrign.ToString(), g.Status.ToString());
                            g.PreCheckInCountDes = g.PreCheckInCount.ToString() != "1" ? "" : "(Pre Check-In)";
                            g.LinenChangeDes = g.LinenChange.Equals("Y") ? "Yes" : "No";
                            g.RoomStatusDes = GetHoldReason(dtBusinessDate, g.RoomStatus, g.RoomKey);
                            g.RoomStatusColor = GetRoomStatus(g.RoomStatus);
                            g.GetRoomDNDButton = GetRoomDNDButton(g.MaidStatus, g.DND, g.Unit);
                            g.GetRoomCleanButton = GetRoomCleanButton(g.Unit, g.DND);
                            g.GetRoomDirtyButton = GetRoomDirtyButton(g.Unit, g.DND);
                            g.GetLinenItemButton = GetLinenItemButton(g.Unit, g.DND);
                            g.GetHistoryLog = GetHistoryLog(g.RoomKey, "");
                            g.Pax = GetPax(g.Adult.ToString(), g.Child.ToString());
                            g.MarketSegment = GetMKG(g.Group1, g.Group2, g.Group3, g.Group4);
                            g.GetOptButton = GetRoomOptButton(g.MaidStatus,g.Unit);
                            #endregion
                            #region DndImg
                            g.ContentType = !DBNull.Value.Equals(dr["Document"]) ? dr["Document"].ToString() : "";
                            g.Data = !DBNull.Value.Equals(dr["Image"]) ? dr["Image"] as byte[] : Array.Empty<byte>();
                            var base64Image = Convert.ToBase64String(g.Data);
                            g.imageSrc = $"data:{g.ContentType};base64,{base64Image}";
                            #endregion
                            result.Add(g);
                        }
                        dr.Close();
                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private string GetItemCode(Guid? reservationKey)
        {
            string strReturnValue = "";
            try
            {
                DataTable dt = GetItemByReservationKey(reservationKey.ToString());
                if (dt.Rows.Count > 0)
                {
                    List<string> list = new List<string>();
                    foreach (DataRow row in dt.Rows)
                    {
                        string code = !DBNull.Value.Equals(row["Code"]) ? row["Code"].ToString() : "";
                        if (code != string.Empty)
                        {
                            list.Add(code);
                        }
                    }

                    //strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 1);
                    string[] array = list.ToArray();
                    strReturnValue = string.Join(",", array);
                }
                else
                {
                    strReturnValue = "";
                }
                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }
        private DataTable GetItemByReservationKey(string reskey)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(reskey)
                }
              };

            using (var command = _connectionManager.CreateCommandSP(GetItemByReservationKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
            {

                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    return ds.Tables[0];
                }
                else { return dt; }
            }
        }


        public DataTable GetStaffInfoByRoomNo(string roomNo)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetStaffInfoByRoomNoQuery(roomNo), CommandType.Text, MultiTenancySide))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    dt = ds.Tables[0];
                }

            }
            return dt;

        }

        #region MobileView
        public static string GetRoomCleanButton(object roomNo, object dndStatus)
        {
            string strReturnValue = "";
            try
            {
                if (!dndStatus.ToString().Equals("1"))
                    strReturnValue = "btnClean"; //"<a  id=\"btnClean" + roomNo.ToString() + "\" runat=\"server\"   class=\"btn btn-success btn-lg\"  href=\"javascript:CPhysicalInspection('" + roomNo.ToString() + "');\" > <span class=\"text-white\" style=\"\"> <i class=\"fa fa-thumbs-up fa-fw\"></i> Clean </span></a>";
                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }
        public static string GetRoomDirtyButton(object roomNo, object dndStatus)
        {
            string strReturnValue = "";
            try
            {
                if (!dndStatus.ToString().Equals("1"))
                    strReturnValue = "btnDirty";// "<a  id=\"btnDirty" + roomNo.ToString() + "\" runat=\"server\" style=\"margin-left: 6px;\"  class=\"btn btn-danger btn-lg\"  href=\"javascript:DPhysicalInspection('" + roomNo.ToString() + "');\" > <span class=\"text-white\" style=\"\"> <i class=\"fa fa-thumbs-down fa-fw\"></i> Dirty </span></a>";
                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }
        public static string GetLinenItemButton(object roomNo, object dndStatus)
        {
            string strReturnValue = "";
            try
            {
                if (!dndStatus.ToString().Equals("1"))
                    strReturnValue = "btnLinenSheet";//"<br /> <a  id=\"btnLinenSheet" + roomNo.ToString() + "\" runat=\"server\" style=\"margin-top: 10px;\"  class=\"btn btn-primary btn-lg\"  href=\"javascript:OnLinenSheetTask('" + roomNo.ToString() + "');\" >  <span class=\"text-white\" style=\"\"> <span class=\"glyphicon glyphicon-list-alt\" ></span> Supervisor Checklist </span></a>";

                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }
        public static string GetHistoryLog(object roomkey, string staffkey)
        {
            string strReturnValue = "";
            try
            {
                strReturnValue = "btnLog";//"<a id=\"btnLog\" runat=\"server\" class=\"btn btn-default btn-lg\" style=\"background-color: #E1BB20; margin-top: 10px;\" href=\"javascript:ShowLog('" + roomkey.ToString() + "', '" + staffkey + "');\" > <span class=\"text-white\"> <span class=\"fa fa-history\" ></span> History</span></a>";

                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }
        private string GetRoomDNDButton(string attendantStatus, int? dndStatus, string roomNo)
        {
            string strReturnValue = "";
            string HouseKeepingMaidStatusMaidInRoom = "Attendant";
            try
            {
                if (!attendantStatus.ToString().ToLower().Equals(HouseKeepingMaidStatusMaidInRoom.ToLower()))
                {
                    if (dndStatus.ToString().Equals("1"))
                    {
                        strReturnValue = "Disable";
                        //"<a  id=\"btnDND" + roomNo.ToString() + "\" runat=\"server\"   href=\"javascript:OnDisableDND('" + roomNo.ToString() + "');\" > <span class=\"text-red\" style=\"font-size:larger;\"> <i class=\"fa fa-toggle-on  fa-lg\"></i><b> DND </b></span></a>";
                    }
                    else
                    {
                        strReturnValue = "Enable";
                        //"<a  id=\"btnDND" + roomNo.ToString() + "\" runat=\"server\"   href=\"javascript:OnEnableDND('" + roomNo.ToString() + "');\" > <span class=\"text-primary\" style=\"font-size:larger;\"> <i class=\"fa fa-toggle-off fa-lg\"></i><b> DND </b></span></a>";
                    }
                }

                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }
        public string GetPax(object Adult, object Child)
        {

            string strReturnValue = "0 A 0 C";
            string AdultData = Adult.ToString();
            string ChildData = Child.ToString();
            AdultData = string.IsNullOrEmpty(AdultData) ? "0" : AdultData;
            ChildData = string.IsNullOrEmpty(ChildData) ? "0" : ChildData;
            strReturnValue = AdultData + " A " + ChildData + " C";
            return strReturnValue;
        }
        public string GetMKG(object g1, object g2, object g3, object g4)
        {
            string strReturnValue = "";
            string gg1 = g1.ToString();
            string gg2 = g2.ToString();
            string gg3 = g3.ToString();
            string gg4 = g4.ToString();
            gg1 = string.IsNullOrEmpty(gg1) ? "" : gg1 + " ";
            gg2 = string.IsNullOrEmpty(gg2) ? "" : gg2 + " ";
            gg3 = string.IsNullOrEmpty(gg3) ? "" : gg3 + " ";
            gg4 = string.IsNullOrEmpty(gg4) ? "" : gg4;
            strReturnValue = gg1 + gg2 + gg3 + gg4;
            return strReturnValue;
        }
        public string GetRoomOptButton(string attendantStatus, string roomNo)
        {

            string strReturnValue = "";
            string HouseKeepingMaidStatusMaidInRoom = "Attendant";
            try
            {
                if (!attendantStatus.ToString().ToLower().Equals(HouseKeepingMaidStatusMaidInRoom.ToLower()))
                {
                    int optstatus = GetOptStatus(roomNo);
                    if (optstatus.ToString().Equals("1"))
                    {
                        strReturnValue = "disableopt";
                        //strReturnValue = "<a  id=\"btnDND" + roomNo.ToString() + "\" runat=\"server\"   href=\"javascript:OnDisableOnOpt('" + roomNo.ToString() + "');\" > <span class=\"text-red\" style=\"font-size:larger;\"> <i class=\"fa fa-toggle-on  fa-lg\"></i><b> Opt Out </b></span></a>";
                    }
                    else
                    {
                        strReturnValue = "enableopt";
                        //strReturnValue = "<a  id=\"btnDND" + roomNo.ToString() + "\" runat=\"server\"   href=\"javascript:OnEnableOnOpt('" + roomNo.ToString() + "');\" > <span class=\"text-primary\" style=\"font-size:larger;\"> <i class=\"fa fa-toggle-off fa-lg\"></i><b> Opt Out </b></span></a>";
                    }
                }

                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }

        }
        private string GetETA(DateTime dtBusinessDate, Guid? roomKey)
        {
            string ETA = "";

            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
          {

                new SqlParameter("@roomkey", SqlDbType.UniqueIdentifier)
                {
                    Value = roomKey
                },
                new SqlParameter("@checkindate", SqlDbType.DateTime)
                {
                    Value = dtBusinessDate
                }
          };
            using (var command = _connectionManager.CreateCommandSP(GetETAQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {

                    dt = ds.Tables[0];

                }

            }
            if (dt.Rows.Count > 0)
            {
                ETA = dt.Rows[0]["ETA"].ToString();
            }

            return ETA;
        }
        private int GetOptStatus(string roomNo)
        {
            int status = 0;
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
          {

                new SqlParameter("@Unit", SqlDbType.VarChar)
                {
                    Value = (!string.IsNullOrEmpty(roomNo) && roomNo != "") ? roomNo : null
                }
          };
            using (var command = _connectionManager.CreateCommandSP(GetOptStatusQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {

                    dt = ds.Tables[0];

                }

            }
            if (dt.Rows.Count > 0)
            {
                status = Convert.ToInt32(dt.Rows[0]["RowCountStatus"]);
            }

            return status;

        }
        #region Display Hold Status Reason Helper
        public string GetHoldReason(DateTime dtBusinessDate, string status, object roomKey)
        {
            string strReturnValue = "";
            try
            {
                if (status.ToString() == "Hold")
                {
                    //DateTime dtBusinessDate = GetBusinessDate();
                    DataTable dt = GetRoomBlockByRoomKey(roomKey.ToString(), dtBusinessDate);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            strReturnValue = !string.IsNullOrEmpty(row["Reason"].ToString()) ? "(" + row["Reason"].ToString() + ")" : "";
                        }
                    }
                }
                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }
        public DataTable GetRoomBlockByRoomKey(string roomkey, DateTime sysDate)
        {

            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
             {

                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(roomkey)
                },
                new SqlParameter("@date", SqlDbType.DateTime)
                {
                    Value = sysDate
                }
             };

            using (var command = _connectionManager.CreateCommandSP(GetRoomBlockByRoomKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    return ds.Tables[0];
                }
                else { return dt; }
            }

        }
        #endregion
        #region Display Room Status Helper
        //RoomStatus
        //Vacant
        //Out Of Order
        //Hold
        //Due Out
        //Occupied
        public static string GetRoomStatus(object status)
        {
            string strReturnValue = status.ToString();
            try
            {
                switch (status.ToString())
                {
                    case "Occupied":
                        strReturnValue = "#FFB3B3";// "<span style=\"color: #FFB3B3\"><b>" + status + "</b></span>";
                        break;
                    case "Due Out":
                        strReturnValue = "#c9302c";//"<span style=\"color: #c9302c\"><b>" + status + "</b></span>";
                        break;
                    case "Vacant":
                        strReturnValue = "#5cb85c";//"<span style=\"color: #5cb85c\"><b>" + status + "</b></span>";
                        break;
                    default:
                        strReturnValue = "#0000ff";//"<span class=\"text-blue\"><b>" + status + "</b></span>";
                        break;
                }

                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }
        #endregion
        #region Display Guest Arrived Helper
        public static string GetGuestArrived(string inputValue, string status)
        {
            string strReturnValue = "";
            try
            {
                strReturnValue = inputValue.Trim();
                if (strReturnValue == "99:99" && status != "1")
                    return "";
                else if (strReturnValue == "99:99" && status == "1")
                    return "";
                else if (strReturnValue != "99:99" && status != "1")
                    return "";
                else if (string.IsNullOrEmpty(strReturnValue))
                    return "";
                else
                    strReturnValue = "(Guest Arrived)";
                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }

        #endregion
        #endregion
        public async Task<List<MaidStatusListOutPut>> BindMaidStatusListCount(DateTime dtBusinessDate, string maidKey, string floorNo)
        {
            var result = new List<MaidStatusListOutPut>();
            bool blnFilterByFloorNo = false;
            if (!string.IsNullOrEmpty(floorNo) && !floorNo.Equals("0"))
                blnFilterByFloorNo = true;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetRoomCountByMaidKeyQuery(blnFilterByFloorNo, dtBusinessDate, maidKey, floorNo), CommandType.Text, MultiTenancySide))
            {
                using (var dr = await command.ExecuteReaderAsync())
                {

                    while (dr.Read())
                    {
                        MaidStatusListOutPut g = new MaidStatusListOutPut();
                        //  g.MaidStatusKey = (!DBNull.Value.Equals(dr["MaidStatusKey"])) ? (!string.IsNullOrEmpty(dr["MaidStatusKey"].ToString()) ? new Guid(dr["MaidStatusKey"].ToString()) : Guid.Empty) : Guid.Empty;
                        g.MaidStatus = !DBNull.Value.Equals(dr["MaidStatus"]) ? dr["MaidStatus"].ToString() : "";
                        g.RoomCount = !DBNull.Value.Equals(dr["RoomCount"]) ? Convert.ToInt32(dr["RoomCount"]) : 0;
                        result.Add(g);
                    }
                    dr.Close();

                }
            }
            return result;
        }
        public List<MaidStatusListOutPut> BindMaidStatusListCountSup(DateTime dtBusinessDate, string maidKey, string floorNo)
        {
            var result = new List<MaidStatusListOutPut>();
            bool blnFilterByFloorNo = false;
            if (!string.IsNullOrEmpty(floorNo) && !floorNo.Equals("0"))
                blnFilterByFloorNo = true;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
             {

                new SqlParameter("@date", SqlDbType.DateTime)
                {
                    Value = dtBusinessDate
                },
                new SqlParameter("@MaidKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(maidKey)
                },
                new SqlParameter("@FloorNo", SqlDbType.VarChar)
                {
                    Value = blnFilterByFloorNo==true?floorNo.Trim():DBNull.Value
                }
             };

            using (var command = _connectionManager.CreateCommandSP(GetRoomCountQuerySup(blnFilterByFloorNo, dtBusinessDate, maidKey, floorNo), CommandType.Text, MultiTenancySide, parameters))
            {
                using (var dr = command.ExecuteReaderAsync())
                {

                    while (dr.Result.Read())
                    {
                        MaidStatusListOutPut g = new MaidStatusListOutPut();
                        //  g.MaidStatusKey = (!DBNull.Value.Equals(dr["MaidStatusKey"])) ? (!string.IsNullOrEmpty(dr["MaidStatusKey"].ToString()) ? new Guid(dr["MaidStatusKey"].ToString()) : Guid.Empty) : Guid.Empty;
                        g.MaidStatus = !DBNull.Value.Equals(dr.Result["MaidStatus"]) ? dr.Result["MaidStatus"].ToString() : "";
                        g.RoomCount = !DBNull.Value.Equals(dr.Result["RoomCount"]) ? Convert.ToInt32(dr.Result["RoomCount"]) : 0;
                        result.Add(g);
                    }
                    dr.Result.Close();

                }
            }
            return result;
        }

        public async Task<List<RoomStatusPageOutput>> RoomStatusBindGrid(DateTime searchDate, int floor, string roomStatusKey, string guestStatus, string[] list)
        {
            var result = new List<RoomStatusPageOutput>();
            bool blnFilterByRoomStatusKey = false;
            bool blnFilterByGuestStatus = false;
            bool blnFilterByFloorNo = false;
            if (!string.IsNullOrEmpty(floor.ToString()) && floor != 0)
                blnFilterByFloorNo = true;
            if (!string.IsNullOrEmpty(roomStatusKey) && !roomStatusKey.Equals(Guid.Empty.ToString()))
                blnFilterByRoomStatusKey = true;
            if (list != null)
            {
                blnFilterByGuestStatus = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(guestStatus) && guestStatus != "ALL" && guestStatus != "90")
                    blnFilterByGuestStatus = true;
            }
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            string RoomStatusVacant = "vacant";
            string RoomStatusDueOut = "due out";
            string RoomStatusOccupied = "occupied";
            string HouseKeepingMaidStatusClean = "Clean";
            string HouseKeepingMaidStatusDirty = "Dirty";
            string HouseKeepingMaidStatusMaidInRoom = "Attendant";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@date", SqlDbType.DateTime)
                {
                    Value = searchDate
                },
                new SqlParameter("@Floor", SqlDbType.VarChar)
                {
                    Value = blnFilterByFloorNo==true?floor:DBNull.Value
                },
                new SqlParameter("@RoomStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value =blnFilterByRoomStatusKey==true? Guid.Parse(roomStatusKey):DBNull.Value
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetHotelRoomByDateAndFloorQuery(blnFilterByRoomStatusKey, blnFilterByGuestStatus, blnFilterByFloorNo, guestStatus, list), CommandType.Text, MultiTenancySide, parameters))
            {

                using (var dr = await command.ExecuteReaderAsync())
                {

                    while (dr.Read())
                    {
                        RoomStatusPageOutput g = new RoomStatusPageOutput();
                        g.Unit = !DBNull.Value.Equals(dr["Unit"]) ? dr["Unit"].ToString() : "";
                        g.MaidStatus = !DBNull.Value.Equals(dr["MaidStatus"]) ? dr["MaidStatus"].ToString() : "";
                        g.RoomStatus = !DBNull.Value.Equals(dr["RoomStatus"]) ? dr["RoomStatus"].ToString() : "";
                        g.RoomType = !DBNull.Value.Equals(dr["RoomType"]) ? dr["RoomType"].ToString() : "";
                        g.DND = !DBNull.Value.Equals(dr["DND"]) ? Convert.ToInt32(dr["DND"]) : 0;
                        g.Maid = !DBNull.Value.Equals(dr["Maid"]) ? dr["Maid"].ToString() : "";
                        g.Guest = !DBNull.Value.Equals(dr["Guest"]) ? dr["Guest"].ToString() : "";
                        #region MobileUi
                        g.RoomstatusTextColor = g.RoomStatus.ToLower().Equals(RoomStatusDueOut) ? "#c9302c" : g.RoomStatus.ToLower().Equals(RoomStatusVacant) ? "#5cb85c" : "#0000ff";
                        g.RoomstatusPBGColor = g.RoomStatus.ToLower().Equals(RoomStatusOccupied) ? "#FFB3B3" : "#e6e6e6";
                        if (g.RoomStatus.ToLower().Equals(RoomStatusOccupied))
                        {
                            if (!g.MaidStatus.ToLower().Equals(HouseKeepingMaidStatusMaidInRoom.ToLower()))
                            {
                                if (g.DND.ToString().Equals("1"))
                                {
                                    g.DNDStatus = "Disable";
                                    g.DNDColor = "Red";
                                }
                                else
                                {
                                    g.DNDStatus = "Enable";
                                    g.DNDColor = "#428bca";
                                }
                            }
                            else
                            {
                                g.DNDStatus = "-";
                            }

                        }
                        else
                        {
                            g.DNDStatus = "-";

                        }
                        g.MaidStatusTextColor = g.MaidStatus.ToLower().Equals(HouseKeepingMaidStatusDirty.ToLower()) ? "#c9302c" : g.MaidStatus.ToLower().Equals(HouseKeepingMaidStatusClean.ToLower()) ? "#5cb85c" : "#0000ff";
                        if (!string.IsNullOrEmpty(g.Guest))
                        {
                            g.GuestDes = g.Guest.Length > 20 ? g.Guest.Substring(0, 20) + "..." : g.Guest;
                        }
                        if (!string.IsNullOrEmpty(g.Maid))
                        {
                            g.MaidDes = GetDescriptionByLength(g.Maid, 30);
                        }
                        #endregion
                        result.Add(g);
                    }
                    dr.Close();
                }
            }
            return result;
        }
        #region Display Description Helper
        public static string GetDescriptionByLength(string inputValue, int length)
        {
            string strReturnValue = "";
            try
            {
                strReturnValue = inputValue.Trim();
                if (strReturnValue == "")
                    return "-";
                if (strReturnValue.Length > length)
                {
                    strReturnValue = strReturnValue.Substring(0, length) + "..";
                }
                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }

        #endregion
        public List<RoomStatusHotelRoomOutput> GetHotelRoomByDateAndFloor(DateTime searchDate, int floor, string roomStatusKey, string guestStatus, string[] list)
        {
            var result = new List<RoomStatusHotelRoomOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            bool blnFilterByRoomStatusKey = false;
            bool blnFilterByGuestStatus = false;
            bool blnFilterByFloorNo = false;
            if (!string.IsNullOrEmpty(floor.ToString()) && floor != 0)
                blnFilterByFloorNo = true;
            if (!string.IsNullOrEmpty(roomStatusKey) && !roomStatusKey.Equals(Guid.Empty.ToString()))
                blnFilterByRoomStatusKey = true;
            if (list != null)
            {
                blnFilterByGuestStatus = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(guestStatus) && guestStatus != "ALL" && guestStatus != "90")
                    blnFilterByGuestStatus = true;
            }
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@date", SqlDbType.DateTime)
                {
                    Value = searchDate
                },
                new SqlParameter("@Floor", SqlDbType.VarChar)
                {
                    Value = blnFilterByFloorNo==true?floor:DBNull.Value
                },
                new SqlParameter("@RoomStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value =blnFilterByRoomStatusKey==true? Guid.Parse(roomStatusKey):DBNull.Value
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetHotelRoomByDateAndFloorQuery(blnFilterByRoomStatusKey, blnFilterByGuestStatus, blnFilterByFloorNo, guestStatus, list), CommandType.Text, MultiTenancySide, parameters))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    dt = ds.Tables[0];

                }

            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    RoomStatusHotelRoomOutput g = new RoomStatusHotelRoomOutput();
                    g.Room = !DBNull.Value.Equals(dr["Unit"]) ? dr["Unit"].ToString() : "";
                    g.RoomStatus = !DBNull.Value.Equals(dr["RoomStatus"]) ? dr["RoomStatus"].ToString() : "";
                    result.Add(g);
                }
            }
            return result;

            //bool blnFilterByRoomStatusKey = false;
            //bool blnFilterByGuestStatus = false;
            //bool blnFilterByFloorNo = false;
            //if (!string.IsNullOrEmpty(floor.ToString()) && floor != 0)
            //    blnFilterByFloorNo = true;
            //if (!string.IsNullOrEmpty(roomStatusKey) && !roomStatusKey.Equals(Guid.Empty.ToString()))
            //    blnFilterByRoomStatusKey = true;
            //if (list != null)
            //{
            //    blnFilterByGuestStatus = true;
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(guestStatus) && guestStatus != "ALL" && guestStatus != "90")
            //        blnFilterByGuestStatus = true;
            //}
            //_connectionManager.EnsureConnectionOpen(MultiTenancySide);
            //using (var command = _connectionManager.CreateCommandOnly(GetHotelRoomByDateAndFloorQuery(blnFilterByRoomStatusKey, blnFilterByGuestStatus, blnFilterByFloorNo, guestStatus, list, searchDate, floor, roomStatusKey), CommandType.Text,MultiTenancySide))
            //{
            //    using (var dr = await command.ExecuteReaderAsync())
            //    {
            //        var result = new List<RoomStatusHotelRoomOutput>();
            //        while (dr.Read())
            //        {
            //            RoomStatusHotelRoomOutput g = new RoomStatusHotelRoomOutput();
            //            // g.RoomKey = (!DBNull.Value.Equals(dr["RoomKey"])) ? (!string.IsNullOrEmpty(dr["RoomKey"].ToString()) ? new Guid(dr["RoomKey"].ToString()) : Guid.Empty) : Guid.Empty;
            //            g.Room = !DBNull.Value.Equals(dr["Unit"]) ? dr["Unit"].ToString() : "";
            //            //g.MaidStatusKey = (!DBNull.Value.Equals(dr["MaidStatusKey"])) ? (!string.IsNullOrEmpty(dr["MaidStatusKey"].ToString()) ? new Guid(dr["MaidStatusKey"].ToString()) : Guid.Empty) : Guid.Empty;
            //            //g.MaidStatus = !DBNull.Value.Equals(dr["MaidStatus"]) ? dr["MaidStatus"].ToString() : "";
            //            //g.RoomStatusKey = (!DBNull.Value.Equals(dr["RoomStatusKey"])) ? (!string.IsNullOrEmpty(dr["RoomStatusKey"].ToString()) ? new Guid(dr["RoomStatusKey"].ToString()) : Guid.Empty) : Guid.Empty;
            //            g.RoomStatus = !DBNull.Value.Equals(dr["RoomStatus"]) ? dr["RoomStatus"].ToString() : "";
            //            //g.RoomType = !DBNull.Value.Equals(dr["RoomType"]) ? dr["RoomType"].ToString() : "";
            //            //g.InterconnectRoom = !DBNull.Value.Equals(dr["InterconnectRoom"]) ? dr["InterconnectRoom"].ToString() : "";
            //            //g.Floor = !DBNull.Value.Equals(dr["Floor"]) ? Convert.ToInt32(dr["Floor"]) : 0;
            //            //g.LinenChange = !DBNull.Value.Equals(dr["LinenChange"]) ? dr["LinenChange"].ToString() : "";
            //            //g.DND = !DBNull.Value.Equals(dr["DND"]) ? Convert.ToInt32(dr["DND"]) : 0;
            //            //g.CleaningTime = !DBNull.Value.Equals(dr["CleaningTime"]) ? dr["CleaningTime"].ToString() : "";
            //            //g.LinenDays = !DBNull.Value.Equals(dr["LinenDays"]) ? Convert.ToInt32(dr["LinenDays"]) : 0;
            //            //g.Bed = !DBNull.Value.Equals(dr["Bed"]) ? Convert.ToInt32(dr["Bed"]) : 0;
            //            //g.Maid = !DBNull.Value.Equals(dr["Maid"]) ? dr["Maid"].ToString() : "";
            //            //g.HMMNotes = !DBNull.Value.Equals(dr["HMMNotes"]) ? dr["HMMNotes"].ToString() : "";
            //            //g.Guest = !DBNull.Value.Equals(dr["Guest"]) ? dr["Guest"].ToString() : "";

            //            result.Add(g);
            //        }
            //        return result;
            //    }
            //}
        }
        public List<MinibarCORoomOutput> GetChkOutRoomByDateAndFloor(DateTime searchDate, int floor)
        {
            var result = new List<MinibarCORoomOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            bool blnFilterByFloorNo = false;
            if (!string.IsNullOrEmpty(floor.ToString()) && floor != 0)
                blnFilterByFloorNo = true;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@date", SqlDbType.DateTime)
                {
                    Value = searchDate
                },
                new SqlParameter("@Floor", SqlDbType.VarChar)
                {
                    Value = blnFilterByFloorNo==true?floor:DBNull.Value
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetChkOutRoomByDateAndFloorQuery(blnFilterByFloorNo), CommandType.Text, MultiTenancySide, parameters))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    dt = ds.Tables[0];

                }

            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MinibarCORoomOutput g = new MinibarCORoomOutput();
                    g.Room = !DBNull.Value.Equals(dr["Unit"]) ? dr["Unit"].ToString() : "";
                    g.Status = !DBNull.Value.Equals(dr["Status"]) ? Convert.ToInt32(dr["Status"]) : 0;
                    g.RecheckInVirtualRoom = !DBNull.Value.Equals(dr["RecheckInVirtualRoom"]) ? Convert.ToInt32(dr["RecheckInVirtualRoom"]) : 0;
                    //g.RoomStatus= (dr["Status"].ToString().Trim().Equals("2") && dr["RecheckInVirtualRoom"].ToString().Trim().Equals("1")) ? "Occupied" : "Vacant";
                    g.RoomStatus = (g.Status == 2 && g.RecheckInVirtualRoom == 1) ? "Occupied" : "Vacant";
                    result.Add(g);
                }
            }
            return result;

        }
        public async Task<List<SalesPrice>> GetGSTInclusiveAmt(double total, string postCodeKey)
        {
            var result = new List<SalesPrice>();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetCalculatePriceForGSTInclusiveQuery(total, postCodeKey), CommandType.Text, MultiTenancySide))
            {
                using (var dr = await command.ExecuteReaderAsync())
                {

                    while (dr.Read())
                    {
                        SalesPrice salesPrice = new SalesPrice();
                        salesPrice.salesPrice = !DBNull.Value.Equals(dr["Rate"]) ? Convert.ToDouble(dr["Rate"]) : 0.0;
                        salesPrice.salesTax1 = !DBNull.Value.Equals(dr["Tax1"]) ? Convert.ToDouble(dr["Tax1"]) : 0.0;
                        salesPrice.salesTax2 = !DBNull.Value.Equals(dr["Tax2"]) ? Convert.ToDouble(dr["Tax2"]) : 0.0;
                        salesPrice.salesTax3 = !DBNull.Value.Equals(dr["Tax3"]) ? Convert.ToDouble(dr["Tax3"]) : 0.0;
                        salesPrice.salesTotal = total;
                        result.Add(salesPrice);
                    }
                    dr.Close();

                }
            }
            return result;
        }

        public async Task<List<SalesPrice>> GetGSTExclusiveAmt(double total, string postCodeKey)
        {
            var result = new List<SalesPrice>();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetCalculatePriceForGSTExclusiveQuery(total, postCodeKey), CommandType.Text, MultiTenancySide))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }

                SalesPrice salesPrice = new SalesPrice();
                if (ds.Tables.Count == 3)
                {
                    salesPrice.salesPrice = total;
                    salesPrice.salesTax1 = ds.Tables[0].Rows[0]["Tax1"] == null ? 0 : Convert.ToDouble(ds.Tables[0].Rows[0]["Tax1"]);
                    salesPrice.salesTax2 = ds.Tables[1].Rows[0]["Tax2"] == null ? 0 : Convert.ToDouble(ds.Tables[1].Rows[0]["Tax2"]);
                    salesPrice.salesTax3 = ds.Tables[2].Rows[0]["Tax3"] == null ? 0 : Convert.ToDouble(ds.Tables[2].Rows[0]["Tax3"]);
                    salesPrice.salesTotal = total + salesPrice.salesTax1 + salesPrice.salesTax2 + salesPrice.salesTax3;
                }
                //using (var dr = await command.ExecuteReaderAsync())
                //{
                //    var result = new List<SalesPrice>();
                //    SalesPrice salesPrice = new SalesPrice();
                //    salesPrice.salesPrice = total;
                //    salesPrice.salesTotal = total;
                //    while (dr.Read())
                //    {
                //        int i = 0;
                //        if (i == 0)
                //        {
                //            salesPrice.salesTax1 = !DBNull.Value.Equals(dr["Tax1"]) ? Convert.ToDouble(dr["Tax1"]) : 0.0;
                //            salesPrice.salesTotal = +salesPrice.salesTax1;
                //        }
                //        else if (i == 1)
                //        {
                //            salesPrice.salesTax2 = !DBNull.Value.Equals(dr["Tax2"]) ? Convert.ToDouble(dr["Tax2"]) : 0.0;
                //            salesPrice.salesTotal = +salesPrice.salesTax2;
                //        }
                //        else if (i == 2)
                //        {
                //            salesPrice.salesTax3 = !DBNull.Value.Equals(dr["Tax3"]) ? Convert.ToDouble(dr["Tax3"]) : 0.0;
                //            salesPrice.salesTotal = +salesPrice.salesTax3;
                //        }

                //        i++;
                //    }
                result.Add(salesPrice);

                //}
            }
            return result;
        }

        public int UpdateMaidStatusByRoomKey(Guid roomKey, Guid maidStatusKey, string notes)
        {

            int intRowAffected = 0;

            var context = GetContext();
            var connection = context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            bool blnUpdateNote = false;
            if (!string.IsNullOrEmpty(notes))
                blnUpdateNote = true;
            try
            {

                using (SqlCommand command = (SqlCommand)connection.CreateCommand())
                {

                    command.Transaction = null;

                    command.CommandType = CommandType.Text;
                    command.CommandText = UpdateMaidStatusByRoomKeyQuery(blnUpdateNote);
                    command.Parameters.Clear();
                    command.Parameters.Add("@RoomKey", SqlDbType.UniqueIdentifier).Value = roomKey;
                    command.Parameters.Add("@MaidStatusKey", SqlDbType.UniqueIdentifier).Value = maidStatusKey;
                    if (blnUpdateNote)
                        command.Parameters.Add("@HMMNotes", SqlDbType.VarChar).Value = notes;
                    ConvertNullParameterValuesToDBNull(command);
                    intRowAffected = command.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {

                // OrderTrans.Rollback();
            }
            finally
            {
                //command.Dispose();
                // OrderTrans.Dispose();
                //connection.Close();
            }
            return intRowAffected;

        }
        public int UpdateOptReason(ReservationOptOutModel r)
        {
            int intRowAffected = 0;

            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                SqlParameter[] parameters = new SqlParameter[]
                {

                new SqlParameter("@ReservationOptOutKey", SqlDbType.UniqueIdentifier)
                {
                    Value = (r.ReservationOptOutKey == Guid.Empty ? Guid.NewGuid() : r.ReservationOptOutKey)
                },
                new SqlParameter("@ReservationOptOutCode", SqlDbType.VarChar)
                {
                    Value = r.ReservationOptOutCode != null ?r.ReservationOptOutCode: DBNull.Value
                },
                new SqlParameter("@ReservationOptOutReason", SqlDbType.VarChar)
                {
                    Value = r.ReservationOptOutReason
                },
                new SqlParameter("@AttendantID", SqlDbType.UniqueIdentifier)
                {
                    Value = (r.AttendantID == null ? DBNull.Value : r.AttendantID)
                },
                new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                {
                    Value = (r.ReservationKey == null ? DBNull.Value : r.ReservationKey)
                },
                new SqlParameter("@OptOut", SqlDbType.DateTime)
                {
                    Value = r.OptOut
                },
                new SqlParameter("@Unit", SqlDbType.VarChar)
                {
                    Value = (!string.IsNullOrEmpty(r.Unit) && r.Unit != "") ? r.Unit : DBNull.Value
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = r.TenantId
                }
              };
                using (var command = _connectionManager.CreateCommandSP(InsertReservationOut(), CommandType.Text, MultiTenancySide, parameters))
                {

                    intRowAffected = command.ExecuteNonQuery();
                }
                return intRowAffected;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetLinenItem(string roomkey, DateTime date)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
              {

                new SqlParameter("@date", SqlDbType.DateTime)
                {
                    Value = date
                },
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(roomkey)
                }
              };
            using (var command = _connectionManager.CreateCommandSP(GetStartLinenItemQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    dt = ds.Tables[0];
                }

            }
            return dt;

        }
        public DataTable GetSupLinenItem(string roomkey)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetSupLinenItemQuery(roomkey), CommandType.Text, MultiTenancySide))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    return ds.Tables[0];
                }
                else { return dt; }
            }

        }
        public static void ConvertNullParameterValuesToDBNull(SqlCommand command)
        {
            ConvertNullParameterValuesToDBNull(command.Parameters);
        }

        public static void ConvertNullParameterValuesToDBNull(SqlParameterCollection paramColl)
        {
            foreach (IDataParameter parameter in paramColl)
            {
                if (parameter.Value == null)
                {
                    parameter.Value = DBNull.Value;
                }
            }
        }

        public DataTable GetHotelRoomByRoomNo(string roomNo)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetHotelRoomByRoomNoQuery(roomNo), CommandType.Text, MultiTenancySide))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    return ds.Tables[0];
                }
                else { return dt; }
            }

        }
        public int UpdateAssignment(Guid maidkey, string roomno)
        {
            int s = 0;

            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);

                using (var command = _connectionManager.CreateCommandOnly(UpdateAssignUnAssignByRoomNoQuery(maidkey, roomno), CommandType.Text, MultiTenancySide))
                {
                    command.ExecuteNonQuery();
                    s = 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return s;
        }
        public List<ReservationOutput> GetReservationByRoomKey(string roomKey)
        {

            var list = new List<ReservationOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value = new Guid(roomKey)
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetReservationByRoomKey(), CommandType.Text, MultiTenancySide, parameters))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    dt = ds.Tables[0];

                }

            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DateTime? nullable;
                    ReservationOutput o = new ReservationOutput();
                    o.ReservationKey = (!DBNull.Value.Equals(dr["ReservationKey"])) ? (!string.IsNullOrEmpty(dr["ReservationKey"].ToString()) ? new Guid(dr["ReservationKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.DocNo = (!DBNull.Value.Equals(dr["DocNo"])) ? (!string.IsNullOrEmpty(dr["DocNo"].ToString()) ? dr["DocNo"].ToString() : "") : "";
                    o.CheckInDate = (DateTime)(Convert.IsDBNull(dr["CheckInDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["CheckInDate"])).Value);
                    o.CheckOutDate = (DateTime)(Convert.IsDBNull(dr["CheckOutDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["CheckOutDate"])).Value);
                    o.GuestKey = (!DBNull.Value.Equals(dr["GuestKey"])) ? (!string.IsNullOrEmpty(dr["GuestKey"].ToString()) ? new Guid(dr["GuestKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.GuestName = (!DBNull.Value.Equals(dr["Name"])) ? (!string.IsNullOrEmpty(dr["Name"].ToString()) ? dr["Name"].ToString() : "") : "";
                    o.RoomKey = (!DBNull.Value.Equals(dr["RoomKey"])) ? (!string.IsNullOrEmpty(dr["RoomKey"].ToString()) ? new Guid(dr["RoomKey"].ToString()) : Guid.Empty) : Guid.Empty;

                    list.Add(o);
                }
            }
            return list;

        }

        public DateTime GetBusinessDate()
        {
            DateTime result = DateTime.Now;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetSystemControlQuery(), CommandType.Text, MultiTenancySide))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                        result = Convert.ToDateTime(ds.Tables[0].Rows[0]["SystemDate"]);
                }

            }
            return result;

        }
        public List<DDLReason> GetAllReason()
        {
            var lst = new List<DDLReason>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            //drNew["Reason"] = "";
            //drNew["HousekeepingOptOutReasonCode"] = "--Please select--";
            using (var command = _connectionManager.CreateCommandOnly(GetAllReasonQuery(), CommandType.Text, MultiTenancySide))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    dt = ds.Tables[0];

                }

            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DDLReason o = new DDLReason();
                    o.Reason = !DBNull.Value.Equals(dr["Reason"]) ? dr["Reason"].ToString() : "";
                    o.HousekeepingOptOutReasonCode = !DBNull.Value.Equals(dr["HousekeepingOptOutReasonCode"]) ? dr["HousekeepingOptOutReasonCode"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }
        public List<DDLDirtyReason> GetDirtyAllReason()
        {
            var lst = new List<DDLDirtyReason>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            //drNew["Reason"] = "";
            //drNew["HousekeepingOptOutReasonCode"] = "--Please select--";
            using (var command = _connectionManager.CreateCommandOnly(GetAllDirtyReasonQuery(), CommandType.Text, MultiTenancySide))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    dt = ds.Tables[0];

                }

            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DDLDirtyReason o = new DDLDirtyReason();
                    o.Reason = !DBNull.Value.Equals(dr["Reason"]) ? dr["Reason"].ToString() : "";
                    o.HousekeepingDirtyReasonCode = !DBNull.Value.Equals(dr["HousekeepingDirtyReasonCode"]) ? dr["HousekeepingDirtyReasonCode"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }
        public int CheckDndImage(DndImage image)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {

                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                     Value =  image.RoomKey
                }

           };
            using (var command = _connectionManager.CreateCommandSP(CheckDndImageQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    intRowAffected = ds.Tables[0].Rows.Count;

                }
            }
            return intRowAffected;
        }
        public int UpdateDndImage(DndImage image)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  image.RoomKey
                 },
                new SqlParameter("@LastModifiedStaff", SqlDbType.UniqueIdentifier)
                {
                     Value =  image.LastModifiedStaff
                 },
                new SqlParameter("@Document", SqlDbType.VarChar)
                {
                    Value = image.DocumentName
                },
                 new SqlParameter("@Image", SqlDbType.VarBinary)
                 {
                     Value = image.Signature
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(UpdateDndImageQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }

            return intRowAffected;
        }

        public int InsertDndImage(DndImage image)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@DndphotoKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  image.DndphotoKey
                 },
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  image.RoomKey
                 },
                 new SqlParameter("@LastModifiedStaff", SqlDbType.UniqueIdentifier)
                {
                     Value =  image.LastModifiedStaff
                 },
                new SqlParameter("@Sort", SqlDbType.Int)
                {
                     Value =  image.Sort
                 },
                new SqlParameter("@Document", SqlDbType.VarChar)
                {
                    Value = image.DocumentName
                },
                 new SqlParameter("@Image", SqlDbType.VarBinary)
                 {
                     Value = image.Signature
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(InsertDndImageQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        #endregion

        #region SQL Query
        private static string GetItemByReservationKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select Code from ReservationAdditional ra   ");
            sb.Append(" left join Item i on ra.ItemKey = i.ItemKey ");
            sb.Append(" WHERE  ReservationKey = @ReservationKey and i.Hkg=1; ");
            return sb.ToString();
        }
        private static string InsertReservationOut()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert into [ReservationOptOut] ");
            sb.Append(" ([ReservationOptOutKey], [ReservationOptOutCode], [ReservationOptOutReason], [AttendantID], [ReservationKey], [OptOut], [Unit], [TenantId])  ");
            sb.Append(" Values ");
            sb.Append(" (@ReservationOptOutKey, @ReservationOptOutCode, @ReservationOptOutReason, @AttendantID,  @ReservationKey, @OptOut, @Unit,@TenantId)  ");
            return sb.ToString();
        }
        private static string GetOptStatusQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT CASE WHEN(SELECT COUNT(*) FROM ReservationOptOut where Unit=@unit) % 2 = 0 THEN 0 ELSE 1 END AS RowCountStatus;");
            return sb.ToString();
        }
        private static string GetETAQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select ETA from Reservation where roomkey=@roomkey and checkindate=@checkindate and Status=1");
            return sb.ToString();
        }
        private static string GetRoomBlockByRoomKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select * from RoomBlock where RoomKey = @RoomKey and BlockDate = @date and Active in (1,2)  ");
            return sb.ToString();
        }
        private static string GetRoomByMaidStatusKeyQuery(bool blnFilterByMaidKey, bool blnFilterByFloorNo, bool blnFilterByRoomStatusKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  ");
            sb.Append("   S.RoomKey, Unit,S.MaidStatusKey,M.MaidStatus,S.RoomStatusKey,R.RoomStatus,S.ReservationKey,RoomType,InterconnectRoom,Floor, LinenChange, DND,S.Adult,S.Child,G1.Group1,G2.Group2,G3.Group3,G4.Group4,S.ETD, ");
            sb.Append("   CleaningTime,LinenDays,Bed,S.CheckInDate,S.CheckInTime,S.CheckOutDate,S.CheckOutTime,I.Maid,HMMNotes,S.GuestArrived,S.Status,case when S.PreCheckInCount is null or S.PreCheckInCount = 0 then 0 else S.PreCheckInCount end As PreCheckInCount,GS.Status AS GuestStatus,dpl.Image,dpl.Document,dpl.CreatedDate ");
            sb.Append(" FROM  ");
            sb.Append("   dbo.UDF_DayRoomStatusiClean( (ISNULL((SELECT systemdate FROM control), GETDATE())) ) S ");
            sb.Append(" LEFT JOIN   ");
            sb.Append("   (Select A.RoomKey,InterconnectRoom=B.Unit,");
            sb.Append("   Maid=M.Name,A.HMMNotes from Room A Left join Room B on A.InterconnectRoomKey=B.RoomKey Left Join Maid M on ");
            //sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey Left Join (Select dp.Image, dp.Document, dp.CreatedDate, dp.RoomKey from Dndphoto dp where dp.CreatedDate = (SELECT MAX([CreatedDate]) FROM [dbo].[Dndphoto])) dpl on  S.RoomKey=dpl.RoomKey ");
            //sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey  LEFT JOIN ( SELECT dp.RoomKey, dp.Image, dp.Document, dp.CreatedDate FROM Dndphoto dp JOIN ( SELECT RoomKey, MAX(CreatedDate) AS MaxCreatedDate FROM Dndphoto GROUP BY RoomKey) latest ON dp.RoomKey = latest.RoomKey AND dp.CreatedDate = latest.MaxCreatedDate) dpl ON S.RoomKey = dpl.RoomKey ");
            sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey Left Join  ( SELECT dp.RoomKey, dp.Image, dp.Document, dp.CreatedDate FROM Dndphoto dp JOIN (SELECT RoomKey, MAX(CreatedDate) AS MaxCreatedDate FROM Dndphoto WHERE CAST(CreatedDate AS DATE) = CAST(ISNULL((SELECT TOP 1 SystemDate FROM Control WHERE SystemDate IS NOT NULL),GETDATE()) AS DATE)GROUP BY RoomKey) latest ON dp.RoomKey = latest.RoomKey AND dp.CreatedDate = latest.MaxCreatedDate) dpl ON S.RoomKey = dpl.RoomKey ");
            sb.Append(" LEFT JOIN   ");
            sb.Append("   MaidStatus M on S.MaidStatusKey=M.MaidStatusKey  ");
            sb.Append(" LEFT JOIN   ");
            sb.Append("   RoomStatus R on S.RoomStatusKey=R.RoomStatusKey ");
            sb.Append(" Left join Reservation V on S.ReservationKey = V.ReservationKey Left Join GuestStatus GS on V.GuestStatus = GS.StatusCode ");
            sb.Append(" LEFT JOIN Group1 G1 ON V.Group1Key = G1.Group1Key  LEFT JOIN Group2 G2 ON V.Group2Key = G2.Group2Key  LEFT JOIN Group3 G3 ON V.Group3Key = G3.Group3Key ");
            sb.Append(" LEFT JOIN Group4 G4 ON V.Group4Key = G4.Group4Key");
            sb.Append(" WHERE  ");
            sb.Append("    M.MaidStatusKey = @MaidStatusKey ");
            if (blnFilterByMaidKey)
                sb.Append("  AND  S.MaidKey = @MaidKey  ");
            if (blnFilterByFloorNo)
                sb.Append("  AND  Floor = @FloorNo ");
            if (blnFilterByRoomStatusKey)
                sb.Append("  AND  S.RoomStatusKey = @RoomStatusKey ");
            //sb.Append(" ORDER BY S.GuestArrived, PreCheckInCount desc, Floor, S.Unit;  ");
            // sb.Append(" ORDER BY S.GuestArrived, Floor, S.Unit;");
            sb.Append(" ORDER BY CASE WHEN ISNULL(NULLIF(S.GuestArrived, ''), '') = '' THEN 1 ELSE 0 END, S.Floor,S.Unit ");


            return sb.ToString();
        }
        private static string GetSupervisorRoomByMaidStatusKeyQuery(bool blnFilterByMaidKey, bool blnFilterByFloorNo, bool blnFilterByRoomStatusKey, bool blnFilterByMaidStatusKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  ");
            sb.Append("   S.RoomKey, Unit,S.MaidStatusKey,M.MaidStatus,S.RoomStatusKey,R.RoomStatus,S.ReservationKey,RoomType,InterconnectRoom,Floor, LinenChange, DND,S.Adult,S.Child,  ");
            sb.Append("   CleaningTime,LinenDays,Bed,S.CheckInDate,S.CheckInTime,S.CheckOutDate,S.CheckOutTime,I.Maid,HMMNotes,S.GuestArrived,S.Status,case when S.PreCheckInCount is null or S.PreCheckInCount = 0 then 0 else S.PreCheckInCount end As PreCheckInCount,GS.Status AS GuestStatus,dpl.Image,dpl.Document,dpl.CreatedDate ");
            sb.Append(" ,(SELECT STRING_AGG(i.Code, ', ') FROM ReservationAdditional ra LEFT JOIN Item i ON ra.ItemKey = i.ItemKey WHERE ReservationKey = S.ReservationKey AND i.Hkg = 1 AND S.Status IN (1,2)) AS ItemCodes ");
            sb.Append(" FROM  ");
            sb.Append("   dbo.UDF_DayRoomStatusiClean( (ISNULL((SELECT systemdate FROM control), GETDATE())) ) S ");
            sb.Append(" LEFT JOIN   ");
            sb.Append("   (Select A.RoomKey,InterconnectRoom=B.Unit,");
            sb.Append("   Maid=M.Name,A.HMMNotes from Room A Left join Room B on A.InterconnectRoomKey=B.RoomKey Left Join Maid M on ");
            //sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey Left Join (Select dp.Image, dp.Document, dp.CreatedDate, dp.RoomKey from Dndphoto dp where dp.CreatedDate = (SELECT MAX([CreatedDate]) FROM [dbo].[Dndphoto])) dpl on  S.RoomKey=dpl.RoomKey ");
            //sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey  LEFT JOIN ( SELECT dp.RoomKey, dp.Image, dp.Document, dp.CreatedDate FROM Dndphoto dp JOIN ( SELECT RoomKey, MAX(CreatedDate) AS MaxCreatedDate FROM Dndphoto GROUP BY RoomKey) latest ON dp.RoomKey = latest.RoomKey AND dp.CreatedDate = latest.MaxCreatedDate) dpl ON S.RoomKey = dpl.RoomKey ");
            sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey Left Join  ( SELECT dp.RoomKey, dp.Image, dp.Document, dp.CreatedDate FROM Dndphoto dp JOIN (SELECT RoomKey, MAX(CreatedDate) AS MaxCreatedDate FROM Dndphoto WHERE CAST(CreatedDate AS DATE) = CAST(ISNULL((SELECT TOP 1 SystemDate FROM Control WHERE SystemDate IS NOT NULL),GETDATE()) AS DATE)GROUP BY RoomKey) latest ON dp.RoomKey = latest.RoomKey AND dp.CreatedDate = latest.MaxCreatedDate) dpl ON S.RoomKey = dpl.RoomKey ");
            sb.Append(" LEFT JOIN   ");
            sb.Append("   MaidStatus M on S.MaidStatusKey=M.MaidStatusKey  ");
            sb.Append(" LEFT JOIN   ");
            sb.Append("   RoomStatus R on S.RoomStatusKey=R.RoomStatusKey ");
            sb.Append(" Left join Reservation V on S.ReservationKey = V.ReservationKey Left Join GuestStatus GS on V.GuestStatus = GS.StatusCode ");
            if (blnFilterByMaidKey)
            {
                sb.Append("  Where  S.MaidKey = @MaidKey  ");
            }
            if (blnFilterByFloorNo)
            {
                if (blnFilterByMaidKey)
                    sb.Append("  AND  Floor = @FloorNo ");
                else
                    sb.Append(" Where Floor = @FloorNo ");
            }
            if (blnFilterByRoomStatusKey)
            {
                if (blnFilterByMaidKey || blnFilterByFloorNo)
                    sb.Append("  AND  S.RoomStatusKey = @RoomStatusKey ");
                else
                    sb.Append(" Where S.RoomStatusKey = @RoomStatusKey ");
            }
            if (blnFilterByMaidStatusKey)
            {
                if (blnFilterByMaidKey || blnFilterByFloorNo || blnFilterByRoomStatusKey)
                    sb.Append("  AND  M.MaidStatusKey = @MaidStatusKey ");
                else
                    sb.Append(" Where M.MaidStatusKey = @MaidStatusKey ");
            }
            //sb.Append(" ORDER BY S.GuestArrived, PreCheckInCount desc, Floor, S.Unit;  ");
            // sb.Append(" ORDER BY S.GuestArrived, Floor, S.Unit;");
            sb.Append(" ORDER BY CASE WHEN ISNULL(NULLIF(S.GuestArrived, ''), '') = '' THEN 1 ELSE 0 END, S.Floor,S.Unit ");


            return sb.ToString();
        }
        private static string GetChkOutRoomByDateAndFloorQuery(bool blnFilterByFloorNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select distinct S.RoomKey, Unit,S.MaidStatusKey,M.MaidStatus,S.RoomStatusKey,R.RoomStatus,RoomType,InterconnectRoom,Floor, LinenChange, DND,");
            sb.Append(" CleaningTime,LinenDays,Bed,S.CheckInDate,S.CheckInTime,S.CheckOutDate,S.CheckOutTime,I.Maid,HMMNotes, S.Status, V.RecheckInVirtualRoom  ");
            sb.Append(" from dbo.UDF_DayRoomStatusiClean( @date ) S Left Join (Select A.RoomKey,InterconnectRoom=B.Unit,");
            sb.Append(" Maid=M.Name,A.HMMNotes from Room A Left join Room B on A.InterconnectRoomKey=B.RoomKey Left Join Maid M on ");
            sb.Append(" A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey ");
            sb.Append(" Left Join MaidStatus M on S.MaidStatusKey=M.MaidStatusKey  Left Join RoomStatus R on S.RoomStatusKey=R.RoomStatusKey ");
            sb.Append(" Left join Reservation V on S.ReservationKey = V.ReservationKey ");
            //sb.Append(" where V.RecheckInVirtualRoom = 1 ");
            if (blnFilterByFloorNo)
                sb.Append(" where Floor = @Floor ");
            sb.Append(" order by Unit ");
            return sb.ToString();
        }
        private static string GetHotelRoomByDateAndFloorQuery(bool blnFilterByRoomStatusKey, bool blnFilterByGuestStatus, bool blnFilterByFloorNo, string gueststatus, string[] list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select distinct S.RoomKey, Unit,S.MaidStatusKey,M.MaidStatus,S.RoomStatusKey,R.RoomStatus,RoomType,InterconnectRoom,Floor, LinenChange, DND,");
            sb.Append(" CleaningTime,LinenDays,Bed,S.CheckInDate,S.CheckInTime,S.CheckOutDate,S.CheckOutTime,I.Maid,HMMNotes, S.Guest  ");
            sb.Append(" from dbo.UDF_DayRoomStatusShowiClean( @date ) S Left Join (Select A.RoomKey,InterconnectRoom=B.Unit,");
            sb.Append(" Maid=M.Name,A.HMMNotes from Room A Left join Room B on A.InterconnectRoomKey=B.RoomKey Left Join Maid M on ");
            sb.Append(" A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey ");
            sb.Append(" Left Join MaidStatus M on S.MaidStatusKey=M.MaidStatusKey  Left Join RoomStatus R on S.RoomStatusKey=R.RoomStatusKey ");
            sb.Append(" Left join Reservation V on S.ReservationKey = V.ReservationKey Left Join GuestStatus GS on V.GuestStatus = GS.StatusCode ");
            //sb.Append(" where S.Guest = G.Name ");
            if (blnFilterByFloorNo)
                sb.Append(" where Floor = @Floor ");
            if (blnFilterByRoomStatusKey)
                if (blnFilterByFloorNo)
                    sb.Append("  AND   S.RoomStatusKey = @RoomStatusKey  ");
                else
                    sb.Append("  where   S.RoomStatusKey = @RoomStatusKey  ");
            if (blnFilterByGuestStatus)
                if (list != null)
                {
                    if (blnFilterByFloorNo || blnFilterByRoomStatusKey)
                    {
                        sb.Append(" AND GS.Status in ( ");
                        for (int i = 0; i < list.Length; i++)
                        {
                            sb.Append("'" + list[i] + "',");
                        }
                        sb.Append(")");
                    }
                    else
                    {
                        sb.Append(" where GS.Status in ( ");
                        for (int i = 0; i < list.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(list[i]))
                                sb.Append("'" + list[i] + "',");
                        }
                        sb.Append(")");
                    }
                }
                else
                {
                    if (blnFilterByFloorNo || blnFilterByRoomStatusKey)
                        sb.Append(" AND GS.Status = '" + gueststatus + "'");
                    else
                        sb.Append(" where GS.Status = '" + gueststatus + "'");
                }
            sb.Append(" order by Floor, Unit ");
            var text = sb.ToString();
            var str = text.Replace(",)", ")");
            return str;
        }
        private static string GetReservationByRoomKey()
        {
            StringBuilder sbQueryBuilder = new StringBuilder();
            sbQueryBuilder.Append(" SELECT ReservationKey,DocNo,CheckInDate,CheckOutDate,Reservation.GuestKey,Guest.Name,RoomKey ");
            sbQueryBuilder.Append(" FROM Reservation LEFT JOIN Guest ON Reservation.GuestKey = Guest.GuestKey ");
            sbQueryBuilder.Append(" where Reservation.RoomKey=@RoomKey and Reservation.Status = 2; "); // Status 2 : Check-In
            return sbQueryBuilder.ToString();
        }
        //private static string GetStartLinenItemQuery(string roomkey, DateTime date)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(" IF EXISTS(SELECT * FROM AttendantCheckList WHERE RoomKey = '" + roomkey + "' and CONVERT(date, DocDate) = CONVERT(date, '" + date + "') and Stop = 0) ");
        //    sb.Append(" BEGIN ");
        //    sb.Append(" SELECT att.LinenChecklistKey, att.ItemKey, chk.ItemCode, chk.Description, att.Quantity FROM AttendantCheckList att JOIN CheckList chk ");
        //    sb.Append(" ON chk.ItemKey = att.ItemKey WHERE RoomKey = '" + roomkey + "' AND chk.Active = 1 AND CONVERT(date, DocDate) = CONVERT(date, '" + date + "') AND Stop = 0 ORDER BY chk.Sort, chk.ItemCode ");
        //    sb.Append(" END ");
        //    sb.Append(" ELSE ");
        //    sb.Append(" BEGIN ");
        //    sb.Append(" SELECT '00000000-0000-0000-0000-000000000000' AS LinenChecklistKey, ItemKey, ItemCode, Description, 0 AS Quantity FROM CheckList ");
        //    sb.Append(" WHERE Attendant = 1 AND Active = 1 ORDER BY Sort, ItemCode; ");
        //    sb.Append(" END ");
        //    return sb.ToString();
        //}
        private static string GetStartLinenItemQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" IF EXISTS(SELECT * FROM AttendantCheckList WHERE RoomKey = @RoomKey and CONVERT(date, DocDate) = CONVERT(date, @date) and Stop = 0) ");
            sb.Append(" BEGIN ");
            sb.Append(" SELECT att.LinenChecklistKey, att.ItemKey, chk.ItemCode, chk.Description, att.Quantity FROM AttendantCheckList att JOIN CheckList chk ");
            sb.Append(" ON chk.ItemKey = att.ItemKey WHERE RoomKey = @RoomKey AND chk.Active = 1 AND CONVERT(date, DocDate) = CONVERT(date, @date) AND Stop = 0 ORDER BY chk.Sort, chk.ItemCode ");
            sb.Append(" END ");
            sb.Append(" ELSE ");
            sb.Append(" BEGIN ");
            sb.Append(" SELECT '00000000-0000-0000-0000-000000000000' AS LinenChecklistKey, ItemKey, ItemCode, Description, 0 AS Quantity FROM CheckList ");
            sb.Append(" WHERE Attendant = 1 AND Active = 1 ORDER BY Sort, ItemCode; ");
            sb.Append(" END ");
            return sb.ToString();
        }
        private static string GetSupLinenItemQuery(string roomkey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" DECLARE @historyDate datetime, @DocDate datetime, @endDate datetime ");
            sb.Append(" SELECT TOP(1) @historyDate = ChangedDate FROM History WHERE ModuleName = 'iClean' AND TableName = 'Room' AND TenantId=1 AND  Detail LIKE '%STARTS%' AND ");
            sb.Append(" SourceKey = '" + roomkey + "' ORDER BY Seq desc ");
            sb.Append(" SELECT top(1) @DocDate= DocDate FROM AttendantCheckList where RoomKey='" + roomkey + "' order by DocDate desc ");
            sb.Append(" SELECT TOP(1) @endDate = ChangedDate FROM History WHERE ModuleName = 'iClean' AND TableName = 'Room' AND TenantId=1 AND Detail LIKE '%DIRTY%' AND SourceKey = '" + roomkey + "' ORDER BY Seq desc ");
            sb.Append(" IF EXISTS(SELECT * FROM SupervisorCheckList WHERE DocDate >= @historyDate AND RoomKey = '" + roomkey + "') ");
            sb.Append(" BEGIN ");
            sb.Append(" SELECT sup.InspectionChecklistKey, sup.ItemKey, chk.ItemCode, chk.Description, att.Quantity, sup.Checked FROM SupervisorCheckList sup, CheckList chk, AttendantCheckList att ");
            sb.Append(" WHERE sup.RoomKey ='" + roomkey + "' AND chk.ItemKey = sup.ItemKey AND att.RoomKey = '" + roomkey + "' AND chk.ItemKey = att.ItemKey AND att.DocDate = @DocDate AND chk.Active = 1 AND sup.DocDate >= @historyDate ORDER BY chk.Sort, chk.ItemCode ");
            sb.Append(" END ");
            sb.Append(" ELSE ");
            sb.Append(" BEGIN ");
            sb.Append(" IF EXISTS(select * from AttendantCheckList where RoomKey = '" + roomkey + "' AND DocDate >= @historyDate) ");
            sb.Append(" begin ");
            sb.Append(" SELECT '00000000-0000-0000-0000-000000000000' AS InspectionChecklistKey, chk.ItemKey, ItemCode, Description, att.Quantity, 0 AS Checked FROM CheckList chk, AttendantCheckList att ");
            sb.Append(" WHERE Supervisor = 1 AND Active = 1 AND att.RoomKey = '" + roomkey + "' AND chk.ItemKey = att.ItemKey AND att.DocDate = @DocDate ORDER BY Sort, ItemCode; ");
            sb.Append(" END ");
            sb.Append(" else if exists(select * from AttendantCheckList where RoomKey = '" + roomkey + "' AND DocDate >= @endDate) ");
            sb.Append(" begin ");
            sb.Append(" SELECT '00000000-0000-0000-0000-000000000000' AS InspectionChecklistKey, chk.ItemKey, ItemCode, Description, Quantity, 0 AS Checked FROM CheckList chk Join AttendantCheckList att ");
            sb.Append(" on chk.ItemKey = att.ItemKey WHERE Supervisor = 1 AND Active = 1 AND RoomKey ='" + roomkey + "' AND att.DocDate = @DocDate ORDER BY Sort, ItemCode; ");
            sb.Append(" end ");
            sb.Append(" ELSE ");
            sb.Append(" BEGIN ");
            sb.Append(" SELECT '00000000-0000-0000-0000-000000000000' AS InspectionChecklistKey, chk.ItemKey, chk.ItemCode, chk.Description, '-' as Quantity, 0 AS Checked FROM CheckList chk ");
            sb.Append(" WHERE Supervisor = 1 AND Active = 1 ORDER BY Sort, ItemCode; ");
            sb.Append(" END ");
            sb.Append(" END ");
            return sb.ToString();
        }

        private static string GetMinibarItemByResKeyQuery(string resKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT resRate.* ");
            sb.Append(" FROM ReservationRate resRate ");
            sb.Append(" LEFT JOIN Item i  ");
            sb.Append(" ON  resRate.ItemKey = i.ItemKey   ");
            sb.Append(" WHERE ");
            sb.Append("  i.Minibar = 1 AND  resRate.ReservationKey = '" + resKey + "'  ");
            sb.Append(" ORDER BY  resRate.Seq DESC;");

            return sb.ToString();
        }
        private static string GetLaundryItemByResKeyQuery(string resKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT resRate.* ");
            sb.Append(" FROM ReservationRate resRate ");
            sb.Append(" LEFT JOIN Item i  ");
            sb.Append(" ON  resRate.ItemKey = i.ItemKey   ");
            sb.Append(" WHERE ");
            sb.Append("  i.Laundry = 1 AND  resRate.ReservationKey = '" + resKey + "'  ");
            sb.Append(" ORDER BY  resRate.Seq DESC;");

            return sb.ToString();
        }
        private static string GetSystemControlQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT * FROM Control ;    ");
            return sb.ToString();
        }
        private static string GetContactListByMsgCodeQuery(string msgCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("  SELECT DISTINCT s.StaffKey, s.Username , s.Contact_ID , s.FirebaseToken_Id,mt.MessageKey  ");
            sb.Append("  FROM  Staff s");
            sb.Append("  RIGHT JOIN SqoopeGroupLink sgl ON sgl.StaffKey = s.StaffKey ");
            sb.Append("  RIGHT JOIN SqoopeGroup sg ON sg.SqoopeGroupKey = sgl.SqoopeGroupKey ");
            sb.Append("  RIGHT JOIN SqoopeMsgGroupLink smgl ON smgl.SqoopeGroupKey = sg.SqoopeGroupKey ");
            sb.Append("  RIGHT JOIN SqoopeMsgType mt ON mt.MessageKey = smgl.SqoopeMessageKey  ");
            sb.Append("  WHERE mt.Code = '" + msgCode + "'  AND  (s.Sec_Supervisor=10 or s.maidkey IS NOT NULL or s.Contact_ID IS NOT NULL AND s.Contact_ID <> '') AND s.Active=1");//s.Contact_ID IS NOT NULL AND s.Contact_ID <> ''
            sb.Append("  AND s.staffkey IS NOT NULL;");
            // sb.Append("        s.Contact_ID IS NOT NULL AND s.Contact_ID <> '' ;   ");
            return sb.ToString();
        }
        private static string GetIRContactListByMsgCodeQuery(string msgCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("  SELECT DISTINCT s.StaffKey, s.Username , s.Contact_ID , s.FirebaseToken_IdiRepair,mt.MessageKey  ");
            sb.Append("  FROM  Staff s");
            sb.Append("  RIGHT JOIN SqoopeGroupLink sgl ON sgl.StaffKey = s.StaffKey ");
            sb.Append("  RIGHT JOIN SqoopeGroup sg ON sg.SqoopeGroupKey = sgl.SqoopeGroupKey ");
            sb.Append("  RIGHT JOIN SqoopeMsgGroupLink smgl ON smgl.SqoopeGroupKey = sg.SqoopeGroupKey ");
            sb.Append("  RIGHT JOIN SqoopeMsgType mt ON mt.MessageKey = smgl.SqoopeMessageKey  ");
            sb.Append("  WHERE mt.Code = '" + msgCode + "'  AND  (s.TechnicianKey IS NOT NULL or s.Contact_ID IS NOT NULL AND s.Contact_ID <> '')");
            sb.Append("  AND s.staffkey IS NOT NULL;");
            // sb.Append("        s.Contact_ID IS NOT NULL AND s.Contact_ID <> '' ;   ");
            return sb.ToString();
        }
        private static string GetCalculatePriceForGSTExclusiveQuery(double total, string postCodeKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT dbo.UDF_Tax1 (" + total + " , '" + postCodeKey + "') AS Tax1  ;");
            sb.Append("SELECT dbo.UDF_Tax2 (" + total + " , '" + postCodeKey + "') AS Tax2  ;");
            sb.Append("SELECT dbo.UDF_Tax3 (" + total + " , '" + postCodeKey + "') AS Tax3  ;");
            return sb.ToString();
        }
        private static string GetCalculatePriceForGSTInclusiveQuery(double total, string postCodeKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Select * from UDF_Price (" + total + ",'" + postCodeKey + "')");
            return sb.ToString();
        }
        private static string GetHotelRoomQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT   RoomKey, Unit ");
            sb.Append(" FROM  Room ");
            sb.Append(" WHERE Active = 1 ");
            sb.Append(" ORDER BY  Floor, Unit ;");
            return sb.ToString();
        }

        private static string GetHotelRoomFloorQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT   Distinct Floor ");
            sb.Append(" FROM  Room ");
            sb.Append(" ORDER BY   Floor ;");
            return sb.ToString();
        }

        //private static string GetHotelRoomByDateAndFloorQuery(bool blnFilterByRoomStatusKey, bool blnFilterByGuestStatus, bool blnFilterByFloorNo, string gueststatus, string[] list, DateTime searchDate, int floor, string roomStatusKey)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(" select distinct S.RoomKey, Unit,S.MaidStatusKey,M.MaidStatus,S.RoomStatusKey,R.RoomStatus,RoomType,InterconnectRoom,Floor, LinenChange, DND,");
        //    sb.Append(" CleaningTime,LinenDays,Bed,S.CheckInDate,S.CheckInTime,S.CheckOutDate,S.CheckOutTime,I.Maid,HMMNotes, S.Guest  ");
        //    sb.Append(" from dbo.UDF_DayRoomStatusShowiClean( '" + searchDate + "' ) S Left Join (Select A.RoomKey,InterconnectRoom=B.Unit,");
        //    sb.Append(" Maid=M.Name,A.HMMNotes from Room A Left join Room B on A.InterconnectRoomKey=B.RoomKey Left Join Maid M on ");
        //    sb.Append(" A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey ");
        //    sb.Append(" Left Join MaidStatus M on S.MaidStatusKey=M.MaidStatusKey  Left Join RoomStatus R on S.RoomStatusKey=R.RoomStatusKey ");
        //    sb.Append(" Left join Reservation V on S.ReservationKey = V.ReservationKey Left Join GuestStatus GS on V.GuestStatus = GS.StatusCode ");
        //    //sb.Append(" where S.Guest = G.Name ");
        //    if (blnFilterByFloorNo)
        //        sb.Append(" where Floor = " + floor + " ");
        //    if (blnFilterByRoomStatusKey)
        //        if (blnFilterByFloorNo)
        //            sb.Append("  AND   S.RoomStatusKey = '" + roomStatusKey + "'  ");
        //        else
        //            sb.Append("  where   S.RoomStatusKey = '" + roomStatusKey + "'  ");
        //    if (blnFilterByGuestStatus)
        //        if (list != null)
        //        {
        //            if (blnFilterByFloorNo || blnFilterByRoomStatusKey)
        //            {
        //                sb.Append(" AND GS.Status in ( ");
        //                for (int i = 0; i < list.Length; i++)
        //                {
        //                    sb.Append("'" + list[i] + "',");
        //                }
        //                sb.Append(")");
        //            }
        //            else
        //            {
        //                sb.Append(" where GS.Status in ( ");
        //                for (int i = 0; i < list.Length; i++)
        //                {
        //                    if (!string.IsNullOrEmpty(list[i]))
        //                        sb.Append("'" + list[i] + "',");
        //                }
        //                sb.Append(")");
        //            }
        //        }
        //        else
        //        {
        //            if (blnFilterByFloorNo || blnFilterByRoomStatusKey)
        //                sb.Append(" AND GS.Status = '" + gueststatus + "'");
        //            else
        //                sb.Append(" where GS.Status = '" + gueststatus + "'");
        //        }
        //    sb.Append(" order by Floor, Unit ");
        //    var text = sb.ToString();
        //    var str = text.Replace(",)", ")");
        //    return str;
        //}

        //private static string GetChkOutRoomByDateAndFloorQuery(bool blnFilterByFloorNo, DateTime searchDate, int floor)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(" select distinct S.RoomKey, Unit,S.MaidStatusKey,M.MaidStatus,S.RoomStatusKey,R.RoomStatus,RoomType,InterconnectRoom,Floor, LinenChange, DND,");
        //    sb.Append(" CleaningTime,LinenDays,Bed,S.CheckInDate,S.CheckInTime,S.CheckOutDate,S.CheckOutTime,I.Maid,HMMNotes, S.Status, V.RecheckInVirtualRoom  ");
        //    sb.Append(" from dbo.UDF_DayRoomStatusiClean( '" + searchDate + "' ) S Left Join (Select A.RoomKey,InterconnectRoom=B.Unit,");
        //    sb.Append(" Maid=M.Name,A.HMMNotes from Room A Left join Room B on A.InterconnectRoomKey=B.RoomKey Left Join Maid M on ");
        //    sb.Append(" A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey ");
        //    sb.Append(" Left Join MaidStatus M on S.MaidStatusKey=M.MaidStatusKey  Left Join RoomStatus R on S.RoomStatusKey=R.RoomStatusKey ");
        //    sb.Append(" Left join Reservation V on S.ReservationKey = V.ReservationKey ");
        //    //sb.Append(" where V.RecheckInVirtualRoom = 1 ");
        //    if (blnFilterByFloorNo)
        //        sb.Append(" where Floor = " + floor + " ");
        //    sb.Append(" order by Unit ");
        //    return sb.ToString();
        //}

        private static string GetHotelRoomByRoomNoQuery(string roomNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT   r.* , m.Name ");
            sb.Append(" FROM  Room r");
            sb.Append(" LEFT JOIN Maid m ON  m.MaidKey = r.MaidKey ");
            sb.Append(" WHERE  r.Unit = '" + roomNo + "' ;");
            return sb.ToString();
        }

        private static string UpdateAssignUnAssignByRoomNoQuery(Guid MaidKey, string unit)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE   Room ");
            if (MaidKey != Guid.Empty)
            {
                sb.Append(" SET    MaidKey = '" + MaidKey + "' ");
            }
            else
            {
                sb.Append(" SET    MaidKey =null");
            }

            sb.Append(" WHERE  unit= '" + unit + "' ;");
            return sb.ToString();
        }
        //private static string GetSupervisorRoomByMaidStatusKeyQuery(bool blnFilterByMaidKey, bool blnFilterByFloorNo, bool blnFilterByRoomStatusKey, bool blnFilterByMaidStatusKey, DateTime dtBusinessDate, string maidKey, string floorNo, string maidStatusKey, string roomStatusKey)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(" SELECT  ");
        //    sb.Append("   S.RoomKey, Unit,S.MaidStatusKey,M.MaidStatus,S.RoomStatusKey,R.RoomStatus,RoomType,InterconnectRoom,Floor, LinenChange, DND,  ");
        //    sb.Append("   CleaningTime,LinenDays,Bed,CheckInDate,CheckInTime,CheckOutDate,CheckOutTime,I.Maid,HMMNotes,GuestArrived,S.Status,PreCheckInCount  ");
        //    sb.Append(" FROM  ");
        //    sb.Append("   dbo.UDF_DayRoomStatusiClean( '" + dtBusinessDate + "' ) S ");
        //    sb.Append(" LEFT JOIN   ");
        //    sb.Append("   (Select A.RoomKey,InterconnectRoom=B.Unit,");
        //    sb.Append("   Maid=M.Name,A.HMMNotes from Room A Left join Room B on A.InterconnectRoomKey=B.RoomKey Left Join Maid M on ");
        //    sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey ");
        //    sb.Append(" LEFT JOIN   ");
        //    sb.Append("   MaidStatus M on S.MaidStatusKey=M.MaidStatusKey  ");
        //    sb.Append(" LEFT JOIN   ");
        //    sb.Append("   RoomStatus R on S.RoomStatusKey=R.RoomStatusKey ");
        //    if (blnFilterByMaidKey)
        //    {
        //        sb.Append("  Where  S.MaidKey = '" + maidKey + "'  ");
        //    }
        //    if (blnFilterByFloorNo)
        //    {
        //        if (blnFilterByMaidKey)
        //            sb.Append("  AND  Floor = " + floorNo + " ");
        //        else
        //            sb.Append(" Where Floor = " + floorNo + " ");
        //    }
        //    if (blnFilterByRoomStatusKey)
        //    {
        //        if (blnFilterByMaidKey || blnFilterByFloorNo)
        //            sb.Append("  AND  S.RoomStatusKey = '" + roomStatusKey + "' ");
        //        else
        //            sb.Append(" Where S.RoomStatusKey = '" + roomStatusKey + "' ");
        //    }
        //    if (blnFilterByMaidStatusKey)
        //    {
        //        if (blnFilterByMaidKey || blnFilterByFloorNo || blnFilterByRoomStatusKey)
        //            sb.Append("  AND  M.MaidStatusKey = '" + maidStatusKey + "' ");
        //        else
        //            sb.Append(" Where M.MaidStatusKey = '" + maidStatusKey + "' ");
        //    }
        //    sb.Append(" ORDER BY S.GuestArrived, S.PreCheckInCount desc, Floor, S.Unit;  ");

        //    return sb.ToString();
        //}


        //private static string GetRoomByMaidKeyQuery(bool blnFilterByMaidStatusKey, bool blnFilterByRoomStatusKey, bool blnFilterByFloor, bool blnFilterByGuestStatus)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(" SELECT  ");
        //    sb.Append("   S.RoomKey, Unit,S.MaidStatusKey,M.MaidStatus,S.RoomStatusKey,R.RoomStatus,S.ReservationKey,RoomType,InterconnectRoom,Floor, LinenChange, DND, ");
        //    sb.Append("   CleaningTime,LinenDays,Bed,S.CheckInDate,S.CheckInTime,S.CheckOutDate,S.CheckOutTime,I.Maid,HMMNotes,S.GuestArrived,S.Status,S.PreCheckInCount,GS.Status AS GuestStatus  ");
        //    sb.Append(" FROM  dbo.UDF_DayRoomStatusiClean( @date ) S ");
        //    sb.Append(" LEFT JOIN   ");
        //    sb.Append("   (Select A.RoomKey,InterconnectRoom=B.Unit,");
        //    sb.Append("   Maid=M.Name,A.HMMNotes from Room A Left join Room B on A.InterconnectRoomKey=B.RoomKey Left Join Maid M on ");
        //    sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey ");
        //    sb.Append(" LEFT JOIN  MaidStatus M on S.MaidStatusKey=M.MaidStatusKey  ");
        //    sb.Append(" LEFT JOIN  RoomStatus R on S.RoomStatusKey=R.RoomStatusKey ");
        //    sb.Append(" Left join Reservation V on S.ReservationKey = V.ReservationKey Left Join GuestStatus GS on V.GuestStatus = GS.StatusCode ");
        //    sb.Append(" WHERE  ");
        //    sb.Append("    S.MaidKey = @MaidKey ");
        //    if (blnFilterByMaidStatusKey)
        //        sb.Append("  AND  S.MaidStatusKey = @MaidStatusKey ");
        //    if (blnFilterByRoomStatusKey)
        //        sb.Append("  AND  S.RoomStatusKey = @RoomStatusKey ");
        //    if (blnFilterByFloor)
        //        sb.Append(" AND Floor = @Floor ");
        //    if (blnFilterByGuestStatus)
        //        sb.Append(" AND GS.Status = @GuestStatus ");
        //    sb.Append(" ORDER BY S.GuestArrived, S.PreCheckInCount desc, Floor, Unit ;  ");

        //    return sb.ToString();
        //}

        private static string GetRoomCountByMaidKeyQuery(bool blnFilterByFloorNo, DateTime dtBusinessDate, string maidKey, string floorNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  ");
            sb.Append("   CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER) AS MaidStatusKey, 'ALL' AS MaidStatus, Count(R.MaidKey) AS RoomCount ");
            sb.Append(" FROM  ");
            sb.Append("   ROOM R ");
            sb.Append(" WHERE  ");
            sb.Append("    R.MaidKey = '" + maidKey + "' ");
            sb.Append(" GROUP BY  R.MaidKey   ");
            sb.Append(" Union ALL  ");
            sb.Append(" SELECT  ");
            sb.Append("   R.MaidStatusKey,MS.MaidStatus, Count(R.MaidStatusKey) AS RoomCount ");
            sb.Append(" FROM  ");
            sb.Append("   ROOM R ");
            sb.Append(" LEFT JOIN   ");
            sb.Append("   MaidStatus MS on R.MaidStatusKey=MS.MaidStatusKey ");
            sb.Append(" WHERE  ");
            sb.Append("    R.MaidKey = '" + maidKey + "' AND MS.MaidStatus not in ('Maintenance Required', 'Maintenance in the Room') ");
            if (blnFilterByFloorNo)
                sb.Append("  AND  Floor = " + floorNo + " ");
            sb.Append(" GROUP BY  R.MaidStatusKey,MS.MaidStatus  ");
            sb.Append(" ORDER BY  MaidStatus ;  ");

            return sb.ToString();
        }

        private static string GetRoomCountQuery(bool blnFilterByFloorNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  ");
            sb.Append("   CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER) AS MaidStatusKey, 'ALL' AS MaidStatus, Count(R.MaidKey) AS RoomCount ");
            sb.Append(" FROM  ");
            sb.Append("   ROOM R ");
            sb.Append(" WHERE  ");
            sb.Append("    R.MaidKey = @MaidKey ");
            sb.Append(" GROUP BY  R.MaidKey   ");
            sb.Append(" Union ALL  ");
            sb.Append(" SELECT  ");
            sb.Append("   R.MaidStatusKey,MS.MaidStatus, Count(R.MaidStatusKey) AS RoomCount ");
            sb.Append(" FROM  ");
            sb.Append("   ROOM R ");
            sb.Append(" LEFT JOIN   ");
            sb.Append("   MaidStatus MS on R.MaidStatusKey=MS.MaidStatusKey ");
            sb.Append(" WHERE  ");
            sb.Append("  MS.MaidStatus in ('Clean', 'Dirty') ");
            if (blnFilterByFloorNo)
                sb.Append("  AND  Floor = @FloorNo ");
            sb.Append(" GROUP BY  R.MaidStatusKey,MS.MaidStatus  ");
            sb.Append(" ORDER BY  MaidStatus ;  ");

            return sb.ToString();
        }
        private static string GetRoomCountQuerySup(bool blnFilterByFloorNo, DateTime dtBusinessDate, string maidKey, string floorNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  ");
            //sb.Append("   CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER) AS MaidStatusKey, 'ALL' AS MaidStatus, Count(R.MaidKey) AS RoomCount ");
            sb.Append("   CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER) AS MaidStatusKey, 'ALL' AS MaidStatus, Count(R.MaidStatusKey) AS RoomCount ");
            sb.Append(" FROM  ");
            sb.Append("   ROOM R ");
            //sb.Append(" WHERE  ");
            //sb.Append("    R.MaidKey = '" + maidKey + "' ");
            //sb.Append(" GROUP BY  R.MaidKey   ");
            sb.Append(" Union ALL  ");
            sb.Append(" SELECT  ");
            sb.Append("   R.MaidStatusKey,MS.MaidStatus, Count(R.MaidStatusKey) AS RoomCount ");
            sb.Append(" FROM  ");
            sb.Append("   ROOM R ");
            sb.Append(" LEFT JOIN   ");
            sb.Append("   MaidStatus MS on R.MaidStatusKey=MS.MaidStatusKey ");
            sb.Append(" WHERE  ");
            sb.Append("  MS.MaidStatus in ('Clean', 'Dirty') ");
            if (blnFilterByFloorNo)
                sb.Append("  AND  Floor = " + floorNo + " ");
            sb.Append(" GROUP BY  R.MaidStatusKey,MS.MaidStatus  ");
            sb.Append(" ORDER BY  MaidStatus ;  ");

            return sb.ToString();
        }
        //private static string GetRoomByMaidStatusKeyQuery(bool blnFilterByMaidKey, bool blnFilterByFloorNo, bool blnFilterByRoomStatusKey, DateTime dtBusinessDate, string maidStatusKey, string maidKey, string floorNo, string roomStatusKey)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(" SELECT  ");
        //    sb.Append("   S.RoomKey, Unit,S.MaidStatusKey,M.MaidStatus,S.RoomStatusKey,R.RoomStatus,RoomType,InterconnectRoom,Floor, LinenChange, DND,  ");
        //    sb.Append("   CleaningTime,LinenDays,Bed,CheckInDate,CheckInTime,CheckOutDate,CheckOutTime,I.Maid,HMMNotes,GuestArrived,S.Status,PreCheckInCount  ");
        //    sb.Append(" FROM  ");
        //    sb.Append("   dbo.UDF_DayRoomStatusiClean( '" + dtBusinessDate + "' ) S ");
        //    sb.Append(" LEFT JOIN   ");
        //    sb.Append("   (Select A.RoomKey,InterconnectRoom=B.Unit,");
        //    sb.Append("   Maid=M.Name,A.HMMNotes from Room A Left join Room B on A.InterconnectRoomKey=B.RoomKey Left Join Maid M on ");
        //    sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey ");
        //    sb.Append(" LEFT JOIN   ");
        //    sb.Append("   MaidStatus M on S.MaidStatusKey=M.MaidStatusKey  ");
        //    sb.Append(" LEFT JOIN   ");
        //    sb.Append("   RoomStatus R on S.RoomStatusKey=R.RoomStatusKey ");
        //    sb.Append(" WHERE  ");
        //    sb.Append("    M.MaidStatusKey = '" + maidStatusKey + "'");
        //    if (blnFilterByMaidKey)
        //        sb.Append("  AND  S.MaidKey = '" + maidKey + "'");
        //    if (blnFilterByFloorNo)
        //        sb.Append("  AND  Floor = " + floorNo + " ");
        //    if (blnFilterByRoomStatusKey)
        //        sb.Append("  AND  S.RoomStatusKey = '" + roomStatusKey + "' ");
        //    sb.Append(" ORDER BY S.GuestArrived, S.PreCheckInCount desc, Floor, S.Unit;  ");

        //    return sb.ToString();
        //}

        private static string UpdateMaidStatusByRoomKeyQuery(bool blnUpdateNote)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE   Room ");
            sb.Append(" SET    MaidStatusKey = @MaidStatusKey ");
            if (blnUpdateNote)
                sb.Append(" , HMMNotes = @HMMNotes ");
            sb.Append(" WHERE  RoomKey = @RoomKey ;");
            return sb.ToString();
        }


        private static string UpdateDNDStatusByRoomKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE   Room ");
            sb.Append(" SET    DND = @DND ");
            sb.Append(" WHERE  RoomKey = @RoomKey ;");
            return sb.ToString();
        }
        private static string UpdateAssignAttendantByRoomKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE   Room ");
            sb.Append(" SET    MaidKey = @MaidKey ");
            sb.Append(" WHERE  RoomKey = @RoomKey ;");
            return sb.ToString();
        }

        //private static string GetRoomBlockByRoomKeyQuery(string roomkey, DateTime sysDate)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(" select * from RoomBlock where RoomKey = '" + roomkey + "' and BlockDate = '" + sysDate + "' and Active in (1,2)  ");
        //    return sb.ToString();
        //}
        private static string GetStaffInfoByRoomNoQuery(string roomNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT * FROM Staff s ");
            sb.Append(" LEFT JOIN  Room r ON r.MaidKey = s.MaidKey ");
            sb.Append(" WHERE r.Unit = " + roomNo + " ;  ");
            return sb.ToString();
        }
        private static string GetAllReasonQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT HousekeepingOptOutReasonCode,Reason FROM HousekeepingOptOutReason");
            sb.Append(" where Active = 1 ORDER BY  HousekeepingOptOutReasonCode;");
            return sb.ToString();
        }
        
        private static string GetAllDirtyReasonQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT HousekeepingDirtyReasonCode,Reason FROM HousekeepingDirtyReason");
            sb.Append(" where Active = 1 ORDER BY  HousekeepingDirtyReasonCode;");
            return sb.ToString();
        }
        private static string InsertDndImageQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert into Dndphoto ([DndphotoKey],[RoomKey],[LastModifiedStaff],[Sort],[Document],[Image],[CreatedDate]) ");
            sb.Append(" Values (@DndphotoKey,@RoomKey,@LastModifiedStaff,@Sort,@Document,@Image,ISNULL((SELECT CAST(CONVERT(date, SystemDate) AS datetime) + CAST(CONVERT(time, GETDATE()) AS datetime) FROM Control WHERE SystemDate IS NOT NULL),GETDATE()))");//getdate()) ");
            return sb.ToString();
        }
        private static string UpdateDndImageQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Update Dndphoto set [LastModifiedStaff]=@LastModifiedStaff,[Document]=@Document,[Image]=@Image ");
            sb.Append(" where RoomKey=@RoomKey");
            return sb.ToString();
        }
        private static string CheckDndImageQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select [RoomKey] from Dndphoto ");
            sb.Append(" where RoomKey=@RoomKey ");
            return sb.ToString();
        }

       

        #endregion
    }
}
