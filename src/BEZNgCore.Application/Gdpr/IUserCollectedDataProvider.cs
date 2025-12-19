using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using BEZNgCore.Dto;

namespace BEZNgCore.Gdpr;

public interface IUserCollectedDataProvider
{
    Task<List<FileDto>> GetFiles(UserIdentifier user);
}
