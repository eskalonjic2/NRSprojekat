using System;

namespace CinemaluxAPI.Services
{
    public class OrderItemResponseDTO
    {
        public long Id { get; set; }
        public string OrderTypeCode { get; set; }
        public string OrderItemName { get; set; }
        public double OrderItemPrice { get; set; }
        public byte Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}