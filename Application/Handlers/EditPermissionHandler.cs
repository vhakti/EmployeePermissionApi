using EmployeePermissionApi.Application.Commands;
using EmployeePermissionApi.Data;
using MediatR;

namespace EmployeePermissionApi.Application.Handlers
{
    public class EditPermissionHandler : IRequestHandler<EditPermissionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EditPermissionHandler> _logger;

        public EditPermissionHandler(IUnitOfWork unitOfWork, ILogger<EditPermissionHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(EditPermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var permission = await _unitOfWork.EmployeeRepository.GetPermissionsAsync(0) // Dummy employeeId for simplicity
                    .ContinueWith(t => t.Result.FirstOrDefault(p => p.Id == request.PermissionId));
                if (permission == null)
                {
                    _logger.LogError("Permission with ID {PermissionId} not found", request.PermissionId);
                    throw new Exception("Permission not found");
                }

                permission.Type = request.NewPermissionType;
                await _unitOfWork.EmployeeRepository.UpdatePermissionAsync(permission);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Permission {PermissionId} updated to {NewPermissionType}", request.PermissionId, request.NewPermissionType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing permission {PermissionId}", request.PermissionId);
                throw;
            }
        }
    }
}
