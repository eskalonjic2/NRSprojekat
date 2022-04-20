namespace CinemaluxAPI.Services
{
    public class EditEmployeeDTO
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? ManagerId { get; set; }
        public string ContactPhone { get; set; }
        public string Password { get; set; }
        public float? Salary { get; set; }
    }
}