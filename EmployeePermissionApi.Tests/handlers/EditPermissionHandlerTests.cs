using EmployeePermissionApi.Application.Commands;
using EmployeePermissionApi.Application.Handlers;
using EmployeePermissionApi.Data;
using EmployeePermissionApi.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePermissionApi.Tests.handlers
{
    public class EditPermissionHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;        
        private readonly Mock<ILogger<EditPermissionHandler>> _loggerMock;
        private readonly EditPermissionHandler _handler;

        public EditPermissionHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();         
            _loggerMock = new Mock<ILogger<EditPermissionHandler>>();
            _handler = new EditPermissionHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_UpdatesPermissionAndIndexes()
        {
            // Arrange
            var command = new EditPermissionCommand { PermissionId = 1, NewPermissionType = PermissionType.Write };
            var permission = new Permission { Id = 1, EmployeeId = 1, Type = PermissionType.Read };

            _unitOfWorkMock.Setup(u => u.EmployeeRepository.GetPermissionsAsync(0)).ReturnsAsync(new List<Permission> { permission });
            _unitOfWorkMock.Setup(u => u.EmployeeRepository.UpdatePermissionAsync(It.IsAny<Permission>())).Verifiable();
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(PermissionType.Write, permission.Type);
            _unitOfWorkMock.Verify(u => u.EmployeeRepository.UpdatePermissionAsync(It.Is<Permission>(p => p.Id == 1 && p.Type == PermissionType.Write)), Times.Once());
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once());
            
        }

        [Fact]
        public async Task Handle_PermissionNotFound_ThrowsException()
        {
            // Arrange
            var command = new EditPermissionCommand { PermissionId = 999, NewPermissionType = PermissionType.Write };
            _unitOfWorkMock.Setup(u => u.EmployeeRepository.GetPermissionsAsync(0)).ReturnsAsync(new List<Permission>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Permission not found", exception.Message);
           
        }
    }
}
