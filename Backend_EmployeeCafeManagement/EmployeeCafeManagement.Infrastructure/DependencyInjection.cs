using EmployeeCafeManagement.Domain.Interfaces;
using EmployeeCafeManagement.Infrastructure.Data;
using EmployeeCafeManagement.Infrastructure.Repositorys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeCafeManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmployeeCafeInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICafeRepository, CafeRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            return services;
        }
    }
}
