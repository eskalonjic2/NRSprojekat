using System;

namespace CinemaluxAPI.Services
{
    public class OrderGridDTO
    {
        public long Id{ get; set; }
        public string EmployeeName{ get; set; }
        public bool WasReserved{ get; set; }
        public string PaymentTypeCode{ get; set; }
        public int TotalTickets { get; set; }
        public double TotalPrice{ get; set; }
        public DateTime CreatedAt{ get; set; }
        public DateTime? FinalizedAt{ get; set; }
    }
}