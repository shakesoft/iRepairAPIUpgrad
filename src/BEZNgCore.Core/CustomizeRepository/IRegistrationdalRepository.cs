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
    public interface IRegistrationdalRepository : IRepository<GuestStatus, Guid>
    {
        List<CountryOutput> GenerateDDLCountry();
        List<CityOutput> GenerateDDLCity();
        List<TitleOutput> GenerateDDLTitle();
        List<PurposeOutput> GenerateDDLPurposeofStay();
        DataTable GetReservationByFolioNumber(string folioNumber);
        DocumentSign BindMainGuestSignature(string resKey, string strGuestKey);
        CGuest GetGuestInfoByGuestKey(string guestKey);
        List<ReservationHistory> GetReservationByGuestKey(string GuestKey);
        Guid GetCityKey(string City);
        String GetNationality(string CountryKey);
        int UpdateMainGuestInfo(CGuest guest);
        int UpdateReservationAndMainGuestInfo(string reservationKey, string strPreCheckInCount);
        int InsertHistoryList(List<CHistory> listHistory);
        int UpdateChkOutReservation(string reservationKey);
        DateTime GetBusinessDate();
        DataTable GetChkOutBillingContactBy(string reservationKey);
        DataTable GetReservationGuestByReservationKey(string reservationKey);
        int AddReservationGuest(CGuest guest);
        int UpdateReservationGuest(CGuest guest);
        int RemoveReservationGuest(CGuest guest);
        Guid GetReservationKey(string docno);
        int AddGuest(CGuest guest);
        int UpdateGuestDocument(DocumentSign document);
        int UpdateScreenShootImage(DocumentSign document);
        int InsertGuestDocument(DocumentSign document);
        ReservationDetailOutput GetReservationByReservationKey(string reservationKey);
        List<ReservationRateOutput> GetTransactionByReservationKey(string reservationKey, string pMTDecrypt);
        int InsertGuestEmailList(EmailList el);
        int UpdateEmailHistory(EmailHistory eh);
        int InsertHistory(EmailHistory eh);
        DataTable GetReservationRequestByGuestKey(string guestKey, int cancelReq, int openReq, int inPReq, int completeReq, DateTime requestDate, DateTime toDate);
        string GetReservationRequestID();
        List<RequestTypeOutput> GetAllRequestType();
        ReservationRequestOutput GetReservationRequestNew(string reservationRequestKey);
        string GetGuestName(string guestKey);
        int AddRequestGuest(ReservationRequestInput a);
        int UpdateRequestGuest(ReservationRequestInput a);
    }
}
