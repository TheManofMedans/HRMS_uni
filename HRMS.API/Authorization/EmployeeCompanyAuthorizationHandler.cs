using HRMS.API.Authentication;
using HRMS.Application.Interfaces.Repositories;
using HRMS.domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace HRMS.API.Authorization
{
    public class EmployeeCompanyAuthorizationHandler : AuthorizationHandler<EmployeeCompanyRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeCompanyAuthorizationHandler(IHttpContextAccessor contextAccessor,IEmployeeRepository employeeRepository)
        {
            _contextAccessor = contextAccessor;
            _employeeRepository = employeeRepository;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,EmployeeCompanyRequirement requirement)
        {
            if (context.User.IsInRole("SuperAdmin"))
            {
                context.Succeed(requirement);
                return;
            }
            var httpcontext = _contextAccessor.HttpContext;
            if (httpcontext == null)
            {
                return;
            }
            if (!httpcontext.Request.RouteValues.TryGetValue("id",out var employeeIdObj))
            {
                return;
            }
            if (!int.TryParse(employeeIdObj?.ToString(),out var employeeId))
            {
                return;
            }
            var employeeIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "employee_id").Value;
            if (employeeIdClaim is not null && int.TryParse(employeeIdClaim,out var calleremployeeId) && calleremployeeId == employeeId)
            {
                context.Succeed(requirement);
                return;
            }
            if (!httpcontext.Request.Query.TryGetValue("companyId",out var companyId))
            {
                return;
            }
            var claims = context.User.Claims.Where(c => c.Type == "company_role").ToList();
            var employee = await _employeeRepository.GetByIdWithDepartmentsAsync(employeeId);
            if (employee == null)
            {
                return;
            }
            bool worksatcompany = employee.EmployeeDepartments.Any(ed => ed.Department.CompanyId == companyId);
            foreach (var claim in claims)
            {
                var items = claim.Value.Split(":");
                if (items.Length != 2)
                {
                    continue;
                }
                if (!int.TryParse(items[0],out var callerCompanyId))
                {
                    continue;
                }
                if (callerCompanyId != companyId)
                {
                    continue;
                }
                if (!Enum.TryParse<CompanyRole>(items[1],out var callerCompanyRole))
                {
                    continue;
                }
                if (CompanyRoleHierarchy.Meets(callerCompanyRole,requirement.minimumRole))
                {
                    context.Succeed(requirement);
                    return;
                }
            }
            return;
        }
    }
}
