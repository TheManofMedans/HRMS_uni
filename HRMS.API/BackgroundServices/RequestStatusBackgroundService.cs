using HRMS.Application.Interfaces.Repositories;
using HRMS.domain.Enums;

namespace HRMS.API.BackgroundServices
{
    public class RequestStatusBackgroundService : BackgroundService
    {
        private readonly ILogger<RequestStatusBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromHours(1);

        public RequestStatusBackgroundService(ILogger<RequestStatusBackgroundService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                await ChangeRequestStatusAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while updating requests!");
            }
            await Task.Delay(_interval, cancellationToken);
        }
        private async Task ChangeRequestStatusAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var requestRepository = scope.ServiceProvider.GetRequiredService<IRequestRepository>();
            int updated = 0;
            List<RequestType> requestTypes = new List<RequestType>();
            requestTypes.Add(RequestType.Leave);
            requestTypes.Add(RequestType.Remote_Work);
            requestTypes.Add(RequestType.Shift_Change);
            requestTypes.Add(RequestType.Mission);
            var requests = await requestRepository.GetWithTypesAsync(requestTypes);
            foreach ( var request in requests)
            {
                if (request.EndDate <  DateTime.UtcNow)
                {
                    request.Status = RequestStatus.Canceled;
                }
                requestRepository.Update(request);
                updated++;
            }
            if (updated > 0)
            {
                var isadded = await requestRepository.SaveChangesAsync();
                if (!isadded)
                {
                    throw new Exception("Error While updating requests!");
                }
            }
            _logger.LogInformation("Request checks complete! {updated} records updated",updated);

        }
    }
}
