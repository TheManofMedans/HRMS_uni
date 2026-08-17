using HRMS.domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace HRMS.API.Authorization
{
    public class EmployeeCompanyRequirement : IAuthorizationRequirement
    {
        public CompanyRole minimumRole { get; }
        public EmployeeCompanyRequirement(CompanyRole minimumRole)
        {
            this.minimumRole = minimumRole;
        }

    }
}
