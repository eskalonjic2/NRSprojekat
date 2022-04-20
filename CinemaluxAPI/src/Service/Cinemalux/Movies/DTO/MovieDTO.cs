namespace CinemaluxAPI.src.Services.Movies.DTO
{
    public class MovieDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string OverviewLinks { get; set; }
        public string? ReleaseYear { get; set; }
        public short RunningTimeInMinutes { get; set; }
        public byte? AgeRating { get; set; }
        public string[] Genres { get; set; }
        public bool? Has3D { get; set; }
        public bool? HasLocalAudio { get; set; }
        public bool HasLocalSubtitles { get; set; }
        public byte ProfitPercentageShare { get; set; }
        public string VideoLinks { get; set; }
        public string BackdropImageURL { get; set; }
        public string ImageURL { get; set; }
    }
}
