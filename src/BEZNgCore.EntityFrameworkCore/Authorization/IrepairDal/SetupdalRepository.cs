using Abp.Data;
using Abp.EntityFrameworkCore;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.EntityFrameworkCore;
using BEZNgCore.EntityFrameworkCore.Repositories;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.Authorization.IrepairDal
{
      public class SetupdalRepository : BEZNgCoreRepositoryBase<MWorkType, int>, ISetupdalRepository
    {
        private readonly IActiveTransactionProvider _transactionProvider;
        ConnectionManager _connectionManager;
        public SetupdalRepository(IDbContextProvider<BEZNgCoreDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
            _connectionManager = new ConnectionManager(dbContextProvider, _transactionProvider);
        }
        #region interfaceimplement & related function 
        public List<SetupTabOutput> GetActiveMArea()
        {
            var lst = new List<SetupTabOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetActiveMAreaQuery(), CommandType.Text, MultiTenancySide))
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
            if(dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SetupTabOutput o = new SetupTabOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Description = (!DBNull.Value.Equals(dr["Description"])) ? (!string.IsNullOrEmpty(dr["Description"].ToString()) ? dr["Description"].ToString() : "") : "";
                    o.ActiveStatus = GetActiveStatusValue(!DBNull.Value.Equals(dr["Active"]) ? Convert.ToInt32(dr["Active"]) : 0);

                    lst.Add(o);
                }
            }
            return lst;
        }

        public List<MTechnicianOutput> GetActiveMTechnician()
        {
            var lst = new List<MTechnicianOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetActiveMTechnicianQuery(), CommandType.Text, MultiTenancySide))
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
            if(dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MTechnicianOutput o = new MTechnicianOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Name = (!DBNull.Value.Equals(dr["Name"])) ? (!string.IsNullOrEmpty(dr["Name"].ToString()) ? dr["Name"].ToString() : "") : "";
                    o.MPhone = (!DBNull.Value.Equals(dr["MPhone"])) ? (!string.IsNullOrEmpty(dr["MPhone"].ToString()) ? dr["MPhone"].ToString() : "-") : "-";
                    o.Email = (!DBNull.Value.Equals(dr["Email"])) ? (!string.IsNullOrEmpty(dr["Email"].ToString()) ? dr["Email"].ToString() : "-") : "-";
                    o.CompanyName = (!DBNull.Value.Equals(dr["CompanyName"])) ? (!string.IsNullOrEmpty(dr["CompanyName"].ToString()) ? dr["CompanyName"].ToString() : "-") : "-";
                    o.ContractorStatus = GetContractorStatusValue((!DBNull.Value.Equals(dr["Contractor"])) ? (!string.IsNullOrEmpty(dr["Contractor"].ToString()) ? dr["Contractor"].ToString() : "") : "");
                    o.OPhone = (!DBNull.Value.Equals(dr["OPhone"])) ? (!string.IsNullOrEmpty(dr["OPhone"].ToString()) ? dr["OPhone"].ToString() : "-") : "-";
                    o.Fax = (!DBNull.Value.Equals(dr["Fax"])) ? (!string.IsNullOrEmpty(dr["Fax"].ToString()) ? dr["Fax"].ToString() : "-") : "-";
                    o.ActiveStatus = GetActiveStatusValue(!DBNull.Value.Equals(dr["Active"]) ? Convert.ToInt32(dr["Active"]) : 0);
                    o.Note = (!DBNull.Value.Equals(dr["Note"])) ? (!string.IsNullOrEmpty(dr["Note"].ToString()) ? dr["Note"].ToString() : "-") : "-";
                    lst.Add(o);
                }
            }
            return lst;
        }

        public List<SetupTabOutput> GetActiveMWorkTimeSheetNoteTemplate()
        {
            var lst = new List<SetupTabOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetActiveMWorkNoteTemplateQuery(), CommandType.Text, MultiTenancySide))
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
            if(dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SetupTabOutput o = new SetupTabOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Description = (!DBNull.Value.Equals(dr["Description"])) ? (!string.IsNullOrEmpty(dr["Description"].ToString()) ? dr["Description"].ToString() : "") : "";
                    o.ActiveStatus = GetActiveStatusValue(!DBNull.Value.Equals(dr["Active"]) ? Convert.ToInt32(dr["Active"]) : 0);

                    lst.Add(o);
                }
            }
            return lst;
        }

        public List<SetupTabOutput> GetActiveMWorkType()
        {
            var lst = new List<SetupTabOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetActiveMWorkTypeQuery(), CommandType.Text, MultiTenancySide))
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
            if(dt.Rows.Count>0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SetupTabOutput o = new SetupTabOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Description = (!DBNull.Value.Equals(dr["Description"])) ? (!string.IsNullOrEmpty(dr["Description"].ToString()) ? dr["Description"].ToString() : "") : "";
                    o.ActiveStatus = GetActiveStatusValue(!DBNull.Value.Equals(dr["Active"]) ? Convert.ToInt32(dr["Active"]) : 0);

                    lst.Add(o);
                }
            }
            return lst;
        }
        public List<ExternalAccessOutput> GetExternalAccess()
        {
            var lst = new List<ExternalAccessOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetExternalAccessQuery(), CommandType.Text, MultiTenancySide))
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
                    ExternalAccessOutput o = new ExternalAccessOutput();
                    o.ExternalAccessKey = (!DBNull.Value.Equals(dr["ExternalAccessKey"])) ? (!string.IsNullOrEmpty(dr["ExternalAccessKey"].ToString()) ? new Guid(dr["ExternalAccessKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.Start = (!DBNull.Value.Equals(dr["Start"])) ? (!string.IsNullOrEmpty(dr["Start"].ToString()) ? dr["Start"].ToString() : "") : "";
                    o.END = (!DBNull.Value.Equals(dr["END"])) ? (!string.IsNullOrEmpty(dr["END"].ToString()) ? dr["END"].ToString() : "") : "";
             
                    lst.Add(o);
                }
            }
            return lst;
        }
        public List<MPriorityOutput> GetActivePriority()
        {
            var lst = new List<MPriorityOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetActiveMPriority(), CommandType.Text, MultiTenancySide))
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
                    MPriorityOutput o = new MPriorityOutput();
                    o.PriorityID = !DBNull.Value.Equals(dr["PriorityID"]) ? Convert.ToInt32(dr["PriorityID"]) : 0;
                    o.Priority = (!DBNull.Value.Equals(dr["Priority"])) ? (!string.IsNullOrEmpty(dr["Priority"].ToString()) ? dr["Priority"].ToString() : "") : "";
                    o.Sort = !DBNull.Value.Equals(dr["Sort"]) ? Convert.ToInt32(dr["Sort"]) : 0;
                    o.ActiveStatus = GetActiveStatusValue(!DBNull.Value.Equals(dr["Active"]) ? Convert.ToInt32(dr["Active"]) : 0);
                    lst.Add(o);
                }
            }
            return lst;
        }
        public List<SetupTabOutput> GetActiveWorkOrderStatus()
        {
            var lst = new List<SetupTabOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetActiveMWorkOrderStatusQuery(), CommandType.Text, MultiTenancySide))
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
            if(dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SetupTabOutput o = new SetupTabOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Description = (!DBNull.Value.Equals(dr["Description"])) ? (!string.IsNullOrEmpty(dr["Description"].ToString()) ? dr["Description"].ToString() : "") : "";
                    o.ActiveStatus = GetActiveStatusValue(!DBNull.Value.Equals(dr["Active"]) ? Convert.ToInt32(dr["Active"]) : 0);

                    lst.Add(o);
                }
            }
            return lst;
        }

        public List<SetupTabOutput> GetAllMArea()
        {
            var lst = new List<SetupTabOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetAllMAreaQuery(), CommandType.Text, MultiTenancySide))
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
            if(dt.Rows.Count>0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SetupTabOutput o = new SetupTabOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Description = (!DBNull.Value.Equals(dr["Description"])) ? (!string.IsNullOrEmpty(dr["Description"].ToString()) ? dr["Description"].ToString() : "") : "";
                    o.ActiveStatus = GetActiveStatusValue(!DBNull.Value.Equals(dr["Active"]) ? Convert.ToInt32(dr["Active"]) : 0);


                    lst.Add(o);
                }
            }
            return lst;
        }

        public List<MTechnicianOutput> GetAllMTechnician()
        {
            var lst = new List<MTechnicianOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetAllMTechnicianQuery(), CommandType.Text, MultiTenancySide))
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
            if(dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MTechnicianOutput o = new MTechnicianOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Name = (!DBNull.Value.Equals(dr["Name"])) ? (!string.IsNullOrEmpty(dr["Name"].ToString()) ? dr["Name"].ToString() : "") : "";
                    o.MPhone = (!DBNull.Value.Equals(dr["MPhone"])) ? (!string.IsNullOrEmpty(dr["MPhone"].ToString()) ? dr["MPhone"].ToString() : "-") : "-";
                    o.Email = (!DBNull.Value.Equals(dr["Email"])) ? (!string.IsNullOrEmpty(dr["Email"].ToString()) ? dr["Email"].ToString() : "-") : "-";
                    o.CompanyName = (!DBNull.Value.Equals(dr["CompanyName"])) ? (!string.IsNullOrEmpty(dr["CompanyName"].ToString()) ? dr["CompanyName"].ToString() : "-") : "-";
                    o.ContractorStatus = GetContractorStatusValue((!DBNull.Value.Equals(dr["Contractor"])) ? (!string.IsNullOrEmpty(dr["Contractor"].ToString()) ? dr["Contractor"].ToString() : "") : "");
                    o.OPhone = (!DBNull.Value.Equals(dr["OPhone"])) ? (!string.IsNullOrEmpty(dr["OPhone"].ToString()) ? dr["OPhone"].ToString() : "-") : "-";
                    o.Fax = (!DBNull.Value.Equals(dr["Fax"])) ? (!string.IsNullOrEmpty(dr["Fax"].ToString()) ? dr["Fax"].ToString() : "-") : "-";
                    o.ActiveStatus = GetActiveStatusValue(!DBNull.Value.Equals(dr["Active"]) ? Convert.ToInt32(dr["Active"]) : 0);
                    o.Note = (!DBNull.Value.Equals(dr["Note"])) ? (!string.IsNullOrEmpty(dr["Note"].ToString()) ? dr["Note"].ToString() : "-") : "-";
                    lst.Add(o);
                }
            }
            return lst;
        }

        public List<SetupTabOutput> GetAllMWorkTimeSheetNoteTemplate()
        {
            var lst = new List<SetupTabOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetAllMWorkNoteTemplateQuery(), CommandType.Text, MultiTenancySide))
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
            if(dt.Rows.Count>0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SetupTabOutput o = new SetupTabOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Description = (!DBNull.Value.Equals(dr["Description"])) ? (!string.IsNullOrEmpty(dr["Description"].ToString()) ? dr["Description"].ToString() : "") : "";
                    o.ActiveStatus = GetActiveStatusValue(!DBNull.Value.Equals(dr["Active"]) ? Convert.ToInt32(dr["Active"]) : 0);


                    lst.Add(o);
                }
            }
            return lst;
        }

        public List<SetupTabOutput> GetAllMWorkType()
        {
            var lst = new List<SetupTabOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetAllMWorkTypeQuery(), CommandType.Text, MultiTenancySide))
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
            if(dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SetupTabOutput o = new SetupTabOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Description = (!DBNull.Value.Equals(dr["Description"])) ? (!string.IsNullOrEmpty(dr["Description"].ToString()) ? dr["Description"].ToString() : "") : "";
                    o.ActiveStatus = GetActiveStatusValue(!DBNull.Value.Equals(dr["Active"]) ? Convert.ToInt32(dr["Active"]) : 0);


                    lst.Add(o);
                }
            }
            return lst;
        }

        public List<MPriorityOutput> GetAllPriority()
        {
            var lst = new List<MPriorityOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetWorkNotesQuery(), CommandType.Text, MultiTenancySide))
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
            if(dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MPriorityOutput o = new MPriorityOutput();
                    o.PriorityID = !DBNull.Value.Equals(dr["PriorityID"]) ? Convert.ToInt32(dr["PriorityID"]) : 0;
                    o.Priority = (!DBNull.Value.Equals(dr["Priority"])) ? (!string.IsNullOrEmpty(dr["Priority"].ToString()) ? dr["Priority"].ToString() : "") : "";
                    o.Sort = !DBNull.Value.Equals(dr["Sort"]) ? Convert.ToInt32(dr["Sort"]) : 0;
                    o.ActiveStatus = GetActiveStatusValue(!DBNull.Value.Equals(dr["Active"]) ? Convert.ToInt32(dr["Active"]) : 0);
                    lst.Add(o);
                }
            }
            return lst;
        }

        public List<SetupTabOutput> GetAllWorkOrderStatus()
        {
            var lst = new List<SetupTabOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetAllMWorkOrderStatusQuery(), CommandType.Text, MultiTenancySide))
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
            if(dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SetupTabOutput o = new SetupTabOutput();
                    o.Seqno = !DBNull.Value.Equals(dr["Seqno"]) ? Convert.ToInt32(dr["Seqno"]) : 0;
                    o.Description = (!DBNull.Value.Equals(dr["Description"])) ? (!string.IsNullOrEmpty(dr["Description"].ToString()) ? dr["Description"].ToString() : "") : "";
                    o.ActiveStatus = GetActiveStatusValue(!DBNull.Value.Equals(dr["Active"]) ? Convert.ToInt32(dr["Active"]) : 0);


                    lst.Add(o);
                }
            }
            return lst;
        }
        public static bool GetActiveStatusValue(object status)
        {
            bool blnStatus = false;
            try
            {
                switch (status.ToString())
                {
                    case "0":
                        blnStatus = false;
                        break;
                    case "1":
                        blnStatus = true;
                        break;
                    default:
                        break;
                }
                return blnStatus;
            }
            catch
            {
                return blnStatus;
            }
        }
        public static bool GetContractorStatusValue(object status)
        {
            bool blnStatus = false;
            try
            {
                switch (status.ToString())
                {
                    case "N":
                        blnStatus = false;
                        break;
                    case "Y":
                        blnStatus = true;
                        break;
                    default:
                        break;
                }
                return blnStatus;
            }
            catch
            {
                return blnStatus;
            }
        }
        public int InsertMPriority(MPriorityModel priority)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Priority", SqlDbType.NVarChar)
                {
                    Value = priority.Priority
                },
                new SqlParameter("@Sort", SqlDbType.Int)
                {
                    Value = priority.Sort
                },
                 new SqlParameter("@Active", SqlDbType.Int)
                {
                    Value = priority.Active
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = priority.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetMPriorityQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int InsertMWorkOrderStatus(MWorkOrderStatus status)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Description", SqlDbType.NVarChar)
                {
                    Value = status.Description
                },
                new SqlParameter("@Active", SqlDbType.Int)
                {
                    Value = status.Active
                },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = status.CreatedBy
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = status.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetInsertMWorkOrderStatusQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int InsertMWorkTimeSheetNoteTemplate(MWorkTimeSheetNoteTemplate note)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Description", SqlDbType.NVarChar)
                {
                    Value = note.Description
                },
                new SqlParameter("@Active", SqlDbType.Int)
                {
                    Value = note.Active
                },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = note.CreatedBy
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = note.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetInserMWorkTimeSheetNoteTemplatetQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int InsertMArea(MArea workArea)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Description", SqlDbType.NVarChar)
                {
                    Value = workArea.Description
                },
                new SqlParameter("@Active", SqlDbType.Int)
                {
                    Value = workArea.Active
                },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = workArea.CreatedBy
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = workArea.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetInsertMAreaQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int InsertMWorkType(MWorkType workType)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@Description", SqlDbType.NVarChar)
                {
                    Value = workType.Description
                },
                new SqlParameter("@Active", SqlDbType.Int)
                {
                    Value = workType.Active
                },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = workType.CreatedBy
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = workType.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetInsertMWorkTypeQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int GetMaxSort()
        {
            int intSuccessful = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
           
            using (var command = _connectionManager.CreateCommandOnly(GetMaxSortQuery(), CommandType.Text, MultiTenancySide))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    intSuccessful = Convert.ToInt32(obj.ToString());
                }
            }
            return intSuccessful;
        }
        public int UpdateMWorkOrderStatus(MWorkOrderStatus status)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = status.Id
                },
                new SqlParameter("@Description", SqlDbType.NVarChar)
                {
                    Value = status.Description
                },
                new SqlParameter("@Active", SqlDbType.Int)
                {
                    Value = status.Active
                },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = status.ModifiedBy
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = status.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateWorkOrderStatusQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int UpdateMWorkType(MWorkType status)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = status.Id
                },
                new SqlParameter("@Description", SqlDbType.NVarChar)
                {
                    Value = status.Description
                },
                new SqlParameter("@Active", SqlDbType.Int)
                {
                    Value = status.Active
                },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = status.ModifiedBy
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = status.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateMWorkTypeQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int UpdateMArea(MArea status)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = status.Id
                },
                new SqlParameter("@Description", SqlDbType.NVarChar)
                {
                    Value = status.Description
                },
                new SqlParameter("@Active", SqlDbType.Int)
                {
                    Value = status.Active
                },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = status.ModifiedBy
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = status.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateMAreaQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
         public int UpdateMWorkTimeSheetNoteTemplate(MWorkTimeSheetNoteTemplate status)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = status.Id
                },
                new SqlParameter("@Description", SqlDbType.NVarChar)
                {
                    Value = status.Description
                },
                new SqlParameter("@Active", SqlDbType.Int)
                {
                    Value = status.Active
                },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = status.ModifiedBy
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = status.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateNoteQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int UpdateMPriority(MPriorityModel status)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@PriorityID", SqlDbType.Int)
                {
                    Value = status.PriorityID
                },
                new SqlParameter("@Priority", SqlDbType.NVarChar)
                {
                    Value = status.Priority
                },
                new SqlParameter("@Sort", SqlDbType.Int)
                {
                    Value = status.Sort
                },
                 new SqlParameter("@Active", SqlDbType.Int)
                {
                    Value = status.Active
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = status.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateMPriorityQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        public int UpdateMTechnician(MTechnician status)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@Seqno", SqlDbType.Int)
                {
                    Value = status.Id
                },
                new SqlParameter("@Active", SqlDbType.Int)
                {
                    Value = status.Active
                },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier)
                {
                    Value = status.ModifiedBy
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = status.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateTechnicianQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        #endregion
        #region sqlquery
        private static string GetUpdateTechnicianQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE MTechnician ");
            sb.Append(" SET   ");
            sb.Append("   Active = @Active ,   ");
            sb.Append("   ModifiedBy = @ModifiedBy , ModifiedOn = GETDATE(),TenantId=@TenantId ");
            sb.Append(" WHERE   ");
            sb.Append("    Seqno = @Seqno ;");
            return sb.ToString();
        }
        private static string GetUpdateMPriorityQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE MPriority ");
            sb.Append(" SET   ");
            sb.Append("   Active = @Active ,Priority = @Priority , ");
            sb.Append("   Sort = @Sort ,TenantId=@TenantId  ");
            sb.Append(" WHERE   ");
            sb.Append("    PriorityID = @PriorityID ;");
            return sb.ToString();
        }
        private static string GetUpdateNoteQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE MWorkTimeSheetNoteTemplate ");
            sb.Append(" SET   ");
            sb.Append("   Description = @Description , ");
            sb.Append("   Active = @Active ,   ");
            sb.Append("   ModifiedBy = @ModifiedBy , ModifiedOn = GETDATE(),TenantId=@TenantId   ");
            sb.Append(" WHERE   ");
            sb.Append("    Seqno = @Seqno ;");
            return sb.ToString();
        }
        private static string GetUpdateMAreaQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE MArea ");
            sb.Append(" SET   ");
            sb.Append("   Description = @Description , ");
            sb.Append("   Active = @Active ,   ");
            sb.Append("   ModifiedBy = @ModifiedBy , ModifiedOn = GETDATE(),TenantId=@TenantId   ");
            sb.Append(" WHERE   ");
            sb.Append("    Seqno = @Seqno ;");
            return sb.ToString();
        }
        private static string GetUpdateMWorkTypeQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE MWorkType ");
            sb.Append(" SET   ");
            sb.Append("   Description = @Description , ");
            sb.Append("   Active = @Active ,   ");
            sb.Append("   ModifiedBy = @ModifiedBy , ModifiedOn = GETDATE(),TenantId=@TenantId  ");
            sb.Append(" WHERE   ");
            sb.Append("    Seqno = @Seqno ;");
            return sb.ToString();
        }
        private static string GetUpdateWorkOrderStatusQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE MWorkOrderStatus  ");
            sb.Append(" SET ");
            sb.Append("   Active = @Active , Description = @Description, ");
            sb.Append("   ModifiedBy = @ModifiedBy , ModifiedOn = GETDATE(),TenantId=@TenantId ");
            sb.Append(" WHERE  ");
            sb.Append("   Seqno = @Seqno ;");
            return sb.ToString();
        }
        private static string GetMPriorityQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO MPriority (Priority,Active, Sort ,TenantId)");
            sb.Append(" VALUES   ");
            sb.Append("  ( @Priority,@Active,@Sort,@TenantId ) ;");
            return sb.ToString();
        }
        private static string GetInsertMWorkOrderStatusQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO MWorkOrderStatus (Description, Active, CreatedBy, CreatedOn,TenantId )");
            sb.Append(" VALUES   ");
            sb.Append("  ( @Description , @Active , @CreatedBy, GETDATE(),@TenantId ) ;");
            return sb.ToString();
        }
        private static string GetInserMWorkTimeSheetNoteTemplatetQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO MWorkTimeSheetNoteTemplate (Description, Active, CreatedBy, CreatedOn,TenantId )");
            sb.Append(" VALUES   ");
            sb.Append("  ( @Description , @Active , @CreatedBy, GETDATE(),@TenantId ) ;");
            return sb.ToString();
        }
        private static string GetInsertMAreaQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO MArea (Description, Active, CreatedBy, CreatedOn,TenantId )");
            sb.Append(" VALUES   ");
            sb.Append("  ( @Description , @Active , @CreatedBy, GETDATE(),@TenantId ) ;");
            return sb.ToString();
        }
        private static string GetInsertMWorkTypeQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO MWorkType (Description, Active, CreatedBy, CreatedOn,TenantId )");
            sb.Append(" VALUES   ");
            sb.Append("  ( @Description , @Active , @CreatedBy, GETDATE(),@TenantId ) ;");
            return sb.ToString();
        }
        private static string GetMaxSortQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT TOP(1) Sort ");
            sb.Append(" from MPriority order by Sort desc ");
            return sb.ToString();
        }
        private static string GetAllMWorkTypeQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MWorkType ");
            sb.Append(" ORDER BY  Description ;");
            return sb.ToString();
        }
        private static string GetAllMWorkOrderStatusQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MWorkOrderStatus ");
            sb.Append(" ORDER BY  Description ;");
            return sb.ToString();
        }
        private static string GetActiveMWorkOrderStatusQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MWorkOrderStatus where Active = 1 ");
            sb.Append(" ORDER BY  Description ;");
            return sb.ToString();
        }
        private static string GetActiveMWorkTypeQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MWorkType where Active = 1 ");
            sb.Append(" ORDER BY  Description ;");
            return sb.ToString();
        }
        private static string GetActiveMAreaQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MArea where Active = 1 ");
            sb.Append(" ORDER BY  Description ;");
            return sb.ToString();
        }
        private static string GetAllMAreaQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MArea ");
            sb.Append(" ORDER BY  Description ;");
            return sb.ToString();
        }
        private static string GetActiveMWorkNoteTemplateQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MWorkTimeSheetNoteTemplate where Active = 1 ");
            sb.Append(" ORDER BY  Description ;");
            return sb.ToString();
        }
        private static string GetAllMWorkNoteTemplateQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MWorkTimeSheetNoteTemplate ");
            sb.Append(" ORDER BY  Description ;");
            return sb.ToString();
        }
        private static string GetActiveMTechnicianQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MTechnician where Active = 1 ");
            sb.Append(" ORDER BY  Name ;");
            return sb.ToString();
        }
        private static string GetAllMTechnicianQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MTechnician ");
            sb.Append(" ORDER BY  Name ;");
            return sb.ToString();
        }
        private static string GetWorkNotesQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MPriority ");
            return sb.ToString();
        }
        private static string GetActiveMPriority()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  * ");
            sb.Append(" FROM    MPriority where Active = 1");
            return sb.ToString();
        }
        private static string GetExternalAccessQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select [ExternalAccessKey],[Start],[END] from ExternalAccess where Active=1");
            return sb.ToString();
        }
        


        #endregion

    }
}
