namespace CinemaluxAPI.src.Services.Ticket.DTO
{
    public class AddTicketsDTO
    {
        public long ScreeningId { get; set; }
        public long OrderId { get; set; }
        public int ReservationId { get; set; }
        public string TicketTypeCode { get; set; }
        public string[] SeatLabels { get; set; }
    }
}