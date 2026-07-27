using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.domain.Entities
{
    public class UserCompany
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;
        public CompanyRole Role { get; set; }
        public DateTime JoinedAt = DateTime.UtcNow;
    }
}
