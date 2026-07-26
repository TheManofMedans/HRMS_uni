using System.Reflection;

namespace HRMS.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var applicationAssembly = typeof(HRMS.Application.Services.EmployeeService).Assembly;
            RegisterByConvention(services, applicationAssembly);
            return services;
        }
        public static IServiceCollection AddInfrustructureServices(this IServiceCollection services)
        {
            var infrastructureAssembly = typeof(HRMS.Infrastructure.Persistence.HRMSDbContext).Assembly;
            RegisterByConvention(services, infrastructureAssembly);
            return services;
        }
        private static void RegisterByConvention(IServiceCollection services, Assembly assembly)
        {
            var candidates = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract);
            foreach (var candidate in candidates)
            {
                var matchingInterface = candidate.GetInterfaces()
                    .FirstOrDefault(c => c.Name == "I" +  candidate.Name);
                if (matchingInterface != null)
                {
                    services.AddScoped(matchingInterface, candidate);
                }
            }
        }
    }
}
