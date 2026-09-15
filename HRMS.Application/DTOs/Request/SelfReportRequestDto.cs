using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Request
{
    public class SelfReportRequestDto
    {
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public int ShiftId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
