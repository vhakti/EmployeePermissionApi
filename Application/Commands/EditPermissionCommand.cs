using EmployeePermissionApi.Models;
using MediatR;

namespace EmployeePermissionApi.Application.Commands
{
    public class EditPermissionCommand : IRequest
    {
        public int PermissionId { get; set; }
        public PermissionType NewPermissionType { get; set; }
    }
}
