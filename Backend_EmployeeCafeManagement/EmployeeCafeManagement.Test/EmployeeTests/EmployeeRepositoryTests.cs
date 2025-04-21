using EmployeeCafeManagement.Application.Middleware.GlobalExceptions;
using EmployeeCafeManagement.Domain.DTO;
using EmployeeCafeManagement.Domain.Entities;
using EmployeeCafeManagement.Infrastructure.Data;
using EmployeeCafeManagement.Infrastructure.Repositorys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeCafeManagement.Test.EmployeeTests
{
    public class EmployeeRepositoryTests
    {
        private async Task<AppDbContext> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task GetEmployeesAsync_ReturnsAllEmployees_WhenCafeIsNull()
        {
            // Arrange
            Guid id1 = new Guid();
            Guid id2 = new Guid();
            var context = await GetDbContextAsync();

            var cafe1 = new Cafe()
            {
                Id = id1,
                Name = "Cafe A",
                Description = "A Lush Creamy Lussy drink with Chilled Flavour.",
                Logo = "https://example.com/logos/sunset-brew.png",
                Location = "Location A"
            };

            var cafe2 = new Cafe()
            {
                Id = id2,
                Name = "Cafe B",
                Description = "A Lush Creamy Lussy drink with Chilled Flavour.",
                Logo = "https://example.com/logos/sunset-brew.png",
                Location = "Location B",
                EmployeeCafes = new List<EmployeeCafe>()
            };

            var emp1 = new EmployeeDto
            {
                Id = "UIABC1213",
                Name = "Alice",
                EmailAddress = "alice@example.com",
                PhoneNumber = "87442353",
                Gender = "Female",
                CafeId = id1,
                StartDate = DateTime.Today.AddDays(-10)
            };

            var emp2 = new EmployeeDto
            {
                Id = "UIABC7678",
                Name = "Jhon",
                EmailAddress = "jhon@example.com",
                PhoneNumber = "91234567",
                Gender = "Male",
                CafeId = id2,
                StartDate = DateTime.Today.AddDays(-20)
            };

            var repo = new EmployeeRepository(context);
            var cafeRepo = new CafeRepository(context);


            await cafeRepo.AddCafeAsync(cafe1);
            await cafeRepo.AddCafeAsync(cafe2);

            await repo.AddEmployeeAsync(emp1);
            await repo.AddEmployeeAsync(emp2);
            await context.SaveChangesAsync();

            // Act
            var result = (await repo.GetEmployeesAsync(null)).ToList();

            // Assert
            Assert.Equal(5, result.Count);  // Total count=5, because 3 records are coming from Seeding
            Assert.Contains(result, r => r.Name == "Alice" && r.DaysWorked >= 10);
            Assert.Contains(result, r => r.Name == "Jhon" && r.DaysWorked >= 20);
        }

        [Fact]
        public async Task AddEmployeeAsync_ValidEmployee_AddsSuccessfully()
        {
            var context = await GetDbContextAsync();
            var repo = new EmployeeRepository(context);

            var employeeDto = new EmployeeDto
            {
                Id = "UIABC1234",
                Name = "Alice",
                EmailAddress = "alice@example.com",
                PhoneNumber = "91234567",
                Gender = "Female",
                CafeId = Guid.NewGuid(),
                StartDate = DateTime.Today
            };

            await repo.AddEmployeeAsync(employeeDto);

            var added = await context.Employees.FirstOrDefaultAsync(e => e.Id == "UIABC1234");
            Assert.NotNull(added);
            Assert.Equal("Alice", added.Name);
        }

        [Fact]
        public async Task AddEmployeeAsync_InvalidPhone_ThrowsException()
        {
            var context = await GetDbContextAsync();
            var repo = new EmployeeRepository(context);

            var dto = new EmployeeDto
            {
                Id = "UIXYZ1234",
                Name = "Bob",
                EmailAddress = "bob@example.com",
                PhoneNumber = "71234567", // Invalid
                Gender = "Male",
                CafeId = Guid.NewGuid(),
                StartDate = DateTime.Today
            };

            await Assert.ThrowsAsync<RepositoryException>(() => repo.AddEmployeeAsync(dto));
        }

        [Fact]
        public async Task AddEmployeeAsync_InvalidEmailAddress_ThrowsException()
        {
            var context = await GetDbContextAsync();
            var repo = new EmployeeRepository(context);

            var dto = new EmployeeDto
            {
                Id = "UIXYZ1234",
                Name = "Bob",
                EmailAddress = "bob#example.com",
                PhoneNumber = "71234567", // Invalid
                Gender = "Male",
                CafeId = Guid.NewGuid(),
                StartDate = DateTime.Today
            };

            await Assert.ThrowsAsync<RepositoryException>(() => repo.AddEmployeeAsync(dto));
        }

        [Fact]
        public async Task AddEmployeeAsync_InvalidGender_ThrowsException()
        {
            var context = await GetDbContextAsync();
            var repo = new EmployeeRepository(context);

            var dto = new EmployeeDto
            {
                Id = "UIXYZ1234",
                Name = "Bob",
                EmailAddress = "bob@example.com",
                PhoneNumber = "71234567", // Invalid
                Gender = "UnKnown",
                CafeId = Guid.NewGuid(),
                StartDate = DateTime.Today
            };

            await Assert.ThrowsAsync<RepositoryException>(() => repo.AddEmployeeAsync(dto));
        }

        [Fact]
        public async Task AddEmployeeAsync_InvalidStartDate_ThrowsException()
        {
            var context = await GetDbContextAsync();
            var repo = new EmployeeRepository(context);

            var dto = new EmployeeDto
            {
                Id = "UIXYZ1234",
                Name = "Bob",
                EmailAddress = "bob@example.com",
                PhoneNumber = "71234567", // Invalid
                Gender = "Male",
                CafeId = Guid.NewGuid(),
                StartDate = DateTime.Today.AddDays(+10)
            };

            await Assert.ThrowsAsync<RepositoryException>(() => repo.AddEmployeeAsync(dto));
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ValidData_UpdatesEmployee()
        {
            var context = await GetDbContextAsync();

            var cafe1 = new Cafe()
            {
                Id = Guid.NewGuid(),
                Name = "Cafe1",
                Description = "A Lush Creamy Lussy drink with Chilled Flavour.",
                Logo = "https://example.com/logos/sunset-brew.png",
                Location = "Chennai"
            };

            var cafe2 = new Cafe()
            {
                Id = Guid.NewGuid(),
                Name = "Cafe2",
                Description = "A Lush Creamy Lussy drink with Chilled Flavour.",
                Logo = "https://example.com/logos/sunset-brew.png",
                Location = "Chennai"
            };


            var employee = new Employee
            {
                Id = "UIUPDATE1",
                Name = "OldName",
                EmailAddress = "old@example.com",
                PhoneNumber = "91234567",
                Gender = "Male",
                EmployeeCafe = new EmployeeCafe
                {
                    CafeId = cafe1.Id,
                    StartDate = DateTime.Today.AddDays(-5)
                }
            };

            context.Cafes.AddRange(cafe1, cafe2);
            context.Employees.Add(employee);
            context.EmployeesCafe.Add(employee.EmployeeCafe);
            await context.SaveChangesAsync();

            var repo = new EmployeeRepository(context);

            employee.Name = "NewName";
            employee.EmailAddress = "new@example.com";
            await repo.UpdateEmployeeAsync(employee, cafe2.Id, DateTime.Today.AddDays(-1));

            var updated = await context.Employees.FirstOrDefaultAsync(e => e.Id == "UIUPDATE1");
            Assert.Equal("NewName", updated.Name);
            Assert.Equal("new@example.com", updated.EmailAddress);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_RemovesEmployee()
        {
            var context = await GetDbContextAsync();

            var employee = new Employee
            {
                Id = "UIDELETE1",
                Name = "DeleteMe",
                EmailAddress = "delete@example.com",
                PhoneNumber = "91234567",
                Gender = "Female",
                EmployeeCafe = new EmployeeCafe
                {
                    CafeId = Guid.NewGuid(),
                    StartDate = DateTime.Today.AddDays(-10)
                }
            };

            context.Employees.Add(employee);
            context.EmployeesCafe.Add(employee.EmployeeCafe);
            await context.SaveChangesAsync();

            var repo = new EmployeeRepository(context);
            await repo.DeleteEmployeeAsync(employee);

            var deleted = await context.Employees.FirstOrDefaultAsync(e => e.Id == "UIDELETE1");
            Assert.Null(deleted);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectEmployee()
        {
            var context = await GetDbContextAsync();
            var employee = new Employee
            {
                Id = "UIGET001",
                Name = "Target",
                EmailAddress = "target@example.com",
                PhoneNumber = "91234567",
                Gender = "Male",
                EmployeeCafe = new EmployeeCafe()
            };
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            var repo = new EmployeeRepository(context);
            var result = await repo.GetByIdAsync("UIGET001");

            Assert.NotNull(result);
            Assert.Equal("Target", result?.Name);
        }

        [Fact]
        public async Task EmployeeExistsAsync_ReturnsTrueIfExists()
        {
            var context = await GetDbContextAsync();
            context.Employees.Add(new Employee
            {
                Id = "UICHECK1",
                Name = "CheckMe",
                EmailAddress = "check@example.com",
                PhoneNumber = "91234567",
                Gender = "Female",
                EmployeeCafe = new EmployeeCafe()
            });
            await context.SaveChangesAsync();

            var repo = new EmployeeRepository(context);
            var exists = await repo.EmployeeExistsAsync("UICHECK1");

            Assert.True(exists);
        }
    }
}
