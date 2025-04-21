using EmployeeCafeManagement.Application.Commands.Caffee;
using EmployeeCafeManagement.Application.Middleware.GlobalExceptions;
using EmployeeCafeManagement.Application.Queries.Cafe;
using EmployeeCafeManagement.Domain.DTO;
using EmployeeCafeManagement.Domain.Entities;
using EmployeeCafeManagement.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeCafeManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CafeController(ICafeRepository _cafeRepo, ISender _sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<CafeEmployeeCafeDto>> GetCafes([FromQuery] string? location)
        {
            IEnumerable<CafeEmployeeCafeDto> cafes = await _sender.Send(new GetCafesByLocationQuery(location));

            return cafes;
        }

        // POST: /api/cafe
        [HttpPost]
        public async Task<IActionResult> CreateCafe([FromBody] CafeDto cafeDto)
        {
            var cafe = new Cafe()
            {
                Id = cafeDto.Id,
                Name = cafeDto.Name,
                Description = cafeDto.Description,
                Logo = cafeDto.Logo,
                Location = cafeDto.Location
            };

            cafe.Id = Guid.NewGuid();
            await _sender.Send(new CreateCafeCommand(cafe));
            return CreatedAtAction(nameof(GetCafes), new { id = cafe.Id }, cafe);
        }

        // PUT: /api/cafe
        [HttpPut]
        public async Task<IActionResult> UpdateCafe([FromBody] CafeDto cafeDto)
        {
            var cafe = new Cafe()
            {
                Id = cafeDto.Id,
                Name = cafeDto.Name,
                Description = cafeDto.Description,
                Logo = cafeDto.Logo,
                Location = cafeDto.Location
            };

            if (!await _cafeRepo.CafeExistsAsync(cafe.Id))
                throw new RepositoryException($"Cafe not found.");

            await _sender.Send(new UpdateCafeCommand(cafe));
            return NoContent();
        }

        // DELETE: /api/cafe?id=<guid>
        [HttpDelete]
        public async Task<IActionResult> DeleteCafe([FromQuery] Guid id)
        {
            var cafe = await _cafeRepo.GetByIdAsync(id);
            if (cafe == null)
                throw new RepositoryException($"Cafe not found.");

            await _sender.Send(new  DeleteCafeCommand(cafe));
            return NoContent();
        }
    }
}
