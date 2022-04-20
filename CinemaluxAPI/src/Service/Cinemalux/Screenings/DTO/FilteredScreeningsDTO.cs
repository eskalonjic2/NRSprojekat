using System;

namespace CinemaluxAPI.Services.Screenings.DTO
{
    public class FilteredScreeningsDTO
    {
        public long Id { get; set; }
        public string MovieTitle { get; set; }
        public string HallName { get; set; }
        public string DefaultTicketTypeCode { get; set; }
        public int Capacity { get; set; }
        public string[] BookedSeats { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public bool Is3D { get; set; }
        public bool HasLocalSubtitles { get; set; }
        public bool HasLocalAudio { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}