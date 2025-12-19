 using Abp.Data;
using Abp.EntityFrameworkCore;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.EntityFrameworkCore;
using BEZNgCore.EntityFrameworkCore.Repositories;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;
using Stripe;
using Stripe.Terminal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Abp.Domain.Uow.AbpDataFilters;
using static Castle.MicroKernel.ModelBuilder.Descriptors.InterceptorDescriptor;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BEZNgCore.Authorization.IrepairDal
{
    public class MaidStatusRepository : BEZNgCoreRepositoryBase<MaidStatus, Guid>, IMaidStatusRepository
    {
        private readonly IActiveTransactionProvider _transactionProvider;
        ConnectionManager _connectionManager;
        public MaidStatusRepository(IDbContextProvider<BEZNgCoreDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
            _connectionManager = new ConnectionManager(dbContextProvider, _transactionProvider);
        }

        #region interfaceimplement & related function 
        public List<ViewLogOutput> GetHistory(string logType, string staffKey, string roomKey)
        {
            var result = new List<ViewLogOutput>();
            DataTable dt = new DataTable();
            bool blnByType = false;
            bool blnByStaff = false;
            bool blnByRoom = false;
            if (!string.IsNullOrEmpty(logType))
                blnByType = true;
            if (!string.IsNullOrEmpty(staffKey) && !staffKey.Equals(Guid.Empty.ToString()))
                blnByStaff = true;
            if (!string.IsNullOrEmpty(roomKey) && !roomKey.Equals(Guid.Empty.ToString()))
                blnByRoom = true;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetHistoryQuery(blnByType, blnByStaff, blnByRoom, logType, staffKey, roomKey), CommandType.Text, MultiTenancySide))
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
                    ViewLogOutput g = new ViewLogOutput();
                    g.ChangedDate = Convert.ToDateTime(dr["ChangedDate"]);
                    g.TableName = !DBNull.Value.Equals(dr["TableName"]) ? dr["TableName"].ToString() : "";
                    g.Detail = !DBNull.Value.Equals(dr["Detail"]) ? dr["Detail"].ToString() : "";
                    g.ChangeDateDes = Convert.ToDateTime(dr["ChangedDate"]).ToString("dd/MM/yyyy HH:mm:ss");
                    result.Add(g);

                }
            }
            return result;
        }

        public List<ViewLogIROutput> GetIRHistory(string logType, string staffKey)
        {
            var lst = new List<ViewLogIROutput>();
            DataTable dt = new DataTable();
            bool blnByType = false;
            bool blnByStaff = false;
            if (!string.IsNullOrEmpty(logType))
                blnByType = true;
            if (!string.IsNullOrEmpty(staffKey) && !staffKey.Equals(Guid.Empty.ToString()))
                blnByStaff = true;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@TableName", SqlDbType.VarChar)
                {
                    Value =blnByType==true?logType:DBNull.Value
                },
                new SqlParameter("@StaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value = blnByStaff==true?Guid.Parse(staffKey):DBNull.Value
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetHistoryQuery(blnByType, blnByStaff), CommandType.Text, MultiTenancySide, parameters))
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
                    ViewLogIROutput g = new ViewLogIROutput();
                    g.ChangedDate = (DateTime)(Convert.IsDBNull(dr["ChangedDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["ChangedDate"])).Value);
                    g.DateTime = GetDateTimeToDisplay(g.ChangedDate);
                    g.LogType = (!DBNull.Value.Equals(dr["TableName"])) ? (!string.IsNullOrEmpty(dr["TableName"].ToString()) ? dr["TableName"].ToString() : "") : "";
                    g.Details = (!DBNull.Value.Equals(dr["Detail"])) ? (!string.IsNullOrEmpty(dr["Detail"].ToString()) ? dr["Detail"].ToString() : "") : "";
                    lst.Add(g);
                }
            }
            return lst;
        }
        public static string GetDateTimeToDisplay(object inputDate)
        {
            try
            {
                string strReturnValue = "";
                if (inputDate != null && inputDate.ToString() != "")
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
        public List<DDLMPriorityOutput> GetDDLPriority()
        {
            var lst = new List<DDLMPriorityOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetPriorityQuery(), CommandType.Text, MultiTenancySide))
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
                    DDLMPriorityOutput o = new DDLMPriorityOutput();
                    o.strPriorityStatus = !DBNull.Value.Equals(dr["PriorityID"]) ? dr["PriorityID"].ToString() : "";
                    o.strPriorityDesc = !DBNull.Value.Equals(dr["Priority"]) ? dr["Priority"].ToString() : "";
                    lst.Add(o);
                }
            }
            return lst;
        }

        #region iclean
        public List<HouseKeeping> LoadHouseKeepingList_New(DateTime dtBusinessDate, string maidKey)
        {
            var lst = new List<HouseKeeping>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@date", SqlDbType.DateTime)
                {
                    Value =dtBusinessDate
                },
                new SqlParameter("@MaidKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(maidKey)
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetHousekeeping(), CommandType.Text, MultiTenancySide, parameters))
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
                    HouseKeeping g = new HouseKeeping();
                    g.Unit = (!DBNull.Value.Equals(dr["Unit"])) ? (!string.IsNullOrEmpty(dr["Unit"].ToString()) ? dr["Unit"].ToString() : "") : "";
                    //(DateTime)(Convert.IsDBNull(dr["ChangedDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["ChangedDate"])).Value);
                    g.RoomStatus = (!DBNull.Value.Equals(dr["RoomStatus"])) ? (!string.IsNullOrEmpty(dr["RoomStatus"].ToString()) ? dr["RoomStatus"].ToString() : "") : "";
                    g.RoomType = (!DBNull.Value.Equals(dr["RoomType"])) ? (!string.IsNullOrEmpty(dr["RoomType"].ToString()) ? dr["RoomType"].ToString() : "") : "";
                    List<dynamic> slist = LoadServiceList(dr["RoomType"].ToString());
                    if (slist.Count == 3)
                    {
                        if (dr["RoomStatus"].ToString() == "Occupied")
                        {
                            int LT = slist[1].Val;
                            g.Services = LT.ToString();
                        }
                        else if (dr["RoomStatus"].ToString() == "Due Out")
                        {
                            int DS = slist[2].Val;
                            g.Services = DS.ToString();
                        }
                        else
                        {
                            int FS = slist[0].Val;
                            g.Services = FS.ToString();
                        }
                    }
                    lst.Add(g);
                }
            }
            return lst;

        }

        private List<dynamic> LoadServiceList(string roomtypeKey)
        {
            List<dynamic> slist = new List<dynamic>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            string strRooms = "SELECT Col,Val FROM (SELECT * FROM RoomType where RoomType='" + roomtypeKey + "')" + " P  UNPIVOT( val FOR Col IN ([FS], [LT], [DS]) ) pvt";
            using (var command = _connectionManager.CreateCommandOnly(strRooms, CommandType.Text, MultiTenancySide))
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
                    slist.Add(new
                    {
                        Col = Convert.IsDBNull(dr["Col"]) ? string.Empty : Convert.ToString(dr["Col"]),
                        Val = Convert.IsDBNull(dr["Val"]) ? 0 : GetIntFromString(Convert.ToString(dr["Val"]))
                    });
                   
                }
            }
            return slist;
        }

        private static int? GetIntFromString(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? new int?(result) : null);
        }

        private static string GetHousekeeping()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select Unit, R.RoomStatus,RoomType ");
            sb.Append(" from dbo.UDF_DayRoomStatus(@date) S Left Join(Select A.RoomKey, InterconnectRoom = B.Unit, ");
            sb.Append(" Maid = M.Name, A.HMMNotes from Room A Left join Room B on A.InterconnectRoomKey = B.RoomKey Left Join Maid M on ");
            sb.Append("  A.MaidKey = M.MaidKey where A.Active = 1) I on S.RoomKey = I.RoomKey ");
            sb.Append(" Left Join MaidStatus M on S.MaidStatusKey = M.MaidStatusKey  Left Join RoomStatus R on S.RoomStatusKey = R.RoomStatusKey");
            sb.Append(" where(@MaidKey is null or s.MaidKey = @MaidKey) ");
            sb.Append(" order by s.Floor, Unit ");
            return sb.ToString();
        }
        public List<ShowLogOutput> GetRoomHistory(string staffkey, string roomkey, int pageSize)
        {

            var result = new List<ShowLogOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            bool blnByStaff = false;
            bool blnByRoom = false;

            if (!string.IsNullOrEmpty(staffkey) && !staffkey.Equals(Guid.Empty.ToString()))
                blnByStaff = true;
            if (!string.IsNullOrEmpty(roomkey) && !roomkey.Equals(Guid.Empty.ToString()))
                blnByRoom = true;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@StaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value = blnByStaff==true?Guid.Parse(staffkey):DBNull.Value
                },
                new SqlParameter("@SourceKey", SqlDbType.UniqueIdentifier)
                {
                    Value = blnByRoom==true?Guid.Parse(roomkey):DBNull.Value
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetRoomHistoryQuery(blnByStaff, blnByRoom, pageSize), CommandType.Text, MultiTenancySide, parameters))
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
                    ShowLogOutput g = new ShowLogOutput();
                    g.GetDateTimeToDisplay = GetDateTimeToDisplay((DateTime)(Convert.IsDBNull(dr["ChangedDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["ChangedDate"])).Value)) + "=>  " + (!DBNull.Value.Equals(dr["Detail"]) ? dr["Detail"].ToString() : "");

                    result.Add(g);

                }
            }
            return result;
        }
        public void InsertSupLinenItem(SupervisorCheckList supervisorCheckList, Guid roomKey, Guid staffKey)
        {
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                SqlParameter[] parameters = new SqlParameter[]
                {
                
                new SqlParameter("@ItemKey", SqlDbType.UniqueIdentifier)
                {
                    Value = supervisorCheckList.ItemKey
                },
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value =  roomKey
                },
                new SqlParameter("@DocDate", SqlDbType.DateTime)
                {
                    Value = supervisorCheckList.DocDate
                },
                new SqlParameter("@Checked", SqlDbType.Int)
                {
                    Value = supervisorCheckList.Checked
                },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = staffKey
                },
                 new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = supervisorCheckList.TenantId
                }
                };
                using (var command = _connectionManager.CreateCommandSP(InsertSupLinenItemQueryS(), CommandType.Text, MultiTenancySide, parameters))
                {

                    command.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            //try
            //{
            //    _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            //    using (var command = _connectionManager.CreateCommandOnly(InsertSupLinenItemQuery(supervisorCheckList.ItemKey, roomKey, supervisorCheckList.DocDate, supervisorCheckList.Checked, staffKey, (int)supervisorCheckList.TenantId), CommandType.Text, MultiTenancySide))
            //    {
            //        command.ExecuteNonQuery();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public void UpdateSupLinenItem(SupervisorCheckList supervisorCheckList, Guid roomKey, Guid staffKey)
        {
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@InspectionChecklistKey", SqlDbType.UniqueIdentifier)
                {
                    Value = supervisorCheckList.Id
                },
                new SqlParameter("@ItemKey", SqlDbType.UniqueIdentifier)
                {
                    Value = supervisorCheckList.ItemKey
                },
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value =  roomKey
                },
                new SqlParameter("@DocDate", SqlDbType.DateTime)
                {
                    Value = supervisorCheckList.DocDate
                },
                new SqlParameter("@Checked", SqlDbType.Int)
                {
                    Value = supervisorCheckList.Checked
                },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = staffKey
                },
                 new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = supervisorCheckList.TenantId
                }
                };
                using (var command = _connectionManager.CreateCommandSP(UpdateSupLinenItemQueryS(), CommandType.Text, MultiTenancySide, parameters))
                {

                    command.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            //try
            //{
            //    _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            //    using (var command = _connectionManager.CreateCommandOnly(UpdateSupLinenItemQuery(supervisorCheckList.Id, supervisorCheckList.ItemKey, roomKey, supervisorCheckList.DocDate, supervisorCheckList.Checked, staffKey, (int)supervisorCheckList.TenantId), CommandType.Text, MultiTenancySide))
            //    {
            //        command.ExecuteNonQuery();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        public async Task<string> GetServiceRefuseKey(Guid ItemKey)
        {
            string dt = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetServiceRefuseKeySQL(ItemKey), CommandType.Text, MultiTenancySide))
            {
                using (var dr = await command.ExecuteReaderAsync())
                {
                    while (dr.Read())
                    {
                        dt = !DBNull.Value.Equals(dr["ItemKey"]) ? dr["ItemKey"].ToString() : "";

                    }
                    dr.Close();
                }
            }
            return dt;
        }
        public void InsertAttLinenItem(AttendantCheckList attendantCheckList, Guid roomKey, Guid staffKey)
        {
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                SqlParameter[] parameters = new SqlParameter[]
                {
               
                new SqlParameter("@ItemKey", SqlDbType.UniqueIdentifier)
                {
                    Value = attendantCheckList.ItemKey
                },
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value =  roomKey
                },
                new SqlParameter("@DocDate", SqlDbType.DateTime)
                {
                    Value = attendantCheckList.DocDate
                },
                new SqlParameter("@Quantity", SqlDbType.Int)
                {
                    Value = attendantCheckList.Quantity
                },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = staffKey
                },
                 new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = attendantCheckList.TenantId
                }
                };
                using (var command = _connectionManager.CreateCommandSP(InsertAttLinenItemQueryS(), CommandType.Text, MultiTenancySide, parameters))
                {

                    command.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            //try
            //{
            //    _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            //    using (var command = _connectionManager.CreateCommandOnly(InsertAttLinenItemQuery(attendantCheckList.ItemKey, roomKey, attendantCheckList.DocDate, attendantCheckList.Quantity, staffKey, (int)attendantCheckList.TenantId), CommandType.Text, MultiTenancySide))
            //    {
            //        command.ExecuteNonQuery();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public void UpdateAttLinenItem(AttendantCheckList attendantCheckList, Guid roomKey, Guid staffKey)
        {
           
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@LinenChecklistKey", SqlDbType.UniqueIdentifier)
                {
                    Value = attendantCheckList.Id
                },
                new SqlParameter("@ItemKey", SqlDbType.UniqueIdentifier)
                {
                    Value = attendantCheckList.ItemKey
                },
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value =  roomKey
                },
                new SqlParameter("@DocDate", SqlDbType.DateTime)
                {
                    Value = attendantCheckList.DocDate
                },
                new SqlParameter("@Quantity", SqlDbType.Int)
                {
                    Value = attendantCheckList.Quantity
                },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = staffKey
                },
                 new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = attendantCheckList.TenantId
                }
                };
                using (var command = _connectionManager.CreateCommandSP(UpdateAttLinenItemQueryS(), CommandType.Text, MultiTenancySide, parameters))
                {

                   command.ExecuteNonQuery();
                }
             

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //try
            //{
            //    _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            //    using (var command = _connectionManager.CreateCommandOnly(UpdateAttLinenItemQuery(attendantCheckList.Id, attendantCheckList.ItemKey, roomKey, attendantCheckList.DocDate, attendantCheckList.Quantity, staffKey, (int)attendantCheckList.TenantId), CommandType.Text, MultiTenancySide))
            //    {
            //        command.ExecuteNonQuery();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        public async Task<List<GetDashRoomByMaidKeyOutput>> GetMyTaskBindGrid(DateTime dtBusinessDate, string maidKey, string maidStatusKey, string roomStatusKey, string floor, string guestStatus, bool hasStartedTask, Guid staffKey)
        {
            var result = new List<GetDashRoomByMaidKeyOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            try
            {
                bool blnFilterByMaidStatusKey = false;
                if (!string.IsNullOrEmpty(maidStatusKey) && !maidStatusKey.Equals(Guid.Empty.ToString()))
                    blnFilterByMaidStatusKey = true;
                bool blnFilterByRoomStatusKey = false;
                if (!string.IsNullOrEmpty(roomStatusKey) && !roomStatusKey.Equals(Guid.Empty.ToString()))
                    blnFilterByRoomStatusKey = true;
                bool blnFilterByFloor = false;
                if (!string.IsNullOrEmpty(floor) && floor != "0")
                    blnFilterByFloor = true;
                bool blnFilterByGuestStatus = false;
                if (!string.IsNullOrEmpty(guestStatus) && guestStatus != "ALL")
                    blnFilterByGuestStatus = true;
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
                new SqlParameter("@MaidStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value = blnFilterByMaidStatusKey==true?Guid.Parse(maidStatusKey):DBNull.Value
                },
                new SqlParameter("@RoomStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value = blnFilterByRoomStatusKey==true?Guid.Parse(roomStatusKey):DBNull.Value
                },
                new SqlParameter("@Floor", SqlDbType.Int)
                {
                    Value = blnFilterByFloor==true?Convert.ToInt32(floor):DBNull.Value
                },
                new SqlParameter("@GuestStatus", SqlDbType.VarChar)
                {
                    Value = blnFilterByGuestStatus==true?guestStatus:DBNull.Value
                }
             };
                using (var command = _connectionManager.CreateCommandSP(GetRoomByMaidKeyQuery(blnFilterByMaidStatusKey, blnFilterByRoomStatusKey, blnFilterByFloor, blnFilterByGuestStatus), CommandType.Text, MultiTenancySide, parameters))
                {
                    using (var dr = await command.ExecuteReaderAsync())
                    {

                        while (dr.Read())
                        {
                            GetDashRoomByMaidKeyOutput g = new GetDashRoomByMaidKeyOutput();
                            g.Floor = !DBNull.Value.Equals(dr["Floor"]) ? Convert.ToInt32(dr["Floor"]) : 0;
                            g.RoomKey = (!DBNull.Value.Equals(dr["RoomKey"])) ? (!string.IsNullOrEmpty(dr["RoomKey"].ToString()) ? new Guid(dr["RoomKey"].ToString()) : Guid.Empty) : Guid.Empty;
                            g.Unit = !DBNull.Value.Equals(dr["Unit"]) ? dr["Unit"].ToString() : "";
                            g.GuestArrived = !DBNull.Value.Equals(dr["GuestArrived"]) ? dr["GuestArrived"].ToString() == "99:99" ? "" : dr["GuestArrived"].ToString() : "";
                            g.GuestArrivedOrign = !DBNull.Value.Equals(dr["GuestArrived"]) ? dr["GuestArrived"].ToString() : "";
                            g.Status = !DBNull.Value.Equals(dr["Status"]) ? Convert.ToInt32(dr["Status"]) : 0;
                            g.PreCheckInCount = !DBNull.Value.Equals(dr["PreCheckInCount"]) ? Convert.ToInt32(dr["PreCheckInCount"]) : 0;
                            g.RoomType = !DBNull.Value.Equals(dr["RoomType"]) ? dr["RoomType"].ToString() : "";
                            g.LinenChange = !DBNull.Value.Equals(dr["LinenChange"]) ? dr["LinenChange"].ToString() : "";
                            g.InterconnectRoom = !DBNull.Value.Equals(dr["InterconnectRoom"]) ? dr["InterconnectRoom"].ToString() : "";
                            g.RoomStatus = !DBNull.Value.Equals(dr["RoomStatus"]) ? dr["RoomStatus"].ToString() : "";
                            g.MaidStatus = !DBNull.Value.Equals(dr["MaidStatus"]) ? dr["MaidStatus"].ToString() : "";
                            g.GuestStatus = !DBNull.Value.Equals(dr["GuestStatus"]) ? dr["GuestStatus"].ToString() : "";
                            g.ReservationKey = (!DBNull.Value.Equals(dr["ReservationKey"])) ? (!string.IsNullOrEmpty(dr["ReservationKey"].ToString()) ? new Guid(dr["ReservationKey"].ToString()) : Guid.Empty) : Guid.Empty;
                            g.HMMNotes = !DBNull.Value.Equals(dr["HMMNotes"]) ? dr["HMMNotes"].ToString() : "";
                            g.DND = !DBNull.Value.Equals(dr["DND"]) ? Convert.ToInt32(dr["DND"]) : 0;
                            g.Adult= !DBNull.Value.Equals(dr["Adult"]) ? Convert.ToInt32(dr["Adult"]) : 0;
                            g.Child = !DBNull.Value.Equals(dr["Child"]) ? Convert.ToInt32(dr["Child"]) : 0;
                            g.Group1 = !DBNull.Value.Equals(dr["Group1"]) ? dr["Group1"].ToString() : "";
                            g.Group2 = !DBNull.Value.Equals(dr["Group2"]) ? dr["Group2"].ToString() : "";
                            g.Group3 = !DBNull.Value.Equals(dr["Group3"]) ? dr["Group3"].ToString() : "";
                            g.Group4 = !DBNull.Value.Equals(dr["Group4"]) ? dr["Group4"].ToString() : "";
                            //g.ETA = !DBNull.Value.Equals(dr["ETA"]) ? dr["ETA"].ToString() : "";
                            g.ETA = GetETA(dtBusinessDate,g.RoomKey);
                            g.ETD = !DBNull.Value.Equals(dr["ETD"]) ? dr["ETD"].ToString() : "";
                            #region mobileui
                            g.LinenChangeDes = g.LinenChange.Equals("Y") ? "Yes" : "No";
                            //if (g.ReservationKey != Guid.Empty)
                            //{
                            //    g.Items = GetItemCode(g.ReservationKey);
                            //}
                            //else { g.Items = ""; }
                            g.Items = !DBNull.Value.Equals(dr["ItemCodes"]) ? dr["ItemCodes"].ToString() : "";
                            g.GetRoomDNDButton = GetRoomDNDButton(g.MaidStatus, g.DND, g.Unit);
                            g.GetStartOrEndTaskButton = GetStartOrEndTaskButton(g.MaidStatus, g.Unit, hasStartedTask, g.DND, g.RoomKey);//, staffKey.ToString());
                            g.GetGuestArrived = GetGuestArrived(g.GuestArrivedOrign.ToString(), g.Status.ToString());
                            g.PreCheckInCountDes = g.PreCheckInCount.ToString() != "1" ? "" : "(Pre Check-In)";
                            g.RoomStatusColor = GetRoomStatus(g.RoomStatus);
                            g.AttendantStatusColor = GetMaidStatus(g.MaidStatus);
                            g.StartStatus = hasStartedTask == true ? "Disable" : "Enable";
                            g.LoginStaffKey = staffKey.ToString();
                            g.Pax = GetPax(g.Adult.ToString(), g.Child.ToString());
                            g.MarketSegment = GetMKG(g.Group1, g.Group2, g.Group3,g.Group4);
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
        #region mobileui
        private string GetMaidStatus(string status)
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
        public string GetRoomOptButton(string attendantStatus,string roomNo)
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
                    Value = (!string.IsNullOrEmpty(roomNo) && roomNo != "") ? roomNo : DBNull.Value
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
        private string GetRoomStatus(string status)
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
        private string GetGuestArrived(string inputValue, string status)
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
        private string GetStartOrEndTaskButton(string maidStatus, string roomNo, bool blnHasStartedTask, int? dndStatus, Guid? roomkey)//, string staffkey)
        {
            string strReturnValue = maidStatus.ToString();
            string HouseKeepingMaidStatusMaidInRoom = "Attendant";
            string HouseKeepingMaidStatusMaidInRoom2 = "Maid In Room";
            try
            {
                if (!dndStatus.ToString().Equals("1"))
                {
                    if (maidStatus.ToString().Equals("Dirty"))
                    {
                        if (blnHasStartedTask.ToString().ToLower().Equals("false"))
                            strReturnValue = "two";
                        //"<a  id=\"btnStart" + roomNo.ToString() + "\" runat=\"server\"   class=\"btn btn-success btn-lg\"  href=\"javascript:OnStartTask('" + roomNo.ToString() + "');\" > <span class=\"text-white\" style=\"\"> <span class=\"glyphicon glyphicon-play\" ></span> START </span></a>" +
                        //              "<a id=\"btnLog\" runat=\"server\" class=\"btn btn-default btn-lg\" style=\"background-color: #E1BB20; margin-left: 8px;\" href=\"javascript:ShowLog('" + roomkey.ToString() + "', '" + staffkey + "');\" > <span class=\"text-white\"> <span class=\"fa fa-history\" ></span> History</span></a>";
                        else
                            strReturnValue = "two";
                        //"<a  id=\"btnStart" + roomNo.ToString() + "\" runat=\"server\"   class=\"btn btn-success disabled btn-outline btn-lg\"  href=\"javascript:OnStartTask('" + roomNo.ToString() + "');\" > <span class=\"text-white\" style=\"\"> <span class=\"glyphicon glyphicon-play\" ></span> START </span></a>" +
                        //              "<a id=\"btnLog\" runat=\"server\" class=\"btn btn-default btn-lg\" style=\"background-color: #E1BB20; margin-left: 8px;\" href=\"javascript:ShowLog('" + roomkey.ToString() + "', '" + staffkey + "');\" > <span class=\"text-white\"> <span class=\"fa fa-history\" ></span> History</span></a>";
                    }
                    else if (maidStatus.ToString().Equals(HouseKeepingMaidStatusMaidInRoom) || maidStatus.ToString().Equals(HouseKeepingMaidStatusMaidInRoom2))
                    {
                        strReturnValue = "four";
                        //"<a  id=\"btnCancel" + roomNo.ToString() + "\" runat=\"server\"   class=\"btn btn-primary btn-lg\"  href=\"javascript:OnDelayTask('" + roomNo.ToString() + "');\" > <span class=\"text-white\" style=\"\"> <span class=\"glyphicon glyphicon-pause\" ></span> Pause </span> </a>" +
                        //                "<a  id=\"btnEnd" + roomNo.ToString() + "\" runat=\"server\" style=\"margin-left: 8px; margin-right: 6px;\"  class=\"btn btn-danger btn-lg\"  href=\"javascript:OnEndTask('" + roomNo.ToString() + "');\" >  <span class=\"text-white\" style=\"\"> <span class=\"glyphicon glyphicon-stop\" ></span> END </span></a>" +
                        //                "<br/> <a  id=\"btnLinenSheet" + roomNo.ToString() + "\" runat=\"server\" class=\"btn btn-success btn-lg\" style=\"margin-top: 10px;\"  href=\"javascript:OnLinenSheetTask('" + roomNo.ToString() + "');\" >  <span class=\"text-white\" style=\"\"> <span class=\"glyphicon glyphicon-list-alt\" ></span> Attendant Checklist </span></a> <br/>" +
                        //                "<a id =\"btnLog\" runat=\"server\" class=\"btn btn-default btn-lg\" style=\"background-color: #E1BB20; margin-top: 10px;\" href=\"javascript:ShowLog('" + roomkey.ToString() + "', '" + staffkey + "');\" > <span class=\"text-white\"> <span class=\"fa fa-history\" ></span> History</span></a>";
                    }
                    else
                    {
                    }
                }

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
                    strReturnValue=string.Join(",", array);
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
       
        //private DataTable GetItemByReservationKey(string reskey)
        //{
        //    DataTable dt = new DataTable();
        //    var context = GetContext();
        //    var connection = context.Database.GetDbConnection();

        //    if (connection.State != ConnectionState.Open)
        //    {
        //        connection.Open();
        //    }
        //    try
        //    {

        //        using (SqlCommand command = (SqlCommand)connection.CreateCommand())
        //        {

        //            command.CommandType = CommandType.Text;
        //            command.CommandText = GetItemByReservationKeyQuery();
        //            command.Parameters.Add("@ReservationKey", SqlDbType.UniqueIdentifier).Value = new Guid(reskey);
        //            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
        //            {
        //                adapter.Fill(dt);
        //            }
        //        }
        //        return dt;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataTable GetAllMaidStatus()
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetAllMaidStatusQuery(), CommandType.Text, MultiTenancySide))
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
        public async Task<List<GetMaidStatusOutput>> GetMaidStatusKeyByStatusAsync(string status)
        {
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetMaidStatusByStatusQuery(status), CommandType.Text, MultiTenancySide))
            {
                using (var dr = await command.ExecuteReaderAsync())
                {
                    var result = new List<GetMaidStatusOutput>();
                    while (dr.Read())
                    {
                        GetMaidStatusOutput o = new GetMaidStatusOutput();
                        o.MaidStatusKey = dr["MaidStatusKey"].ToString();
                        result.Add(o);
                    }
                    dr.Close();
                    return result;
                }
            }
        }

        public int UpdateMaidStatusByRoomKey(Guid roomKey, Guid maidStatusKey, string hMMNotes)
        {
            int s = 0;
            bool blnUpdateNote = false;
            if (!string.IsNullOrEmpty(hMMNotes))
                blnUpdateNote = true;
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);

                using (var command = _connectionManager.CreateCommandOnly(UpdateMaidStatusByRoomKeyQuery(blnUpdateNote, roomKey, maidStatusKey, hMMNotes), CommandType.Text, MultiTenancySide))
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

        public int InsertHistory(History h)
        {
            int s = 0;

            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);

                using (var command = _connectionManager.CreateCommandOnly(InsertHistoryQuery(h), CommandType.Text, MultiTenancySide))
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

        public void UpdateRoomHistoryLinkKey(Guid historyKey, Guid roomKey)
        {
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);

                using (var command = _connectionManager.CreateCommandOnly(GetUpdateRoomHistoryLinkKeyQuery(historyKey, roomKey), CommandType.Text, MultiTenancySide))
                {
                    command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public Task<int> GetAttLinenItemByKey(Guid roomkey, DateTime date)
        {

            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
              {

                new SqlParameter("@date", SqlDbType.DateTime)
                {
                    Value = date
                },
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value = roomkey
                }
              };

            using (var command = _connectionManager.CreateCommandSP(GetLinenItemByKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                int dt = 0;
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    return Task.FromResult(ds.Tables[0].Rows.Count);
                }
                else { return Task.FromResult(dt); }
            }
        }
        public void StopUpdateLinenItem(Guid roomKey)
        {
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                using (var command = _connectionManager.CreateCommandOnly("UPDATE AttendantCheckList SET Stop = 1 WHERE RoomKey = '" + roomKey + "'", CommandType.Text, MultiTenancySide))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<GetMaidStatusOutput>> GetBusinessDate()
        {
            var result = new List<GetMaidStatusOutput>();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetSystemControlQuery(), CommandType.Text, MultiTenancySide))
            {
                using (var dr = await command.ExecuteReaderAsync())
                {

                    while (dr.Read())
                    {
                        GetMaidStatusOutput g = new GetMaidStatusOutput();
                        g.BusinessDate = Convert.ToDateTime(dr["SystemDate"]);
                        result.Add(g);
                    }
                    dr.Close();
                }
            }
            return result;
        }
        public async Task<List<GetDashRoomByMaidKeyOutput>> GetRoomByMaidKey(DateTime dtBusinessDate, string maidKey, string maidStatusKey, string roomStatusKey, string floor, string guestStatus)
        {
            var result = new List<GetDashRoomByMaidKeyOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            try
            {
                bool blnFilterByMaidStatusKey = false;
                if (!string.IsNullOrEmpty(maidStatusKey) && !maidStatusKey.Equals(Guid.Empty.ToString()))
                    blnFilterByMaidStatusKey = true;
                bool blnFilterByRoomStatusKey = false;
                if (!string.IsNullOrEmpty(roomStatusKey) && !roomStatusKey.Equals(Guid.Empty.ToString()))
                    blnFilterByRoomStatusKey = true;
                bool blnFilterByFloor = false;
                if (!string.IsNullOrEmpty(floor) && floor != "0")
                    blnFilterByFloor = true;
                bool blnFilterByGuestStatus = false;
                if (!string.IsNullOrEmpty(guestStatus) && guestStatus != "ALL")
                    blnFilterByGuestStatus = true;
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
                new SqlParameter("@MaidStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value = blnFilterByMaidStatusKey==true?Guid.Parse(maidStatusKey):DBNull.Value
                },
                new SqlParameter("@RoomStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value = blnFilterByRoomStatusKey==true?Guid.Parse(roomStatusKey):DBNull.Value
                },
                new SqlParameter("@Floor", SqlDbType.Int)
                {
                    Value = blnFilterByFloor==true?Convert.ToInt32(floor):DBNull.Value
                },
                new SqlParameter("@GuestStatus", SqlDbType.VarChar)
                {
                    Value = blnFilterByGuestStatus==true?guestStatus:DBNull.Value
                }
              };
                using (var command = _connectionManager.CreateCommandSP(GetRoomByMaidKeyQuery(blnFilterByMaidStatusKey, blnFilterByRoomStatusKey, blnFilterByFloor, blnFilterByGuestStatus), CommandType.Text, MultiTenancySide, parameters))
                {
                    using (var dr = await command.ExecuteReaderAsync())
                    {

                        while (dr.Read())
                        {


                            GetDashRoomByMaidKeyOutput g = new GetDashRoomByMaidKeyOutput();
                            g.Floor = !DBNull.Value.Equals(dr["Floor"]) ? Convert.ToInt32(dr["Floor"]) : 0;
                            g.RoomKey = (!DBNull.Value.Equals(dr["RoomKey"])) ? (!string.IsNullOrEmpty(dr["RoomKey"].ToString()) ? new Guid(dr["RoomKey"].ToString()) : Guid.Empty) : Guid.Empty;
                            g.Unit = !DBNull.Value.Equals(dr["Unit"]) ? dr["Unit"].ToString() : "";
                            g.GuestArrived = !DBNull.Value.Equals(dr["GuestArrived"]) ? dr["GuestArrived"].ToString() : "";
                            g.Status = !DBNull.Value.Equals(dr["Status"]) ? Convert.ToInt32(dr["Status"]) : 0;
                            g.PreCheckInCount = !DBNull.Value.Equals(dr["PreCheckInCount"]) ? Convert.ToInt32(dr["PreCheckInCount"]) : 0;
                            g.RoomType = !DBNull.Value.Equals(dr["RoomType"]) ? dr["RoomType"].ToString() : "";
                            g.LinenChange = !DBNull.Value.Equals(dr["LinenChange"]) ? dr["LinenChange"].ToString() : "";
                            g.InterconnectRoom = !DBNull.Value.Equals(dr["InterconnectRoom"]) ? dr["InterconnectRoom"].ToString() : "";
                            g.RoomStatus = !DBNull.Value.Equals(dr["RoomStatus"]) ? dr["RoomStatus"].ToString() : "";
                            g.MaidStatus = !DBNull.Value.Equals(dr["MaidStatus"]) ? dr["MaidStatus"].ToString() : "";
                            g.GuestStatus = !DBNull.Value.Equals(dr["GuestStatus"]) ? dr["GuestStatus"].ToString() : "";
                            g.ReservationKey = (!DBNull.Value.Equals(dr["ReservationKey"])) ? (!string.IsNullOrEmpty(dr["ReservationKey"].ToString()) ? new Guid(dr["ReservationKey"].ToString()) : Guid.Empty) : Guid.Empty;
                            g.HMMNotes = !DBNull.Value.Equals(dr["HMMNotes"]) ? dr["HMMNotes"].ToString() : "";
                            g.MaidStatus = !DBNull.Value.Equals(dr["MaidStatus"]) ? dr["MaidStatus"].ToString() : "";
                            g.DND = !DBNull.Value.Equals(dr["DND"]) ? Convert.ToInt32(dr["DND"]) : 0;

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

        public async Task<List<GetDashRoomByMaidStatusKeyOutput>> GetDashRoomByMaidStatusKey(DateTime dtBusinessDate, string maidStatusKey, string maidKey, string floorNo, string roomStatusKey)
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
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
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
                    Value =blnFilterByFloorNo==true?floorNo.Trim():DBNull.Value
                }
             };
                using (var command = _connectionManager.CreateCommandSP(GetRoomByMaidStatusKeyQuery(blnFilterByMaidKey, blnFilterByFloorNo, blnFilterByRoomStatusKey), CommandType.Text, MultiTenancySide, parameters))
                {
                    using (var dr = await command.ExecuteReaderAsync())
                    {

                        while (dr.Read())
                        {
                            GetDashRoomByMaidStatusKeyOutput g = new GetDashRoomByMaidStatusKeyOutput();
                            g.Floor = Convert.ToInt32(dr["Floor"]);
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

        public List<MaidOutput> GetAllAttendant()
        {

            var lst = new List<MaidOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetStaffMaidQuery(), CommandType.Text, MultiTenancySide))
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
                    MaidOutput o = new MaidOutput();
                    o.StaffKey = (!DBNull.Value.Equals(dr["StaffKey"])) ? (!string.IsNullOrEmpty(dr["StaffKey"].ToString()) ? new Guid(dr["StaffKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.UserName = !DBNull.Value.Equals(dr["UserName"]) ? dr["UserName"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }
        public List<DDLRoomOutput> GetAllRoom()
        {
            var lst = new List<DDLRoomOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetHotelRoomQuery(), CommandType.Text, MultiTenancySide))
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
                    DDLRoomOutput o = new DDLRoomOutput();
                    o.RoomKey = (!DBNull.Value.Equals(dr["RoomKey"])) ? (!string.IsNullOrEmpty(dr["RoomKey"].ToString()) ? new Guid(dr["RoomKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.Unit = !DBNull.Value.Equals(dr["Unit"]) ? dr["Unit"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }

        public List<TechnicianOutput> GetStaffAllTechnician()
        {
            var lst = new List<TechnicianOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetStaffTechnicianQuery(), CommandType.Text, MultiTenancySide))
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
                    TechnicianOutput o = new TechnicianOutput();
                    o.StaffKey = (!DBNull.Value.Equals(dr["StaffKey"])) ? (!string.IsNullOrEmpty(dr["StaffKey"].ToString()) ? new Guid(dr["StaffKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.UserName = !DBNull.Value.Equals(dr["UserName"]) ? dr["UserName"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }

        #endregion

        #endregion
        #region SQL Query

        private string GetPriorityQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT PriorityID,Priority ");
            sb.Append(" FROM MPriority where Active = 1");
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


        private static string GetStaffTechnicianQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  s.StaffKey, s.UserName , tech.Seqno   ");
            sb.Append(" FROM  Staff  s ");
            sb.Append(" RIGHT JOIN  MTechnician tech ON tech.TechnicianKey = s.TechnicianKey ");
            sb.Append(" WHERE    ");
            sb.Append("    s.TechnicianKey IS NOT NULL AND s.Active = 1  AND ");
            sb.Append("    tech.Active = 1 ");
            sb.Append(" ORDER BY  s.UserName ; ");
            return sb.ToString();
        }
        private static string GetStaffMaidQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  StaffKey, UserName FROM  Staff ");
            sb.Append(" WHERE   MaidKey IS NOT NULL AND Active = 1  ");
            sb.Append(" ORDER BY  UserName ; ");
            return sb.ToString();
        }
        private static string GetRoomByMaidKeyQuery(bool blnFilterByMaidStatusKey, bool blnFilterByRoomStatusKey, bool blnFilterByFloor, bool blnFilterByGuestStatus)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  ");
            sb.Append("   S.RoomKey, Unit,S.MaidStatusKey,M.MaidStatus,S.RoomStatusKey,R.RoomStatus,S.ReservationKey,RoomType,InterconnectRoom,Floor, LinenChange, DND,S.Adult,S.Child,G1.Group1,G2.Group2,G3.Group3,G4.Group4,S.ETD, ");
            sb.Append("   CleaningTime,LinenDays,Bed,S.CheckInDate,S.CheckInTime,S.CheckOutDate,S.CheckOutTime,I.Maid,HMMNotes,S.GuestArrived,S.Status,case when S.PreCheckInCount is null or S.PreCheckInCount = 0 then 0 else S.PreCheckInCount end As PreCheckInCount,GS.Status AS GuestStatus,dpl.Image,dpl.Document,dpl.CreatedDate ");//dp.DndphotoKey
            sb.Append("  ,(SELECT STRING_AGG(i.Code, ', ') FROM ReservationAdditional ra LEFT JOIN Item i ON ra.ItemKey = i.ItemKey WHERE ReservationKey = S.ReservationKey AND i.Hkg = 1 AND S.Status IN (1,2)) AS ItemCodes ");
            sb.Append(" FROM  dbo.UDF_DayRoomStatusiClean( (ISNULL((SELECT systemdate FROM control), GETDATE()))) S ");
            sb.Append(" LEFT JOIN   ");
            sb.Append("   (Select A.RoomKey,InterconnectRoom=B.Unit,");
            sb.Append("   Maid=M.Name,A.HMMNotes from Room A Left join Room B on A.InterconnectRoomKey=B.RoomKey Left Join Maid M on ");
            //sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey Left Join (Select dp.Image, dp.Document, dp.CreatedDate, dp.RoomKey from Dndphoto dp where dp.CreatedDate = (SELECT MAX([CreatedDate]) FROM [dbo].[Dndphoto])) dpl on  S.RoomKey=dpl.RoomKey ");
            //sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey Left Join  ( SELECT dp.RoomKey, dp.Image, dp.Document, dp.CreatedDate FROM Dndphoto dp JOIN (SELECT RoomKey, MAX(CreatedDate) AS MaxCreatedDate FROM Dndphoto GROUP BY RoomKey) latest ON dp.RoomKey = latest.RoomKey AND dp.CreatedDate = latest.MaxCreatedDate) dpl ON S.RoomKey = dpl.RoomKey ");
            sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey Left Join  ( SELECT dp.RoomKey, dp.Image, dp.Document, dp.CreatedDate FROM Dndphoto dp JOIN (SELECT RoomKey, MAX(CreatedDate) AS MaxCreatedDate FROM Dndphoto WHERE CAST(CreatedDate AS DATE) = CAST(ISNULL((SELECT TOP 1 SystemDate FROM Control WHERE SystemDate IS NOT NULL),GETDATE()) AS DATE)GROUP BY RoomKey) latest ON dp.RoomKey = latest.RoomKey AND dp.CreatedDate = latest.MaxCreatedDate) dpl ON S.RoomKey = dpl.RoomKey ");
            sb.Append(" LEFT JOIN  MaidStatus M on S.MaidStatusKey=M.MaidStatusKey  ");
            sb.Append(" LEFT JOIN  RoomStatus R on S.RoomStatusKey=R.RoomStatusKey ");
            sb.Append(" Left join Reservation V on S.ReservationKey = V.ReservationKey Left Join GuestStatus GS on V.GuestStatus = GS.StatusCode ");
            sb.Append(" LEFT JOIN Group1 G1 ON V.Group1Key = G1.Group1Key  LEFT JOIN Group2 G2 ON V.Group2Key = G2.Group2Key  LEFT JOIN Group3 G3 ON V.Group3Key = G3.Group3Key ");
            sb.Append(" LEFT JOIN Group4 G4 ON V.Group4Key = G4.Group4Key");
            sb.Append(" WHERE  ");
            sb.Append("    S.MaidKey = @MaidKey ");
            if (blnFilterByMaidStatusKey)
                sb.Append("  AND  S.MaidStatusKey = @MaidStatusKey ");
            if (blnFilterByRoomStatusKey)
                sb.Append("  AND  S.RoomStatusKey = @RoomStatusKey ");
            if (blnFilterByFloor)
                sb.Append(" AND Floor = @Floor ");
            if (blnFilterByGuestStatus)
                sb.Append(" AND GS.Status = @GuestStatus ");
            // sb.Append(" ORDER BY S.GuestArrived, Floor, Unit;");
            sb.Append(" ORDER BY CASE WHEN ISNULL(NULLIF(S.GuestArrived, ''), '') = '' THEN 1 ELSE 0 END, S.Floor,S.Unit ");

            return sb.ToString();
        }

        private static string GetRoomHistoryQuery(bool blnByStaff, bool blnByRoom, int pageSize)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  TOP " + pageSize + "  ChangedDate, Detail  ");
            sb.Append(" FROM    History ");
            sb.Append(" WHERE    ModuleName = 'iClean' AND TenantId=1 ");
            sb.Append("  AND  (TableName = 'Room' or TableName = 'RoomDND' or TableName = 'RoomOPT') ");
            if (blnByStaff)
                sb.Append("  AND  StaffKey = @StaffKey ");
            if (blnByRoom)
                sb.Append("  AND  SourceKey = @SourceKey ");
            sb.Append(" ORDER BY  Changeddate DESC;");
            return sb.ToString();
        }
        private static string GetServiceRefuseKeySQL(Guid itemKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select ItemKey from CheckList  ");
            sb.Append(" where ItemKey = '" + itemKey + "' and (ItemCode = 'SR' or Description = 'Service Refuse') and Active = 1 ");
            return sb.ToString();
        }
        private string GetHistoryQuery(bool blnByType, bool blnByStaff, bool blnByRoom, string logType, string staffKey, string roomKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  [HistoryKey], [ChangedDate], [Detail], [TableName] ");
            sb.Append(" FROM    History ");
            sb.Append(" WHERE    ModuleName = 'iClean'  AND TenantId=1 AND ChangedDate >= DATEADD(DAY, -7, GETDATE())  ");
            if (blnByType)
                sb.Append("  AND  TableName ='" + logType + "' ");
            if (blnByStaff)
                sb.Append("  AND  StaffKey = '" + staffKey + "' ");
            if (blnByRoom)
                sb.Append("  AND  SourceKey = '" + roomKey + "' ");
            sb.Append(" ORDER BY  Changeddate DESC;");
            return sb.ToString();
        }
        private static string GetHistoryQuery(bool blnByType, bool blnByStaff)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  [HistoryKey], [ChangedDate], [Detail], [TableName] ");
            sb.Append(" FROM    History ");
            sb.Append(" WHERE    ModuleName = 'iRepair'  AND TenantId=1 AND ChangedDate >= DATEADD(DAY, -7, GETDATE())  ");
            if (blnByType)
                sb.Append("  AND  TableName = @TableName ");
            if (blnByStaff)
                sb.Append("  AND  StaffKey = @StaffKey ");
            sb.Append(" ORDER BY  Changeddate DESC;");
            return sb.ToString();
        }

        #region iclean
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
        private static string InsertSupLinenItemQuery(Guid itemKey, Guid roomKey, DateTime docDate, int chk, Guid staffKey, int TenantId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO SupervisorCheckList ");
            sb.Append(" (ItemKey, DocDate, RoomKey, CreatedBy, Checked,TenantId) ");
            sb.Append(" VALUES ('" + itemKey + "','" + docDate.ToString("yyyy-MM-dd HH:mm:ss") + "','" + roomKey + "', '" + staffKey + "', " + chk + ", " + TenantId + ") ");
            return sb.ToString();
        }

        private static string UpdateSupLinenItemQuery(Guid id, Guid itemKey, Guid roomKey, DateTime docDate, int chk, Guid staffKey, int TenantId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE SupervisorCheckList ");
            sb.Append(" SET ItemKey = '" + itemKey + "', DocDate='" + docDate.ToString("yyyy-MM-dd HH:mm:ss") + "', RoomKey ='" + roomKey + "', ModifiedBy ='" + staffKey + "', Checked = " + chk + ", TenantId =" + TenantId + "");
            sb.Append(" WHERE InspectionChecklistKey = '" + id + "' ");
            return sb.ToString();
        }
        private static string InsertAttLinenItemQuery(Guid itemKey, Guid roomKey, DateTime docDate, int quantity, Guid staffKey, int TenantId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO AttendantCheckList ");
            sb.Append(" (ItemKey, DocDate, Quantity, RoomKey, CreatedBy,TenantId) ");
            sb.Append(" VALUES ('" + itemKey + "','" + docDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " + quantity + ",  '" + roomKey + "', '" + staffKey + "', " + TenantId + ") ");
            return sb.ToString();
        }

        private static string UpdateAttLinenItemQuery(Guid Id, Guid itemKey, Guid roomKey, DateTime docDate, int quantity, Guid staffKey, int TenantId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE AttendantCheckList ");
            sb.Append(" SET ItemKey = '" + itemKey + "', ModifiedDate = '" + docDate.ToString("yyyy-MM-dd HH:mm:ss") + "', Quantity =" + quantity + ", RoomKey ='" + roomKey + "', ModifiedBy = '" + staffKey + "', TenantId =" + TenantId + "");
            sb.Append(" WHERE LinenChecklistKey = '" + Id + "'; ");
            return sb.ToString();
        }

        #region systemdateformat issue
        private static string InsertSupLinenItemQueryS()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO SupervisorCheckList ");
            sb.Append(" (ItemKey, DocDate, RoomKey, CreatedBy, Checked,TenantId) ");
            sb.Append(" VALUES (@ItemKey, @DocDate, @RoomKey, @CreatedBy, @Checked,@TenantId) ");
            return sb.ToString();
        }

        private static string UpdateSupLinenItemQueryS()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE SupervisorCheckList ");
            sb.Append(" SET ItemKey = @ItemKey, DocDate= @DocDate, RoomKey = @RoomKey, ModifiedBy = @ModifiedBy, Checked = @Checked,TenantId=@TenantId ");
            sb.Append(" WHERE InspectionChecklistKey = @InspectionChecklistKey ");
            return sb.ToString();
        }
        private static string InsertAttLinenItemQueryS()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO AttendantCheckList ");
            sb.Append(" (ItemKey, DocDate, Quantity, RoomKey, CreatedBy,TenantId) ");
            sb.Append(" VALUES (@ItemKey, @DocDate, @Quantity,  @RoomKey, @CreatedBy,@TenantId) ");
            return sb.ToString();
        }

        private static string UpdateAttLinenItemQueryS()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE AttendantCheckList ");
            sb.Append(" SET ItemKey = @ItemKey, ModifiedDate = @DocDate, Quantity = @Quantity, RoomKey = @RoomKey, ModifiedBy = @ModifiedBy,TenantId=@TenantId ");
            sb.Append(" WHERE LinenChecklistKey = @LinenChecklistKey; ");
            return sb.ToString();
        }
        #endregion
        private static string GetItemByReservationKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(" select Code from ReservationAdditional ra   ");
            //sb.Append(" left join Item i on ra.ItemKey = i.ItemKey ");
            //sb.Append(" WHERE  ReservationKey = @ReservationKey and i.Hkg=1; ");
            sb.Append(" select Code from ReservationAdditional ra   ");
            sb.Append(" left join Item i on ra.ItemKey = i.ItemKey Left join Reservation R ON r.ReservationKey=ra.ReservationKey");
            sb.Append(" WHERE  ReservationKey = @ReservationKey and i.Hkg=1 And r.Status not in (10) ");
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
        //    sb.Append("    M.MaidStatusKey ='" + maidStatusKey + "'");
        //    if (blnFilterByMaidKey)
        //        sb.Append("  AND  S.MaidKey = '" + maidKey + "'  ");
        //    if (blnFilterByFloorNo)
        //        sb.Append("  AND  Floor = " + floorNo + "");
        //    if (blnFilterByRoomStatusKey)
        //        sb.Append("  AND  S.RoomStatusKey ='" + roomStatusKey + "'");
        //    sb.Append(" ORDER BY S.GuestArrived, S.PreCheckInCount desc, Floor, S.Unit;  ");

        //    return sb.ToString();
        //}

        private static string GetRoomByMaidStatusKeyQuery(bool blnFilterByMaidKey, bool blnFilterByFloorNo, bool blnFilterByRoomStatusKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  ");
            sb.Append("   S.RoomKey, Unit,S.MaidStatusKey,M.MaidStatus,S.RoomStatusKey,R.RoomStatus,RoomType,InterconnectRoom,Floor, LinenChange, DND,  ");
            sb.Append("   CleaningTime,LinenDays,Bed,CheckInDate,CheckInTime,CheckOutDate,CheckOutTime,I.Maid,HMMNotes,GuestArrived,S.Status,PreCheckInCount  ");
            sb.Append(" FROM  ");
            sb.Append("   dbo.UDF_DayRoomStatusiClean( @date ) S ");
            sb.Append(" LEFT JOIN   ");
            sb.Append("   (Select A.RoomKey,InterconnectRoom=B.Unit,");
            sb.Append("   Maid=M.Name,A.HMMNotes from Room A Left join Room B on A.InterconnectRoomKey=B.RoomKey Left Join Maid M on ");
            sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey ");
            sb.Append(" LEFT JOIN   ");
            sb.Append("   MaidStatus M on S.MaidStatusKey=M.MaidStatusKey  ");
            sb.Append(" LEFT JOIN   ");
            sb.Append("   RoomStatus R on S.RoomStatusKey=R.RoomStatusKey ");
            sb.Append(" WHERE  ");
            sb.Append("    M.MaidStatusKey = @MaidStatusKey ");
            if (blnFilterByMaidKey)
                sb.Append("  AND  S.MaidKey = @MaidKey  ");
            if (blnFilterByFloorNo)
                sb.Append("  AND  Floor = @FloorNo ");
            if (blnFilterByRoomStatusKey)
                sb.Append("  AND  S.RoomStatusKey = @RoomStatusKey ");
            // sb.Append(" ORDER BY S.GuestArrived, S.PreCheckInCount desc, Floor, S.Unit;  ");
            sb.Append(" ORDER BY CASE WHEN ISNULL(NULLIF(S.GuestArrived, ''), '') = '' THEN 1 ELSE 0 END, S.PreCheckInCount, S.Floor,S.Unit ");

            return sb.ToString();
        }
        private static string GetAllMaidStatusQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT * ");
            sb.Append(" FROM  MaidStatus ");
            sb.Append(" ORDER BY  Seq  DESC;");
            return sb.ToString();
        }
        private static string GetMaidStatusByStatusQuery(string MaidStatus)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT * ");
            sb.Append(" FROM  MaidStatus ");
            sb.Append(" WHERE MaidStatus = '" + MaidStatus + "' ;");
            return sb.ToString();
        }
        private static string UpdateMaidStatusByRoomKeyQuery(bool blnUpdateNote, Guid roomKey, Guid maidStatusKey, string hMMNotes)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE   Room ");
            sb.Append(" SET    MaidStatusKey = '" + maidStatusKey + "' ");
            if (blnUpdateNote)
                sb.Append(" , HMMNotes = '" + hMMNotes + "' ");
            sb.Append(" WHERE  RoomKey = '" + roomKey + "' ;");
            return sb.ToString();
        }

        public static string InsertHistoryQuery(History h)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert into history ");
            sb.Append(" (HistoryKey,StaffKey, SourceKey,ModuleName,Operation,ChangedDate,Detail,TableName,TenantId)  ");
            sb.Append(" Values ");
            sb.Append(" ('" + h.Id + "','" + h.StaffKey + "', '" + h.SourceKey + "','iClean','" + h.Operation + "' , GETDATE() ,'" + h.Detail + "' ,'" + h.TableName + "'," + h.TenantId + ") ");
            return sb.ToString();
        }
        private static string GetUpdateRoomHistoryLinkKeyQuery(Guid historyKey, Guid roomKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE  History  ");
            sb.Append(" SET     LinkKey = '" + historyKey + "'  ");
            sb.Append(" WHERE   HistoryKey IN  ");
            sb.Append(" (SELECT  TOP 1 HistoryKey FROM History  ");
            sb.Append(" WHERE ");
            sb.Append(" ModuleName = 'iClean' AND TableName = 'Room' AND TenantId=1 ");
            sb.Append(" AND SourceKey ='" + roomKey + "' AND Detail LIKE '%STARTS%'    ");
            sb.Append(" AND LinkKey IS NULL    ");
            sb.Append(" ORDER BY ChangedDate DESC);   ");
            return sb.ToString();
        }
        //private static string GetLinenItemByKeyQuery(Guid roomkey, DateTime date)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(" SELECT att.LinenChecklistKey, att.ItemKey, chk.ItemCode, chk.Description, att.Quantity FROM AttendantCheckList att JOIN CheckList chk ");
        //    sb.Append(" ON chk.ItemKey = att.ItemKey WHERE RoomKey = '" + roomkey + "' AND chk.Active = 1 AND CONVERT(date, DocDate) = CONVERT(date, '" + date + "') AND Stop = 0 ORDER BY chk.Sort, chk.ItemCode ");
        //    return sb.ToString();
        //}
        private static string GetLinenItemByKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT att.LinenChecklistKey, att.ItemKey, chk.ItemCode, chk.Description, att.Quantity FROM AttendantCheckList att JOIN CheckList chk ");
            sb.Append(" ON chk.ItemKey = att.ItemKey WHERE RoomKey = @RoomKey AND chk.Active = 1 AND CONVERT(date, DocDate) = CONVERT(date, @date) AND Stop = 0 ORDER BY chk.Sort, chk.ItemCode ");
            return sb.ToString();
        }
        private static string GetSystemControlQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT * FROM Control ;    ");
            return sb.ToString();
        }
        //private static string GetRoomByMaidKeyQuery(bool blnFilterByMaidStatusKey, bool blnFilterByRoomStatusKey, bool blnFilterByFloor, bool blnFilterByGuestStatus, DateTime dtBusinessDate, string maidKey, string maidStatusKey, string roomStatusKey, string floor, string guestStatus)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(" SELECT  ");
        //    sb.Append("   S.RoomKey, Unit,S.MaidStatusKey,M.MaidStatus,S.RoomStatusKey,R.RoomStatus,S.ReservationKey,RoomType,InterconnectRoom,Floor, LinenChange, DND, ");
        //    sb.Append("   CleaningTime,LinenDays,Bed,S.CheckInDate,S.CheckInTime,S.CheckOutDate,S.CheckOutTime,I.Maid,HMMNotes,S.GuestArrived,S.Status,S.PreCheckInCount,GS.Status AS GuestStatus  ");
        //    sb.Append(" FROM  dbo.UDF_DayRoomStatusiClean( '" + dtBusinessDate + "' ) S ");
        //    sb.Append(" LEFT JOIN   ");
        //    sb.Append("   (Select A.RoomKey,InterconnectRoom=B.Unit,");
        //    sb.Append("   Maid=M.Name,A.HMMNotes from Room A Left join Room B on A.InterconnectRoomKey=B.RoomKey Left Join Maid M on ");
        //    sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey ");
        //    sb.Append(" LEFT JOIN  MaidStatus M on S.MaidStatusKey=M.MaidStatusKey  ");
        //    sb.Append(" LEFT JOIN  RoomStatus R on S.RoomStatusKey=R.RoomStatusKey ");
        //    sb.Append(" Left join Reservation V on S.ReservationKey = V.ReservationKey Left Join GuestStatus GS on V.GuestStatus = GS.StatusCode ");
        //    sb.Append(" WHERE  ");
        //    sb.Append("    S.MaidKey = '" + maidKey + "'  ");
        //    if (blnFilterByMaidStatusKey)
        //        sb.Append("  AND  S.MaidStatusKey ='" + maidStatusKey + "'");
        //    if (blnFilterByRoomStatusKey)
        //        sb.Append("  AND  S.RoomStatusKey = '" + roomStatusKey + "'");
        //    if (blnFilterByFloor)
        //        sb.Append(" AND Floor = " + floor + "");
        //    if (blnFilterByGuestStatus)
        //        sb.Append(" AND GS.Status ='" + guestStatus + "'");
        //    sb.Append(" ORDER BY S.GuestArrived, S.PreCheckInCount desc, Floor, Unit ;  ");

        //    return sb.ToString();
        //}

        #endregion
        #endregion
    }
}
