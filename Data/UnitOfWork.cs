using EmployeePermissionApi.Data.Repositories;

namespace EmployeePermissionApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmployeeDbContext _context;
        public IEmployeeRepository EmployeeRepository { get; }

        public UnitOfWork(EmployeeDbContext context)
        {
            _context = context;
            EmployeeRepository = new EmployeeRepository(context);
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
