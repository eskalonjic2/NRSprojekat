namespace CinemaluxAPI.Service.Web.DTO
{
    public class UserSuccessfulLoginDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public byte Role { get; set; }
        public string Token { get; set; }
    }
}