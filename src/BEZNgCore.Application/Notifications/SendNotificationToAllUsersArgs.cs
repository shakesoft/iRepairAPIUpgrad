using Abp.Notifications;

namespace BEZNgCore.Notifications;

public class SendNotificationToAllUsersArgs
{
    public string NotificationName { get; set; }
    public string Message { get; set; }
    public NotificationSeverity Severity { get; set; }
}