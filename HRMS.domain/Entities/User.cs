using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace HRMS.domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string SSN {  get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public GenderType Gender { get; set; }
        public ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();

    }
}
