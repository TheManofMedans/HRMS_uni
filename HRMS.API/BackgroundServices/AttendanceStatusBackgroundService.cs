
using HRMS.Application.Interfaces.Repositories;
using HRMS.domain.Enums;
using HRMS.domain.Entities;
namespace HRMS.API.BackgroundServices
{
    public class AttendanceStatusBackgroundService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromHours(1);
        private readonly TimeSpan graceperiod = TimeSpan.FromMinutes(5);
        public AttendanceStatusBackgroundService (IServiceScopeFactory scopeFactory,ILogger logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating attendance statuses!");
            }
            await Task.Delay(_interval, stoppingToken);
        }
        private async Task UpdateAttendanceStatusAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var attendancerepository = scope.ServiceProvider.GetRequiredService<IAttendanceRepository>();
            var now = DateTime.UtcNow;
            var pendingattendances = await attendancerepository.GetByStatusAsync(AttendanceStatus.Pending);
            foreach ( var attendance in pendingattendances )
            {
                var shiftend = CalculateShiftEnd(attendance.Date,attendance.Shift.StartTime,attendance.Shift.EndTime);
                if (now < shiftend)
                {
                    continue;
                }
            }
        }
        private static DateTime CalculateShiftEnd(DateTime date, TimeSpan shiftstart, TimeSpan shiftend)
        {
            var end = date.Date.Add(shiftstart);
            if (shiftend <= shiftstart)
            {
               end = end.AddDays(1);
            }
            return end;
        }
    }
}
