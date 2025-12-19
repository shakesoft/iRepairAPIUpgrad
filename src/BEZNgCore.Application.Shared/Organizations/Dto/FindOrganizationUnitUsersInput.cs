using BEZNgCore.Dto;

namespace BEZNgCore.Organizations.Dto;

public class FindOrganizationUnitUsersInput : PagedAndFilteredInputDto
{
    public long OrganizationUnitId { get; set; }
}

