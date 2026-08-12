using HRMS.Application.Interfaces.Repositories;

namespace HRMS.API.Authorization.Resolvers
{
    public class RequestOwnerResolver : IResourceOwnerResolver
    {
        private readonly IRequestRepository _requestRepository;
        public RequestOwnerResolver(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }
        public async Task<int?> ResolveEmployeeId(int resourceId)
        {
            var request = await _requestRepository.GetByIdAsync(resourceId);
            return request?.EmployeeId;
        }
    }
}
