using EmployeeCafeManagement.Domain.Entities;
using EmployeeCafeManagement.Infrastructure.Data;
using EmployeeCafeManagement.Infrastructure.Repositorys;
using EmployeeCafeManagement.Application.Middleware.GlobalExceptions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmployeeCafeManagement.Test.CafeTests
{
    public class CafeRepositoryTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // isolate each test
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetCafesAsync_ReturnsCafes_WhenLocationIsNull()
        {
            using var context = GetInMemoryDbContext();
            var cafe = new Cafe
            {
                Id = Guid.NewGuid(),
                Name = "Test Cafe",
                Location = "NY",
                Description = "Cozy",
                Logo = "logo.png",
                EmployeeCafes = new List<EmployeeCafe>()
            };
            context.Cafes.Add(cafe);
            await context.SaveChangesAsync();

            var repo = new CafeRepository(context);
            var result = await repo.GetCafesAsync(null);

            Assert.Single(result);
            Assert.Equal("Test Cafe", result.First().Name);
        }

        [Fact]
        public async Task GetCafesAsync_ThrowsRepositoryException_OnError()
        {
            var repo = new CafeRepository(null!);
            await Assert.ThrowsAsync<RepositoryException>(() => repo.GetCafesAsync(null));
        }
        [Fact]
        public async Task GetByIdAsync_ReturnsCafe_WhenExists()
        {
            using var context = GetInMemoryDbContext();
            var id = new Guid("4AB1234F-9D86-401F-A21B-16421FFFC058");

            var cafe = new Cafe()
            {
                Id = id,
                Name = "Test",
                Description = "A Lush Creamy Lussy drink with Chilled Flavour.",
                Logo = "https://example.com/logos/sunset-brew.png",
                Location = "Chennai",
                EmployeeCafes = null
            };
            context.Cafes.Add(cafe);
            await context.SaveChangesAsync();

            var repo = new CafeRepository(context);
            var result = await repo.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result!.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsRepositoryException_OnError()
        {
            var repo = new CafeRepository(null!);
            await Assert.ThrowsAsync<RepositoryException>(() => repo.GetByIdAsync(Guid.NewGuid()));
        }
        [Fact]
        public async Task AddCafeAsync_AddsCafe()
        {
            using var context = GetInMemoryDbContext();
            var repo = new CafeRepository(context);

            var cafe = new Cafe()
            {
                Id = Guid.NewGuid(),
                Name = "CafeX",
                Description = "A Lush Creamy Lussy drink with Chilled Flavour.",
                Logo = "https://example.com/logos/sunset-brew.png",
                Location = "Chennai"
            };

            await repo.AddCafeAsync(cafe);

            Assert.Single(context.Cafes);
        }

        [Fact]
        public async Task AddCafeAsync_ThrowsRepositoryException_OnError()
        {
            var repo = new CafeRepository(null!);
            await Assert.ThrowsAsync<RepositoryException>(() => repo.AddCafeAsync(new Cafe()));
        }
        [Fact]
        public async Task UpdateCafeAsync_UpdatesCafe()
        {
            using var context = GetInMemoryDbContext();

            var cafe = new Cafe()
            {
                Id = Guid.NewGuid(),
                Name = "Original",
                Description = "A Lush Creamy Lussy drink with Chilled Flavour.",
                Logo = "https://example.com/logos/sunset-brew.png",
                Location = "Chennai"
            };


            context.Cafes.Add(cafe);
            await context.SaveChangesAsync();

            cafe.Name = "Updated";
            var repo = new CafeRepository(context);
            await repo.UpdateCafeAsync(cafe);

            Assert.Equal("Updated", context.Cafes.First().Name);
        }

        [Fact]
        public async Task UpdateCafeAsync_ThrowsRepositoryException_OnError()
        {
            var repo = new CafeRepository(null!);
            await Assert.ThrowsAsync<RepositoryException>(() => repo.UpdateCafeAsync(new Cafe()));
        }
        [Fact]
        public async Task DeleteCafeAsync_DeletesCafeAndEmployees()
        {
            using var context = GetInMemoryDbContext();
            var cafeId = Guid.NewGuid();
            string empId = "UIABC2222";

            var cafe = new Cafe()
            {
                Id = cafeId,
                Name = "Cafe",
                Description = "A Lush Creamy Lussy drink with Chilled Flavour.",
                Logo = "https://example.com/logos/sunset-brew.png",
                Location = "Chennai"
            };
            context.Cafes.Add(cafe);
            context.EmployeesCafe.Add(new EmployeeCafe { CafeId = cafeId, EmployeeId = empId });

            await context.SaveChangesAsync();

            var repo = new CafeRepository(context);

            var exists = await repo.CafeExistsAsync(cafeId);
            if (exists)
            {
                await repo.DeleteCafeAsync(cafe);
            }
            
            Assert.Empty(context.Cafes);
            Assert.Empty(context.Employees);
            Assert.Empty(context.EmployeesCafe);
        }

        [Fact]
        public async Task DeleteCafeAsync_ThrowsRepositoryException_OnError()
        {
            var repo = new CafeRepository(null!);
            await Assert.ThrowsAsync<RepositoryException>(() => repo.DeleteCafeAsync(new Cafe { Id = Guid.NewGuid() }));
        }
        [Fact]
        public async Task CafeExistsAsync_ReturnsTrue_WhenExists()
        {
            using var context = GetInMemoryDbContext();
            var id = Guid.NewGuid();

            var cafe = new Cafe()
            {
                Id = id,
                Name = "Cafe",
                Description = "A Lush Creamy Lussy drink with Chilled Flavour.",
                Logo = "https://example.com/logos/sunset-brew.png",
                Location = "Chennai"
            };
            context.Cafes.Add(cafe);

            await context.SaveChangesAsync();

            var repo = new CafeRepository(context);
            var exists = await repo.CafeExistsAsync(id);

            Assert.True(exists);
        }

        [Fact]
        public async Task CafeExistsAsync_ReturnsFalse_WhenNotExists()
        {
            using var context = GetInMemoryDbContext();
            var repo = new CafeRepository(context);
            var exists = await repo.CafeExistsAsync(Guid.NewGuid());

            Assert.False(exists);
        }

        [Fact]
        public async Task CafeExistsAsync_ThrowsRepositoryException_OnError()
        {
            var repo = new CafeRepository(null!);
            await Assert.ThrowsAsync<RepositoryException>(() => repo.CafeExistsAsync(Guid.NewGuid()));
        }
    }
}