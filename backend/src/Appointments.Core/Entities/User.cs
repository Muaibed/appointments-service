namespace Appointments.Core.Entities
{
    public enum Role
    {
        Admin,
        User,
    }
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!; 
        public Role Role { get; set; }
    }
}
