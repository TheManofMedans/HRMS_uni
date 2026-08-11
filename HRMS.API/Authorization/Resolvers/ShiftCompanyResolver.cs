using HRMS.Application.Interfaces.Repositories;

namespace HRMS.API.Authorization.Resolvers
{
    public class ShiftCompanyResolver : ICompanyResolver
    {
        private readonly IShiftRepository _shiftRepository;
        public ShiftCompanyResolver(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }
        public async Task<int?> ResolveCompanyId (int resourceId)
        {
            var Shift = await _shiftRepository.GetByIdAsync (resourceId);
            return Shift?.CompanyId;
        }
    }
}
