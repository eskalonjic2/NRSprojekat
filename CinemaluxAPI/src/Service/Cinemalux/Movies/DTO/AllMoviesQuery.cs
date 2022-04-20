namespace CinemaluxAPI.src.Services.Movies.DTO
{
    public class AllMoviesQuery
    {
        public string Title { get; set; }
        public int SortId { get; set; }
        public bool IsAscending { get; set; }
    }
}