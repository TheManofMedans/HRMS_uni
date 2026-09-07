using HRMS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyNotifications([FromQuery] bool unread)
        {
            var notifications = await _notificationService.GetMyNotifications(unread);
            return Ok(notifications);
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "Notification_OwnOrHREmployee")]
        public async Task<IActionResult> GetNotification(int id)
        {
            var notification = await _notificationService.GetByIdAsync(id);
            return notification is null ? NotFound() : Ok(notification);
        }
    }
}
