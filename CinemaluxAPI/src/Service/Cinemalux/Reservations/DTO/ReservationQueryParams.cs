using CinemaluxAPI.Common;
using JetBrains.Annotations;

#nullable enable

namespace CinemaluxAPI.Services.Reservations.DTO
{
    public class ReservationQueryParams: GridParams
    {
        public bool? IncludeTypeCode { get; set; }
        public bool? IncludeScreening { get; set; }
        public long? ScreeningId { get; set; }
        public string? TypeCode { get; set; }
    }
}