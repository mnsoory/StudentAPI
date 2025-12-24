

namespace StudentAPI.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public virtual ICollection<UserPermission> Permissions { get; set; } = new List<UserPermission>();
    }
}
