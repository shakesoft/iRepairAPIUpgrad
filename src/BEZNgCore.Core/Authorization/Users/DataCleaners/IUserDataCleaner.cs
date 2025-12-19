using Abp;
using System.Threading.Tasks;

namespace BEZNgCore.Authorization.Users.DataCleaners;

public interface IUserDataCleaner
{
    Task CleanUserData(UserIdentifier userIdentifier);
}

