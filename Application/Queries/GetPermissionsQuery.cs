using EmployeePermissionApi.Models;
using MediatR;

namespace EmployeePermissionApi.Application.Queries
{
    public class GetPermissionsQuery : IRequest<List<Permission>>
    {
        public int EmployeeId { get; set; }
    }
}
