using Abp.Data;
using Abp.EntityFrameworkCore;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.EntityFrameworkCore;
using BEZNgCore.EntityFrameworkCore.Repositories;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Castle.Core.Internal;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Nancy.Diagnostics;
using Nancy.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BEZNgCore.Authorization.IrepairDal
{
      public class CommondalRepository : BEZNgCoreRepositoryBase<Title, Guid>, ICommondalRepository
    {
        private readonly IActiveTransactionProvider _transactionProvider;
        ConnectionManager _connectionManager;
        public CommondalRepository(IDbContextProvider<BEZNgCoreDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
            _connectionManager = new ConnectionManager(dbContextProvider, _transactionProvider);
        }


        #region interfaceimplement & related function 
        public int InsertWOImage(WOImage image)
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
            using (var command = _connectionManager.CreateCommandSP(InsertWOImageQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int InsertLfImage(LfImage image)
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
                 new SqlParameter("@LostFoundKey", SqlDbType.UniqueIdentifier)
                 {
                     Value = image.LostFoundKey
                 },
                 new SqlParameter("@Signature", SqlDbType.VarBinary)
                 {
                     Value = image.Signature
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(InsertLfImageQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        public DataTable GetDocumentByLFKey(Guid id)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetDocumentByLFKeyQuery(id), CommandType.Text, MultiTenancySide))
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
        public DataTable GetDocumentByLFhmsKey(Guid id)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly(GetDocumentByLFhmsKeyQuery(id), CommandType.Text, MultiTenancySide))
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
        public int CheckLfImage(LfImage image)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
                
                new SqlParameter("@Sort", SqlDbType.Int)
                {
                     Value =  image.Sort
                },
                new SqlParameter("@LostFoundKey", SqlDbType.UniqueIdentifier)
                {
                     Value = image.LostFoundKey
                }
                
           };
            using (var command = _connectionManager.CreateCommandSP(CheckLfImageQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    adapter.Fill(ds);
                }
                if (ds.Tables.Count == 1)
                {
                    intRowAffected= ds.Tables[0].Rows.Count;

                }
            }
            return intRowAffected;
        }

        public int UpdateLfImage(LfImage image)
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
                 new SqlParameter("@LostFoundKey", SqlDbType.UniqueIdentifier)
                 {
                     Value = image.LostFoundKey
                 },
                 new SqlParameter("@Signature", SqlDbType.VarBinary)
                 {
                     Value = image.Signature
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(UpdateLfImageQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
           
            return intRowAffected;
        }

        public int CheckLfImagehms(LostFoundImageDto image)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {

                new SqlParameter("@LostFoundKey", SqlDbType.UniqueIdentifier)
                {
                     Value = image.LostFoundKey
                }

           };
            using (var command = _connectionManager.CreateCommandSP(CheckLfImagehmsQuery(), CommandType.Text, MultiTenancySide, parameters))
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

        public int InsertLfImagehms(LostFoundImageDto image)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@LostFoundImageKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  image.LostFoundImageKey
                 },
                new SqlParameter("@LostFoundKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  image.LostFoundKey
                 },
                new SqlParameter("@LostFoundImage", SqlDbType.VarChar)
                {
                     Value =  image.LostFoundImage
                 },
                new SqlParameter("@LostFoundImages", SqlDbType.Image)
                {
                     Value =  image.LostFoundImages!= null?image.LostFoundImages:DBNull.Value
                 },
                new SqlParameter("@LostFoundImages2", SqlDbType.Image)
                {
                    Value = image.LostFoundImages2!= null?image.LostFoundImages2:DBNull.Value
                },
                 new SqlParameter("@LostFoundImages3", SqlDbType.Image)
                 {
                     Value = image.LostFoundImages3!= null?image.LostFoundImages3:DBNull.Value
                 },
                  new SqlParameter("@LostFoundImages4", SqlDbType.Image)
                {
                    Value = image.LostFoundImages4!= null?image.LostFoundImages4:DBNull.Value
                },
                 new SqlParameter("@LostFoundImages5", SqlDbType.Image)
                 {
                      Value =image.LostFoundImages5 != null?image.LostFoundImages5:DBNull.Value
                    
                 },
                 new SqlParameter("@CreatedUser", SqlDbType.UniqueIdentifier)
                 {
                     Value = image.CreatedUser
                 }, new SqlParameter("@TenantId", SqlDbType.Int)
                {
                    Value = image.TenantId
                }
           };
            using (var command = _connectionManager.CreateCommandSP(InsertLfImagehmsQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int UpdateLfImagehms(LostFoundImageDto image)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
           {
              
                new SqlParameter("@LostFoundKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  image.LostFoundKey
                 },
                new SqlParameter("@LostFoundImage", SqlDbType.VarChar)
                {
                     Value =  image.LostFoundImage
                 },
                new SqlParameter("@LostFoundImages", SqlDbType.Image)
                {
                     Value =  image.LostFoundImages!= null?image.LostFoundImages:DBNull.Value
                 },
                new SqlParameter("@LostFoundImages2", SqlDbType.Image)
                {
                    Value = image.LostFoundImages2!= null?image.LostFoundImages2:DBNull.Value
                },
                 new SqlParameter("@LostFoundImages3", SqlDbType.Image)
                 {
                     Value = image.LostFoundImages3!= null?image.LostFoundImages3:DBNull.Value
                 },
                  new SqlParameter("@LostFoundImages4", SqlDbType.Image)
                {
                    Value = image.LostFoundImages4!= null?image.LostFoundImages4:DBNull.Value
                },
                 new SqlParameter("@LostFoundImages5", SqlDbType.Image)
                 {
                      Value =image.LostFoundImages5 != null?image.LostFoundImages5:DBNull.Value

                 },
                 new SqlParameter("@CreatedUser", SqlDbType.UniqueIdentifier)
                 {
                     Value = image.CreatedUser
                 }
           };
            using (var command = _connectionManager.CreateCommandSP(UpdateLfImagehmsQuery(image), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
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
        public DataTable GetReservationRequestByGuestKey(DateTime requestDate, DateTime toDate,int status, string requestTypeKey)
        {
            DataTable dt = new DataTable();
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);

            bool blnByStatus = false;
            bool blnrequestTypeKey = false;
            if (status>-2)
                blnByStatus = true;
            if (!string.IsNullOrEmpty(requestTypeKey) && !requestTypeKey.Equals(Guid.Empty.ToString()))
                blnrequestTypeKey = true;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Status", SqlDbType.Int)
                {
                    Value = blnByStatus==true?status:DBNull.Value
                },
                 new SqlParameter("@requestTypeKey", SqlDbType.UniqueIdentifier)
                {
                     Value = blnrequestTypeKey==true?Guid.Parse(requestTypeKey):DBNull.Value
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
            using (var command = _connectionManager.CreateCommandSP(GetReservationRequestByGuestKeyQuery(blnByStatus, blnrequestTypeKey), CommandType.Text, MultiTenancySide, parameters))
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

        public HReservationRequestOutput GetReservationRequestNew(string reservationRequestKey)
        {
            HReservationRequestOutput o = new HReservationRequestOutput();

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
                    o.statusCode = Convert.IsDBNull(row["StatusDesc"]) ? null : Convert.ToString(row["StatusDesc"]);
                    o.statusDesc = o.statusCode == "Cancelled request" ? "CANCELLED" : o.statusCode == "Open request" ? "OPEN" : o.statusCode == "In Progress request" ? "IN PROGRESS" : "COMPLETED";
                    o.btnEdit = o.Status == 10 ? false : true;
                    o.RequestTypeKey = (Guid)row["RequestTypeKey"];
                    o.RequestTypeName = Convert.IsDBNull(row["RequestTypeName"]) ? null : Convert.ToString(row["RequestTypeName"]);
                    o.GuestKey = (Guid)row["GuestKey"];
                    o.ReservationKey = (Guid)row["ReservationKey"];
                    o.GuestRequest = Convert.IsDBNull(row["GuestRequest"]) ? null : Convert.ToString(row["GuestRequest"]);
                    o.RequestDate = Convert.IsDBNull(row["RequestDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(row["RequestDate"]));
                    o.HotelResponse = Convert.IsDBNull(row["HotelResponse"]) ? null : Convert.ToString(row["HotelResponse"]);
                    o.ResponseDate = Convert.IsDBNull(row["ResponseDate"]) ? ((DateTime?)(nullable = null)) : new DateTime?(Convert.ToDateTime(row["ResponseDate"]));
                    o.FolioNo = Convert.IsDBNull(row["Docno"]) ? null : Convert.ToString(row["Docno"]);
                    o.unit = Convert.IsDBNull(row["Unit"]) ? null : Convert.ToString(row["Unit"]);
                    o.GuestName = Convert.IsDBNull(row["Name"]) ? null : Convert.ToString(row["Name"]);
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
        public int UpdateRequestGuest(HReservationRequestInput a)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            bool HotelResponse = false;
            if (!string.IsNullOrEmpty(a.HotelResponse))
                HotelResponse = true;
            SqlParameter[] parameters = new SqlParameter[]
          {
                 new SqlParameter("@ReservationRequestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =   Guid.Parse(a.ReservationRequestKey.ToString())
                 },
                 new SqlParameter("@Status", SqlDbType.Int)
                 {
                     Value =   a.Status
                 },
                 new SqlParameter("@RequestTypeKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  a.RequestTypeKey
                 },
                 new SqlParameter("@HotelResponse", SqlDbType.VarChar)
                 {
                     Value = a.HotelResponse
                 },
                 new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier)
                 {
                     Value =   a.ModifiedBy
                 }
                 //,
                 //new SqlParameter("@TenantId", SqlDbType.Int)
                 //{
                 //    Value =   a.TenantId
                 //}

          };
            using (var command = _connectionManager.CreateCommandSP(UpdateGuestRequest(HotelResponse), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int UpdateAutoComplete(HReservationRequestInput a)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
          {
                 new SqlParameter("@ReservationRequestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =   a.ReservationRequestKey
                 },
                 new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier)
                 {
                     Value =   a.ModifiedBy
                 }
                 //,
                 //new SqlParameter("@TenantId", SqlDbType.Int)
                 //{
                 //    Value =   a.TenantId
                 //}

          };
            using (var command = _connectionManager.CreateCommandSP(UpdateAutoCompleteGuestRequest(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }

        public int UpdateGuestRequestStatus(HReservationRequestInput a)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
          {
                 new SqlParameter("@ReservationRequestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =   a.ReservationRequestKey
                 },
                 new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier)
                 {
                     Value =   a.ModifiedBy
                 }
                 ,
                 new SqlParameter("@Status", SqlDbType.Int)
                 {
                     Value =   a.Status
                 }

          };
            using (var command = _connectionManager.CreateCommandSP(UpdateGuestRequestStatusQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        public int SaveRequestGuest(HSReservationRequestInput a)
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            SqlParameter[] parameters = new SqlParameter[]
          {
                 new SqlParameter("@ReservationRequestKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =   Guid.Parse(a.ReservationRequestKey.ToString())
                 },
                 new SqlParameter("@RequestTypeKey", SqlDbType.UniqueIdentifier)
                 {
                     Value =  a.RequestTypeKey
                 },
                 new SqlParameter("@HotelResponse", SqlDbType.VarChar)
                 {
                     Value = a.HotelResponse
                 },
                 new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier)
                 {
                     Value =   a.ModifiedBy
                 }
          };
            using (var command = _connectionManager.CreateCommandSP(SaveGuestRequestQuery(), CommandType.Text, MultiTenancySide, parameters))
            {
                intRowAffected = command.ExecuteNonQuery();
            }
            return intRowAffected;
        }
        public int GetOpenProgressCount()
        {
            int intRowAffected = 0;
            _connectionManager.EnsureConnectionOpen(MultiTenancySide);
            using (var command = _connectionManager.CreateCommandOnly("select COUNT(*) AS TotalCount from ReservationRequest where Status=1 or Status=2", CommandType.Text, MultiTenancySide))
            {
                object obj = command.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    intRowAffected = Convert.ToInt32(obj.ToString());
                }

            }
          
            return intRowAffected;
        }
        #endregion
        #region sqlquery
        private static string SaveGuestRequestQuery()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE ReservationRequest SET RequestTypeKey=@RequestTypeKey,HotelResponse=@HotelResponse,ModifiedBy=@ModifiedBy WHERE ReservationRequestKey=@ReservationRequestKey");//,TenantId=@TenantId
            return builder.ToString();

        }

        private static string UpdateGuestRequestStatusQuery()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE ReservationRequest SET Status=@Status,ModifiedBy=@ModifiedBy WHERE ReservationRequestKey=@ReservationRequestKey");//,TenantId=@TenantId
            return builder.ToString();

        }
        private static string UpdateAutoCompleteGuestRequest()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE ReservationRequest SET Status=10,ModifiedBy=@ModifiedBy,ResponseDate=GETDATE() WHERE ReservationRequestKey=@ReservationRequestKey");//,TenantId=@TenantId
            return builder.ToString();

        }
        private static string UpdateGuestRequest(bool HotelResponse)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE ReservationRequest SET Status=@Status,RequestTypeKey=@RequestTypeKey,ModifiedBy=@ModifiedBy");//,TenantId=@TenantId
            if (HotelResponse)
                builder.Append(",HotelResponse =@HotelResponse");
            builder.Append(" WHERE ReservationRequestKey=@ReservationRequestKey");
            return builder.ToString();

        }

        private static string getReservationRequestGuest()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select ReservationRequestKey, ReservationRequestID, ReservationRequest.Status,Reservation.Docno,ReservationRequest.RequestTypeKey,RequestType.RequestTypeName, ");
            sb.Append("ReservationRequest.GuestKey,ReservationRequest.ReservationKey,Guest.Name,StatusDesc = CASE ReservationRequest.Status WHEN -1  THEN 'Cancelled request' WHEN 1    THEN 'Open request'  WHEN 2 ");
            sb.Append("THEN 'In Progress request'  WHEN 10  THEN 'Completed request' ELSE 'Pending'  END,U.Unit, ");
            sb.Append("GuestRequest,RequestDate,HotelResponse,ResponseDate from ReservationRequest inner join RequestType on ReservationRequest.RequestTypeKey = RequestType.RequestTypeKey ");
            sb.Append("inner join Reservation on ReservationRequest.ReservationKey = Reservation.ReservationKey inner join Guest on ReservationRequest.GuestKey = Guest.GuestKey inner Join Room U on Reservation.RoomKey = U.RoomKey ");
            sb.Append("where ReservationRequestKey =@ReservationRequestKey");
            //sb.Append(" select ReservationRequestKey,ReservationRequestID,Status,ReservationRequest.RequestTypeKey,RequestType.RequestTypeName,GuestKey,ReservationKey,");
            //sb.Append(" GuestRequest,RequestDate,HotelResponse,ResponseDate from ReservationRequest inner join RequestType on ReservationRequest.RequestTypeKey=RequestType.RequestTypeKey");
            //sb.Append(" where ReservationRequestKey=@ReservationRequestKey ");
            return sb.ToString();
        }
        private static string GetReservationRequestByGuestKeyQuery(bool blnByStatus, bool blnrequestTypeKey)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Select rr.ReservationRequestKey, rr.ReservationRequestID,Gu.Name, V.ReservationKey, rr.RequestDate,rr.ResponseDate,V.DocNo,V.Adult,V.Child,Y.RateCode,U.Unit,req.RequestTypeName ");
            builder.Append(" , StatusDesc = CASE rr.Status WHEN -1  THEN 'Cancelled request' WHEN 1    THEN 'Open request'  WHEN 2   THEN 'In Progress request'  WHEN 10  THEN 'Completed request' ELSE 'Pending'  END,rr.GuestRequest,rr.HotelResponse ");
            builder.Append(" from ReservationRequest rr join Reservation V on v.ReservationKey=rr.ReservationKey join RateType Y on V.RateTypeKey=Y.RateTypeKey  ");
            builder.Append(" Join Room U on V.RoomKey=U.RoomKey Join RoomType RT on V.RoomTypeKey = RT.RoomTypeKey Join RequestType req on req.RequestTypeKey=rr.RequestTypeKey Join Guest Gu On Gu.GuestKey=V.GuestKey ");
            builder.Append("  where convert(varchar(10),rr.RequestDate, 120) Between convert(varchar(10), @RequestDate, 120) And convert(varchar(10), @ToDate, 120) ");
            if (blnByStatus)
                builder.Append("  AND  rr.Status =@Status  ");
            if (blnrequestTypeKey)
                builder.Append("  AND  rr.RequestTypeKey = @requestTypeKey ");
            builder.Append(" Order by rr.RequestDate ");
            return builder.ToString();
        }
        private static string GetRequestType()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT RequestTypeKey, RequestTypeName ");
            sb.Append(" FROM RequestType");
            sb.Append(" Order by Sort ");
            return sb.ToString();
        }
        private static string UpdateLfImagehmsQuery(LostFoundImageDto image)
        {
            string sql = "Update LostFoundImage set [LostFoundImage]=@LostFoundImage,[ModifiedDate]=GETDATE(),[CreatedUser]=@CreatedUser";
            if (image.LostFoundImages != null) 
            {
                sql = sql + ",[LostFoundImages]= @LostFoundImages";
            }
            if (image.LostFoundImages2 != null)
            {
                sql = sql + ",[LostFoundImages2]=@LostFoundImages2";
            }
            if (image.LostFoundImages3 != null)
            {
                sql = sql + ",[LostFoundImages3]=@LostFoundImages3";
            }
            if (image.LostFoundImages4 != null)
            {
                sql = sql + ",[LostFoundImages4]=@LostFoundImages4";
            }
            if (image.LostFoundImages5 != null)
            {
                sql = sql + ",[LostFoundImages5]=@LostFoundImages5";
            }
            sql = sql + " where LostFoundKey=@LostFoundKey";
            return sql;
           /* StringBuilder sb = new StringBuilder();
            sb.Append(" Update LostFoundImage set [LostFoundImage]=@LostFoundImage,[ModifiedDate]=GETDATE(),[CreatedUser]=@CreatedUser,[LostFoundImages]=@LostFoundImages, ");
            sb.Append(" [LostFoundImages2]=@LostFoundImages2,[LostFoundImages3]=@LostFoundImages3,[LostFoundImages4]=@LostFoundImages4,[LostFoundImages5]=@LostFoundImages5 where LostFoundKey=@LostFoundKey");
            return sb.ToString();*/
        }
        private static string InsertLfImagehmsQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert Into LostFoundImage ");
            sb.Append(" ([LostFoundImageKey],[LostFoundKey], [LostFoundImage], [CreatedDate],[CreatedUser], [LostFoundImages],[LostFoundImages2],[LostFoundImages3],[LostFoundImages4],[LostFoundImages5],[TenantId])");
            sb.Append(" Values (@LostFoundImageKey,@LostFoundKey, @LostFoundImage, GETDATE(), @CreatedUser, @LostFoundImages,@LostFoundImages2,@LostFoundImages3,@LostFoundImages4,@LostFoundImages5,@TenantId)");
          
            return sb.ToString();
        }
        private static string CheckLfImagehmsQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select [LostFoundKey] from LostFoundImage ");
            sb.Append(" where LostFoundKey=@LostFoundKey ");
            return sb.ToString();
        }
        private static string CheckLfImageQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select [sort] from Document ");
            sb.Append(" where Sort=@Sort and LostFoundKey=@LostFoundKey ");
            return sb.ToString();
        }
        private static string UpdateLfImageQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Update Document set [LastModifiedStaff]=@LastModifiedStaff,[Document]=@Document,[Description]=@Description,[Signature]=@Signature ");
            sb.Append(" where Sort=@Sort and LostFoundKey=@LostFoundKey");
            return sb.ToString();
        }
        private static string GetDocumentByLFhmsKeyQuery(Guid id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select [LostFoundImage],[LostFoundImages],[LostFoundImages2],[LostFoundImages3],[LostFoundImages4],[LostFoundImages5] from LostFoundImage ");
            sb.Append(" where LostFoundKey='" + id + "'");
            return sb.ToString();
        }
        private static string GetDocumentByLFKeyQuery(Guid id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select [sort],[Description],[Document],[Signature] from Document ");
            sb.Append(" where LostFoundKey='" + id + "'");
            return sb.ToString();
        }
        private static string InsertWOImageQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert into Document ([DocumentKey],[Sort],[LastModifiedStaff],[Document],[Description],[MWorkOrderKey],[Signature]) ");
            sb.Append(" Values (@DocumentKey,@Sort,@LastModifiedStaff,@Document, @Description, @MWorkOrderKey,@Signature) ");
            return sb.ToString();
        }
        private static string InsertLfImageQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert into Document ([DocumentKey],[Sort],[LastModifiedStaff],[Document],[Description],[LostFoundKey],[Signature]) ");
            sb.Append(" Values (@DocumentKey,@Sort,@LastModifiedStaff,@Document, @Description, @LostFoundKey,@Signature) ");
            return sb.ToString();
        }

        #endregion

    }
}
