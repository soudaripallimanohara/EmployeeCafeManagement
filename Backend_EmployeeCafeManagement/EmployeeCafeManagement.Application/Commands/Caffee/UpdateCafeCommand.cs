using EmployeeCafeManagement.Domain.Entities;
using EmployeeCafeManagement.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCafeManagement.Application.Commands.Caffee
{
    public record UpdateCafeCommand(Cafe cafe) : IRequest;

    public class UpdateCafeCommandHandler(ICafeRepository cafeRepository) : IRequestHandler<UpdateCafeCommand>
    {
        public async Task Handle(UpdateCafeCommand request, CancellationToken cancellationToken)
        {
            await cafeRepository.UpdateCafeAsync(request.cafe);
        }
    }
}
