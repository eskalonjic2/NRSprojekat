using System;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;
using CinemaluxAPI.src.Services.Ticket.DTO;

namespace CinemaluxAPI.Services
{
    public class OrderResponseDTO
    {
        public OrderPreview order { get; set; }
        public TicketDTO[] tickets { get; set; }
        public OrderItemResponseDTO[] orderItems { get; set; }

        public partial class OrderPreview
        {
            public long Id { get; set; }
            public int? ReservationId { get; set; }  
            public string PaymentTypeCode { get; set; }  
            public double TotalPrice { get; set; }
            public DateTime CreatedAt { get; set; }
            public string CreatedBy { get; set; }
            public DateTime ModifiedAt { get; set; }
            public string ModifiedBy { get; set; }
        }
    }
}