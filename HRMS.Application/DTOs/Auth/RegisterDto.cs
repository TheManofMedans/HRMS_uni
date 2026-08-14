using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Auth
{
    public class RegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string SSN {  get; set; } = string.Empty;
        public GenderType Gender { get; set; }
        public string Password { get; set; } = string.Empty;
        public int? CompanyId { get; set; }
        public CompanyRole? Role { get; set; }
    }
}
