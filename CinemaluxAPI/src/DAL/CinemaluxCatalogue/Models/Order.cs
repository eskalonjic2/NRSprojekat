using System;
using System.Collections.Generic;
using CinemaluxAPI.Common.Extensions;

#nullable disable

namespace CinemaluxAPI.DAL.CinemaluxCatalogue.Models
{
    public partial class Order : ICreatableEntity, IModifiableEntity, IArchivableEntity
    {
        public Order()
        {
            DiscountItems = new HashSet<DiscountItem>();
            OrderItems = new HashSet<OrderItem>();
            Tickets = new HashSet<Ticket>();
        }

        public long Id { get; set; }
        public int EmployeeId { get; set; }
        public int? ReservationId { get; set; }
        public string PaymentTypeCode { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual Reservation Reservation { get; set; }
        public virtual ICollection<DiscountItem> DiscountItems { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
