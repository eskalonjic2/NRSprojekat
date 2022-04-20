using System;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

namespace CinemaluxAPI.Services.Screenings.DTO
{
    public class MovieScreeningsTimelineDTO
    {
        public int HallId { get; set; }
        public string HallName { get; set; }
        public ScreeningTimelineDTO[] Screenings { get; set; }

        public class ScreeningTimelineDTO
        {
            public long Id { get; set; }
            public string MovieTitle { get; set; }
            public DateTime Date { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
        }
    }
}