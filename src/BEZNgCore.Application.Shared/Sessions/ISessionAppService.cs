using System.Threading.Tasks;
using Abp.Application.Services;
using BEZNgCore.Sessions.Dto;

namespace BEZNgCore.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

    Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
}

