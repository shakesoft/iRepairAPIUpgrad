using System.Threading.Tasks;
using Abp.Webhooks;

namespace BEZNgCore.WebHooks;

public interface IWebhookEventAppService
{
    Task<WebhookEvent> Get(string id);
}

