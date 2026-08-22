using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Employee
{
    public class RegisterEmployeeDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string SSN {  get; set; } = string.Empty;
        public string Address {  get; set; } = string.Empty;
        public GenderType Gender { get; set; }
        public DateTime HireDate { get; set; }
        public string Password {  get; set; } = string.Empty;
        public int departmentId { get; set; }
        public string Salary { get; set; } = string.Empty;
        //public string? JobDescription {  get; set; } = string.Empty;
        public PayrollStatus PayrollStatus { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }
        public DateTime AssignedAt { get; set; }
    }
}
