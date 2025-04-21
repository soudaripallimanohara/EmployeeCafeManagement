using EmployeeCafeManagement.Api.Controllers;
using EmployeeCafeManagement.Application.Commands.Caffee;
using EmployeeCafeManagement.Application.Queries.Cafe;
using EmployeeCafeManagement.Domain.DTO;
using EmployeeCafeManagement.Domain.Entities;
using EmployeeCafeManagement.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using EmployeeCafeManagement.Application.Middleware.GlobalExceptions;

namespace EmployeeCafeManagement.Test.CafeTests
{
    public class CafeControllerTests
    {
        private readonly Mock<ICafeRepository> _mockCafeRepo = new();
        private readonly Mock<ISender> _mockSender = new();
        private readonly CafeController _controller;

        public CafeControllerTests()
        {
            _controller = new CafeController(_mockCafeRepo.Object, _mockSender.Object);
        }

        [Fact]
        public async Task GetCafes_ReturnsCafes_WhenLocationIsProvided()
        {
            // Arrange
            var location = "Downtown";
            var expected = new List<CafeEmployeeCafeDto>
        {
            new() { Id = Guid.NewGuid(), Name = "Cafe A", Location = "Downtown" },
            new() { Id = Guid.NewGuid(), Name = "Cafe B", Location = "Downtown" }
        };

            _mockSender
                .Setup(s => s.Send(It.IsAny<GetCafesByLocationQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _controller.GetCafes(location);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateCafe_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var cafeDto = new CafeDto
            {
                Name = "Test Cafe",
                Description = "Nice place",
                Logo = "logo.png",
                Location = "Uptown"
            };

            _mockSender
                .Setup(s => s.Send(It.IsAny<CreateCafeCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateCafe(cafeDto) as CreatedAtActionResult;

            // Assert
            result.Should().NotBeNull();
            result!.ActionName.Should().Be(nameof(_controller.GetCafes));
            result.Value.Should().BeOfType<Cafe>();
        }

        [Fact]
        public async Task UpdateCafe_ReturnsNoContent_WhenCafeExists()
        {
            // Arrange
            var cafeDto = new CafeDto
            {
                Id = Guid.NewGuid(),
                Name = "Updated Cafe",
                Description = "Updated Desc",
                Logo = "newlogo.png",
                Location = "Midtown"
            };

            _mockCafeRepo.Setup(r => r.CafeExistsAsync(cafeDto.Id)).ReturnsAsync(true);
            _mockSender.Setup(s => s.Send(It.IsAny<UpdateCafeCommand>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateCafe(cafeDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateCafe_ThrowsRepositoryException_WhenCafeDoesNotExist()
        {
            // Arrange
            var cafeDto = new CafeDto { Id = Guid.NewGuid() };
            _mockCafeRepo.Setup(r => r.CafeExistsAsync(cafeDto.Id)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<RepositoryException>(() => _controller.UpdateCafe(cafeDto));
        }

        [Fact]
        public async Task DeleteCafe_ReturnsNoContent_WhenCafeExists()
        {
            // Arrange
            var cafeId = Guid.NewGuid();
            var cafe = new Cafe { Id = cafeId, Name = "Cafe X" };

            _mockCafeRepo.Setup(r => r.GetByIdAsync(cafeId)).ReturnsAsync(cafe);
            _mockSender.Setup(s => s.Send(It.IsAny<DeleteCafeCommand>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCafe(cafeId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteCafe_ThrowsRepositoryException_WhenCafeNotFound()
        {
            // Arrange
            var cafeId = Guid.NewGuid();
            _mockCafeRepo.Setup(r => r.GetByIdAsync(cafeId)).ReturnsAsync((Cafe?)null);

            // Act & Assert
            await Assert.ThrowsAsync<RepositoryException>(() => _controller.DeleteCafe(cafeId));
        }
    }
}
