using Abp.Domain.Repositories;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService.DAL
{
    public class ReservationDAL
    {
        IRepository<Reservation, Guid> db;
        IRepository<Guest, Guid> dbguest;
        public ReservationDAL(IRepository<Reservation, Guid> reservationRepository, IRepository<Guest, Guid> guestRepository)
        {
            db = reservationRepository;
            dbguest = guestRepository;
        }

        public List<ReservationOutput> GetReservationByRoomKey(string roomKey)
        {
            List<ReservationOutput> lst = new List<ReservationOutput>();
            try
            {
                Guid r = new Guid(roomKey);
                lst = (from a in db.GetAll()
                       where a.Status == 2 && a.RoomKey == r
                       join u in dbguest.GetAll() on a.GuestKey equals u.Id
                       select new ReservationOutput
                       {
                           ReservationKey = a.Id,
                           DocNo = a.DocNo,
                           CheckInDate = a.CheckInDate,
                           CheckOutDate = a.CheckOutDate,
                           GuestKey = a.GuestKey,
                           GuestName = u.Name,
                           RoomKey = a.RoomKey
                       }).ToList();

            }
            catch { }
            return lst;
        }
        public string GetReservationKeyByRoomKey(string roomKey)
        {
            string strReservationKey = Guid.Empty.ToString();
            try
            {
                Guid r = new Guid(roomKey);

                strReservationKey = db.GetAll().Where(x => x.Status == 2 && x.RoomKey == r)
                   .Select(x => x.Id).FirstOrDefault().ToString();


            }
            catch { }
            return strReservationKey;
        }

    }
}
