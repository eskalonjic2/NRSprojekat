using System;

namespace CinemaluxAPI.Service.Web.Movies.DTO
{
    public class OrganizationMovieOverviewDTO
    {
        public short Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Genres { get; set; }
        public string ImageURL { get; set; }
        public string BackdropImageURL { get; set; }
        public double AverageRating { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}