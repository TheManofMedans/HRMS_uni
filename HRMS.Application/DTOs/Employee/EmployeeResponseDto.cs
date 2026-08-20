using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Employee
{
    public class EmployeeResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string SSN {  get; set; } = string.Empty;
        public GenderType Gender { get; set; }
        public int UserId { get; set; }
        public DateTime HireDate { get; set; }
        public List<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();
    }
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public string Salary {  get; set; } = string.Empty;
        public EmploymentStatus EmployementStatus { get; set; }
        public string JobDescription {  get; set; } = string.Empty;
        public PayrollStatus PayrollStatus { get; set; }
    }
}
