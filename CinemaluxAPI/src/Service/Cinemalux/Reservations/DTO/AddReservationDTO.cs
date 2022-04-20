namespace CinemaluxAPI.Services.Reservations.DTO
{
    public class AddReservationDTO
    {
        public long ScreeningId { get; set; }
        public string ReservationTypeCode { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ContactPhone { get; set; }
        public bool IsPaid { get; set; }
        public TicketReservationDTO[] Tickets { get; set; }
    }
}