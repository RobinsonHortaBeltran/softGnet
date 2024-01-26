namespace apiNew.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }

        public string? Salt { get; set; }
    }
}