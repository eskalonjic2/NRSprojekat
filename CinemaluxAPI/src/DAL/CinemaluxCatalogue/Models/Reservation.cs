using System;
using System.Collections.Generic;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

#nullable disable

namespace CinemaluxAPI.DAL.CinemaluxCatalogue.Models
{
    public partial class Reservation : ICreatableEntity, IModifiableEntity, IArchivableEntity
    {
        public Reservation()
        {
            Orders = new HashSet<Order>();
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string ReservationTypeCode { get; set; }
        public long ScreeningId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ContactPhone { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; }

        public virtual ReservationType ReservationType { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

        public virtual Screening Screening { get; set; }
    }
}