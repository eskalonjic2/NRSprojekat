using CinemaluxAPI.Common.Enumerations;

namespace CinemaluxAPI.Auth.DTO
{
    public class ValidUserResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
    }
}