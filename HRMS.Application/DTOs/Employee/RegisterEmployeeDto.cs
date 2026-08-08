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
        public string PhoneNumber { get; set; } = string.Empty;
        public string SSN {  get; set; } = string.Empty;
        public string Address {  get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string Password {  get; set; } = string.Empty;
    }
}
