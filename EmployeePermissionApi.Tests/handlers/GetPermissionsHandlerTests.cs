using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using EmployeePermissionApi.Data;
using EmployeePermissionApi.Models;
using EmployeePermissionApi.Application.Queries;
using EmployeePermissionApi.Application.Handlers;

namespace EmployeePermissionApi.Tests.handlers
{
    public class GetPermissionsHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetPermissionsHandler>> _loggerMock;
        private readonly GetPermissionsHandler _handler;

        public GetPermissionsHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetPermissionsHandler>>();
            _handler = new GetPermissionsHandler(_unitOfWorkMock.Object,  _loggerMock.Object); 
        }

        [Fact]
        public async Task Handle_ValidEmployeeId_ReturnsPermissions()
        {
            // Arrange
            var query = new GetPermissionsQuery { EmployeeId = 1 };
            var permissions = new List<Permission>
            {
                new Permission { Id = 1, EmployeeId = 1, Type = PermissionType.Read },
                new Permission { Id = 2, EmployeeId = 1, Type = PermissionType.Write }
            };

            _unitOfWorkMock.Setup(u => u.EmployeeRepository.GetPermissionsAsync(1)).ReturnsAsync(permissions);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Type == PermissionType.Read);
            Assert.Contains(result, p => p.Type == PermissionType.Write);
            _unitOfWorkMock.Verify(u => u.EmployeeRepository.GetPermissionsAsync(1), Times.Once());
            
        }

        [Fact]
        public async Task Handle_NoPermissions_ReturnsEmptyList()
        {
            // Arrange
            var query = new GetPermissionsQuery { EmployeeId = 111 };
            _unitOfWorkMock.Setup(u => u.EmployeeRepository.GetPermissionsAsync(111)).ReturnsAsync(new List<Permission>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
          
        }
    }
}
