using DevKid.src.Domain.Entities;

namespace DevKid.src.Application.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    public class UserCreateDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    public class UserUpdateDto
    {
        public string? Name { get; set; }
    }
}
