using HRMS.domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace HRMS.API.Authorization
{
    public class OwnershipOrRoleRequirement : IAuthorizationRequirement
    {
        public Type CompanyResolverType { get; }
        public Type OwnerResolverType { get; }
        public CompanyRole MinimumRole { get; }
        public string RouteParameterName { get; }
        public OwnershipOrRoleRequirement(Type companyResolverType, Type ownerResolverType, CompanyRole minimumRole, string routeParameters)
        {
            CompanyResolverType = companyResolverType;
            OwnerResolverType = ownerResolverType;
            MinimumRole = minimumRole;
            RouteParameterName = routeParameters;
        }
    }
}
