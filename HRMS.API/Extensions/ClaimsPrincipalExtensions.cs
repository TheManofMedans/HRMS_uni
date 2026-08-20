using HRMS.API.Authentication;
using HRMS.API.Authorization;
using HRMS.domain.Enums;
using System.Security.Claims;
namespace HRMS.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool hasSufficientCompanyRole(this ClaimsPrincipal principal,int companyId, CompanyRole minimumRole) 
        {
            var companyRoleClaims = principal.Claims.Where(c => c.Type == "company_role");
            foreach (var claim in companyRoleClaims)
            {
                var items = claim.Value.Split(":");
                if (items.Length != 2)
                {
                    continue;
                }
                if (!int.TryParse(items[0], out var claimCompanyId))
                { 
                    continue; 
                }
                if (!Enum.TryParse<CompanyRole>(items[1],out var claimCompanyRole))
                {
                    continue;
                }
                if (claimCompanyId != companyId)
                {
                    continue;
                }
                if (CompanyRoleHierarchy.Meets(claimCompanyRole,minimumRole))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool HasAnySufficientCompanyRole(this ClaimsPrincipal user, CompanyRole minimumRole)
        {
            var companyRoleClaims = user.Claims.Where(c => c.Type == "company_role");

            foreach (var claim in companyRoleClaims)
            {
                var items = claim.Value.Split(':');
                if (items.Length != 2) continue;
                if (!Enum.TryParse<CompanyRole>(items[1], out var claimRole)) continue;

                if (CompanyRoleHierarchy.Meets(claimRole, minimumRole))
                    return true;
            }

            return false;
        }
    }
}
