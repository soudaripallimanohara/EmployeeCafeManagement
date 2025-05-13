using EmployeeCafeManagement.Domain.Entities;
using EmployeeCafeManagement.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCafeManagement.Application.Commands.Employ
{
    public record UpdateEmployeeCommand(Employee employee, Guid? cafeId, DateTime? startDate) : IRequest;

    public class UpdateEmployeeCommandHandler(IEmployeeRepository EmployeeRepository) : IRequestHandler<UpdateEmployeeCommand>
    {
        public Task Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            return EmployeeRepository.UpdateEmployeeAsync(request.employee, request.cafeId, request.startDate);
        }
    }
}
