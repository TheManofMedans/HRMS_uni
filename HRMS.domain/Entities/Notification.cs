using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Enums;
namespace HRMS.domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int? RequestId { get; set; }
        public Request? Request { get; set; }
        public int? AttendanceId { get; set; }
        public Attendance? Attendance { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool isRead { get; set; }
    }
}
