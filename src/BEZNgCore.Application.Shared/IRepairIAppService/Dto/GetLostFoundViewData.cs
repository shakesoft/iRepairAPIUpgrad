using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetLostFoundViewData
    {
        public GetLostFoundViewData()
        {
            ItemStatus = new HashSet<DDLStatusICOutput>();
            Room = new HashSet<GetHotelRoom>();
            Area = new HashSet<MareaOutput>();
        }
        public ICollection<DDLStatusICOutput> ItemStatus { get; set; }
        public ICollection<GetHotelRoom> Room { get; set; }
        public ICollection<MareaOutput> Area { get; set; }
    }
    public class GeHGuestRequestViewData
    {
        public GeHGuestRequestViewData()
        {
            GuestRequestStatus = new HashSet<GuestRequestStatusOutput>();
            ddlRequestType = new HashSet<RequestTypeOutput>();
            requestDate = DateTime.Now.AddDays(-7);
            toDate = DateTime.Now;
        }
        public DateTime requestDate { get; set; }
        public DateTime toDate { get; set; }
        public ICollection<GuestRequestStatusOutput> GuestRequestStatus { get; set; }
        public ICollection<RequestTypeOutput> ddlRequestType { get; set; }
    }
    public class HRequestGuestDataEntryOutput
    {
        public HRequestGuestDataEntryViewData RequestGuestDataEntryDropdown { get; set; }
        public HReservationRequestOutput ReservationRequestOutput { get; set; }

    }
    public class HReservationRequestInput
    {
        public HReservationRequestInput() { HotelResponse = ""; }
        public Guid ReservationRequestKey { get; set; }
        public Guid? RequestTypeKey { get; set; }
        public int Status { get; set; }
        public string HotelResponse { get; set; }
        public Guid? ModifiedBy { get; set; }
    }

    public class HSReservationRequestInput
    {
        public HSReservationRequestInput() { HotelResponse = ""; }
        public Guid ReservationRequestKey { get; set; }
        public Guid? RequestTypeKey { get; set; }
        public string HotelResponse { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
    public class HReservationRequestOutput
    {
        public HReservationRequestOutput()
        {
            btnEdit = true;
        }
        public bool btnEdit { get; set; }
        public Guid ReservationRequestKey { get; set; }//
        public string ReservationRequestID { get; set; }//
        public Guid? RequestTypeKey { get; set; }//
        public string RequestTypeName { get; set; }//
        public int Status { get; set; }//
        public string statusDesc { get; set; }
        public string statusCode { get; set; }
        public Guid? ReservationKey { get; set; }//
        public Guid? GuestKey { get; set; }//
        public string GuestRequest { get; set; }//
        public DateTime? RequestDate { get; set; }//
        public string HotelResponse { get; set; }//
        public DateTime? ResponseDate { get; set; }//
        public string FolioNo { get; set; }//Docno
        public string unit { get; set; }
        public string GuestName { get; set; }//Name
    }
    public class HRequestGuestDataEntryViewData
    {
        public HRequestGuestDataEntryViewData()
        {
            GuestRequestStatus = new HashSet<GuestRequestStatusOutput>();
            ddlRequestType = new HashSet<RequestTypeOutput>();
        }
        public ICollection<GuestRequestStatusOutput> GuestRequestStatus { get; set; }
        public ICollection<RequestTypeOutput> ddlRequestType { get; set; }

    }
    public class GuestRequestStatusOutput
    {
        public string GuestRequestStatus { get; set; }
        public string GuestRequestStatusCode { get; set; }

        public string GuestRequestStatusDesc { get; set; }
        public GuestRequestStatusOutput(string status, string code,string des) => (GuestRequestStatus, GuestRequestStatusCode, GuestRequestStatusDesc) = (status, code,des);
    }
    public class HRequestGuestOutput
    {
        public Guid ReservationRequestKey { get; set; }
        public string ReservationRequestID { get; set; }
        public DateTime? RequestDate { get; set; }
        public string RequestDatedes { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string ResponseDatedes { get; set; }
        public string HotelResponse { get; set; }
        public string StatusDesc { get; set; }
        public string StatusCode { get; set; }
        public string GuestRequest { get; set; }
        public string RequestTypeName { get; set; }
        public string Unit { get; set; }
        public string DocNo { get; set; }
        public string Name { get; set; }
    }
    public class MareaOutput
    {
        public int Seqno { get; set; }
        public string Description { get; set; }
    }
}
