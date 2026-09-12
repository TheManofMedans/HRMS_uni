using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.PaySlip
{
    public class PaySlipResponseDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DepartmentDto Department { get; set; } = null!;
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public decimal BasePay { get; set; }
        public decimal AbsenceDeduction { get; set; }
        public decimal LateDeduction { get; set; }
        public decimal UnpaidLeaveDeduction { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal NetPay { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class CompanyPayrollSummaryDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public decimal TotalNetPay { get; set; }
        public List<PaySlipResponseDto> Payslips { get; set; } = new();
    }
}
