namespace CinemaluxAPI.Auth
{
    public class Identity
    {
        public int Id { get; set; }   
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public byte Role { get; set; }
    }
}