using EmployeeCafeManagement.Application.Commands;
using EmployeeCafeManagement.Application.Commands.Employ;
using EmployeeCafeManagement.Application.Middleware;
using EmployeeCafeManagement.Application.Middleware.GlobalExceptions;
using EmployeeCafeManagement.Application.Queries;
using EmployeeCafeManagement.Application.Queries.Employee;
using EmployeeCafeManagement.Domain.DTO;
using EmployeeCafeManagement.Domain.Entities;
using EmployeeCafeManagement.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Text.RegularExpressions;

namespace EmployeeCafeManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly ISender _sender;

        public EmployeeController(IEmployeeRepository employeeRepo, ISender sender)
        {
            _employeeRepo = employeeRepo;
            _sender = sender;
        }

        // GET: /api/employee?cafe=<cafe>
        [HttpGet]
        public async Task<IActionResult> GetEmployees([FromQuery] string? cafe)
        {
            var employees = await _sender.Send(new GetEmployeesByCafeQuery(cafe));
            return Ok(employees);
        }

        // POST: /api/employee
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (await _employeeRepo.EmployeeExistsAsync(employeeDto.Id))
                throw new IdAlreadyExistException($"Employee ID already exists.");

            await _sender.Send(new AddEmployeeCommand(employeeDto));

            return CreatedAtAction(nameof(GetEmployees), new { id = employeeDto.Id }, employeeDto);


        }

        // PUT: /api/employee
        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employeeDto)
        {
            var employee = await _employeeRepo.GetByIdAsync(employeeDto.Id);
            if (employee == null)
                throw new RepositoryException($"Employee not found");

            employee.Name = employeeDto.Name;
            employee.EmailAddress = employeeDto.EmailAddress;
            employee.PhoneNumber = employeeDto.PhoneNumber;
            employee.Gender = employeeDto.Gender;

            await _sender.Send(new UpdateEmployeeCommand(employee, employeeDto.CafeId, employeeDto.StartDate));

            return NoContent();
        }

        // DELETE: /api/employee?id=<id>
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee([FromQuery] string id)
        {
            var employee = await _employeeRepo.GetByIdAsync(id.ToUpper());
            if (employee == null)
                return NotFound("Employee not found.");

            await _sender.Send(new DeleteEmployeeCommand(employee));
            return NoContent();
        }
    }
}
