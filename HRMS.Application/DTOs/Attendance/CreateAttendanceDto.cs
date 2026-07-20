using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Attendance
{
    public class CreateAttendanceDto
    {
        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime Date { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; } = AttendanceStatus.Pending;
    }
}
