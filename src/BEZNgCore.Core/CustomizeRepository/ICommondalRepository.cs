using Abp.Domain.Repositories;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.CustomizeRepository
{
    public interface ICommondalRepository : IRepository<Title, Guid>
    {
        int CheckLfImage(LfImage image);
        DataTable GetDocumentByLFKey(Guid id);
        DataTable GetDocumentByLFhmsKey(Guid id);
        int InsertLfImage(LfImage image);
        int InsertWOImage(WOImage image);
        int UpdateLfImage(LfImage image);
        int CheckLfImagehms(LostFoundImageDto image);
        int InsertLfImagehms(LostFoundImageDto image);
        int UpdateLfImagehms(LostFoundImageDto image);
        List<RequestTypeOutput> GetAllRequestType();
        DataTable GetReservationRequestByGuestKey(DateTime requestDate, DateTime toDate, int status, string requestTypeKey);
        HReservationRequestOutput GetReservationRequestNew(string reservationRequestKey);
        string GetGuestName(string guestKey);
        int UpdateRequestGuest(HReservationRequestInput a);
        int UpdateAutoComplete(HReservationRequestInput a);
        int UpdateGuestRequestStatus(HReservationRequestInput a);
        int SaveRequestGuest(HSReservationRequestInput a);
        int GetOpenProgressCount(); 
    }
}
