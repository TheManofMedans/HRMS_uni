using HRMS.domain.Enums;
using Microsoft.AspNetCore.Authorization;
namespace HRMS.API.Authentication
{
    public class CompanyRoleRequirement : IAuthorizationRequirement
    {
        public CompanyRole MinimumRole { get; }
        public CompanyRoleRequirement(CompanyRole minimumRole)
        {
            MinimumRole = minimumRole;
        }
    }
}
