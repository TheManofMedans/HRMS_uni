using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Enums;

namespace HRMS.Application.DTOs.Employee
{
    public class UpdateEmployeeDto
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public GenderType? Gender { get; set; }
        public string? JobDescription {  get; set; } = string.Empty;
        public PayrollStatus? PayrollStatus { get; set; }
        public decimal? Salary {  get; set; }
        public EmploymentStatus? EmploymentStatus { get; set; }
    }
}
