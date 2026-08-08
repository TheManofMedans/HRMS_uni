using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Attendance
{
    public class UpdateAttendanceDto
    {
        public DateTime? ClockedIn { get; set; }
        public DateTime? ClockedOut { get; set; }
    }
}
