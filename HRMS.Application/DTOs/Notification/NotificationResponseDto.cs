using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Notification
{
    public class NotificationResponseDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public UserDto User { get; set; } = null!;
        public AttendanceDto? Attendance { get; set; }
        public ShiftDto? Shift { get; set; }
        public RequestDto? Request { get; set; }
    }
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public CompanyRole? Role { get; set; }
    }
    public class AttendanceDto
    {
        public int Id { get; set; }
        public int departmentId { get; set; }
        public string DepartmentName {  get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
    public class ShiftDto
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
    public class RequestDto
    {
        public int Id { get; set; }
        public RequestType Type { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
