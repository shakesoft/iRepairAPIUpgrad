using System.Collections.Generic;
using System.Threading.Tasks;
using BEZNgCore.Authorization.Users.Dto;
using BEZNgCore.Dto;

namespace BEZNgCore.Authorization.Users.Exporting;

public interface IUserListExcelExporter
{
    Task<FileDto> ExportToFile(List<UserListDto> userListDtos, List<string> selectedColumns);
}