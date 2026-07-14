using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.User
{
    public class CreateUserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SSN { get; set; } = string.Empty;
        public string Phone {  get; set; } = string.Empty;
        public int? CompanyId { get; set; }
        public CompanyRole? Role { get; set; }
    }
}
