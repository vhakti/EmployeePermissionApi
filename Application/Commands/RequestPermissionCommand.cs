using EmployeePermissionApi.Models;
using MediatR;

namespace EmployeePermissionApi.Application.Commands
{
    public class RequestPermissionCommand : IRequest<int>
    {
        public int EmployeeId { get; set; }
        public PermissionType PermissionType { get; set; }
    }
}
