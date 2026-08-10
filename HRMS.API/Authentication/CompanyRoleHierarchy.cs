using HRMS.domain.Enums;
using Microsoft.AspNetCore.HttpOverrides;
namespace HRMS.API.Authentication
{
    public static class CompanyRoleHierarchy
    {
        private static Dictionary<CompanyRole, int> Rank = new()
        {
            {CompanyRole.CEO,0 },
            {CompanyRole.HRManager,1 },
            {CompanyRole.HREmployee,2 },
            {CompanyRole.View_Only,3 }
        };
        public static bool Meets(CompanyRole actual, CompanyRole required)
        {
            return (Rank[actual] <= Rank[required]);
        }
    }
}
