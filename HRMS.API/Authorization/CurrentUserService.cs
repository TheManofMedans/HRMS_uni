using HRMS.Application.Interfaces;
using System.Security.Claims;
namespace HRMS.API.Authorization
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        private ClaimsPrincipal? User => _contextAccessor.HttpContext?.User;
        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
        public bool IsSuperAdmin => User?.IsInRole("SuperAdmin") ?? false;
        public int? EmployeeId
        {
            get
            {
                var claim = User?.Claims?.FirstOrDefault(c => c.Type == "employee_id");
                return claim is not null && int.TryParse(claim.Value,out var employeeId) ? employeeId : null;
            }
        }
        public IReadOnlyDictionary<int, string> CompanyRoles
        {
            get
            {
                var result = new Dictionary<int, string>();
                var claims = User?.Claims.Where(c => c.Type == "company_role") ?? Enumerable.Empty<Claim>();
                foreach (var claim in claims)
                {
                    var items = claim.Value.Split(':');
                    if (items.Length != 2)
                    {
                        continue;
                    }
                    if (!int.TryParse(items[0],out var companyId))
                    {
                        continue;
                    }
                    result[companyId] = items[1];
                }
                return result;
            }
        }
    }
}
