using HRMS.domain.Enums;
using Microsoft.AspNetCore.Authorization;
namespace HRMS.API.Authentication
{
    public class CompanyRoleRequirement : IAuthorizationRequirement
    {
        public CompanyRole MinimumRole { get; }
        public Type ResolverType { get; }
        public string RouteParameters { get; }
        public CompanyRoleRequirement(CompanyRole minimumRole, Type resolverType, string routeParameters)
        {
            MinimumRole = minimumRole;
            ResolverType = resolverType;
            RouteParameters = routeParameters;
        }
    }
}
