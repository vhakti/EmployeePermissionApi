using EmployeePermissionApi.Application.Queries;
using EmployeePermissionApi.Data;
using EmployeePermissionApi.Models;
using MediatR;

namespace EmployeePermissionApi.Application.Handlers
{
    public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, List<Permission>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetPermissionsHandler> _logger;

        public GetPermissionsHandler(IUnitOfWork unitOfWork, ILogger<GetPermissionsHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<Permission>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var permissions = await _unitOfWork.EmployeeRepository.GetPermissionsAsync(request.EmployeeId);
                _logger.LogInformation("Retrieved {Count} permissions for Employee {EmployeeId}", permissions.Count, request.EmployeeId);
                return permissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving permissions for Employee {EmployeeId}", request.EmployeeId);
                throw;
            }
        }
    }
}
