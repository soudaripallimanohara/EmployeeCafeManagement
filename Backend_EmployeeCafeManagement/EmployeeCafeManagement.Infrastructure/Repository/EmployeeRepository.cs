using EmployeeCafeManagement.Application.Middleware;
using EmployeeCafeManagement.Application.Middleware.GlobalExceptions;
using EmployeeCafeManagement.Domain.DTO;
using EmployeeCafeManagement.Domain.Entities;
using EmployeeCafeManagement.Domain.Interfaces;
using EmployeeCafeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;


namespace EmployeeCafeManagement.Infrastructure.Repositorys
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<EmployeeCafeDto>> GetEmployeesAsync(Guid? cafe)
        {
            try
            {
                var employees = _context.Employees
                    .Include(e => e.EmployeeCafe)
                        .ThenInclude(ec => ec.Cafe)
                    .AsQueryable();

                if(cafe != null)
                    employees = employees.Where(e => e.EmployeeCafe.CafeId == cafe);

                var result = await employees
                    .Select(e => new
                    {
                        e.Id,
                        e.Name,
                        e.EmailAddress,
                        e.PhoneNumber,
                        e.Gender,
                        StartDate = e.EmployeeCafe.StartDate,
                        CafeName = e.EmployeeCafe.Cafe.Name
                    })
                    .ToListAsync();

                var finalResult = result
                    .Select(e => new EmployeeCafeDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        EmailAddress = e.EmailAddress,
                        PhoneNumber = e.PhoneNumber,
                        Gender = e.Gender,
                        StartDate = e.StartDate,
                        DaysWorked = e.StartDate.HasValue
                            ? (int)(DateTime.Now - e.StartDate.Value).TotalDays
                            : 0,
                        Cafe = e.CafeName
                    })
                    .OrderByDescending(e => e.DaysWorked)
                    .ToList();

                return finalResult;
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Failed to get employees. {ex.Message}");
            }
        }

        public async Task<Employee?> GetByIdAsync(string id)
        {
            try
            {
                var emp = await _context.Employees
                .Include(e => e.EmployeeCafe)
                .FirstOrDefaultAsync(e => e.Id == id);

                return emp;
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Failed to retrieve employee with ID: {id} {ex}");
            }
        }

        public async Task AddEmployeeAsync(EmployeeDto employDto)
        {   
            if (!Regex.IsMatch(employDto.Id, @"^UI[a-zA-Z0-9]{7}$"))
            {
                throw new RepositoryException("Failed to add new employee, Employee identity should be in the format ‘UIXXXXXXX’ where X is alphanumeric");
            }
            if (!Regex.IsMatch(employDto.PhoneNumber, @"^[89]\d{7}$"))
            {
                throw new RepositoryException("Failed to add new employee, Invalid Phone number, Phone must start with 8/9, and 8 digits");
            }

            if (!Regex.IsMatch(employDto.EmailAddress, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new RepositoryException("Failed to add new employee, Invalid Email Id");
            }

            if (!Regex.IsMatch(employDto.Gender, @"^(Male|Female)$"))
            {
                throw new RepositoryException("Failed to add new employee, Gender of the employee (Male/Female)");
            }

            int dateCompare = DateTime.Compare((DateTime)employDto.StartDate, DateTime.Now);

            if (dateCompare > 0)
            {
                throw new RepositoryException("Failed to add new employee, Start date cannot be greater than current date");
            }

            var employee = new Employee
                {
                    Id = employDto.Id.ToUpper(),
                    Name = employDto.Name,
                    EmailAddress = employDto.EmailAddress,
                    PhoneNumber = employDto.PhoneNumber,
                    Gender = employDto.Gender,
                    EmployeeCafe = new EmployeeCafe() { EmployeeId = employDto.Id, CafeId = employDto.CafeId, StartDate = employDto.StartDate }
                };

                if(employee == null)
                {
                    throw new RepositoryException($"Failed to add new employee.");
                }

                _context.Employees.Add(employee);

                await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee, Guid? cafeId, DateTime? startDate)
        { 

            if (!Regex.IsMatch(employee.Id, @"^UI[a-zA-Z0-9]{7}$"))
            {
                throw new RepositoryException("Employee identifier should be in the format ‘UIXXXXXXX’ where X is alphanumeric");
            }
            if (!Regex.IsMatch(employee.PhoneNumber, @"^[89]\d{7}$"))
            {
                throw new RepositoryException("Invalid Phone number, Phone must start with 8/9, and 8 digits");
            }

            if (!Regex.IsMatch(employee.EmailAddress, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new RepositoryException("Invalid Email Id");
            }

            if (!Regex.IsMatch(employee.Gender, @"^(Male|Female)$"))
            {
                throw new RepositoryException("Gender of the employee (Male/Female)");
            }

            int dateCompare = DateTime.Compare((DateTime)startDate, DateTime.Now);

            if (dateCompare > 0)
            {
                throw new RepositoryException("Start date cannot be greater than current date");
            }

            _context.Entry(employee).State = EntityState.Modified;

            var relationship = await _context.EmployeesCafe.FirstOrDefaultAsync(ec => ec.EmployeeId == employee.Id);
            if (relationship != null)
            {
                if (cafeId.HasValue)
                {
                    relationship.CafeId = cafeId.Value;
                }
                if (startDate.HasValue)
                {
                    relationship.StartDate = startDate.Value;
                }
            }
            else if (cafeId.HasValue && startDate.HasValue)
            {
                _context.EmployeesCafe.Add(new EmployeeCafe
                {
                    EmployeeId = employee.Id,
                    CafeId = cafeId.Value,
                    StartDate = startDate.Value
                });
            }

            await _context.SaveChangesAsync();

        }
        public async Task DeleteEmployeeAsync(Employee employee)
        {
            try
            {
                var rel = await _context.EmployeesCafe.FirstOrDefaultAsync(ec => ec.EmployeeId == employee.Id);
                if (rel != null)
                    _context.EmployeesCafe.Remove(rel);

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Failed to delete employee with ID: {employee.Id} {ex}");
            }
        }

        public async Task<bool> EmployeeExistsAsync(string id)
        {
            try
            {
                return await _context.Employees.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Failed to check existence for employee ID: {id} {ex}");
            }
        }
    }
}
