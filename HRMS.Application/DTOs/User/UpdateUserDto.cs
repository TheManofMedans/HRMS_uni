using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.User
{
    public class UpdateUserDto
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? SSN {  get; set; } = string.Empty;
        public GenderType? Gender { get; set; }
        public string? CurrentPass { get; set; } = string.Empty;
        public string? NewPassword {  get; set; } = string.Empty;
    }
}
