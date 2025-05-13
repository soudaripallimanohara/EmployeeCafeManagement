using EmployeeCafeManagement.Domain.DTO;
using EmployeeCafeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCafeManagement.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        //Task<IEnumerable<EmployeeCafeDto>> GetEmployeesAsync(string? cafeName);
        Task<IEnumerable<EmployeeCafeDto>> GetEmployeesAsync(Guid? cafeName);
        Task<Employee?> GetByIdAsync(string id);
        Task AddEmployeeAsync(EmployeeDto employeeDto);
        Task UpdateEmployeeAsync(Employee employee, Guid? cafeId, DateTime? startDate);
        Task DeleteEmployeeAsync(Employee employee);
        Task<bool> EmployeeExistsAsync(string id);
    }
}
