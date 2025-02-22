using EmployeePermissionApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeePermissionApi.Data
{
    public class EmployeeDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Permissions)
                .WithOne(p => p.Employee)
                .HasForeignKey(p => p.EmployeeId);
        }
    }
}
