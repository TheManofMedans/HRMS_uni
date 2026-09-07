using HRMS.Application.Interfaces.Repositories;

namespace HRMS.API.Authorization.Resolvers
{
    public class NotificationCompanyResolver : ICompanyResolver
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationCompanyResolver(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<int?> ResolveCompanyId(int resourceId)
        {
            var notification = await _notificationRepository.GetByIdAsync(resourceId);
            if (notification?.Attendance != null)
            {
                return notification?.Attendance.Department.CompanyId;
            }
            else if (notification?.Request != null) 
            {
                return notification?.Request.Department.CompanyId;
            }
            return null;
        }
    }
}
