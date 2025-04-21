using EmployeeCafeManagement.Domain.DTO;
using EmployeeCafeManagement.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCafeManagement.Application.Queries.Cafe
{
    public record GetCafesByLocationQuery(string location) : IRequest<IEnumerable<CafeEmployeeCafeDto>>;

    public class GetCafesByLocationHandler(ICafeRepository cafeRepository) : IRequestHandler<GetCafesByLocationQuery, IEnumerable<CafeEmployeeCafeDto>>
    {
        public async Task<IEnumerable<CafeEmployeeCafeDto>> Handle(GetCafesByLocationQuery request, CancellationToken cancellationToken)
        {
            return await cafeRepository.GetCafesAsync(request.location);
        }
    }
}
