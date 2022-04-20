using System;
using CinemaluxAPI.Common.Enumerations;

namespace CinemaluxAPI.Services
{
    public class AddEmployeeDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public Int32 BornAt { get; set; } 
        public string Password { get; set; }
        public Role Role { get; set; }
        public string Address { get; set; }
        public string Key { get; set; }
        public float? Salary { get; set; }
        public int? ManagerId { get; set; }
    }
}
