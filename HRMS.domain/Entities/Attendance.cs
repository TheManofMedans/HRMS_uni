using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.domain.Entities
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public int ShiftId { get; set; }
        public Shift Shift { get; set; } = null!;
        public int departmentId { get; set; }
        public Department Department { get; set; } = null!;
        public DateTime Date { get; set; }
        public DateTime? Clockedin {  get; set; }
        public DateTime? Clockedout { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; }
    }
}
