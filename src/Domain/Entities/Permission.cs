using System.ComponentModel.DataAnnotations;

namespace DevKid.src.Domain.Entities
{
    public class Permission
    {
        [Key]
        public string Slug { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<Role> Roles { get; set; } = new();
    }
}
