using System.Threading.Tasks;
using BEZNgCore.Sessions.Dto;

namespace BEZNgCore.Web.Session;

public interface IPerRequestSessionCache
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
}

