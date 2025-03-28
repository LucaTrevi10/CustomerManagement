namespace CustomerUI.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsLogged { get; set; }
    }
}
