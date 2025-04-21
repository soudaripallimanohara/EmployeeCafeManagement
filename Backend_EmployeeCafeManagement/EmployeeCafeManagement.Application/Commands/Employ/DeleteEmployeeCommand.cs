using EmployeeCafeManagement.Domain.Entities;
using EmployeeCafeManagement.Domain.Interfaces;
using MediatR;


namespace EmployeeCafeManagement.Application.Commands.Employ
{
    public record DeleteEmployeeCommand(Employee employee) : IRequest;

    public class DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository) : IRequestHandler<DeleteEmployeeCommand>
    {
        public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            await employeeRepository.DeleteEmployeeAsync(request.employee);
        }
    }
}
