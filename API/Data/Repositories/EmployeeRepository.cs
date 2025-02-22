using EmployeePermissionApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeePermissionApi.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;

        public EmployeeRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetByIdAsync(int id) => await _context.Employees.Include(e => e.Permissions).FirstOrDefaultAsync(e => e.Id == id);
        public async Task<List<Permission>> GetPermissionsAsync(int employeeId) => await _context.Permissions.Where(p => p.EmployeeId == employeeId).ToListAsync();
        public async Task AddPermissionAsync(Permission permission) => await _context.Permissions.AddAsync(permission);
        public async Task UpdatePermissionAsync(Permission permission) => _context.Permissions.Update(permission);
    }
}
