using HRMS.Application.Interfaces.Repositories;

namespace HRMS.API.Authorization.Resolvers
{
    public class AttendanceOwnerResolver : IResourceOwnerResolver
    {
        private readonly IAttendanceRepository _attendanceRepository;
        public AttendanceOwnerResolver(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }
        public async Task<int?> ResolveEmployeeId (int resourceId)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(resourceId);
            return attendance?.EmployeeId;
        }
    }
}
