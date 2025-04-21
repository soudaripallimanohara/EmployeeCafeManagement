using EmployeeCafeManagement.Application;
using EmployeeCafeManagement.Infrastructure;

namespace EmployeeCafeManagement.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmployeeCafeApiDI(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddEmployeeCafeApplicaionDI()
                .AddEmployeeCafeInfrastructureDI(configuration);

            return services;
        }
    }
}
