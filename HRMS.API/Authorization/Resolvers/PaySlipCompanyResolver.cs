using HRMS.Application.Interfaces.Repositories;

namespace HRMS.API.Authorization.Resolvers
{
    public class PaySlipCompanyResolver : ICompanyResolver
    {
        private readonly IPaySlipRepository _paySlipRepository;
        public PaySlipCompanyResolver(IPaySlipRepository paySlipRepository)
        {
            _paySlipRepository = paySlipRepository;
        }
        public async Task<int?> ResolveCompanyId(int resourceId)
        {
            var paySlip = await _paySlipRepository.GetByIdAsync(resourceId);
            return paySlip?.Department?.CompanyId;
        }
    }
}
