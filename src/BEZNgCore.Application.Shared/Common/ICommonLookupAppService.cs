using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BEZNgCore.Common.Dto;
using BEZNgCore.Editions.Dto;

namespace BEZNgCore.Common;

public interface ICommonLookupAppService : IApplicationService
{
    Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

    Task<PagedResultDto<FindUsersOutputDto>> FindUsers(FindUsersInput input);

    GetDefaultEditionNameOutput GetDefaultEditionName();
}

