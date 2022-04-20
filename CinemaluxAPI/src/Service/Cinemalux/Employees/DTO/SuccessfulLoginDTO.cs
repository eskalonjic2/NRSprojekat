using CinemaluxAPI.Common.Enumerations;

namespace CinemaluxAPI.Services
{
    public class SuccessfulLoginDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public byte Role { get; set; }
        public string Token { get; set; }
        
    }
}