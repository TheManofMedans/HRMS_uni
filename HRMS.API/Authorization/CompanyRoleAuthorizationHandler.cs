using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using HRMS.domain.Enums;
using HRMS.API.Authorization;
namespace HRMS.API.Authentication
{
    public class CompanyRoleAuthorizationHandler : AuthorizationHandler<CompanyRoleRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IServiceProvider _serviceProvider;
        public CompanyRoleAuthorizationHandler(IHttpContextAccessor contextAccessor, IServiceProvider serviceProvider)
        {
            _contextAccessor = contextAccessor;
            _serviceProvider = serviceProvider;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CompanyRoleRequirement requirement)
        {
            if (context.User.IsInRole("SuperAdmin"))
            {
                context.Succeed(requirement);
                return;
            }
            var routevalues = _contextAccessor.HttpContext?.Request.RouteValues;
            if (routevalues is null || !routevalues.TryGetValue(requirement.RouteParameters,out var resourceIdObj))
            {
                return ;
            }
            if (!int.TryParse(resourceIdObj?.ToString(),out var resourceId))
            {
                return;
            }
            var resolver = (ICompanyResolver)_serviceProvider.GetRequiredService(requirement.ResolverType);
            var companyId = await resolver.ResolveCompanyId(resourceId);
            if (companyId == null)
            {
                return;
            }
            var companyRoleClaims = context.User.Claims.Where(c => c.Type == "company_role");
            foreach(var claim in companyRoleClaims)
            {
                var items = claim.Value.Split(':');
                if (items.Length != 2)
                {
                    continue;
                }
                if (!int.TryParse(items[0],out var id))
                {
                    continue;
                }
                if (companyId != id)
                {
                    continue;
                }
                if (!Enum.TryParse<CompanyRole>(items[1],out var currentRole))
                {
                    continue;
                }
                if(CompanyRoleHierarchy.Meets(currentRole,requirement.MinimumRole))
                {
                    context.Succeed(requirement);
                    return;
                }
                
            }
            return;
        }
    }
}
