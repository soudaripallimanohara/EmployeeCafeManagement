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
    public record DeleteCafeCommand(Cafe cafe) : IRequest;

    public class DeleteCafeCommandHandler(ICafeRepository cafeRepository) : IRequestHandler<DeleteCafeCommand>
    {
        public async Task Handle(DeleteCafeCommand request, CancellationToken cancellationToken)
        {
            await cafeRepository.DeleteCafeAsync(request.cafe);
        }
    }

}
