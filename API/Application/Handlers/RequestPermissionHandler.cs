using EmployeePermissionApi.Application.Commands;
using EmployeePermissionApi.Data;
using EmployeePermissionApi.Models;
using MediatR;

namespace EmployeePermissionApi.Application.Handlers
{
    public class RequestPermissionHandler : IRequestHandler<RequestPermissionCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RequestPermissionHandler> _logger;
        private readonly IElasticSearchClient _elasticClient;

        public RequestPermissionHandler(IUnitOfWork unitOfWork, ILogger<RequestPermissionHandler> logger, IElasticSearchClient elasticClient)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _elasticClient = elasticClient;
        }

        public async Task<int> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                {
                    _logger.LogError("Employee with ID {EmployeeId} not found", request.EmployeeId);
                    throw new Exception("Employee not found");
                }

                var permission = new Permission { EmployeeId = request.EmployeeId, Type = request.PermissionType };
                await _unitOfWork.EmployeeRepository.AddPermissionAsync(permission);
                await _unitOfWork.SaveChangesAsync();

                // Indaxar en Elasticsearch..
                await _elasticClient.IndexPermissionAsync(permission);


                _logger.LogInformation("Permission {PermissionType} added to Employee {EmployeeId}", request.PermissionType, request.EmployeeId);
                return permission.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting permission for Employee {EmployeeId}", request.EmployeeId);
                throw;
            }
        }
    }
}
