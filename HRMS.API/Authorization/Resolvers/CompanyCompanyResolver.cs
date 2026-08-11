using HRMS.Application.Exceptions;
using HRMS.Application.Interfaces.Repositories;
namespace HRMS.API.Authorization.Resolvers
{
    public class CompanyCompanyResolver : ICompanyResolver
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyCompanyResolver(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task<int?> ResolveCompanyId(int resourceId)
        {
            var company = await _companyRepository.GetByIdAsync(resourceId);
            return company?.Id;
        }
    }
}
