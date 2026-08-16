using HRMS.API.Authentication;
using HRMS.domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace HRMS.API.Authorization
{
    public class OwnerOrRoleAuthorizationHandler : AuthorizationHandler<OwnershipOrRoleRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IServiceProvider _serviceProvider;
        public OwnerOrRoleAuthorizationHandler(IHttpContextAccessor contextAccessor, IServiceProvider serviceProvider)
        {
            _contextAccessor = contextAccessor;
            _serviceProvider = serviceProvider;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnershipOrRoleRequirement requirement)
        {
            if (context.User.IsInRole("SuperAdmin"))
            {
                context.Succeed(requirement);
                return;
            }
            var routeValues = _contextAccessor.HttpContext?.Request.RouteValues;
            if (routeValues == null ||! routeValues.TryGetValue(requirement.RouteParameterName,out var resourceIdObj))
            {
                return;
            }
            if (!int.TryParse(resourceIdObj?.ToString(), out var resourceId)) 
            { 
                return; 
            }
            var employeerIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "employee_id");
            if (employeerIdClaim != null &&  int.TryParse(employeerIdClaim.ToString(),out var callerEmployeeId))
            {
                var ownerResolver =(IResourceOwnerResolver) _serviceProvider.GetRequiredService(requirement.OwnerResolverType);
                var employeeId = await ownerResolver.ResolveEmployeeId(resourceId);
                if (employeeId == callerEmployeeId)
                {
                    context.Succeed(requirement);
                    return;
                }
            }
            var CompanyResolver = (ICompanyResolver)_serviceProvider.GetRequiredService(requirement.CompanyResolverType);
            var ownerCompanyId = await CompanyResolver.ResolveCompanyId(resourceId);
            if (ownerCompanyId == null)
            {
                return;
            }
            var userclaims = context.User.Claims.Where(c => c.Type == "company_role").ToList();
            foreach (var claim in userclaims)
            {
                var items = claim.Value.Split(":");
                if (items.Length < 2)
                {
                    continue;
                }
                if (!int.TryParse(items[0], out var claimCompanyId))
                {
                    continue;
                }
                if (claimCompanyId != ownerCompanyId)
                {
                    continue;
                }
                if (!Enum.TryParse<CompanyRole>(items[1], out var CallerRole))
                {
                    continue;
                }
                if (CompanyRoleHierarchy.Meets(CallerRole,requirement.MinimumRole))
                {
                    context.Succeed(requirement);
                    return;
                }
            }
            return;
        }
    }
}
