using System;

namespace CinemaluxAPI.Service.Web.DTO
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsLocked { get; set; }
        public int TotalReservations { get; set; }
        public int TotalReviews { get; set; }
        public bool IsArchived { get; set; }
        public bool IsBanned { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}