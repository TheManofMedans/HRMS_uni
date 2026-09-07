using AutoMapper;
using HRMS.Application.DTOs.Notification;
using HRMS.Application.Exceptions;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using HRMS.domain.Entities;
using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        public NotificationService(ICurrentUserService currentUserService, INotificationRepository notificationRepository
            ,ICurrentUserService currentUser, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _notificationRepository = notificationRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }
        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                throw new NotFoundException(nameof(Notification),notificationId);
            }
            notification.isRead = true;
            _notificationRepository.Update(notification);
            var isadded = await _notificationRepository.SaveChangesAsync();
            if (!isadded)
            {
                throw new Exception("Could not update notification!");
            }
            return isadded;
        }
        public async Task<IEnumerable<NotificationResponseDto>> GetMyNotifications(bool unread)
        {
            var Notifications = await _notificationRepository.GetAllAsync();
            var notifs = FilterVisible(Notifications);
            if (unread)
            {
                notifs = notifs.Where(n => n.isRead == false).ToList();
            }
            return _mapper.Map<IEnumerable<NotificationResponseDto>>(notifs);
        }
        public async Task<NotificationResponseDto?> GetByIdAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            return notification is null ? null : _mapper.Map<NotificationResponseDto>(notification);
        }
        public async Task NotifyAsync(int userId, string message, NotificationType Type, int? attendaceId = null, int? requestId = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Type = Type,
                isRead = false,
                RequestId = requestId,
                AttendanceId = attendaceId,
            };
            await _notificationRepository.CreateAsync(notification);
            var isadded = await _notificationRepository.SaveChangesAsync();
            if (!isadded)
            {
                throw new Exception("Could not add the notification to the database!");
            }
        }
        private IEnumerable<Notification> FilterVisible(IEnumerable<Notification> notifications)
        {
            if (_currentUser.IsSuperAdmin)
            {
                return notifications;
            }
            /*List<Notification> notifs = new();
            foreach (var notification in notifications)
            {
                if (notification.UserId == _currentUser.UserId)
                {
                    notifs.Add(notification);
                }
                else
                {
                    if (notification.Attendance != null)
                    {
                        if (!_currentUser.CompanyRoles.TryGetValue(notification.Attendance.Department.CompanyId, out var userRole))
                        {
                            continue;
                        }
                        if (!Enum.TryParse<CompanyRole>(userRole, out var companyRole))
                        {
                            continue;
                        }
                        if (companyRole <= CompanyRole.HRManager)
                        {
                            notifs.Add(notification);
                        }
                    }
                    else if (notification.Request != null)
                    {
                        if (!_currentUser.CompanyRoles.TryGetValue(notification.Request.Department.CompanyId,out var userRole))
                        {
                            continue;
                        }
                        if (!Enum.TryParse<CompanyRole>(userRole,out var companyRole))
                        {
                            continue;
                        }
                        if (companyRole <= CompanyRole.HRManager)
                        {
                            notifs.Add(notification);
                        }
                    }
                }
            }
            return notifs;*/
            return notifications.Where(n => n.UserId == _currentUser.UserId || (n.Attendance != null 
            && _currentUser.CompanyRoles.TryGetValue(n.Attendance.Department.CompanyId,out var userRole) 
            && Enum.TryParse<CompanyRole>(userRole,out var actualRole) && actualRole <= CompanyRole.HRManager)
            || (n.Request != null && _currentUser.CompanyRoles.TryGetValue(n.Request.Department.CompanyId,out var reqUserRole)
            && Enum.TryParse<CompanyRole>(reqUserRole,out var reqRole) && reqRole <= CompanyRole.HRManager))
                .ToList();
        }
    }
}
