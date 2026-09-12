
using System.Runtime.CompilerServices;
using HRMS.Application.Exceptions;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
namespace HRMS.API.BackgroundServices
{
    public class PaySlipGenerationBackgrounService : BackgroundService
    {
        private readonly ILogger<PaySlipGenerationBackgrounService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        public PaySlipGenerationBackgrounService(ILogger<PaySlipGenerationBackgrounService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var delay = TimeUntilNextRun();
                await Task.Delay(delay);
                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                try
                {
                    await GeneratePaySlips();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while running weekly payroll batch.");
                }
            }
        }
        private static TimeSpan TimeUntilNextRun()
        {
            var now  = DateTime.UtcNow;
            var runTime = new TimeSpan(2, 0, 0);
            int daysUntilSaturday = ((int)DayOfWeek.Saturday - (int)now.DayOfWeek + 7);
            var nextRun = now.Date.AddDays(daysUntilSaturday).Add(runTime);
            if (nextRun <= now)
            {
                nextRun = nextRun.AddDays(7);
            }
            return nextRun - now;
        }
        private async Task GeneratePaySlips()
        {
            using var scope = _scopeFactory.CreateScope();
            var payslipRepository = scope.ServiceProvider.GetRequiredService<IPaySlipRepository>();
            var payslipService = scope.ServiceProvider.GetRequiredService<IPaySlipService>();
            var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
            var weekStart = DateTime.Today.AddDays(-7);
            var assignments = await employeeRepository.GetAllAssignmentsAsync();
            int succeeded = 0;
            int skipped = 0;
            int failed = 0;
            foreach(var assignment in assignments)
            {
                try
                {
                    await payslipService.GenerateForWeekAsync(assignment.EmployeeID, assignment.DepartmentID, weekStart);
                    succeeded++;
                }
                catch(ConflictException)
                {
                    skipped++;
                }
                catch (Exception ex)
                {
                    failed++;
                    _logger.LogError(ex, "Failed to calculate payroll for employee {EmployeeId}, department {DepartmentId}.",
                        assignment.EmployeeID, assignment.DepartmentID);
                }
            }
            _logger.LogInformation("Weekly payroll batch complete: {Succeeded} succeeded, {Skipped} already existed, {Failed} failed.",
                succeeded, skipped, failed);
        }
    }
}
