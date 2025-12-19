using Abp.Data;
using Abp.EntityFrameworkCore;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.EntityFrameworkCore.Repositories;
using BEZNgCore.EntityFrameworkCore;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEZNgCore.IRepairIAppService.Dto;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using Castle.Core.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Abp.Domain.Entities;
//using Z.BulkOperations.Internal.InformationSchema;
using Stripe;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;
using System.Diagnostics;

namespace BEZNgCore.Authorization.IrepairDal
{
    public class RegistrationdalRepository : BEZNgCoreRepositoryBase<GuestStatus, Guid>, IRegistrationdalRepository
    {
        private readonly IActiveTransactionProvider _transactionProvider;
        ConnectionManager _connectionManager;
        private static string skey = "Brillantez6";
        public RegistrationdalRepository(IDbContextProvider<BEZNgCoreDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
            _connectionManager = new ConnectionManager(dbContextProvider, _transactionProvider);
        }
        #region interfaceimplement & related function 
        public List<CityOutput> GenerateDDLCity()
        {
            CityOutput oo = new CityOutput();
            oo.CityKey = Guid.Empty;
            oo.City = "--";
            var lst = new List<CityOutput>();
            lst.Add(oo);
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetAllCity(), CommandType.Text, MultiTenancySide))
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
                    CityOutput o = new CityOutput();
                    o.CityKey = (!DBNull.Value.Equals(dr["CityKey"])) ? (!string.IsNullOrEmpty(dr["CityKey"].ToString()) ? new Guid(dr["CityKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.City = !DBNull.Value.Equals(dr["City"]) ? dr["City"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }
        public List<CountryOutput> GenerateDDLCountry()
        {
            CountryOutput oo = new CountryOutput();
            oo.NationalityKey = Guid.Empty;
            oo.Nationality = "--";
            var lst = new List<CountryOutput>();
            lst.Add(oo);
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetAllCountry(), CommandType.Text, MultiTenancySide))
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
                    CountryOutput o = new CountryOutput();
                    o.NationalityKey = (!DBNull.Value.Equals(dr["NationalityKey"])) ? (!string.IsNullOrEmpty(dr["NationalityKey"].ToString()) ? new Guid(dr["NationalityKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.Nationality = !DBNull.Value.Equals(dr["Nationality"]) ? dr["Nationality"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }
        public List<PurposeOutput> GenerateDDLPurposeofStay()
        {
            PurposeOutput oo = new PurposeOutput();
            oo.PurposeStayKey = Guid.Empty;
            oo.PurposeStay = "";
            var lst = new List<PurposeOutput>();
            lst.Add(oo);
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetAllPurposeOfStay(), CommandType.Text, MultiTenancySide))
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
                    PurposeOutput o = new PurposeOutput();
                    o.PurposeStayKey = (!DBNull.Value.Equals(dr["PurposeStayKey"])) ? (!string.IsNullOrEmpty(dr["PurposeStayKey"].ToString()) ? new Guid(dr["PurposeStayKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.PurposeStay = !DBNull.Value.Equals(dr["PurposeStay"]) ? dr["PurposeStay"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }
        public List<TitleOutput> GenerateDDLTitle()
        {
            TitleOutput oo = new TitleOutput();
            oo.TitleKey = Guid.Empty;
            oo.Title = "";
            var lst = new List<TitleOutput>();
            lst.Add(oo);
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            using (var command = _connectionManager.CreateCommandOnly(GetAllTitle(), CommandType.Text, MultiTenancySide))
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
                    TitleOutput o = new TitleOutput();
                    o.TitleKey = (!DBNull.Value.Equals(dr["TitleKey"])) ? (!string.IsNullOrEmpty(dr["TitleKey"].ToString()) ? new Guid(dr["TitleKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.Title = !DBNull.Value.Equals(dr["Title"]) ? dr["Title"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }
        public DocumentSign BindMainGuestSignature(string resKey, string strGuestKey)
        {

            DocumentSign entity = new DocumentSign();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(resKey)
                },
                new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(strGuestKey)
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetGuestDocumentsInfo(), CommandType.Text, MultiTenancySide, parameters))
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
                    entity.DocumentKey = new Guid(dr["DocumentKey"].ToString());
                    entity.GuestKey = new Guid(dr["GuestKey"].ToString());
                    entity.DocumentName = ConvertObjectToString(dr["Document"]);
                    entity.DocumentStore = dr["DocumentStore"] as byte[];
                    entity.Description = ConvertObjectToString(dr["Description"]);
                    entity.Signature = dr["Signature"] as byte[];
                }
            }
            return entity;
        }
        public static string ConvertObjectToString(object input)
        {
            string str = null;
            if (input != null)
            {
                str = input.ToString();
            }
            return str;
        }
        public CGuest GetGuestInfoByGuestKey(string guestKey)
        {
            CGuest guest = new CGuest();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(guestKey)
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetGuestInfoByGuestKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
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
                    guest.GuestKey = new Guid(guestKey);
                    guest.Address = Convert.IsDBNull(dr["Address"]) ? null : Convert.ToString(dr["Address"]);
                    guest.City = Convert.IsDBNull(dr["City"]) ? null : Convert.ToString(dr["City"]);
                    guest.CountryKey = Convert.IsDBNull(dr["CountryKey"]) ? null : Convert.ToString(dr["CountryKey"]);
                    guest.DOB = Convert.IsDBNull(dr["DOB"]) ? null : (DateTime?)(dr["DOB"]);
                    guest.EMail = Convert.IsDBNull(dr["EMail"]) ? null : Convert.ToString(dr["EMail"]);
                    guest.Fax = Convert.IsDBNull(dr["Fax"]) ? null : Convert.ToString(dr["Fax"]);
                    guest.FirstName = Convert.IsDBNull(dr["FirstName"]) ? null : Convert.ToString(dr["FirstName"]);
                    guest.Gender = Convert.IsDBNull(dr["Gender"]) ? null : Convert.ToString(dr["Gender"]) == "M" ? (char?)'M' : (char?)'F';
                    guest.LastName = Convert.IsDBNull(dr["LastName"]) ? null : Convert.ToString(dr["LastName"]);
                    guest.Mobile = Convert.IsDBNull(dr["Mobile"]) ? null : Convert.ToString(dr["Mobile"]);
                    guest.Name = Convert.IsDBNull(dr["Name"]) ? null : Convert.ToString(dr["Name"]);
                    guest.NationalityKey = Convert.IsDBNull(dr["NationalityKey"]) ? null : Convert.ToString(dr["NationalityKey"]);
                    guest.NationalityName = Convert.IsDBNull(dr["NationalityName"]) ? null : Convert.ToString(dr["NationalityName"]);
                    guest.Passport = Convert.IsDBNull(dr["Passport"]) ? null : Convert.ToString(dr["Passport"]);
                    guest.PassportExpiry = Convert.IsDBNull(dr["PassportExpiry"]) ? null : (DateTime?)(dr["PassportExpiry"]);
                    guest.Postal = Convert.IsDBNull(dr["Postal"]) ? null : Convert.ToString(dr["Postal"]);
                    guest.Tel = Convert.IsDBNull(dr["Tel"]) ? null : Convert.ToString(dr["Tel"]);
                    guest.Title = Convert.IsDBNull(dr["Title"]) ? null : Convert.ToString(dr["Title"]);

                }
            }
            return guest;
        }
        public DataTable GetReservationByFolioNumber(string folioNumber)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Docno", SqlDbType.VarChar)
                {
                    Value =folioNumber
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetReservationByFolioNumberQuery(), CommandType.Text, MultiTenancySide, parameters))
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
        public List<ReservationHistory> GetReservationByGuestKey(string GuestKey)
        {
            var lst = new List<ReservationHistory>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(GuestKey)
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetReservationByGuestKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
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
                    ReservationHistory o = new ReservationHistory();
                    o.CheckInDate = (DateTime)(Convert.IsDBNull(dr["CheckInDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["CheckInDate"])).Value);
                    o.CheckInDatedes = o.CheckInDate.ToString("dd/MM/yyyy");
                    o.CheckOutDate = (DateTime)(Convert.IsDBNull(dr["CheckOutDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(dr["CheckOutDate"])).Value);
                    o.CheckOutDatedes=o.CheckOutDate.ToString("dd/MM/yyyy");
                    o.StatusDesc= (!DBNull.Value.Equals(dr["StatusDesc"])) ? (!string.IsNullOrEmpty(dr["StatusDesc"].ToString()) ? dr["StatusDesc"].ToString() : "") : "";
                    o.DocNo= (!DBNull.Value.Equals(dr["DocNo"])) ? (!string.IsNullOrEmpty(dr["DocNo"].ToString()) ? dr["DocNo"].ToString() : "") : "";
                    o.Adult= !DBNull.Value.Equals(dr["Adult"]) ? Convert.ToInt32(dr["Adult"]) : 0;
                    o.Child = !DBNull.Value.Equals(dr["Child"]) ? Convert.ToInt32(dr["Child"]) : 0;
                    o.RateCode= (!DBNull.Value.Equals(dr["RateCode"])) ? (!string.IsNullOrEmpty(dr["RateCode"].ToString()) ? dr["RateCode"].ToString() : "") : "";
                    o.Unit = !DBNull.Value.Equals(dr["Unit"]) ? dr["Unit"].ToString() : "";
                    o.RoomTypeName= (!DBNull.Value.Equals(dr["RoomType"])) ? (!string.IsNullOrEmpty(dr["RoomType"].ToString()) ? dr["RoomType"].ToString() : "") : "";
                    o.Amount= !DBNull.Value.Equals(dr["Amount"]) ? Convert.ToDecimal(dr["Amount"]) : 0;

                    lst.Add(o);
                }
            }
            return lst;
        }
        public Guid GetCityKey(string City)
        {
            Guid dt =Guid.Empty;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@city", SqlDbType.NVarChar)
                {
                    Value = City
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetCityKey(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    dt = Guid.Parse(obj.ToString());
                }

            }
            return dt;
        }
        public Guid GetReservationKey(string docno)
        {
            Guid dt = Guid.Empty;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@docno", SqlDbType.NVarChar)
                {
                    Value = docno
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetReservationKey(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    dt = Guid.Parse(obj.ToString());
                }

            }
            return dt;
        }
        public string GetNationality(string CountryKey)
        {
            string dt = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@NationalityKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(CountryKey)
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetCountry(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    dt = obj.ToString();
                }
                

            }
            return dt;
        }
        public int UpdateMainGuestInfo(CGuest guest)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                {
                    Value =  guest.GuestKey
                },
                new SqlParameter("@Title", SqlDbType.NVarChar)
                {
                    Value = guest.Title
                },
                new SqlParameter("@Passport", SqlDbType.NVarChar)
                {
                    Value =guest.Passport
                },
                new SqlParameter("@Tel", SqlDbType.NVarChar)
                {
                    Value = guest.Tel
                },
                new SqlParameter("@Mobile", SqlDbType.NVarChar)
                {
                    Value = guest.Mobile
                },
                 new SqlParameter("@Address", SqlDbType.NVarChar)
                {
                    Value = guest.Address
                },
                  new SqlParameter("@Postal", SqlDbType.NVarChar)
                {
                    Value = guest.Postal
                },
                new SqlParameter("@City", SqlDbType.NVarChar)
                {
                    Value = guest.City
                },
                new SqlParameter("@NationalityKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(guest.NationalityKey)
                },
                new SqlParameter("@CountryKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(guest.CountryKey)
                },
                new SqlParameter("@DOB", SqlDbType.DateTime)
                {
                    Value = guest.DOB
                },
                new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = guest.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateMainGuestSQL(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        public int UpdateReservationAndMainGuestInfo(string reservationKey, string strPreCheckInCount)
        {

            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                {
                    Value =  Guid.Parse(reservationKey)
                },
                new SqlParameter("@PreCheckInCount", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(strPreCheckInCount)
                }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateReservationMainGuestSQL(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        public int InsertHistoryList(List<CHistory> listHistory)
        {
            int intSuccessful = 0;
            foreach (CHistory history in listHistory)
            {
                intSuccessful = InsertHistory(history);
            }
            return intSuccessful;
        }
        private int InsertHistory(CHistory history)
        {
           
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@HistoryKey", SqlDbType.UniqueIdentifier)
                {
                    Value = history.HistoryKey
                },
                new SqlParameter("@SourceKey", SqlDbType.UniqueIdentifier)
                {
                    Value = (history.SourceKey == null ? DBNull.Value : history.SourceKey)
                },
                new SqlParameter("@ModuleName", SqlDbType.VarChar)
                {
                    Value ="iCheckIn"
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
        public int UpdateChkOutReservation(string reservationKey)
        {
            int intRowAffected = 0;
            
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                {
                    Value =Guid.Parse(reservationKey)
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateChkOutReservationSQL(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        public DateTime GetBusinessDate()
        {
            DateTime dtBusinessDate = DateTime.Now;
            DataTable dt = new DataTable();
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
                    dt = ds.Tables[0];

                }

            }
            if (dt.Rows.Count > 0)
            {
                dtBusinessDate = Convert.ToDateTime(dt.Rows[0]["SystemDate"]);
            }
            return dtBusinessDate;
        }
        public DataTable GetChkOutBillingContactBy(string reservationKey)
        {
           
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                {
                    Value =Guid.Parse(reservationKey)
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetChkOutBillingContactBy(), CommandType.Text, MultiTenancySide, parameters))
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
        public DataTable GetReservationGuestByReservationKey(string reservationKey)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                {
                    Value =Guid.Parse(reservationKey)
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetReservationGuestByReservationKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
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
                dt.Columns.Add("No");
                int intCount = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["No"] = ++intCount + ".";
                }
            }
            return dt;
        }
        public int AddReservationGuest(CGuest guest)
        {
            int intRowAffected = 0;
            //SqlTransaction sqlTrans = sqlCon.BeginTransaction();

            string strGuestKey = CheckGuestExistsSQL(guest.Passport, guest.EMail);
            if (!string.IsNullOrEmpty(strGuestKey))
            {
                DataTable dt = DtGetGuestInfoByGuestKey(strGuestKey);
                if (dt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[0]["GuestStay"].ToString()))
                        guest.GuestStay = Convert.ToInt32(dt.Rows[0]["GuestStay"]);
                }

                guest.GuestKey = new Guid(strGuestKey);
                intRowAffected = UpdateGuestSQL(guest);
            }
            else
            {
                intRowAffected = InsertGuestInfo(guest);
            }

            intRowAffected = InsertReservationGuest(guest);
            return intRowAffected;

            //sqlTrans.Commit();
            
        }
        private DataTable DtGetGuestInfoByGuestKey(string guestKey)
        {

            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(guestKey)
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetGuestInfoByGuestKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
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
        private int InsertReservationGuest(CGuest guest)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  guest.ReservationKey
                 },
                 new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value = guest.GuestKey
                 },
                 new SqlParameter("@GuestStay", SqlDbType.Int)
                 {
                     Value =guest.GuestStay
                 },
                 new SqlParameter("@CheckInDate", SqlDbType.DateTime)
                 {
                     Value = guest.CheckInDate
                 },
                 new SqlParameter("@CheckOutDate", SqlDbType.DateTime)
                 {
                     Value = guest.CheckOutDate
                 },
                 
                 new SqlParameter("@TenantId", SqlDbType.Int)
                 {
                     Value = guest.TenantId
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(GetInsertReservationGuestQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        private int InsertGuestInfo(CGuest guest)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  guest.GuestKey
                 },
                new SqlParameter("@Title", SqlDbType.NVarChar)
                 {
                     Value = (guest.Title == null ? DBNull.Value : guest.Title)
                 },
                 new SqlParameter("@Name", SqlDbType.NVarChar)
                 {
                     Value = guest.FirstName + " " + guest.LastName
                 },
                 new SqlParameter("@FirstName", SqlDbType.NVarChar)
                 {
                     Value = guest.FirstName
                 },
                 new SqlParameter("@LastName", SqlDbType.NVarChar)
                 {
                     Value = guest.LastName
                 },
                 new SqlParameter("@Passport", SqlDbType.NVarChar)
                 {
                     Value =guest.Passport
                 },
                 new SqlParameter("@Tel", SqlDbType.NVarChar)
                 {
                     Value = guest.Tel
                 },
                 new SqlParameter("@Mobile", SqlDbType.NVarChar)
                 {
                     Value = guest.Mobile
                 },
                 new SqlParameter("@Email", SqlDbType.NVarChar)
                 {
                     Value = guest.EMail
                 },
                 new SqlParameter("@Address", SqlDbType.NVarChar)
                 {
                     Value = guest.Address
                 },
                 new SqlParameter("@Postal", SqlDbType.NVarChar)
                 {
                     Value = guest.Postal
                 },
                new SqlParameter("@City", SqlDbType.NVarChar)
                 {
                     Value =  (guest.City == null ? DBNull.Value : guest.City)
                 },
                 new SqlParameter("@NationalityKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  (guest.NationalityKey == null ? DBNull.Value : Guid.Parse(guest.NationalityKey))
                 },
                 new SqlParameter("@CountryKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  (guest.CountryKey == null ? DBNull.Value : Guid.Parse(guest.CountryKey))
                 },
                 new SqlParameter("@DOB", SqlDbType.DateTime)
                 {
                     Value = guest.DOB
                 },
                 new SqlParameter("@TenantId", SqlDbType.Int)
                 {
                     Value = guest.TenantId
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(GetInsertGuestSQL(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        private int UpdateGuestSQL(CGuest guest)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  guest.GuestKey
                 },
                 new SqlParameter("@Title", SqlDbType.NVarChar)
                 {
                     Value = (guest.Title == null ? DBNull.Value : guest.Title)
                 },
                 new SqlParameter("@Name", SqlDbType.NVarChar)
                 {
                     Value = guest.FirstName + " " + guest.LastName
                 },
                 new SqlParameter("@FirstName", SqlDbType.NVarChar)
                 {
                     Value = guest.FirstName
                 },
                 new SqlParameter("@LastName", SqlDbType.NVarChar)
                 {
                     Value = guest.LastName
                 },
                 new SqlParameter("@Passport", SqlDbType.NVarChar)
                 {
                     Value =guest.Passport
                 },
                 new SqlParameter("@Tel", SqlDbType.NVarChar)
                 {
                     Value = guest.Tel
                 },
                 new SqlParameter("@Mobile", SqlDbType.NVarChar)
                 {
                     Value = guest.Mobile
                 },
                  new SqlParameter("@Email", SqlDbType.NVarChar)
                 {
                     Value = guest.EMail
                 },
                  new SqlParameter("@Address", SqlDbType.NVarChar)
                 {
                     Value = guest.Address
                 },
                   new SqlParameter("@Postal", SqlDbType.NVarChar)
                 {
                     Value = guest.Postal
                 },
                new SqlParameter("@City", SqlDbType.NVarChar)
                 {
                     Value =  (guest.City == null ? DBNull.Value : guest.City)
                 },
                 new SqlParameter("@NationalityKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  (guest.NationalityKey == null ? DBNull.Value : Guid.Parse(guest.NationalityKey))
                 },
                 new SqlParameter("@CountryKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  (guest.CountryKey == null ? DBNull.Value : Guid.Parse(guest.CountryKey))
                 },
                 new SqlParameter("@DOB", SqlDbType.DateTime)
                 {
                     Value = guest.DOB
                 },
                 new SqlParameter("@TenantId", SqlDbType.Int)
                 {
                     Value = guest.TenantId
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateGuestSQL(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        private string CheckGuestExistsSQL(string passport, string eMail)
        {
            string dt = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Passport", SqlDbType.NVarChar)
                {
                    Value = passport
                },
                 new SqlParameter("@EMail", SqlDbType.NVarChar)
                {
                    Value = eMail
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetCheckGuestExistsSQL(), CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                    dt = obj.ToString();

            }
            return dt;
            
        }
        public int UpdateReservationGuest(CGuest guest)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  guest.GuestKey
                 },
                 new SqlParameter("@Title", SqlDbType.NVarChar)
                 {
                     Value = (guest.Title == null ? DBNull.Value : guest.Title)
                 },
                 new SqlParameter("@Name", SqlDbType.NVarChar)
                 {
                     Value = guest.FirstName + " " + guest.LastName
                 },
                 new SqlParameter("@FirstName", SqlDbType.NVarChar)
                 {
                     Value = guest.FirstName
                 },
                 new SqlParameter("@LastName", SqlDbType.NVarChar)
                 {
                     Value = guest.LastName
                 },
                 new SqlParameter("@Passport", SqlDbType.NVarChar)
                 {
                     Value =guest.Passport
                 },
                 new SqlParameter("@Tel", SqlDbType.NVarChar)
                 {
                     Value = guest.Tel
                 },
                 new SqlParameter("@Mobile", SqlDbType.NVarChar)
                 {
                     Value = guest.Mobile
                 },
                 new SqlParameter("@Email", SqlDbType.NVarChar)
                 {
                     Value = guest.EMail
                 },
                 new SqlParameter("@Address", SqlDbType.NVarChar)
                 {
                     Value = guest.Address
                 },
                 new SqlParameter("@Postal", SqlDbType.NVarChar)
                 {
                     Value = guest.Postal
                 },
                 new SqlParameter("@City", SqlDbType.NVarChar)
                 {
                     Value =  (guest.City == null ? DBNull.Value : guest.City)
                 },
                 new SqlParameter("@NationalityKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  (guest.NationalityKey == null ? DBNull.Value : Guid.Parse(guest.NationalityKey))
                 },
                 new SqlParameter("@CountryKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  (guest.CountryKey == null ? DBNull.Value : Guid.Parse(guest.CountryKey))
                 },
                 new SqlParameter("@DOB", SqlDbType.DateTime)
                 {
                     Value = guest.DOB
                 },
                 new SqlParameter("@TenantId", SqlDbType.Int)
                 {
                     Value = guest.TenantId
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(GetUpdateGuestSQL(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
         
           
        }
        public int RemoveReservationGuest(CGuest guest)
        {
           
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  guest.ReservationKey
                 },
                new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  guest.GuestKey
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(GetRemoveReservationGuestQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        public int AddGuest(CGuest guest)
        {
           
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  guest.GuestKey
                 },
                new SqlParameter("@Title", SqlDbType.NVarChar)
                 {
                     Value = (guest.Title == null ? DBNull.Value : guest.Title)
                 },
                 new SqlParameter("@Name", SqlDbType.NVarChar)
                 {
                     Value = guest.FirstName + " " + guest.LastName
                 },
                 new SqlParameter("@FirstName", SqlDbType.NVarChar)
                 {
                     Value = guest.FirstName
                 },
                 new SqlParameter("@LastName", SqlDbType.NVarChar)
                 {
                     Value = guest.LastName
                 },
                 //new SqlParameter("@Passport", SqlDbType.NVarChar)
                 //{
                 //    Value =guest.Passport
                 //},
                 //new SqlParameter("@Tel", SqlDbType.NVarChar)
                 //{
                 //    Value = guest.Tel
                 //},
                 new SqlParameter("@Mobile", SqlDbType.NVarChar)
                 {
                     Value = guest.Mobile
                 },
                 new SqlParameter("@Email", SqlDbType.NVarChar)
                 {
                     Value = guest.EMail
                 },
                 new SqlParameter("@Address", SqlDbType.NVarChar)
                 {
                     Value = guest.Address
                 },
                 new SqlParameter("@Postal", SqlDbType.NVarChar)
                 {
                     Value = guest.Postal
                 },
                //new SqlParameter("@City", SqlDbType.NVarChar)
                // {
                //     Value =  (guest.City == null ? DBNull.Value : guest.City)
                // },
                // new SqlParameter("@NationalityKey", SqlDbType.UniqueIdentifier)
                // {
                //     Value =  (guest.NationalityKey == null ? DBNull.Value : Guid.Parse(guest.NationalityKey))
                // },
                // new SqlParameter("@CountryKey", SqlDbType.UniqueIdentifier)
                // {
                //     Value =  (guest.CountryKey == null ? DBNull.Value : Guid.Parse(guest.CountryKey))
                // },
                 new SqlParameter("@DOB", SqlDbType.DateTime)
                 {
                     Value = guest.DOB
                 },
                 new SqlParameter("@TenantId", SqlDbType.Int)
                 {
                     Value = 1
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(AddGuest(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int UpdateGuestDocument(DocumentSign document)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@DocumentStore", SqlDbType.Image)
                 {
                     Value =  document.DocumentStore
                 },
                 new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                 {
                     Value = document.ReservationKey
                 },
                 new SqlParameter("@Signature", SqlDbType.VarBinary)
                 {
                     Value = document.Signature
                 }
                
           };
            using (var command = _connectionManager.CreateCommandSP(UpdateGuestDocumentsInfo(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        public int UpdateScreenShootImage(DocumentSign document)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@DocumentStore", SqlDbType.Image)
                 {
                     Value =  document.DocumentStore
                 },
                 new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                 {
                     Value = document.ReservationKey
                 }

           };
            using (var command = _connectionManager.CreateCommandSP(UpdateScreenShootImage(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int InsertGuestDocument(DocumentSign document)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@DocumentKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  document.DocumentKey
                 },
                new SqlParameter("@Document", SqlDbType.VarChar)
                 {
                     Value =  document.DocumentName
                 },
                new SqlParameter("@Description", SqlDbType.VarChar)
                 {
                     Value =  document.Description
                 },
                new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  document.GuestKey
                 },
                new SqlParameter("@DocumentStore", SqlDbType.Image)
                 {
                     Value =  document.DocumentStore
                 },
                 new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                 {
                     Value = document.ReservationKey
                 },
                 new SqlParameter("@Signature", SqlDbType.VarBinary)
                 {
                     Value = document.Signature
                 }
                
           };
            using (var command = _connectionManager.CreateCommandSP(InsertGuestDocumentsInfo(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public ReservationDetailOutput GetReservationByReservationKey(string reservationKey)
        {
            ReservationDetailOutput b = new ReservationDetailOutput();
           
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(reservationKey)
                }
            };
            using (var command = _connectionManager.CreateCommandSP(getReservation(), CommandType.Text, MultiTenancySide, parameters))
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
                List<ReservationDetailOutput> a = new List<ReservationDetailOutput>();
                foreach (DataRow row in dt.Rows)
                {
                    DateTime? nullable;
                    Guid? nullable2;
                    int? nullable3;
                    double? nullable4;
                    char? nullable5;
                    ReservationDetailOutput item = new ReservationDetailOutput
                    {
                        ReservationKey = (Guid)row["ReservationKey"],
                        DocDate = Convert.IsDBNull(row["DocDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(row["DocDate"])),
                        ReservationType = Convert.ToChar(row["ReservationType"]),
                        CompanyName = Convert.IsDBNull(row["CompanyName"]) ? null : Convert.ToString(row["CompanyName"]),
                        GuestName = Convert.IsDBNull(row["GuestName"]) ? null : Convert.ToString(row["GuestName"]),
                        Status = Convert.IsDBNull(row["Status"]) ? ((int?)(nullable3 = null)) : new int?(Convert.ToInt32(row["Status"])),
                        CheckInDate = Convert.IsDBNull(row["CheckInDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(row["CheckInDate"])),
                        CheckInTime = Convert.IsDBNull(row["CheckInTime"]) ? null : Convert.ToString(row["CheckInTime"]),
                        CheckOutDate = Convert.IsDBNull(row["CheckOutDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(row["CheckOutDate"])),
                        CheckOutTime = Convert.IsDBNull(row["CheckOutTime"]) ? null : Convert.ToString(row["CheckOutTime"]),
                        Adult = Convert.IsDBNull(row["Adult"]) ? ((int?)(nullable3 = null)) : new int?(Convert.ToInt32(row["Adult"])),
                        Child = Convert.IsDBNull(row["Child"]) ? ((int?)(nullable3 = null)) : new int?(Convert.ToInt32(row["Child"])),
                        StatusString = Convert.IsDBNull(row["StatusString"]) ? null : Convert.ToString(row["StatusString"]),
                        RateCode = Convert.IsDBNull(row["RateCode"]) ? null : Convert.ToString(row["RateCode"]),
                        DocNo = Convert.IsDBNull(row["DocNo"]) ? null : Convert.ToString(row["DocNo"])

                    };
                    a.Add(item);

                }
                b= a[0];
            }

            return b;
        }

        public List<ReservationRateOutput> GetTransactionByReservationKey(string reservationKey, string pMTDecrypt)
        {
            var lst = new List<ReservationRateOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                {
                     Value =  Guid.Parse(reservationKey)
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(getReservationRate(), CommandType.Text, MultiTenancySide, parameters))
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
                    ReservationRateOutput oReservationRate = new ReservationRateOutput();

                   // oReservationRate.ReservationRateKey = new Guid(Convert.ToString(dr["ReservationRateKey"]));
                   // oReservationRate.ReservationKey = Convert.IsDBNull(dr["ReservationKey"]) == true ? null : (Guid?)new Guid(Convert.ToString(dr["ReservationKey"]));
                    oReservationRate.ChargeDate = Convert.IsDBNull(dr["ChargeDate"]) == true ? null : (DateTime?)Convert.ToDateTime(dr["ChargeDate"]);
                    oReservationRate.ChargeDatedes = oReservationRate.ChargeDate != null ? oReservationRate.ChargeDate.Value.ToString("dd/MM/yyyy"):"";
                    oReservationRate.Rate = Convert.IsDBNull(dr["Rate"]) == true ? 0.00 : (Double?)Convert.ToDouble(dr["Rate"]);
                    oReservationRate.Ratedes = oReservationRate.Rate.Value.ToString("0.00");
                    oReservationRate.Tax1 = Convert.IsDBNull(dr["Tax1"]) == true ? 0.00 : (Double?)Convert.ToDouble(dr["Tax1"]);
                    oReservationRate.Tax1des = oReservationRate.Tax1.Value.ToString("0.00");
                    oReservationRate.Tax2 = Convert.IsDBNull(dr["Tax2"]) == true ? 0.00 : (Double?)Convert.ToDouble(dr["Tax2"]);
                    oReservationRate.Tax2des = oReservationRate.Tax2.Value.ToString("0.00");
                    oReservationRate.Tax3 = Convert.IsDBNull(dr["Tax3"]) == true ? 0.00 : (Double?)Convert.ToDouble(dr["Tax3"]);
                    oReservationRate.Tax3des = oReservationRate.Tax3.Value.ToString("0.00");
                    //  oReservationRate.Overwrite = Convert.IsDBNull(dr["Overwrite"]) == true ? null : (int?)Convert.ToInt32(dr["Overwrite"]);
                    oReservationRate.OverwriteReason = Convert.IsDBNull(dr["OverwriteReason"]) == true ? "" : Convert.ToString(dr["OverwriteReason"]);
                  //  oReservationRate.OverwriteStaff = Convert.IsDBNull(dr["OverwriteStaff"]) == true ? null : (Guid?)new Guid(Convert.ToString(dr["OverwriteStaff"]));
                    oReservationRate.OverwriteTime = Convert.IsDBNull(dr["OverwriteTime"]) == true ? null : (DateTime?)Convert.ToDateTime(dr["OverwriteTime"]);
                    oReservationRate.OverwriteTimedes = oReservationRate.OverwriteTime != null ? oReservationRate.OverwriteTime.Value.ToString("dd/MM/yyyy hh:mm:ss tt").ToUpper() : "";
                    oReservationRate.BillTo = Convert.IsDBNull(dr["BillTo"]) == true ? 0 : (int?)Convert.ToInt32(dr["BillTo"]);
                 //   oReservationRate.Status = Convert.IsDBNull(dr["Status"]) == true ? null : (int?)Convert.ToInt32(dr["Status"]);
                    oReservationRate.Ref1 = Convert.IsDBNull(dr["Ref1"]) == true ? null : Convert.ToString(dr["Ref1"]);
                    oReservationRate.Ref2 = Convert.IsDBNull(dr["Ref2"]) == true ? null : Convert.ToString(dr["Ref2"]);
                 //   oReservationRate.PostCodeKey = Convert.IsDBNull(dr["PostCodeKey"]) == true ? null : (Guid?)new Guid(Convert.ToString(dr["PostCodeKey"]));

                    oReservationRate.Covers = Convert.IsDBNull(dr["Covers"]) == true ? "" : Convert.ToString(dr["Covers"]);
                  //  oReservationRate.PostDate = Convert.IsDBNull(dr["PostDate"]) == true ? null : (DateTime?)Convert.ToDateTime(dr["PostDate"]);
                  //  oReservationRate.RoomKey = Convert.IsDBNull(dr["RoomKey"]) == true ? null : (Guid?)new Guid(Convert.ToString(dr["RoomKey"]));
                    oReservationRate.UserName = Convert.IsDBNull(dr["UserName"]) == true ? "" : Convert.ToString(dr["UserName"]);
                   // oReservationRate.Void = Convert.IsDBNull(dr["Void"]) == true ? null : (int?)Convert.ToInt32(dr["Void"]);
                   // oReservationRate.ARTransKey = Convert.IsDBNull(dr["ARTransKey"]) == true ? null : (Guid?)new Guid(Convert.ToString(dr["ARTransKey"]));
                   // oReservationRate.BillToName = Convert.IsDBNull(dr["BillToName"]) == true ? null : Convert.ToString(dr["BillToName"]);
                   // oReservationRate.InvoiceNo = Convert.IsDBNull(dr["InvoiceNo"]) == true ? null : Convert.ToString(dr["InvoiceNo"]);
                   // oReservationRate.Total = Convert.IsDBNull(dr["Total"]) == true ? 0.00 : (Double?)Convert.ToDouble(dr["Total"]);
                   // oReservationRate.VoucherNo = Convert.IsDBNull(dr["VoucherNo"]) == true ? null : Convert.ToString(dr["VoucherNo"]);
                   // oReservationRate.ShiftKey = Convert.IsDBNull(dr["ShiftKey"]) == true ? null : (Guid?)new Guid(Convert.ToString(dr["ShiftKey"]));
                   // oReservationRate.ShiftNo = Convert.IsDBNull(dr["ShiftNo"]) == true ? null : Convert.ToString(dr["ShiftNo"]);
                   // oReservationRate.VoidSourceKey = Convert.IsDBNull(dr["VoidSourceKey"]) == true ? null : (Guid?)new Guid(Convert.ToString(dr["VoidSourceKey"]));
                  //  oReservationRate.ItemKey = Convert.IsDBNull(dr["ItemKey"]) == true ? null : (Guid?)new Guid(Convert.ToString(dr["ItemKey"]));
                   // oReservationRate.Consolidated = Convert.IsDBNull(dr["Consolidated"]) == true ? null : (int?)Convert.ToInt32(dr["Consolidated"]);
                   // oReservationRate.ForeignCurrencyKey = Convert.IsDBNull(dr["ForeignCurrencyKey"]) == true ? null : (Guid?)new Guid(Convert.ToString(dr["ForeignCurrencyKey"]));
                    oReservationRate.ForeignAmount = Convert.IsDBNull(dr["ForeignAmount"]) == true ? 0.00 : (double?)Convert.ToDouble(dr["ForeignAmount"]);
                    oReservationRate.ForeignAmountdes = oReservationRate.ForeignAmount.Value.ToString("0.00");
                    // oReservationRate.PaymentTypeKey = Convert.IsDBNull(dr["PaymentTypeKey"]) == true ? null : (Guid?)new Guid(Convert.ToString(dr["PaymentTypeKey"]));
                    //  oReservationRate.StaffKey = Convert.IsDBNull(dr["StaffKey"]) == true ? null : (Guid?)new Guid(Convert.ToString(dr["StaffKey"]));
                    //  oReservationRate.SecondaryAmount = Convert.IsDBNull(dr["SecondaryAmount"]) == true ? null : (double?)Convert.ToDouble(dr["SecondaryAmount"]);
                    oReservationRate.Description = Convert.IsDBNull(dr["Description"]) == true ? "" : Convert.ToString(dr["Description"]);
                  //  oReservationRate.CardName = Convert.IsDBNull(dr["CardName"]) == true ? null : Convert.ToString(dr["CardName"]);
                    oReservationRate.ForeignExchangeRate = Convert.IsDBNull(dr["ForeignExchangeRate"]) == true ? 0.00 : (double?)Convert.ToDouble(dr["ForeignExchangeRate"]);
                    oReservationRate.ForeignExchangeRatedes = oReservationRate.ForeignExchangeRate.Value.ToString("0.00");
                    //  oReservationRate.SecondaryExchangeRate = Convert.IsDBNull(dr["SecondaryExchangeRate"]) == true ? null : (double?)Convert.ToDouble(dr["SecondaryExchangeRate"]);
                    //  oReservationRate.AdditionalBed = Convert.IsDBNull(dr["Sync"]) == true ? null : (byte?)Convert.ToByte(dr["Sync"]);
                    oReservationRate.AwardedPoint = Convert.IsDBNull(dr["AwardedPoint"]) == true ? 0 : (int?)Convert.ToInt32(dr["AwardedPoint"]);
                    oReservationRate.RedeemPoint = Convert.IsDBNull(dr["RedemptPoint"]) == true ? 0 : (int?)Convert.ToInt32(dr["RedemptPoint"]);

                    oReservationRate.Total = Convert.IsDBNull(dr["CalculatedTotal"]) == true ? 0.00 : (Double?)Convert.ToDouble(dr["CalculatedTotal"]);// Convert.IsDBNull(dr["CalculatedTotal"]) == true ? Math.Round((Double)0, 2, MidpointRounding.AwayFromZero) : Math.Round((Double)(Double?)Convert.ToDouble(dr["CalculatedTotal"]),2,MidpointRounding.AwayFromZero);
                    oReservationRate.Totaldes = oReservationRate.Total.Value.ToString("0.00");
                    oReservationRate.PostCodeName = Convert.IsDBNull(dr["PostCode"]) == true ? "" : Convert.ToString(dr["PostCode"]);
                    oReservationRate.ForeignCurrencyName = Convert.IsDBNull(dr["Currency"]) == true ? "" : Convert.ToString(dr["Currency"]);
                  //  oReservationRate.OverwriteStaffName = Convert.IsDBNull(dr["FullName"]) == true ? null : Convert.ToString(dr["FullName"]);
                    #region mui
                    if (!string.IsNullOrEmpty(pMTDecrypt))
                    {
                        oReservationRate.lblRef1 = Ref1Ref2Process(oReservationRate.Ref1, pMTDecrypt);
                        oReservationRate.lblRef2 = Ref1Ref2Process(oReservationRate.Ref2, pMTDecrypt);
                        if (pMTDecrypt != "10" && pMTDecrypt != "15")
                        {
                            oReservationRate.lblRef1 = "******";
                            oReservationRate.lblRef2 = "******";
                        }
                    }
                    else
                    {
                        oReservationRate.lblRef1 = oReservationRate.Ref1==null?"": oReservationRate.Ref1;
                        oReservationRate.lblRef2 = oReservationRate.Ref2 == null ? "" : oReservationRate.Ref2;
                    }
                    #endregion
                    lst.Add(oReservationRate);
                }
                
            }
            return lst;   
            
        }

        public int InsertGuestEmailList(EmailList el)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@mail_from", SqlDbType.VarChar)
                 {
                     Value =   el.mail_from
                 },
                 new SqlParameter("@mail_to", SqlDbType.VarChar)
                 {
                     Value =   el.mail_to
                 },
                  new SqlParameter("@mail_subject", SqlDbType.VarChar)
                 {
                     Value =   el.mail_subject
                 },
                   new SqlParameter("@mail_body", SqlDbType.VarChar)
                 {
                     Value =   el.mail_body
                 },
                 new SqlParameter("@date_tosend", SqlDbType.DateTime)
                 {
                     Value = el.date_tosend
                 },
                 new SqlParameter("@date_sent", SqlDbType.DateTime)
                 {
                     Value = el.date_sent
                 },
                 new SqlParameter("@mail_status", SqlDbType.Int)
                 {
                     Value = el.mail_status
                 },
                   new SqlParameter("@doc_no", SqlDbType.VarChar)
                 {
                     Value =   el.doc_no
                 }

           };
            using (var command = _connectionManager.CreateCommandSP(InsertGuestEmailList(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int UpdateEmailHistory(EmailHistory pOHistory)
        {
           
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                 {
                     Value = pOHistory.ReservationKey
                 },
                 new SqlParameter("@SentDateTime", SqlDbType.DateTime)
                 {
                     Value = pOHistory.SentDateTime
                 },
                  new SqlParameter("@Sent", SqlDbType.Int)
                 {
                     Value = 1
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(UpdateEmailHistory(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int InsertHistory(EmailHistory pOHistory)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =    pOHistory.ReservationKey
                 },
                 new SqlParameter("@SentDateTime", SqlDbType.DateTime)
                 {
                     Value =   pOHistory.SentDateTime
                 },
                  new SqlParameter("@Sort", SqlDbType.Int)
                 {
                     Value =   pOHistory.Sort
                 },
                   new SqlParameter("@From", SqlDbType.VarChar)
                 {
                     Value =   pOHistory.From
                 },
                 new SqlParameter("@To", SqlDbType.VarChar)
                 {
                     Value = pOHistory.To
                 },
                 new SqlParameter("@Sent", SqlDbType.Int)
                 {
                     Value = pOHistory.Sent
                 },
                 new SqlParameter("@Content", SqlDbType.VarChar)
                 {
                     Value = pOHistory.Content
                 },
                   new SqlParameter("@Subject", SqlDbType.VarChar)
                 {
                     Value = pOHistory.Subject
                 }

           };
            using (var command = _connectionManager.CreateCommandSP(insertEmailHistory(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        public DataTable GetReservationRequestByGuestKey(string guestKey, int cancelReq, int openReq, int inPReq, int completeReq, DateTime requestDate, DateTime toDate)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
         
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CancelRequest", SqlDbType.Int)
                {
                    Value =cancelReq
                },
                new SqlParameter("@OpenRequest", SqlDbType.Int)
                {
                    Value =openReq
                },
                new SqlParameter("@InProcessRequest", SqlDbType.Int)
                {
                    Value =inPReq
                },
                new SqlParameter("@CompleteRequest", SqlDbType.Int)
                {
                    Value =completeReq
                },
                new SqlParameter("@RequestDate", SqlDbType.DateTime)
                {
                    Value =requestDate
                },
                new SqlParameter("@ToDate", SqlDbType.DateTime)
                {
                    Value =toDate
                }
            };
            using (var command = _connectionManager.CreateCommandSP(GetReservationRequestByGuestKeyQuery(), CommandType.Text, MultiTenancySide, parameters))
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
        public string GetReservationRequestID()
        {
            string res = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
      
            using (var command = _connectionManager.CreateCommandOnly("declare @RefNo  VARCHAR(15) Select @RefNo = [dbo].[UDF_GenerateReservationRequestID]() select @RefNo", CommandType.Text, MultiTenancySide))
            {
                res=Convert.ToString(command.ExecuteScalar());
            }
            return res;
        }
        public List<RequestTypeOutput> GetAllRequestType()
        {
           
            var lst = new List<RequestTypeOutput>();
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetRequestType(), CommandType.Text, MultiTenancySide))
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
                    RequestTypeOutput o = new RequestTypeOutput();
                    o.RequestTypeKey = (!DBNull.Value.Equals(dr["RequestTypeKey"])) ? (!string.IsNullOrEmpty(dr["RequestTypeKey"].ToString()) ? new Guid(dr["RequestTypeKey"].ToString()) : Guid.Empty) : Guid.Empty;
                    o.RequestTypeName = !DBNull.Value.Equals(dr["RequestTypeName"]) ? dr["RequestTypeName"].ToString() : "";

                    lst.Add(o);
                }
            }
            return lst;
        }
        public ReservationRequestOutput GetReservationRequestNew(string reservationRequestKey)
        {
            ReservationRequestOutput o = new ReservationRequestOutput();

            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ReservationRequestKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(reservationRequestKey)
                }
            };
            using (var command = _connectionManager.CreateCommandSP(getReservationRequestGuest(), CommandType.Text, MultiTenancySide, parameters))
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
                foreach (DataRow row in dt.Rows)
                {
                    DateTime? nullable;
                    o.ReservationRequestKey = (Guid)row["ReservationRequestKey"];
                    o.ReservationRequestID = Convert.IsDBNull(row["ReservationRequestID"]) ? null : Convert.ToString(row["ReservationRequestID"]);
                    o.Status = Convert.ToInt32(row["Status"]);
                    o.StatusName = o.Status == -1 ? "Cancelled request" : o.Status == 1 ? "Open request" : o.Status == 2 ? "In Progress request" : o.Status == 10 ? "Completed request" : "Pending";
                    o.btnEdit = o.Status == 10 ? false : true;
                    o.RequestTypeKey = (Guid)row["RequestTypeKey"];
                    o.RequestTypeName= Convert.IsDBNull(row["RequestTypeName"]) ? null : Convert.ToString(row["RequestTypeName"]);
                    o.GuestKey = (Guid)row["GuestKey"];
                    o.ReservationKey = (Guid)row["ReservationKey"];
                    o.GuestRequest = Convert.IsDBNull(row["GuestRequest"]) ? null : Convert.ToString(row["GuestRequest"]);
                    o.RequestDate = Convert.IsDBNull(row["RequestDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(row["RequestDate"]));
                    o.HotelResponse = Convert.IsDBNull(row["HotelResponse"]) ? null : Convert.ToString(row["HotelResponse"]);
                    o.ResponseDate = Convert.IsDBNull(row["ResponseDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(row["ResponseDate"]));
  
                }
                
            }

            return o;
        }

        public string GetGuestName(string GuestKey)
        {
            string dt = "";
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                {
                    Value = Guid.Parse(GuestKey)
                }
            };
            using (var command = _connectionManager.CreateCommandSP("select Name from Guest where GuestKey=@GuestKey", CommandType.Text, MultiTenancySide, parameters))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    dt = obj.ToString();
                }


            }
            return dt;
        }
        public int AddRequestGuest(ReservationRequestInput a)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@ReservationRequestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =   a.ReservationRequestKey
                 },
                 new SqlParameter("@ReservationRequestID", SqlDbType.VarChar)
                 {
                     Value =   a.ReservationRequestID
                 },
                  new SqlParameter("@Status", SqlDbType.Int)
                 {
                     Value =   a.Status
                 },
                   new SqlParameter("@RequestTypeKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  a.RequestTypeKey
                 },
                 new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value = a.GuestKey
                 },
                 new SqlParameter("@ReservationKey", SqlDbType.UniqueIdentifier)
                 {
                     Value = a.ReservationKey
                 },
                 new SqlParameter("@GuestRequest", SqlDbType.VarChar)
                 {
                     Value = a.GuestRequest
                 },
                   new SqlParameter("@RequestDate", SqlDbType.DateTime)
                 {
                     Value =   a.RequestDate
                 },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier)
                 {
                     Value =   a.CreatedBy
                 },
                new SqlParameter("@TenantId", SqlDbType.Int)
                 {
                     Value =   a.TenantId
                 }

           };
            using (var command = _connectionManager.CreateCommandSP(insertGuestRequest(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int UpdateRequestGuest(ReservationRequestInput a)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
          {
                 new SqlParameter("@ReservationRequestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =   a.ReservationRequestKey
                 },
                 new SqlParameter("@Status", SqlDbType.Int)
                 {
                     Value =   a.Status
                 },
                 new SqlParameter("@RequestTypeKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  a.RequestTypeKey
                 },
                 new SqlParameter("@GuestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value = a.GuestKey
                 },
                 new SqlParameter("@GuestRequest", SqlDbType.VarChar)
                 {
                     Value = a.GuestRequest
                 },
                 new SqlParameter("@RequestDate", SqlDbType.DateTime)
                 {
                     Value =   a.RequestDate
                 },
                 new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier)
                 {
                     Value =   a.ModifiedBy
                 },
                 new SqlParameter("@TenantId", SqlDbType.Int)
                 {
                     Value =   a.TenantId
                 }

          };
            using (var command = _connectionManager.CreateCommandSP(UpdateGuestRequest(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        #region UI Helper
        private string Ref1Ref2Process(string cc,string PMTDecrypt)
        {
            string ccc = cc;

            if (string.IsNullOrEmpty(cc))
            {

            }
            else
            {
                if (PMTDecrypt == "10")
                {
                    if (ValidateCreditCard(cc))
                    {

                    }
                    else
                    {
                        if (cc.Length < 17)
                        {

                        }
                        else
                        {
                            ccc = DecryptInput((cc), skey);
                        }
                    }

                }
                else if (PMTDecrypt == "15")
                {
                    if (ValidateCreditCard(cc))
                    {

                    }
                    else
                    {
                        if (cc.Length < 17)
                        {

                        }
                        else
                        {
                            ccc = DecryptInput((cc), skey);
                        }
                    }

                }
                else
                {
                    if (ValidateCreditCard(cc))
                    {
                        ccc = EncryptInput((cc), skey);
                    }
                    else
                    {
                        if (cc.Length < 17)
                        {
                            ccc = EncryptInput((cc), skey);
                        }
                        else
                        {

                        }
                    }
                }
            }
            return ccc;
        }
        private static bool ValidateCreditCard(string creditCardNumber)
        {
            creditCardNumber = Regex.Replace(creditCardNumber, @"[^\d]", "");
            Regex expression = new Regex(@"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$");
            return expression.IsMatch(creditCardNumber);
        }

        private static string EncryptInput(string plainText, string saltKey)
        {
            string PasswordHash = "Brillantez";
            string SaltKey = saltKey;//Brillantez6
            string VIKey = "123456789012345678";
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }
        private static string DecryptInput(string encryptedText, string saltkey)
        {
            string decrypt = string.Empty;
            try
            {
                string PasswordHash = "Brillantez";
                string SaltKey = saltkey;
                string VIKey = "123456789012345678";
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
                byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                decrypt = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
            }
            catch (Exception ex)
            {
                decrypt = encryptedText;
            }
            return decrypt;
        }
        #endregion

        #endregion
        #region sqlquery
        private static string UpdateGuestRequest()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE ReservationRequest SET Status=@Status,RequestTypeKey=@RequestTypeKey,GuestKey=@GuestKey,GuestRequest=@GuestRequest,RequestDate=@RequestDate,ModifiedBy=@ModifiedBy,TenantId=@TenantId WHERE ReservationRequestKey=@ReservationRequestKey");
            return builder.ToString();

        }
        private static string insertGuestRequest()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Insert Into ReservationRequest (ReservationRequestKey,ReservationRequestID,Status,RequestTypeKey,GuestKey,ReservationKey,GuestRequest,RequestDate,CreatedBy,TenantId) ");
            builder.Append(" values ");
            builder.Append(" (@ReservationRequestKey,@ReservationRequestID,@Status,@RequestTypeKey,@GuestKey,@ReservationKey,@GuestRequest,@RequestDate,@CreatedBy,@TenantId)");
            return builder.ToString();

        }
        private static string getReservationRequestGuest()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select ReservationRequestKey,ReservationRequestID,Status,ReservationRequest.RequestTypeKey,RequestType.RequestTypeName,GuestKey,ReservationKey,");
            sb.Append(" GuestRequest,RequestDate,HotelResponse,ResponseDate from ReservationRequest inner join RequestType on ReservationRequest.RequestTypeKey=RequestType.RequestTypeKey");
            sb.Append(" where ReservationRequestKey=@ReservationRequestKey ");
            return sb.ToString();
        }
        private static string GetRequestType()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT RequestTypeKey, RequestTypeName ");
            sb.Append(" FROM RequestType");
            sb.Append(" Order by Sort ");
            return sb.ToString();
        }
        private static string GetReservationRequestByGuestKeyQuery()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Select rr.ReservationRequestKey, rr.ReservationRequestID,Gu.Name, V.ReservationKey, rr.RequestDate,rr.ResponseDate,V.DocNo,V.Adult,V.Child,Y.RateCode,U.Unit,req.RequestTypeName ");
            builder.Append(" , StatusDesc = CASE rr.Status WHEN -1  THEN 'Cancelled request' WHEN 1    THEN 'Open request'  WHEN 2   THEN 'In Progress request'  WHEN 10  THEN 'Completed request' ELSE 'Pending'  END,rr.GuestRequest,rr.HotelResponse ");
            builder.Append(" from ReservationRequest rr join Reservation V on v.ReservationKey=rr.ReservationKey join RateType Y on V.RateTypeKey=Y.RateTypeKey  ");
            builder.Append(" Join Room U on V.RoomKey=U.RoomKey Join RoomType RT on V.RoomTypeKey = RT.RoomTypeKey Join RequestType req on req.RequestTypeKey=rr.RequestTypeKey Join Guest Gu On Gu.GuestKey=V.GuestKey ");
            builder.Append("  where rr.Status in(@CancelRequest,@OpenRequest,@InProcessRequest,@CompleteRequest)  And convert(varchar(10),rr.RequestDate, 120) Between convert(varchar(10), @RequestDate, 120) And convert(varchar(10), @ToDate, 120) Order by rr.RequestDate ");
            return builder.ToString();
        }
        private static string UpdateEmailHistory()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" UPDATE EmailHistory SET Sent=@Sent WHERE ReservationKey=@ReservationKey and SentDateTime=@SentDateTime");
            return builder.ToString();

        }
        private static string insertEmailHistory()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Insert Into EmailHistory (SentDateTime,ReservationKey,Sort,[From],[To],Sent,[Content],Subject ) ");
            builder.Append(" values ");
            builder.Append(" (@SentDateTime,@ReservationKey,@Sort,@From,@To,@Sent,@Content,@Subject )");
            return builder.ToString();

        }
        public static string InsertGuestEmailList()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" INSERT INTO EmailList (mac_id, batch_id, mail_from, mail_to, mail_subject, mail_body, date_tosend,date_sent,mail_status,doc_no) ");
            builder.Append("  VALUES('Hotel','1',@mail_from,@mail_to,@mail_subject,@mail_body,@date_tosend,@date_sent,@mail_status,@doc_no) ");
            return builder.ToString();
        }
        public static string getReservation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("  SELECT  RT.RateCode ,dbo.UDF_GetReportingRateCode(V.ReportingRateTypeKey) AS ReportingRateCode , StatusString = CASE V.Status  ");
            builder.Append("  WHEN -2 THEN 'No Show' ");
            builder.Append("  WHEN -1 THEN 'Cancel' ");
            builder.Append("  WHEN 0 THEN 'Pending'");
            builder.Append("  WHEN 1 THEN 'Reservation'");
            builder.Append("  WHEN 2 THEN 'Check In' ");
            builder.Append(" WHEN 10 THEN 'Check Out'  END, V.*, B.Name 'BookingAgentName',C.Name 'CompanyName' , G.Name 'GuestName',G.LanguageCode,G.Subscribe,dbo.UDF_GuestStayCount(G.GuestKey) AS GuestStayCount ");
            builder.Append(" , Room.Unit, RoomType.RoomType ");
            builder.Append(" FROM Reservation V LEFT JOIN RateType RT ");
            builder.Append(" ON V.RateTypeKey = RT.RateTypeKey  ");
            builder.Append(" LEFT JOIN Company C ON V.CompanyKey = C.CompanyKey ");
            builder.Append(" LEFT JOIN Company B ON V.BookingAgentKey = B.CompanyKey");
            builder.Append(" LEFT JOIN Guest G ON V.GuestKey = G.GuestKey ");
            builder.Append(" LEFT JOIN Room ON V.RoomKey  = Room.RoomKey  ");
            builder.Append(" LEFT JOIN RoomType ON V.RoomTypeKey = RoomType.RoomTypeKey ");
            builder.Append(" WHERE V.ReservationKey = @ReservationKey ");
            return builder.ToString();
        }
        public static string getReservationRate()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Select R.* ,CalculatedTotal=case when (R.Rate+R.Tax1+R.Tax2+R.Tax3)is Null then 0 ");
            builder.Append(" else (R.Rate+R.Tax1+R.Tax2+R.Tax3) end,S.FullName ,C.Currency , ");
            builder.Append(" PC.PostCode ");
            builder.Append(" from ReservationRate R Join Reservation V on R.ReservationKey=V.ReservationKey ");
            builder.Append(" Left Join Postcode PC ON R.PostCodeKey = PC.PostcodeKey ");
            builder.Append(" LEFT JOIN Staff S ON R.OverwriteStaff = S.StaffKey ");
            builder.Append(" LEFT JOIN Currency C ON R.ForeignCurrencyKey = C.CurrencyKey ");
            builder.Append(" where R.ReservationKey= @ReservationKey ");
            builder.Append(" and R.Consolidated=0 ");
            builder.Append(" order by R.ChargeDate desc,V.DocNo,R.Seq desc ");
            return builder.ToString();
        }
        public static string InsertGuestDocumentsInfo()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Insert into Document ");
            builder.Append(" (DocumentKey,Document, Description, GuestKey,DocumentStore,ReservationKey, Signature ) ");
            builder.Append(" Values (@DocumentKey,@Document, @Description, @GuestKey,@DocumentStore,@ReservationKey, @Signature);  ");
            return builder.ToString();
        }
        public static string UpdateGuestDocumentsInfo()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" UPDATE Document SET DocumentStore=@DocumentStore , Signature = @Signature");
            builder.Append(" WHERE ReservationKey=@ReservationKey AND Document='Guest Signature' ");
            return builder.ToString();
        }
        public static string UpdateScreenShootImage()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" UPDATE Document SET DocumentStore=@DocumentStore");
            builder.Append(" WHERE ReservationKey=@ReservationKey AND Document='Guest Signature' ");
            return builder.ToString();
        }
        private static string GetRemoveReservationGuestQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE   ReservationGuest ");
            sb.Append(" SET ");
            sb.Append("  ReservationKey = NULL ");
            sb.Append(" WHERE ");
            sb.Append("  ReservationKey = @ReservationKey AND  GuestKey = @GuestKey ; ");
            return sb.ToString();
        }
        private static string GetInsertGuestSQL()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO Guest ");
            sb.Append(" (GuestKey,Title,Name,FirstName,LastName,  Passport, ");
            sb.Append("  Tel,Mobile,Email,Address,Postal, City , NationalityKey, CountryKey, DOB,TenantId) ");
            sb.Append(" VALUES ");
            sb.Append(" (@GuestKey, @Title, @Name,@FirstName,@LastName,  @Passport, ");
            sb.Append(" @Tel, @Mobile, @Email, @Address, @Postal, @City, @NationalityKey, ");
            sb.Append(" @CountryKey, @DOB,@TenantId)");
            return sb.ToString();
        }
        private static string GetInsertReservationGuestQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Insert Into ReservationGuest");
            sb.Append(" ( ReservationKey, GuestKey, CheckInDate, CheckOutDate, GuestStay,TenantId )");
            sb.Append(" Values ");
            sb.Append(" ( @ReservationKey,  @GuestKey, @CheckInDate, @CheckOutDate, @GuestStay,@TenantId ) ");
            return sb.ToString();
        }
        public static string GetCheckGuestExistsSQL()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select GuestKey  ");
            sb.Append(" FROM Guest ");
            sb.Append(" WHERE Passport = @Passport  AND EMail = @EMail ");

            return sb.ToString();
        }
        private static string GetUpdateGuestSQL()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Guest ");
            sb.Append(" Set Name = @NAME , ");
            sb.Append(" Title = @Title , ");
            sb.Append(" FirstName = @FirstName , ");
            sb.Append(" LastName = @LastName , ");
            sb.Append(" Passport= @Passport, ");
            sb.Append(" Tel= @Tel, ");
            sb.Append(" Email= @Email, ");
            sb.Append(" Address= @Address, ");
            sb.Append(" Postal= @Postal, ");
            sb.Append(" Mobile= @Mobile, ");
            sb.Append(" City = @City, ");
            sb.Append(" NationalityKey = @NationalityKey, ");
            sb.Append(" CountryKey = @CountryKey, ");
            sb.Append(" DOB = @DOB,TenantId=@TenantId ");
            sb.Append(" WHERE GuestKey = @GuestKey");
            return sb.ToString();
        }
        private static string GetAllCity()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT CityKey, City ");
            sb.Append(" FROM City ");
            sb.Append(" Order by City ASC ");
            return sb.ToString();
        }
        private static string GetAllCountry()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT NationalityKey, Nationality ");
            sb.Append(" FROM Nationality ");
            sb.Append(" Order by Nationality ASC ");
            return sb.ToString();
        }
        private static string GetAllPurposeOfStay()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT * FROM PurposeStay    ");
            sb.Append(" WHERE  Active = 1 ");
            sb.Append(" ORDER BY  Sort ; ");
            return sb.ToString();
        }
        private static string GetAllTitle()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT TitleKey, Title ");
            sb.Append(" FROM Title where Active = 1 ");
            sb.Append(" Order by Sort ");
            return sb.ToString();
        }
        public static string GetGuestDocumentsInfo()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Select DocumentKey,GuestKey,Document,DocumentStore,Description, Signature ");
            builder.Append(" from Document ");
            builder.Append(" where ReservationKey= @ReservationKey and GuestKey=@GuestKey");
            return builder.ToString();
        }
        private static string GetGuestInfoByGuestKeyQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT g.Gender,g.DOB,g.Tel,g.Mobile,g.Fax,g.EMail,");
            sb.Append("  g.DOB , g.GuestStay ,  ");
            sb.Append(" g.Postal,g.CountryKey,n1.Nationality as NationalityName,g.NationalityKey,");
            sb.Append(" n.Nationality,g.PassportExpiry,g.Title,");
            sb.Append(" g.LastName,g.FirstName,g.[Address],g.City,");
            sb.Append(" g.Passport,g.Name FROM Guest g");
            sb.Append(" LEFT JOIN Nationality n ON g.CountryKey = n.NationalityKey");
            sb.Append(" LEFT JOIN Nationality n1 ON n1.NationalityKey = g.NationalityKey");
            sb.Append(" WHERE g.GuestKey = @GuestKey");
            return sb.ToString();
        }

        public static string GetReservationByGuestKeyQuery()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Select V.ReservationKey, V.CheckInDate,V.CheckOutDate,V.DocNo,V.Adult,V.Child,Y.RateCode,U.Unit,Amount=sum(Rate), RT.RoomType ");
            builder.Append(" , StatusDesc = CASE V.Status ");
            builder.Append("   WHEN 1  THEN 'Reservation' ");
            builder.Append("   WHEN 2   THEN 'Check In' ");
            builder.Append("   WHEN 10  THEN 'Check Out' ");
            builder.Append("   WHEN 0   THEN 'Pending' ");
            builder.Append("   WHEN -10 THEN 'No Show/Cancel' ");
            builder.Append("  WHEN -1  THEN 'No Show/Cancel'  ");
            builder.Append("   ELSE 'Pending'  END");
            builder.Append(" from Reservation V join RateType Y on V.RateTypeKey=Y.RateTypeKey ");
            builder.Append(" Join Room U on V.RoomKey=U.RoomKey Join RoomType RT on V.RoomTypeKey = RT.RoomTypeKey ");
            builder.Append(" Join ReservationRate T on V.ReservationKey=T.ReservationKey ");
            builder.Append(" Join (select P.* from PostCode P join BillingCode B on P.BillingCodeKey=B.BillingCodeKey where B.Code<>'NOT APPLICABLE')  P on T.PostcodeKey=P.PostcodeKey ");
            builder.Append(" where GuestKey= @GuestKey and T.PostCodeKey not in (Select PostCodeKey from PaymentType) ");
            builder.Append(" Group By V.ReservationKey, V.CheckInDate,V.CheckOutDate,V.DocNo,V.Adult,V.Child,Y.RateCode,U.Unit, RT.RoomType, V.Status ");
            builder.Append(" Order by V.CheckOutDate,V.DocNo ");
            return builder.ToString();
        }
        private static string GetReservationByFolioNumberQuery()
        {
            StringBuilder sbQueryBuilder = new StringBuilder();
            sbQueryBuilder.Append(" SELECT   res.ReservationKey, res.DocDate , res.DocNo , res.GuestKey, res.Status , ");
            sbQueryBuilder.Append("     res.RateTypeKey, res.RoomTypeKey , res.CheckInDate , res.CheckOutDate ,res.Remark , res.PurposeStayKey , res.PreCheckInCount , ");
            sbQueryBuilder.Append("     rate.Description AS RateDescription , ");
            sbQueryBuilder.Append("     room.Seq AS RoomSeq ,  room.Description AS RoomDescription ");
            sbQueryBuilder.Append(" FROM  Reservation res  ");
            sbQueryBuilder.Append(" LEFT JOIN  RateType rate ON  res.RateTypeKey = rate.RateTypeKey ");
            sbQueryBuilder.Append(" LEFT JOIN  RoomType room ON  res.RoomTypeKey = room.RoomTypeKey ");
            sbQueryBuilder.Append(" WHERE  res.DocNo = @DocNo AND res.Status in (1,2); ");
            return sbQueryBuilder.ToString();
        }
        private static string GetCityKey()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT CityKey ");
            sb.Append(" FROM City where City = @city ");
            sb.Append(" Order by City ASC ");
            return sb.ToString();
        }
        private static string GetReservationKey()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ReservationKey ");
            sb.Append(" FROM Reservation where docno = @docno ");
            return sb.ToString();
        }
        private static string GetCountry()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT Nationality ");
            sb.Append(" FROM Nationality where NationalityKey = @NationalityKey ");
            return sb.ToString();
        }
        private static string GetUpdateMainGuestSQL()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Guest ");
            sb.Append(" Set Title = @Title , ");
            sb.Append(" Passport= @Passport, ");
            sb.Append(" Tel= @Tel, ");
            sb.Append(" Address= @Address, ");
            sb.Append(" Postal= @Postal, ");
            sb.Append(" Mobile= @Mobile, ");
            sb.Append(" City = @City, ");
            sb.Append(" NationalityKey = @NationalityKey, ");
            sb.Append(" CountryKey = @CountryKey, ");
            sb.Append(" DOB = @DOB, ");
            sb.Append(" TenantId=@TenantId ");
            sb.Append(" WHERE GuestKey = @GuestKey");
            return sb.ToString();
        }
        private static string GetUpdateReservationMainGuestSQL()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Reservation ");
            sb.Append(" Set  ");
            sb.Append(" PreCheckInCount= @PreCheckInCount ");
            sb.Append(" WHERE ");
            sb.Append("  ReservationKey = @ReservationKey ;");
            return sb.ToString();
        }
        public static string InsertHistory()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert into history ");
            sb.Append(" (HistoryKey,SourceKey, ModuleName, Operation, ChangedDate, Detail, staffkey, TableName,TenantId )  ");
            sb.Append(" Values ");
            sb.Append(" (@HistoryKey,@SourceKey, @ModuleName, @Operation, GETDATE() ,@Detail, null, @TableName,@TenantId )  ");
            return sb.ToString();
        }
        private static string GetUpdateChkOutReservationSQL()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Reservation ");
            sb.Append(" Set  ");
            sb.Append(" Status = 10 ");
            sb.Append(" WHERE ");
            sb.Append("  ReservationKey = @ReservationKey ;");
            return sb.ToString();
        }
        public static string GetChkOutBillingContactBy()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" select ReservationBillingContactKey,AccountKey, ");
            builder.Append(" ReservationKey=coalesce(B.ReservationKey,R.ReservationKey), ");
            builder.Append(" Ledger=coalesce(R.BillTo,B.Billing),B.AccountType, ");
            builder.Append(" Account=coalesce(C.Name,G.Name),Balance=coalesce(R.Balance,0.00) ,Invoice,InvoiceNo ");
            builder.Append(" from ReservationBillingContact B ");
            builder.Append(" Full Outer join (Select ReservationKey,BillTo,Balance=sum(Rate+Tax1+Tax2+Tax3) ");
            builder.Append("\t\t\t\t\t from ReservationRate where ReservationKey=@ReservationKey ");
            builder.Append("\t\t\t\t\t Group By ReservationKey,BillTo) R ");
            builder.Append(" on R.ReservationKey=B.ReservationKey and R.BillTo=B.Billing ");
            builder.Append(" Left Join Guest G on B.AccountKey=G.GuestKey ");
            builder.Append(" Left Join Company C on B.AccountKey=C.CompanyKey ");
            builder.Append(" where B.ReservationKey= @ReservationKey ");
            builder.Append(" Group By ReservationBillingContactKey,AccountKey, ");
            builder.Append(" B.ReservationKey,R.ReservationKey,R.BillTo,B.Billing,B.AccountType,C.Name,G.Name,R.Balance,B.Invoice,B.InvoiceNo ");
            builder.Append(" order by B.Billing ");
            return builder.ToString();
        }
        private static string GetSystemControlQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT * FROM Control ;    ");
            return sb.ToString();
        }
        private static string GetReservationGuestByReservationKeyQuery()
        {
            StringBuilder sbQueryBuilder = new StringBuilder();
            sbQueryBuilder.Append(" SELECT   resg.* , g.Name  ");
            sbQueryBuilder.Append(" FROM  ReservationGuest resg  ");
            sbQueryBuilder.Append("  LEFT JOIN Guest g ON resg.GuestKey = g.GuestKey   ");
            sbQueryBuilder.Append(" WHERE      ");
            sbQueryBuilder.Append(" resg.ReservationKey = @ReservationKey    ");
            sbQueryBuilder.Append(" ORDER BY  g.Name ;");
            return sbQueryBuilder.ToString();
        }
        private static string AddGuest()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into Guest");
            sb.Append("(GuestKey, FirstName, LastName,EMail,DOB,Mobile,Address,Postal,Name,Title,TenantId)");
            sb.Append("values");
            sb.Append("(@GuestKey, @FirstName, @LastName,@EMail,@DOB,@Mobile,@Address,@Postal,@Name,@Title,@TenantId)");
            return sb.ToString();
        }
        #endregion
    }
}
