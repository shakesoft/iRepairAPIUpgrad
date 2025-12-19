using Abp.Application.Services;
using BEZNgCore.IRepairIAppService.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IRepairIAppService
{
    public interface IMsgNotificationAppService : IApplicationService
    {
        Task<List<string>> SendNotiiClean(MessageNotiWeb input);
        Task<List<string>> SendNotiiRepair(MessageNotiWeb input);
    }
}
