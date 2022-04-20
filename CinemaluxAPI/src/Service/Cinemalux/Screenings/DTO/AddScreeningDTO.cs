using System;

namespace CinemaluxAPI.Services.Screenings.DTO
{
    public class AddScreeningDTO
    {
        public short MovieId { get; set; }
        public byte HallId { get; set; }
        public string DefaultTicketTypeCode { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan ScreeningTime { get; set; }
        public bool HasLocalAudio { get; set; }
        public bool HasLocalSubtitles { get; set; }
        public bool Has3D { get; set; }
    }
}