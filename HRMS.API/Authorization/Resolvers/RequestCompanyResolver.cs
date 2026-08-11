using HRMS.Application.Interfaces.Repositories;

namespace HRMS.API.Authorization.Resolvers
{
    public class RequestCompanyResolver : ICompanyResolver
    {
        private readonly IRequestRepository _requestRepository;
        public RequestCompanyResolver(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }
        public async Task<int?> ResolveCompanyId(int resourceId)
        {
            var Request = await _requestRepository.GetByIdAsync(resourceId);
            return Request?.Department?.CompanyId;
        }
    }
}
