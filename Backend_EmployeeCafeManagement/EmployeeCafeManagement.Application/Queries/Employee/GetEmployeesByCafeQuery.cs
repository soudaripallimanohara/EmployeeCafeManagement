using EmployeeCafeManagement.Domain.DTO;
using EmployeeCafeManagement.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCafeManagement.Application.Queries.Employee
{
    public record GetEmployeesByCafeQuery(string? cafe) : IRequest<IEnumerable<EmployeeCafeDto>>;

    public class GetEmployeesByCafeHandler(IEmployeeRepository EmployeeRepository) : IRequestHandler<GetEmployeesByCafeQuery, IEnumerable<EmployeeCafeDto>>
    {
        public Task<IEnumerable<EmployeeCafeDto>> Handle(GetEmployeesByCafeQuery request, CancellationToken cancellationToken)
        {
            return EmployeeRepository.GetEmployeesAsync(request.cafe);
        }
    }
}
