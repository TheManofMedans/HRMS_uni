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
        public static IServiceCollection AddCompanyResolvers(this IServiceCollection services)
        {
            var resolverAssembly = typeof(HRMS.API.Authorization.ICompanyResolver).Assembly;
            RegisterCompanyResolvers(services, resolverAssembly);
            return services;
        }
        public static IServiceCollection AddOwnerResolvers(this IServiceCollection services)
        {
            var resolverAssembly = typeof(HRMS.API.Authorization.IResourceOwnerResolver).Assembly;
            RegisterOwnerResolvers(services, resolverAssembly);
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
        public static void RegisterCompanyResolvers(IServiceCollection services,Assembly assembly)
        {
            var candidates = assembly.GetTypes().Where(c => c.IsClass && !c.IsAbstract);
            foreach(var candidate in candidates)
            {
                var interfaces = candidate.GetInterfaces().FirstOrDefault(c => c.Name == "ICompanyResolver");
                if (interfaces != null)
                {
                    services.AddScoped(interfaces, candidate);
                }
            }
        }
        public static void RegisterOwnerResolvers(IServiceCollection services,Assembly assembly)
        {
            var candidates = assembly.GetTypes().Where(c => c.IsClass && !c.IsAbstract);
            foreach (var candidate in candidates)
            {
                var specialinterface = candidate.GetInterfaces().FirstOrDefault(i => i.Name == "IResourceOwnerResolver");
                if (specialinterface != null)
                {
                    services.AddScoped(specialinterface, candidate);
                }
            }
        }
    }
}
