using HRMS.Application.DTOs.Notification;
using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationResponseDto>> GetMyNotifications(bool unreadOnly);
        Task<bool> MarkAsReadAsync(int id);
        Task NotifyAsync(int userId, string message, NotificationType type, int? attendanceId = null, int? requestId = null );
        Task<NotificationResponseDto?> GetByIdAsync(int id);
    }
}
