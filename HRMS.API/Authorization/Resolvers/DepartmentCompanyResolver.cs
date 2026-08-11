using HRMS.Application.Interfaces.Repositories;

namespace HRMS.API.Authorization.Resolvers
{
    public class DepartmentCompanyResolver : ICompanyResolver
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentCompanyResolver(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<int?> ResolveCompanyId(int resourceId)
        {
            var department = await _departmentRepository.GetByIdAsync(resourceId);
            return department?.CompanyId;
        }
    }
}
