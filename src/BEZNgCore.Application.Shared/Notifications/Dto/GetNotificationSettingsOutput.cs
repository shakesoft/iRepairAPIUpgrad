using System.Collections.Generic;

namespace BEZNgCore.Notifications.Dto;

public class GetNotificationSettingsOutput
{
    public bool ReceiveNotifications { get; set; }

    public List<NotificationSubscriptionWithDisplayNameDto> Notifications { get; set; }
}

