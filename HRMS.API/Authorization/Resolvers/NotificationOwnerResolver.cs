using HRMS.Application.Interfaces.Repositories;

namespace HRMS.API.Authorization.Resolvers
{
    public class NotificationOwnerResolver : IResourceOwnerResolver
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationOwnerResolver(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<int?> ResolveEmployeeId(int resourceId)
        {
            var notification = await _notificationRepository.GetByIdAsync(resourceId);
            if(notification?.Attendance != null)
            {
                return notification?.Attendance.EmployeeId;
            }
            else if (notification?.Request != null)
            {
                return notification?.Request.EmployeeId;
            }
            return null;
        }
    }
}
