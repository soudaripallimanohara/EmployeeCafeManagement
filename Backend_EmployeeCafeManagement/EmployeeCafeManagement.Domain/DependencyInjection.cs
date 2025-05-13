using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCafeManagement.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmployeeCafeeDomainDI(this IServiceCollection services)
        {
            return services;
        }
    }
}
