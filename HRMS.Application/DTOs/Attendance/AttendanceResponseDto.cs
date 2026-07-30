using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Attendance
{
    public class AttendanceResponseDto
    {
        public int Id {  get; set; }
        public EmployeeDto Employee { get; set; }
        public DepartmentDto Department { get; set; }
        public ShiftDto Shift { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ClockedIn {  get; set; }
        public DateTime? ClockedOut { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; }
    }
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class ShiftDto
    {
        public int Id { get; set; }
        public string ShiftName { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
