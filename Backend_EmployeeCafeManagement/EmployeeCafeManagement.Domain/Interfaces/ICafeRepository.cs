using EmployeeCafeManagement.Domain.DTO;
using EmployeeCafeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCafeManagement.Domain.Interfaces
{
    public interface ICafeRepository
    {
        Task<IEnumerable<CafeEmployeeCafeDto>> GetCafesAsync(string location);
        Task<Cafe?> GetByIdAsync(Guid id);
        Task AddCafeAsync(Cafe cafe);
        Task UpdateCafeAsync(Cafe cafe);
        Task DeleteCafeAsync(Cafe cafe);
        Task<bool> CafeExistsAsync(Guid id);
    }
}
