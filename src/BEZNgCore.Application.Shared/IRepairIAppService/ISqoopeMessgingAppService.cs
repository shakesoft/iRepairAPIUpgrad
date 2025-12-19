using Abp.Application.Services;
using BEZNgCore.IRepairIAppService.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IRepairIAppService
{
    public interface ISqoopeMessgingAppService : IApplicationService
    {
        //Task SendMessageToSqoope(string msgCode, string roomNo, string loginStaffKey, string attendant, string previousAttendantKey);
        Task<List<MessageNotiView>> SendMessageToSqoope(string msgCode, string roomNo, string loginStaffKey, string attendant, string previousAttendantKey);
        Task<List<MessageNotiView>> SendiRepairMessageToSqoope(string msgCode, List<MWorkOrderInput> listWork, List<BlockRoom> listRoom, string loginStaffKey);
        Task<List<MessageNotiView>> SendiCleanWOMessageToSqoope(string msgCode, List<MWorkOrderInput> listWork, List<BlockRoom> listRoom, string loginStaffKey);
    }
}
