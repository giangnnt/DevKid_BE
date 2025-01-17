namespace DevKid.src.Domain.Entities
{
    public class User
    {
        public Guid id { get; set; }
        public int RileId { get; set; }
        public Role Role { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

    }
}
