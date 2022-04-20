using System;
using System.Collections.Generic;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

#nullable disable

namespace CinemaluxAPI.DAL.CinemaluxCatalogue.Models
{
    public partial class Screening : ICreatableEntity, IModifiableEntity, IArchivableEntity
    {
        public Screening()
        {
            Tickets = new HashSet<Ticket>();
            Reservations = new HashSet<Reservation>();
        }

        public long Id { get; set; }
        public short MovieId { get; set; }
        public byte HallId { get; set; }
        public string DefaultTicketTypeCode { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan ScreeningTime { get; set; }
        public bool HasLocalSubtitles { get; set; }
        public bool Is3D { get; set; }
        public bool HasLocalAudio { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; }

        public virtual TicketType DefaultTicketType { get; set; }
        public virtual Hall Hall { get; set; }
        public virtual CinemaluxMovie CinemaluxMovie { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}