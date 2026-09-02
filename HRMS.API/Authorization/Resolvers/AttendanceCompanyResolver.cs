using HRMS.Application.Interfaces.Repositories;

namespace HRMS.API.Authorization.Resolvers
{
    public class AttendanceCompanyResolver : ICompanyResolver
    {
        private readonly IAttendanceRepository _attendanceRepository;
        public AttendanceCompanyResolver(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }
        public async Task<int?> ResolveCompanyId (int resourceId)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(resourceId);
            return attendance?.Department?.CompanyId;
        }
    }
}
