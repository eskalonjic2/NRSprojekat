using System;

namespace CinemaluxAPI.Service.Web.DTO
{
    public class UserMovieReviewsDTO
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public string ImageURL { get; set; }
        public string Title { get; set; }
        public string Review { get; set; }
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}