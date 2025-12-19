using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using BEZNgCore.Authorization.IrepairDal;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{
    public class HistoryAppService : BEZNgCoreAppServiceBase
    {
        //private readonly IRepository<History, Guid> _historyRepository;
        //private readonly IRepository<Staff, Guid> _staffRepository;
        private readonly IMworkorderdalRepository _mwordorderdalRepository;
        public HistoryAppService(
            IMworkorderdalRepository mwordorderdalRepository
        //IRepository<History, Guid> historyRepository, 
        //IRepository<Staff, Guid> staffRepository
            )
        {
            _mwordorderdalRepository = mwordorderdalRepository;
            //_historyRepository = historyRepository;
            //_staffRepository = staffRepository;
        }

        //public async Task<PagedResultDto<GetHistoryForICViewDto>> GetTodayHistory()
        //{
        //    var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
        //    Guid staff = user.Result.StaffKey;
        //    var totalCount = 0;
        //    var results = new List<GetHistoryForICViewDto>();
        //    var filteredHistory = _historyRepository.GetAll().Where(x => x.ModuleName.Equals("iClean") && x.StaffKey == staff && x.ChangedDate >= DateTime.Now.AddHours(-24));
        //    if(filteredHistory.Any())
        //    {
        //    totalCount = await filteredHistory.CountAsync();
        //        var pagedAndFilteredHistory = from s in filteredHistory
        //                                      orderby s.ChangedDate descending
        //                                      select s;


        //        var dbList = await pagedAndFilteredHistory.ToListAsync();


        //        foreach (var o in dbList)
        //        {
        //            var res = new GetHistoryForICViewDto()
        //            {
        //                History = new HistoryICDto
        //                {
        //                    ChangedDate = o.ChangedDate,
        //                    Detail = o.Detail,
        //                    Id = o.Id,
        //                    HistoryKey = o.Id,
        //                    TableName = o.TableName
        //                }
        //            };

        //            results.Add(res);
        //        }
        //    }


        //    return new PagedResultDto<GetHistoryForICViewDto>(
        //        totalCount,
        //        results
        //    );

        //}



        [HttpGet]
        public async Task<PagedResultDto<HistoryICleanDto>> GetTodayHistory()
        {
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                Guid staffkey = user.StaffKey;

                List<HistoryICleanDto> dt = new List<HistoryICleanDto>();

                dt = _mwordorderdalRepository.GetIcleanTodayHistory(staffkey);

                var Count = dt.Count;

                return new PagedResultDto<HistoryICleanDto>(
                Count,
                dt
                );
            }
        }
    }
}
