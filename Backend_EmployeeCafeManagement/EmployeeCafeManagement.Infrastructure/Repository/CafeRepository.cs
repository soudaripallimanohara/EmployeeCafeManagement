using EmployeeCafeManagement.Application.Middleware;
using EmployeeCafeManagement.Application.Middleware.GlobalExceptions;
using EmployeeCafeManagement.Domain.DTO;
using EmployeeCafeManagement.Domain.Entities;
using EmployeeCafeManagement.Domain.Interfaces;
using EmployeeCafeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCafeManagement.Infrastructure.Repositorys
{
    public class CafeRepository : ICafeRepository
    {
        private readonly AppDbContext _context;

        public CafeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CafeEmployeeCafeDto>> GetCafesAsync(string location)
        {
            try
            {
                return await _context.Cafes
                    .Include(c => c.EmployeeCafes)
                    .Where(c => string.IsNullOrEmpty(location) || c.Location == location)
                    .Select(c => new CafeEmployeeCafeDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        Logo = c.Logo,
                        Location = c.Location,
                        EmployeesCount = c.EmployeeCafes.Count()
                    })
                    .OrderByDescending(c => c.EmployeesCount)
                    .ToListAsync();
            }
            catch
            {
                throw new RepositoryException("Failed to retrieve cafes.");
            }
        }
        public async Task<Cafe?> GetByIdAsync(Guid id)
        {
            try
            {
                var cafe = await _context.Cafes
                .FirstOrDefaultAsync(e => e.Id == id);

                return cafe;
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Failed to retrieve cafe with ID: {id}");
            }
        }

        public async Task AddCafeAsync(Cafe cafe)
        {
            try
            {
                _context.Cafes.Add(cafe);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Failed to add new cafe. {ex}");
            }
        }
        public async Task UpdateCafeAsync(Cafe cafe)
        {
            try
            {
                _context.Entry(cafe).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Failed to update cafe with ID: {cafe.Id} {ex}");
            }
        }
        public async Task DeleteCafeAsync(Cafe cafe)
        {
            try
            {
                var employeesCafe = _context.EmployeesCafe.Where(ec => ec.CafeId == cafe.Id);

                var employeeIdsToDelete = _context.EmployeesCafe
                .Where(ec => ec.CafeId == cafe.Id)
                .Select(ec => ec.EmployeeId)
                .ToList();

                // Get the matching employee entities
                var employeesToDelete = _context.Employees
                    .Where(e => employeeIdsToDelete.Contains(e.Id))
                    .ToList();

                _context.EmployeesCafe.RemoveRange(employeesCafe);
                // Remove them from the context
                _context.Employees.RemoveRange(employeesToDelete);

                _context.Cafes.Remove(cafe);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Failed to delete cafe with ID: {cafe.Id} {ex}");
            }
        }
        public async Task<bool> CafeExistsAsync(Guid id)
        {
            try
            {
                return await _context.Cafes.AnyAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Failed to check existence for cafe ID: {id} {ex}");
            }
        }
    }
}
