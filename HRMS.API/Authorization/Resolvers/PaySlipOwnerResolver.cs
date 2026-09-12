using HRMS.Application.Interfaces.Repositories;

namespace HRMS.API.Authorization.Resolvers
{
    public class PaySlipOwnerResolver : IResourceOwnerResolver
    {
        private readonly IPaySlipRepository _paySlipRepository;
        public PaySlipOwnerResolver(IPaySlipRepository paySlipRepository)
        {
            _paySlipRepository = paySlipRepository;
        }
        public async Task<int?> ResolveEmployeeId(int resourceId)
        {
            var paySlip = await _paySlipRepository.GetByIdAsync(resourceId);
            return paySlip?.EmployeeId;
        }
    }
}
