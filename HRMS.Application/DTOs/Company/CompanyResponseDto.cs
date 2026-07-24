using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Company
{
    public class CompanyResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string RegNum { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<UserDto> Users { get; set; } = new();
    }
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

    }
}
