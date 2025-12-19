using BEZNgCore.Dto;

namespace BEZNgCore.WebHooks.Dto;

public class GetAllSendAttemptsInput : PagedInputDto
{
    public string SubscriptionId { get; set; }
}

