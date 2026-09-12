using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.domain.Entities
{
    public class PaySlip
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public decimal BasePay { get; set; }
        public decimal AbsenceDeduction { get; set; }
        public decimal LateDeduction { get; set; }
        public decimal UnpaidLeaveDeduction { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal NetPay { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
}
