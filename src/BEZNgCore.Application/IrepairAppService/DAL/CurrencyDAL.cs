using Abp.Domain.Repositories;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService.DAL
{
    public class CurrencyDAL
    {
        IRepository<Currency, Guid> db;
        public CurrencyDAL(IRepository<Currency, Guid> currencyRepository)
        {
            db = currencyRepository;
        }
        public string GetCurrencyKey()
        {
            string strCurrencyKey = Guid.Empty.ToString();
            try
            {
                strCurrencyKey = db.GetAll().Where(x => x.BaseCurrency == 1).Select(x => x.Id).FirstOrDefault().ToString();

            }
            catch (Exception ex)
            {
            }
            return strCurrencyKey;
        }
    }
}
