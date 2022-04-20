using System;
using JetBrains.Annotations;

namespace CinemaluxAPI.Services.Reservations.DTO
{
    public class ReservationGridDTO
    {
        public int Id { get; set; }
        [CanBeNull] public string ReservationTypeCode { get; set; }
        public string FullName { get; set; }
        public string ContactPhone { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        [CanBeNull] public ReservationGridScreeningDTO Screening { get; set; }

        public class ReservationGridScreeningDTO
        {
            public long Id { get; set; }
            public string MovieTitle { get; set; }
            public string HallName { get; set; }
            public TimeSpan Time { get; set; }
            public DateTime Date { get; set; }
        }
    }
}