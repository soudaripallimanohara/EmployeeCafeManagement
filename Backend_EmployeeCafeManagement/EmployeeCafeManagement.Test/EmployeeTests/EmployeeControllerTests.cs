using Xunit;
using Moq;
using System.Threading.Tasks;
using EmployeeCafeManagement.Api.Controllers;
using EmployeeCafeManagement.Domain.DTO;
using EmployeeCafeManagement.Domain.Entities;
using EmployeeCafeManagement.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using EmployeeCafeManagement.Application.Commands.Employ;
using EmployeeCafeManagement.Application.Queries.Employee;
using EmployeeCafeManagement.Application.Middleware.GlobalExceptions;


namespace EmployeeCafeManagement.Test.EmployeeTests
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepoMock;
        private readonly Mock<ISender> _senderMock;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _employeeRepoMock = new Mock<IEmployeeRepository>();
            _senderMock = new Mock<ISender>();
            _controller = new EmployeeController(_employeeRepoMock.Object, _senderMock.Object);
        }

        [Fact]
        public async Task GetEmployees_ReturnsOk_WithList()
        {
            // Arrange
            var cafeName = "TestCafe";
            var expectedEmployees = new List<EmployeeCafeDto>
            {
                new EmployeeCafeDto { Id = "UI0000001", Name = "John Doe", Cafe = cafeName }
            };

            _senderMock.Setup(s => s.Send(It.IsAny<GetEmployeesByCafeQuery>(), default))
                       .ReturnsAsync(expectedEmployees);

            // Act
            var result = await _controller.GetEmployees(cafeName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEmployees = Assert.IsAssignableFrom<IEnumerable<EmployeeCafeDto>>(okResult.Value);
            Assert.Single(returnedEmployees);
        }

        [Fact]
        public async Task CreateEmployee_WhenEmployeeExists_ThrowsIdAlreadyExistException()
        {
            // Arrange
            var employeeDto = new EmployeeDto { Id = "UI0000001" };
            _employeeRepoMock.Setup(r => r.EmployeeExistsAsync(employeeDto.Id)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<IdAlreadyExistException>(() => _controller.CreateEmployee(employeeDto));
        }

        [Fact]
        public async Task CreateEmployee_Valid_ReturnsCreatedAtAction()
        {
            // Arrange
            var employeeDto = new EmployeeDto { Id = "UI0000002" };
            _employeeRepoMock.Setup(r => r.EmployeeExistsAsync(employeeDto.Id)).ReturnsAsync(false);
            _senderMock.Setup(s => s.Send(It.IsAny<AddEmployeeCommand>(), default)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateEmployee(employeeDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(EmployeeController.GetEmployees), createdResult.ActionName);
        }

        [Fact]
        public async Task UpdateEmployee_WhenNotFound_ThrowsRepositoryException()
        {
            // Arrange
            var dto = new EmployeeDto { Id = "UI1234567" };
            _employeeRepoMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync((Employee)null);

            // Act & Assert
            await Assert.ThrowsAsync<RepositoryException>(() => _controller.UpdateEmployee(dto));
        }

        [Fact]
        public async Task UpdateEmployee_Valid_NoContent()
        {
            // Arrange
            var dto = new EmployeeDto
            {
                Id = "UI0000003",
                Name = "Alice",
                EmailAddress = "alice@test.com",
                PhoneNumber = "91234567",
                Gender = "Female"
            };

            var existing = new Employee { Id = dto.Id };
            _employeeRepoMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(existing);
            _senderMock.Setup(s => s.Send(It.IsAny<UpdateEmployeeCommand>(), default)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateEmployee(dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_NotFound_ReturnsNotFound()
        {
            // Arrange
            string id = "UI9999999";
            _employeeRepoMock.Setup(r => r.GetByIdAsync(id.ToUpper())).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.DeleteEmployee(id);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Employee not found.", notFound.Value);
        }

        [Fact]
        public async Task DeleteEmployee_Valid_ReturnsNoContent()
        {
            // Arrange
            var employee = new Employee { Id = "UI0000011" };
            _employeeRepoMock.Setup(r => r.GetByIdAsync(employee.Id)).ReturnsAsync(employee);
            _senderMock.Setup(s => s.Send(It.IsAny<DeleteEmployeeCommand>(), default)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteEmployee(employee.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}