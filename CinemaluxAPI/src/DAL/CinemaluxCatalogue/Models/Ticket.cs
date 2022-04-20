using System;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

#nullable disable

namespace CinemaluxAPI.DAL.CinemaluxCatalogue.Models
{
    public partial class Ticket : ICreatableEntity, IModifiableEntity, IArchivableEntity
    {
        public long Id { get; set; }
        public long ScreeningId { get; set; }
        public long? OrderId { get; set; }
        public int? ReservationId { get; set; }
        public string TicketTypeCode { get; set; }
        public bool IsUsed { get; set; }
        public string SeatLabel { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; }

        public virtual Order Order { get; set; }
        public virtual Reservation Reservation { get; set; }
        public virtual Screening Screening { get; set; }
        public virtual TicketType TicketType { get; set; }
    }
}