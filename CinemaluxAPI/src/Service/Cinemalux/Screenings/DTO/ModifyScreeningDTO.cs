using System;

namespace CinemaluxAPI.Services.Screenings.DTO
{
    public class ModifyScreeningDTO
    {
        public short MovieId { get; set; }
        public byte HallId { get; set; }
        public string DefaultTicketTypeCode { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan ScreeningTime { get; set; }
    }
}