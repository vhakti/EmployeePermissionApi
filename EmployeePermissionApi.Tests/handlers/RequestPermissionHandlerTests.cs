using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using EmployeePermissionApi.Models;
using EmployeePermissionApi.Application.Handlers;
using EmployeePermissionApi.Data;
using Microsoft.Extensions.Logging;
using EmployeePermissionApi.Application.Commands;
namespace EmployeePermissionApi.Tests.handlers
{
    public class RequestPermissionHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;       
        private readonly Mock<ILogger<RequestPermissionHandler>> _loggerMock;
        private readonly RequestPermissionHandler _handler;

        public RequestPermissionHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();           
            _loggerMock = new Mock<ILogger<RequestPermissionHandler>>();
            _handler = new RequestPermissionHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_AddsPermissionAndIndexes_ReturnsId()
        {
            // Arrange
            var command = new RequestPermissionCommand { EmployeeId = 1, PermissionType = PermissionType.Read };
            var employee = new Employee { Id = 1, Name = "Test Employee", Permissions = new List<Permission>() };
            var permission = new Permission { Id = 1, EmployeeId = 1, Type = PermissionType.Read };

            _unitOfWorkMock.Setup(u => u.EmployeeRepository.GetByIdAsync(1)).ReturnsAsync(employee);
            _unitOfWorkMock.Setup(u => u.EmployeeRepository.AddPermissionAsync(It.IsAny<Permission>())).Callback<Permission>(p => p.Id = 1);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
          
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(1, result);
            _unitOfWorkMock.Verify(u => u.EmployeeRepository.AddPermissionAsync(It.Is<Permission>(p => p.EmployeeId == 1 && p.Type == PermissionType.Read)), Times.Once());
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once());
       
            
        }

        [Fact]
        public async Task Handle_EmployeeNotFound_ThrowsException()
        {
            // Arrange
            var command = new RequestPermissionCommand { EmployeeId = 999, PermissionType = PermissionType.Read };
            _unitOfWorkMock.Setup(u => u.EmployeeRepository.GetByIdAsync(999)).ReturnsAsync((Employee)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Employee not found", exception.Message);
            
        }
    }
}
