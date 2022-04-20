#nullable enable

namespace CinemaluxAPI.Services
{
    public class CreateOrderDTO
    {
        public int? ReservationId { get; set; }
        public string? PaymentTypeCode { get; set; }
    }
}