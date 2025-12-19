using Abp.Application.Services.Dto;

namespace BEZNgCore.Notifications.Dto;

public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}

