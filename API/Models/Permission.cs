using System.Text.Json.Serialization;

namespace EmployeePermissionApi.Models
{
    public enum PermissionType
    {
        Read,
        Write,
        Delete,
        Edit
    }

    public class Permission
    {
        public int Id { get; set; }
        public PermissionType Type { get; set; }
        public int EmployeeId { get; set; }
        [JsonIgnore]
        public Employee Employee { get; set; }
    }
}
