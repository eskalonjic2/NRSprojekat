using System;

namespace CinemaluxAPI.Services.Screenings.DTO
{
    public class ScreeningDTO
    {
        public long Id { get; set; }
        public short MovieId { get; set; }
        public string MovieTitle { get; set; }
        public byte HallId { get; set; }
        public string DefaultTicketTypeCode { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan ScreeningTime { get; set; }
        public bool HasLocalSubtitles { get; set; }
        public bool Is3D { get; set; }
        public bool HasLocalAudio { get; set; }
    }
}