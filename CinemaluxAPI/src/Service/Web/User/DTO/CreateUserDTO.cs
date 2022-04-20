namespace CinemaluxAPI.Service.Web.DTO
{
    public class CreateUserDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; } 
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
    }
}