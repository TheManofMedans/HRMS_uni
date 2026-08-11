using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using HRMS.domain.Enums;
namespace HRMS.API.Authentication
{
    public class CompanyRoleAuthorizationHandler : AuthorizationHandler<CompanyRoleRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CompanyRoleAuthorizationHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CompanyRoleRequirement requirement)
        {
            var routevalues = _contextAccessor.HttpContext?.Request.RouteValues;
            if (routevalues is null || !routevalues.TryGetValue("companyId", out var companyIdObj))
            {
                return Task.CompletedTask;
            }
            if (!int.TryParse(companyIdObj.ToString(),out var companyId))
            {
                return Task.CompletedTask;
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
                    return Task.CompletedTask;
                }
                
            }
            return Task.CompletedTask;
        }
    }
}
