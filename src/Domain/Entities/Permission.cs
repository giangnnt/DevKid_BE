namespace DevKid.src.Domain.Entities
{
    public class Permission
    {
        public string Slug { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<RolePermission> rolePermissions { get; set; } = new();
    }
}
