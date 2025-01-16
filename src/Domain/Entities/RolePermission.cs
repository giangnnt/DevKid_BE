namespace DevKid.src.Domain.Entities
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public string PermissionSlug { get; set; } = null!;
        public Permission Permission { get; set; } = null!;
    }
}
