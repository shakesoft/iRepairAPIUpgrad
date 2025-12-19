using Abp.Data;
using Abp.EntityFrameworkCore;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.EntityFrameworkCore;
using BEZNgCore.EntityFrameworkCore.Repositories;
using BEZNgCore.IrepairModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Data.SqlClient;
using BEZNgCore.IRepairIAppService.Dto;
using System.Threading.Tasks;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data.SqlTypes;

namespace BEZNgCore.Authorization.IrepairDal
{
    public class MworkorderdalRepository : BEZNgCoreRepositoryBase<RoomStatus, Guid>, IMworkorderdalRepository
    {
        private readonly IActiveTransactionProvider _transactionProvider;
        ConnectionManager _connectionManager;
        public MworkorderdalRepository(IDbContextProvider<BEZNgCoreDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
            _connectionManager = new ConnectionManager(dbContextProvider, _transactionProvider);
        }


        #region interfaceimplement & related function 
        public DataTable GetWOByTechnician(int technicianID, int woStatus, string roomStatus)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            bool blnFilterByWOStatus = false;
            bool blnByRoomStatus = false;
            if (woStatus >= 0)
                blnFilterByWOStatus = true;
            if (!string.IsNullOrEmpty(roomStatus) && roomStatus != "ALL")
                blnByRoomStatus = true;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MTechnician", SqlDbType.Int)
                {
                    Value = technicianID
                },
                new SqlParameter("@MWorkOrderStatus", SqlDbType.Int)
                {
                    Value =blnFilterByWOStatus==true?woStatus:DBNull.Value
                },
                new SqlParameter("@RoomStatus", SqlDbType.VarChar)
                {
                    Value = blnByRoomStatus==true?roomStatus:DBNull.Value
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetWOByTechnicianQuery(blnFilterByWOStatus, blnByRoomStatus), CommandType.Text, MultiTenancySide, parameters))
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
        public DataTable GetUnassignedTechWorkOrderCount()
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetUnassignedTechWorkOrderCountQuery(), CommandType.Text, MultiTenancySide))
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
                    Value =blnFilterByFloor==true?Convert.ToInt32(floor):DBNull.Value
                },
                 new SqlParameter("@GuestStatus", SqlDbType.VarChar)
                {
                    Value =blnFilterByGuestStatus==true?guestStatus:DBNull.Value
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
        public List<HistoryDto> GetTodayHistory(Guid staffkey)
        {

            var lst = new List<HistoryDto>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@StaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value = staffkey
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetTodayHistoryQuery(), CommandType.Text, MultiTenancySide, parameters))
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
                    HistoryDto g = new HistoryDto();
                    g.HistoryDes = (Convert.ToDateTime(dr["ChangedDate"])).ToString("dd/MM/yyyy HH:mm:ss") + " =>" + dr["Detail"];
                    lst.Add(g);
                }
            }
            return lst;

        }
        public List<WOStatusOutput> GetWorkOrderStatusCountByTechnician(int technicalID)
        {
            var lst = new List<WOStatusOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@MTechnician", SqlDbType.Int)
                {
                    Value = technicalID
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetWorkOrderStatusCountByTechnicianQuery(), CommandType.Text, MultiTenancySide, parameters))
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
                    WOStatusOutput o = new WOStatusOutput();
                    o.strWOStatus = dr["WorkOrderStatus"].ToString();
                    o.strWOStatusDesc = dr["WODescription"].ToString() + ": " + string.Format("{0:n0}", Convert.ToInt32(dr["TaskCount"].ToString()));
                    lst.Add(o);
                }
            }
            return lst;
        }
        public List<GetMyTaskDataOutput> GetWorkOrderByTechnician(int technicianID, int woStatus, string roomStatus, int priority)
        {
            var lst = new List<GetMyTaskDataOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            bool blnFilterByWOStatus = false;
            bool blnByRoomStatus = false;
            bool blnByPriority = false;
            if (woStatus >= 0)
                blnFilterByWOStatus = true;
            if (priority > 0)
                blnByPriority = true;
            if (!string.IsNullOrEmpty(roomStatus) && roomStatus != "ALL")
                blnByRoomStatus = true;
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@MTechnician", SqlDbType.Int)
                {
                    Value = technicianID
                },
                new SqlParameter("@MWorkOrderStatus", SqlDbType.Int)
                {
                    Value =blnFilterByWOStatus==true? woStatus:DBNull.Value
                },
                new SqlParameter("@RoomStatus", SqlDbType.VarChar)
                {
                    Value =blnByRoomStatus==true? roomStatus:DBNull.Value
                },
                new SqlParameter("@Priority", SqlDbType.Int)
                {
                    Value = blnByPriority==true?priority:DBNull.Value
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetWorkOrderByTechnicianQuery(blnFilterByWOStatus, blnByRoomStatus, blnByPriority), CommandType.Text, MultiTenancySide, parameters))
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
                    GetMyTaskDataOutput o = new GetMyTaskDataOutput();
                    string strWO = dr["WorkOrderStatus"].ToString();
                    string strPrio = !DBNull.Value.Equals(dr["PriorityDes"]) ? dr["PriorityDes"].ToString() : ""; 
                        //dr["Priority"].ToString() == "1" ? "Low" : dr["Priority"].ToString() == "2" ? "Medium" : dr["Priority"].ToString() == "3" ? "High" : "";
                    int seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    string Note = !DBNull.Value.Equals(dr["Notes"]) ? dr["Notes"].ToString() : "";
                    string RoomStatus = !DBNull.Value.Equals(dr["RoomStatus"]) ? dr["RoomStatus"].ToString() : "";
                    Guid RoomKey = (!DBNull.Value.Equals(dr["RoomKey"])) ? (!string.IsNullOrEmpty(dr["RoomKey"].ToString()) ? new Guid(dr["RoomKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    // StringBuilder sb = new StringBuilder();
                    if (strWO == "In Progress")
                    {
                        // sb.Append("<span style='color: #d9534f;'><b>" + strWO + "</b></span> &nbsp; " + GetColorPriority(strPrio) + "<br/>");
                        o.WorkOrderStatus = strWO;//sb.ToString();
                        o.WorkOrderStatusColor = "#d9534f";
                        o.Priority = strPrio;
                        o.PriorityColor = GetColorPriority(strPrio);
                    }
                    else if (strWO == "Completed")
                    {
                        // sb.Append("<span style='color: #5cb85c;'><b>" + strWO + "</b></span> &nbsp; " + GetColorPriority(strPrio));
                        o.WorkOrderStatus = strWO;//sb.ToString();
                        o.WorkOrderStatusColor = "#5cb85c";
                        o.Priority = strPrio;
                        o.PriorityColor = GetColorPriority(strPrio);
                    }
                    else if (strWO == "Initial Entry")
                    {
                        // sb.Append("<span style='color: #5bc0de;'><b>" + strWO + "</b></span> &nbsp; " + GetColorPriority(strPrio) + "<br/>");
                        o.WorkOrderStatus = strWO;//sb.ToString();
                        o.WorkOrderStatusColor = "#5bc0de";
                        o.Priority = strPrio;
                        o.PriorityColor = GetColorPriority(strPrio);
                    }
                    else
                    {
                        o.WorkOrderStatus = strWO;//sb.ToString();
                                                  // o.WorkOrderStatusColor = "#5bc0de";
                        o.Priority = strPrio;
                        o.PriorityColor = GetColorPriority(strPrio);
                    }
                    o.MWorkOrderKey = (!DBNull.Value.Equals(dr["MWorkOrderKey"])) ? (!string.IsNullOrEmpty(dr["MWorkOrderKey"].ToString()) ? new Guid(dr["MWorkOrderKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.Id = seqno;// !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Description = !DBNull.Value.Equals(dr["Description"]) ? dr["Description"].ToString() : "";
                    // o.WorkOrderStatus= !DBNull.Value.Equals(dr["WorkOrderStatus"]) ? dr["WorkOrderStatus"].ToString() : "";
                    o.WOStatusButton = GetUpdateWOStatusButton(seqno, strWO);
                    o.BlockRoomButton = GetBlockRoomButton(seqno, strWO);
                    o.TStartOrEndTaskButton = GetTStartOrEndTaskButton(seqno, strWO);
                    o.DescriptionByLength = GetDescriptionByLength(Note, 30);
                    o.Room = (!DBNull.Value.Equals(dr["Room"])) ? (!string.IsNullOrEmpty(dr["Room"].ToString()) ? dr["Room"].ToString() : "-") : "-";
                    o.RoomStatus = RoomStatus;
                    o.ColorRoomStatus = GetColorRoomStatus(RoomStatus);
                    o.RoomKey = RoomKey;
                    o.WOMaidStatus = GetWOMaidStatus(RoomKey);
                    o.Area = (!DBNull.Value.Equals(dr["Area"])) ? (!string.IsNullOrEmpty(dr["Area"].ToString()) ? dr["Area"].ToString() : "-") : "-";
                    o.WorkType = (!DBNull.Value.Equals(dr["WorkType"])) ? (!string.IsNullOrEmpty(dr["WorkType"].ToString()) ? dr["WorkType"].ToString() : "-") : "-";
                    o.StaffName = (!DBNull.Value.Equals(dr["StaffName"])) ? (!string.IsNullOrEmpty(dr["StaffName"].ToString()) ? dr["StaffName"].ToString() : "-") : "-";
                    
                    if (dr["ReportedOn"] == DBNull.Value)
                    {
                        
                         o.DateToDisplay = "";
                    }
                    else
                    {
                        o.ReportedOn = (DateTime)(Convert.IsDBNull(dr["ReportedOn"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["ReportedOn"])).Value);
                        o.DateToDisplay = GetDateToDisplay(o.ReportedOn);
                    }
                    lst.Add(o);
                }
            }
            return lst;
        }
        public static string GetColorPriority(object roomStatus)
        {
            string strReturnValue = roomStatus.ToString();
            try
            {
                if (strReturnValue == "Low")
                {
                    strReturnValue = "#5cb85c"; //"<b><span style=\"color: #5cb85c \">" + roomStatus.ToString() + "</span></b>";
                }
                else if (strReturnValue == "Medium")
                {
                    strReturnValue = "#5bc0de"; //"<b><span style=\"color: #5bc0de \">" + roomStatus.ToString() + "</span></b>";
                }
                else if (strReturnValue == "High")
                {
                    strReturnValue = "#d9534f";// "<b><span style=\"color: #d9534f \">" + roomStatus.ToString() + "</span></b>";
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
        public static string GetUpdateWOStatusButton(object seqno, object WorkOrderStatus)
        {
            string status = WorkOrderStatus.ToString();
            string strReturnValue = "";
            try
            {
                if (!status.Contains("Completed"))
                {
                    strReturnValue = "OnUpdateWorkOrderStatus";//"<a class=\"btn btn-primary btn-outline btn-lg\" href='javascript:OnUpdateWorkOrderStatus(" + seqno.ToString() + ");' title=\"Click to update Work Order Status\" > <i class=\"fa fa-pencil-square fa-lg\"></i></a>";
                }
                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }
        public static string GetBlockRoomButton(object seqno, object WorkOrderStatus)
        {
            string status = WorkOrderStatus.ToString();
            string strReturnValue = "";
            try
            {
                if (!status.Contains("Completed"))
                {
                    strReturnValue = "OnAddBlockRoom";//"<a class=\"btn btn-primary btn-outline btn-lg\" href='javascript:OnAddBlockRoom(" + seqno.ToString() + ");' title=\"Click to add Block Room\" > <i class=\"fa fa-minus-square-o\"></i></a>";
                }
                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }
        public string GetTStartOrEndTaskButton(object seqno, object WorkOrderStatus)
        {
            string status = WorkOrderStatus.ToString();
            string strReturnValue = "";
            bool blnHasStartedTask = MaidHasStartedTask(seqno.ToString());
            try
            {
                if (!status.Contains("Completed"))
                {
                    if (blnHasStartedTask.ToString().ToLower().Equals("false"))
                        strReturnValue = "START";// "<a  id=\"btnStart\" runat=\"server\"   class=\"btn btn-success btn-outline btn-lg\"  href=\"javascript:OnStartTimeSheet(" + seqno.ToString() + ");\" > <span class=\"glyphicon glyphicon-play\" ></span> START </a>";
                    else
                        strReturnValue = "END";// "<a  id=\"btnEnd\" runat=\"server\"   class=\"btn btn-danger btn-outline btn-lg\"  href=\"javascript:OnEndTimeSheet(" + seqno.ToString() + ");\" > <span class=\"glyphicon glyphicon-stop\" ></span> END </a>";
                }

                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }
        public bool MaidHasStartedTask(string woID)
        {
            bool blnHasStartedTask = false;
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);


            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value =  Convert.ToInt32(woID)
                 }
            };
            using (var command = _connectionManager.CreateCommandSP(GetRoomCountByMaidKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {

                    dt = ds.Tables[0];
                    //DataTable dtRoom = GetRoomCountByMaidKey(woID);


                }
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow room in dt.Rows)
                {
                    if (room["TimeTo"].ToString() != null && room["TimeTo"].ToString() == "")
                        blnHasStartedTask = true;
                }
            }
            return blnHasStartedTask;
        }
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

        public static string GetColorRoomStatus(object roomStatus)
        {
            string strReturnValue = roomStatus.ToString();
            try
            {
                if (strReturnValue == "Vacant")
                {
                    strReturnValue = "#5cb85c";//"<b><span style=\"color: #5cb85c \">" + roomStatus.ToString() + "</span></b>";
                }
                else if (strReturnValue == "Out of Order" || strReturnValue == "Hold" || strReturnValue == "Out Of Service")
                {
                    strReturnValue = "#5bc0de"; //"<b><span style=\"color: #5bc0de \">" + roomStatus.ToString() + "</span></b>";
                }
                else if (strReturnValue == "Occupied")
                {
                    strReturnValue = "#FFB3B3"; //"<b><span style=\"color: #FFB3B3 \">" + roomStatus.ToString() + "</span></b>";
                }
                else if (strReturnValue == "Due Out")
                {
                    strReturnValue = "#d9534f";// "<b><span style=\"color: #d9534f \">" + roomStatus.ToString() + "</span></b>";
                }
                else
                {
                    strReturnValue = "-";
                }
                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }

        public string GetWOMaidStatus(object roomkey)
        {
            string strReturnValue = "-";
            if (!string.IsNullOrEmpty(roomkey.ToString()))
            {
                var key = Guid.Parse(roomkey.ToString());
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);


                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value =  key
                 }
                };
                using (var command = _connectionManager.CreateCommandSP(GetMaidStatusByRoomKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
                {
                    object obj = command.ExecuteScalar();
                    if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                    {
                        strReturnValue = obj.ToString();
                    }

                }
                return strReturnValue;

            }
            else
            {
                return "-";
            }
        }

        public static string GetDateToDisplay(object inputDate)
        {
            try
            {
                string strReturnValue = "";
                if (inputDate != null && inputDate.ToString() != "")
                {
                    strReturnValue = Convert.ToDateTime(inputDate).ToString("dd/MM/yyyy");
                }
                else
                {
                    strReturnValue = " -";
                }
                return strReturnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetWIPWorkOrder()
        {
            DataTable dt = new DataTable();
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                using (var command = _connectionManager.CreateCommandOnly(GetWIPWorkOrderQuery(), CommandType.Text, MultiTenancySide))
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetWorkOrderByID(int mworkorderno)
        {
            DataTable dt = new DataTable();
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = mworkorderno
                }
           };
                using (var command = _connectionManager.CreateCommandSP(GetWorkOrderByIDQuery(), CommandType.Text, MultiTenancySide, parameters))
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsRoomBlockExist(BlockRoom room)
        {
            bool blnExist = true;
            int intCount = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MWorkOrderno", SqlDbType.Int)
                {
                    Value = room.Mworkorderno
                },
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value=room.Roomkey
                },
                new SqlParameter("@FromBlockDate", SqlDbType.Date)
                {
                    Value=room.BlockFromDate
                },
                 new SqlParameter("@ToBlockDate", SqlDbType.Date)
                {
                    Value=room.BlockToDate
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetBlockRoomCountByDateRangeQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                    intCount = Convert.ToInt32(obj.ToString());
            }
            if (intCount == 0)
                blnExist = false;
            return blnExist;
        }

        public DataTable GetReservationByRoomKeyDateRange(Guid roomKey, DateTime fromDate)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value = roomKey
                },
                new SqlParameter("@fromDate", SqlDbType.DateTime)
                {
                    Value = fromDate
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetReservationByRoomKeyDateRange(), CommandType.Text, MultiTenancySide, parameters))
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

        public int Insert(List<BlockRoom> listRoom)
        {
            int intSuccessful = 0;
            foreach (BlockRoom room in listRoom)
            {
                intSuccessful = Insert(room);
            }

            return intSuccessful;
        }
        public int Insert(BlockRoom room)
        {
            int intRowAffected = 0;
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                SqlParameter[] parameters = new SqlParameter[]
          {
                new SqlParameter("@MWorkOrderNo", SqlDbType.Int)
                {
                    Value = room.Mworkorderno
                },
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value = room.Roomkey
                },
                new SqlParameter("@Active", SqlDbType.Int)
                {
                    Value =  room.Active
                },
                new SqlParameter("@BlockDate", SqlDbType.DateTime)
                {
                    Value = room.Blockdate
                },
                new SqlParameter("@Reason", SqlDbType.VarChar)
                {
                    Value = room.Reason
                },
                new SqlParameter("@Comment", SqlDbType.VarChar)
                {
                    Value = room.Comment
                },
                new SqlParameter("@BlockStaff", SqlDbType.VarChar)
                {
                    Value = (room.Blockstaff == null ? DBNull.Value : room.Blockstaff)//room.Blockstaff
                },
                new SqlParameter("@UnblockStaff", SqlDbType.VarChar)
                {
                    Value = (room.Unblockstaff == null ? DBNull.Value : room.Unblockstaff)//room.Unblockstaff
                },
                new SqlParameter("@BlockTime", SqlDbType.DateTime)
                {
                    Value = (room.Blocktime == null ? DBNull.Value : room.Blocktime)//room.Blocktime
                },
                new SqlParameter("@UnblockTime", SqlDbType.DateTime)
                {
                    Value = (room.Unblocktime == null ? DBNull.Value : room.Unblocktime)//room.Unblocktime
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = room.TenantId
            }
          };
                using (var command = _connectionManager.CreateCommandSP(GetInsertQuery(), CommandType.Text, MultiTenancySide, parameters))
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

        public int InsertHistory(History history)
        {
            int intRowAffected = 0;
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);

                SqlParameter[] parameters = new SqlParameter[]
               {
                new SqlParameter("@HistoryKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.NewGuid()
                },
                new SqlParameter("@StaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value = history.StaffKey
            },
                new SqlParameter("@SourceKey", SqlDbType.UniqueIdentifier)
                {
                    Value =  (history.SourceKey == null ? DBNull.Value : history.SourceKey)
                },
                new SqlParameter("@ModuleName", SqlDbType.VarChar)
                {
                    Value = history.ModuleName
            },new SqlParameter("@Operation", SqlDbType.Char)
                {
                    Value =  history.Operation
                },
                new SqlParameter("@TableName", SqlDbType.VarChar)
                {
                    Value =history.TableName
            },new SqlParameter("@Detail", SqlDbType.VarChar)
                {
                    Value = (history.Detail.Trim().Length > 200 ? history.Detail.Trim().Substring(0, 190) + "..." : history.Detail.Trim())
                },
                new SqlParameter("@NewValue", SqlDbType.VarChar)
                {
                    Value = (history.NewValue == null ? DBNull.Value : history.NewValue)
            },
                new SqlParameter("@OldValue", SqlDbType.VarChar)
                {
                    Value = (history.OldValue == null ? DBNull.Value : history.OldValue)
            },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = history.TenantId
            }
               };
                using (var command = _connectionManager.CreateCommandSP(InsertHistory(), CommandType.Text, MultiTenancySide, parameters))
                {

                    intRowAffected = command.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intRowAffected;
        }

        public List<GetViewBlockRoomOutput> GetBlockRoomWorkOrderBy(int? technicianID, string roomKey, DateTime? fromDate, DateTime? toDate)
        {
            var lst = new List<GetViewBlockRoomOutput>();
            DataTable dt = new DataTable();
            try
            {
                bool blnByTechnician = false;
                bool blnByRoom = false;
                bool blnByStartDate = false;
                bool blnByEndDate = false;
                if (technicianID != -1)//0 && technicianID != null)
                    blnByTechnician = true;
                if (!string.IsNullOrEmpty(roomKey) && !roomKey.Equals(Guid.Empty.ToString()))
                    blnByRoom = true;
                if (fromDate != null)
                    blnByStartDate = true;
                if (toDate != null)
                    blnByEndDate = true;
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                SqlParameter[] parameters = new SqlParameter[]
         {
                new SqlParameter("@MTechnician", SqlDbType.Int)
                {
                    Value = technicianID
                },
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(roomKey)
                },
                new SqlParameter("@FromDate", SqlDbType.Date)
                {
                    Value = fromDate
                },
                new SqlParameter("@ToDate", SqlDbType.Date)
                {
                    Value = toDate
                }
         };
                using (var command = _connectionManager.CreateCommandSP(GetBlockRoomWorkOrderByQuery(blnByTechnician, blnByRoom, blnByStartDate, blnByEndDate), CommandType.Text, MultiTenancySide, parameters))
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
                        GetViewBlockRoomOutput o = new GetViewBlockRoomOutput();
                        o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                        o.BlockDate = GetDateToDisplay((DateTime)(Convert.IsDBNull(dr["BlockDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["BlockDate"])).Value));
                        o.Room = (!DBNull.Value.Equals(dr["Unit"])) ? (!string.IsNullOrEmpty(dr["Unit"].ToString()) ? dr["Unit"].ToString() : "") : "";
                        o.WorkOrder = (!DBNull.Value.Equals(dr["Description"])) ? (!string.IsNullOrEmpty(dr["Description"].ToString()) ? dr["Description"].ToString() : "") : "";
                        o.Blockroomstatus= (!DBNull.Value.Equals(dr["RoomStatus"])) ? (!string.IsNullOrEmpty(dr["RoomStatus"].ToString()) ? dr["RoomStatus"].ToString() : "") : "";
                        lst.Add(o);
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetAllWoOutput> GetIncompletedWorkOrderStatus(int tech, int worktype, int workstatus, int area, string unit, Guid staff,int mprority)
        {

            var lst = new List<GetAllWoOutput>();
            DataTable dt = new DataTable();
            try
            {
                bool blnByTechnician = false;
                bool blnByArea = false;
                bool blnByWorkType = false;
                bool blnWorkStatus = false;
                bool blnByRoom = false;
                bool blnByStaff = false;
                bool blnByMpriority = false;
                if (workstatus != 999)// && workstatus != null)
                    blnWorkStatus = true;
                if (tech != -1)// && tech != null)
                    blnByTechnician = true;
                if (area != 99)// && area != null)
                    blnByArea = true;
                if (worktype != 999)// && worktype != null)
                    blnByWorkType = true;
                if (unit != "ALL")
                    blnByRoom = true;
                if (staff != Guid.Empty)
                    blnByStaff = true;
                if (mprority != -1)
                    blnByMpriority = true;
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                SqlParameter[] parameters = new SqlParameter[]
         {
                new SqlParameter("@MWorkStatus", SqlDbType.Int)
                {
                    Value =blnWorkStatus==true? workstatus:DBNull.Value
                },
                new SqlParameter("@MArea", SqlDbType.Int)
                {
                    Value =blnByArea==true?area:DBNull.Value
                },
                new SqlParameter("@MTechnician", SqlDbType.Int)
                {
                    Value =blnByTechnician==true? tech:DBNull.Value
                },
                new SqlParameter("@MWorkType", SqlDbType.Int)
                {
                    Value =blnByWorkType==true?worktype:DBNull.Value
                },
                new SqlParameter("@Room", SqlDbType.VarChar)
                {
                    Value =blnByRoom==true?unit:DBNull.Value
                },
                new SqlParameter("@ReportedBy", SqlDbType.UniqueIdentifier)
                {
                    Value =blnByStaff==true?staff:DBNull.Value
                }
                ,
                new SqlParameter("@Priority", SqlDbType.Int)
                {
                    Value =blnByMpriority==true? mprority:DBNull.Value
                },
         };
                using (var command = _connectionManager.CreateCommandSP(GetIncompletedWorkOrderStatusQuery(blnWorkStatus, blnByArea, blnByTechnician, blnByWorkType, blnByRoom, blnByStaff, blnByMpriority), CommandType.Text, MultiTenancySide, parameters))
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
                        GetAllWoOutput o = new GetAllWoOutput();
                        o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                        if (dr["Room"] == DBNull.Value)
                        {
                            o.Room = "";
                        }
                        else
                        {
                            o.Room = (!DBNull.Value.Equals(dr["Room"])) ? (!string.IsNullOrEmpty(dr["Room"].ToString()) ? dr["Room"].ToString() : "") : "";
                        }
                        if (dr["MAreaDesc"] == DBNull.Value)
                        {
                            o.MAreaDesc = "";
                        }
                        else
                        {
                            o.MAreaDesc = (!DBNull.Value.Equals(dr["MAreaDesc"])) ? (!string.IsNullOrEmpty(dr["MAreaDesc"].ToString()) ? dr["MAreaDesc"].ToString() : "") : "";
                        }
                        if (dr["MWorkTypeDesc"] == DBNull.Value)
                        {
                            o.MWorkTypeDesc = "";
                        }
                        else
                        {
                            o.MWorkTypeDesc = (!DBNull.Value.Equals(dr["MWorkTypeDesc"])) ? (!string.IsNullOrEmpty(dr["MWorkTypeDesc"].ToString()) ? dr["MWorkTypeDesc"].ToString() : "") : "";
                        }
                        if (dr["MTechnicianName"] == DBNull.Value)
                        {
                            o.MTechnicianName = "";
                        }
                        else
                        {
                            o.MTechnicianName = (!DBNull.Value.Equals(dr["MTechnicianName"])) ? (!string.IsNullOrEmpty(dr["MTechnicianName"].ToString()) ? dr["MTechnicianName"].ToString() : "") : "";
                        }
                        o.MWorkOrderStatusDesc = (!DBNull.Value.Equals(dr["MWorkOrderStatusDesc"])) ? (!string.IsNullOrEmpty(dr["MWorkOrderStatusDesc"].ToString()) ? dr["MWorkOrderStatusDesc"].ToString() : "") : "";
                        o.Description = (!DBNull.Value.Equals(dr["Description"])) ? (!string.IsNullOrEmpty(dr["Description"].ToString()) ? dr["Description"].ToString() : "") : "";
                        if (dr["ReportedOn"] == DBNull.Value)
                        {
                            o.ReportedOnDes = "";
                        }
                        else
                        {
                            o.ReportedOn = (DateTime)(Convert.IsDBNull(dr["ReportedOn"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["ReportedOn"])).Value);
                            o.ReportedOnDes = o.ReportedOn.ToString("dd/MM/yyyy");
                        }
                        if (dr["Priority"] == DBNull.Value)
                        {
                            o.MPority = "";
                        }
                        else
                        {
                            o.MPority = (!DBNull.Value.Equals(dr["Priority"])) ? (!string.IsNullOrEmpty(dr["Priority"].ToString()) ? dr["Priority"].ToString() : "") : "";
                        }
                        
                        lst.Add(o);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lst;
        }
        public DataTable GetBlockRoomByKey(string key)
        {
            DataTable dt = new DataTable();
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@RoomBlockKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(key)
                }
           };
                using (var command = _connectionManager.CreateCommandSP(GetBlockRoomByBlockRoomKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetBlockRoomByWorkOrderID(int mworkorderno)
        {
            DataTable dt = new DataTable();
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);
                SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@MWorkOrderNo", SqlDbType.Int)
                {
                    Value = mworkorderno
                }
           };
                using (var command = _connectionManager.CreateCommandSP(GetBlockRoomByWrokOrderIDQuery(), CommandType.Text, MultiTenancySide, parameters))
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateBlockroom(BlockRoom room)
        {
            int intRowAffected = 0;
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);

                SqlParameter[] parameters = new SqlParameter[]
               {
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value = room.Roomkey
                },
                new SqlParameter("@Active", SqlDbType.Int)
                {
                    Value = room.Active
            },
                new SqlParameter("@Reason", SqlDbType.VarChar)
                {
                    Value =  room.Reason
                },
                new SqlParameter("@Comment", SqlDbType.VarChar)
                {
                    Value = room.Comment
                },
                   new SqlParameter("@BlockStaff", SqlDbType.VarChar)
                {
                    Value =room.Active >= 1?room.Blockstaff:DBNull.Value
                },
                new SqlParameter("@BlockTime", SqlDbType.DateTime)
                {
                    Value =room.Active >= 1?room.Blocktime:DBNull.Value
                },
                 new SqlParameter("@UnblockStaff", SqlDbType.VarChar)
                {
                    Value =room.Active >= 1?DBNull.Value:room.Unblockstaff
                },
                new SqlParameter("@UnblockTime", SqlDbType.DateTime)
                {
                    Value =room.Active >= 1?DBNull.Value:room.Unblocktime
                },
                new SqlParameter("@RoomBlockKey", SqlDbType.UniqueIdentifier)
                {
                    Value =room.Roomblockkey
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = room.TenantId
                }
               };
                using (var command = _connectionManager.CreateCommandSP(GetUpdateQuery(Convert.ToInt32(room.Active)), CommandType.Text, MultiTenancySide, parameters))
                {

                    intRowAffected = command.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intRowAffected;
        }
        public int UpdateWorkNote(MWorkNote workNote)
        {
            int intRowAffected = 0;
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);

                SqlParameter[] parameters = new SqlParameter[]
               {
                new SqlParameter("@MWorkNotesKey", SqlDbType.UniqueIdentifier)
                {
                    Value = workNote.MWorkNotesKey
                },
                new SqlParameter("@Details", SqlDbType.VarChar)
                {
                    Value =workNote.Details
                },
               new SqlParameter("@MWorkOrderKey", SqlDbType.UniqueIdentifier)
                {
                    Value = workNote.MWorkOrderKey
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = workNote.TenantId
                }
               };
                using (var command = _connectionManager.CreateCommandSP(GetUpdateWorkNoteQuery(), CommandType.Text, MultiTenancySide, parameters))
                {

                    intRowAffected = command.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intRowAffected;
        }

        public DataTable GetWorkNoteByKey(string key)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@MWorkNotesKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(key)
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetWorkNoteByWorkNoteKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
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

        public int InsertWorkNote(MWorkNote workNote)
        {
            int intRowAffected = 0;
            try
            {
                _connectionManager.EnsureConnectionOpen(MultiTenancySide);

                SqlParameter[] parameters = new SqlParameter[]
               {
                new SqlParameter("@MWorkOrderKey", SqlDbType.UniqueIdentifier)
                {
                    Value = workNote.MWorkOrderKey
                },
                new SqlParameter("@Details", SqlDbType.VarChar)
                {
                    Value =workNote.Details
                },
               new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = workNote.CreatedBy
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = workNote.TenantId
                }
               };
                using (var command = _connectionManager.CreateCommandSP(GetInsertWorkNoteQuery(), CommandType.Text, MultiTenancySide, parameters))
                {

                    intRowAffected = command.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intRowAffected;
        }
        public List<HistoryICleanDto> GetIcleanTodayHistory(Guid staffkey)
        {
            var lst = new List<HistoryICleanDto>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@StaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value = staffkey
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetIcleanTodayHistoryQuery(), CommandType.Text, MultiTenancySide, parameters))
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
                    HistoryICleanDto g = new HistoryICleanDto();
                    DateTime? nullable;
                    g.ChangedDate = (DateTime)(Convert.IsDBNull(dr["ChangedDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["ChangedDate"])).Value);
                    g.Detail = (!DBNull.Value.Equals(dr["Detail"])) ? (!string.IsNullOrEmpty(dr["Detail"].ToString()) ? dr["Detail"].ToString() : "") : "";
                    g.ChangedDateStr= Convert.IsDBNull(dr["ChangedDate"]) ? "" :(Convert.ToDateTime(dr["ChangedDate"])).ToString("dd/MM/yyyy HH:mm:ss");
                    lst.Add(g);
                }
            }
            return lst;
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
        #endregion

        #region SQL Query

        private string GetPriorityQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT PriorityID,Priority ");
            sb.Append(" FROM MPriority where Active = 1");
            return sb.ToString();
        }
        private static string GetIcleanTodayHistoryQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  [ChangedDate], [Detail]");
            sb.Append(" FROM    History ");
            sb.Append(" WHERE    ModuleName = 'iClean' ");
            sb.Append(" AND  StaffKey = @StaffKey AND TenantId=1 ");
            sb.Append(" AND  ChangedDate >= DATEADD(HOUR, -24, GETDATE())  ");
            sb.Append(" ORDER BY  Changeddate DESC;");
            return sb.ToString();
        }
        private static string GetWorkNoteByWorkNoteKeyQuery()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Select * ");
            builder.Append(" from MWorkNotes ");
            builder.Append(" where MWorkNotesKey = @MWorkNotesKey ");
            return builder.ToString();
        }
        private static string GetUpdateWorkNoteQuery()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Update MWorkNotes ");
            builder.Append(" SET ");
            builder.Append("   MWorkOrderKey = @MWorkOrderKey,TenantId=@TenantId ");
            builder.Append(" , Details = @Details  ");
            builder.Append(" WHERE MWorkNotesKey = @MWorkNotesKey ");
            return builder.ToString();
        }
        private static string GetInsertWorkNoteQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO MWorkNotes ");
            sb.Append("  ( Details, CreatedBy, CreatedOn, MWorkOrderKey,TenantId)  ");
            sb.Append("   VALUES ");
            sb.Append(" (@Details, @CreatedBy, GETDATE(), @MWorkOrderKey,@TenantId) ");
            return sb.ToString();
        }

        private static string GetUpdateQuery(int active)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Update RoomBlock ");
            builder.Append(" SET ");
            builder.Append("   RoomKey = @RoomKey");
            //builder.Append(" ,BlockDate = @BlockDate");
            builder.Append(" ,Active = @Active ");
            builder.Append(" ,Reason = @Reason ");
            builder.Append(" ,Comment = @Comment ");
            if (active > 0)
            {
                builder.Append(" ,BlockStaff = @BlockStaff");
                builder.Append(" ,BlockTime = @BlockTime ");
            }
            else
            {
                builder.Append(" ,UnblockStaff = @UnblockStaff");
                builder.Append(" ,UnblockTime = @UnblockTime ");
            }
            builder.Append(" WHERE RoomBlockKey = @RoomBlockKey ");
            return builder.ToString();
        }
        private static string GetBlockRoomByWrokOrderIDQuery()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Select rb.* , r.Unit ");
            builder.Append(" from RoomBlock rb");
            builder.Append(" LEFT JOIN  Room r ON  r.RoomKey = rb.RoomKey ");
            builder.Append(" where rb.MWorkOrderNo= @MWorkOrderNo ");
            builder.Append(" ORDER BY  r.Unit , rb.BlockDate ");
            return builder.ToString();
        }
        private static string GetBlockRoomByBlockRoomKeyQuery()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Select rb.* , r.Unit ");
            builder.Append(" from RoomBlock rb");
            builder.Append(" LEFT JOIN  Room r ON  r.RoomKey = rb.RoomKey ");
            builder.Append(" where rb.RoomBlockKey = @RoomBlockKey ");
            return builder.ToString();
        }
        public static string GetIncompletedWorkOrderStatusQuery(bool blnWorkStatus, bool blnByArea, bool blnByTechnician, bool blnByWorkType, bool blnByRoom, bool blnByStaff, bool blnByMpriority)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("  SELECT   wo.* ,  area.Description AS MAreaDesc, wt.Description AS MWorkTypeDesc, tech.Name AS MTechnicianName, wos.Description AS MWorkOrderStatusDesc, mp.Priority ");
            builder.Append("  FROM  MWorkOrder wo  ");
            builder.Append("  LEFT JOIN MArea area ON area.Seqno = wo.MArea  ");
            builder.Append("  LEFT JOIN MWorkType wt ON wt.Seqno = wo.MWorkType  ");
            builder.Append("  LEFT JOIN MTechnician tech ON tech.Seqno = wo.MTechnician ");
            builder.Append("  LEFT JOIN MWorkOrderStatus wos ON wos.Seqno = wo.MWorkOrderStatus  ");
            builder.Append("  LEFT JOIN Room r ON r.Unit = wo.Room  ");
            builder.Append("  LEFT JOIN MPriority mp ON mp.PriorityID = wo.Priority ");
            if (blnByArea != false && blnByTechnician != false && blnByWorkType != false && blnWorkStatus != false && blnByRoom != false && blnByStaff != false) { }
            else
            {
                if (blnWorkStatus)
                    builder.Append(" where wo.MWorkOrderStatus = @MWorkStatus ");
                if (blnByArea)
                    if (blnWorkStatus)
                        builder.Append(" AND wo.MArea = @MArea ");
                    else
                        builder.Append(" where wo.MArea = @MArea ");
                if (blnByTechnician)
                    if (blnByArea || blnWorkStatus)
                        builder.Append(" AND wo.MTechnician = @MTechnician ");
                    else
                        builder.Append(" where wo.MTechnician = @MTechnician ");
                if (blnByWorkType)
                    if (blnByArea || blnByTechnician || blnWorkStatus)
                        builder.Append(" AND wo.MWorkType = @MWorkType ");
                    else
                        builder.Append(" where wo.MWorkType = @MWorkType ");
                if (blnByRoom)
                    if (blnByArea || blnByTechnician || blnWorkStatus || blnByWorkType)
                        builder.Append(" AND wo.Room = @Room ");
                    else
                        builder.Append(" where wo.Room = @Room ");
                if (blnByStaff)
                    if (blnByArea || blnByTechnician || blnWorkStatus || blnByWorkType || blnByRoom)
                        builder.Append(" AND wo.ReportedBy = @ReportedBy ");
                else
                        builder.Append(" where wo.ReportedBy = @ReportedBy ");
                if (blnByMpriority)
                    if (blnByArea || blnByTechnician || blnWorkStatus || blnByWorkType || blnByRoom || blnByStaff)
                        builder.Append(" AND wo.Priority = @Priority ");
                    else
                        builder.Append(" where wo.Priority = @Priority ");
                
            }
            builder.Append(" order by wo.Seqno desc, r.Floor, wo.Room ");
            return builder.ToString();
        }
        private static string GetBlockRoomWorkOrderByQuery(bool blnByTechnician, bool blnByRoom, bool blnByStartDate, bool blnByEndDate)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  rb.RoomBlockKey, wo.Seqno, wo.Description, tech.Name , rb.RoomKey , rb.BlockDate , r.Unit,CASE WHEN rb.Active = 2 THEN 'out of service' ELSE 'Out of Order' END AS RoomStatus ");
            sb.Append(" FROM    MWorkOrder wo");
            sb.Append(" RIGHT JOIN RoomBlock rb ON rb.MWorkOrderno = wo.Seqno  ");
            sb.Append(" LEFT JOIN Room r ON r.RoomKey = rb.RoomKey  ");
            sb.Append(" LEFT JOIN MTechnician tech ON tech.Seqno = wo.MTechnician      ");
            sb.Append(" WHERE (rb.Active = 1 OR rb.Active = 2) ");//wo.MWorkOrderStatus IN (0, 1, 2) AND
            if (blnByTechnician)
                sb.Append("  AND wo.MTechnician = @MTechnician     ");
            if (blnByRoom)
                sb.Append("  AND r.RoomKey = @RoomKey    ");
            if (blnByStartDate)
                sb.Append("  AND rb.BlockDate >= @FromDate ");
            else
                sb.Append(" AND rb.BlockDate >= CONVERT(DATE, GETDATE())   ");
            if (blnByEndDate)
                sb.Append("  AND rb.BlockDate <= @ToDate   ");
            sb.Append(" ORDER BY  rb.BlockDate , r.Unit , wo.Seqno ; ");
            return sb.ToString();
        }
        public static string InsertHistory()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert into history ");
            sb.Append(" (HistoryKey, StaffKey, SourceKey, ModuleName, Operation, ChangedDate, Detail, TableName, NewValue, OldValue,TenantId )  ");
            sb.Append(" Values ");
            sb.Append(" (@HistoryKey, @StaffKey, @SourceKey,  @ModuleName, @Operation, GETDATE() ,@Detail, @TableName, @NewValue, @OldValue,@TenantId )  ");
            return sb.ToString();
        }
        private static string GetInsertQuery()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" INSERT INTO RoomBlock ");
            builder.Append("  ( RoomKey, Active, BlockDate, Reason, Comment, BlockStaff, UnblockStaff, BlockTime, UnblockTime, MWorkOrderNo,TenantId)  ");
            builder.Append("   VALUES ");
            builder.Append(" (@RoomKey, @Active, @BlockDate, @Reason, @Comment, @BlockStaff, @UnblockStaff, @BlockTime, @UnblockTime, @MWorkOrderNo,@TenantId) ");
            return builder.ToString();
        }
        private static string GetReservationByRoomKeyDateRange()
        {
            StringBuilder sbQueryBuilder = new StringBuilder();
            sbQueryBuilder.Append(" SELECT ReservationKey, CheckInDate, CheckOutDate, DocNo  FROM  Reservation   ");
            sbQueryBuilder.Append(" WHERE  Status in (1,2)  AND ");
            sbQueryBuilder.Append("   RoomKey = @RoomKey and @fromDate between CheckInDate and CheckOutDate ; ");
            return sbQueryBuilder.ToString();
        }
        private static string GetBlockRoomCountByDateRangeQuery()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT  Count(MWorkOrderno) AS RoomBlockCount ");
            builder.Append(" FROM RoomBlock ");
            builder.Append(" WHERE     ");
            builder.Append("   MWorkOrderno = @MWorkOrderno  AND  RoomKey = @RoomKey AND");
            builder.Append("   BlockDate >= @FromBlockDate  AND BlockDate <= @ToBlockDate   AND ");
            builder.Append("   Active !=0  ; ");
            return builder.ToString();
        }
        private static string GetWorkOrderByIDQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MWorkOrder ");
            sb.Append(" WHERE   Seqno = @Seqno ; ");
            return sb.ToString();
        }
        private static string GetWIPWorkOrderQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  Seqno,  MWorkOrderKey , CONCAT( '#', Seqno, ' : ', Description) AS Description ");
            //sb.Append(" SELECT  Seqno, MWorkOrderKey , Description ");
            sb.Append(" FROM    MWorkOrder ");
            sb.Append(" WHERE   MWorkOrderStatus NOT IN (2, 3, 4, 5) ");
            sb.Append(" ORDER BY  Seqno Desc ;");
            return sb.ToString();
        }
        private static string GetSystemControlQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT * FROM Control ;    ");
            return sb.ToString();
        }
        private static string GetRoomByMaidKeyQuery(bool blnFilterByMaidStatusKey, bool blnFilterByRoomStatusKey, bool blnFilterByFloor, bool blnFilterByGuestStatus)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  ");
            sb.Append("   S.RoomKey, Unit,S.MaidStatusKey,M.MaidStatus,S.RoomStatusKey,R.RoomStatus,S.ReservationKey,RoomType,InterconnectRoom,Floor, LinenChange, DND, ");
            sb.Append("   CleaningTime,LinenDays,Bed,S.CheckInDate,S.CheckInTime,S.CheckOutDate,S.CheckOutTime,I.Maid,HMMNotes,S.GuestArrived,S.Status,S.PreCheckInCount,GS.Status AS GuestStatus  ");
            sb.Append(" FROM  dbo.UDF_DayRoomStatusiClean( @date ) S ");
            sb.Append(" LEFT JOIN   ");
            sb.Append("   (Select A.RoomKey,InterconnectRoom=B.Unit,");
            sb.Append("   Maid=M.Name,A.HMMNotes from Room A Left join Room B on A.InterconnectRoomKey=B.RoomKey Left Join Maid M on ");
            sb.Append("   A.MaidKey=M.MaidKey where A.Active=1) I on S.RoomKey=I.RoomKey ");
            sb.Append(" LEFT JOIN  MaidStatus M on S.MaidStatusKey=M.MaidStatusKey  ");
            sb.Append(" LEFT JOIN  RoomStatus R on S.RoomStatusKey=R.RoomStatusKey ");
            sb.Append(" Left join Reservation V on S.ReservationKey = V.ReservationKey Left Join GuestStatus GS on V.GuestStatus = GS.StatusCode ");
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
            //sb.Append(" ORDER BY S.GuestArrived, S.PreCheckInCount desc, Floor, Unit ;  ");
            sb.Append(" ORDER BY CASE WHEN ISNULL(NULLIF(S.GuestArrived, ''), '') = '' THEN 1 ELSE 0 END, S.PreCheckInCount, S.Floor,S.Unit ");


            return sb.ToString();
        }
        private static string GetWOByTechnicianQuery(bool blnFilterByWOStatus, bool blnByRoomStatus)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  wo.* , a.Description AS Area, wt.Description AS WorkType, wos.Description AS WorkOrderStatus, tech.Name, rs.RoomStatus  ");
            sb.Append(" FROM    MWorkOrder wo");
            sb.Append(" LEFT JOIN MArea a  ON a.Seqno = wo.MArea  ");
            sb.Append(" LEFT JOIN MWorkType wt  ON wt.Seqno = wo.MWorkType  ");
            sb.Append(" LEFT JOIN MWorkOrderStatus wos  ON wos.Seqno = wo.MWorkOrderStatus    ");
            sb.Append(" LEFT JOIN MTechnician tech  ON tech.Seqno = wo.MTechnician      ");
            sb.Append(" LEFT JOIN Room r on r.Unit = wo.Room LEFT JOIN RoomStatus rs on rs.RoomStatusKey = r.RoomStatusKey ");
            sb.Append(" WHERE  wo.MTechnician = @MTechnician  ");
            if (blnFilterByWOStatus)
                sb.Append("   AND  wo.MWorkOrderStatus = @MWorkOrderStatus    ");
            if (blnByRoomStatus)
                sb.Append(" AND rs.RoomStatus = @RoomStatus ");
            sb.Append(" ORDER BY  Seqno DESC ;   ");
            return sb.ToString();
        }
        private static string GetUnassignedTechWorkOrderCountQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  Count(Seqno) AS noOfWork ");
            sb.Append(" FROM    MWorkOrder ");
            sb.Append(" WHERE   MTechnician IS NULL  AND  MWorkOrderStatus NOT IN (2, 3, 4, 5) ");
            return sb.ToString();
        }
        private static string GetTodayHistoryQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  [HistoryKey], [ChangedDate], [Detail], [TableName] ");
            sb.Append(" FROM    History ");
            sb.Append(" WHERE    ModuleName = 'iRepair' AND TenantId=1 ");
            sb.Append(" AND  StaffKey = @StaffKey ");
            //sb.Append(" AND  ChangedDate >= CONVERT (datetime, GETDATE()) ");
            sb.Append(" AND  ChangedDate >= DATEADD(HOUR, -24, GETDATE())  ");
            sb.Append(" ORDER BY  Changeddate DESC;");
            return sb.ToString();
        }
        private static string GetWorkOrderStatusCountByTechnicianQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  CAST(-1 AS INT) AS WorkOrderStatus, 'ALL' AS WODescription, Count(wo.Seqno) AS TaskCount ");
            sb.Append(" FROM  MWorkOrder wo");
            sb.Append(" WHERE  wo.MTechnician = @MTechnician ");
            sb.Append(" GROUP BY  wo.MTechnician  union ");
            sb.Append(" SELECT  CAST(0 AS INT) AS WorkOrderStatus, 'Initial Entry' AS WODescription, Count(wo.Seqno) AS TaskCount ");
            sb.Append(" FROM  MWorkOrder wo WHERE  wo.MWorkOrderStatus = 0 and wo.MTechnician = @MTechnician ");
            sb.Append("Union ALL   ");
            sb.Append(" SELECT  wo.MWorkOrderStatus, wos.Description AS WODescription, Count(wo.MTechnician) AS TaskCount ");
            sb.Append(" FROM  MWorkOrder wo ");
            sb.Append(" LEFT JOIN  MWorkOrderStatus wos on wo.MWorkOrderStatus = wos.Seqno");
            sb.Append(" WHERE   wo.MTechnician = @MTechnician and wo.MWorkOrderStatus!=0 ");
            sb.Append(" GROUP BY  wo.MWorkOrderStatus, wos.Description  ");
            sb.Append(" ORDER BY  WorkOrderStatus;   ");
            return sb.ToString();
        }
        private static string GetWorkOrderByTechnicianQuery(bool blnFilterByWOStatus, bool blnByRoomStatus, bool blnByPriority)
        {
            StringBuilder sb = new StringBuilder();
            #region old
            //sb.Append(" SELECT  wo.* , a.Description AS Area, wt.Description AS WorkType, wos.Description AS WorkOrderStatus, tech.Name, rs.RoomStatus,prio.Priority as PriorityDes  ");
            //sb.Append(" FROM  MWorkOrder wo");
            //sb.Append(" LEFT JOIN MArea a  ON a.Seqno = wo.MArea  ");
            //sb.Append(" LEFT JOIN MWorkType wt  ON wt.Seqno = wo.MWorkType  ");
            //sb.Append(" LEFT JOIN MWorkOrderStatus wos  ON wos.Seqno = wo.MWorkOrderStatus    ");
            //sb.Append(" LEFT JOIN MTechnician tech  ON tech.Seqno = wo.MTechnician  LEFT JOIN MPriority prio  ON prio.Sort = wo.Priority    ");
            //sb.Append(" LEFT JOIN Room r on r.Unit = wo.Room LEFT JOIN RoomStatus rs on rs.RoomStatusKey = r.RoomStatusKey ");
            #endregion
            #region new
            sb.Append("SELECT wo.* , a.Description AS Area, wt.Description AS WorkType, wos.Description AS WorkOrderStatus, tech.Name, S.RoomKey, Unit,M.MaidStatus,R.RoomStatus,prio.Priority as PriorityDes ");
            sb.Append("FROM  MWorkOrder wo LEFT JOIN MArea a  ON a.Seqno = wo.MArea   LEFT JOIN MWorkType wt  ON wt.Seqno = wo.MWorkType ");
            sb.Append("LEFT JOIN MWorkOrderStatus wos  ON wos.Seqno = wo.MWorkOrderStatus ");
            sb.Append("LEFT JOIN MTechnician tech  ON tech.Seqno = wo.MTechnician  LEFT JOIN MPriority prio  ON prio.Sort = wo.Priority ");
            sb.Append("LEFT JOIN dbo.UDF_DayRoomStatus((ISNULL((SELECT systemdate FROM control), GETDATE()))) S on S.Unit = wo.Room Left Join MaidStatus M on S.MaidStatusKey = M.MaidStatusKey ");
            sb.Append("Left Join RoomStatus R on S.RoomStatusKey = R.RoomStatusKey");
            #endregion

            sb.Append(" WHERE  wo.MTechnician = @MTechnician  ");
            if (blnFilterByWOStatus)
                sb.Append("   AND  wo.MWorkOrderStatus = @MWorkOrderStatus    ");
            if (blnByRoomStatus)
                sb.Append(" AND R.RoomStatus = @RoomStatus ");
            if (blnByPriority)
                sb.Append(" AND wo.Priority = @Priority ");
            sb.Append(" ORDER BY  Seqno DESC, prio.Sort ;   ");
            return sb.ToString();
        }
        private static string GetRoomCountByMaidKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select top 1 * from MWorkTimeSheet where Hdr_Seqno = @Seqno order by Seqno desc  ");
            return sb.ToString();
        }
        private static string GetMaidStatusByRoomKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT MaidStatus ");
            sb.Append(" FROM MaidStatus where MaidStatusKey = (select MaidStatusKey from Room where RoomKey = @RoomKey) ");
            return sb.ToString();
        }

       


        #endregion
    }
}
