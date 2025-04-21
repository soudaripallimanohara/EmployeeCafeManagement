using EmployeeCafeManagement.Domain.Interfaces;
using EmployeeCafeManagement.Domain.Entities;
using MediatR;


namespace EmployeeCafeManagement.Application.Commands.Caffee
{
    public record CreateCafeCommand(Cafe cafe) : IRequest;

    public class CreateCafeCommandHandler(ICafeRepository cafeRepository) : IRequestHandler<CreateCafeCommand>
    {
        public async Task Handle(CreateCafeCommand request, CancellationToken cancellationToken)
        {
            await cafeRepository.AddCafeAsync(request.cafe);
        }
    }
}