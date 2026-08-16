
using HRMS.Application.Interfaces.Repositories;
using HRMS.domain.Enums;
using HRMS.domain.Entities;
namespace HRMS.API.BackgroundServices
{
    public class AttendanceStatusBackgroundService : BackgroundService
    {
        private readonly ILogger<AttendanceStatusBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromHours(1);
        private readonly TimeSpan graceperiod = TimeSpan.FromMinutes(5);
        private readonly TimeSpan AbsentPeriod = TimeSpan.FromMinutes(30);
        public AttendanceStatusBackgroundService (IServiceScopeFactory scopeFactory,ILogger<AttendanceStatusBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await UpdateAttendanceStatusAsync();
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
            int updatecount = 0;
            foreach ( var attendance in pendingattendances )
            {
                var shiftstart = CalculateShiftStart(attendance.Date, attendance.Shift.StartTime);
                var shiftend = CalculateShiftEnd(attendance.Date,attendance.Shift.StartTime,attendance.Shift.EndTime);
                if (now < shiftend)
                {
                    continue;
                }
                if (now < shiftstart)
                {
                    continue;
                }
                if (attendance.Clockedin is null)
                {
                    attendance.AttendanceStatus = AttendanceStatus.Absent;
                }
                else if (attendance.Clockedout is null)
                { 
                    attendance.AttendanceStatus = AttendanceStatus.NoClockOut;
                }
                else
                {
                    var late = shiftstart.Add(graceperiod);
                    var absent = shiftstart.Add(AbsentPeriod);
                    if (now >= late &&  now < absent)
                    {
                        attendance.AttendanceStatus = AttendanceStatus.Late;
                    }
                    else if (now >= absent)
                    {
                        attendance.AttendanceStatus = AttendanceStatus.Absent;
                    }
                    else
                    {
                        attendance.AttendanceStatus = AttendanceStatus.Present;
                    }
                }
                attendancerepository.Update(attendance);
                updatecount++;
            }
            if (updatecount > 0)
            {
                var isadded = await attendancerepository.SaveChangesAsync();
                if (!isadded)
                {
                    throw new Exception("Could not save changes to attendances!");
                }
            }
            _logger.LogInformation("Attendance status check completed. {Count} records updated.",updatecount);
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
        private static DateTime  CalculateShiftStart(DateTime date,TimeSpan startShift)
        {
            var shiftstart = date.Date.Add(startShift);
            return shiftstart;
        }
    }
}
