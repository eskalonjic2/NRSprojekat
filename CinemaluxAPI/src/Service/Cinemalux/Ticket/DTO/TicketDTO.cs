using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CinemaluxAPI.src.Services.Ticket.DTO
{
    public class TicketDTO
    {
        public long Id { get; set; }
        public long ScreeningId { get; set; }
        public string MovieTitle { get; set; }
        public long? OrderId { get; set; }
        public int? ReservationId { get; set; }
        public string TicketTypeCode { get; set; }
        public double TicketPrice { get; set; }
        public string SeatLabel { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsUsed { get; set; }

    }
}
