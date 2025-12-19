using Abp.Data;
using Abp.EntityFrameworkCore;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.EntityFrameworkCore;
using BEZNgCore.EntityFrameworkCore.Repositories;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.Authorization.IrepairDal
{

    public class MworkordertimesheetdalRepository : BEZNgCoreRepositoryBase<MWorkOrderStatus, int>, IMworkordertimesheetdalRepository
    {
        private readonly IActiveTransactionProvider _transactionProvider;
        ConnectionManager _connectionManager;
        public MworkordertimesheetdalRepository(IDbContextProvider<BEZNgCoreDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
            _connectionManager = new ConnectionManager(dbContextProvider, _transactionProvider);
        }
        #region interfaceimplement & related function 
        public List<BlockUnBlockRoomListOutput> GetBlockRoomByWorkOrderID(int woID)
        {
            var lst = new List<BlockUnBlockRoomListOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MWorkOrderNo", SqlDbType.Int)
                {
                    Value = woID
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
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DateTime? nullable;
                    string blocktime = "";
                    if (!string.IsNullOrEmpty(dr["BlockTime"].ToString()))
                    {
                        blocktime = GetDateTimeToDisplay(new DateTime?(Convert.ToDateTime(dr["BlockTime"])).Value);

                    }
                    string blockstaffname = (!DBNull.Value.Equals(dr["BlockStaff"])) ? (!string.IsNullOrEmpty(dr["BlockStaff"].ToString()) ? dr["BlockStaff"].ToString() : "") : "";
                    string bb = blockstaffname + "@" + blocktime;
                    string unblocktime = "";
                    if (!string.IsNullOrEmpty(dr["UnBlockTime"].ToString()))
                    {
                        unblocktime = GetDateTimeToDisplay(new DateTime?(Convert.ToDateTime(dr["UnBlockTime"])).Value);

                    }
                    string unblockstaffname = (!DBNull.Value.Equals(dr["UnBlockStaff"])) ? (!string.IsNullOrEmpty(dr["UnBlockStaff"].ToString()) ? dr["UnBlockStaff"].ToString() : "") : "";
                    string ubb = unblockstaffname + "@" + unblocktime;
                    BlockUnBlockRoomListOutput o = new BlockUnBlockRoomListOutput();
                    o.RoomBlockKey = (!DBNull.Value.Equals(dr["RoomBlockKey"])) ? (!string.IsNullOrEmpty(dr["RoomBlockKey"].ToString()) ? new Guid(dr["RoomBlockKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.Unit = (!DBNull.Value.Equals(dr["Unit"])) ? (!string.IsNullOrEmpty(dr["Unit"].ToString()) ? dr["Unit"].ToString() : "") : "";
                    o.GetBlockRoomStatusSymbol = GetBlockRoomStatusSymbol(!DBNull.Value.Equals(dr["Active"]) ? Convert.ToInt32(dr["Active"]) : 0);
                    o.GetEditBlockRoomButton = "OnUpdateBlockRoom";//GetEditBlockRoomButton(o.RoomBlockKey);
                    o.BlockDate = GetDateToDisplay((DateTime)(Convert.IsDBNull(dr["BlockDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["BlockDate"])).Value));
                    o.Reason = (!DBNull.Value.Equals(dr["Reason"])) ? (!string.IsNullOrEmpty(dr["Reason"].ToString()) ? dr["Reason"].ToString() : "-") : "-";
                    o.Comment = (!DBNull.Value.Equals(dr["Comment"])) ? (!string.IsNullOrEmpty(dr["Comment"].ToString()) ? dr["Comment"].ToString() : "-") : "-";
                    o.BlockedBy = bb;
                    o.UnblockedBy = ubb;

                    lst.Add(o);
                }
            }
            return lst;
        }
        #region Display Block Room Symbol Helper
        public static string GetBlockRoomStatusSymbol(object status)
        {
            string strReturnValue = "";
            try
            {
                strReturnValue = status.ToString();
                switch (strReturnValue)
                {
                    case "0":
                        strReturnValue = "<i class=\"fa fa-square-o fa-fw\"></i>";
                        break;
                    case "1":
                    case "2":
                        strReturnValue = "<i class=\"fa fa-minus-square fa-fw\"></i>";
                        break;
                    default:
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

        #region Display Edit button @BlockRoom
        public static string GetEditBlockRoomButton(object blockRoomKey)
        {
            string strReturnValue = "";
            try
            {
                //<a class="btn btn-primary btn-outline btn-lg"  href='javascript:OnUpdateBlockRoom(<%# Eval("RoomBlockKey") %>);' >  <i class="fa fa-pencil-square fa-lg"></i>   </a>  
                strReturnValue = "<a  id=\"btnEdit" + blockRoomKey.ToString() + "\" runat=\"server\"   class=\"btn btn-primary btn-outline btn-lg\"  href=\"javascript:OnUpdateBlockRoom('" + blockRoomKey.ToString() + "');\" >   <i class=\"fa fa-pencil-square\"></i> </a>";
                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }

        #endregion
        public List<WoWorkNote> GetWorkNotes(string woKey)
        {
            var lst = new List<WoWorkNote>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@woKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(woKey)
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetWorkNotesQuery(), CommandType.Text, MultiTenancySide, parameters))
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
                    WoWorkNote o = new WoWorkNote();
                    o.MWorkNotesKey = (!DBNull.Value.Equals(dr["MWorkNotesKey"])) ? (!string.IsNullOrEmpty(dr["MWorkNotesKey"].ToString()) ? new Guid(dr["MWorkNotesKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.Details = (!DBNull.Value.Equals(dr["Details"])) ? (!string.IsNullOrEmpty(dr["Details"].ToString()) ? dr["Details"].ToString() : "-") : "-";
                    o.CreatedOn = GetDateTimeToDisplay((DateTime)(Convert.IsDBNull(dr["CreatedOn"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["CreatedOn"])).Value));
                    o.GetEditWorkNoteButton = "OnUpdateWorkNote";
                    lst.Add(o);
                }
            }
            return lst;
        }


        public DataTable GetWorkOrderByID(int seqno)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = seqno
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

        public List<WoSecurityAuditlist> GetWorkOrderHistory(string woKey)
        {
            var lst = new List<WoSecurityAuditlist>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@woKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(woKey)
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
                    DateTime? nullable;
                    WoSecurityAuditlist o = new WoSecurityAuditlist();
                    o.HistoryKey = (!DBNull.Value.Equals(dr["HistoryKey"])) ? (!string.IsNullOrEmpty(dr["HistoryKey"].ToString()) ? new Guid(dr["HistoryKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.ChangedDate = GetDateTimeToDisplay((DateTime)(Convert.IsDBNull(dr["ChangedDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["ChangedDate"])).Value));
                    o.UserName = (!DBNull.Value.Equals(dr["UserName"])) ? (!string.IsNullOrEmpty(dr["UserName"].ToString()) ? dr["UserName"].ToString() : "") : "";
                    o.OldValue = (!DBNull.Value.Equals(dr["OldValue"])) ? (!string.IsNullOrEmpty(dr["OldValue"].ToString()) ? dr["OldValue"].ToString() : "-") : "-";
                    o.NewValue = (!DBNull.Value.Equals(dr["NewValue"])) ? (!string.IsNullOrEmpty(dr["NewValue"].ToString()) ? dr["NewValue"].ToString() : "") : "";
                    o.Detail = (!DBNull.Value.Equals(dr["Detail"])) ? (!string.IsNullOrEmpty(dr["Detail"].ToString()) ? dr["Detail"].ToString() : "") : "";
                    lst.Add(o);
                }
            }
            return lst;
        }
        public string GetDateTimeToDisplay(object inputDate)
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
        public List<WoTimeSheetListOutput> GetWorkTimeSheetByWOID(int woID)
        {
            var lst = new List<WoTimeSheetListOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Hdr_Seqno", SqlDbType.Int)
                {
                    Value = woID
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetWorkTimeSheetByWOIDQuery(), CommandType.Text, MultiTenancySide, parameters))
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
                    WoTimeSheetListOutput o = new WoTimeSheetListOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.WorkDate = GetDateToDisplay((DateTime)(Convert.IsDBNull(dr["WDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["WDate"])).Value));
                    o.StartDate = (!DBNull.Value.Equals(dr["TimeFrom"])) ? (!string.IsNullOrEmpty(dr["TimeFrom"].ToString()) ? dr["TimeFrom"].ToString() : "") : "";
                    o.EndDate = (!DBNull.Value.Equals(dr["TimeTo"])) ? (!string.IsNullOrEmpty(dr["TimeTo"].ToString()) ? dr["TimeTo"].ToString() : "") : "";
                    o.Technician = (!DBNull.Value.Equals(dr["Name"])) ? (!string.IsNullOrEmpty(dr["Name"].ToString()) ? dr["Name"].ToString() : "") : "";
                    o.Notes = (!DBNull.Value.Equals(dr["Notes"])) ? (!string.IsNullOrEmpty(dr["Notes"].ToString()) ? dr["Notes"].ToString() : "-") : "-";
                    lst.Add(o);
                }
            }
            return lst;
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
        public bool MaidHasStartedTask(int seqno)
        {
            bool blnHasStartedTask = false;


            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = seqno
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

        public List<VWorkOrderOutput> GetUnassignedTechWorkOrder()
        {
            var lst = new List<VWorkOrderOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetUnassignedTechWorkOrderQuery(), CommandType.Text, MultiTenancySide))
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
                    string note = (!DBNull.Value.Equals(dr["Notes"])) ? (!string.IsNullOrEmpty(dr["Notes"].ToString()) ? dr["Notes"].ToString() : "") : "";
                    VWorkOrderOutput o = new VWorkOrderOutput();
                    o.MWorkOrderKey = (!DBNull.Value.Equals(dr["MWorkOrderKey"])) ? (!string.IsNullOrEmpty(dr["MWorkOrderKey"].ToString()) ? new Guid(dr["MWorkOrderKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Description = (!DBNull.Value.Equals(dr["Description"])) ? (!string.IsNullOrEmpty(dr["Description"].ToString()) ? dr["Description"].ToString() : "") : "";
                    o.WorkOrderStatus = (!DBNull.Value.Equals(dr["WorkOrderStatus"])) ? (!string.IsNullOrEmpty(dr["WorkOrderStatus"].ToString()) ? dr["WorkOrderStatus"].ToString() : "") : "";
                    o.Name = (!DBNull.Value.Equals(dr["Name"])) ? (!string.IsNullOrEmpty(dr["Name"].ToString()) ? dr["Name"].ToString() : "-") : "-";
                    o.Notes = GetDescriptionByLength(note, 30);
                    o.Room = (!DBNull.Value.Equals(dr["Room"])) ? (!string.IsNullOrEmpty(dr["Room"].ToString()) ? dr["Room"].ToString() : "-") : "-";
                    o.Area = (!DBNull.Value.Equals(dr["Area"])) ? (!string.IsNullOrEmpty(dr["Area"].ToString()) ? dr["Area"].ToString() : "-") : "-";
                    o.WorkType = (!DBNull.Value.Equals(dr["WorkType"])) ? (!string.IsNullOrEmpty(dr["WorkType"].ToString()) ? dr["WorkType"].ToString() : "-") : "-";
                    o.StaffName = (!DBNull.Value.Equals(dr["StaffName"])) ? (!string.IsNullOrEmpty(dr["StaffName"].ToString()) ? dr["StaffName"].ToString() : "") : "";
                    if (dr["ReportedOn"] == DBNull.Value)
                    {
                        o.ReportedOn = "";
                    }
                    else
                    {
                        o.ReportedOn = GetDateToDisplay((DateTime)(Convert.IsDBNull(dr["ReportedOn"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["ReportedOn"])).Value));
                    }
                     
                  
                    lst.Add(o);
                }
            }
            return lst;
        }
        public string GetDescriptionByLength(string inputValue, int length)
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
        public List<VWorkOrderOutput> GetWorkOrderByStatus(int woStatus)
        {
            var lst = new List<VWorkOrderOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MWorkOrderStatus", SqlDbType.Int)
                {
                    Value = woStatus
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetWorkOrderByStatusQuery(), CommandType.Text, MultiTenancySide, parameters))
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
                    string note = (!DBNull.Value.Equals(dr["Notes"])) ? (!string.IsNullOrEmpty(dr["Notes"].ToString()) ? dr["Notes"].ToString() : "") : "";
                    VWorkOrderOutput o = new VWorkOrderOutput();
                    o.MWorkOrderKey = (!DBNull.Value.Equals(dr["MWorkOrderKey"])) ? (!string.IsNullOrEmpty(dr["MWorkOrderKey"].ToString()) ? new Guid(dr["MWorkOrderKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Description = (!DBNull.Value.Equals(dr["Description"])) ? (!string.IsNullOrEmpty(dr["Description"].ToString()) ? dr["Description"].ToString() : "") : "";
                    o.WorkOrderStatus = (!DBNull.Value.Equals(dr["WorkOrderStatus"])) ? (!string.IsNullOrEmpty(dr["WorkOrderStatus"].ToString()) ? dr["WorkOrderStatus"].ToString() : "") : "";
                    o.Name = (!DBNull.Value.Equals(dr["Name"])) ? (!string.IsNullOrEmpty(dr["Name"].ToString()) ? dr["Name"].ToString() : "-") : "-";
                    o.Notes = GetDescriptionByLength(note, 30);
                    o.Room = (!DBNull.Value.Equals(dr["Room"])) ? (!string.IsNullOrEmpty(dr["Room"].ToString()) ? dr["Room"].ToString() : "-") : "-";
                    o.Area = (!DBNull.Value.Equals(dr["Area"])) ? (!string.IsNullOrEmpty(dr["Area"].ToString()) ? dr["Area"].ToString() : "-") : "-";
                    o.WorkType = (!DBNull.Value.Equals(dr["WorkType"])) ? (!string.IsNullOrEmpty(dr["WorkType"].ToString()) ? dr["WorkType"].ToString() : "-") : "-";
                    o.StaffName = (!DBNull.Value.Equals(dr["StaffName"])) ? (!string.IsNullOrEmpty(dr["StaffName"].ToString()) ? dr["StaffName"].ToString() : "") : "";
                   // o.ReportedOn = GetDateToDisplay((DateTime)(Convert.IsDBNull(dr["ReportedOn"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["ReportedOn"])).Value));
                    if (dr["ReportedOn"] == DBNull.Value)
                    {
                        o.ReportedOn = "";
                    }
                    else
                    {
                        o.ReportedOn = GetDateToDisplay((DateTime)(Convert.IsDBNull(dr["ReportedOn"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["ReportedOn"])).Value));
                    }
                    lst.Add(o);
                }
            }
            return lst;
        }

        public int UpdatAssignTechnicianToWorkOrder(List<MWorkOrderInput> listWork)
        {
            int intRowAffected = 0;
            foreach (MWorkOrderInput work in listWork)
            {
                intRowAffected = UpdatAssignTechnicianToWorkOrder(work);
            }


            return intRowAffected;
        }


        private int UpdatAssignTechnicianToWorkOrder(MWorkOrderInput work)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = work.Id
                },
                new SqlParameter("@MWorkOrderKey", SqlDbType.UniqueIdentifier)
                {
                    Value = work.MWorkOrderKey
                },
                new SqlParameter("@MTechnician", SqlDbType.Int)
                {
                    Value = (work.MTechnician == null ? DBNull.Value : work.MTechnician)
                },
                new SqlParameter("@LastUpdateBy", SqlDbType.NVarChar)
                {
                    Value =work.LastUpdateBy
                },
                new SqlParameter("@LastUpdateStaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value =work.LastUpdateStaffKey
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = work.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetAssignTechnicianQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        public int InsertHistoryList(List<History> listHistory)
        {
            int intRowAffected = 0;
            foreach (History history in listHistory)
            {
                intRowAffected = InsertHistory(history);
            }
            return intRowAffected;
        }

        private int InsertHistory(History history)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@HistoryKey", SqlDbType.UniqueIdentifier)
                {
                    Value = history.Id
                },
                new SqlParameter("@StaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value = history.StaffKey
                },
                new SqlParameter("@SourceKey", SqlDbType.UniqueIdentifier)
                {
                    Value =   (history.SourceKey == null ? DBNull.Value : history.SourceKey)
                },
                new SqlParameter("@ModuleName", SqlDbType.VarChar)
                {
                    Value =history.ModuleName
                },
                new SqlParameter("@Operation", SqlDbType.Char)
                {
                    Value =history.Operation
                },
                new SqlParameter("@TableName", SqlDbType.VarChar)
                {
                    Value =history.TableName
                },
                new SqlParameter("@Detail", SqlDbType.VarChar)
                {
                    Value = (history.Detail.Trim().Length > 200 ? history.Detail.Trim().Substring(0, 190) + "..." : history.Detail.Trim())
                },
                new SqlParameter("@NewValue", SqlDbType.NVarChar)
                {
                    Value =(history.NewValue == null ? DBNull.Value : history.NewValue)
                },
                new SqlParameter("@OldValue", SqlDbType.NVarChar)
                {
                    Value =(history.OldValue == null ? DBNull.Value : history.OldValue)
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
            return intRowAffected;
        }

        public int UpdatWOStatusToWorkOrder(List<MWorkOrderInput> listWork)
        {
            int intRowAffected = 0;
            foreach (MWorkOrderInput work in listWork)
            {
                intRowAffected = UpdatWOStatusToWorkOrder(work);
            }


            return intRowAffected;
        }
        private int UpdatWOStatusToWorkOrder(MWorkOrderInput work)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            int intWOStatus = Convert.ToInt32(work.MWorkOrderStatus);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = work.Id
                },
                new SqlParameter("@MWorkOrderKey", SqlDbType.UniqueIdentifier)
                {
                    Value = work.MWorkOrderKey
                },
                new SqlParameter("@MWorkOrderStatus", SqlDbType.Int)
                {
                    Value = (work.MWorkOrderStatus == null ? DBNull.Value : work.MWorkOrderStatus)
                },
                new SqlParameter("@LastUpdateBy", SqlDbType.NVarChar)
                {
                    Value =work.LastUpdateBy
                },
                new SqlParameter("@LastUpdateStaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value =work.LastUpdateStaffKey
                },
                 new SqlParameter("@CompletedBy", SqlDbType.NVarChar)
                {
                    Value =work.LastUpdateBy
                },
                new SqlParameter("@CompletedStaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value =work.LastUpdateStaffKey
                },
                 new SqlParameter("@SignedOffBy", SqlDbType.NVarChar)
                {
                    Value =work.LastUpdateBy
                },
                new SqlParameter("@SignedOffStaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value =work.LastUpdateStaffKey
                },
                 new SqlParameter("@CancelledBy", SqlDbType.NVarChar)
                {
                    Value =work.LastUpdateBy
                },
                new SqlParameter("@CancelledStaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value =work.LastUpdateStaffKey
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = work.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateWorkOrderStatusQuery(intWOStatus), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public string GetMTechnicianBySeqNo(int technicalID)
        {
            string intRowAffected = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = technicalID
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetMTechnicianBySeqnoQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    intRowAffected = obj.ToString();
                }
            }
            return intRowAffected;
        }

        public int Inserttimesheet(MWorkTimeSheetInput timeSheet)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Hdr_Seqno", SqlDbType.Int)
                {
                    Value = timeSheet.Hdr_Seqno
                },
                new SqlParameter("@MTechnician", SqlDbType.Int)
                {
                    Value = (timeSheet.MTechnician == null ? DBNull.Value : timeSheet.MTechnician)
                },
                new SqlParameter("@WDate", SqlDbType.DateTime)
                {
                    Value = timeSheet.WDate
                },
                new SqlParameter("@TimeFrom", SqlDbType.DateTime)
                {
                    Value =(timeSheet.TimeFrom == null ?  DBNull.Value : timeSheet.TimeFrom)
                },
                new SqlParameter("@TimeTo", SqlDbType.DateTime)
                {
                    Value =(timeSheet.TimeTo == null ?  DBNull.Value : timeSheet.TimeTo)
                },
                 new SqlParameter("@Notes", SqlDbType.NVarChar)
                {
                    Value =timeSheet.Notes
                },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier)
                {
                    Value =timeSheet.CreatedBy
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = timeSheet.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetInsertTimesheetQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int UpdateRoomMaidStatus(Room room)
        {

            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            bool blnUpdateNote = false;
            if (!string.IsNullOrEmpty(room.HMMNotes))
                blnUpdateNote = true;
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value = room.Id
                },
                new SqlParameter("@MaidStatusKey", SqlDbType.UniqueIdentifier)
                {
                    Value =Guid.Parse(room.MaidStatusKey.ToString())
                },
                new SqlParameter("@HMMNotes", SqlDbType.VarChar)
                {
                    Value =  (room.HMMNotes==null?DBNull.Value:room.HMMNotes)

                }
           };
            using (var command = _connectionManager.CreateCommandSP(UpdateMaidStatusByRoomKeyQuery(blnUpdateNote), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public List<GetMaidStatusOutput> GetMaidStatusKeyByStatus(string status)
        {
            var list = new List<GetMaidStatusOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@MaidStatus", SqlDbType.VarChar)
                {
                    Value = status
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetMaidStatusByStatusQuery(), CommandType.Text, MultiTenancySide, parameters))
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

                    GetMaidStatusOutput o = new GetMaidStatusOutput();
                    o.MaidStatusKey = (!DBNull.Value.Equals(dr["MaidStatusKey"])) ? (!string.IsNullOrEmpty(dr["MaidStatusKey"].ToString()) ? dr["MaidStatusKey"].ToString() : "") : "";
                    list.Add(o);
                }
            }
            return list;
        }
        public DataTable GetWorkTimeSheetByHdr_Seqno(int hdr_Seqno)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = hdr_Seqno
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetWorkTimeSheetByHdr_SeqnoQuery(), CommandType.Text, MultiTenancySide, parameters))
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

        public int UpdateByHdr_Seqno(MWorkTimeSheetInput timeSheet)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value =  timeSheet.Seqno
                },
                new SqlParameter("@MTechnician", SqlDbType.Int)
                {
                    Value =(timeSheet.MTechnician == null ? DBNull.Value : timeSheet.MTechnician)
                },
                new SqlParameter("@WDate", SqlDbType.DateTime)
                {
                    Value =  timeSheet.WDate

                },
                 new SqlParameter("@TimeTo", SqlDbType.DateTime)
                {
                    Value =  (timeSheet.TimeTo == null ? DBNull.Value : timeSheet.TimeTo)

                },
                  new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier)
                {
                    Value =  timeSheet.ModifiedBy

                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateHdr_SeqnoQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public DataTable GetBlockRoomByWorkOrderIDDatatable(int v)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MWorkOrderNo", SqlDbType.Int)
                {
                    Value = v
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

        public int UpdateStatus(List<BlockRoom> listRoom)
        {
            int intRowAffected = 0;
            foreach (BlockRoom blockroom in listRoom)
            {
                intRowAffected = UpdateBlockroomStatus(blockroom);
            }
            return intRowAffected;
        }

        private int UpdateBlockroomStatus(BlockRoom room)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            int Activestatus = Convert.ToInt32(room.Active);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@RoomBlockKey", SqlDbType.UniqueIdentifier)
                {
                    Value = room.Roomblockkey
                },
                new SqlParameter("@Active", SqlDbType.Int)
                {
                    Value = room.Active
                },
                new SqlParameter("@BlockStaff", SqlDbType.VarChar)
                {
                    Value = Activestatus >= 1?(room.Blockstaff==null?DBNull.Value:room.Blockstaff):DBNull.Value
                },
                 new SqlParameter("@BlockTime", SqlDbType.DateTime)
                {
                    Value =Activestatus >= 1?(room.Blocktime==null?DBNull.Value:room.Blocktime):DBNull.Value
                },
                 new SqlParameter("@UnblockStaff", SqlDbType.VarChar)
                {
                    Value = Activestatus < 1?(room.Unblockstaff==null?DBNull.Value:room.Unblockstaff):DBNull.Value
                },
                 new SqlParameter("@UnblockTime", SqlDbType.DateTime)
                {
                    Value = Activestatus < 1?(room.Unblocktime==null?DBNull.Value:room.Unblocktime):DBNull.Value
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = room.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateStatusQuery(Activestatus), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        public int UpdatWorkOrder(MWorkOrderInput work)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            int intWOStatus = Convert.ToInt32(work.MWorkOrderStatus);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = work.Id
                },
                new SqlParameter("@MWorkOrderKey", SqlDbType.UniqueIdentifier)
                {
                    Value = work.MWorkOrderKey
                },
                new SqlParameter("@Room", SqlDbType.VarChar)
                {
                    Value = (work.Room == null ? DBNull.Value : work.Room)
                },
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value =(work.RoomKey == null ? DBNull.Value : work.RoomKey)
                },
                new SqlParameter("@MArea", SqlDbType.Int)
                {
                    Value =  (work.MArea == null ? DBNull.Value : work.MArea)
                },
                new SqlParameter("@MWorkType", SqlDbType.Int)
                {
                    Value = (work.MWorkType == null ?  DBNull.Value : work.MWorkType)
                },
                new SqlParameter("@MWorkOrderStatus", SqlDbType.Int)
                {
                    Value =  (work.MWorkOrderStatus == null ? DBNull.Value : work.MWorkOrderStatus)
                },
                new SqlParameter("@MTechnician", SqlDbType.Int)
                {
                    Value = (work.MTechnician == null ? DBNull.Value : work.MTechnician)
                },
                new SqlParameter("@Description", SqlDbType.NVarChar)
                {
                    Value = (work.Description == null ? DBNull.Value : work.Description)
                },
                new SqlParameter("@Notes", SqlDbType.NVarChar)
                {
                    Value =(work.Notes == null ? DBNull.Value : work.Notes)
                },
                new SqlParameter("@ScheduledFrom", SqlDbType.DateTime)
                {
                    Value = (work.ScheduledFrom == null ? DBNull.Value : work.ScheduledFrom)
                },
                new SqlParameter("@ScheduledTo", SqlDbType.DateTime)
                {
                    Value = (work.ScheduledTo == null ? DBNull.Value : work.ScheduledTo)
                },
                new SqlParameter("@SignedOff", SqlDbType.NVarChar)
                {
                    Value =(work.SignedOff == null ? DBNull.Value : work.SignedOff)
                },
                new SqlParameter("@Cancelled", SqlDbType.NVarChar)
                {
                    Value =(work.Cancelled == null ? DBNull.Value : work.Cancelled)
                },
                new SqlParameter("@StaffName", SqlDbType.NVarChar)
                {
                    Value =work.StaffName
                },
                new SqlParameter("@ReportedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = work.ReportedBy
                },
                new SqlParameter("@ReportedOn", SqlDbType.DateTime)
                {
                    Value = work.ReportedOn
                },
                new SqlParameter("@Priority", SqlDbType.Int)
                {
                    Value = (work.Priority == null ? DBNull.Value : work.Priority)
                },
                new SqlParameter("@LastUpdateBy", SqlDbType.NVarChar)
                {
                    Value = work.LastUpdateBy
                },
                new SqlParameter("@LastUpdateStaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value = work.LastUpdateStaffKey
                },
                new SqlParameter("@CompletedBy", SqlDbType.NVarChar)
                {
                    Value = intWOStatus== 3?(work.LastUpdateBy==null?DBNull.Value:work.LastUpdateBy):DBNull.Value
                },
                new SqlParameter("@CompletedStaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value = intWOStatus== 3?(work.LastUpdateStaffKey==null?DBNull.Value:work.LastUpdateStaffKey):DBNull.Value
                },
                new SqlParameter("@SignedOffBy", SqlDbType.NVarChar)
                {
                    Value = intWOStatus== 4?(work.LastUpdateBy==null?DBNull.Value:work.LastUpdateBy):DBNull.Value
                },
                new SqlParameter("@SignedOffStaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value = intWOStatus== 4?(work.LastUpdateStaffKey==null?DBNull.Value:work.LastUpdateStaffKey):DBNull.Value
                },
                new SqlParameter("@CancelledBy", SqlDbType.NVarChar)
                {
                    Value = intWOStatus== 5?(work.LastUpdateBy==null?DBNull.Value:work.LastUpdateBy):DBNull.Value
                },
                new SqlParameter("@CancelledStaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value = intWOStatus== 5?(work.LastUpdateStaffKey==null?DBNull.Value:work.LastUpdateStaffKey):DBNull.Value
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = work.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateWorkOrderQuery(intWOStatus), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        public string GetMaidStatusByRoomKey(Guid key)
        {
            string intRowAffected = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value = key
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetMaidStatusByRoomKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    intRowAffected = obj.ToString();
                }
            }
            return intRowAffected;
        }
        public DataTable GetWorkTimeSheetByID(int seqno)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = seqno
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetWorkTimeSheetByIDQuery(), CommandType.Text, MultiTenancySide, parameters))
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
        public int UpdateTimesheet(MWorkTimeSheetInput timeSheet)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value =  timeSheet.Seqno
                },
                new SqlParameter("@MTechnician", SqlDbType.Int)
                {
                    Value =(timeSheet.MTechnician == null ? DBNull.Value : timeSheet.MTechnician)
                },
                new SqlParameter("@WDate", SqlDbType.DateTime)
                {
                    Value =  timeSheet.WDate

                },
                 new SqlParameter("@TimeFrom", SqlDbType.DateTime)
                {
                    Value =  (timeSheet.TimeFrom == null ? DBNull.Value : timeSheet.TimeFrom)

                },
                  new SqlParameter("@TimeTo", SqlDbType.DateTime)
                {
                    Value =  (timeSheet.TimeTo == null ? DBNull.Value : timeSheet.TimeTo)

                },
                   new SqlParameter("@Notes", SqlDbType.NVarChar)
                {
                    Value =  timeSheet.Notes

                },
                    new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier)
                {
                    Value =  timeSheet.ModifiedBy

                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
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


        public List<DDLAreaOutput> GetAllArea()
        {
            var lst = new List<DDLAreaOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetMAreaQuery(), CommandType.Text, MultiTenancySide))
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
                    DDLAreaOutput o = new DDLAreaOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Description = !DBNull.Value.Equals(dr["Description"]) ? dr["Description"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }

        public List<DDLWorkTypeOutput> GetAllWorkType()
        {
            var lst = new List<DDLWorkTypeOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetMWorkTypeQuery(), CommandType.Text, MultiTenancySide))
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
                    DDLWorkTypeOutput o = new DDLWorkTypeOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Description = !DBNull.Value.Equals(dr["Description"]) ? dr["Description"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }

        public List<MaidOutput> GetAllReportedBy()
        {
            var lst = new List<MaidOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetAllStaffQuery(), CommandType.Text, MultiTenancySide))
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

        public List<DDLWorkStatusOutput> GetAllCurrentStatus()
        {
            var lst = new List<DDLWorkStatusOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetMWorkOrderStatusQuery(), CommandType.Text, MultiTenancySide))
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
                    DDLWorkStatusOutput o = new DDLWorkStatusOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? dr["Seqno"].ToString() : "";
                    o.Description = !DBNull.Value.Equals(dr["Description"]) ? dr["Description"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }

        public List<DDPriorityOutput> GetAllPriority()
        {
            var lst = new List<DDPriorityOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetMPriority(), CommandType.Text, MultiTenancySide))
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
                    DDPriorityOutput o = new DDPriorityOutput();
                    o.Sort = !DBNull.Value.Equals(dr["Sort"]) ? Convert.ToInt32(dr["Sort"]) : 0;
                    o.Priority = !DBNull.Value.Equals(dr["Priority"]) ? dr["Priority"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }

        public List<DDLTechnicianOutput> GetAllTechnician()
        {
            var lst = new List<DDLTechnicianOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetMTechnicianQuery(), CommandType.Text, MultiTenancySide))
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
                    DDLTechnicianOutput o = new DDLTechnicianOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? dr["Seqno"].ToString() : "";
                    o.Name = !DBNull.Value.Equals(dr["Name"]) ? dr["Name"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }

        public string GetAreaByKey(int key)
        {
            string intRowAffected = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = key
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetAreaByKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    intRowAffected = obj.ToString();
                }
            }
            return intRowAffected;
        }

        public string GetWorkTypeByKey(int key)
        {
            string intRowAffected = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = key
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetWorkTypeByKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    intRowAffected = obj.ToString();
                }
            }
            return intRowAffected;
        }

        public string GetRoomByKey(Guid key)
        {
            string intRowAffected = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                {
                    Value = key
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetRoomByKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    intRowAffected = obj.ToString();
                }
            }
            return intRowAffected;
        }


        public string GetWorkStatusByKey(int seqno)
        {
            string intRowAffected = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = seqno
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetWorkStatusByKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    intRowAffected = obj.ToString();
                }
            }
            return intRowAffected;
        }

        public string GetReportedName(Guid id)
        {
            string intRowAffected = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@StaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value = id
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetReportNameQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    intRowAffected = obj.ToString();
                }
            }
            return intRowAffected;
        }

        public string GetPriorityName(int sort)
        {
            string intRowAffected = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Sort", SqlDbType.Int)
                {
                    Value = sort
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetPriorityName(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    intRowAffected = obj.ToString();
                }
            }
            return intRowAffected;
        }

        public Guid GetTechnicianKey(Guid id)
        {
            Guid intRowAffected = Guid.Empty;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@StaffKey", SqlDbType.UniqueIdentifier)
                {
                    Value = id
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetTechnicianKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    intRowAffected = Guid.Parse(obj.ToString());
                }
            }
            return intRowAffected;
        }

        public int GetTechnicalID(Guid technicianKey)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@TechnicianKey", SqlDbType.UniqueIdentifier)
                {
                    Value = technicianKey
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetTechnicalIDQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    intRowAffected = Convert.ToInt32(obj.ToString());
                }
            }
            return intRowAffected;
        }
        public List<DDLNoteTemplateOutput> GetAllNoteTemplate()
        {
            var lst = new List<DDLNoteTemplateOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetMWorkNoteTemplateQuery(), CommandType.Text, MultiTenancySide))
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
                    DDLNoteTemplateOutput o = new DDLNoteTemplateOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? dr["Seqno"].ToString() : "";
                    o.Description = !DBNull.Value.Equals(dr["Description"]) ? dr["Description"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }

        public DataTable GetDocumentByWoKey(Guid id)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetDocumentByWoKeyQuery(id), CommandType.Text, MultiTenancySide))
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

        public int CheckWoImage(WOImage image)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {

                new SqlParameter("@Sort", SqlDbType.Int)
                {
                     Value =  image.Sort
                },
                new SqlParameter("@MWorkOrderKey", SqlDbType.UniqueIdentifier)
                {
                     Value = image.MWorkOrderKey
                }

           };
            using (var command = _connectionManager.CreateCommandSP(CheckWoImageQuery(), CommandType.Text, MultiTenancySide, parameters))
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
        public int InsertWoImage(WOImage image)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@DocumentKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  image.DocumentKey
                 },
                new SqlParameter("@Sort", SqlDbType.Int)
                {
                     Value =  image.Sort
                 },
                new SqlParameter("@LastModifiedStaff", SqlDbType.UniqueIdentifier)
                {
                     Value =  image.LastModifiedStaff
                 },
                new SqlParameter("@Document", SqlDbType.VarChar)
                {
                    Value = image.DocumentName
                },
                 new SqlParameter("@Description", SqlDbType.VarChar)
                 {
                     Value = image.Description
                 },
                 new SqlParameter("@MWorkOrderKey", SqlDbType.UniqueIdentifier)
                 {
                     Value = image.MWorkOrderKey
                 },
                 new SqlParameter("@Signature", SqlDbType.VarBinary)
                 {
                     Value = image.Signature
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(InsertWoImageQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        public int UpdateWoImage(WOImage image)
        {


            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@DocumentKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  image.DocumentKey
                 },
                new SqlParameter("@Sort", SqlDbType.Int)
                {
                     Value =  image.Sort
                 },
                new SqlParameter("@LastModifiedStaff", SqlDbType.UniqueIdentifier)
                {
                     Value =  image.LastModifiedStaff
                 },
                new SqlParameter("@Document", SqlDbType.VarChar)
                {
                    Value = image.DocumentName
                },
                 new SqlParameter("@Description", SqlDbType.VarChar)
                 {
                     Value = image.Description
                 },
                 new SqlParameter("@MWorkOrderKey", SqlDbType.UniqueIdentifier)
                 {
                     Value = image.MWorkOrderKey
                 },
                 new SqlParameter("@Signature", SqlDbType.VarBinary)
                 {
                     Value = image.Signature
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(UpdateWoImageQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }

            return intRowAffected;
        }

        public int MRCheckExit(Guid id, Guid? maidStatusKey)
        {

            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@RoomKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  id
                 },
                new SqlParameter("@MaidStatusKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  maidStatusKey
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(CheckMRRoomQuery(), CommandType.Text, MultiTenancySide, parameters))
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
        #endregion

        #region sqlquery
        private static string CheckMRRoomQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select [Unit] from Room ");
            sb.Append(" where RoomKey=@RoomKey and MaidStatusKey=@MaidStatusKey ");
            return sb.ToString();
        }
        private static string CheckWoImageQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select [sort] from Document ");
            sb.Append(" where Sort=@Sort and MWorkOrderKey=@MWorkOrderKey ");
            return sb.ToString();
        }
        private static string InsertWoImageQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert into Document ([DocumentKey],[Sort],[LastModifiedStaff],[Document],[Description],[MWorkOrderKey],[Signature]) ");
            sb.Append(" Values (@DocumentKey,@Sort,@LastModifiedStaff,@Document, @Description, @MWorkOrderKey,@Signature) ");
            return sb.ToString();
        }
        private static string UpdateWoImageQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Update Document set [LastModifiedStaff]=@LastModifiedStaff,[Document]=@Document,[Description]=@Description,[Signature]=@Signature ");
            sb.Append(" where Sort=@Sort and MWorkOrderKey=@MWorkOrderKey");
            return sb.ToString();
        }
        private static string GetDocumentByWoKeyQuery(Guid id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select [sort],[Description],[Document],[Signature] from Document ");
            sb.Append(" where MWorkOrderKey='" + id + "'");
            return sb.ToString();
        }
        private static string GetMWorkNoteTemplateQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  Seqno, Description ");
            sb.Append(" FROM    MWorkTimeSheetNoteTemplate ");
            sb.Append(" WHERE   Active = 1 ");
            sb.Append(" ORDER BY  Description ;");
            return sb.ToString();
        }
        private static string GetTechnicalIDQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT Seqno ");
            sb.Append(" FROM  MTechnician where TechnicianKey = @TechnicianKey ");
            return sb.ToString();
        }
        private static string GetTechnicianKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT TechnicianKey ");
            sb.Append(" FROM  Staff where StaffKey = @StaffKey ");
            return sb.ToString();
        }
        private static string GetPriorityName()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT Priority ");
            sb.Append(" FROM  Mpriority where Sort = @Sort ");
            return sb.ToString();
        }
        private static string GetReportNameQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT UserName ");
            sb.Append(" FROM  Staff where StaffKey = @StaffKey ");
            return sb.ToString();
        }
        private static string GetWorkStatusByKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT Description ");
            sb.Append(" FROM  MWorkOrderStatus where Seqno = @Seqno ");
            return sb.ToString();
        }
        private static string GetRoomByKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT Unit ");
            sb.Append(" FROM  Room where RoomKey = @RoomKey ");
            return sb.ToString();
        }
        private static string GetWorkTypeByKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT Description ");
            sb.Append(" FROM  MWorkType where Seqno = @Seqno ");
            return sb.ToString();
        }
        private static string GetAreaByKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT Description ");
            sb.Append(" FROM  MArea where Seqno = @Seqno ");
            return sb.ToString();
        }
        private static string GetMTechnicianQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MTechnician ");
            sb.Append(" WHERE   Active = 1 ");
            sb.Append(" ORDER BY  Name ;");
            return sb.ToString();
        }
        private static string GetMPriority()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT Sort,Priority ");
            sb.Append(" FROM MPriority where Active = 1 ORDER BY  Priority;");
            return sb.ToString();
        }
        private static string GetMWorkOrderStatusQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT Seqno,Description ");
            sb.Append(" FROM MWorkOrderStatus ");
            sb.Append(" WHERE Active = 1 and Description!='' and Description is not null ");
            sb.Append(" ORDER BY Description ;");
            return sb.ToString();
        }
        private static string GetAllStaffQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  StaffKey, UserName FROM  Staff ");
            sb.Append(" WHERE   Active = 1  ");
            sb.Append(" ORDER BY  UserName ; ");
            return sb.ToString();
        }
        private static string GetMWorkTypeQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  Seqno, Description ");
            sb.Append(" FROM    MWorkType ");
            sb.Append(" WHERE   Active = 1 ");
            sb.Append(" ORDER BY  Description ;");
            return sb.ToString();
        }
        private static string GetMAreaQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  Seqno, Description ");
            sb.Append(" FROM    MArea ");
            sb.Append(" WHERE   Active = 1 ");
            sb.Append(" ORDER BY  Description ;");
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

        private static string GetUpdateQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE MWorkTimeSheet ");
            sb.Append(" SET   ");
            sb.Append("   MTechnician = @MTechnician , ");
            sb.Append("   WDate = @WDate , TimeFrom = @TimeFrom , TimeTo = @TimeTo , ");
            sb.Append("   Notes = @Notes , ModifiedBy = @ModifiedBy , ModifiedOn = GETDATE() ");
            sb.Append(" WHERE   ");
            sb.Append("    Seqno = @Seqno ;");
            return sb.ToString();
        }
        private static string GetWorkTimeSheetByIDQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Select ts.* , tech.Name  ");
            sb.Append(" FROM MWorkTimeSheet ts ");
            sb.Append(" LEFT JOIN  MTechnician  tech  ON tech.Seqno = ts.MTechnician   ");
            sb.Append(" WHERE  ts.Seqno = @Seqno ; ");
            return sb.ToString();
        }
        private static string GetMaidStatusByRoomKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT MaidStatus ");
            sb.Append(" FROM MaidStatus where MaidStatusKey = (select MaidStatusKey from Room where RoomKey = @RoomKey) ");
            return sb.ToString();
        }
        private static string GetUpdateWorkOrderQuery(int woStatus)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE MWorkOrder  ");
            sb.Append(" SET    ");
            sb.Append("   Room = @Room, MWorkOrderKey = @MWorkOrderKey, RoomKey = @RoomKey, MArea = @MArea, MWorkType = @MWorkType, MWorkOrderStatus = @MWorkOrderStatus, MTechnician = @MTechnician, ");
            sb.Append("   Description = @Description, Notes = @Notes, ScheduledFrom = @ScheduledFrom, ScheduledTo = @ScheduledTo, ");
            sb.Append("   SignedOff = @SignedOff, Cancelled = @Cancelled, Priority = @Priority, TenantId=@TenantId,");
            //sb.Append("   EnteredBy = @EnteredBy, EnteredStaffKey = @EnteredStaffKey, EnteredDateTime = @EnteredDateTime, ");

            if (woStatus == 3)
            {
                sb.Append("     CompletedBy = @CompletedBy , CompletedStaffKey = @CompletedStaffKey , CompletedDateTime = GETDATE()  ,  ");
            }
            else if (woStatus == 4)
            {
                sb.Append("     SignedOffBy = @SignedOffBy , SignedOffStaffKey = @SignedOffStaffKey , SignedOffDateTime = GETDATE()  ,  ");
            }
            else if (woStatus == 5)
            {
                sb.Append("     CancelledBy = @CancelledBy , CancelledStaffKey = @CancelledStaffKey , CancelledDateTime = GETDATE() ,  ");
            }

            //sb.Append("   CompletedBy = @CompletedBy, CompletedStaffKey = @CompletedStaffKey, CompletedDateTime = @CompletedDateTime, ");
            //sb.Append("   SignedOffBy = @SignedOffBy, SignedOffStaffKey = @SignedOffStaffKey, SignedOffDateTime = @SignedOffDateTime, ");
            //sb.Append("   CancelledBy = @CancelledBy, CancelledStaffKey = @CancelledStaffKey, CancelledDateTime = @CancelledDateTime, ");
            sb.Append("   StaffName = @StaffName, ReportedBy = @ReportedBy , ReportedOn = @ReportedOn, ");
            sb.Append("   LastUpdateBy = @LastUpdateBy , LastUpdateStaffKey = @LastUpdateStaffKey , LastUpdateDateTime = GETDATE()   ");
            sb.Append(" WHERE  Seqno = @Seqno  ; ");
            return sb.ToString();
        }
        private static string GetUpdateStatusQuery(int active)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Update RoomBlock ");
            builder.Append(" SET  ");
            builder.Append("  Active = @Active ");
            builder.Append("  ,TenantId = @TenantId ");
            if (active >= 1)
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
        private static string GetUpdateHdr_SeqnoQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE MWorkTimeSheet ");
            sb.Append(" SET   ");
            sb.Append("   MTechnician = @MTechnician , ");
            sb.Append("   WDate = @WDate , TimeTo = @TimeTo , ");
            sb.Append("   ModifiedBy = @ModifiedBy , ModifiedOn = GETDATE() ");
            sb.Append(" WHERE   ");
            sb.Append("    Seqno = @Seqno ;");
            return sb.ToString();
        }
        private static string GetWorkTimeSheetByHdr_SeqnoQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Select top 1 ts.* , tech.Name  ");
            sb.Append(" FROM MWorkTimeSheet ts ");
            sb.Append(" LEFT JOIN  MTechnician  tech  ON tech.Seqno = ts.MTechnician   ");
            sb.Append(" WHERE  ts.Hdr_Seqno = @Seqno order by Seqno desc ; ");
            return sb.ToString();
        }
        private static string GetMaidStatusByStatusQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT * ");
            sb.Append(" FROM  MaidStatus ");
            sb.Append(" WHERE MaidStatus = @MaidStatus ;");
            return sb.ToString();
        }

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

        private static string GetInsertTimesheetQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO MWorkTimeSheet ");
            sb.Append(" (Hdr_Seqno, MTechnician, WDate, TimeFrom, TimeTo, Notes, CreatedBy, CreatedOn ,TenantId) ");
            sb.Append(" VALUES ");
            sb.Append(" ( @Hdr_Seqno, @MTechnician, @WDate, @TimeFrom, @TimeTo, @Notes, @CreatedBy, GETDATE(),@TenantId ) ; ");
            return sb.ToString();
        }
        private static string GetMTechnicianBySeqnoQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  Name ");
            sb.Append(" FROM  MTechnician where Seqno = @Seqno ");
            sb.Append(" ORDER BY  Name ;");
            return sb.ToString();
        }
        private static string GetUpdateWorkOrderStatusQuery(int woStatus)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE MWorkOrder  ");
            sb.Append(" SET   MWorkOrderStatus = @MWorkOrderStatus , MWorkOrderKey = @MWorkOrderKey, ");
            if (woStatus == 3)
            {
                sb.Append("     CompletedBy = @CompletedBy , CompletedStaffKey = @CompletedStaffKey , CompletedDateTime = GETDATE()  ,  ");
            }
            else if (woStatus == 4)
            {
                sb.Append("     SignedOffBy = @SignedOffBy , SignedOffStaffKey = @SignedOffStaffKey , SignedOffDateTime = GETDATE()  ,  ");
            }
            else if (woStatus == 5)
            {
                sb.Append("     CancelledBy = @CancelledBy , CancelledStaffKey = @CancelledStaffKey , CancelledDateTime = GETDATE() ,  ");
            }
            sb.Append("       LastUpdateBy = @LastUpdateBy , LastUpdateStaffKey = @LastUpdateStaffKey , LastUpdateDateTime = GETDATE()   ");
            sb.Append(" WHERE  Seqno = @Seqno  ; ");
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

        private static string GetAssignTechnicianQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE MWorkOrder  ");
            sb.Append(" SET   MTechnician = @MTechnician , LastUpdateBy = @LastUpdateBy , LastUpdateStaffKey = @LastUpdateStaffKey , LastUpdateDateTime = GETDATE() ,TenantId=@TenantId  ");
            sb.Append(" WHERE  Seqno = @Seqno  ; ");
            return sb.ToString();
        }
        private static string GetWorkOrderByStatusQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  wo.* , a.Description AS Area, wt.Description AS WorkType, wos.Description AS WorkOrderStatus, tech.Name  ");
            sb.Append(" FROM    MWorkOrder wo");
            sb.Append(" LEFT JOIN MArea a  ON a.Seqno = wo.MArea  ");
            sb.Append(" LEFT JOIN MWorkType wt  ON wt.Seqno = wo.MWorkType  ");
            sb.Append(" LEFT JOIN MWorkOrderStatus wos  ON wos.Seqno = wo.MWorkOrderStatus    ");
            sb.Append(" LEFT JOIN MTechnician tech  ON tech.Seqno = wo.MTechnician      ");
            sb.Append(" WHERE  MWorkOrderStatus = @MWorkOrderStatus   ");
            sb.Append(" ORDER BY wo.Seqno DESC   ; ");
            return sb.ToString();
        }
        private static string GetUnassignedTechWorkOrderQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  wo.* , a.Description AS Area, wt.Description AS WorkType, wos.Description AS WorkOrderStatus, tech.Name  ");
            sb.Append(" FROM    MWorkOrder wo");
            sb.Append(" LEFT JOIN MArea a  ON a.Seqno = wo.MArea  ");
            sb.Append(" LEFT JOIN MWorkType wt  ON wt.Seqno = wo.MWorkType  ");
            sb.Append(" LEFT JOIN MWorkOrderStatus wos  ON wos.Seqno = wo.MWorkOrderStatus    ");
            sb.Append(" LEFT JOIN MTechnician tech  ON tech.Seqno = wo.MTechnician      ");
            sb.Append(" WHERE  MTechnician IS NULL   ");
            sb.Append(" ORDER BY wo.Seqno DESC ; ");
            return sb.ToString();
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
        private static string GetWorkNotesQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MWorkNotes ");
            sb.Append(" WHERE    MWorkOrderKey = @woKey ");
            return sb.ToString();
        }
        private static string GetWorkOrderByIDQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MWorkOrder ");
            sb.Append(" WHERE   Seqno = @Seqno ; ");
            return sb.ToString();
        }
        private static string GetTodayHistoryQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT [HistoryKey],[ChangedDate],[Detail],[TableName],[OldValue],[NewValue],[UserName] ");
            sb.Append(" FROM (SELECT [HistoryKey],[ChangedDate],[Detail],[TableName],[OldValue],[NewValue],[StaffKey] ");
            sb.Append(" FROM History WHERE ModuleName = 'iRepair' AND SourceKey = @woKey AND TenantId=1 ) h ");
            sb.Append(" INNER JOIN Staff s ON h.StaffKey = s.StaffKey ORDER BY  Changeddate DESC; ");
            return sb.ToString();
        }
        private static string GetWorkTimeSheetByWOIDQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Select ts.* , tech.Name  ");
            sb.Append(" FROM MWorkTimeSheet ts ");
            sb.Append(" LEFT JOIN MTechnician tech ON tech.Seqno = ts.MTechnician  ");
            sb.Append(" WHERE  ts.Hdr_Seqno = @Hdr_Seqno order by TimeFrom desc; ");
            return sb.ToString();
        }
        private static string GetRoomCountByMaidKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select top 1 * from MWorkTimeSheet where Hdr_Seqno = @Seqno order by Seqno desc  ");
            return sb.ToString();
        }

        #endregion
    }
}
