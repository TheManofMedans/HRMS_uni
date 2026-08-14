using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Employee
{
    public class CreateEmployeeDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public GenderType Gender { get; set; }
        public string SSN { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public int UserId { get; set; }
        //public List<int?> DepartmentIds { get; set; }
    }
}
