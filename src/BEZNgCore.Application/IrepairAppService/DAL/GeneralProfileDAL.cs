using Abp.Domain.Repositories;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService.DAL
{
    public class GeneralProfileDAL
    {
        IRepository<GeneralProfile, Guid> db;
        public GeneralProfileDAL(IRepository<GeneralProfile, Guid> generalprofileRepository)
        {
            db = generalprofileRepository;
        }

        public bool IsGSTInclusive()
        {
            bool blnGSTInclusive = false;
            string ProfileName = "GSTInclusive";
            var obj = db.GetAll().Where(x => x.ProfileName == ProfileName).Select(x => x.ProfileValue).FirstOrDefault();

            if (obj == "True")
            {
                blnGSTInclusive = true;
            }

            return blnGSTInclusive;

        }
        public bool IsUseSqoopeMessagingService()
        {
            bool blnGSTInclusive = false;
            string ProfileName = "SqoopeIntegration";
            var obj = db.GetAll().Where(x => x.ProfileName == ProfileName).Select(x => x.ProfileValue).FirstOrDefault();

            if (obj == "True")
            {
                blnGSTInclusive = true;
            }

            return blnGSTInclusive;
        }

    }
}
