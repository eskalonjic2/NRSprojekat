using System;
using System.Collections.Generic;
using CinemaluxAPI.Common.Extensions;

#nullable disable

namespace CinemaluxAPI.DAL.CinemaluxCatalogue.Models
{
    public partial class TicketType : ICreatableEntity,  IArchivableEntity
    {
        public TicketType()
        {
            Screenings = new HashSet<Screening>();
            Tickets = new HashSet<Ticket>();
        }

        public string Code { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; }

        public virtual ICollection<Screening> Screenings { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
