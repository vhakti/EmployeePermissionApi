using EmployeePermissionApi.Data.Repositories;

namespace EmployeePermissionApi.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository EmployeeRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
