using Abp.Domain.Repositories;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService.DAL
{
    public class ReservationRateDAL
    {
        IRepository<ReservationRate, Guid> db;
        public ReservationRateDAL(IRepository<ReservationRate, Guid> reservationrateRepository)
        {
            db = reservationrateRepository;
        }

        public int InsertReservationRate(ReservationRate d)
        {
            int intSuccessful = 0;

            try
            {
                Guid id = db.InsertAndGetId(d);
                if (id != Guid.Empty)
                    intSuccessful = 1;
            }
            catch (Exception ex)
            {
            }
            return intSuccessful;
        }
        public List<ReservationRate> check(Guid? reservationKey)
        {
            List<ReservationRate> lst = new List<ReservationRate>();
            lst = db.GetAll()
                         .Where(x => x.ReservationKey == reservationKey)
                         .ToList();
            return lst;
        }
        //public List<ReservationRate> GetSort(DateTime ReservationKey)
        //{
        //    List<ReservationRate> lst = new List<ReservationRate>();
        //    lst = db.GetAll()
        //                 .Where(x => x.ReservationKey == yesterday.Year && x.EnteredDateTime.Value.Date.Month == yesterday.Month && x.EnteredDateTime.Value.Date > yesterday)
        //                 .ToList();
        //    return lst;
        //}
    }
}
