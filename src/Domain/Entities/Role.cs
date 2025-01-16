namespace DevKid.src.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<RolePermission> rolePermissions { get; set; } = new();
    }
}
