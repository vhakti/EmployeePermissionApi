namespace EmployeePermissionApi.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Permission> Permissions { get; set; } = new();
    }
}
