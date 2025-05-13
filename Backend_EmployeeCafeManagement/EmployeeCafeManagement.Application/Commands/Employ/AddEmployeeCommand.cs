using EmployeeCafeManagement.Domain.DTO;
using EmployeeCafeManagement.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCafeManagement.Application.Commands.Employ
{
    public record AddEmployeeCommand(EmployeeDto employDto) : IRequest;

    public class AddEmployeeCommandHanler(IEmployeeRepository EmployeeRepository) : IRequestHandler<AddEmployeeCommand>
    {
        public Task Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            return EmployeeRepository.AddEmployeeAsync(request.employDto);
        }
    }
}
