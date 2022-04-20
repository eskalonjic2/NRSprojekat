namespace CinemaluxAPI.Service.Web.MovieReviews.DTO
{
    public class PostReviewDTO
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Review { get; set; }
        public double Rating { get; set; }
    }
}