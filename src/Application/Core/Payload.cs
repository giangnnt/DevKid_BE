namespace DevKid.src.Application.Core
{
    public class Payload
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public Guid SessionId { get; set; }
        public Guid Jti { get; set; }
    }
}
