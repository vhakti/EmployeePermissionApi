using EmployeePermissionApi.Models;

namespace EmployeePermissionApi.Data.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(int id);
        Task<List<Permission>> GetPermissionsAsync(int employeeId);
        Task AddPermissionAsync(Permission permission);
        Task UpdatePermissionAsync(Permission permission);
        Task<Permission> GetPermissionByIdAsync(int id);
    }
}
