using System;

namespace CinemaluxAPI.Service.Web.MovieReviews.DTO
{
    public class GetReviewDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserUsername { get; set; }
        public string Title { get; set; }
        public string Review { get; set; }
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}