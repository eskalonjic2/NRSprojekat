namespace CinemaluxAPI.Services.Reservations.DTO
{
    public class TicketReservationDTO
    {
        public long ScreeningId { get; set; }
        public string TicketTypeCode { get; set; }
        public string SeatLabel { get; set; }
    }
}