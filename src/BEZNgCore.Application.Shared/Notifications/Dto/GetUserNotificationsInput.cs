using System;
using Abp.Notifications;
using BEZNgCore.Dto;

namespace BEZNgCore.Notifications.Dto;

public class GetUserNotificationsInput : PagedInputDto
{
    public UserNotificationState? State { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}

