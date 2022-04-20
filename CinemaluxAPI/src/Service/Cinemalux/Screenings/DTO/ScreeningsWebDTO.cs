namespace CinemaluxAPI.Services.Screenings.DTO
{
    public class ScreeningsWebDTO
    {
        public long Id { get; set; }
        public string MovieTitle { get; set; }
        public string ImageURL { get; set; }
        public string BackdropImageURL { get; set; }
        public string[] Genres { get; set; }
        public short RunningTimeInMinutes { get; set; }
        public bool Has3D { get; set; }
        public bool HasLocalAudio { get; set; }
        public bool HasSubtitles { get; set; }
    }
}