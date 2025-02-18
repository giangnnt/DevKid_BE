namespace DevKid.src.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsActive { get; set; }
        public string Phone { get; set; } = null!;

    }
}
