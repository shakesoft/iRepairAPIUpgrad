using System.Threading.Tasks;
using BEZNgCore.Authorization.Users;

namespace BEZNgCore.WebHooks;

public interface IAppWebhookPublisher
{
    Task PublishTestWebhook();
}

